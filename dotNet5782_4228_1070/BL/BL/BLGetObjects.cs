using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL
    {
        private BLDrone GetBLDroneById(int id)
        {
            return dronesInBL.Find(d => d.Id == id);
        }

        public BLStation getStationById(int id)
        {
            IDal.DO.Station s = dal.getStationById(id);
            BLStation BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        public BLCustomer getCustomerById(int id)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            BLCustomer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        public BLDrone getDroneById(int id)
        {
            IDal.DO.Drone d = dal.getDroneById(id);
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }

        public BLParcel getParcelById(int id)
        {
            IDal.DO.Parcel p = dal.getParcelById(id);
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }

        public IDal.DO.Drone getDalDroneById(int id)
        {
            return dal.getDroneById(id);
        }

        public IDal.DO.Station getDalStationById(int id)
        {
            return dal.getStationById(id);
        }

        public IDal.DO.Customer getDalCustomerById(int id)
        {
            return dal.getCustomerById(id);
        }

        public IDal.DO.Parcel getDalParcelById(int id)
        {
            return dal.getParcelById(id);
        }

        public IDal.DO.DroneCharge getDalDroneChargeById(int id)
        {
            return dal.getDroneChargeById(id);
        }
    }
}
