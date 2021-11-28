using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;
using System.Linq.Expressions;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {

        //private Expression<Func<BLDrone>> getBLDroneByIdP(int id)
        //{
        //    var d = dronesInBL.First(d => d.Id == id);
        //    return d;
        //}
        //public static Func<int, BLDrone> multiplyByFive = id =>
        //{
        //    BLDrone d = dronesInBL.First(d => d.Id == id);
        //    return d;
        //};
        
        //int result = multiplyByFive(7);
        public BLStation GetStationByIdP(int id)
        {
            IDal.DO.Station s = dal.getStationById(id);
            BLStation BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        public BLCustomer GetCustomerByIdP(int id)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            BLCustomer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        public BLDrone GetDroneByIdP(int id)
        {
            IDal.DO.Drone d = dal.getDroneById(id);
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }

        public BLParcel GetParcelByIdP(int id)
        {
            IDal.DO.Parcel p = dal.getParcelById(id);
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
    }
}
