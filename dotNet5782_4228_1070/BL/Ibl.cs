using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

namespace IBL
{
    public interface Ibl
    {
        public void addStation(int id, string name, int latitude, int longitude, int chargeSlot);
        //public void addStation(int id, string name, BLPosition position, int chargeSlot);
        //public void addDrone(int id, string model, IDal.DO.WeightCategories maxWeight, int stationId);
        void addDrone(int id, string model, int maxWeight, int stationId);
        public void AddCustomer(int id, string name, string phone, BLPosition position);
        public void addParcel(int senderId, int targetId, int weight, int priority)


        //==================================
        // Updates
        //==================================
        public void droneChangeModel(int id, string newModel);
        public void stationChangeDetails(int id, string name = null, int chargeStand = -1);//-1 is defualt value
        public void updateCustomerDetails(int id, string name = null, string phone = null);
        public void sendDroneToCharge(int droneId);
        //==================================
        // Get object by ID
        //==================================
        public void getParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority);
        public BLStation getStationById(int id);
        public BLCustomer getCustomerById(int id);
        public BLDrone getDroneById(int id);
        //BLDrone getBLDroneById(int id); //to be private
        public BLParcel getParcelById(int id);
        //==================================
        // Get list of objects
        //==================================
        public List<BLStation> displayStations();
        public List<BLCustomer> displayCustomers();
        public List<BLDrone> displayDrones();
        public List<BLParcel> displayParcel();
        public List<BLParcel> displayFreeParcel();
    }

}

