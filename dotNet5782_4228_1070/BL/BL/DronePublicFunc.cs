﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;


namespace BL
{
    public sealed partial class BL : BlApi.Ibl, BlApi.ISimulation
    {
        public Action<Drone> DroneChange { get; set; }

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
            if (dal.IsDroneById(droneToAdd.Id))
            {
                #region Exceptions
                throw new ObjExistException(typeof(BO.Drone), droneToAdd.Id);
                #endregion
            }
            else
            {
                lock (dal)
                {
                    if ((int)droneToAdd.MaxWeight > 3 || (int)droneToAdd.MaxWeight < 1)
                        throw new Exception("Weight of drone is out of range");
                    DO.WeightCategories maxWeightconvertToEnum = droneToAdd.MaxWeight;
                    int battery = r.Next(20, 40);
                    DO.Station s;
                    try
                    {
                        s = dal.getStationWithSpecificCondition(s => s.Id == stationId).First();
                    }
                    #region Exceptions
                    catch (InvalidOperationException)
                    {
                        throw new ObjNotExistException(typeof(DO.Station), stationId);
                    }
                    #endregion
                    Station sBL = convertDalToBLStation(s);
                    if (s.ChargeSlots - sBL.DronesCharging.Count > 0)
                    {
                        Position p = new Position() { Longitude = s.Longitude, Latitude = s.Latitude };
                        Drone dr = new Drone() { Id = droneToAdd.Id, Model = droneToAdd.Model, MaxWeight = maxWeightconvertToEnum, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
                        dronesList.Add(dr);
                        dal.AddDroneToCharge(new DO.DroneCharge() { StationId = s.Id, DroneId = droneToAdd.Id });
                        dal.AddDrone(convertBLToDalDrone(dr));
                        DroneChange?.Invoke(dr);
                    }
                    else
                    {
                        #region Exceptions
                        throw new Exception($"The charging slots of station: {stationId} is full.\nPlease enter a differant station.");
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// Return DronesList
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> getDrones()
        {
            lock (dronesList)
            {
                return dronesList;
            }
        }

        /// <summary>
        /// Return DroneList converted to DronesToList
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> returnDronesToList()
        {
            lock (dronesList)
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
        public IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status)
        {
            IEnumerable<Drone> IList = getDrones(); //if Both null took it out of th eif becuase Ienumerable needed a statment...
            if (weight >= 0 && status == -1)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (DO.WeightCategories)weight && d.Status == (DroneStatus)status);
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
            return getDroneWithSpecificConditionFromDronesList(d => d.Id == droneRequestedId).First();
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
                    dronesList[index].Id = droneId;
                    dronesList[index].Model = newModel;
                    dal.changeDroneInfo(convertBLToDalDrone(dronesList[index]));
                    DroneChange?.Invoke(dronesList[index]);
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
            try
            {
                lock (dal)
                {
                    lock (dronesList)
                    {
                        //EventsChanging.DroneChanged +=
                        //DroneChanged += PO.Drone.Change;
                        Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First(); //I have the drone already??????
                        if (drone.Status == DroneStatus.Available)
                        {
                            DO.Station availbleStationForCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition, drone.Battery);
                            if (availbleStationForCharging.Id == 0)
                                throw new ObjNotExistException("Drone cann't charge.\nNo Available charging slots\nPlease try later");
                            DO.DroneCharge droneCharge = new DO.DroneCharge() { StationId = availbleStationForCharging.Id, DroneId = droneId };
                            Position availbleStationforCharging = new Position() { Latitude = availbleStationForCharging.Latitude, Longitude = availbleStationForCharging.Longitude };
                            double dis = (distance(drone.DronePosition, availbleStationforCharging));
                            ////////if (dis != 0) // if the drone is supposed to fly to tha station to charge
                            ////////    drone.Battery = (int)dis * (int)electricityUsageWhenDroneIsEmpty;
                            if (dis != 0)
                            {// if the drone is supposed to fly to tha station to charge
                                double batteryForDis = (double)dis * (double)electricityUsageWhenDroneIsEmpty; //to erase
                                batteryForDis = Math.Round(batteryForDis, 1);
                                if (drone.Battery - batteryForDis < 0) //to erase
                                    throw new Exceptions.ObjNotAvailableException("Not enough battery for drone to be send to a close station to charge.");
                                drone.Battery = batteryForDis;
                            }
                            drone.Status = DroneStatus.Maintenance;
                            drone.DronePosition = availbleStationforCharging;
                            dal.AddDroneToCharge(droneCharge);
                            dal.changeStationInfo(availbleStationForCharging);
                            dal.changeDroneInfo(convertBLToDalDrone(drone));
                            drone.SartToCharge = DateTime.Now;
                            DroneChange?.Invoke(drone);
                            return drone;
                        }
                        #region exceptions
                        else
                        {
                            throw new ObjNotAvailableException("The Drone is not avalable for charging\nPlease try later.....");
                        }
                        #endregion
                    }
                }
            }
            #region exceptions
            catch (ObjNotExistException e)
            {
                throw new ObjNotExistException(e.Message);
            }
            catch (Exception e1)
            {
                throw new ObjNotAvailableException("The Drone can't charge now\nPlease try later.....", e1);
            }
            #endregion
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
                        Drone blDrone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId /*&& d.Status == DroneStatus.Maintenance*/).First();
                        DO.DroneCharge droneChargeByStation = dal.getDroneChargeWithSpecificCondition(d => d.DroneId == blDrone.Id).First();/////////////////
                        DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == droneChargeByStation.StationId).First();
                        //changeInfoOfStation(s.Id, null, s.ChargeSlots);
                        blDrone.Status = DroneStatus.Available;
                        dal.removeDroneChargeByDroneId(droneId);
                        TimeSpan second = (TimeSpan)(DateTime.Now - blDrone.SartToCharge) * 100;
                        double baterryToAdd = second.TotalMinutes * chargingRateOfDrone;
                        baterryToAdd = Math.Round(baterryToAdd, 1);
                        blDrone.Battery += baterryToAdd;
                        blDrone.Battery = Math.Round(blDrone.Battery, 1);
                        blDrone.Battery = Math.Min(blDrone.Battery, 100);
                        DroneChange?.Invoke(blDrone);
                        // Do later: remove drone charge;
                        dal.removeDroneChargeByDroneId(blDrone.Id);
                        return blDrone;
                    }
                }
            }
            #region Exceptions
            catch (Exception e)
            {
                throw new Exception("Can't free Drone from charge.\nPlease try later...", e);
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
                if (dal.IsDroneById(drone.Id))
                    lock (dal)
                    {
                        dal.removeDrone(convertBLToDalDrone(drone));
                    }
                #region Exceptions
                else
                    throw new Exceptions.ObjExistException(typeof(Drone), drone.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
            #endregion
        }

        /// <summary>
        /// Get drone status
        /// </summary>
        /// <param name="droneId">Drones' id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetDroneStatusInDelivery(Drone droneInt)
        {
            return (int)GetfromEnumDroneStatusInDelivery(droneInt);
        }

        public DeliveryStatusAction GetfromEnumDroneStatusInDelivery(Drone droneId)
        {
            // return DeliveryStatusAction(GetDroneStatusInDelivery(droneId));

            Drone drone = droneId;
            if (drone.Status == DroneStatus.Available)
            {
                return DeliveryStatusAction.Available;
            }
            else if (drone.Status == DroneStatus.Delivery)
            {
                if (drone.ParcelInTransfer != null)
                {
                    if (drone.DronePosition.Latitude == drone.ParcelInTransfer.SenderPosition.Latitude &&
                        drone.DronePosition.Longitude == drone.ParcelInTransfer.SenderPosition.Longitude) // i erased else if
                    {
                        return DeliveryStatusAction.PickedParcel;
                    }

                    if (drone.DronePosition.Latitude == drone.ParcelInTransfer.SenderPosition.Latitude
                                && drone.DronePosition.Longitude == drone.ParcelInTransfer.SenderPosition.Longitude)
                    {
                        return DeliveryStatusAction.DeliveredParcel;
                    }
                    return DeliveryStatusAction.AsignedParcel;
                }
                else
                    return DeliveryStatusAction.DeliveredParcel;

            }

            throw new Exception("No macthing status");
        }

        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            int index = dronesList.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
            dronesList[index] = droneWithUpdateInfo;
            //DroneChange?.Invoke(dronesList[index]);
        }
    }
}
