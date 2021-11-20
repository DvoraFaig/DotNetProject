using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;

namespace IDal
{
    namespace DO
    {
        public interface IDal
        {
            //Add functions//
            void AddDrone(Drone d);
            //public void AddDrone(int id, string Model, DO.WeightCategories MaxWeight, DO.DroneStatus Status, double Battery);
            void AddStation(Station s);
            //void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);          
            void AddCustomer(Customer c);
            void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
            void AddParcelToDelivery(int id, int Serderid, int TargetId, DO.WeightCategories Weight, DO.Priorities Priority, DateTime requestedTime) { }
            void AddParcel(Parcel parcel);
            void AddDroneCharge(DroneCharge parcel);
            //amount for id//
            int amountParcels();
            //Update functions//
            string PairAParcelWithADrone(Parcel parcel);
            void DroneCollectsAParcel(Parcel parcel);
            void CostumerGetsParcel(Drone drone, Parcel parcel);
            void sendDroneToCharge(Drone drone);
            void freeDroneFromCharge(Drone drone);
            Station getStationById(int id);
            Drone getDroneById(int id);
            Customer getCustomerById(int id);
            Parcel getParcelById(int id);
            DroneCharge getDroneChargeById(int id);
            DroneCharge getDroneChargeByDroneId(int id);
            IEnumerable<Station> displayStations();
            IEnumerable<Drone> displayDrone();
            IEnumerable<Parcel> displayParcels();
            IEnumerable<Parcel> displayFreeParcels();
            IEnumerable<DroneCharge> displayDroneCharge();
            IEnumerable<Customer> displayCustomers();
            double[] electricityUseByDrone();
            Parcel getParcelByDroneId(int droneId);
            void changeStationInfo(Station s);
            void changeParcelInfo(Parcel p);
            void changeDroneInfo(Drone d);

        }
    }
}
