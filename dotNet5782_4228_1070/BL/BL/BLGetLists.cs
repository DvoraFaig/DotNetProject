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
    public sealed partial class BL : IBL.Ibl
    {
        public List<BLStation> displayStations()
        {
            IEnumerable<IDal.DO.Station> dalStations = dal.displayStations();
            List<BLStation> arr = new List<BLStation>();
            foreach (var s in dalStations)
            {
                arr.Add(convertDalToBLStation(s));
            }
            return arr;
        }

        public List<BLCustomer> displayCustomers()
        {
            IEnumerable<IDal.DO.Customer> cList = dal.displayCustomers();
            List<BLCustomer> arr = new List<BLCustomer>();
            foreach (var c in cList)
            {
                arr.Add(convertDalToBLCustomer(c));
            }
            return arr;
        }

        public List<BLDrone> displayDrones()
        {
            IEnumerable<IDal.DO.Drone> dList = dal.displayDrone();
            List<BLDrone> arr = new List<BLDrone>();
            foreach (var d in dList)
            {
                arr.Add(convertDalToBLDrone(d));
            }
            return arr;
        }

        public List<BLParcel> displayParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }

        public List<BLParcel> displayFreeParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayFreeParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }
        public List<BLStation> displayEmptyDroneCharge()
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
