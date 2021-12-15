using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        public void DroneChangeModel(Drone newDrone)
        {
            try
            {
                dronesInBL.Remove(newDrone);
                dronesInBL.Add(newDrone);
                dal.changeDroneInfo(newDrone.Id, newDrone.Model);
            }
            catch (Exception)
            {
                //throw new InvalidOperationException("Couldn't change Model");
                throw new Exception("Couldn't change Model");
            }
        }
        public void DroneChangeModel(DroneToList newDrone)
        {
            try
            {
                Drone d = getBLDroneWithSpecificCondition(drone => drone.Id == newDrone.Id).First();
                dronesInBL.Remove(d);
                dronesInBL.Add(d);
                dal.changeDroneInfo(d.Id, d.Model);
            }
            catch (Exception)
            {
                throw new InvalidStringException("Drones' Model");
            }
        }

        public void StationChangeDetails(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
        {
            IDal.DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == id).First();
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
            IDal.DO.Customer c;
            try
            {
                c = dal.getCustomerWithSpecificCondition(c => c.ID == id).First();
                if (name != null)
                    c.Name = name;
                if (phone != null && phone.Length >= 9 && phone.Length <= 10)
                    c.Phone = phone;
                dal.changeCustomerInfo(c);
            }
            catch (Exception)
            {
                throw new ObjNotExistException(typeof(IDal.DO.Customer), id);
            }
        }

        public void SendDroneToCharge(int droneId)
        {
            try
            {
                Drone drone = getBLDroneWithSpecificCondition(d => d.Id == droneId).First();
                if (drone.Status == DroneStatus.Available)
                {
                    IDal.DO.Station availbleSforCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition);
                    IDal.DO.DroneCharge droneCharge = new IDal.DO.DroneCharge() { StationId = availbleSforCharging.Id, DroneId = droneId };
                    drone.Battery = (distance(drone.DronePosition, new Position() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude })) * (int)Electricity.Empty;
                    drone.Status = DroneStatus.Maintenance;
                    drone.DronePosition = new Position() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude };
                    availbleSforCharging.ChargeSlots--;
                    dal.AddDroneCharge(droneCharge);
                    dal.changeStationInfo(availbleSforCharging);
                    dal.changeDroneInfo(convertBLToDalDrone(drone));
                }
                else
                {
                    throw new ObjNotAvailableException("The Drone can't charge now\nPlease try later.....");
                }
            }
            catch (Exception)
            {
                throw new ObjNotExistException(typeof(Drone), droneId);
            }
        }

        public void FreeDroneFromCharging(int droneId, double timeCharging)
        {
            try
            {
                Drone blDrone = getBLDroneWithSpecificCondition(d => d.Id == droneId && d.Status == DroneStatus.Maintenance).First();
                blDrone.Status = DroneStatus.Available;
                blDrone.Battery += (double)timeCharging * requestElectricity(4);
                IDal.DO.DroneCharge droneChargeByStation = dal.getDroneChargeWithSpecificCondition(d => d.DroneId == blDrone.Id).First();
                IDal.DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == droneChargeByStation.StationId).First();
                s.ChargeSlots++;
                StationChangeDetails(s.Id, null, s.ChargeSlots);
            }
            catch (Exception)
            {
                throw new ObjNotExistException(typeof(Drone), droneId);
                //throw new InvalidOperationException();
            }
        }

        public void PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            try
            {
                IDal.DO.Customer senderP;
                IDal.DO.Customer targetP;
                IDal.DO.Customer senderMaxParcel;
                double disMaxPToSender = Math.Pow(2, 53); // the biggest number
                Drone droneToParcel = getBLDroneWithSpecificCondition(d => d.Id == droneId).First();
                IEnumerable<IDal.DO.Parcel> parcels = dal.displayParcels();
                IDal.DO.Parcel maxParcel = new IDal.DO.Parcel();// = new IDal.DO.Parcel() { Weight = 0 };//parcels.First(); //check if weight is good=====================
                foreach (IDal.DO.Parcel p in parcels)
                {
                    if (!p.Requeasted.Equals(default(IDal.DO.Parcel).Requeasted))
                    {
                        senderP = dal.getCustomerWithSpecificCondition(c => c.ID == p.SenderId).First();
                        targetP = dal.getCustomerWithSpecificCondition(c => c.ID == p.TargetId).First();
                        Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        Position targetPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                        double disSenderToTarget = distance(senderPosition, targetPosition);
                        IDal.DO.Station MinDisFromTargetTostation = findAvailbleAndClosestStationForDrone(targetPosition);
                        double disTargetToStation = distance(targetPosition, new Position() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude });
                        double droneElectricity = requestElectricity((int)p.Weight);
                        if (droneToParcel.Battery - (int)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity) > 0) //[4]
                        {
                            if (p.Weight <= droneToParcel.MaxWeight) //BLParcel bLParcel = convertDalToBLParcel(p);
                            {
                                if (maxParcel.Equals(default(IDal.DO.Parcel)))
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
                Drone drone = getBLDroneWithSpecificCondition(d => d.Id == droneId && d.Status == DroneStatus.Delivery).First();
                IDal.DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (!parcel.PickUp.Equals(default(IDal.DO.Parcel).PickUp))
                {
                    throw new Exception("The parcel is collected already");
                }
                if (parcel.Scheduled.Equals(default(IDal.DO.Parcel).Scheduled))
                {
                    throw new Exception("The parcel is not schedueld.");
                }
                IDal.DO.Customer senderP;
                try
                {
                    senderP = dal.getCustomerWithSpecificCondition(customer => customer.ID == parcel.SenderId).First();
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
                Drone bLDroneToSuplly = getBLDroneWithSpecificCondition(d => d.Id == droneId).First();
                IDal.DO.Parcel parcelToDelivery = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (parcelToDelivery.PickUp.Equals(default(IDal.DO.Parcel).PickUp) && !parcelToDelivery.Delivered.Equals(default(IDal.DO.Parcel).Delivered))
                {
                    throw new Exception("Drone cann't deliver this parcel.");
                }
                IDal.DO.Customer senderP;
                IDal.DO.Customer targetP;
                senderP = dal.getCustomerWithSpecificCondition(c => c.ID == parcelToDelivery.SenderId).First();
                targetP = dal.getCustomerWithSpecificCondition(c => c.ID == parcelToDelivery.TargetId).First();
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
            catch (ObjNotExistException e)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
            catch (Exception)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
        }

        private static ParcelStatuses findParcelStatus(IDal.DO.Parcel p)
        {
            if (p.Requeasted == DateTime.MinValue)
                return (ParcelStatuses)0;
            else if (p.Scheduled == DateTime.MinValue)
                return (ParcelStatuses)1;
            else if (p.PickUp == DateTime.MinValue)
                return (ParcelStatuses)2;
            else // if (p.Delivered == DateTime.MinValue)
                return (ParcelStatuses)3;
        }

        private void updateBLDrone(Drone d)
        {
            try
            {
                Drone findDrone = getBLDroneWithSpecificCondition(e => e.Id == d.Id).First();
                findDrone = d;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void GetParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority)
        {
            IDal.DO.Parcel p = new IDal.DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Requeasted = DateTime.Now, Weight = weight };
            dal.AddParcel(p);
        }

        private DroneInParcel createBLDroneInParcel(IDal.DO.Parcel p, int droneId)
        {
            Drone d = getBLDroneById(droneId);
            return new DroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithParcel = d.DronePosition };
        }
    }
}
