using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalExceptions;

namespace IDal
{
    namespace DO
    {
        public interface IDal
        {
            //Add functions//
            void AddDrone(Drone d);
            void AddStation(Station s);
            void AddCustomer(Customer c);
            void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
            void AddParcelToDelivery(int id, int Serderid, int TargetId, DO.WeightCategories Weight, DO.Priorities Priority, DateTime requestedTime) { }
            void AddParcel(Parcel parcel);
            void AddDroneCharge(DroneCharge parcel);
            int amountParcels();
            //==================
            //Update functions//
            //==================
            string PairAParcelWithADrone(Parcel parcel);
            void DroneCollectsAParcel(Parcel parcel);
            void CostumerGetsParcel(Drone drone, Parcel parcel);
            void sendDroneToCharge(Drone drone);
            void freeDroneFromCharge(Drone drone);
            //==================
            //get objects
            //==================
            Station getStationById(int id);
            Drone getDroneById(int id);
            Customer getCustomerById(int id);
            Parcel getParcelById(int id);
            DroneCharge getDroneChargeById(int id);
            DroneCharge getDroneChargeByDroneId(int id);
            Parcel getParcelByDroneId(int droneId);

            //==================
            //display lists
            //==================
            IEnumerable<Station> displayStations();
            IEnumerable<Drone> displayDrone();
            IEnumerable<Parcel> displayParcels();
            IEnumerable<Parcel> displayFreeParcels();
            IEnumerable<DroneCharge> displayDroneCharge();
            IEnumerable<Customer> displayCustomers();
            //==================
            //change info
            //==================
            void changeStationInfo(Station s);
            void changeParcelInfo(Parcel p);
            void changeDroneInfo(int id, string newModel);
            public void changeCustomerInfo(Customer c);
            //===================
            double[] electricityUseByDrone();
        }
    }
}
