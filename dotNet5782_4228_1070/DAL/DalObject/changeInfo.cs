
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
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcel">remove current parcel</param>
        public void removeParcel(Parcel parcel)
        {
            DataSource.Parcels.Remove(parcel);
        }
       
        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            Station sToErase = getStationWithSpecificCondition(s => s.Id == stationWithUpdateInfo.Id).First();
            DataSource.Stations.Remove(sToErase);
            DataSource.Stations.Add(stationWithUpdateInfo);
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            DataSource.Drones.Remove(droneWithUpdateInfo);
            DataSource.Drones.Add(droneWithUpdateInfo);
        }

        /// <summary>
        ///  Change specific drone info
        /// </summary>
        /// <param name="goodParcel">The parcel with the changed info</param>
        public void changeParcelInfo(Parcel goodParcel)
        {
            Parcel pToErase = getParcelWithSpecificCondition(p => p.Id == goodParcel.Id).First();
            DataSource.Parcels.Remove(pToErase);
            DataSource.Parcels.Add(goodParcel);
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="goodCustomer">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer goodCustomer)
        {
            Customer cToErase = getCustomerWithSpecificCondition( c => c.Id == goodCustomer.Id ).First();
            DataSource.Customers.Remove(cToErase);
            DataSource.Customers.Add(goodCustomer);
        }
    }
}
