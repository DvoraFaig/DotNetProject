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
        /// If station with the same id exist
        /// </summary>
        /// <param name="id">Looking for station with this id</param>
        /// <returns></returns>
        public Boolean IsStationById(int id)
        {
            return DataSource.Stations.Any(s => s.Id == id);
        }

        /// <summary>
        /// If drone with the same id exist
        /// </summary>
        /// <param name="id">Looking for drone with this id</param>
        /// <returns></returns>
        public Boolean IsDroneById(int id)
        {
            return DataSource.Drones.Any(d => d.Id == id);
        }

        /// <summary>
        /// If customer with the same id exist
        /// </summary>
        /// <param name="id">Looking for customer with this id</param>
        /// <returns></returns>
        public Boolean IsCustomerById(int id)
        {
            return DataSource.Customers.Any(c => c.Id == id);
        }

        /// <summary>
        /// If parcel with the same id exist
        /// </summary>
        /// <param name="id">Looking for parcel with this id</param>
        /// <returns></returns>
        public Boolean IsParcelById(int id)
        {
            return DataSource.Parcels.Any(p => p.Id == id);
        }
    }
}
