using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

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
        internal static List<Worker> Workers;

        /// <summary>
        /// Initialize info of the program.
        /// </summary>
        public static void Initialize()
        {
            Drones = new List<Drone>();
            Stations = new List<Station>();
            Customers = new List<Customer>();
            Parcels = new List<Parcel>();
            DroneCharges = new List<DroneCharge>();
            Workers = new List<Worker>();

            Random r = new Random();

            int amountStations = r.Next(2, 5);
            for (int i = 1; i <= amountStations; i++)
            {
                Station s = new Station()
                {
                    Id = i,
                    Name = $"station{i}",
                    ChargeSlots = r.Next(5, 10),
                    Latitude = r.Next(1, 10),
                    Longitude = r.Next(1, 10)
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
                    //Id = r.Next(100000000, 1000000000),
                    Id = i,
                    Name = $"Customer{i}",
                    Phone = $"{r.Next(100000000, 1000000000)}",
                    Latitude = r.Next(1, 10),
                    Longitude = r.Next(1, 10)
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
                p.SenderId = Customers[r.Next(0, Customers.Count)].Id;
                p.TargetId = Customers[r.Next(0, Customers.Count)].Id;
                do
                {
                    p.TargetId = Customers[r.Next(0, Customers.Count)].Id;
                } while (p.SenderId == p.TargetId);
                int matchToDrone = r.Next(0, 2);
                if (matchToDrone == 0)  //find an availble drone with matching weight and no parcel is schedueld with it
                {
                    //////p.DroneId = Drones.Find(d =>
                    //////(d.MaxWeight >= p.Weight
                    //////    &&
                    //////    Parcels.Find(p => p.DroneId == d.Id).Id == 0)).Id;
                    //
                    //find avilable drone with drone.weight >= parcel.weight
                    //
                    p.DroneId = Drones.FirstOrDefault(d =>
                    (d.MaxWeight >= p.Weight
                        &&
                   !Parcels.Any(p => p.DroneId == d.Id))).Id;
                }
                if (p.DroneId > 0)
                {
                    int amountTimesToStart = r.Next(0, 3);
                    switch (amountTimesToStart)
                    {
                        case 0:
                            p.Scheduled = DateTime.Now;
                            break;
                        case 1:
                            p.Scheduled = DateTime.Now;
                            p.PickUp = DateTime.Now;
                            break;
                        case 2:
                            p.Scheduled = DateTime.Now;
                            p.PickUp = DateTime.Now;
                            p.Delivered = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                }
                Parcels.Add(p);
            }

            int amountWorkers = r.Next(2, 10);
            for (int i = 1; i <= amountStations; i++)
            {
                Workers.Add(new Worker()
                {
                    Id = i,
                    Password = $"Worker{i}",
                });
        }
        //DroneCharge
        //int indexStation;
        //List<Drone> dronesNotInDelivery = Drones.FindAll(d => 0 == Parcels.Find(p => p.DroneId == d.Id).DroneId);
        //for (int i = 0; i < dronesNotInDelivery.Count; i++)
        //{
        //    int fullChargingSlotsInStation;
        //    indexStation = r.Next(0, Stations.Count);
        //    fullChargingSlotsInStation = DroneCharges.Count(d => d.StationId == Stations[indexStation].Id);
        //    if (Stations[indexStation].ChargeSlots > fullChargingSlotsInStation) //if there is a place for another Drone to charge
        //    {
        //        DroneCharges.Add(new DroneCharge() { DroneId = dronesNotInDelivery[i].Id, StationId = Stations[indexStation].Id });
        //    }
        //}


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
        Config.empty = .1;
            Config.lightWeight = .3;
            Config.mediumWeight = .5;
            Config.heavyWeight = .6;
            Config.chargingRate = .7;
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
