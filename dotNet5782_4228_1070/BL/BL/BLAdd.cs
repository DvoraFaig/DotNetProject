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
        public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot)
        {
            if (dal.IsStationById(id))
            {
                throw new ObjExistException("station", id);
            }
            else
            {
                IDal.DO.Station s = new IDal.DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
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
                IDal.DO.WeightCategories maxWeightconvertToEnum = (IDal.DO.WeightCategories)maxWeight;
                Random r = new Random();
                int battery = r.Next(20, 40);
                IDal.DO.Station s = dal.getStationById(stationId);
                BLStation sBL = convertDalToBLStation(s);
                if (s.ChargeSlots - sBL.DronesCharging.Count > 0)
                {
                    BLPosition p = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
                    BLDrone dr = new BLDrone() { Id = id, Model = model, MaxWeight = maxWeightconvertToEnum, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
                    dronesInBL.Add(dr);
                    dal.AddDroneCharge(new IDal.DO.DroneCharge() { StationId = s.Id, DroneId = id });
                    dal.AddDrone(convertBLToDalDrone(dr));
                }
                else
                {
                    throw new Exception($"The charging slots of station: {stationId} is full.\nPlease enter a differant station.");
                }
            }
        }

        public void AddCustomer(int id, string name, string phone, BLPosition position)
        {
            if (dal.IsCustomerById(id))
            {
                throw new ObjExistException("customer", id);
            }
            else
            {
                IDal.DO.Customer c = new IDal.DO.Customer() { ID = id, Name = name, Phone = phone, Latitude = position.Latitude, Longitude = position.Longitude };
                dal.AddCustomer(c);
            }
        }

        public void AddParcel(int senderId, int targetId, int weight, int priority)
        {
            if (dal.IsCustomerById(senderId) && dal.IsCustomerById(targetId))
            {
                IDal.DO.Parcel p = new IDal.DO.Parcel()
                {
                    Id = dal.amountParcels() + 1,
                    SenderId = senderId,
                    TargetId = targetId,
                    Weight = (IDal.DO.WeightCategories)weight,
                    Priority = (IDal.DO.Priorities)priority,
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


