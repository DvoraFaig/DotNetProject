﻿using System;
using System.Collections.Generic;
using System.Text;
using BO;
namespace BlApi
{
    public interface Ibl
    {
        public Action<Drone> DroneChangeAction { get; set; }
        public Action<Customer> CustomerChangeAction { get; set; }
        public Action<Parcel> ParcelChangeAction { get; set; }
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
        public void RemoveStation(Station station);
        //public void RemoveCustomer(Customer customer);
        public void RemoveCustomer(int customerId);
        public void RemoveDrone(Drone drone);
        public void RemoveDroneCharge(int droneId);


        //================
        //  Display
        //================
        public List<StationToList> GetStationsToList();
        public IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0);

        public IEnumerable<Drone> getDrones();/// change from List - chrck
        public IEnumerable<DroneToList> returnDronesToList();
        public IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status);

        public IEnumerable<ParcelToList> GetParcelToList();
        public IEnumerable<Parcel> getParcels();
        public Parcel getParcelByDrone(int droneId);
        public bool checkIfExistParcelByDrone(int droneId);
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
        //public void UpdateCustomerDetails(int id, string name = null, string phone = null);
        public BO.Customer UpdateCustomerDetails(int id, string name = null, string phone = null);
        public Drone SendDroneToCharge(int droneId);
        //public Drone FreeDroneFromCharging(int droneId, double timeCharging);
        public Drone FreeDroneFromCharging(int droneId/*, double timeCharging*/);
        public Drone PairParcelWithDrone(int droneId);
        public Drone DronePicksUpParcel(int droneId);
        public Drone DeliveryParcelByDrone(int idDrone);

        public void changeDroneInfo(Drone droneWithUpdateInfo);

        //public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority);

        //========================
        //  Get Data About Object
        //========================
        //public int GetDroneStatusInDelivery(Drone drone);
        public int GetDroneStatusInDelivery(Drone droneId);
        public DeliveryStatusAction GetfromEnumDroneStatusInDelivery(Drone droneId);

        //===================
        //  Remove
        //===================
        public void RemoveParcel(int parcelId);
        //public void removeDroneChargeByDroneId(int droneId);


        //===================
        //  StartSimulation
        //===================
        public void StartSimulation(Drone std, Action<Drone, droneStatusInDelivery, double> updateStudent, Func<bool> needToStop);

        //===========================
        //  Drones electricity usage.
        //===========================
        public double requestElectricity(int choice);



        DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition, double droneBattery);

        //=========================
        //Simulation
        //=========================
        Parcel convertDalToBLParcelSimulation(DO.Parcel p);
        void removeDroneChargeByDroneId(int droneId);
        public Customer convertDalToBLCustomer(DO.Customer customer);
        public void changeParcelInfo(Parcel parcel);


    }
}
