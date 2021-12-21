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
        public IEnumerable<Station> displayStations()
        {
            return from s in DataSource.Stations select s;
        }
        public IEnumerable<Drone> displayDrone()
        {
            return from d in DataSource.Drones select d;
        }
        public IEnumerable<Parcel> displayParcels()
        {
            return from p in DataSource.Parcels select p;
        }
        public IEnumerable<Customer> displayCustomers()
        {
            return from c in DataSource.Customers select c;
        }
        public IEnumerable<DroneCharge> displayDroneCharge()
        {
            return from d in DataSource.DroneCharges select d;
        }
    }
}
