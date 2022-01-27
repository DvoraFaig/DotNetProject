using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl , BlApi.ISimulation
    {
        /// <summary>
        /// Receive a BO drone and return a converted DO drone - copy information.
        /// </summary>
        /// <param name="drone">Drone to convert</param>
        /// <returns></returns>
        private DO.Drone convertBLToDalDrone(Drone drone)
        {
            return new DO.Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = drone.MaxWeight
            };
        }

        /// <summary>
        /// Receive a BO station and return a converted DO station - copy information.
        /// </summary>
        /// <param name="s">Station to convert</param>
        /// <returns></returns>
        private DO.Station convertBLToDalStation(Station s)
        {
            DO.Station station = new DO.Station();
            station.Id = s.Id;
            station.Name = s.Name;
            station.Longitude = s.StationPosition.Longitude;
            station.Latitude = s.StationPosition.Latitude;
            int DronesCharging;
            try //if station doesn't have droneCharging.
            {
                DronesCharging = s.DronesCharging.Count();
            }
            catch (Exception)
            {
                DronesCharging = 0;
            }
            station.ChargeSlots = s.DroneChargeAvailble + DronesCharging;
            station.IsActive = true;
            return station;
        }

        /// <summary>
        /// Receive a DO customer and return a converted BO customer - copy information.      
        /// </summary>
        /// <param name="customer">Customer to convert</param>
        /// <returns></returns>
        private DO.Customer convertBLToDalCustomer(Customer customer)
        {
            return new DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.CustomerPosition.Longitude,
                Latitude = customer.CustomerPosition.Latitude,
                IsActive = true
            };
        }

        /// <summary>
        /// Receive a BO parcel and return a converted DO parcel - copy information.
        /// </summary>
        /// <param name="parcel">Parcel to convert</param>
        /// <returns></returns>
        private DO.Parcel convertBLToDalParcel(Parcel parcel)
        {
            DO.Parcel convertedParcel = new DO.Parcel()
            {
                Id = parcel.Id,
                Weight = parcel.Weight,
                Priority = parcel.Priority,
                Requeasted = parcel.Requeasted,
                Scheduled = parcel.Scheduled,
                PickUp = parcel.PickUp,
                Delivered = parcel.Delivered
            };
            if (parcel.Sender != null) convertedParcel.SenderId = parcel.Sender.Id;
            if (parcel.Target != null) convertedParcel.TargetId = parcel.Target.Id;
            if (parcel.Drone != null) convertedParcel.DroneId = parcel.Drone.Id;
            return convertedParcel;
        }

        /// <summary>
        /// Recieve a DO station and returns a converted BO station - add more infomation
        /// </summary>
        /// <param name="station">Station to convert</param>
        /// <returns></returns>
        private Station convertDalToBLStation(DO.Station station)
        {
            List<ChargingDrone> blDroneChargingByStation = new List<ChargingDrone>();
            IEnumerable<DO.DroneCharge> droneChargesByStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id);
            foreach (DO.DroneCharge droneCharge in droneChargesByStation)
            {
                blDroneChargingByStation.Add(new ChargingDrone() { Id = droneCharge.DroneId, Battery = getDroneByIdFromDronesList(droneCharge.DroneId).Battery });
            };
            int availableChargingSlots = station.ChargeSlots - blDroneChargingByStation.Count();
            return new Station() { Id = station.Id, Name = station.Name, StationPosition = new BO.Position() { Longitude = station.Longitude, Latitude = station.Latitude }, DroneChargeAvailble = availableChargingSlots, DronesCharging = blDroneChargingByStation };
        }

        /// <summary>
        /// Recieve a DO customer and returns a converted BO customer - add more infomation
        /// </summary>
        /// <param name="customer">Customer to convert</param>
        /// <returns></returns>
        public Customer convertDalToBLCustomer(DO.Customer customer)
        {
            IEnumerable<DO.Parcel> sendingParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == customer.Id);
            IEnumerable<DO.Parcel> targetParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == customer.Id);
            List<ParcelAtCustomer> customerAsSender = returnParcelsAtCustomer(sendingParcels, true);
            List<ParcelAtCustomer> customerAsTarget = returnParcelsAtCustomer(targetParcels, false);
            return new Customer() { Id = customer.Id, Name = customer.Name, Phone = customer.Phone, CustomerPosition = new BO.Position() { Longitude = customer.Longitude, Latitude = customer.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsTarget };
        }

        /// <summary>
        /// converte CustomerToList from DO.Customer
        /// </summary>
        /// <param name="customer">customer to convert</param>
        /// <returns></returns>
        private CustomerToList converteCustomerToList(DO.Customer customer)
        {
            CustomerToList customerToList = new CustomerToList() { Id = customer.Id, Name = customer.Name, Phone = customer.Phone };
            customerToList.AmountAsSendingDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == customer.Id && p.Delivered != null).Count();
            customerToList.AmountAsSendingUnDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.SenderId == customer.Id && p.Delivered == null).Count();
            customerToList.AmountAsTargetDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == customer.Id && p.Delivered != null).Count();
            customerToList.AmountAsTargetUnDeliveredParcels = dal.getParcelWithSpecificCondition(p => p.TargetId == customer.Id && p.Delivered == null).Count();
            return customerToList;
        }

        /// <summary>
        /// Return a list ParcelAtCustomer (BO object) of a specific customer occurding to the parameters.
        /// </summary>
        /// <param name="parcelsOfSpecificCustomer">Parcels that are sent / recieved by the specific customer</param>
        /// <param name="senderOrTaget">Customers' parcels is a sender / target </param>
        /// <returns></returns>
        private List<ParcelAtCustomer> returnParcelsAtCustomer(IEnumerable<DO.Parcel> parcelsOfSpecificCustomer, bool senderOrTaget)
        {
            List<ParcelAtCustomer> parcelCustomers = new List<ParcelAtCustomer>();
            CustomerInParcel bLCustomerInParcel = new CustomerInParcel();
            DO.ParcelStatuses parcelStatusesTemp;
            foreach (DO.Parcel parcel in parcelsOfSpecificCustomer)
            {
                if (senderOrTaget) //sender
                {
                    bLCustomerInParcel.Id = parcel.SenderId;
                    bLCustomerInParcel.Name = dal.getCustomerWithSpecificCondition(c => c.Id == parcel.SenderId).First().Name;
                }
                else //target
                {
                    bLCustomerInParcel.Id = parcel.TargetId;
                    bLCustomerInParcel.Name = dal.getCustomerWithSpecificCondition(c => c.Id == parcel.TargetId).First().Name;
                }
                parcelStatusesTemp = dal.findParcelStatus(parcel);
                parcelCustomers.Add(new ParcelAtCustomer() { Id = parcel.Id, Priority = parcel.Priority, Weight = parcel.Weight, SenderOrTargetCustomer = bLCustomerInParcel, ParcelStatus = parcelStatusesTemp });
            }
            return parcelCustomers;
        }

        /// <summary>
        /// Return a IEnumerable of DroneToList (BO object) from IEnumerable of BO.Drone.
        /// </summary>
        /// <param name="drones">The drones to convert</param>
        /// <returns></returns>
        private IEnumerable<DroneToList> convertDronesToDronesToList(IEnumerable<Drone> drones)
        {
            List<DroneToList> dronesToList = new List<DroneToList>();
            DroneToList toAdd = new DroneToList();
            foreach (Drone drone in drones)
            {
                if (drone.ParcelInTransfer == (null))
                    toAdd = new DroneToList() { Id = drone.Id, Model = drone.Model, MaxWeight = drone.MaxWeight, droneStatus = drone.Status, Battery = drone.Battery, DronePosition = drone.DronePosition };
                else
                    toAdd = new DroneToList() { Id = drone.Id, Model = drone.Model, MaxWeight = drone.MaxWeight, droneStatus = drone.Status, Battery = drone.Battery, DronePosition = drone.DronePosition, IdParcel = drone.ParcelInTransfer.Id };
                dronesToList.Add(toAdd);
            }
            return dronesToList;
        }

        /// <summary>
        /// convert list Parcels ToParcelsToList
        /// </summary>
        /// <param name="parcels">BO.Parcel</param>
        /// <returns></returns>
        private List<ParcelToList> convertBLParcelToBLParcelsToList(IEnumerable<Parcel> parcels)
        {
            List<ParcelToList> listParcels = new List<ParcelToList>();
            ParcelToList toAdd = new ParcelToList();
            foreach (Parcel parcel in parcels)
            {
                toAdd = new ParcelToList()
                {
                    Id = parcel.Id,
                    SenderName = parcel.Sender.Name,
                    TargetName = parcel.Target.Name,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                    ParcelStatus = findParcelStatus(parcel)
                };
                listParcels.Add(toAdd);
            }
            return listParcels;
        }
        private ParcelToList convertParcelToParcelToList(Parcel parcel)
        { 
            return new ParcelToList()
            {
                Id = parcel.Id,
                SenderName = parcel.Sender.Name,
                TargetName = parcel.Target.Name,
                Weight = parcel.Weight,
                Priority = parcel.Priority,
                ParcelStatus = findParcelStatus(parcel)
            };
        }

        /// <summary>
        /// Praivet Convert Dal To BL Parcel needed to be used by the simulation.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Parcel convertDalToBLParcelSimulation(DO.Parcel p)
        {
            return convertDalToBLParcel(p);
        }


        /// <summary>
        /// Convert DalToBL Parcel
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Parcel convertDalToBLParcel(DO.Parcel p) ///////////////////////////////
        {
            DroneInParcel drone = null;
            Parcel parcel = new Parcel();
            if (!p.Scheduled.Equals(default(DO.Parcel).Scheduled)) //if the parcel is paired with a drone
            {
                drone = createDroneInParcel(p, getDroneWithSpecificConditionFromDronesList(d => d.Id == (int)p.DroneId).First().Id);
            }
            return new Parcel()
            {
                Id = p.Id,
                Sender = new CustomerInParcel() { Id = p.SenderId, Name = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First().Name },
                Target = new CustomerInParcel() { Id = p.TargetId, Name = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First().Name },
                Weight = p.Weight,
                Drone = drone,
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered,
                Priority = p.Priority
            };
        }

        /// <summary>
        /// Convert DO.Customer CustomerInParcel
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private CustomerInParcel convertDalToBLCustomerInParcel(DO.Customer customer)
        {
            return new CustomerInParcel()
            {
                Id = customer.Id,
                Name = customer.Name
            };
        }

        /// <summary>
        /// get sender and target of a parcel and create ParcelInTransfer
        /// </summary>
        /// <param name="p">DO.parcel</param>
        /// <param name="sender">sender of parcel</param>
        /// <param name="target">target of parcel</param>
        /// <returns></returns>
        private ParcelInTransfer returnAParcelInTransfer(DO.Parcel p, DO.Customer sender, DO.Customer target)
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
                //parcelStatus = false,
                isWaiting = p.PickUp == null ? true : false,
                Priority = p.Priority,
                distance = distance(senderP, targetP),
                Weight = p.Weight
            };
        }

        
    }
}

