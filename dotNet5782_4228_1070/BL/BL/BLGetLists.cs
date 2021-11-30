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
            IEnumerable<IDal.DO.Customer> cList = dal.displayCustomers();//c.id>100000000
            List<BLCustomer> arr = new List<BLCustomer>();
            foreach (var c in cList)
            {
                arr.Add(convertDalToBLCustomer(c));
            }
            return arr;
        }

        public List<BLDrone> DisplayDrones()
        {
            return dronesInBL;
        }
        public List<BLParcel> DisplayParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }

        public List<BLParcel> DisplayFreeParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.getParcelWithSpecificCondition(x => x.DroneId == null);
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
            List<BLStation> arrEmptySlots = new List<BLStation>();
            foreach (IDal.DO.Station s in dalStations)
            {
                int amountDroneChargeFullInStation = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == s.Id).Count();
                if (s.ChargeSlots > amountDroneChargeFullInStation)
                {
                    arrEmptySlots.Add(convertDalToBLStation(s));
                }
            }
            return arrEmptySlots;
        }

        public List<BLDrone> DisplayDroneByWeight(IDal.DO.WeightCategories weight)
        {
            List<BLDrone> list = new List<BLDrone>();
            IEnumerable<BLDrone> IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == weight);
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return list;
        }

        public List<BLDrone> DisplayDroneByStatus(DroneStatus status)
        {
            List<BLDrone> list = new List<BLDrone>();
            IEnumerable<BLDrone> IList = getBLDroneWithSpecificCondition(d => d.Status == status);
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return list;
        }

    }
}
