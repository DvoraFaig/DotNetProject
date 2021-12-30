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
        /// <summary>
        /// Returns a IEnumerable<CustomerToList> by recieving customers and converting them to CustomerToList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerToList> GetCustomersToList()
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerToList> customerToLists = new List<CustomerToList>();
            return from customer in customers
                   select converteCustomerToList(customer);
        }

        /// <summary>
        /// Return a List<CustomerInParcel>
        /// </summary>
        /// <param name="customerInParcel"></param>
        /// <returns></returns>
        public List<CustomerInParcel> GetLimitedCustomersList(CustomerInParcel customerInParcel = null)
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerInParcel> customerToLists = new List<CustomerInParcel>();
            foreach (var customer in customers)
            {
                customerToLists.Add(convertDalToBLCustomerInParcel(customer));
            }
            if (customerInParcel != null)
            {
                customerInParcel = customerToLists.SingleOrDefault(c=>c.Id==customerInParcel.Id);
                customerToLists.Remove(customerInParcel);
            }
            return customerToLists;
        }

        /// <summary>
        /// Returns a IEnumerable<StationToList> by recieving staions and converting them to BLStationToList
        /// </summary>
        /// <returns></returns>
        public List<StationToList> GetStationsToList()
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var station in stations)
            {
                int occupiedChargeSlotsInStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id).Count();
                int avilableChargeSlotsInStation = station.ChargeSlots - occupiedChargeSlotsInStation;
                stationToList.Add(new StationToList() { Id = station.Id, Name = station.Name, DroneChargeAvailble = avilableChargeSlotsInStation, DroneChargeOccupied = occupiedChargeSlotsInStation });

            }
            return stationToList;
        }

        /// <summary>
        /// Return IEnumerable<StationToList> by receiving a converted list of station (one of BO.Station is availableChargeSlots).
        /// </summary>
        /// <param name="amountAvilableSlots"></param>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0)
        {
            List<StationToList> stationToList = GetStationsToList();
            return (from station in stationToList
                    where station.DroneChargeAvailble >= amountAvilableSlots
                    select station);
        }

        /// <summary>
        /// Return DronesList
        /// </summary>
        /// <returns></returns>
        public List<Drone> DisplayDrones()
        {
            return droensList;
        }

        /// <summary>
        /// Return DroneList converted to DronesToList
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> DisplayDronesToList()
        {
            return convertDronesToDronesToList(droensList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> GetParcelToList()//////
        {
            IEnumerable<BO.Parcel> parcels = (from parcel in dal.GetParcels()
                   select convertDalToBLParcel(parcel));
            return convertBLParcelToBLParcelsToList(parcels);
        }

        /// <summary>
        /// Returns a IEnumerable<Parcels> by recieving parcels from dal and converting them to BO.Parcel.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> getParcels()
        {
            IEnumerable<DO.Parcel> parcels = dal.GetParcels();
            return (from parcel in parcels
                    select convertDalToBLParcel(parcel));
        }

        /// <summary>
        /// Receive weight and status and returns List<BLDroneToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight">if 3>weight>-1 == values of DroneStatus. if weight==-1 weight is null</param>
        /// <param name="status">if 3>status>-1 == values of DroneStatus. if status==-1 weight is null</param>
        /// <returns></returns>
        public IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status)
        {
            IEnumerable<Drone> IList = DisplayDrones(); //if Both null took it out of th eif becuase Ienumerable needed a statment...
            if (weight >= 0 && status == -1)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.Status == (DroneStatus)status);
            else if (weight >= 0 && status >= 0)
                IList = getDroneWithSpecificConditionFromDronesList(d => d.MaxWeight == (DO.WeightCategories)weight && d.Status == (DroneStatus)status);
            return convertDronesToDronesToList(IList);
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
            IEnumerable<Parcel> IList;
            if (weight >= 0 && status >= 0 && priority >= 0) IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status && p.Priority == (DO.Priorities)priority);
            else if (weight >= 0 && status >= 0 && priority == -1) IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status );
            else if (weight >= 0 && status == -1 && priority >= 0) IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight&& p.Priority == (DO.Priorities)priority);
            else if (weight >= 0 && status == -1 && priority == -1) IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight);
            else if (weight == -1 && status >= 0 && priority >= 0) IList = getParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status&& p.Priority == (DO.Priorities)priority);
            else if (weight == -1 && status >= 0 && priority == -1) IList = getParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status);
            else if (weight == -1 && status == -1 && priority >= 0) IList = getParcelWithSpecificCondition(p => p.Priority == (DO.Priorities)priority);
            else IList = getParcels();
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
//public List<ParcelToList> DisplayParcelToList(List<Parcel> parcels)
//{
//    return convertBLParcelToBLParcelsToList(parcels);
//}
//public IEnumerable<Station> GetStation()
//{
//    IEnumerable<DO.Station> stations = dal.GetStations();
//    return (from station in stations
//    select convertDalToBLStation(station));
//}
//public IEnumerable<BLStationToList> DisplayStationsWithFreeSlots()
//{
//    List<BLStationToList> stationToList = GetStationsToList();
//    return (from station in stationToList
//            where station.DroneChargeAvailble > 0
//            select station);
//}

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
//public List<DroneToList> DisplayDronesToList()
//{
//    return convertBLDroneToBLDronesToList(dronesInBL);
//}
//public IEnumerable<Parcel> DisplayFreeParcel()
//{
//    IEnumerable<DO.Parcel> freeParcels = dal.getParcelWithSpecificCondition(x => x.DroneId == null);
//    return (from parcel in freeParcels
//            select convertDalToBLParcel(parcel));
//}
//public List<Station> DisplayEmptyDroneCharge()
//{
//    IEnumerable<DO.Station> stations = dal.GetStations();
//    List<Station> stationsWithEmptySlots = new List<Station>();
//    foreach (DO.Station station in stations)
//    {
//        int amountDroneChargeFullInStation = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
//        if (station.ChargeSlots > amountDroneChargeFullInStation)
//        {
//            stationsWithEmptySlots.Add(convertDalToBLStation(station));
//        }
//    }
//    return stationsWithEmptySlots;
//}