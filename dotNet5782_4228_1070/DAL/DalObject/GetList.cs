using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;




namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations()
        {
            return from s in DataSource.Stations
                   where s.IsActive == true
                   select s;
        }

        /// <summary>
        /// Get all drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            return from d in DataSource.Drones 
                   where d.IsActive == true
                   select d;
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels()
        {
            return from p in DataSource.Parcels select p;
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return from c in DataSource.Customers
                   where c.IsActive == true
                   select c;
        }

        /// <summary>
        /// Get all droneCharge.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneCharge()
        {
            return from d in DataSource.DroneCharges select d;
        }
    }
}
