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
            public void AddDrone(Drone d);
            public void AddDrone(int id, string Model, DO.WeightCategories MaxWeight, DO.DroneStatus Status, double Battery);
            public void AddStation(Station s);
            public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);          
            public void AddCustomer(Customer c);
            public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
            public void AddParcelToDelivery(int id, int Serderid, int TargetId, DO.WeightCategories Weight, DO.Priorities Priority, DateTime requestedTime) { }
            public void AddParcelToDelivery(Parcel parcel);

            //Update functions//
            public string PairAParcelWithADrone(Parcel parcel);
            public void DroneCollectsAParcel(Parcel parcel);
            public void CostumerGetsParcel(Drone drone, Parcel parcel);
            public void sendDroneToCharge(Drone drone);
            public void freeDroneFromCharge(Drone drone);
            public Station getStationById(int id);
            public Drone getDroneById(int id);
            public Customer getCustomerById(int id);


            public Parcel getParcelById(int id);
            public DroneCharge getDroneChargeById(int id);
            public DroneCharge getDroneChargeByDroneId(int id);
            public IEnumerable<Station> displayStations();
            public IEnumerable<Drone> displayDrone();
            public IEnumerable<Parcel> displayParcels();
            public IEnumerable<Parcel> displayFreeParcels();
            public IEnumerable<DroneCharge> displayDroneCharge();
            public IEnumerable<Customer> displayCustomers();

            //בקשת צריכת חשמל
            public double[] electricityUseByDrone(Drone drone);
            ///
            public Parcel getParcelByDroneId(int droneId);

        }
    }
}
