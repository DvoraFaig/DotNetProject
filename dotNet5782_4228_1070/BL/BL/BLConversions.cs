using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL
    {
        private IDal.DO.Station convertBLToDalStation(BLStation s)
        {
            return new IDal.DO.Station() { 
                Id = s.ID, 
                Name = s.Name, 
                ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(), 
                Longitude = s.StationPosition.Longitude, 
                Latitude = s.StationPosition.Latitude };
        }

        private IDal.DO.Drone convertBLToDalDrone(BLDrone d)
        {
            return new IDal.DO.Drone() { 
                Id = d.Id, Model = d.Model, 
                MaxWeight = d.MaxWeight
            };
        }

        private IDal.DO.Customer convertBLToDalCustomer(BLCustomer c)
        {
            return new IDal.DO.Customer() { 
                ID = c.ID, 
                Name = c.Name, 
                Phone = c.Phone, 
                Longitude = c.CustomerPosition.Longitude, 
                Latitude = c.CustomerPosition.Latitude, };
        }

        private IDal.DO.Parcel convertBLToDalParcel(BLParcel p)
        {
            return new IDal.DO.Parcel() { 
                Id = p.Id, 
                SenderId = p.Sender.Id, 
                TargetId = p.Target.Id, 
                Weight = p.Weight, 
                Priority = p.Priority, 
                DroneId = p.Drone.Id, 
                Requeasted = p.Requeasted, 
                Scheduled = p.Scheduled, 
                PickUp = p.PickUp, 
                Delivered = p.Delivered };
        }

        private static BLStation convertDalToBLStation(IDal.DO.Station s)
        {
            List<BLChargingDrone> blDroneChargingByStation = new List<BLChargingDrone>();
            List<IDal.DO.DroneCharge> droneChargesByStation = dal.displayDroneCharge().ToList();
            droneChargesByStation = droneChargesByStation.FindAll(d => d.StationId == s.Id);
            droneChargesByStation.ForEach(d =>
            {
                blDroneChargingByStation.Add(new BLChargingDrone() { Id = d.DroneId, Battery = GetBLDroneById(d.DroneId).Battery });
            });
            return new BLStation() { ID = s.Id, Name = s.Name, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude }, DroneChargeAvailble = s.ChargeSlots, DronesCharging = blDroneChargingByStation };
        }

        private static BLCustomer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            List<IDal.DO.Parcel> sendingParcels = parcels.FindAll(p => p.SenderId == c.ID);
            List<IDal.DO.Parcel> targetParcels = parcels.FindAll(p => p.TargetId == c.ID);
            List<BLParcelAtCustomer> customerAsSender = new List<BLParcelAtCustomer>();
            List<BLParcelAtCustomer> customerAsTarget = new List<BLParcelAtCustomer>();
            sendingParcels.ForEach(p => customerAsSender.Add(createtDalParcelToBLParcelAtCustomer(p, c)));
            targetParcels.ForEach(p => customerAsTarget.Add(createtDalParcelToBLParcelAtCustomer(p, c)));
            return new BLCustomer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsSender };
        }

        private static BLDrone convertDalToBLDrone(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            BLDrone BLDrone = GetBLDroneById(d.Id);// = dronesInBL.Find(e => e.Id == d.Id);
            //try
            //{
            //    return GetBLDroneById(d.Id); //if there is no such BLdrone -> there is an error becuase the obj is in DAL Obj
            //}
            //catch
            //{
            //    throw new NoDataMatchingBetweenDalandBL<IDal.DO.Drone>(d);
            //}
            Random r = new Random();
            
            return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
        }

        private static BLChargingDrone convertDalToBLChargingDrone(IDal.DO.DroneCharge d) //convertDalToBLChargingDrone the opposite BL to DAL
        {
            return new BLChargingDrone() { 
                Id = d.DroneId, 
                Battery = GetBLDroneById(d.DroneId).Battery };
        }

        private static BLParcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            BLDroneInParcel drone = null;
            if (!p.Scheduled.Equals(null)) //if the parcel is paired with a drone
            {
                drone = createBLDroneInParcel(p, GetBLDroneById(p.DroneId).Id);
            }

            return new BLParcel()
            {
                Id = p.Id,
                Sender = new BLCustomerInParcel() { Id = p.SenderId, name = dal.getCustomerById(p.SenderId).Name },
                Target = new BLCustomerInParcel() { Id = p.TargetId, name = dal.getCustomerById(p.TargetId).Name },
                Weight = p.Weight,
                Drone = createBLDroneInParcel(p, p.DroneId),
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
        }
    }
}
