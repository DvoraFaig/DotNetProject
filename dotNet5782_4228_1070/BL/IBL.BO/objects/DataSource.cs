using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace DalObject
{
    //internal class DataSource
    //{
    //    //Arrays of the classes
    //    internal static List<Drone> Drones = new List<Drone>();
    //    internal static List<Station> Stations = new List<Station>();
    //    internal static List<Customer> Customers = new List<Customer>();
    //    internal static List<Parcel> Parcels = new List<Parcel>();
    //    internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();


    //    public static void Initialize()
    //    {
    //        Random r = new Random();

    //        int amountStations = r.Next(2, 5);
    //        for (int i = 0; i < amountStations; i++)
    //        {
                
    //            int Id = i;
    //            string Name = $"station{Stations[i].Id}";
    //            int ChargeSlots = r.Next(0, 10);
    //            double Latitude = r.Next(0, 400);
    //            double  Longitude = r.Next(0, 400);
    //            Station s = new Station(Id, Name, ChargeSlots, Latitude, Longitude);
    //            Stations.Add(s);
    //        }

    //        int amountDrones = r.Next(5,10);
    //        for (int i=1; i<= amountDrones; i++)
    //        {
    //            int Id = i;
    //            string Model = $"Drone{Drones[i].Id}";
    //            WeightCategories MaxWeight = (WeightCategories)r.Next(0, 3);
    //            Model = $"Model{r.Next(0, 3)}";
    //            DroneStatus Status = (DroneStatus)r.Next(0, 3);
    //            int Battery = 100;
    //            Drone d = new Drone(Id , Model , MaxWeight , Status, Battery);
    //            Drones.Add(d);
    //        }
            
    //        int amountCustomer = r.Next(10, 100);
    //        for (int i = 1; i <= amountStations; i++)
    //        {
    //            int Id = i;
    //            string Name = $"Customer{Customers[i].ID}";
    //            string Phone =$"{r.Next(10000000,100000000)}"; //Phonenumber of 7 digits
    //            double Latitude = r.Next(0, 400);
    //            double Longitude = r.Next(0, 400);
    //            Customer c = new Customer(Id, Name,Phone , Latitude , Longitude);
    //            Customers.Add(c);

    //        }

    //        int amountParcels = r.Next(10, 1000);
    //        for(int i =1; i <= amountParcels; i++)
    //        {
    //            //Parcel p = new Parcel();
    //            int Id = r.Next(0, 1000); 
    //            int SenderId = r.Next(0, 400);
    //            int TargetId = r.Next(0, 400);// which costumer
    //            WeightCategories Weight = (WeightCategories)r.Next(0,3);
    //            Priorities Priority = (Priorities)r.Next(0,3);
    //            DateTime Requeasted =DateTime.Now;
    //            Parcel p = new Parcel(Id,SenderId,TargetId,Weight,Priority,Requeasted);
    //            Parcels.Add(p);

    //        }
    //    }

    //    internal static class Config
    //    {

            

    //    }
    //}
}
