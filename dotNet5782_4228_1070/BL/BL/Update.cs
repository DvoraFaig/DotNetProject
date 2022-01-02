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
        public void RemoveParcel(Parcel parcel)
        {
            if (parcel.Drone == null)
            {
                try
                {
                    dal.removeParcel(dal.getParcelWithSpecificCondition(p => p.Id == parcel.Id).First());
                }
                catch (ArgumentNullException) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id); }
                catch (InvalidOperationException) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id); }
            }
            else throw new Exceptions.ObjNotAvailableException("Can't remove parcel. Parcel asign to drone.");
        }

        /// <summary>
        /// Change name of drones' model.
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the new Model name</param>
        public void ChangeDroneModel(Drone droneWithUpdateInfo)
        {
            try
            {
                int index = droensList.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
                droensList[index] = droneWithUpdateInfo;
                //droensList.Remove(newDrone);
                //droensList.Add(newDrone);
                dal.changeDroneInfo(convertBLToDalDrone(droneWithUpdateInfo));
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Couldn't change Model of drone with id {droneWithUpdateInfo.Id} ");
            }
        }



        public void StationChangeDetails(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
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
            catch (Exception)
            {
                throw new ObjNotExistException(typeof(DO.Customer), id);
            }
        }

        public void SendDroneToCharge(int droneId)
        {
            try
            {
                Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                if (drone.Status == DroneStatus.Available)
                {
                    DO.Station availbleSforCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition);
                    if (availbleSforCharging.Id == 0)
                        throw new ObjNotExistException("Drone cann't charge");
                    DO.DroneCharge droneCharge = new DO.DroneCharge() { StationId = availbleSforCharging.Id, DroneId = droneId };
                    Position availbleStationforCharging = new Position() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude };
                    double dis = (distance(drone.DronePosition, availbleStationforCharging));
                    if (dis != 0) // if the drone is supposed to fly to tha station to charge
                        drone.Battery = (int)dis * (int)electricityUsageWhenDroneIsEmpty;
                    drone.Status = DroneStatus.Maintenance;
                    drone.DronePosition = new Position() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude };
                    availbleSforCharging.ChargeSlots--;
                    dal.AddDroneToCharge(droneCharge);
                    dal.changeStationInfo(availbleSforCharging);
                    dal.changeDroneInfo(convertBLToDalDrone(drone));
                }
                else
                {
                    throw new ObjNotAvailableException("The Drone can't charge now\nPlease try later.....");
                }
            }
            catch (ObjNotExistException)
            {
                throw new ObjNotExistException(typeof(Drone),droneId);
            }
            catch (Exception e)
            {
                throw new ObjNotAvailableException("The Drone can't charge now\nPlease try later.....");
            }
        }

        public void FreeDroneFromCharging(int droneId, double timeCharging)
        {
            try
            {
                Drone blDrone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId && d.Status == DroneStatus.Maintenance).First();
                blDrone.Status = DroneStatus.Available;
                blDrone.Battery += (double)timeCharging * chargingRateOfDrone;//requestElectricity(4);
                DO.DroneCharge droneChargeByStation = dal.getDroneChargeWithSpecificCondition(d => d.DroneId == blDrone.Id).First();
                DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == droneChargeByStation.StationId).First();
                s.ChargeSlots++;
                StationChangeDetails(s.Id, null, s.ChargeSlots);
            }
            catch (Exception)
            {
                //throw new ObjNotExistException(typeof(Drone), droneId);
                throw new Exception("Can't free Drone from charge.\nPlease try later...");
            }
        }

        public void PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            try
            {
                DO.Customer senderP;
                DO.Customer targetP;
                DO.Customer senderMaxParcel;
                double disMaxPToSender = Math.Pow(2, 53); // the biggest number
                Drone droneToParcel = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                IEnumerable<DO.Parcel> parcels = dal.GetParcels();
                DO.Parcel maxParcel = new DO.Parcel();// = new IDal.DO.Parcel() { Weight = 0 };//parcels.First(); //check if weight is good=====================
                foreach (DO.Parcel p in parcels)
                {
                    if (!p.Requeasted.Equals(default(DO.Parcel).Requeasted))
                    {
                        senderP = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First();
                        targetP = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First();
                        Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        Position targetPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                        double disSenderToTarget = distance(senderPosition, targetPosition);
                        DO.Station MinDisFromTargetTostation = findAvailbleAndClosestStationForDrone(targetPosition);
                        double disTargetToStation = distance(targetPosition, new Position() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude });
                        double droneElectricity = requestElectricity((int)p.Weight);
                        if (droneToParcel.Battery - (int)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity) > 0) //[4]
                        {
                            if (p.Weight <= droneToParcel.MaxWeight) //BLParcel bLParcel = convertDalToBLParcel(p);
                            {
                                if (maxParcel.Equals(default(DO.Parcel)))
                                {
                                    maxParcel = p;
                                    senderMaxParcel = senderP;
                                    disMaxPToSender = disDroneToSenderP;
                                }
                                else
                                {
                                    if (maxParcel.Priority < p.Priority)
                                    {
                                        maxParcel = p;
                                        senderMaxParcel = senderP;
                                        disMaxPToSender = disDroneToSenderP;
                                    }
                                    else if (maxParcel.Priority == p.Priority /*&& p.Weight <= droneToParcel.MaxWeight && maxParcel.Weight <= droneToParcel.MaxWeight*/)
                                    {
                                        if (maxParcel.Weight < p.Weight)
                                            maxParcel = p;
                                        else if (maxParcel.Weight == p.Weight)
                                        {

                                            if (disDroneToSenderP < disMaxPToSender)
                                            {
                                                maxParcel = p;
                                                senderMaxParcel = senderP;
                                                disMaxPToSender = disDroneToSenderP;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (maxParcel.Equals(null))
                {
                    throw new Exception("Drone with the parcels' conditions wasn't found.");
                }
                droneToParcel.Status = DroneStatus.Delivery;
                droneToParcel.ParcelInTransfer = returnAParcelInTransfer(
                    maxParcel, convertBLToDalCustomer(GetCustomerById(maxParcel.SenderId)),
                    convertBLToDalCustomer(GetCustomerById(maxParcel.TargetId)));
                updateBLDrone(droneToParcel);
                maxParcel.DroneId = droneToParcel.Id;
                maxParcel.Scheduled = DateTime.Now;
                dal.changeParcelInfo(maxParcel);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public void DronePicksUpParcel(int droneId)// ParcelStatuses.PickedUp          
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
                drone.Battery -= disDroneToSenderP * requestElectricity((int)parcel.Weight);
                drone.DronePosition = senderPosition;
                updateBLDrone(drone);
                parcel.PickUp = DateTime.Now;
                dal.changeParcelInfo(parcel);
            }

            catch (Exception)
            {
                throw new ObjNotExistException(typeof(Drone), droneId);
            }
        }

        public void DeliveryParcelByDrone(int droneId) //ParcelStatuses.Delivered.
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
                bLDroneToSuplly.Battery -= electricity * disSenderToTarget;
                bLDroneToSuplly.DronePosition = targetPosition;
                bLDroneToSuplly.Status = DroneStatus.Available;
                updateBLDrone(bLDroneToSuplly);
                parcelToDelivery.Delivered = DateTime.Now;
                dal.changeParcelInfo(parcelToDelivery);
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

        private void updateBLDrone(Drone d)
        {
            try
            {
                Drone findDrone = getDroneWithSpecificConditionFromDronesList(e => e.Id == d.Id).First();
                findDrone = d;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        //public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority)
        //{
        //    DO.Parcel p = new DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Requeasted = DateTime.Now, Weight = weight };
        //    dal.AddParcel(p);
        //}

        private DroneInParcel createBLDroneInParcel(DO.Parcel p, int droneId)
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