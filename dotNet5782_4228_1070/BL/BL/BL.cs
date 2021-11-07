using System;
using System.Collections.Generic;
using System.Text;
using IBL;
//using IBL.BO;
using DalObject;
using IDal.DO;

namespace IBL
{
    namespace BO
    {
        public partial class BL : Ibl
        {
            DalObject.DalObject d;
            //BLdataSource blData = new BLdataSource();
            /*public BL()
            {
                d = new DalObject.DalObject();
                //יש לבקש משכבת הנתונים ולשמור בשדות נפרדים את צריכת החשמל ע"י
                //הרחפנים ואת קצב טעינתם -בהתאם למה שרשום לעיל

                IDal.DO.Drone[] dronesInDal = (IDal.DO.Drone[])d.displayDrone();
                foreach (IDal.DO.Drone dr in dronesInDal)
                {
                    //BL הוראות בבנאי מופע של
                    //כאן יש דברים שקשורים לבטריות ושטויות אחרות. להתייחס בהמשך
                    Drone d = new Drone(dr.Id, dr.Model,*//*maxWeight*//* ,*//*Status*//*, dr.Battery);

                }
            }*/
            public void addStation(int id, string name, int emptyChargeSlot, double longitude, double latitude)
            {

            }
           /* public void addStation(int id, string name, int emptyChargeSlot, double longitude, double latitude)
            {
                Station s = new Station(id, name, emptyChargeSlot, longitude, latitude);
                d.AddStation(s.station);
                BLdataSource.BLStations.Add(s);
            }*/
            public void addDrone(int id, WeightCategories maxWeight, int stationId)
            {
                if ((d.getStationById(stationId).ChargeSlots) > 0)
                {
                    Random r = new Random();
                    int battery = r.Next(20, 40);
                    Drone dr = new Drone(id, /*model*/, maxWeight, DroneStatus.Maintenance, battery);
                    BLdataSource.BLDrones.Add(dr);
                    d.AddDrone(id, /*model*/, maxWeight,/*delete ths argument*/ , battery);
                }
                else
                {
                    throw new Exception("Charge slot is full. The drone was not added.");
                }
            }
            public void addCustomer(int id, string name, string phone)
            {
                Customer c = new Customer(id, name, phone /*,longitude, latitude*/);
                BLdataSource.BLCustomers.Add(c);
                d.AddCustomer(id, name, phone/*,longitude, latitude*/ );
            }
            /*static DalObject.DalObject dalObject; //static - to enable it to call a non static func from a static func;



            public static void addStation()
            {
                Random r = new Random();
                int amountS = DalObject.DalObject.amountStations();
                if (amountS >= 5)
                {
                    // Find 
                    throw new InvalidOperationException("== Cann't add stations =="); 
                }
                Console.WriteLine("Enter a station Name: ");
                string Name = Console.ReadLine();
                int ChargeSlots = r.Next(0, 5);
                Console.WriteLine("Enter a Latitude");
                int Latitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a Longitude");
                int Longitude = Convert.ToInt32(Console.ReadLine());
                dalObject.AddStation(amountS, Name, ChargeSlots, Longitude, Latitude);
            }

            public static void addDrone()
            {
                Random r = new Random();
                int amountD = DalObject.DalObject.amountDrones();
                if (amountD >= 10)
                {
                    throw new InvalidOperationException("== Cann't add Drones ==");
                }
                Console.WriteLine("Enter a Model");
                string Model = Console.ReadLine();
                Console.WriteLine("Enter WeightCategory (1, 2 or 3):");
                WeightCategories MaxWeight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
                DroneStatus Status = DroneStatus.Available;
                double Battery = 100;
                dalObject.AddDrone(amountD, Model, MaxWeight, Status, Battery);
            }
            public static Customer addCustomer()
            {
                Random r = new Random();
                Customer c;
                int amountC = DalObject.DalObject.amountCustomers();
                if (amountC >= 100)
                {
                    throw new InvalidOperationException("== Cann't add costumers ==");
                }
                int id = 0;
                do
                {
                    id = r.Next(100, 1000);
                    c = DalObject.DalObject.getCustomerById(id);
                } while (!c.Equals(null)); //check if the id exist
                Console.WriteLine("Enter costumer's Name: ");
                string Name = Console.ReadLine();
                Console.WriteLine("Enter costumer's Phone: ");
                string Phone = Console.ReadLine();
                Console.WriteLine("Enter a Latitude: ");
                int Latitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a Longitude: ");
                int Longitude = Convert.ToInt32(Console.ReadLine());
                int ChargeSlots = r.Next(0, 200);
                dalObject.AddCustomer(id, Name, Phone, Longitude, Latitude);
                return c;
            }
            //need sometimes to use parcel's detailes - return parcel.
            public static Parcel addParcel()
            {
                Random r = new Random();
                int id = 0;
                Parcel p;
                int amountP = DalObject.DalObject.amountParcels();
                if (amountP >= 1000)
                {
                    throw new InvalidOperationException("== Cann't add costumers ==");
                }
                do
                {
                    id = r.Next(1000, 10000);
                    p = DalObject.DalObject.getParcelById(id);
                } while (!p.Equals(null)); //check if the id exist
                int Serderid;
                int TargetId;
                do
                {
                    Console.WriteLine("Enter the sending costumer's id: ");
                    Serderid = Convert.ToInt32(Console.ReadLine());///////
                    if (DalObject.DalObject.getCustomerById(Serderid).Equals(null))
                    {
                        Console.WriteLine("Error: no found customer");
                    }
                } while (DalObject.DalObject.getCustomerById(Serderid).Equals(null));
                do
                {
                    Console.WriteLine("Enter the receiving costumer's id: ");
                    TargetId = Convert.ToInt32(Console.ReadLine());
                    if (DalObject.DalObject.getCustomerById(Serderid).Equals(null))
                    {
                        Console.WriteLine("Error: no found customer");
                    }
                } while (DalObject.DalObject.getCustomerById(TargetId).Equals(null));
                Console.WriteLine("Enter WeightCategory (1, 2 or 3):");
                WeightCategories Weight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter priority (1, 2 or 3):");
                Priorities Priority = (Priorities)Convert.ToInt32(Console.ReadLine());
                DateTime requestedTime = DateTime.Now;
                dalObject.AddParcelToDelivery(id, Serderid, TargetId, Weight, Priority, requestedTime);
                return p;
            }*/
        }
    }
}
