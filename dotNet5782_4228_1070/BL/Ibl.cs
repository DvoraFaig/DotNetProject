using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;
using DalObject;

namespace IBL
{
    interface IBL
    {
        ////Add functions//
        //public void AddDrone(int id, string Model, WeightCategories MaxWeight, BO.DroneStatus Status, double Battery);
        //public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);
        //public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
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

    }
}
