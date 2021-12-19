using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Linq.Expressions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        public List<Station> DisplayStations()
        {
            IEnumerable<DO.Station> stations = dal.displayStations();
            List<Station> stationsWithMoreInfo = new List<Station>();
            foreach (var station in stations)
            {
                stationsWithMoreInfo.Add(convertDalToBLStation(station));
            }
            return stationsWithMoreInfo;

        }

        public List<BLStationToList> DisplayStationsToList()
        {
            IEnumerable<DO.Station> stations = dal.displayStations();
            List<BLStationToList> stationToList = new List<BLStationToList>();
            foreach (var station in stations)
            {
                int occupiedChargeSlotsInStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id).Count();
                int avilableChargeSlotsInStation = station.ChargeSlots - occupiedChargeSlotsInStation;
                stationToList.Add(new BLStationToList() { Id = station.Id, Name = station.Name, DroneChargeAvailble = avilableChargeSlotsInStation, DroneChargeOccupied = occupiedChargeSlotsInStation });

            }
            return stationToList;
        }
        public IEnumerable<BLStationToList> DisplayStationsWithFreeSlots()
        {
            List<BLStationToList> stationToList = DisplayStationsToList();
            return (from station in stationToList
                    where station.DroneChargeAvailble > 0
                    select station);
        }

        public List<Customer> DisplayCustomers()
        {
            IEnumerable<DO.Customer> customers = dal.displayCustomers();//c.id>100000000
            List<Customer> costomersWithMoreInfo = new List<Customer>();
            foreach (var customer in customers)
            {
                costomersWithMoreInfo.Add(convertDalToBLCustomer(customer));
            }
            return costomersWithMoreInfo;
        }

        public List<Drone> DisplayDrones()
        {
            return dronesInBL;
        }
        public List<DroneToList> DisplayDronesToList()
        {
            return convertBLDroneToBLDronesToList(dronesInBL);
        }
        public List<Parcel> DisplayParcel()
        {
            IEnumerable<DO.Parcel> parcels = dal.displayParcels();
            List<Parcel> parcelsWithMoreInfo = new List<Parcel>();
            foreach (var parcel in parcels)
            {
                parcelsWithMoreInfo.Add(convertDalToBLParcel(parcel));
            }
            return parcelsWithMoreInfo;
        }

        public List<Parcel> DisplayFreeParcel()
        {
            IEnumerable<DO.Parcel> freeParcels = dal.getParcelWithSpecificCondition(x => x.DroneId == null);
            List<Parcel> FreeParcelsWithMoreInfo = new List<Parcel>();
            foreach (var freeParcel in freeParcels)
            {
                FreeParcelsWithMoreInfo.Add(convertDalToBLParcel(freeParcel));
            }
            return FreeParcelsWithMoreInfo;
        }

        public List<Station> DisplayEmptyDroneCharge()
        {
            IEnumerable<DO.Station> stations = dal.displayStations();
            List<Station> stationsWithEmptySlots = new List<Station>();
            foreach (DO.Station station in stations)
            {
                int amountDroneChargeFullInStation = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
                if (station.ChargeSlots > amountDroneChargeFullInStation)
                {
                    stationsWithEmptySlots.Add(convertDalToBLStation(station));
                }
            }
            return stationsWithEmptySlots;
        }
        /// <summary>
        /// Receive weight and status and returns List<BLDroneToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight">if 3>weight>-1 == values of DroneStatus. if weight==-1 weight is null</param>
        /// <param name="status">if 3>status>-1 == values of DroneStatus. if status==-1 weight is null</param>
        /// <returns></returns>
        public List<DroneToList> DisplayDroneToListByWeightAndStatus(int weight, int status)
        {
            List<Drone> list = new List<Drone>();
            IEnumerable<Drone> IList = DisplayDrones(); //if Both null took it out of th eif becuase Ienumerable needed a statment...
            if (weight >= 0 && status == -1)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (DO.WeightCategories)weight && d.Status == (DroneStatus)status);
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return convertBLDroneToBLDronesToList(list);
        }
    }
}
