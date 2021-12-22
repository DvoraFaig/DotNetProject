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
        Random r = new Random();
        public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot)
        {
            if (dal.IsStationById(id))
            {
                throw new ObjExistException("station", id);
            }
            else
            {
                DO.Station s = new DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
                dal.AddStation(s);
            }
        }

        public void AddDrone(int id, string model, int maxWeight, int stationId)
        {
            if (dal.IsDroneById(id))
            {
                throw new ObjExistException("drone", id);
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

        public void AddCustomer(int id, string name, string phone, Position position)
        {
            if (dal.IsCustomerById(id))
            {
                throw new ObjExistException("customer", id);
            }
            else
            {
                DO.Customer c = new DO.Customer() { Id = id, Name = name, Phone = phone, Latitude = position.Latitude, Longitude = position.Longitude };
                dal.AddCustomer(c);
            }
        }

        public void AddParcel(int senderId, int targetId, int weight, int priority)
        {
            if (dal.IsCustomerById(senderId) && dal.IsCustomerById(targetId))
            {
                DO.Parcel p = new DO.Parcel()
                {
                    Id = dal.amountParcels() + 1,
                    SenderId = senderId,
                    TargetId = targetId,
                    Weight = (DO.WeightCategories)weight,
                    Priority = (DO.Priorities)priority,
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


