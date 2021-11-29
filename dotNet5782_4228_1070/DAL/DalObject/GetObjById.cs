using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public Station getStationById(int id)
        {
            try
            {
                //return DataSource.Stations.First(s => s.Id == id);
                return getStationWithSpecificCondition(s => s.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Station), id);
            }
        }
        public Drone getDroneById(int id)
        {
            try
            {
                //return DataSource.Drones.First(drone => drone.Id == id);
                return getDroneWithSpecificCondition(drone => drone.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Drone), id);
            }
        }
        public Parcel getParcelById(int id)
        {
            try
            {
                //return DataSource.Parcels.First(parcel => parcel.Id == id);
                return getParcelWithSpecificCondition(parcel => parcel.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Parcel), id);
            }
        }
        public Parcel getParcelByDroneId(int droneId)
        {
            try
            {
                //return DataSource.Parcels.First(p => p.DroneId == droneId);
                return getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Parcel), droneId);
            }
        }
        public Customer getCustomerById(int id)
        {
            try
            {
                //return DataSource.Customers.First(customer => customer.ID == id);
                return getCustomerWithSpecificCondition(customer => customer.ID == id).First();
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Customer), id);
            }
        }
        public DroneCharge getDroneChargeByDroneId(int id)
        {
            try { 
            //return DataSource.DroneCharges.FirstOrDefault(charge => charge.DroneId == id);
            return getDroneChargeWithSpecificCondition(charge => charge.DroneId == id).First();

        }
        public DroneCharge getDroneChargeByStationId(int id)
        {
            try
            {
                //return DataSource.DroneCharges.Find(charge => charge.StationId == id);
                return getDroneChargeWithSpecificCondition(charge => charge.StationId == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(DroneCharge), id);
            }
        }

    }
}
