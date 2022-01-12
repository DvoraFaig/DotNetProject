
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

using DalApi;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        
        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            int index = DataSource.Stations.FindIndex(s => s.Id == stationWithUpdateInfo.Id);
            DataSource.Stations[index] = stationWithUpdateInfo;
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            int index = DataSource.Drones.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
            DataSource.Drones[index] = droneWithUpdateInfo;
        }

        /// <summary>
        ///  Change specific parcel info
        /// </summary>
        /// <param name="parcelWithUpdateInfo">The parcel with the changed info</param>
        public void changeParcelInfo(Parcel parcelWithUpdateInfo)
        {
            int index = DataSource.Parcels.FindIndex(d => d.Id == parcelWithUpdateInfo.Id);
            DataSource.Parcels[index] = parcelWithUpdateInfo;
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer customerWithUpdateInfo)
        {
            int index = DataSource.Customers.FindIndex(d => d.Id == customerWithUpdateInfo.Id);
            DataSource.Customers[index] = customerWithUpdateInfo;
        }

        /// <summary>
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcel">remove current parcel</param>
        public void removeParcel(Parcel parcel)
        {
            DataSource.Parcels.Remove(parcel);
        }

    }
}
