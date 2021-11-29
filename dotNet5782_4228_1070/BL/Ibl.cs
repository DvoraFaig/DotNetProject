using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

namespace IBL
{
    public interface Ibl
    {
        //================
        // Add
        //================
        public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot);
        public void AddDrone(int id, string model, int maxWeight, int stationId);
        public void AddCustomer(int id, string name, string phone, BLPosition position);
        public void AddParcel(int senderId, int targetId, int weight, int priority);
        //================
        // Display
        //================
        public List<BLStation> DisplayStations();
        public List<BLCustomer> DisplayCustomers();
        public List<BLDrone> DisplayDrones();
        public List<BLParcel> DisplayParcel();
        public List<BLParcel> DisplayFreeParcel();
        public List<BLStation> DisplayEmptyDroneCharge();
        //================
        // Get object
        //================
        public BLStation GetStationById(int id);
        public BLCustomer GetCustomerById(int id);
        public BLDrone GetDroneById(int id);
        public BLParcel GetParcelById(int id);
        //================
        // Updates
        //================
        public void DroneChangeModel(int id, string newModel);
        public void StationChangeDetails(int id, string name = null, int ChargeSlots = -1);
        public void UpdateCustomerDetails(int id, string name = null, string phone = null);
        public void SendDroneToCharge(int droneId);
        public void FreeDroneFromCharging(int droneId, double timeCharging);
        public void PairParcelWithDrone(int droneId);
        public void DronePicksUpParcel(int droneId);
        public void DeliveryParcelByDrone(int idDrone);
        public void GetParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority);

        //===================
        //predicat
        //===================
    }

}

