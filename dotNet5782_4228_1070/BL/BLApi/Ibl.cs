using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BlApi
{
    public interface IBl
    {
        public Action<Drone> DroneChangeAction { get; set; }
        Action<Drone, bool > DroneListChangeAction { get; set; }
        Action<Customer> CustomerChangeAction { get; set; }
        Action<Parcel> ParcelChangeAction { get; set; }

        //=================
        //Station functions
        //=================
        IEnumerable<StationToList> GetStationsToList();
        IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0);
        Station GetStationById(int id);
        void AddStation(Station stationToAdd); 
        void RemoveStation(Station station);
        void changeStationInfo(int id, string name = null, int ChargeSlots = -1);


        //===============
        //Drone functions
        //===============
        IEnumerable<DroneToList> GetDronesToList();
        IEnumerable<DroneToList> GetDronesByConditions(int weight, int status);
        Drone GetDroneById(int id);
        void AddDrone(Drone droneToAdd, int stationId);
        void RemoveDrone(Drone drone);
        void ChangeDroneModel(int droneId, string newModel);


        DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition, double droneBattery);
        public double requestElectricity(int choice);
        Drone SendDroneToCharge(int droneId);
        Drone FreeDroneFromCharging(int droneId);
        Drone PairParcelWithDrone(int droneId);
        Drone DronePicksUpParcel(int droneId);
        void changeDroneInfoInDroneList(Drone droneWithUpdateInfo);
        Drone DeliveryParcelByDrone(int idDrone);
        int GetDroneStatusInDelivery(Drone droneId);
        DeliveryStatusAction returnDroneStatusInDelivery(Drone droneId);


        //==================
        //Customer functions
        //==================
        IEnumerable<CustomerToList> GetCustomersToList();
        IEnumerable<CustomerInParcel> GetCustomersExeptOne(int customerId = 0);
        Customer GetCustomerById(int id, string name = null);
        void AddCustomer(BO.Customer customer);
        void RemoveCustomer(int customerId);
        BO.Customer changeCustomerInfo(int id, string name = null, string phone = null);


        //================
        //Parcel functions
        //================
        IEnumerable<ParcelToList> GetParcelToList();
        IEnumerable<ParcelToList> GetParcelsByConditions(int weight, int status, int priority);
        Parcel GetParcelById(int id);
        Parcel GetParcelByDrone(int droneId);
        void AddParcel(Parcel parcelToAdd);
        void RemoveParcel(int parcelId);

       


        //===================
        //  StartSimulation
        //===================
        void StartSimulation(Drone std, Action<Drone, DroneStatusInSim, double> updateStudent, Func<bool> needToStop);

        bool IfWorkerExist(Worker worker);
        Station convertDalToBLStation(DO.Station station);
       
    }
}
