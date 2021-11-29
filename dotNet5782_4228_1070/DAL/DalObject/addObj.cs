﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;





namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public void AddDrone(int id, string Model, IDal.DO.WeightCategories MaxWeight)
        {
            Drone drone = new Drone();
            drone.Id = id;
            drone.Model = Model;
            drone.MaxWeight = MaxWeight;
            DataSource.Drones.Add(drone);
        }
        public void AddDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }
        public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
        {
            Customer customer = new Customer();
            customer.ID = id;
            customer.Name = Name;
            customer.Phone = Phone;
            customer.Longitude = Longitude;
            customer.Latitude = Latitude;
            DataSource.Customers.Add(customer);
        }
        public void AddCustomer(Customer customer)
        {
            DataSource.Customers.Add(customer);
        }
        public void AddDroneCharge(DroneCharge parcel)
        {
            DataSource.DroneCharges.Add(parcel);
        }
        public void AddStation(Station s)
        {
            DataSource.Stations.Add(s);
        }
        public void AddParcelToDelivery(int id, int Serderid, int TargetId, IDal.DO.WeightCategories Weight, IDal.DO.Priorities Priority, DateTime requestedTime)
        {
            Parcel parcel = new Parcel();
            parcel.Id = id;
            parcel.SenderId = Serderid;
            parcel.TargetId = TargetId;
            parcel.Weight = Weight;
            parcel.Priority = Priority;
            parcel.Requeasted = requestedTime;
            DataSource.Parcels.Add(parcel);
        }
        public void AddParcel(Parcel parcel)
        {
            DataSource.Parcels.Add(parcel);
        }

    }
}