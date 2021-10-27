using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace DalObject
{
    public partial class DalObject : IBL.Ibl
    {
        //Parcel
        public static int amountParcels()
        {
            return DataSource.Parcels.Count();
        }
        public void AddParcelToDelivery(int id, int Serderid, int TargetId, WeightCategories Weight, Priorities Priority, DateTime requestedTime)
        {
            Parcel p = new Parcel(id, Serderid, TargetId, Weight, Priority, requestedTime);
            DataSource.Parcels.Add(p);
        }
        public IEnumerable<Parcel> displayParcels()
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id != 0)
                {
                    yield return parcel;
                }
            }
        }
        public IEnumerable<Parcel> displayFreeParcels()
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id != 0 && parcel.DroneId == -1)
                {
                    yield return parcel;
                }
            }
        }
        public static Parcel getParcelById(int id)
        {
            return DataSource.Parcels.FirstOrDefault(parcel => parcel.Id == id);
        }
    }
}
