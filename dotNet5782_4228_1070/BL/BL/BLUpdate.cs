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
        public void DroneChangeModel(int id, string newModel)
        {
            try
            {
                BLDrone d = dronesInBL.First(drone => drone.Id == id);
                if (d.Equals(default(BLDrone)))
                    throw new Exception($"ERROR: Drone {id} not found");
                else
                {
                    dronesInBL.Remove(d);
                    d.Model = newModel;
                    dronesInBL.Add(d);
                    dal.changeDroneInfo(id,newModel);
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void StationChangeDetails(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
        {
            IDal.DO.Station s = dal.getStationById(id);
            if (name != null)
                s.Name = name;
            if (s.ChargeSlots <= ChargeSlots)
                s.ChargeSlots = ChargeSlots;
            if (s.ChargeSlots > ChargeSlots)
            {
                List<IDal.DO.DroneCharge> droneCharges = dal.displayDrone().Cast<IDal.DO.DroneCharge>().ToList();
                int amountDroneChargesFull = droneCharges.Count(station => station.StationId == s.Id);
                if (amountDroneChargesFull < ChargeSlots)
                    s.ChargeSlots = ChargeSlots;
                else
                    throw new Exception($"The amount Charging slots you want to change is smaller than the amount of drones that are charging now in station number {id}");
            }
            dal.changeStationInfo(s);
        }

        public void UpdateCustomerDetails(int id, string name = null, string phone = null)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            if (name != null)
                c.Name = name;
            if (phone != null && phone.Length >=9 && phone.Length <=10)
                c.Phone = phone;
            dal.changeCustomerInfo(c);
        }

        public void SendDroneToCharge(int droneId)
        {
            try
            {
                BLDrone drone = dronesInBL.First(d => d.Id == droneId);
                if (drone.Status == DroneStatus.Available)
                {
                    IDal.DO.Station availbleSforCharging = findAvailbleAndClosestStationForDrone(drone.DronePosition);
                    IDal.DO.DroneCharge droneCharge = new IDal.DO.DroneCharge() { StationId = availbleSforCharging.Id, DroneId = droneId };
                    drone.Battery = (distance(drone.DronePosition, new BLPosition() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude }))*(int)Electricity.empty;
                    drone.Status = DroneStatus.Maintenance;
                    drone.DronePosition = new BLPosition() { Latitude = availbleSforCharging.Latitude, Longitude = availbleSforCharging.Longitude };
                    availbleSforCharging.ChargeSlots--;
                    dal.AddDroneCharge(droneCharge);
                    dal.changeStationInfo(availbleSforCharging);
                    dal.changeDroneInfo(convertBLToDalDrone(drone));
                    //dal.changeStationInfo // slots????????????????????????
                }
                else
                {
                    throw new ObjNotAvailableException("Drone not vailable to charge.");
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void FreeDroneFromCharging(int droneId, double timeCharging)
        {
            try
            {
                BLDrone blDrone = new BLDrone();
                dronesInBL.First(d => d.Id == droneId && d.Status == DroneStatus.Maintenance);

                blDrone.Status = DroneStatus.Available;
                blDrone.Battery += (double)timeCharging * requestElectricity(4);
                IDal.DO.DroneCharge droneChargeByStation = dal.getDroneChargeByDroneId(blDrone.Id);
                IDal.DO.Station s = dal.getStationById(droneChargeByStation.StationId);
                s.ChargeSlots++;
                StationChangeDetails(s.Id, null, s.ChargeSlots);
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            IDal.DO.Customer senderMaxParcel;
            BLDrone droneToParcel = getBLDroneById(droneId);
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            IDal.DO.Parcel maxParcel = new IDal.DO.Parcel() { Weight = 0, };//parcels.First(); //check if weght is good=====================
            foreach (IDal.DO.Parcel p in parcels)
            {
                if (p.Requeasted.Equals(default(IDal.DO.Parcel).Requeasted))
                    break;
                senderP = dal.getCustomerById(p.SenderId);
                targetP = dal.getCustomerById(p.TargetId);
                BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                BLPosition targetPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                senderMaxParcel = dal.getCustomerById(maxParcel.SenderId);
                double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                double disSenderToTarget = distance(senderPosition, targetPosition);
                IDal.DO.Station MinDisFromTargetTostation = findAvailbleAndClosestStationForDrone(targetPosition);
                double disTargetToStation = distance(targetPosition, new BLPosition() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude });
                double disMaxPToSender = distance(droneToParcel.DronePosition, new BLPosition() { Longitude = senderMaxParcel.Longitude, Latitude = senderMaxParcel.Latitude });
                double droneElectricity = requestElectricity((int)p.Weight);
                if (droneToParcel.Battery - (int)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity) > 0) //[4]
                {
                    //BLParcel bLParcel = convertDalToBLParcel(p);
                    if (p.Weight <= droneToParcel.MaxWeight)
                    {
                        if (maxParcel.Priority < p.Priority)
                        {
                            maxParcel = p;
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
                                }
                            }
                        }
                    }
                }
            }
            if (maxParcel.Weight == 0) //in the beggining parcels' weight = 0 to start compairing ;
            {
                throw new Exception("Drone with the parcels' conditions wasn't found.");
            }
            droneToParcel.Status = DroneStatus.Delivery;
            updateBLDrone(droneToParcel);
            maxParcel.DroneId = droneToParcel.Id;
            maxParcel.Scheduled = DateTime.Now;
            dal.changeParcelInfo(maxParcel);
        }

        public void DronePicksUpParcel(int droneId)// ParcelStatuses.PickedUp          
        {
            try
            {
                BLDrone bLDrone = dronesInBL.First(d => d.Id == droneId && d.Status == DroneStatus.Delivery);
                IDal.DO.Parcel p = dal.getParcelByDroneId(droneId);
                if (!p.PickUp.Equals(default(IDal.DO.Parcel).PickUp))
                {
                    throw new Exception("The parcel is collected already");
                }
                if (p.Scheduled.Equals(default(IDal.DO.Parcel).Scheduled))
                {
                    throw new Exception("The parcel is not schedueld.");
                }
                IDal.DO.Customer senderP = dal.getCustomerById(p.SenderId);
                BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                double disDroneToSenderP = distance(bLDrone.DronePosition, senderPosition);

                bLDrone.Battery -= disDroneToSenderP * requestElectricity((int)p.Weight);
                bLDrone.DronePosition = senderPosition;
                updateBLDrone(bLDrone);
                p.PickUp = DateTime.Now;
                dal.changeParcelInfo(p);
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        public void DeliveryParcelByDrone(int idDrone) //ParcelStatuses.Delivered.
        {
            BLDrone bLDroneToSuplly = getBLDroneById(idDrone);
            IDal.DO.Parcel parcelToDelivery = dal.getParcelByDroneId(idDrone);
            if (parcelToDelivery.PickUp.Equals(default(IDal.DO.Parcel).PickUp) && !parcelToDelivery.Delivered.Equals(default(IDal.DO.Parcel).Delivered))
            {
                throw new Exception("Drone cann't deliver this parcel.");
            }
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            senderP = dal.getCustomerById(parcelToDelivery.SenderId);
            targetP = dal.getCustomerById(parcelToDelivery.TargetId);
            BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            BLPosition targetPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            double disSenderToTarget = distance(senderPosition, targetPosition);
            double electricity = requestElectricity((int)parcelToDelivery.Weight);
            bLDroneToSuplly.Battery -= electricity * disSenderToTarget;
            bLDroneToSuplly.DronePosition = targetPosition;
            bLDroneToSuplly.Status = DroneStatus.Available;
            updateBLDrone(bLDroneToSuplly);
            parcelToDelivery.Delivered = DateTime.Now;
            dal.changeParcelInfo(parcelToDelivery);
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
        } //not in Ibl

        private void updateBLDrone(BLDrone d)
        {
            try
            {
                BLDrone findDrone = dronesInBL.First(e => e.Id == d.Id);
                findDrone = d;
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
        }

        internal static string checkNullforPrint<T>(T t)
        {
            if (t.Equals(default(T)))
                return $"--field {t.GetType()} not filled yet.--";
            return t.ToString();
        }

        public void GetParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority)
        {
            IDal.DO.Parcel p = new IDal.DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Delivered = new DateTime(), Requeasted = DateTime.Now, Scheduled = new DateTime(), Weight = weight, PickUp = new DateTime() };
            dal.AddParcel(p);
        }

        private static BLDroneInParcel createBLDroneInParcel(IDal.DO.Parcel p, int droneId)
        {
            BLDrone d = new BLDrone();//= getBLDroneByID(droneId);
            return new BLDroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithParcel = d.DronePosition };
        }

        private static BLParcelAtCustomer createtDalParcelToBLParcelAtCustomer(IDal.DO.Parcel p, IDal.DO.Customer c)
        {
            //if(p.senderId == c.Id || p.TargetId == c.Id)
            return new BLParcelAtCustomer() { Id = p.Id, Weight = p.Weight, Priority = p.Priority, ParcelStatus = findParcelStatus(p), SenderOrTargetCustomer = new BLCustomerInParcel() { Id = c.ID, name = c.Name } };
        }
    }
}
