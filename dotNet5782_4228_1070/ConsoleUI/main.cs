using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;


namespace DAL
{
    enum Choices { Add = 1, Update, ShowWithId, ShowList }
    enum objects { Station = 1, Drone, Customer, Parcel }
    enum UpdateObj { DroneReceivesParcel=1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }
   
    class main
    {
        Random r = new Random();
        //int choice;
        static DalObject.DalObject dalObject = new DalObject.DalObject();

        static void Main(string[] args)
        {
            nav();
        }
        public static void nav()
        {
            //Console.WriteLine("Enter your choice");
            //choices choice = (objects)Convert.ToInt32(Console.ReadLine());
            Choices choice; //defaualt 0
            do
            {
                Console.WriteLine("Enter your choice to add:\n 1.Add \n2.Update\n 3.Show object occurding to an Id\n 4.Show list of an object ");
                choice = (Choices)Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case Choices.Add:
                        additionFunc();
                        break;
                    case Choices.Update:
                        UpdateFunc();
                        break;
                    case Choices.ShowWithId:
                        ShowWithIdFunc();
                        break;
                    case Choices.ShowList:
                        DisplayListFunc();
                        break;
                    default:
                        if ((int)choice != 5)
                            Console.WriteLine("Error");
                        break;
                }
            } while ((int)choice != 5);
        }

        
        public static void additionFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects obj = (objects)Convert.ToInt32(Console.ReadLine());
            Random r = new Random();

     
            switch (obj)
            {
                case objects.Station:
                    int id = r.Next(0, 5);
                    Console.WriteLine("Enter a station Name: ");
                    string Name = Console.ReadLine();
                    int ChargeSlots = r.Next(0, 5);
                    Console.WriteLine("Enter a Latitude");
                    int Latitude = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a Longitude");
                    int Longitude = Convert.ToInt32(Console.ReadLine());
                    dalObject.AddStation( id, Name, ChargeSlots,Longitude, Latitude);
                    break;
                case objects.Drone:
                    id = r.Next(0, 10);
                    Console.WriteLine("Enter a Model");
                    string Model = Console.ReadLine();
                    WeightCategories MaxWeight = (WeightCategories)(r.Next(0, 3));
                    DroneStatus Status = (DroneStatus)(r.Next(0, 3));
                    double Battery = 0;
                    dalObject.AddDrone(id, Model, MaxWeight, Status, Battery);
                    break;
                case objects.Customer:
                    id = r.Next(0, 5);
                    Console.WriteLine("Enter costumer's Name: ");
                    Name = Console.ReadLine();
                    Console.WriteLine("Enter costumer's Phone: ");
                    int Phone = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a Latitude: ");
                    Latitude = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a Longitude: ");
                    Longitude = Convert.ToInt32(Console.ReadLine());
                    ChargeSlots = r.Next(0, 200);
                    dalObject.AddStation(id, Name, Phone, Longitude, Latitude);
                    break;
                case objects.Parcel:
                    id = 0000000000000000000; //יחודי???
                    Console.WriteLine("Enter the sending costumer's id: ");
                    int Serderid = Convert.ToInt32(Console.ReadLine());///////
                    Console.WriteLine("Enter the receiving costumer's id: ");
                    int TargetId = Convert.ToInt32(Console.ReadLine());////////
                    WeightCategories Weight = (WeightCategories)r.Next(0,3);
                    Priorities Priority = (Priorities)r.Next(0,3);
                    Datatime Requeasted =(Datatime)r.Next(0,3);
                    int DroneId=111111;////////////////;////////////////;////////////////;////////////////
                    DateTime Scheduled = DateTime.Now;
                    DateTime PickUp = DateTime.Now;////////////////;////////////////;////////////////;//////////////// 0.0.0
                    DateTime Delivered = DateTime.Now;////////////////;////////////////;////////////////;////////////////0.0.0
                    dalObject.AddParcelToDelivery(id,Serderid,TargetId,Weight,Priority,Requeasted,DroneId,Scheduled,PickUp,Delivered);
                    break;
                default:
                    break;
            }
        }
        public static void UpdateFunc()
        {
            Console.WriteLine("Enter your choice to update:\n 1.DroneReceivesParcel \n2.DroneCollectsAParcel\n 3.CostumerGetsParcel\n 4.sendDroneToCharge\n 5.freeDroneFromCharge ");
            UpdateObj choice = (UpdateObj)Convert.ToInt32(Console.ReadLine());
            //switch (choice)
            //{
            //case UpdateObj.DroneReceivesParcel:
            //    dalObject.DroneReceivesParcel();
            //    break;
            //case UpdateObj.DroneCollectsAParcel:
            //    dalObject.DroneCollectsAParcel();
            //    break;
            //case UpdateObj.CostumerGetsParcel:
            //    dalObject.CostumerGetsParcel();
            //    break;
            //case UpdateObj.sendDroneToCharge:
            //    dalObject.sendDroneToCharge();
            //    break;
            //case UpdateObj.freeDroneFromCharge:
            //    dalObject.freeDroneFromCharge();
            //    break;
            //}


        }
        public static void ShowWithIdFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            objects choice = (objects)Convert.ToInt32(Console.ReadLine());
            if ((int)choice > 0 && (int)choice < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                int id = Convert.ToInt32(Console.ReadLine());
            }
            switch (choice)
            {
                case objects.Station:
                    break;
                case objects.Drone:
                    break;
                case objects.Customer:
                    break;
                case objects.Parcel:
                    break;
                default:
                    Console.WriteLine("eroor");
                    break;
            }
        }
        public static void DisplayListFunc() 
        {
            Console.WriteLine("Enter your choice to display:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            objects choice = (objects)Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case objects.Station:
                    Console.WriteLine( "IEnumerable<Station>");
                    IEnumerable<Station> stations = dalObject.displayStations();
                    //foreach (var station in stations)
                    //{
                    //    Console.WriteLine("dsfgh");
                    //}
                    Console.WriteLine("stations..............");
                    break;
                case objects.Drone:
                    IEnumerable<Drone> drones = dalObject.displayDrone();
                    break;
                case objects.Customer:
                    IEnumerable<Customer>customers = dalObject.displayCustomers();
                    break;
                case objects.Parcel:
                    IEnumerable<Parcel> parcel = dalObject.displayParcels();
                    break;
                default:
                    break;
            }
        }

        
    }
}



