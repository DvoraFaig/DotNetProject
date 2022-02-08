﻿using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BlApi
{
    public interface IBl
    {
        public Action<Drone> DroneChangeAction { get; set; }
        Action<DroneToList, bool > DroneListChangeAction { get; set; }
        Action<Customer> CustomerChangeAction { get; set; }
        Action<Parcel> ParcelChangeAction { get; set; }

        //================
        //  Add
        //================
        void AddStation(Station stationToAdd); 
        void AddDrone(Drone droneToAdd, int stationId);
        void AddCustomer(BO.Customer customer);
        void AddParcel(Parcel parcelToAdd);

        //================
        //  Remove
        //================
        void RemoveStation(Station station);
        void RemoveCustomer(int customerId);
        void RemoveDrone(Drone drone);
        void RemoveDroneCharge(int droneId);

        //public void RemoveCustomer(Customer customer);
        //public void RemoveStation(Station station);

        //================
        //  Display
        //================
        IEnumerable<StationToList> GetStationsToList();
        IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0);
        IEnumerable<Drone> getDrones();/// change from List - check
        IEnumerable<DroneToList> returnDronesToList();
        IEnumerable<DroneToList> DisplayDroneToListByFilters(int weight, int status);
        IEnumerable<ParcelToList> GetParcelToList();
        Parcel getParcelByDrone(int droneId);
        IEnumerable<ParcelToList> DisplayParcelToListByFilters(int weight, int status, int priority);
        IEnumerable<CustomerToList> GetCustomersToList();

        //List<CustomerInParcel> GetLimitedCustomersList(CustomerInParcel customer = null);
        IEnumerable<CustomerInParcel> GetLimitedCustomersList(int customerId = 0);
        IEnumerable<Parcel> getParcels();

        //================
        //  Get object
        //================
        Station GetStationById(int id);
        Customer GetCustomerById(int id);
        Customer GetCustomerByIdAndName(int id, string name);
        Drone GetDroneById(int id);
        Parcel GetParcelById(int id);
        bool ifWorkerExist(Worker worker);

        //================
        //  Updates
        //================
        //public void ChangeDroneModel(Drone drone);
        void ChangeDroneModel(int droneId, string newModel);
        void changeInfoOfStation(int id, string name = null, int ChargeSlots = -1);
        //public void UpdateCustomerDetails(int id, string name = null, string phone = null);
        BO.Customer UpdateCustomerDetails(int id, string name = null, string phone = null);
        Drone SendDroneToCharge(int droneId);
        //public Drone FreeDroneFromCharging(int droneId, double timeCharging);
        Drone FreeDroneFromCharging(int droneId/*, double timeCharging*/);
        Drone PairParcelWithDrone(int droneId);
        Drone DronePicksUpParcel(int droneId);
        Drone DeliveryParcelByDrone(int idDrone);

        void changeDroneInfoInDroneList(Drone droneWithUpdateInfo);

        //public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority);

        //========================
        //  Get Data About Object
        //========================
        //public int GetDroneStatusInDelivery(Drone drone);
        int GetDroneStatusInDelivery(Drone droneId);
        DeliveryStatusAction GetfromEnumDroneStatusInDelivery(Drone droneId);

        //===================
        //  Remove
        //===================
        void RemoveParcel(int parcelId);
        //public void removeDroneChargeByDroneId(int droneId);


        //===================
        //  StartSimulation
        //===================
        void StartSimulation(Drone std, Action<Drone, DroneStatusInSim, double> updateStudent, Func<bool> needToStop);

        //===========================
        //  Drones electricity usage.
        //===========================
        public double requestElectricity(int choice);
        DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition, double droneBattery);
        Station convertDalToBLStation(DO.Station station);

        //=========================
        //Simulation
        //=========================
        //Parcel convertDalToBLParcelSimulation(DO.Parcel p);
        //void removeDroneChargeByDroneId(int droneId);
        //Customer convertDalToBLCustomer(DO.Customer customer);
        //void changeParcelInfo(Parcel parcel);


    }
}