//private ChargingDrone convertDalToBLChargingDrone(DO.DroneCharge droneCharge) //convertDalToBLChargingDrone the opposite BL to DAL
//{
//    return new ChargingDrone()
//    {
//        Id = droneCharge.DroneId,
//        Battery = getBLDroneWithSpecificCondition(d => d.Id == droneCharge.DroneId).First().Battery
//    };
//}
//=======================================================
//private ParcelInTransfer convertDalToParcelInTranspare(DO.Parcel parcel)
//{
//    return new ParcelInTransfer()
//    {
//        Id = parcel.Id,

//    };
//}
//private DO.Station convertBLToDalStation(Station s)
//{
//    return new DO.Station()
//    {
//        Id = s.Id,
//        Name = s.Name,
//        ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(),
//        Longitude = s.StationPosition.Longitude,
//        Latitude = s.StationPosition.Latitude
//    };
//}

//=======================================================
//=======================================================
//private Drone convertDalToBLDrone(DO.Drone d)//////////////////////////////////////////////////////////////////////////////
//{
//    Drone BLDrone = getBLDroneById(d.Id);
//    return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
//}
//private List<DroneToList> convertBLDroneToBLDronesToList(List<Drone> drones)
//{
//    List<DroneToList> listDrones = new List<DroneToList>();
//    DroneToList toAdd = new DroneToList();
//    foreach (Drone d in drones)
//    {
//        if (d.ParcelInTransfer == (null))
//            toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition };
//        else
//            toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition, IdParcel = d.ParcelInTransfer.Id };
//        listDrones.Add(toAdd);
//    }
//    return listDrones;
//}
//=======================================================
//=======================================================
        //private List<ParcelToList> convertBLParcelToBLParcelsToList()
        //{
        //    IEnumerable<DO.Parcel> parcels = dal.GetParcels();
        //    List<ParcelToList> listParcels = new List<ParcelToList>();
        //    ParcelToList toAdd = new ParcelToList();
        //    foreach (DO.Parcel p in parcels)
        //    {
        //        toAdd = new ParcelToList()
        //        {
        //            Id = p.Id,
        //            //SenderName = p.Sender.name,
        //            //TargetName = p.Target.name,
        //            SenderName = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First().Name,
        //            TargetName = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First().Name,
        //            Weight = p.Weight,
        //            Priority = p.Priority,
        //            ParcelStatus = findParcelStatus(p)
        //        };
        //        listParcels.Add(toAdd);
        //    }
        //    return listParcels;
        //}
//=======================================================
