using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface Idal
    {
        //Add functions//
        void AddDrone(Drone d);
        void AddStation(Station s);
        void AddCustomer(Customer c);
        void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
        void AddParcelToDelivery(int id, int Serderid, int TargetId, WeightCategories Weight, Priorities Priority, DateTime requestedTime) { }
        void AddParcel(Parcel parcel);
        void AddDroneCharge(DroneCharge parcel);
        int amountParcels();
        int amountStations();
        //===================
        //remove Obj;
        //====================
        void removeParcel(Parcel parcel);
       

        //==================
        //Update functions//
        //==================
        string PairAParcelWithADrone(Parcel parcel);
        void DroneCollectsAParcel(Parcel parcel);
        void CostumerGetsParcel(Drone drone, Parcel parcel);
        void sendDroneToCharge(Drone drone);
        void freeDroneFromCharge(Drone drone);

        //==================
        //display lists
        //==================
        IEnumerable<Station> GetStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<DroneCharge> GetDroneCharge();
        IEnumerable<Customer> GetCustomers();
        //==================
        //change info
        //==================
        void changeStationInfo(Station s);
        void changeParcelInfo(Parcel p);
        void changeDroneInfo(int id, string newModel);
        void changeDroneInfo(Drone d);
        void changeCustomerInfo(Customer c);
        //===================
        double[] electricityUseByDrone();
        //======================
        //check if object exsist
        //======================
        public Boolean IsCustomerById(int id);
        public Boolean IsParcelById(int id);
        public Boolean IsDroneById(int id);
        public Boolean IsStationById(int id);

        //======================
        //Predicate
        //======================
        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate);
        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate);
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate);
        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate);
        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate);
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate);


        public double[] electricityUseByDrone(Drone drone);


    }

}

//function to use in the futer
//======================================================
//void removeDrone(Drone drone);
//void removeDroneCharge(DroneCharge droneCharge);
//void removeStation(Station station);
//void removeCustomer(Customer customer);

//public void removeStation(Station station)
//{
//    DataSource.Stations.Remove(station);
//}
//public void removeCustomer(Customer customer)
//{
//    DataSource.Customers.Remove(customer);
//}
//public void removeDrone(Drone drone)
//{
//    DataSource.Drones.Remove(drone);
//}
//public void removeDroneCharge(DroneCharge droneCharge)
//{
//    DataSource.DroneCharges.Remove(droneCharge);
//}
//===========================================================
