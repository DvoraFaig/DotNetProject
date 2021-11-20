using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

namespace DalExceptions
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
            for (int i = 1; i <= amountStations; i++)
            {
                Station s = new Station()
                {
                    Id = i,
                    Name = $"station{i}",
                    ChargeSlots = r.Next(1, 10),
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
                    MaxWeight = (WeightCategories)r.Next(1, 4)
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

            int amountParcels = r.Next(1, 5);
            for (int i = 1; i <= amountParcels; i++)
            {
                Parcel p = new Parcel()
                {
                    Id = i,
                    Weight = (WeightCategories)r.Next(1, 4),
                    Priority = (Priorities)r.Next(1, 4),
                    Requeasted = DateTime.Now
                };
                p.SenderId = Customers[r.Next(0, Customers.Count)].ID;
                p.TargetId = Customers[r.Next(0, Customers.Count)].ID;
                do
                {
                    p.TargetId = Customers[r.Next(0, Customers.Count)].ID;
                } while (p.SenderId == p.TargetId);
                int matchToDrone = r.Next(0, 2);
                if (matchToDrone == 0)  //find an availble drone with matching weight and no parcel is schedueld with it
                {
                    p.DroneId = Drones.Find(d =>
                    (d.MaxWeight >= p.Weight
                        &&
                        Parcels.Find(p => p.DroneId == d.Id).Id == 0)).Id;
                    Parcels.Add(p);
                }
            }
            //DroneCharge
            //List<Drone> dronesNotInDelivery = Drones.FindAll(d => 0 == Parcels.Find(p => p.DroneId == d.Id).DroneId);
            //int droneIndex = 0;
            //for (int i = 0; i < Stations.Count; i++)
            //{
            //    if (droneIndex == dronesNotInDelivery.Count)
            //        break;
            //    for (int j = 0; j < Stations[i].ChargeSlots; j++)
            //    {
            //        if (droneIndex == dronesNotInDelivery.Count)
            //            break;
            //        DroneCharges.Add(new DroneCharge() { StationId = Stations[i].Id, DroneId = dronesNotInDelivery[droneIndex].Id });
            //        droneIndex++;
                    
            //    }
            //}
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
