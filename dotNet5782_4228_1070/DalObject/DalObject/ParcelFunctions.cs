using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Add the new parcel to parcels.
        /// </summary>
        /// <param name="newParcel">parcel to add</param>
        public void AddParcel(Parcel newParcel)
        {
            DataSource.Parcels.Add(newParcel);
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels()
        {
            return from p in DataSource.Parcels select p;
        }

        /// <summary>
        /// Get a Parcel/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a parcel/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
        {
            return (from parcel in DataSource.Parcels
                    where predicate(parcel)
                    select parcel);
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
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcel">remove current parcel</param>
        public void removeParcel(Parcel parcelToRemove)
        {
            try
            {
                Parcel parcel = (from p in DataSource.Parcels
                               where p.Id == parcelToRemove.Id
                              && p.SenderId == parcelToRemove.SenderId
                              && p.TargetId == parcelToRemove.TargetId
                              && p.Scheduled == null
                               select p).First();
                parcel.IsActive = false;
                changeParcelInfo(parcel);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Parcel), parcelToRemove.Id, e1);
            }
            //DataSource.Parcels.Remove(parcel);
        }

        /// <summary>
        /// Return how much parcels there is.
        /// </summary>
        /// <returns></returns>
        public int amountParcels()
        {
            return DataSource.Parcels.Count();
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
