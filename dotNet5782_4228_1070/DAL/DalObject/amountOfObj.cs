using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;




namespace DalObject
{
    public partial class DalObject : DO.DalApi
    {
        public int amountStations()
        {
            return DataSource.Stations.Count();
        }
        public int amountDrones()
        {
            return DataSource.Drones.Count();
        }
        public int amountCustomers()
        {
            return DataSource.Customers.Count;
        }
        public int amountDroneCharges()
        {
            return DataSource.DroneCharges.Count();
        }
        public int amountParcels()
        {
            return DataSource.Parcels.Count();
        }


    }
}
