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
        public void AddStation(Station stationToAdd); 
        public void AddDrone(Drone droneToAdd, int stationId);
        public void AddCustomer(BO.Customer customer);
        public void AddParcel(Parcel parcelToAdd);

        //================
        //  Remove
        //================
        //public void RemoveStation(Station station);
        public void RemoveStation(int stationId);
        public void RemoveCustomer(Customer customer);
        public void RemoveDrone(int droneId);

        //================
        //  Display
        //================
        public List<StationToList> GetStationsToList();
        //public IEnumerable<Station> GetStation();
        public IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0);
        public IEnumerable<Drone> DisplayDrones();/// change from List - chrck
        public IEnumerable<DroneToList> DisplayDronesToList();
        public IEnumerable<ParcelToList> GetParcelToList();
        public IEnumerable<Parcel> getParcels();
        public Parcel getParcelByDrone(int droneId);
        public bool checkIfExistParcelByDrone(int droneId);
        public IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status);
        public IEnumerable<ParcelToList> DisplayParcelToListByFilters(int weight, int status, int priority);
        public IEnumerable<CustomerToList> GetCustomersToList();
        public List<CustomerInParcel> GetLimitedCustomersList(CustomerInParcel customer = null);

        //================
        //  Get object
        //================
        public Station GetStationById(int id);
        public Customer GetCustomerById(int id);
        public Customer GetCustomerByIdAndName(int id, string name);
        public Drone GetDroneById(int id);
        public Parcel GetParcelById(int id);
        bool ifWorkerExist(Worker worker);

        //================
        //  Updates
        //================
        //public void ChangeDroneModel(Drone drone);
        public void ChangeDroneModel(int droneId, string newModel);
        public void changeInfoOfStation(int id, string name = null, int ChargeSlots = -1);
        public void UpdateCustomerDetails(int id, string name = null, string phone = null);
        public Drone SendDroneToCharge(int droneId);
        public Drone FreeDroneFromCharging(int droneId, double timeCharging);
        public Drone PairParcelWithDrone(int droneId);
        public Drone DronePicksUpParcel(int droneId);
        public Drone DeliveryParcelByDrone(int idDrone);
        //public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority);
       
        //========================
        //  Get Data About Object
        //========================
        //public int GetDroneStatusInDelivery(Drone drone);
        public int GetDroneStatusInDelivery(int droneId);

        //===================
        //  Remove
        //===================
        public void RemoveParcel(int parcelId);
    }
}