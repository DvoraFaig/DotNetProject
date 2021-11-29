using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;
using System.Linq.Expressions;
using System.Linq;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {

        public Expression<Func<IDal.DO.Parcel, bool>> IsMatchedExpression()
        {
            var parameterExpression = Expression.Parameter(typeof(IDal.DO.Parcel));
            var propertyOrField = Expression.PropertyOrField(parameterExpression, "DroneId");
            var binaryExpression = Expression.GreaterThan(propertyOrField, Expression.Constant(2));
            return Expression.Lambda<Func<IDal.DO.Parcel, bool>>(binaryExpression, parameterExpression);
        }

        private readonly Func<IDal.DO.Parcel, bool> gtPredicatObjectId = x => x.DroneId > 1;
        //public List<BLParcel> DisplayParcel()
        //{
        //    //Expression<Func<IDal.DO.Parcel, bool>> a = IsMatchedExpression();
        //    //IEnumerable<IDal.DO.Parcel> pList = IDal.DO.IDal.GetAllMatchedEntities(a);
        //    IEnumerable<IDal.DO.Parcel> pList = dal.getParcelWithSpecificCondition(gtPredicatObjectId);
        //    IEnumerable<IDal.DO.Parcel> pList2 = dal.getParcelWithSpecificCondition(x => x.Id > 0);

        //    List<BLParcel> arr = new List<BLParcel>();
        //    foreach (var p in pList)
        //    {
        //        arr.Add(convertDalToBLParcel(p));
        //    }
        //    return arr;
        //}


        //public Predicate<IDal.DO.Parcel> GetObjByIdP(int id, List<IDal.DO.Parcel> parcels)
        //{
        //    var predicat =  from item in parcels
        //           where id == item.Id
        //           select item;
        //    return predicat;
        //}



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
        //public BLStation GetStationByIdP(int id)
        //{
        //    IDal.DO.Station s = dal.getStationById(id);
        //    BLStation BLstation = convertDalToBLStation(s);
        //    return BLstation;
        //}

        //public BLCustomer GetCustomerByIdP(int id)
        //{
        //    IDal.DO.Customer c = dal.getCustomerById(id);
        //    BLCustomer BLcustomer = convertDalToBLCustomer(c);
        //    return BLcustomer;
        //}

        //public BLDrone GetDroneByIdP(int id)
        //{
        //    IDal.DO.Drone d = dal.getDroneById(id);
        //    BLDrone BLdrone = convertDalToBLDrone(d);
        //    return BLdrone;
        //}

        //public BLParcel GetParcelByIdP(int id)
        //{
        //    IDal.DO.Parcel p = dal.getParcelById(id);
        //    BLParcel BLparcel = convertDalToBLParcel(p);
        //    return BLparcel;
        //}
    }
}
