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
            IEnumerable<IDal.DO.Drone> dList = dal.displayDrone();
            List<BLDrone> arr = new List<BLDrone>();
            foreach (var d in dList)
            {
                arr.Add(convertDalToBLDrone(d));
            }
            return arr;
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
                if(s.ChargeSlots > amountDroneChargeFullInStation)
                {
                    arrEmptySlots.Add(convertDalToBLStation(s));
                }
            }
            return arrEmptySlots;
        }
        
        public IEnumerable<BLDrone> DisplayMaintenanceDrones()
        {
            return getBLDroneWithSpecificCondition(d => d.Status == DroneStatus.Maintenance);
        }

        public IEnumerable<BLDrone> DisplayDeliveryDrones()
        {
            return getBLDroneWithSpecificCondition(d => d.Status == DroneStatus.Delivery);
        }

        public IEnumerable<BLDrone> DisplayAvailableDrones()
        {
           return getBLDroneWithSpecificCondition(d => d.Status == DroneStatus.Available);
        }
    }
}
