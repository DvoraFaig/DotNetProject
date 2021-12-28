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
        public IEnumerable<Station> DisplayStations()
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
            //List<Station> stationsWithMoreInfo = new List<Station>();
            return (from station in stations
            select convertDalToBLStation(station));
            //foreach (var station in stations)
            //{
            //    stationsWithMoreInfo.Add(convertDalToBLStation(station));
            //}
            //return stationsWithMoreInfo;

        }
        
        public IEnumerable<CustomerToList> DisplayCustomersToList()
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerToList> customerToLists = new List<CustomerToList>();
            return from customer in customers
                   select converteCustomerToList(customer);

            //foreach (var customer in customers)
            //{
            //    customerToLists.Add(converteCustomerToList(customer));
            //}
            //return customerToLists;
        }

        public List<CustomerInParcel> CustomerLimitedDisplay(CustomerInParcel customer = null)
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerInParcel> customerToLists = new List<CustomerInParcel>();
            foreach (var c in customers)
            {
                customerToLists.Add(convertDalToBLCustomerInParcel(c));
            }
            if (customer != null)
            {
                customer = customerToLists.SingleOrDefault(c=>c.Id==customer.Id);
                customerToLists.Remove(customer);
            }
            return customerToLists;
        }

        public List<BLStationToList> DisplayStationsToList()
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
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
        public IEnumerable<BLStationToList> DisplayStationsWithFreeSlots(int amountAvilableSlots = 0)
        {
            List<BLStationToList> stationToList = DisplayStationsToList();
            return (from station in stationToList
                    where station.DroneChargeAvailble >= amountAvilableSlots
                    select station);
        }

        //public List<Customer> DisplayCustomers()
        //{
        //    IEnumerable<DO.Customer> customers = dal.displayCustomers();//c.id>100000000
        //    List<Customer> costomersWithMoreInfo = new List<Customer>();
        //    foreach (var customer in customers)
        //    {
        //        costomersWithMoreInfo.Add(convertDalToBLCustomer(customer));
        //    }
        //    return costomersWithMoreInfo;
        //}

        public List<Drone> DisplayDrones()
        {
            return dronesInBL;
        }

        //public List<DroneToList> DisplayDronesToList()
        //{
        //    return convertBLDroneToBLDronesToList(dronesInBL);
        //}
        public IEnumerable<DroneToList> DisplayDronesToList()
        {
            return convertBLDroneToBLDronesToList(dronesInBL);
        }

        public IEnumerable<ParcelToList> DisplayParcelToList()//////
        {
            IEnumerable<BO.Parcel> parcels = (from parcel in dal.GetParcels()
                   select convertDalToBLParcel(parcel));
            return convertBLParcelToBLParcelsToList(parcels);
        }

        //public List<ParcelToList> DisplayParcelToList(List<Parcel> parcels)
        //{
        //    return convertBLParcelToBLParcelsToList(parcels);
        //}

        public IEnumerable<Parcel> DisplayParcel()
        {
            IEnumerable<DO.Parcel> parcels = dal.GetParcels();
            return (from parcel in parcels
                    select convertDalToBLParcel(parcel));
            //List<Parcel> parcelsWithMoreInfo = new List<Parcel>();
            //foreach (var parcel in parcels)
            //{
            //    parcelsWithMoreInfo.Add(convertDalToBLParcel(parcel));
            //}
            //return parcelsWithMoreInfo;
        }

        public IEnumerable<Parcel> DisplayFreeParcel()
        {
            IEnumerable<DO.Parcel> freeParcels = dal.getParcelWithSpecificCondition(x => x.DroneId == null);
            return (from parcel in freeParcels
                    select convertDalToBLParcel(parcel));
            //List<Parcel> FreeParcelsWithMoreInfo = new List<Parcel>();
            //foreach (var freeParcel in freeParcels)
            //{
            //    FreeParcelsWithMoreInfo.Add(convertDalToBLParcel(freeParcel));
            //}
            //return FreeParcelsWithMoreInfo;
        }

        public List<Station> DisplayEmptyDroneCharge()
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
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
        public IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status)
        {
            //List<Drone> list = new List<Drone>();
            IEnumerable<Drone> IList = DisplayDrones(); //if Both null took it out of th eif becuase Ienumerable needed a statment...
            if (weight >= 0 && status == -1)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getBLDroneWithSpecificCondition(d => d.MaxWeight == (DO.WeightCategories)weight && d.Status == (DroneStatus)status);

            //foreach (var i in IList)
            //{
            //    list.Add(i);
            //}
            //return (from drone in IList
            //        select convertBLDroneToBLDronesToList(list))
            return convertBLDroneToBLDronesToList(IList);
        }

        /// <summary>
        /// Receive weight, status and priority and returns List<ParcelToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public List<ParcelToList> DisplayParcelToListByFilters(int weight, int status, int priority)
        {
            List<Parcel> list = new List<Parcel>();
            IEnumerable<Parcel> IList;// = DisplayParcel();

            if (weight >= 0 && status >= 0 && priority >= 0) IList = getBLParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status && p.Priority == (DO.Priorities)priority);
            else if (weight >= 0 && status >= 0 && priority == -1) IList = getBLParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status );
            else if (weight >= 0 && status == -1 && priority >= 0) IList = getBLParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight&& p.Priority == (DO.Priorities)priority);
            else if (weight >= 0 && status == -1 && priority == -1) IList = getBLParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0 && priority >= 0) IList = getBLParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status&& p.Priority == (DO.Priorities)priority);
            else if (weight == -1 && status >= 0 && priority == -1) IList = getBLParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status);
            else if (weight == -1 && status == -1 && priority >= 0) IList = getBLParcelWithSpecificCondition(p => p.Priority == (DO.Priorities)priority);
            else IList = DisplayParcel();
            foreach (var i in IList)
            {
                list.Add(i);
            }
            return convertBLParcelToBLParcelsToList(list);
        }
    }
}


//public List<CustomerToList> DisplayCustomersToList()
//{
//    IEnumerable<DO.Customer> customers = dal.displayCustomers();
//    List<CustomerToList> customerToLists = new List<CustomerToList>();


//    foreach (var customer in customers)
//    {
//        customerToLists.Add(converteCustomerToList(customer));
//    }
//    return customerToLists;
//}