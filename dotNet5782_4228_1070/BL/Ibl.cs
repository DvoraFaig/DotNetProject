using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

namespace IBL
{
    public interface Ibl
    {
        ////Add functions//
        //public void AddDrone(int id, string Model, WeightCategories MaxWeight, BO.DroneStatus Status, double Battery);
        //public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);
        public void AddStation(BLStation s);
        //public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
        public void AddCustomer(BLCustomer c);

        //public void AddParcelToDelivery(int id, int Serderid, int TargetId, BO.WeightCategories Weight, BO.Priorities Priority, DateTime requestedTime) { }

        ////Update functions//
        //public string PairAParcelWithADrone(Parcel parcel);
        //public void DroneCollectsAParcel(Parcel parcel);
        //public void CostumerGetsParcel(Drone drone, Parcel parcel);
        //public void sendDroneToCharge(Drone drone);
        //public void freeDroneFromCharge(Drone drone);
        //public Station getStationById(int id);
        //public Drone getDroneById(int id);

        //public Parcel getParcelById(int id);
        //public DroneCharge getDroneChargeById(int id);
        //public DroneCharge getDroneChargeByDroneId(int id);
        //public IEnumerable<Station> displayStations();
        //public IEnumerable<Drone> displayDrone();
        //public IEnumerable<Parcel> displayParcels();
        //public IEnumerable<Parcel> displayFreeParcels();
        //public IEnumerable<DroneCharge> displayDroneCharge();
        //public IEnumerable<Customer> displayCustomers();


        public BLStation convertDalToBLStation(IDal.DO.Station s);
        public BLCustomer convertDalToBLCustomer(IDal.DO.Customer c);
        public BLDrone convertDalToBLDrone(IDal.DO.Drone d);
        public BLParcel convertDalToBLParcel(IDal.DO.Parcel p);
        public BLStation getStationById(int id);
        public BLCustomer getCustomerById(int id);
        public BLDrone getDroneById(int id);
        public BLParcel getParcelById(int id);
        public List<BLStation> displayStations();
        public List<BLDrone> displayDrones();
        public List<BLCustomer> displayCustomers();
        public List<BLParcel> displayParcel();
        public List<BLParcel> displayFreeParcel();



        public BLStation convertDalStationToBLStation(IDal.DO.Station s);

        public double distance(float x1, float y1, float x2, float y2);

    }
}
