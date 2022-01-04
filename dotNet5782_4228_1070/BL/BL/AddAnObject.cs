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
        private DO.Station convertBLToDalStation(Station s)
        {
            return new DO.Station()
            {
                Id = s.Id,
                Name = s.Name,
                Longitude = s.StationPosition.Longitude,
                Latitude = s.StationPosition.Latitude,
                ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(),
                IsActive = true //////////////
            };
        }
        /// <summary>
        /// Add a new station. checks if this station exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// </summary>
        /// <param name="stationToAdd">The new station to add.</param>
        public void AddStation(Station stationToAdd)
        {
            DO.Station station;
            try
            {
                station = dal.getStationWithSpecificCondition(s => s.Id == stationToAdd.Id).First();
                if (station.IsActive)
                    throw new ObjExistException(typeof(BO.Station), stationToAdd.Id);
                station.IsActive = true;
                dal.changeStationInfo(station); //convertBLToDalStation(stationToAdd)
                string message = "";
                if (stationToAdd.StationPosition.Latitude != station.Latitude || stationToAdd.StationPosition.Longitude != station.Longitude)
                    message += "Position";
                if(stationToAdd.DronesCharging.Count() + stationToAdd.DroneChargeAvailble != station.ChargeSlots )
                    message += ", Amount charge slots";
                if(message != "")
                    throw new Exceptions.DataOfOjectChanged(typeof(Station), station.Id, $"Data: {message} was changed");
                return;
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            dal.AddStation(convertBLToDalStation(stationToAdd));
        }

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
        public void AddDrone(Drone droneToAdd, int stationId)
        {
            if (dal.IsDroneById(droneToAdd.Id))
            {
                throw new ObjExistException(typeof(BO.Drone), droneToAdd.Id);
            }
            else
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
                catch (InvalidOperationException)
                {
                    throw new ObjNotExistException(typeof(DO.Station), stationId);
                }
                Station sBL = convertDalToBLStation(s);
                if (s.ChargeSlots - sBL.DronesCharging.Count > 0)
                {
                    Position p = new Position() { Longitude = s.Longitude, Latitude = s.Latitude };
                    Drone dr = new Drone() { Id = droneToAdd.Id, Model = droneToAdd.Model, MaxWeight = maxWeightconvertToEnum, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
                    droensList.Add(dr);
                    dal.AddDroneToCharge(new DO.DroneCharge() { StationId = s.Id, DroneId = droneToAdd.Id });
                    dal.AddDrone(convertBLToDalDrone(dr));
                }
                else
                {
                    throw new Exception($"The charging slots of station: {stationId} is full.\nPlease enter a differant station.");
                }
            }
        }

        /// <summary>
        /// Add a new customer. checks if this customer exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// </summary>
        /// <param name="customerToAdd">The new customer to add.</param>
        public void AddCustomer(BO.Customer customerToAdd)
        {
            try
            {
                DO.Customer customer = dal.getCustomerWithSpecificCondition(c => c.Id == customerToAdd.Id).First();
                if(customer.IsActive)
                    throw new ObjExistException(typeof(BO.Customer), customer.Id);
                customer.IsActive = true;
                dal.changeCustomerInfo(customer);
                string message = "";
                if (customerToAdd.CustomerPosition.Latitude != customer.Latitude || customerToAdd.CustomerPosition.Longitude != customer.Longitude)
                    message = "Position";
                if (customerToAdd.Name != customer.Name)
                    message += ", Name";
                if (customerToAdd.Phone != customer.Phone)
                    message += "and Phone";
                if(message != "")
                    throw new Exceptions.DataOfOjectChanged(typeof(Customer), customer.Id, $"Data: {message} was changed");
                return;
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            dal.AddCustomer(convertBLToDalCustomer(customerToAdd));
        }

        /// <summary>
        /// Add a new parcel. checks if this parcel exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// Didn't sent an object because most of the props are initialize in the function and not by the workers/customer(client).
        /// </summary>
        /// <param name="senderId">Parcels' sender(customer) id</param>
        /// <param name="targetId">Parcels' target(customer) id</param>
        /// <param name="weight">Parcels' weight</param>
        /// <param name="priority">Parcels' priority</param>
        public void AddParcel(Parcel parcelToAdd)
        {
            if (dal.IsCustomerById(parcelToAdd.Sender.Id) && dal.IsCustomerById(parcelToAdd.Target.Id))
            {
                DO.Parcel p = new DO.Parcel()
                {
                    Id = dal.amountParcels() + 1,
                    SenderId = parcelToAdd.Sender.Id,
                    TargetId = parcelToAdd.Target.Id,
                    Weight = parcelToAdd.Weight,
                    Priority = parcelToAdd.Priority,
                    Requeasted = DateTime.Now
                };
                dal.AddParcel(p);
            }
            else
            {
                throw new ObjNotExistException($"sender customer {parcelToAdd.Sender.Id} or terget customer {parcelToAdd.Target.Id} not exsist");
            }
        }
    }
}


//public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot)
//{
//    if (dal.IsStationById(id))
//    {
//        throw new ObjExistException("station", id);
//    }
//    else
//    {
//        DO.Station s = new DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
//        dal.AddStation(s);
//    }
//}

//public void AddCustomer(int id, string name, string phone, int latitude, int longitude)
//{
//    if (dal.IsCustomerById(id))
//    {
//        throw new ObjExistException(typeof(BO.Customer), id);
//    }
//    else
//    {
//        DO.Customer c = new DO.Customer() { Id = id, Name = name, Phone = phone, Latitude = latitude, Longitude = longitude };
//        dal.AddCustomer(c);
//    }
//}