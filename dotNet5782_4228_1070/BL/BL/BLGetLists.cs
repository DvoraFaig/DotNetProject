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
        public List<BLDroneToList> DisplayDronesToList()
        {
            return convertBLDroneToBLDronesToList(dronesInBL);
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
        //added check if i could erase the other ones.
        public List<BLDroneToList> DisplayDroneToListByStatus(DroneStatus status)
        {
            List<BLDrone> list = new List<BLDrone>();
            IEnumerable<BLDrone> IList = getBLDroneWithSpecificCondition(d => d.Status == status);

            foreach (var i in IList)
            {
                list.Add(i);
            }
            return convertBLDroneToBLDronesToList(list);
        }
        public List<BLDroneToList> DisplayDroneToListByWeight(IDal.DO.WeightCategories weight)
        {
            List<BLDrone> list = new List<BLDrone>();
            IEnumerable<BLDrone> IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == weight);
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return convertBLDroneToBLDronesToList(list);
        }
        /// <summary>
        /// Receive weight and status and returns List<BLDroneToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight">if 3>weight>-1 == values of DroneStatus. if weight==-1 weight is null</param>
        /// <param name="status">if 3>status>-1 == values of DroneStatus. if status==-1 weight is null</param>
        /// <returns></returns>
        public List<BLDroneToList> DisplayDroneToListByWeightAndStatus(int weight, int status)
        {
            List<BLDrone> list = new List<BLDrone>();
            IEnumerable<BLDrone> IList = DisplayDrones(); //if Both null took it out of th eif becuase Ienumerable needed a statment...
            if (weight >= 0 && status == -1)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (IDal.DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (IDal.DO.WeightCategories)weight && d.Status == (DroneStatus)status);
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return convertBLDroneToBLDronesToList(list);
        }
    }
}
