using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;


namespace BL
{
    sealed partial class BL
    {
        public Action<Drone> DroneChangeAction { get; set; }
        public Action<Drone, bool> DroneListChangeAction { get; set; }

        /// <summary>
        /// Check if drone with the same id exist.
        /// if exist throw an error.
        /// else initioliaze drones info, add it to the drones in namespace BL and send to add furnction in Dal 
        /// Didn't sent an object because most of the props are initialize in the function and not by the workers
        /// </summary>
        /// <param name="id">Drones 'id</param>
        /// <param name="model">Drones' model name</param>
        /// <param name="maxWeight">Drones' maxweight he could carry</param>
        /// <param name="stationId">In witch station to charge the drone in.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone droneToAdd, int stationId)
        {
            Station s = convertDalToBLStation(dal.getStationWithSpecificCondition(s => s.Id == stationId).First());
            if (s.DroneChargeAvailble == 0)
                throw new Exceptions.ObjNotAvailableException(typeof(Station), stationId, "doesn't have available charging slots.");

            dal.AddDroneToCharge(new DO.DroneCharge() { StationId = stationId, DroneId = droneToAdd.Id });
            droneToAdd = createMaintenaceDroneByInfo(droneToAdd, s.StationPosition);

            try
            {
                DO.Drone drone = convertBLToDalDrone(droneToAdd);
                drone.IsActive = true;
                dal.AddDrone(drone);
            }
            catch (DO.Exceptions.DataChanged)
            {
                changeDroneInfoInDroneList(droneToAdd);
                DroneChangeAction?.Invoke(dronesList[dronesList.Count]);
                DroneListChangeAction?.Invoke(dronesList[dronesList.Count], true);
                return;
            }
            catch (DO.Exceptions.ObjExistException)
            {
                throw new ObjExistException(typeof(Drone), droneToAdd.Id);
            }

            dronesList.Add(droneToAdd.Clone<Drone>()); //droneToAdd
            DroneChangeAction?.Invoke(dronesList[dronesList.Count]);
            DroneListChangeAction?.Invoke(dronesList[dronesList.Count], true);

        }


        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationId"></param>
        /// <summary>
        /// Auxiliary function for function AddDrone
        /// </summary>
        /// <param name="drone">Drone with information to copy</param>
        /// <param name="stationId">The station id for drone to charge</param>
        /// <param name="stationPos">The station position == DronePosition</param>
        /// <returns></returns>
        private Drone createMaintenaceDroneByInfo(Drone drone, Position stationPos)
        {
            drone.Battery = new Random().Next(20, 40);
            drone.Status = DroneStatus.Maintenance;
            drone.DronePosition = stationPos;
            return drone;
        }

        /// <summary>
        /// Return DroneList converted to DronesToList
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesToList()
        {
            lock (dronesList)  //not changing info???
            {
                return convertDronesToDronesToList(dronesList);
            }
        }

        /// <summary>
        /// Receive weight and status and returns List<BLDroneToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight">if 3>weight>-1 == values of DroneStatus. if weight==-1 weight is null</param>
        /// <param name="status">if 3>status>-1 == values of DroneStatus. if status==-1 weight is null</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesByConditions(int weight, int status)
        {
            IEnumerable<Drone> IList = new List<Drone>();

            if (weight == -1 && status == -1)
                return GetDronesToList();
            if (weight >= 0 && status == -1)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (WeightCategories)weight && d.Status == (DroneStatus)status);

            return convertDronesToDronesToList(IList);
        }

