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
        interface IDal
        {
            //Add functions//
            public void AddDrone(int id, string Model, DO.WeightCategories MaxWeight, DO.DroneStatus Status, double Battery);
            public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);
            public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
            public void AddParcelToDelivery(int id, int Serderid, int TargetId, DO.WeightCategories Weight, DO.Priorities Priority, DateTime requestedTime) { }

            //Update functions//
            public string PairAParcelWithADrone(Parcel parcel);
            public void DroneCollectsAParcel(Parcel parcel);
            public void CostumerGetsParcel(Drone drone, Parcel parcel);
            public void sendDroneToCharge(Drone drone);
            public void freeDroneFromCharge(Drone drone);
            public static Station getStationById(int id) { return new Station(); }
            public static Drone getDroneById(int id) { return new Drone(); }

            public static Parcel getParcelById(int id) { return new Parcel(); }
            public static DroneCharge getDroneChargeById(int id) { return new DroneCharge(); }
            public static DroneCharge getDroneChargeByDroneId(int id) { return new DroneCharge(); }
            public IEnumerable<Station> displayStations() { yield return new Station(); }
            public IEnumerable<Drone> displayDrone() { yield return new Drone(); }
            public IEnumerable<Parcel> displayParcels() { yield return new Parcel(); }
            public IEnumerable<Parcel> displayFreeParcels() { yield return new Parcel(); }
            public IEnumerable<DroneCharge> displayDroneCharge() { yield return new DroneCharge(); }
            public IEnumerable<Customer> displayCustomers() { yield return new Customer(); }
        }
    }
}
