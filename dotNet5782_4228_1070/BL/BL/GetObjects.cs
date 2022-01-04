using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.Exceptions;

namespace BL
{
using BO;
    public sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Send a predicate to receive a BO.Drone from BL.DroneList. 
        /// </summary>
        /// <param name="droneRequestedId">The drone with this id</param>
        /// <returns></returns>
        private Drone getDroneByIdFromDronesList(int droneRequestedId)
        {
            try
            {
                return getDroneWithSpecificConditionFromDronesList(d => d.Id == droneRequestedId).First() ;
            }
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(Drone), droneRequestedId,e);
            }
        }

        /// <summary>
        /// Return a drone from droneList from BL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private IEnumerable<Drone> getDroneWithSpecificConditionFromDronesList(Predicate<Drone> predicate)
        {
            return (from drone in droensList
                    where predicate(drone)
                    select drone);
        }

        /// <summary>
        /// Return a BO.Parcel/s(converted) with a specific condition = predicate from parcels = getParcels
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
        {
            IEnumerable<Parcel> parcels = getParcels();
            return (from parcel in parcels
                    where predicate(parcel)
                    select parcel);
        }
        public Parcel getParcelByDrone(int droneId)
        {
            try
            {
                DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (parcel.Equals(null))//
                    throw new Exceptions.ObjNotExistException(typeof(ParcelInTransfer), -1);//

               return convertDalToBLParcel(parcel);
            }
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(ParcelInTransfer),-1 ,e);
            }
        }

        /// <summary>
        /// Check a predicate to dal and check if drone is schedualed to Parcel.
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public bool checkIfExistParcelByDrone(int droneId)
        {
            try
            {
                DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                if (parcel.Equals(null))//
                    return false;
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <summary>
        /// Return a BO.Station(converted) by an id from dal.getStationWithSpecificCondition
        /// </summary>
        /// <param name="stationrequestedId">The id of the drone that's requested</param>
        /// <returns></returns>
        public Station GetStationById(int stationrequestedId)
        {
            DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == stationrequestedId).First();
            Station BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        /// <summary>
        /// Return a BO.Customer(converted) by an id from dal.getCustomerWithSpecificCondition
        /// </summary>
        /// <param name="customerRequestedId">The id of the customer that's requested</param>
        /// <returns></returns>
        public Customer GetCustomerById(int customerRequestedId)
        {
            DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId).First();
            Customer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        /// <summary>
        /// Return a BO.Customer(converted) by id and name from dal.getCustomerWithSpecificCondition
        /// </summary>
        /// <param name="customerRequestedId">The id of the customer that requested<</param>
        /// <param name="customerRequestedName">The name of the customer that requested<</param>
        /// <returns></returns>
        public Customer GetCustomerByIdAndName(int customerRequestedId,string customerRequestedName)
        {
            DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId && c.Name == customerRequestedName).First();
            Customer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        /// <summary>
        /// Get a drone by id from getBLDroneWithSpecificCondition = dronesList.
        /// </summary>
        /// <param name="droneRequestedId">The id of the drone that's requested<</param>
        /// <returns></returns>
        public Drone GetDroneById(int droneRequestedId)
        {
            return getDroneWithSpecificConditionFromDronesList(d => d.Id == droneRequestedId).First();
        }

        /// <summary>
        /// Return a BO.Parcel(converted) that the func receives it by an id from dal.getParcelWithSpecificCondition
        /// </summary>
        /// <param name="parcelRequestedId">The id of the parcel that's requested</param>
        /// <returns></returns>
        public Parcel GetParcelById(int parcelRequestedId)
        {
            DO.Parcel p = dal.getParcelWithSpecificCondition(p => p.Id == parcelRequestedId).First();
            Parcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
    }
}
