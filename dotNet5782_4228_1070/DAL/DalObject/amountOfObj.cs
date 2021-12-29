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
        /// Return how much stations there is.
        /// </summary>
        /// <returns></returns>
        public int amountStations()
        {
            return DataSource.Stations.Count();
        }
        
        /// <summary>
        /// Return how much parcels there is.
        /// </summary>
        /// <returns></returns>
        public int amountParcels()
        {
            return DataSource.Parcels.Count();
        }
    }
}

//public int amountDrones()
//{
//    return DataSource.Drones.Count();
//}
//public int amountCustomers()
//{
//    return DataSource.Customers.Count;
//}
//public int amountDroneCharges()
//{
//    return DataSource.DroneCharges.Count();
//}