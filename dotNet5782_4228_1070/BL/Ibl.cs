using System;
using System.Collections.Generic;
using System.Text;
using BO;
namespace BlApi
{
    public interface Ibl
    {

        //================
        //  Add
        //================
        public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot);
        public void AddDrone(int id, string model, int maxWeight, int stationId);
        public void AddCustomer(int id, string name, string phone, Position position);
        public void AddParcel(int senderId, int targetId, int weight, int priority);

        //================
        //  Display
        //================
        public List<BLStationToList> DisplayStationsToList();
        public List<Station> DisplayStations();
        public IEnumerable<BLStationToList> DisplayStationsWithFreeSlots(int amountAvilableSlots = 0);
        public List<Customer> DisplayCustomers();
        public List<Drone> DisplayDrones();
        public List<DroneToList> DisplayDronesToList();
        public List<ParcelToList> DisplayParcelToList();
        public List<Parcel> DisplayParcel();
        public List<Parcel> DisplayFreeParcel();
        public List<Station> DisplayEmptyDroneCharge();
        public List<DroneToList> DisplayDroneToListByWeightAndStatus(int weight, int status);

        //================
        //  Get object
        //================
        public Station GetStationById(int id);
        public Customer GetCustomerById(int id);
        public Drone GetDroneById(int id);
        public Parcel GetParcelById(int id);

        //================
        //  Updates
        //================
        public void DroneChangeModel(Drone drone);
        public void DroneChangeModel(DroneToList drone);
        public void StationChangeDetails(int id, string name = null, int ChargeSlots = -1);
        public void UpdateCustomerDetails(int id, string name = null, string phone = null);
        public void SendDroneToCharge(int droneId);
        public void FreeDroneFromCharging(int droneId, double timeCharging);
        public void PairParcelWithDrone(int droneId);
        public void DronePicksUpParcel(int droneId);
        public void DeliveryParcelByDrone(int idDrone);
        public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority);

        //===================
        //  predicat
        //===================
        public IEnumerable<Drone> getBLDroneWithSpecificCondition(Predicate<Drone> predicate);

        //===================
        //  Convertions
        //===================

        public Drone convertDroneToListToDrone(int droneId);


        //========================
        //  Get Data About Object
        //========================
        public int GetDroneStatusInDelivery(Drone drone);

        //===================
        //  Remove
        //===================
        public void RemoveParcel(Parcel parcel);
    }
}