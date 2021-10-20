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
        internal static Drone[] Drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];
        internal static DroneCharge[] DroneCharges = new DroneCharge[10];

        public static void Initialize()
        {
            Random r = new Random();

            int amountStations = r.Next(2, 5);
            for (int i = 0; i < amountStations; i++)
            {
                Stations[i].Id = r.Next(0, 10);
                Stations[i].ChargeSlots = r.Next(0, 10);
                Stations[i].Latitude = r.Next(0, 400);
                Stations[i].Longitude = r.Next(0, 400);
                Stations[i].ChargeSlots = r.Next(0, 200);
                Config.indexStations++;

            }

            int amountDrones = r.Next(5,10);
            for (int i=0; i< amountDrones; i++)
            {
                Drones[i].Id = r.Next(0, 10);
                Drones[i].Model = $"Drone{Drones[i].Id}";
                //Console.WriteLine();
                Config.indexDrones++;
            }
            

            int amountCustomer = r.Next(10, 100);
            for (int i = 0; i < amountStations; i++)
            {
                Customers[i].ID = r.Next(0, 100);//יחודי???
                Customers[i].Name = $"Customer{Customers[i].ID}";
                Customers[i].Phone =$"{r.Next(10000000,100000000)}"; //Phonenumber of 7 digits
                Stations[i].Latitude = r.Next(0, 400);
                Stations[i].Longitude = r.Next(0, 400);
                Config.indexCustomers++;
            }

            int amountParcels = r.Next(10, 1000);
            for(int i =0; i<amountParcels; i++)
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

        /*מערכים סטטים של ישויות הנתונים*/
        internal class Config
        {
            internal static int indexDrones = 0;
            internal static int indexStations = 0;
            internal static int indexCustomers = 0;
            internal static int indexParcels = 0;
            internal static int indexDroneCharges = 0;
            /*להוסיף שדה עבור יצירה של מזהה רץ עבור חבילות*/
        }

    }


}
