using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL
    {
        public void droneChangeModel(int id, string newModel)
        {
            BLDrone d = dronesInBL.Find(drone => drone.Id == id);
            _ = !d.Equals(null) ? d.Model = newModel : throw new Exception($"ERROR: Drone {id} not found");
            IDal.DO.Drone dr = dal.getDroneById(id);
            dr.Model = newModel;
            //check if it valid to do it. (the changing of dal - here)
        }

        public void stationChangeDetails(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
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
        }

        public void updateCustomerDetails(int id, string name = null, string phone = null)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            if (c.Equals(null))
            {
                throw new Exceptions.ObjNotExistException<IDal.DO.Customer>(c);
            }
            if (name != null)
                c.Name = name;
            if (phone != null)
                c.Phone = phone;
        }
        public void sendDroneToCharge(int droneId)
        {
            BLDrone drone = dronesInBL.Find(d => d.Id == droneId);
            if (drone.Status == DroneStatus.Available)
            {

            }
            else
            {

            }
        }

        public void freeDroneFromCharging(int droneId, double timeCharging)
        {
            BLDrone blDrone;
            try
            {
                dronesInBL.First(d => d.Id == droneId && d.Status == DroneStatus.Maintenance);
            }
            catch (Exception e)
            {
                throw new Exceptions.ObjNotExistException<BLDrone>(blDrone);
            }

            blDrone.Status = DroneStatus.Available;
            blDrone.Battery += (double)timeCharging * requestElectricity()[4];
            IDal.DO.DroneCharge droneChargeByStation = dal.getDroneChargeByDroneId(blDrone.Id);
            IDal.DO.Station s = dal.getStationById(droneChargeByStation.StationId);
            s.ChargeSlots++;
            stationChangeDetails(s.Id, null, s.ChargeSlots);
            //dal.removeDroneFromCharge();
        }

        public void PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            IDal.DO.Customer senderMaxParcel;
            BLDrone droneToParcel = GetBLDroneById(droneId);
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            IDal.DO.Parcel maxParcel = new IDal.DO.Parcel() { Weight = 0, };//parcels.First(); //check if weght is good=====================
            foreach (IDal.DO.Parcel p in parcels)
            {
                if (p.Requeasted.Equals(null))
                    break;
                senderP = dal.getCustomerById(p.SenderId);
                targetP = dal.getCustomerById(p.TargetId);
                BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                BLPosition targetPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                senderMaxParcel = dal.getCustomerById(maxParcel.SenderId);
                double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                double disSenderToTarget = distance(senderPosition, targetPosition);
                IDal.DO.Station MinDisFromTargetTostation = getMinDistanceFromStation(targetPosition);
                double disTargetToStation = distance(targetPosition, new BLPosition() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude });
                double disMaxPToSender = distance(droneToParcel.DronePosition, new BLPosition() { Longitude = senderMaxParcel.Longitude, Latitude = senderMaxParcel.Latitude });
                double droneElectricity = requestElectricity()[(int)p.Weight];
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
            updateParcel(maxParcel);
        }

        public void DronePicksUpParcel(int droneId)// ParcelStatuses.PickedUp          
        {
            BLDrone bLDrone = dronesInBL.Find(d => d.Id == droneId && d.Status == DroneStatus.Delivery);
            IDal.DO.Parcel p = dal.getParcelByDroneId(droneId);
            if (!p.PickUp.Equals(null))
            {
                throw new Exception("The parcel is collected already");
            }
            if (p.Scheduled.Equals(null))
            {
                throw new Exception("The parcel is not schedueld.");
            }
            IDal.DO.Customer senderP = dal.getCustomerById(p.SenderId);
            BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            double disDroneToSenderP = distance(bLDrone.DronePosition, senderPosition);

            bLDrone.Battery -= disDroneToSenderP * requestElectricity()[(int)p.Weight];
            bLDrone.DronePosition = senderPosition;
            updateBLDrone(bLDrone);
            p.PickUp = DateTime.Now;
            updateParcel(maxParcel);
        }

        public void deliveryParcelByDrone(int idDrone) //ParcelStatuses.Delivered.
        {
            BLDrone bLDroneToSuplly = GetBLDroneById(idDrone);
            IDal.DO.Parcel parcelToDelivery = dal.getParcelByDroneId(idDrone);
            if (parcelToDelivery.PickUp.Equals(null) && !parcelToDelivery.Delivered.Equals(null))
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
            double ectricity = requestElectricity()[(int)parcelToDelivery.Weight];
            bLDroneToSuplly.Battery -= ectricity * disSenderToTarget;
            bLDroneToSuplly.DronePosition = targetPosition;
            bLDroneToSuplly.Status = DroneStatus.Available;
            updateBLDrone(bLDroneToSuplly);
            parcelToDelivery.Delivered = DateTime.Now;
            updateParcel(parcelToDelivery);

        }

        private static bool checkNull<T>(T t)
        {
            if (t.Equals(null))
                return false;
            return true;
        } //not in Ibl
        private static ParcelStatuses findParcelStatus(IDal.DO.Parcel p)
        {
            if (p.Requeasted.Equals(null))
                return (ParcelStatuses)0;
            else if (p.Scheduled.Equals(null))
                return (ParcelStatuses)1;
            else if (p.PickUp.Equals(null))
                return (ParcelStatuses)2;
            else // if (p.Delivered.Equals(null))
                return (ParcelStatuses)3;
        } //not in Ibl
          // Throw match exception - "drone not available to charge"

        public void updateBLDrone(BLDrone d)
        {
            BLDrone findDrone = dronesInBL.Find(e => e.Id == d.Id);
            findDrone = d;
        }

        public IDal.DO.Station getMinDistanceFromStation(BLPosition p)
        {
            return new IDal.DO.Station();
        }

        internal static string checkNullforPrint<T>(T t)
        {
            if (t.Equals(null))
                return $"--field {t.GetType()} not filled yet.--";
            return t.ToString();
        }

        public void getParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority)
        {
            IDal.DO.Parcel p = new IDal.DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Delivered = new DateTime(), Requeasted = DateTime.Now, Scheduled = new DateTime(), Weight = weight, PickUp = new DateTime() };
            dal.AddParcel(p);
        }

        private static BLDroneInParcel createBLDroneInParcel(IDal.DO.Parcel p, int droneId)
        {
            BLDrone d = GetBLDroneById(droneId);
            return new BLDroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithparcel = d.DronePosition };
        }

        private static BLParcelAtCustomer createtDalParcelToBLParcelAtCustomer(IDal.DO.Parcel p, IDal.DO.Customer c)
        {
            //if(p.senderId == c.Id || p.TargetId == c.Id)
            return new BLParcelAtCustomer() { Id = p.Id, Weight = p.Weight, Priority = p.Priority, ParcelStatus = findParcelStatus(p), SenderOrTargetCustomer = new BLCustomerInParcel() { Id = c.ID, name = c.Name } };
        }
    }
}
