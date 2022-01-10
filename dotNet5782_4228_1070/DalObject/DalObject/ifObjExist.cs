using System;
using System.Linq;
using DalApi;
using DO;
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
        /// If station with the requested id exist and active.
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        public Boolean IsStationActive(int requestedId)
        {
            return DataSource.Stations.Any(s => s.Id == requestedId && s.IsActive == true);
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
        /// If drone with the requested id exist and active
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        public Boolean IsDroneActive(int requestedId)
        {
            return DataSource.Drones.Any(d => d.Id == requestedId && d.IsActive);
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
        /// If customer with the requested id exist and Active
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns></returns>
        public Boolean IsCustomerActive(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId && c.IsActive == true);
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
