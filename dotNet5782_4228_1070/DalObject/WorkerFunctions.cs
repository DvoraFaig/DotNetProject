using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Get a Worker/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a worker/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate)
        {
            return (from worker in DataSource.Workers
                    where predicate(worker)
                    select worker);
        }

    }
}

#region extra
//public Predicate<T> GetObjByIdP<T>(Predicate<T> findBy)
//{
//    return findBy;
//}
////public Predicate<T> GetObjByIdP<T>(Predicate<T> findBy)
////{
////    findBy(DataSource.Parcels)       
////}
//public IEnumerable<Parcel> getParcelWithSpecificCondition(Func<Parcel, Boolean> predicate)
//{
//    return DataSource.Parcels.Where<Parcel>(predicate);
//}
////public IEnumerable<Parcel> getParcelWithSpecificCondition(Func<Parcel, Boolean> predicate)
////{
////    return DataSource.Parcels.Where<Parcel>(predicate);
////}
////public IEnumerable<Drone> getDroneWithSpecificCondition(Func<Drone, Boolean> predicate)
////{
////    return DataSource.Drones.Where<Drone>(predicate);
////}
////public IEnumerable<Station> getStationWithSpecificCondition(Func<Station, Boolean> predicate)
////{
////    return DataSource.Stations.Where<Station>(predicate);
////}
////public IEnumerable<Customer> getCustomerWithSpecificCondition(Func<Customer, Boolean> predicate)
////{
////    return DataSource.Customers.Where<Customer>(predicate);
////}
////public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Func<DroneCharge, Boolean> predicate)
////{
////    return DataSource.DroneCharges.Where<DroneCharge>(predicate);
////}
////public DroneCharge getDroneChargeByStationId(Predicate<DroneCharge> predicate)
////{
////    return (from DroneCharge in DataSource.DroneCharges
////            where predicate(DroneCharge)
////            select DroneCharge).First();
////}
////

//public Drone getDroneById(Predicate<Drone> predicate)
//{
//    return (from drone in DataSource.Drones
//            where predicate(drone)
//            select drone).First();
//}
//public Parcel getParcelById(Predicate<Parcel> predicate)
//{
//    return (from parcel in DataSource.Parcels
//            where predicate(parcel)
//            select parcel).First();
//}
//public Customer getCustomerById(Predicate<Customer> predicate)
//{
//    return (from Customer in DataSource.Customers
//            where predicate(Customer)
//            select Customer).First();
//}
//public Station getStationById(Predicate<Station> predicate)
//{
//    return (from station in DataSource.Stations
//            where predicate(station)
//            select station).First();
//}
//public DroneCharge getDroneChargeByDroneId(Predicate<DroneCharge> predicate)
//{
//    return (from DroneCharge in DataSource.DroneCharges
//            where predicate(DroneCharge)
//            select DroneCharge).First();
//}

#endregion