using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        private BLDrone getBLDroneById(int id)
        {
            try
            {
                return getBLDroneWithSpecificCondition(d => d.Id == id).First() ;
            }
            catch (InvalidOperationException )
            {
                throw new ObjNotExistException(typeof(BLDrone), id);
            }
        }
        public IEnumerable<BLDrone> getBLDroneWithSpecificCondition(Predicate<BLDrone> predicate)
        {
            return (from drone in dronesInBL
                    where predicate(drone)
                    select drone);
        }

        public BLStation GetStationById(int id)
        {
            IDal.DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == id).First();
            BLStation BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        public BLCustomer GetCustomerById(int id)
        {
            IDal.DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.ID == id).First();
            BLCustomer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        public BLDrone GetDroneById(int id)
        {
            IDal.DO.Drone d = dal.getDroneWithSpecificCondition(d => d.Id == id).First();
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }

        public BLParcel GetParcelById(int id)
        {
            IDal.DO.Parcel p = dal.getParcelWithSpecificCondition(p => p.Id == id).First();
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
    }
}
