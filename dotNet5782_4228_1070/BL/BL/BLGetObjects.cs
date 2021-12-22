using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Exceptions;

namespace BL
{
using BO;
    public sealed partial class BL : BlApi.Ibl
    {
        private Drone getBLDroneById(int id)
        {
            try
            {
                return getBLDroneWithSpecificCondition(d => d.Id == id).First() ;
            }
            catch (InvalidOperationException )
            {
                throw new ObjNotExistException(typeof(Drone), id);
            }
        }
        private IEnumerable<Drone> getBLDroneWithSpecificCondition(Predicate<Drone> predicate)
        {
            return (from drone in dronesInBL
                    where predicate(drone)
                    select drone);
        }

        private IEnumerable<Parcel> getBLParcelWithSpecificCondition(Predicate<Parcel> predicate)
        {
            List<Parcel> parcels = DisplayParcel();
            return (from parcel in parcels
                    where predicate(parcel)
                    select parcel);
        }
        public Station GetStationById(int id)
        {
            DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == id).First();
            Station BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        public Customer GetCustomerById(int id)
        {
            DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == id).First();
            Customer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        public Drone GetDroneById(int id)
        {
            return getBLDroneWithSpecificCondition(d => d.Id == id).First();
        }

        public Parcel GetParcelById(int id)
        {
            DO.Parcel p = dal.getParcelWithSpecificCondition(p => p.Id == id).First();
            Parcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
    }
}
