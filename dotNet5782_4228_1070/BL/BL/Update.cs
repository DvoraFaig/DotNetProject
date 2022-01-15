using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Checks if the parcel to remove exist.
        /// If exist send to remove
        /// else throw an error
        /// </summary>
        /// <param name="parcel">The parcel </param>
        public void RemoveParcel(int parcelId)
        {
            Parcel parcel = GetParcelById(parcelId);
            if (parcel.Drone == null)
            {
                try
                {
                    dal.removeParcel(dal.getParcelWithSpecificCondition(p => p.Id == parcel.Id).First());
                }
                catch (ArgumentNullException e) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id ,e); }
                catch (InvalidOperationException e1) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id , e1); }
            }
            else throw new Exceptions.ObjNotAvailableException("Can't remove parcel. Parcel asign to drone.");
        }

        /// <summary>
        /// Change name of drones' model.
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the new Model name</param>
        public void ChangeDroneModel(int droneId, string newModel)
        {
            try
            {
                int index = dronesList.FindIndex(d => d.Id == droneId);
                dronesList[index].Id = droneId;
                dronesList[index].Model = newModel;
                dal.changeDroneInfo(convertBLToDalDrone(dronesList[index]));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Couldn't change Model of drone with id {droneId} " , e);
            }
        }

        /// <summary>
        /// Change i=info of station
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="ChargeSlots"></param>
        public void changeInfoOfStation(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
        {
            DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == id).First();
            if (name != null)
                s.Name = name;
            if (s.ChargeSlots <= ChargeSlots)
                s.ChargeSlots = ChargeSlots;
            if (s.ChargeSlots > ChargeSlots)
            {
                int amountDroneChargesFull = dal.getDroneChargeWithSpecificCondition(station => station.StationId == s.Id).Count();
                if (amountDroneChargesFull < ChargeSlots)
                    s.ChargeSlots = ChargeSlots;
                else
                    throw new Exception($"The amount Charging slots you want to change is smaller than the amount of drones that are charging now in station number {id}");
            }
            dal.changeStationInfo(s);
        }

        public void UpdateCustomerDetails(int id, string name = null, string phone = null)
        {
            DO.Customer c;
            try
            {
                c = dal.getCustomerWithSpecificCondition(c => c.Id == id).First();
                if (name != null)
                    c.Name = name;
                if (phone != null && phone.Length >= 9 && phone.Length <= 10)
                    c.Phone = phone;
                dal.changeCustomerInfo(c);
            }
            catch (Exception e)
            {
                throw new ObjNotExistException(typeof(DO.Customer), id , e);
            }
        }

        public Drone SendDroneToCharge(int droneId)
        {
            try
            {
                Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First(); //I have the drone already??????
                if (drone.Status == DroneStatus.Available)
                {
                    DO.Station availbleStationForCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition , drone.Battery);
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
                        batteryForDis = Math.Round(batteryForDis, 2);
                        if (drone.Battery - batteryForDis < 0) //to erase
                            throw new Exceptions.ObjNotAvailableException("Not enough battery for drone to be send to a close station to charge.");
                        drone.Battery =batteryForDis;
                        //drone.Battery = Convert.ToDouble(String.Format("{0:0.00}", batteryForDis));//Math.FormatNumber(batteryForDis,2);
                    }
                    drone.Status = DroneStatus.Maintenance;
                    drone.DronePosition = availbleStationforCharging;//new Position() { Latitude = availbleStationForCharging.Latitude, Longitude = availbleStationForCharging.Longitude };
                    //availbleStationForCharging.ChargeSlots--;
                    dal.AddDroneToCharge(droneCharge);
                    dal.changeStationInfo(availbleStationForCharging);
                    dal.changeDroneInfo(convertBLToDalDrone(drone));
                    drone.SartToCharge = DateTime.Now;
                    return drone;
                }
                #region exceptions
                else
                {
                    throw new ObjNotAvailableException("The Drone is not avalable for charging\nPlease try later.....");
                }
            }
            catch (ObjNotExistException e)
            {
                throw new ObjNotExistException(e.Message);
            }
            catch (Exception e1)
            {
                throw new ObjNotAvailableException("The Drone can't charge now\nPlease try later....." , e1);
            }
            #endregion
        }

        /// <summary>
        /// Free drone from chargeing.
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeCharging"></param>
        /// <returns></returns>
        public Drone FreeDroneFromCharging(int droneId, double timeCharging)
        {
            try
            {
                Drone blDrone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId && d.Status == DroneStatus.Maintenance).First();
                DO.DroneCharge droneChargeByStation = dal.getDroneChargeWithSpecificCondition(d => d.DroneId == blDrone.Id).First();
                DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == droneChargeByStation.StationId).First();
                //s.ChargeSlots++;
                changeInfoOfStation(s.Id, null, s.ChargeSlots);
                blDrone.Status = DroneStatus.Available;
                TimeSpan second = (TimeSpan)(DateTime.Now - blDrone.SartToCharge)*100;///
                double baterryToAdd = second.TotalMinutes * chargingRateOfDrone;
                baterryToAdd = Math.Round(baterryToAdd, 1);
                blDrone.Battery += baterryToAdd;
                blDrone.Battery = Math.Min(100,blDrone.Battery) ;
                return blDrone;
            }
            catch (Exception e)
            {
                throw new Exception("Can't free Drone from charge.\nPlease try later...",e);
            }
        }

        public Drone PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            try
            {
                DO.Customer senderParcel;
                DO.Customer targetParcel;
                DO.Customer senderMaxParcel;
                double disMaxPToSender = -1; // the biggest distance.....
                Drone droneToParcel = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                IEnumerable<DO.Parcel> parcels = dal.GetParcels();
                DO.Parcel maxParcel= new DO.Parcel();// = new IDal.DO.Parcel() { Weight = 0 };//parcels.First(); //check if weight is good=====================
                foreach (DO.Parcel p in parcels)
                {
                    if (p.Scheduled == null)//&& requested!=null .Equals(default(DO.Parcel).Scheduled)
                    {
                        senderParcel = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First();
                        targetParcel = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First();
                        Position senderPosition = new Position() { Longitude = senderParcel.Longitude, Latitude = senderParcel.Latitude };
                        Position targetPosition = new Position() { Longitude = targetParcel.Longitude, Latitude = targetParcel.Latitude };
                        double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                        double disSenderToTarget = distance(senderPosition, targetPosition);
                        double batteryAfterDeliveringByTarget = Math.Round(disDroneToSenderP * electricityUsageWhenDroneIsEmpty + disSenderToTarget * requestElectricity((int)p.Weight),1);
                        DO.Station stationWithMinDisFromTarget = findAvailbleAndClosestStationForDrone(targetPosition , batteryAfterDeliveringByTarget);
                        double disTargetToStation = distance(targetPosition, new Position() { Longitude = stationWithMinDisFromTarget.Longitude, Latitude = stationWithMinDisFromTarget.Latitude });
                        double droneElectricity = requestElectricity((int)p.Weight);
                        double totalBatteryForDeliveryUsage = (double)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity);
                        totalBatteryForDeliveryUsage = Math.Round(totalBatteryForDeliveryUsage, 2);
                        if (droneToParcel.Battery - totalBatteryForDeliveryUsage > 0) //[4]
                        {
                            if (p.Weight <= droneToParcel.MaxWeight) //BLParcel bLParcel = convertDalToBLParcel(p);
                            {
                                if (maxParcel.Equals(default(DO.Parcel)))
                                {
                                    maxParcel = p;
                                    senderMaxParcel = senderParcel;
                                    disMaxPToSender = disDroneToSenderP;
                                }
                                else
                                {
                                    if (maxParcel.Priority < p.Priority)
                                    {
                                        maxParcel = p;
                                        senderMaxParcel = senderParcel;
                                        disMaxPToSender = disDroneToSenderP;
                                    }
                                    else if (maxParcel.Priority == p.Priority /*&& p.Weight <= droneToParcel.MaxWeight && maxParcel.Weight <= droneToParcel.MaxWeight*/)
                                    {
                                        if (maxParcel.Weight < p.Weight)
                                            maxParcel = p;
                                        else if (maxParcel.Weight == p.Weight)
                                        {
                                            if (disDroneToSenderP < disMaxPToSender || disMaxPToSender == -1)
                                            {
                                                maxParcel = p;
                                                senderMaxParcel = senderParcel;
                                                disMaxPToSender = disDroneToSenderP;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (maxParcel.Equals(default(DO.Parcel)))
                {
                    throw new Exceptions.ObjNotAvailableException("No Parcel matching drones' conditions");
                }
                droneToParcel.Status = DroneStatus.Delivery;
                droneToParcel.ParcelInTransfer = returnAParcelInTransfer(
                    maxParcel, convertBLToDalCustomer(GetCustomerById(maxParcel.SenderId)),
                    convertBLToDalCustomer(GetCustomerById(maxParcel.TargetId)));
                updateBLDrone(droneToParcel);
                maxParcel.DroneId = droneToParcel.Id;
                maxParcel.Scheduled = DateTime.Now;
                dal.changeParcelInfo(maxParcel);
                return droneToParcel;
            }
            //////
            catch (Exception e)
            {
                throw new ObjNotAvailableException(e.Message);
            }
            ///////
        }

        public Drone DronePicksUpParcel(int droneId)// ParcelStatuses.PickedUp          
        {
            try
            {
                Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId && d.Status == DroneStatus.Delivery).First();
                DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (!parcel.PickUp.Equals(default(DO.Parcel).PickUp))
                {
                    throw new Exception("The parcel is collected already");
                }
                if (parcel.Scheduled.Equals(default(DO.Parcel).Scheduled))
                {
                    throw new Exception("The parcel is not schedueld.");
                }
                DO.Customer senderP;
                try
                {
                    senderP = dal.getCustomerWithSpecificCondition(customer => customer.Id == parcel.SenderId).First();
                }
                catch (ObjNotExistException)
                {
                    throw new Exception("Drone wasn't abale to pick up the parcel");
                }
                Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                double disDroneToSenderP = distance(drone.DronePosition, senderPosition);
                if (disDroneToSenderP > drone.Battery)///not sure if we need it here. it was supposed to be checked in the pair a parcel with drone.
                    throw new ObjNotAvailableException("The battery usage will be bigger than the drones' battery ");
                drone.Battery -= Math.Round( disDroneToSenderP * requestElectricity((int)parcel.Weight) ,1);
                drone.DronePosition = senderPosition;

                updateBLDrone(drone);
                parcel.PickUp = DateTime.Now;
                dal.changeParcelInfo(parcel);
                return drone;
            }

            catch (Exception e)
            {
                throw new ObjNotExistException(e.Message);
            }
        }

        public Drone DeliveryParcelByDrone(int droneId) //ParcelStatuses.Delivered.
        {
            try
            {
                Drone bLDroneToSuplly = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                DO.Parcel parcelToDelivery = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (parcelToDelivery.PickUp.Equals(default(DO.Parcel).PickUp) && !parcelToDelivery.Delivered.Equals(default(DO.Parcel).Delivered))
                {
                    throw new Exception("Drone cann't deliver this parcel.");
                }
                DO.Customer senderP;
                DO.Customer targetP;
                senderP = dal.getCustomerWithSpecificCondition(c => c.Id == parcelToDelivery.SenderId).First();
                targetP = dal.getCustomerWithSpecificCondition(c => c.Id == parcelToDelivery.TargetId).First();
                Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                Position targetPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                double disSenderToTarget = distance(senderPosition, targetPosition);
                double electricity = requestElectricity((int)parcelToDelivery.Weight);
                bLDroneToSuplly.Battery -= Math.Round(electricity * disSenderToTarget, 1);
                bLDroneToSuplly.DronePosition = targetPosition;
                bLDroneToSuplly.Status = DroneStatus.Available;
                updateBLDrone(bLDroneToSuplly);
                parcelToDelivery.Delivered = DateTime.Now;
                dal.changeParcelInfo(parcelToDelivery);
                return bLDroneToSuplly;
            }
            catch (ObjNotExistException)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
            catch (Exception)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
        }

        private static ParcelStatuses findParcelStatus(DO.Parcel p)
        {
            if(p.Delivered != null)
                return ParcelStatuses.Delivered;
            else if (p.PickUp != null)
                return ParcelStatuses.PickedUp;
            
            else if(p.Scheduled != null)
                return ParcelStatuses.Scheduled;
            else //if (p.Requeasted != null)
                return ParcelStatuses.Requeasted;
        }
        private static ParcelStatuses findParcelStatus(Parcel p)
        {
            if (p.Delivered != null)
                return ParcelStatuses.Delivered;
            else if (p.PickUp != null)
                return ParcelStatuses.PickedUp;

            else if (p.Scheduled != null)
                return ParcelStatuses.Scheduled;
            else //if (p.Requeasted != null)
                return ParcelStatuses.Requeasted;
        }

        /// <summary>
        /// Update drones detailes.
        /// </summary>
        /// <param name="droneWithUpdateInfo"></param>
        private void updateBLDrone(Drone droneWithUpdateInfo)
        {
            try
            {
                int index  = dronesList.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
                dronesList[index] = droneWithUpdateInfo;
                //Drone findDrone = getDroneWithSpecificConditionFromDronesList(e => e.Id == droneWithUpdateInfo.Id).First();
                //findDrone = droneWithUpdateInfo;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

       /// <summary>
       /// return a DroneInParcel occurding to a parcel and drone id.
       /// </summary>
       /// <param name="p">a parcel in drone</param>
       /// <param name="droneId">drone that has a parcel</param>
       /// <returns></returns>
        private DroneInParcel createDroneInParcel(DO.Parcel p, int droneId)
        {
            Drone d = getDroneByIdFromDronesList(droneId);
            return new DroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithParcel = d.DronePosition };
        }
    }
}

//public void DroneChangeModel(DroneToList newDrone)
//{
//    try
//    {
//        Drone d = getDroneWithSpecificConditionFromDronesList(drone => drone.Id == newDrone.Id).First();
//        droensList.Remove(d);
//        droensList.Add(d);
//        dal.changeDroneInfo(convertBLToDalDrone(d));
//        //dal.changeDroneInfo(d.Id, d.Model);
//    }
//    catch (Exception)
//    {
//        throw new InvalidStringException("Drones' Model");
//    }
//}




//////public static double RoundUp(double input, int places)
//////{
//////    double multiplier = Math.Pow(10, Convert.ToDouble(places));
//////    return Math.Ceiling(input * multiplier) / multiplier;
//////}
///
//public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority)
//{
//    DO.Parcel p = new DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Requeasted = DateTime.Now, Weight = weight };
//    dal.AddParcel(p);
//}
