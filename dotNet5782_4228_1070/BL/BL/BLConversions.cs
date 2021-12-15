﻿using System;
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

        private ParcelInTransfer convertDalToParcelInTranspare(IDal.DO.Parcel parcel)
        {
            return new ParcelInTransfer()
            {
                Id = parcel.Id,
                
            }
        }
        private IDal.DO.Station convertBLToDalStation(Station s)
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

        private IDal.DO.Drone convertBLToDalDrone(Drone d)
        {
            return new IDal.DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight
            };
        }

        private IDal.DO.Customer convertBLToDalCustomer(Customer c)
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

        private IDal.DO.Parcel convertBLToDalParcel(Parcel p)
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

        private Station convertDalToBLStation(IDal.DO.Station s)
        {
            List<ChargingDrone> blDroneChargingByStation = new List<ChargingDrone>();
            IEnumerable<IDal.DO.DroneCharge> droneChargesByStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == s.Id);
            foreach(IDal.DO.DroneCharge d in droneChargesByStation)
            {
                blDroneChargingByStation.Add(new ChargingDrone() { Id = d.DroneId, Battery = getBLDroneById(d.DroneId).Battery });
            };
            return new Station() { ID = s.Id, Name = s.Name, StationPosition = new IBL.BO.Position() { Longitude = s.Longitude, Latitude = s.Latitude }, DroneChargeAvailble = s.ChargeSlots - blDroneChargingByStation.Count(), DronesCharging = blDroneChargingByStation };
        }

        private Customer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            IEnumerable<IDal.DO.Parcel> sendingParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == c.ID);
            IEnumerable<IDal.DO.Parcel> targetParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == c.ID);
            List<ParcelAtCustomer> customerAsSender = createBLParcelAtCustomer(sendingParcels, true);
            List<ParcelAtCustomer> customerAsTarget = createBLParcelAtCustomer(targetParcels , false);
            return new Customer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.Position() { Longitude = c.Longitude, Latitude = c.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsTarget };
        }
        private List<ParcelAtCustomer> createBLParcelAtCustomer(IEnumerable<IDal.DO.Parcel> pp, bool senderOrTaget)
        {
            List<ParcelAtCustomer> parcelCustomers = new List<ParcelAtCustomer>();
            CustomerInParcel bLCustomerInParcel = new CustomerInParcel();
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
                parcelCustomers.Add(new ParcelAtCustomer() { Id = p.Id, Priority = p.Priority, Weight = p.Weight, SenderOrTargetCustomer = bLCustomerInParcel, ParcelStatus = parcelStatusesTemp });
            }
            return parcelCustomers;
        }

        private Drone convertDalToBLDrone(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            Drone BLDrone = getBLDroneById(d.Id);
            return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
        }
        private List<DroneToList> convertBLDroneToBLDronesToList(List<Drone> drones )
        {
            List<DroneToList> listDrones = new List<DroneToList>();
            DroneToList toAdd = new DroneToList();
            foreach (Drone d in drones)
            {
                if(d.ParcelInTransfer == (null))
                    toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition};
                else
                    toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition, IdParcel = d.ParcelInTransfer.Id };
                listDrones.Add(toAdd);
            }
            return listDrones;
        }

        public Drone convertDroneToListToDrone(DroneToList d)
        {
            return getBLDroneById(d.Id);
        }

        private Drone copyDalToBLDroneInfo(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100) };
        }

        private ChargingDrone convertDalToBLChargingDrone(IDal.DO.DroneCharge droneCharge) //convertDalToBLChargingDrone the opposite BL to DAL
        {
            return new ChargingDrone()
            {
                Id = droneCharge.DroneId,
                Battery = getBLDroneWithSpecificCondition(d => d.Id == droneCharge.DroneId).First().Battery
            };
        }

        private Parcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            DroneInParcel drone = null;
            if (!p.Scheduled.Equals(default(IDal.DO.Parcel).Scheduled)) //if the parcel is paired with a drone
            {
                drone = createBLDroneInParcel(p, getBLDroneWithSpecificCondition(d => d.Id == (int)p.DroneId).First().Id);
            }

            return new Parcel()
            {
                Id = p.Id,
                Sender = new CustomerInParcel() { Id = p.SenderId, name = dal.getCustomerWithSpecificCondition(c => c.ID == p.SenderId).First().Name },
                Target = new CustomerInParcel() { Id = p.TargetId, name = dal.getCustomerWithSpecificCondition(c => c.ID == p.TargetId).First().Name },
                Weight = p.Weight,
                Drone = createBLDroneInParcel(p, (int)p.DroneId),
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
        }

        private CustomerInParcel convertDalToBLCustomerInTransfer(IDal.DO.Customer customer)
        {
            return new CustomerInParcel()
            {
                Id = customer.ID,
                name = customer.Name
            };
        }

        private ParcelInTransfer createParcelInTransfer(IDal.DO.Parcel p, IDal.DO.Customer sender, IDal.DO.Customer target)
        {
            Position senderP = new Position() { Latitude = sender.Latitude, Longitude = sender.Longitude };
            Position targetP = new Position() { Latitude = target.Latitude, Longitude = target.Longitude };
            return new ParcelInTransfer()
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
