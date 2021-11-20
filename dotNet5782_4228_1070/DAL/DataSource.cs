using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

namespace DalObject
{
    internal static class DataSource
    {
        //Arrays of the classes
        internal static List<Drone> Drones;
        internal static List<Station> Stations;
        internal static List<Customer> Customers;
        internal static List<Parcel> Parcels;
        internal static List<DroneCharge> DroneCharges;


        public static void Initialize()
        {
            Drones = new List<Drone>();
            Stations = new List<Station>();
            Customers = new List<Customer>();
            Parcels = new List<Parcel>();
            DroneCharges = new List<DroneCharge>();

            Random r = new Random();

            int amountStations = r.Next(2, 5);
            for (int i = 0; i < amountStations; i++)
            {
                Station s = new Station()
                {
                    Id = i,
                    Name = $"station{i}",
                    ChargeSlots = r.Next(0, 10),
                    Latitude = r.Next(0, 400),
                    Longitude = r.Next(0, 400)
                };
                Stations.Add(s);
            }

            int amountDrones = r.Next(5, 10);
            for (int i = 1; i <= amountDrones; i++)
            {
                Drone d = new Drone()
                {
                    Id = i,
                    Model = $"Drone{i}",
                    MaxWeight = (WeightCategories)r.Next(0, 3)
                };
                Drones.Add(d);
            }

            int amountCustomer = r.Next(10, 100);
            for (int i = 1; i <= amountStations; i++)
            {
                Customer c = new Customer()
                {
                    ID = i,
                    Name = $"Customer{i}",
                    Phone = $"{r.Next(10000000, 100000000)}", //Phonenumber of 7 digits
                    Latitude = r.Next(0, 400),
                    Longitude = r.Next(0, 400)
                };
                Customers.Add(c);
            }

            int amountParcels = r.Next(10, 1000);
            for (int i = 1; i <= amountParcels; i++)
            {
                Parcel p = new Parcel()
                {
                    Id = r.Next(0, 1000),
                    SenderId = r.Next(0, 400),
                    TargetId = r.Next(0, 400),// which costumer
                    Weight = (WeightCategories)r.Next(0, 3),
                    Priority = (Priorities)r.Next(0, 3),
                    Requeasted = DateTime.Now
                };
                Parcels.Add(p);
            }
        }
        internal static class Config
        {
            internal static double empty;
            internal static double lightWeight;
            internal static double mediumWeight;
            internal static double heavyWeight;
            internal static double chargingRate;
        }
    }
}
