
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
            Customer cToErase = getCustomerWithSpecificCondition( c => c.ID == customer.ID ).First();
            DataSource.Customers.Remove(cToErase);
            DataSource.Customers.Add(customer);
        }
    }
}
