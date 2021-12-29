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

        /// <summary>
        /// Add a new station. checks if this station exist already.
        /// </summary>
        /// <param name="stationToAdd">The new station to add.</param>
        public void AddStation(Station stationToAdd)
        {
            if (dal.IsStationById(stationToAdd.Id))
            {
                throw new ObjExistException( typeof(BO.Station), stationToAdd.Id);
            }
            else
            {
                DO.Station s = new DO.Station() { Id = stationToAdd.Id, Name = stationToAdd.Name, Longitude = stationToAdd.StationPosition.Longitude, Latitude = stationToAdd.StationPosition.Latitude, ChargeSlots = stationToAdd.DroneChargeAvailble};
                dal.AddStation(s);
            }
        }

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
                    dronesInBL.Add(dr);
                    dal.AddDroneCharge(new DO.DroneCharge() { StationId = s.Id, DroneId = id });
                    dal.AddDrone(convertBLToDalDrone(dr));
                }
                else
                {
                    throw new Exception($"The charging slots of station: {stationId} is full.\nPlease enter a differant station.");
                }
            }
        }
        public void AddCustomer(int id, string name, string phone, int latitude , int longitude)
        {
            if (dal.IsCustomerById(id))
            {
                throw new ObjExistException(typeof(BO.Customer), id);
            }
            else
            {
                DO.Customer c = new DO.Customer() { Id = id, Name = name, Phone = phone, Latitude = latitude, Longitude = longitude };
                dal.AddCustomer(c);
            }
        }
        public void AddCustomer(BO.Customer customer)
        {
            if (dal.IsCustomerById(customer.Id))
            {
                throw new ObjExistException(typeof(BO.Customer), customer.Id);
                //throw new ObjExistException("customer", customer.Id);
            }
            else
            {
                DO.Customer customerToAdd = convertBLToDalCustomer(customer);
                dal.AddCustomer(customerToAdd);
            }

        }

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


