
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
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
            Drone dToChange = getDroneById(id);
            DataSource.Drones.Remove(dToChange);
            dToChange.Model = newModel;
            DataSource.Drones.Add(dToChange);
        }
        public void changeParcelInfo(Parcel goodParcel)
        {
            Parcel pToErase = getParcelById(goodParcel.Id);
            DataSource.Parcels.Remove(pToErase);
            DataSource.Parcels.Add(goodParcel);
        }
        public void changeCustomerInfo(Customer c)
        {
            Customer cToErase = getCustomerById(c.ID);
            DataSource.Customers.Remove(cToErase);
            DataSource.Customers.Add(c);
        }
    }
}
