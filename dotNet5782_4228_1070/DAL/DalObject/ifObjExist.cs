using System;
using System.Linq;
//check if needed to throw ArgumentNullException = the list is empty

namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// If station with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        public Boolean IsStationById(int requestedId)
        {
            return DataSource.Stations.Any(s => s.Id == requestedId);
        }

        /// <summary>
        /// If drone with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        public Boolean IsDroneById(int requestedId)
        {
            return DataSource.Drones.Any(d => d.Id == requestedId);
        }

        /// <summary>
        /// If customer with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for customer with this id</param>
        /// <returns></returns>
        public Boolean IsCustomerById(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId);
        }

        /// <summary>
        /// If parcel with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for parcel with this id</param>
        /// <returns></returns>
        public Boolean IsParcelById(int requestedId)
        {
            return DataSource.Parcels.Any(p => p.Id == requestedId);
        }
    }
}
