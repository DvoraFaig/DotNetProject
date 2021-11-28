using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Text;
using IDal;
using IBL.BO;
using static IBL.BO.Exceptions;
using System.Linq.Expressions;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        public List<BLStation> DisplayStations()
        {
            IEnumerable<IDal.DO.Station> dalStations = dal.displayStations();
            List<BLStation> arr = new List<BLStation>();
            foreach (var s in dalStations)
            {
                arr.Add(convertDalToBLStation(s));
            }
            return arr;
        }

        public List<BLCustomer> DisplayCustomers()
        {
            IEnumerable<IDal.DO.Customer> cList = dal.displayCustomers();
            List<BLCustomer> arr = new List<BLCustomer>();
            foreach (var c in cList)
            {
                arr.Add(convertDalToBLCustomer(c));
            }
            return arr;
        }

        public List<BLDrone> DisplayDrones()
        {
            IEnumerable<IDal.DO.Drone> dList = dal.displayDrone();
            List<BLDrone> arr = new List<BLDrone>();
            foreach (var d in dList)
            {
                arr.Add(convertDalToBLDrone(d));
            }
            return arr;
        }
        public Expression<Func<IDal.DO.Parcel, bool>> IsMatchedExpression()
        {
            var parameterExpression = Expression.Parameter(typeof(IDal.DO.Parcel));
            var propertyOrField = Expression.PropertyOrField(parameterExpression, "DroneId");
            var binaryExpression = Expression.GreaterThan(propertyOrField, Expression.Constant(2));
            return Expression.Lambda<Func<IDal.DO.Parcel, bool>>(binaryExpression, parameterExpression);
        }
        private readonly Func<IDal.DO.Parcel, bool> IsMatch = x => x.DroneId > 1;

        public List<BLParcel> DisplayParcel()
        {
            //Expression<Func<IDal.DO.Parcel, bool>> a = IsMatchedExpression();
            //IEnumerable<IDal.DO.Parcel> pList = IDal.DO.IDal.GetAllMatchedEntities(a);
            IEnumerable<IDal.DO.Parcel> pList = dal.GetAllMatchedEntities(IsMatch);
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }
        //public List<BLParcel> DisplayParcel()
        //{
        //    IEnumerable<IDal.DO.Parcel> pList = dal.displayParcels();
        //    List<BLParcel> arr = new List<BLParcel>();
        //    foreach (var p in pList)
        //    {
        //        arr.Add(convertDalToBLParcel(p));
        //    }
        //    return arr;
        //}

        public List<BLParcel> DisplayFreeParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayFreeParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }
        public List<BLStation> DisplayEmptyDroneCharge()
        {
            IEnumerable<IDal.DO.Station> dalStations = dal.displayStations();
            List<BLStation> arr = new List<BLStation>();
            foreach (var s in dalStations)
            {
                if (s.ChargeSlots >= 1)
                {
                    arr.Add(convertDalToBLStation(s));
                }
            }
            return arr;
        }
    }
}
