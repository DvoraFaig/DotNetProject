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
                return dronesInBL.First(d => d.Id == id);
            }
            catch (InvalidOperationException )
            {
                throw new ObjNotExistException(typeof(BLDrone), id);
            }
        }

        public BLStation GetStationById(int id)
        {
                IDal.DO.Station s = dal.getStationById(id);
                BLStation BLstation = convertDalToBLStation(s);
                return BLstation;
            //}
            //catch (IDal.DO.DalExceptions.ObjNotExistException t)
            //{
            //    throw new ObjNotExistException("free Drone From Charging: ", t.Message);
            //}
        }

        public BLCustomer GetCustomerById(int id)
        {
                IDal.DO.Customer c = dal.getCustomerById(id);
                BLCustomer BLcustomer = convertDalToBLCustomer(c);
                return BLcustomer;
        }

        public BLDrone GetDroneById(int id)
        {
            IDal.DO.Drone d = dal.getDroneById(id);
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }

        public BLParcel GetParcelById(int id)
        {
            IDal.DO.Parcel p = dal.getParcelById(id);
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
    }
}
