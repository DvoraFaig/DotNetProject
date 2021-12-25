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

        private ParcelInTransfer convertDalToParcelInTranspare(DO.Parcel parcel)
        {
            return new ParcelInTransfer()
            {
                Id = parcel.Id,

            };
        }
        private DO.Station convertBLToDalStation(Station s)
        {
            return new DO.Station()
            {
                Id = s.ID,
                Name = s.Name,
                ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(),
                Longitude = s.StationPosition.Longitude,
                Latitude = s.StationPosition.Latitude
            };
        }

        private DO.Drone convertBLToDalDrone(Drone d)
        {
            return new DO.Drone()
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight
            };
        }

        private DO.Customer convertBLToDalCustomer(Customer c)
        {
            return new DO.Customer()
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Longitude = c.CustomerPosition.Longitude,
                Latitude = c.CustomerPosition.Latitude,
            };
        }

        private DO.Parcel convertBLToDalParcel(Parcel p)
        {
            DO.Parcel parcel = new DO.Parcel()
            {
                Id = p.Id,
                Weight = p.Weight,
                Priority = p.Priority,
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
            if (p.Sender != null) parcel.SenderId = p.Sender.Id;
            if (p.Target != null) parcel.TargetId = p.Target.Id;
            if (p.Drone != null) parcel.DroneId = p.Drone.Id;
            return parcel;
        }
        /// <summary>
        /// ///////////////////////////////////////////////////
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private Station convertDalToBLStation(DO.Station s)
        {
            List<ChargingDrone> blDroneChargingByStation = new List<ChargingDrone>();
            IEnumerable<DO.DroneCharge> droneChargesByStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == s.Id);
            foreach (DO.DroneCharge d in droneChargesByStation)
            {
                blDroneChargingByStation.Add(new ChargingDrone() { Id = d.DroneId, Battery = getBLDroneById(d.DroneId).Battery });
            };
            int availableChargingSlots = s.ChargeSlots - blDroneChargingByStation.Count();
            return new Station() { ID = s.Id, Name = s.Name, StationPosition = new BO.Position() { Longitude = s.Longitude, Latitude = s.Latitude }, DroneChargeAvailble = availableChargingSlots, DronesCharging = blDroneChargingByStation };
        }
        private Customer convertDalToBLCustomer(DO.Customer c)
        {
            IEnumerable<DO.Parcel> sendingParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == c.Id);
            IEnumerable<DO.Parcel> targetParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == c.Id);
            List<ParcelAtCustomer> customerAsSender = createBLParcelAtCustomer(sendingParcels, true);
            List<ParcelAtCustomer> customerAsTarget = createBLParcelAtCustomer(targetParcels, false);
            return new Customer() { Id = c.Id, Name = c.Name, Phone = c.Phone, CustomerPosition = new BO.Position() { Longitude = c.Longitude, Latitude = c.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsTarget };
        }
        private List<ParcelAtCustomer> createBLParcelAtCustomer(IEnumerable<DO.Parcel> parcels, bool senderOrTaget)
        {
            List<ParcelAtCustomer> parcelCustomers = new List<ParcelAtCustomer>();
            CustomerInParcel bLCustomerInParcel = new CustomerInParcel();
            ParcelStatuses parcelStatusesTemp;
            foreach (DO.Parcel parcel in parcels)
            {
                if (senderOrTaget) //sender
                {
                    bLCustomerInParcel.Id = parcel.SenderId;
                    bLCustomerInParcel.name = dal.getCustomerWithSpecificCondition(c => c.Id == parcel.SenderId).First().Name;
                }
                else //target
                {
                    bLCustomerInParcel.Id = parcel.TargetId;
                    bLCustomerInParcel.name = dal.getCustomerWithSpecificCondition(c => c.Id == parcel.TargetId).First().Name;
                }
                parcelStatusesTemp = findParcelStatus(parcel);
                //if (parcel.Delivered != null)
                //    parcelStatusesTemp = ParcelStatuses.Delivered;
                //else if (parcel.PickUp != null)
                //    parcelStatusesTemp = ParcelStatuses.PickedUp;
                //else if (parcel.Requeasted != null)
                //    parcelStatusesTemp = ParcelStatuses.Requeasted;
                //else
                //    parcelStatusesTemp = ParcelStatuses.Scheduled;
                parcelCustomers.Add(new ParcelAtCustomer() { Id = parcel.Id, Priority = parcel.Priority, Weight = parcel.Weight, SenderOrTargetCustomer = bLCustomerInParcel, ParcelStatus = parcelStatusesTemp });
            }
            return parcelCustomers;
        }

        private Drone convertDalToBLDrone(DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            Drone BLDrone = getBLDroneById(d.Id);
            return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
        }
        private List<DroneToList> convertBLDroneToBLDronesToList(List<Drone> drones)
        {
            List<DroneToList> listDrones = new List<DroneToList>();
            DroneToList toAdd = new DroneToList();
            foreach (Drone d in drones)
            {
                if (d.ParcelInTransfer == (null))
                    toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition };
                else
                    toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition, IdParcel = d.ParcelInTransfer.Id };
                listDrones.Add(toAdd);
            }
            return listDrones;
        }

        private List<ParcelToList> convertBLParcelToBLParcelsToList()
        {
            IEnumerable<DO.Parcel> parcels = dal.displayParcels();
            List<ParcelToList> listParcels = new List<ParcelToList>();
            ParcelToList toAdd = new ParcelToList();
            foreach (DO.Parcel p in parcels)
            {
                toAdd = new ParcelToList() { Id = p.Id, SenderName = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First().Name, TargetName = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First().Name, Weight = p.Weight, Priority = p.Priority, ParcelStatus = findParcelStatus(p) };
                listParcels.Add(toAdd);
            }
            return listParcels;
        }

        private List<ParcelToList> convertBLParcelToBLParcelsToList(List<Parcel> parcels)
        {
            List<ParcelToList> listParcels = new List<ParcelToList>();
            ParcelToList toAdd = new ParcelToList();
            foreach (Parcel p in parcels)
            {
                toAdd = new ParcelToList() { Id = p.Id, SenderName = p.Sender.name, TargetName = p.Target.name, Weight = p.Weight, Priority = p.Priority };
                toAdd.ParcelStatus = findParcelStatus(p);
                listParcels.Add(toAdd);
            }
            return listParcels;
        }

        private Drone copyDalToBLDroneInfo(DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100) };
        }

        private ChargingDrone convertDalToBLChargingDrone(DO.DroneCharge droneCharge) //convertDalToBLChargingDrone the opposite BL to DAL
        {
            return new ChargingDrone()
            {
                Id = droneCharge.DroneId,
                Battery = getBLDroneWithSpecificCondition(d => d.Id == droneCharge.DroneId).First().Battery
            };
        }

        private Parcel convertDalToBLParcel(DO.Parcel p)
        {
            DroneInParcel drone = null;
            Parcel parcel = new Parcel();
            if (!p.Scheduled.Equals(default(DO.Parcel).Scheduled)) //if the parcel is paired with a drone
            {
                drone = createBLDroneInParcel(p, getBLDroneWithSpecificCondition(d => d.Id == (int)p.DroneId).First().Id);
            }
            return new Parcel()
            {
                Id = p.Id,
                Sender = new CustomerInParcel() { Id = p.SenderId, name = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First().Name },
                Target = new CustomerInParcel() { Id = p.TargetId, name = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First().Name },
                Weight = p.Weight,
                Drone = drone,
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered,
                Priority = p.Priority
            };
        }

        private CustomerInParcel convertDalToBLCustomerInParcel(DO.Customer customer)
        {
            return new CustomerInParcel()
            {
                Id = customer.Id,
                name = customer.Name
            };
        }

        private ParcelInTransfer createParcelInTransfer(DO.Parcel p, DO.Customer sender, DO.Customer target)
        {
            Position senderP = new Position() { Latitude = sender.Latitude, Longitude = sender.Longitude };
            Position targetP = new Position() { Latitude = target.Latitude, Longitude = target.Longitude };
            return new ParcelInTransfer()
            {
                TargetPosition = targetP,
                SenderPosition = senderP,
                Id = p.Id,
                SenderCustomer = convertDalToBLCustomerInParcel(sender),
                TargetCustomer = convertDalToBLCustomerInParcel(target),
                parcelStatus = false,
                Priority = p.Priority,
                distance = distance(senderP, targetP),
                Weight = p.Weight
            };
        }
        private CustomerToList converteCustomerToList(DO.Customer customer)
        {
            CustomerToList customerToList = new CustomerToList() { Id = customer.Id, Name = customer.Name, Phone = customer.Phone };
            customerToList.AmountAsSendingDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == customer.Id && p.Delivered != null).Count();
            customerToList.AmountAsSendingUnDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == customer.Id && p.Delivered == null).Count();
            customerToList.AmountAsTargetDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == customer.Id && p.Delivered != null).Count();
            customerToList.AmountAsTargetUnDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == customer.Id && p.Delivered == null).Count();
            return customerToList;
        }
    }
}
