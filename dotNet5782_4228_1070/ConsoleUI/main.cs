using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;

namespace DAL
{
    enum choices { Add = 1, Update, ShowWithId, ShowList }
    enum objects { Station = 1, Drone, Customer, Parcel }
    enum UpdateObj { DroneReceivesParcel=1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }

    Random r = new Random();

    class main
    {
        Random r = new Random();
        public void additionFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            int choice = Console.WriteLine();
            switch (choice)
            {
                case objects.Station:
                    int id = r.Next(0, 5);
                    Console.WriteLine("Enter a station Name: ");
                    string Name = Console.ReadLine();
                    int ChargeSlots = r.Next(0, 5)
                    Console.WriteLine("Enter a Latitude");
                    int Latitude = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a Longitude");
                    int Longitude = Convert.ToInt32(Console.ReadLine());
                    int ChargeSlots = r.Next(0, 200);
                    DalObject.DalObject.AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude);
                    break;
                case objects.Drone:
                    int id = r.Next(0, 10);
                    Console.WriteLine("Enter a Model");
                    string Model = Console.ReadLine();
                    WeightCategories MaxWeight = (WeightCategories)(r.Next(0, 3));
                    DroneStatus Status = (DroneStatus)(r.Next(0, 3));
                    double Battery = 0;
                    DalObject.DalObject.AddDrone(id, Model, MaxWeight, Status, Battery);
                    break;
                case objects.Customer:
                    int id = r.Next(0, 5);
                    Console.WriteLine("Enter costumer's Name: ");
                    string Name = Console.ReadLine();
                    Console.WriteLine("Enter costumer's Phone: ");
                    int Phone = Convert.ToInt64(Console.ReadLine());
                    Console.WriteLine("Enter a Latitude: ");
                    int Latitude = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a Longitude: ");
                    int Longitude = Convert.ToInt32(Console.ReadLine());
                    int ChargeSlots = r.Next(0, 200);
                    DalObject.DalObject.AddStation(int id, string Name, int Phone, double Longitude, double Latitude)
                    break;
                case objects.Parcel:
                    int id 
                    Console.WriteLine("Enter the sending costumer's id: ");
                    int Serderid =Convert.ToInt32(Console.ReadLine())///////
                    Console.WriteLine("Enter the receiving costumer's id: ");
                    int TargetId =Convert.ToInt32(Console.ReadLine())////////
                    WeightCategories Weight = (WeightCategories)r.Next(0,3);
                    Priorities Priority = (Priorities)r.Next(0,3);
                    Datatime Requeasted =(Datatime)r.Next(0,3);
                    //int DroneId;
                    DateTime Scheduled = DateTime.Now;
                    //DateTime PickUp ;
                    //DateTime Delivered ;
                    AddParcelToDelivery(id,Serderid,TargetId,Weight,Priority,Requeasted,DroneId,Scheduled,PickUp,Delivered);
                    break;
                default:
                    break;
            }
        }
        public void UpdateFunc()
        {
            Console.WriteLine("Enter your choice to update:\n 1.DroneReceivesParcel \n2.DroneCollectsAParcel\n 3.CostumerGetsParcel\n 4.sendDroneToCharge\n 5.freeDroneFromCharge ");
            int choice = Console.WriteLine();
            switch(choice)
            {
            case UpdateObj.DroneReceivesParcel:
                    DalObject.DalObject.DroneReceivesParcel();
                break;
            case UpdateObj.DroneCollectsAParcel:
                    DalObject.DalObject.DroneCollectsAParcel();
                break;
            case UpdateObj.CostumerGetsParcel:
                    DalObject.DalObject.CostumerGetsParcel();
                break;
            case UpdateObj.sendDroneToCharge:
                DalObject.DalObject.sendDroneToCharge();
                break;
            case UpdateObj.freeDroneFromCharge:
                    DalObject.DalObject.freeDroneFromCharge();
                break;
    }


        }
        public void ShowWithIdFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            int choice = Console.WriteLine();
            if (choice > 0 && choice < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                int id = Console.ReadLine();
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
        public void DisplayListFunc() 
        {
            Console.WriteLine("Enter your choice to display:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            int choice = Console.WriteLine();
            switch (choice)
            {
                case objects.Station:
                    DalObject.DalObject.displayStations();
                    break;
                case objects.Drone:
                    DalObject.DalObject.displayDrone();
                    break;
                case objects.Customer:
                    DalObject.DalObject.displayCustomers();
                    break;
                case objects.Parcel:
                    DalObject.DalObject.displayParcels();
                    break;
                default:
                    break;
            }
        }

        public void nav()
        {
            Console.WriteLine("Enter your choice");
            int choice = Console.ReadLine();
            do
            {
                Console.WriteLine("Enter your choice to add:\n 1.Add \n2.Update\n 3.Show object occurding to an Id\n 4.Show list of an object ");
                int choice = Console.WriteLine();
                switch (choices)
                {
                    case choices.Add:
                        additionFunc();
                        break;
                    case choices.Update:
                        UpdateFunc();
                        break;
                    case choices.ShowWithId:
                        ShowWithIdFunc();
                        break;
                    case choices.ShowList:
                        DisplayListFunc();
                        break;
                    default:
                        if (choice != 5)
                            Console.WriteLine("Error");
                        break;
                }
            } while (choice != 5);
        }
    }
}



