﻿using System;
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
            DroneCharge getDroneChargeByStationId(int id);
            DroneCharge getDroneChargeByDroneId(int id);
            Parcel getParcelByDroneId(int droneId);

            //==================
            //display lists
            //==================
            IEnumerable<Station> displayStations();
            IEnumerable<Drone> displayDrone();
            IEnumerable<Parcel> displayParcels();
            IEnumerable<DroneCharge> displayDroneCharge();
            IEnumerable<Customer> displayCustomers();
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
        }
    }
}
