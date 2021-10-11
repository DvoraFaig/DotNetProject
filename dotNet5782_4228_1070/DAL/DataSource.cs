using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DALObject
{
    internal class DataSource
    {
        internal static Drone[] Drones = new Drone[10];
        internal static Station[] Stations = new Station[5];
        internal static Customer[] Customers = new Customer[100];
        internal static Parcel[] Parcels = new Parcel[1000];

        public static void Initialize()
        {
            Random r = new Random();

            int amountStations = r.Next(2, 5);
            Console.WriteLine($"Enter {amountStations} stations information");
            for (int i = 0; i < amountStations; i++)
            {
                Stations[i].Id = r.Next(0, 10);
                Console.WriteLine("Enter a Latitude");
                Stations[i].Latitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a Longitude");
                Stations[i].Longitude = Convert.ToInt32(Console.ReadLine());
                Stations[i].ChargeStop = r.Next(0, 200);
                Config.indexStations++;

            }

            int amountDrones = r.Next(5,10);
            Console.WriteLine($"Enter {amountDrones} Drones information");
            for (int i=0; i< amountDrones; i++)
            {
                Drones[i].Id = r.Next(0, 10);
                Console.WriteLine("Enter a Model");
                Drones[i].Model = Console.ReadLine();
                Console.WriteLine();
                Config.indexDrones++;
            }
            

            int amountCusomer = r.Next(10, 100);
            Console.WriteLine($"Enter {amountCusomer} stations information");
            for (int i = 0; i < amountStations; i++)
            {
                Customers[i].ID = r.Next(0, 10);
                Console.WriteLine("Enter a Name");
                Customers[i].Name = Console.ReadLine();
                Console.WriteLine("Enter a phone");
                Customers[i].Phone = Console.ReadLine();
                Console.WriteLine("Enter a Latitude");
                Stations[i].Latitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a Longitude");
                Stations[i].Longitude = Convert.ToInt32(Console.ReadLine());
                Config.indexCustomers++;
            }
            

        }

        /*מערכים סטטים של ישויות הנתונים*/
        internal class Config
        {
            internal static int indexDrones = 0;
            internal static int indexStations = 0;
            internal static int indexCustomers = 0;
            internal static int indexParcels = 0;
            /*להוסיף שדה עבור יצירה של מזהה רץ עבור חבילות*/
        }

    }


}