        /// <summary>
        /// Get a drone by id from getBLDroneWithSpecificCondition = dronesList.
        /// </summary>
        /// <param name="droneRequestedId">The id of the drone that's requested<</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDroneById(int droneRequestedId)
        {
            try
            {
                Drone d = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneRequestedId).First();
                return d.Clone<Drone>();
            }
            #region Exceptions
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(Drone), droneRequestedId, e);
            }
            #endregion
        }


        /// <summary>
        /// Change name of drones' model.
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the new Model name</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChangeDroneModel(int droneId, string newModel)
        {
            try
            {
                lock (dronesList) 
                {
                    int index = dronesList.FindIndex(d => d.Id == droneId);
                    dronesList[index].Model = newModel;
                    dal.changeDroneInfo(convertBLToDalDrone(dronesList[index]));
                    DroneChangeAction?.Invoke(dronesList[index]);
                    DroneListChangeAction?.Invoke(dronesList[index], false);
                }
            }
            #region Exceptions
            //catch(DO.Exceptions.ObjNotExistException e1)
            //{
            //    throw new Exceptions.ObjNotExistException(e1.Message);
            //}
            catch (Exception e)
            {
                throw new InvalidOperationException($"Couldn't change Model of drone with id {droneId} ", e);
            }
            #endregion
        }

        /// <summary>
        /// Send drone to charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone SendDroneToCharge(int droneId)
        {
            lock (dal)
            {
                lock (dronesList)
                {
                    try
                    {
                        //EventsChanging.DroneChanged +=
                        //DroneChanged += PO.Drone.Change;
                        Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                        if (drone.Status != DroneStatus.Available) ///?????????????need??
                            throw new Exceptions.NoDataMatchingBetweenDalandBL("Drone cann't charge.\nNot in Available status");

                        DO.Station availbleStationForCharging;
                        try
                        {
                            availbleStationForCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition, drone.Battery);
                        }
                        catch (Exceptions.ObjNotAvailableException) { throw new ObjNotAvailableException("Drone cann't charge.\nNo Available charging slots\nPlease try later"); }

                        DO.DroneCharge droneCharge = new DO.DroneCharge() { StationId = availbleStationForCharging.Id, DroneId = droneId };
                        Position availbleStationforCharging = new Position() { Latitude = availbleStationForCharging.Latitude, Longitude = availbleStationForCharging.Longitude };
                        double dis = (distance(drone.DronePosition, availbleStationforCharging));

                        if (dis != 0)
                        {
                            double batteryForDis = Math.Round((double)dis * (double)electricityUsageWhenDroneIsEmpty, 1); //to erase
                            if (drone.Battery - batteryForDis < 0) //to erase
                                throw new Exceptions.ObjNotAvailableException("Not enough battery for drone to be send to a close station to charge.");
                            drone.Battery = batteryForDis;
                        }

                        drone.Status = DroneStatus.Maintenance;
                        drone.DronePosition = availbleStationforCharging;
                        dal.AddDroneToCharge(droneCharge);
                        dal.changeStationInfo(availbleStationForCharging);
                        //dal.changeDroneInfo(convertBLToDalDrone(drone)); //???
                        drone.SartToCharge = DateTime.Now;
                        changeDroneInfoInDroneList(drone);
                        DroneChangeAction?.Invoke(drone);
                        DroneListChangeAction?.Invoke(drone, false);
                        return drone;
                    }
                    catch (Exceptions.ObjNotExistException)
                    {
                        throw new Exceptions.ObjNotExistException(typeof(Drone), droneId);
                    }
                }
            }
        }

        /// <summary>
        /// Free drone from chargeing.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeCharging"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone FreeDroneFromCharging(int droneId/*, double timeCharging*/)
        {
            try
            {
                lock (dronesList)
                {
                    lock (dal)
                    {
                        //DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == droneChargeByStation.StationId).First();
                        //changeInfoOfStation(s.Id, null, s.ChargeSlots);
                        //DO.DroneCharge droneChargeByStation = dal.getDroneChargeWithSpecificCondition(d => d.DroneId == drone.Id).First();/////////////////

                        Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId /*&& d.Status == DroneStatus.Maintenance*/).First();
                        dal.removeDroneChargeByDroneId(drone.Id);

                        drone.Status = DroneStatus.Available;
                        TimeSpan second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 100;
                        double baterryToAdd = second.TotalMinutes * chargingRateOfDrone;
                        drone.Battery += Math.Round(baterryToAdd);
                        drone.Battery = Math.Min(drone.Battery, 100);
                        changeDroneInfoInDroneList(drone);
                        DroneChangeAction?.Invoke(drone);
                        DroneListChangeAction?.Invoke(drone, false);
                        return drone;
                    }
                }
            }
            #region Exceptions
            catch (Exception e)
            {
                throw new Exceptions.ObjNotAvailableException("Can't free Drone from charge.\nPlease try later...", e);
            }
            #endregion
        }

        /// <summary>
        /// Send to remove a drone charge by Drone Id
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void removeDroneChargeByDroneId(int droneId)
        {
            dal.removeDroneChargeByDroneId(droneId);
        }

        /// <summary>
        /// Remove specific drone
        /// </summary>
        /// <param name="droneId">remove current drone with droneId</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(Drone drone)
        {
            try
            {
                lock (dal)
                {
                    dal.removeDrone(convertBLToDalDrone(drone));
                }

                int index = dronesList.FindIndex(d => d.Id == drone.Id);
                dronesList.RemoveAt(index);
            }
            catch (DO.Exceptions.ObjNotExistException e1)
            {
                throw new Exceptions.ObjNotExistException(e1.Message);
            }
        }

        /// <summary>
        /// Get drone status
        /// </summary>
        /// <param name="droneId">Drones' id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetDroneStatusInDelivery(Drone droneInt)
        {
            return (int)returnDroneStatusInDelivery(droneInt);
        }

        /// <summary>
        /// Drone Dtatus
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        public DeliveryStatusAction returnDroneStatusInDelivery(Drone drone)
        {
            if (drone.Status == DroneStatus.Available)
                return DeliveryStatusAction.Available;

            else
            {
                if (drone.ParcelInTransfer != null)
                {
                    if (drone.DronePosition.Latitude == drone.ParcelInTransfer.SenderPosition.Latitude &&
                        drone.DronePosition.Longitude == drone.ParcelInTransfer.SenderPosition.Longitude)
                        return DeliveryStatusAction.PickedParcel;
                    

                    if (drone.DronePosition.Latitude == drone.ParcelInTransfer.SenderPosition.Latitude
                                && drone.DronePosition.Longitude == drone.ParcelInTransfer.SenderPosition.Longitude)
                        return DeliveryStatusAction.DeliveredParcel;

                    return DeliveryStatusAction.AsignedParcel;
                }
                else
                    return DeliveryStatusAction.DeliveredParcel;
                
            }
        }

        /// <summary>
        /// Change drone info by drone list in BL
        /// </summary>
        /// <param name="droneWithUpdateInfo"></param>
        public void changeDroneInfoInDroneList(Drone droneWithUpdateInfo)
        {
            int index = dronesList.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
            dronesList[index] = droneWithUpdateInfo;

            //Drone droneToChange = dronesList.Find(d => d.Id == droneWithUpdateInfo.Id);
            //droneToChange = droneWithUpdateInfo;
            //DroneChangeAction?.Invoke(droneToChange);



            ////////DroneChangeAction?.Invoke(dronesList[index]);
            ////////DroneListChangeAction?.Invoke(dronesList[index], false);
        }
    }
}

