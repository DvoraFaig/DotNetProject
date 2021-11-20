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
            //!dal.getStationById(id).Equals(default(IDal.DO.Station))) 
            try
            {
                dal.getStationById(id);
                throw new ObjExistException("station", id);
            }
            catch (IDal.DO.DalExceptions.ObjNotExistException e) {
                IDal.DO.Station s = new IDal.DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
                dal.AddStation(s);
            }
        }

        public void AddDrone(int id, string model, int maxWeight, int stationId)
        {  
            if (!dal.getDroneById(id).Equals(default(IDal.DO.Drone).Id))
            {
                throw new ObjExistException("drone", id);
            }
            IDal.DO.WeightCategories maxWeightconvertToEnum = (IDal.DO.WeightCategories)maxWeight;
            Random r = new Random();
            int battery = r.Next(20, 40);
            IDal.DO.Station s = dal.getStationById(stationId);
            BLStation sBL = convertDalToBLStation(s);
            if (s.ChargeSlots - sBL.DronesCharging.Count > 0) { 
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

        public void AddCustomer(int id, string name, string phone, BLPosition position)
        {
            if (!dal.getCustomerById(id).Equals(default(IDal.DO.Customer).ID))
            {
                throw new ObjExistException("customer", id);
            }
            IDal.DO.Customer c = new IDal.DO.Customer() { ID = id, Name = name, Phone = phone, Latitude = position.Latitude, Longitude = position.Longitude };
           
            dal.AddCustomer(c);
        }

        public void AddParcel(int senderId, int targetId, int weight, int priority)
        {
            IDal.DO.Customer dalCustomer;
            IDal.DO.Parcel p = new IDal.DO.Parcel();
            p.Id = dal.amountParcels() + 1;
            try
            {
                dalCustomer = dal.getCustomerById(senderId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            try
            {
                dalCustomer = dal.getCustomerById(targetId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            p.SenderId = senderId;
            p.TargetId = targetId;
            p.Weight = (IDal.DO.WeightCategories)weight;
            p.Priority = (IDal.DO.Priorities)priority;
            p.Requeasted = DateTime.Now;
            p.Scheduled = new DateTime(0, 0, 0);
            p.PickUp = new DateTime(0, 0, 0);
            p.Delivered = new DateTime(0, 0, 0);
            dal.AddParcel(p);
        }
    }
}


