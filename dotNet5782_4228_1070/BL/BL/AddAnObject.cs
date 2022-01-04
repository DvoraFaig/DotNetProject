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
        /// Add a new station. checks if this station exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// </summary>
        /// <param name="stationToAdd">The new station to add.</param>
        public void AddStation(Station stationToAdd)
        {
            if (dal.IsStationById(stationToAdd.Id))
            {
                throw new ObjExistException(typeof(BO.Station), stationToAdd.Id);
            }
            else
            {
                DO.Station s = new DO.Station() { Id = stationToAdd.Id, Name = stationToAdd.Name, Longitude = stationToAdd.StationPosition.Longitude, Latitude = stationToAdd.StationPosition.Latitude, ChargeSlots = stationToAdd.DroneChargeAvailble };
                dal.AddStation(s);
            }
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
        public void AddDrone(int id, string model, int maxWeight, int stationId)
        {
            if (dal.IsDroneById(id))
            {
                throw new ObjExistException(typeof(BO.Drone), id);
            }
            else
            {
                if (maxWeight > 3 || maxWeight < 1)
                    throw new Exception("Weight of drone is out of range");
                DO.WeightCategories maxWeightconvertToEnum = (DO.WeightCategories)maxWeight;
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
                    Drone dr = new Drone() { Id = id, Model = model, MaxWeight = maxWeightconvertToEnum, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
                    droensList.Add(dr);
                    dal.AddDroneToCharge(new DO.DroneCharge() { StationId = s.Id, DroneId = id });
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
        /// <param name="newCustomer">The new customer to add.</param>
        public void AddCustomer(BO.Customer newCustomer)
        {

            if (dal.IsCustomerById(newCustomer.Id))
            {
                throw new ObjExistException(typeof(BO.Customer), newCustomer.Id);
                //throw new ObjExistException("customer", customer.Id);
            }
            else
            {
                DO.Customer customerToAdd = convertBLToDalCustomer(newCustomer);
                dal.AddCustomer(customerToAdd);
            }

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
        public void AddParcel(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority)
        {
            if (dal.IsCustomerById(senderId) && dal.IsCustomerById(targetId))
            {
                DO.Parcel p = new DO.Parcel()
                {
                    Id = dal.amountParcels() + 1,
                    SenderId = senderId,
                    TargetId = targetId,
                    Weight = weight,
                    Priority = priority,
                    Requeasted = DateTime.Now
                };
                dal.AddParcel(p);
            }
            else
            {
                throw new ObjNotExistException($"sender customer {senderId} or terget customer {targetId} not exsist");
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