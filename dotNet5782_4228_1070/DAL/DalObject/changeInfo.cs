
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
        public void removeParcel(Parcel parcel)
        {
            DataSource.Parcels.Remove(parcel);
        }
        public void removeStation(Station station)
        {
            DataSource.Stations.Remove(station);
        }
        public void removeCustomer(Customer customer)
        {
            DataSource.Customers.Remove(customer);
        }
        public void removeDrone(Drone drone)
        {
            DataSource.Drones.Remove(drone);
        }
        public void removeDroneCharge(DroneCharge droneCharge)
        {
            DataSource.DroneCharges.Remove(droneCharge);
        }
        public void changeStationInfo(Station goodStation)
        {
            Station sToErase = getStationWithSpecificCondition(s => s.Id == goodStation.Id).First();
            DataSource.Stations.Remove(sToErase);
            DataSource.Stations.Add(goodStation);
        }
        public void changeDroneInfo(Drone d)
        {
            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(d);
        }
        public void changeDroneInfo(int id, string newModel)
        {
            Drone dToChange = getDroneWithSpecificCondition(d => d.Id == id).First();
            DataSource.Drones.Remove(dToChange);
            dToChange.Model = newModel;
            DataSource.Drones.Add(dToChange);
        }
        public void changeParcelInfo(Parcel goodParcel)
        {
            Parcel pToErase = getParcelWithSpecificCondition(p => p.Id == goodParcel.Id).First();
            DataSource.Parcels.Remove(pToErase);
            DataSource.Parcels.Add(goodParcel);
        }
        public void changeCustomerInfo(Customer customer)
        {
            Customer cToErase = getCustomerWithSpecificCondition( c => c.Id == customer.Id ).First();
            DataSource.Customers.Remove(cToErase);
            DataSource.Customers.Add(customer);
        }
    }
}
