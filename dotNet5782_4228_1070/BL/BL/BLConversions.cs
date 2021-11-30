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
        private IDal.DO.Station convertBLToDalStation(BLStation s)
        {
            return new IDal.DO.Station()
            {
                Id = s.ID,
                Name = s.Name,
                ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(),
                Longitude = s.StationPosition.Longitude,
                Latitude = s.StationPosition.Latitude
            };
        }

        private IDal.DO.Drone convertBLToDalDrone(BLDrone d)
        {
            return new IDal.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight
            };
        }

        private IDal.DO.Customer convertBLToDalCustomer(BLCustomer c)
        {
            return new IDal.DO.Customer()
            {
                ID = c.ID,
                Name = c.Name,
                Phone = c.Phone,
                Longitude = c.CustomerPosition.Longitude,
                Latitude = c.CustomerPosition.Latitude,
            };
        }

        private IDal.DO.Parcel convertBLToDalParcel(BLParcel p)
        {
            return new IDal.DO.Parcel()
            {
                Id = p.Id,
                SenderId = p.Sender.Id,
                TargetId = p.Target.Id,
                Weight = p.Weight,
                Priority = p.Priority,
                DroneId = p.Drone.Id,
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
        }

        private BLStation convertDalToBLStation(IDal.DO.Station s)
        {
            List<BLChargingDrone> blDroneChargingByStation = new List<BLChargingDrone>();
            IEnumerable<IDal.DO.DroneCharge> droneChargesByStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == s.Id);
            foreach(IDal.DO.DroneCharge d in droneChargesByStation)
            {
                blDroneChargingByStation.Add(new BLChargingDrone() { Id = d.DroneId, Battery = getBLDroneById(d.DroneId).Battery });
            };
            return new BLStation() { ID = s.Id, Name = s.Name, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude }, DroneChargeAvailble = s.ChargeSlots - blDroneChargingByStation.Count(), DronesCharging = blDroneChargingByStation };
        }

        private BLCustomer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            IEnumerable<IDal.DO.Parcel> sendingParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == c.ID);
            IEnumerable<IDal.DO.Parcel> targetParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == c.ID);
            List<BLParcelAtCustomer> customerAsSender = createBLParcelAtCustomer(sendingParcels, true);
            List<BLParcelAtCustomer> customerAsTarget = createBLParcelAtCustomer(targetParcels , false);
            return new BLCustomer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsTarget };
        }
        private List<BLParcelAtCustomer> createBLParcelAtCustomer(IEnumerable<IDal.DO.Parcel> pp, bool senderOrTaget)
        {
            List<BLParcelAtCustomer> parcelCustomers = new List<BLParcelAtCustomer>();
            BLCustomerInParcel bLCustomerInParcel = new BLCustomerInParcel();
            ParcelStatuses parcelStatusesTemp;
            foreach (IDal.DO.Parcel p in pp)
            {
                if (senderOrTaget) //sender
                {
                    bLCustomerInParcel.Id = p.SenderId;
                    bLCustomerInParcel.name = dal.getCustomerWithSpecificCondition(c => c.ID == p.SenderId).First().Name;
                }
                else //target
                {
                    bLCustomerInParcel.Id = p.TargetId;
                    bLCustomerInParcel.name = dal.getCustomerWithSpecificCondition(c => c.ID == p.TargetId).First().Name;
                }
                if (p.Delivered != null)
                    parcelStatusesTemp = ParcelStatuses.Delivered;
                else if (p.PickUp != null)
                    parcelStatusesTemp = ParcelStatuses.PickedUp;
                if (p.Requeasted != null)
                    parcelStatusesTemp = ParcelStatuses.Requeasted;
                else
                    parcelStatusesTemp = ParcelStatuses.Scheduled;
                parcelCustomers.Add(new BLParcelAtCustomer() { Id = p.Id, Priority = p.Priority, Weight = p.Weight, SenderOrTargetCustomer = bLCustomerInParcel, ParcelStatus = parcelStatusesTemp });
            }
            return parcelCustomers;
        }

        private BLDrone convertDalToBLDrone(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            BLDrone BLDrone = getBLDroneById(d.Id);
            return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
        }

        private BLDrone copyDalToBLDroneInfo(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100) };
        }

        private BLChargingDrone convertDalToBLChargingDrone(IDal.DO.DroneCharge droneCharge) //convertDalToBLChargingDrone the opposite BL to DAL
        {
            return new BLChargingDrone()
            {
                Id = droneCharge.DroneId,
                Battery = getBLDroneWithSpecificCondition(d => d.Id == droneCharge.DroneId).First().Battery
            };
        }

        private BLParcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            BLDroneInParcel drone = null;
            if (!p.Scheduled.Equals(default(IDal.DO.Parcel).Scheduled)) //if the parcel is paired with a drone
            {
                drone = createBLDroneInParcel(p, getBLDroneWithSpecificCondition(d => d.Id == (int)p.DroneId).First().Id);
            }

            return new BLParcel()
            {
                Id = p.Id,
                Sender = new BLCustomerInParcel() { Id = p.SenderId, name = dal.getCustomerWithSpecificCondition(c => c.ID == p.SenderId).First().Name },
                Target = new BLCustomerInParcel() { Id = p.TargetId, name = dal.getCustomerWithSpecificCondition(c => c.ID == p.TargetId).First().Name },
                Weight = p.Weight,
                Drone = createBLDroneInParcel(p, (int)p.DroneId),
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
        }

        private BLCustomerInParcel convertDalToBLCustomerInTransfer(IDal.DO.Customer customer)
        {
            return new BLCustomerInParcel()
            {
                Id = customer.ID,
                name = customer.Name
            };
        }

        private BLParcelInTransfer createBLParcelInTransfer(IDal.DO.Parcel p, IDal.DO.Customer sender, IDal.DO.Customer target)
        {
            BLPosition senderP = new BLPosition() { Latitude = sender.Latitude, Longitude = sender.Longitude };
            BLPosition targetP = new BLPosition() { Latitude = target.Latitude, Longitude = target.Longitude };
            return new BLParcelInTransfer()
            {
                TargetPosition = targetP,
                SenderPosition = senderP,
                Id = p.Id,
                SenderCustomer = convertDalToBLCustomerInTransfer(sender),
                TargetCustomer = convertDalToBLCustomerInTransfer(target),
                parcelStatus = false,
                Priority = p.Priority,
                distance = distance(senderP, targetP),
                Weight = p.Weight
            };
        }
    }
}
