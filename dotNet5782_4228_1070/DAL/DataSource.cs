using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;  

namespace DalObject
{
    internal class DataSource
    {
        //Arrays of the classes
        internal static Drone[] Drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];
        internal static DroneCharge[] DroneCharges = new DroneCharge[10];

        public static void Initialize()
        {
            Random r = new Random();

            int amountStations = r.Next(2, 5);
            for (int i = 1; i <= amountStations; i++)
            {
                Stations[i].Id = i;
                Stations[i].Name = $"station{Stations[i].Id}";
                Stations[i].Id = i;
                Stations[i].Name = $"Station{Stations[i].Id}";
                Stations[i].ChargeSlots = r.Next(0, 10);
                Stations[i].Latitude = r.Next(0, 400);
                Stations[i].Longitude = r.Next(0, 400);
                Stations[i].ChargeSlots = r.Next(0, 200);
                Config.indexStations++;

            }

            int amountDrones = r.Next(5,10);
            for (int i=1; i<= amountDrones; i++)
            {
                Drones[i].Id = i;
                Drones[i].Model = $"Drone{Drones[i].Id}";
                Drones[i].Battery = 100;
                Drones[i].MaxWeight = (WeightCategories)r.Next(0, 3);
                Drones[i].Model = $"Model{r.Next(0, 3)}";
                Drones[i].Status = (DroneStatus)r.Next(0, 3);
                Config.indexDrones++;
            }
            

            int amountCustomer = r.Next(10, 100);
            for (int i = 1; i <= amountStations; i++)
            {
                Customers[i].ID = i;//יחודי???
                Customers[i].Name = $"Customer{Customers[i].ID}";
                Customers[i].Phone =$"{r.Next(10000000,100000000)}"; //Phonenumber of 7 digits
                Stations[i].Latitude = r.Next(0, 400);
                Stations[i].Longitude = r.Next(0, 400);
                Config.indexCustomers++;
            }

            int amountParcels = r.Next(10, 1000);
            for(int i =1; i <= amountParcels; i++)
            {
                Parcels[i].Id = r.Next(0, 1000); //יחודי???
                Parcels[i].SenderId = r.Next(0, 400);
                Parcels[i].TargetId = r.Next(0, 400);// which costumer
                Parcels[i].Weight = (WeightCategories)r.Next(0,3);
                Parcels[i].Priority = (Priorities)r.Next(0,3);
                //Parcels[i].Requeasted =(DateTime)r.Next(0,3);
                //Parcels[i].DroneId;
                Parcels[i].Scheduled = DateTime.Now;
                //Parcels[i].PickUp ;
                //Parcels[i].Delivered ;
            }
            /////////////////DroneCharges
        }

        internal static class Config
        {
            internal static int indexDrones = -1;
            internal static int indexStations = -1;
            internal static int indexCustomers = -1;
            internal static int indexParcels = -1;
            internal static int indexDroneCharges = -1;
            /*להוסיף שדה עבור יצירה של מזהה רץ עבור חבילות*/
        }

    }


}
