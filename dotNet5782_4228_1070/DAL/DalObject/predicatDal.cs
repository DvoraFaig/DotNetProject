using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;


namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        //public Predicate<T> GetObjByIdP<T>(Predicate<T> findBy)
        //{
        //    return findBy;
        //}
        ////public Predicate<T> GetObjByIdP<T>(Predicate<T> findBy)
        ////{
        ////    findBy(DataSource.Parcels)       
        ////}
        public Parcel GetAllMatchedEntities(Func<Parcel, Boolean> predicate)
        {
            return DataSource.Parcels.Where<Parcel>(predicate).First();

        }
        public T GetAllMatchedEntities<T>(Func<T, Boolean> predicate)
        {
            return DataSource.T.Where<Station>(predicate).First();

        }
        public Drone getDroneById(int id)
        {
            return (from drone in DataSource.Drones
                    where id == drone.Id
                    select drone).First();
        }
        public Parcel getParcelById(int id)
        {
            return (from parcel in DataSource.Parcels
                    where id == parcel.Id
                    select parcel).First();
        }
        public Customer getCustomerById(int id)
        {
            return (from Customer in DataSource.Customers
                    where id == Customer.ID
                    select Customer).First();
        }
        public Station getStationById(int id)
        {
            return (from station in DataSource.Stations
                    where id == station.Id
                    select station).First();
        } 
        public DroneCharge getDroneChargeByDroneId(int id)
        {
            return (from DroneCharge in DataSource.DroneCharges
                    where id == DroneCharge.DroneId
                    select DroneCharge).First();
        }
        public DroneCharge getDroneChargeByStationId(int id)
        {
            return (from DroneCharge in DataSource.DroneCharges
                    where id == DroneCharge.StationId
                    select DroneCharge).First();
        }
    }
}
