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
    enum UpdateObj { DroneReceivesParcel = 1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }

    class main
    {
        Random r = new Random();
        static DalObject.DalObject dalObject;

        static void Main(string[] args)
        {
            dalObject = new DalObject.DalObject();
            Choices choice; //defaualt 0
            do
            {
                Console.WriteLine("Enter your choice to add:\n 1.Add\n 2.Update\n 3.Show object occurding to an Id\n 4.Show list of an object ");
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
            //int id;

            switch (obj)
            {
                case objects.Station:
                    addStation();
                    break;
                case objects.Drone:
                    addDrone();
                    break;
                case objects.Customer:
                    addCustomer();
                    break;
                case objects.Parcel:
                    addParcel();
                    break;
                default:
                    break;
            }
        }
        public static void UpdateFunc()
        {
            Console.WriteLine("Enter your choice to update:\n 1.DroneReceivesParcel \n 2.DroneCollectsAParcel\n 3.CostumerGetsParcel\n 4.sendDroneToCharge\n 5.freeDroneFromCharge ");
            UpdateObj choice = (UpdateObj)Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case UpdateObj.DroneReceivesParcel:
                    //if()//indexCustomer = 10
                    //{
                    //  cw(_"No Available Drones")
                    //}
                    Customer sendingCstmr;
                    Random r = new Random();
                    int id = 0;
                    do
                    {
                        id = r.Next(0, 100);
                        sendingCstmr = DalObject.DalObject.getCustomerById(id);
                    } while (!sendingCstmr.Equals(null)); //check if the id exist
                    sendingCstmr.Name =
                        sendingCstmr.Phone;

                    Console.WriteLine("Enter a parcel id");
                    int parcelId = Convert.ToInt32(Console.ReadLine());
                    Parcel parcel = new Parcel();
                    parcel = DalObject.DalObject.getParcelById(parcelId);

                    dalObject.PairAParcelWithADrone(parcel, sendingCstmr);
                    break;
                case UpdateObj.DroneCollectsAParcel:
                    int idParcelCollect = Convert.ToInt32(Console.ReadLine());
                    Parcel parcelCollect = new Parcel();
                    parcel = DalObject.DalObject.getParcelById(idParcelCollect);
                    dalObject.DroneCollectsAParcel(parcelCollect);
                    break;
                case UpdateObj.CostumerGetsParcel:
                    //dalObject.CostumerGetsParcel();
                    break;
                case UpdateObj.sendDroneToCharge:
                    int droneId = Convert.ToInt32(Console.ReadLine());
                    Drone drone = new Drone();
                    drone = DalObject.DalObject.getDroneById(droneId);
                    if (!(drone.Status == DroneStatus.Available))
                    {
                        if (drone.Status == DroneStatus.Maintenance)
                            Console.WriteLine("Drone is in maintenance");
                        else
                            Console.WriteLine("Drone is in available");
                        break;
                    }
                    dalObject.sendDroneToCharge(drone);
                    break;
                case UpdateObj.freeDroneFromCharge:
                    int droneIdCharge = Convert.ToInt32(Console.ReadLine());
                    Drone droneCharge = new Drone();
                    droneCharge = DalObject.DalObject.getDroneById(droneIdCharge);
                    dalObject.freeDroneFromCharge(droneCharge);
                    break;
            }
        }
        public static void ShowWithIdFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects choice = (objects)Convert.ToInt32(Console.ReadLine());
            int id = new int();
            if ((int)choice > 0 && (int)choice < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                id = Convert.ToInt32(Console.ReadLine());
            }
            switch (choice)
            {
                case objects.Station:
                    Station s = DalObject.DalObject.getStationById(id);
                    Console.WriteLine(s.ToString());
                    break;
                case objects.Drone:
                    Drone d = DalObject.DalObject.getDroneById(id);
                    Console.WriteLine(d.ToString());
                    break;
                case objects.Customer:
                    Customer c = DalObject.DalObject.getCustomerById(id);
                    Console.WriteLine(c.ToString());
                    break;
                case objects.Parcel:
                    Parcel p = DalObject.DalObject.getParcelById(id);
                    Console.WriteLine(p.ToString());
                    break;
                default:
                    Console.WriteLine("eroor");
                    break;
            }
        }
        public static void DisplayListFunc()
        {
            Console.WriteLine("Enter your choice to display:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects choice = (objects)Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case objects.Station:
                    IEnumerable<Station> stations = dalObject.displayStations();
                    foreach (Station station in stations)
                    {
                        if (station.Id != 0)
                        {
                            Console.WriteLine(station.ToString());
                        }
                    }
                    break;
                case objects.Drone:
                    IEnumerable<Drone> drones = dalObject.displayDrone();
                    foreach (Drone drone in drones)
                    {
                        if (drone.Id != 0)
                        {
                            Console.WriteLine(drone.ToString());
                        }
                    }
                    break;
                case objects.Customer:
                    IEnumerable<Customer> customers = dalObject.displayCustomers();
                    foreach (Customer customer in customers)
                    {
                        if (customer.ID != 0)
                        {
                            Console.WriteLine(customer.ToString());
                        }
                    }
                    break;
                case objects.Parcel:
                    IEnumerable<Parcel> parcels = dalObject.displayParcels();
                    foreach (Parcel parcel in parcels)
                    {
                        Console.WriteLine(parcels.ToString());
                    }
                    break;
                default:
                    break;
            }
        }
        public static void addStation()
        {
            Random r = new Random();
            int id = 0;
            Station s;
            do
            {
                id = r.Next(0, 5);
                s = DalObject.DalObject.getStationById(id);
            } while (!s.Equals(null)); //check if the id exist
            Console.WriteLine("Enter a station Name: ");
            string Name = Console.ReadLine();
            int ChargeSlots = r.Next(0, 5);
            Console.WriteLine("Enter a Latitude");
            int Latitude = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a Longitude");
            int Longitude = Convert.ToInt32(Console.ReadLine());
            dalObject.AddStation(id, Name, ChargeSlots, Longitude, Latitude);
        }
        public static void addDrone()
        {
            Random r = new Random();
            int id = 0;
            Drone d;
            do
            {
                id = r.Next(0, 10);
                d = DalObject.DalObject.getDroneById(id);
            } while (!d.Equals(null)); //check if the id exist
            Console.WriteLine("Enter a Model");
            string Model = Console.ReadLine();
            WeightCategories MaxWeight = (WeightCategories)(r.Next(0, 3));
            DroneStatus Status = (DroneStatus)(r.Next(0, 3));
            double Battery = 0;
            dalObject.AddDrone(id, Model, MaxWeight, Status, Battery);
        }
        public static void addCustomer()
        {
            Random r = new Random();
            int id = 0;
            Customer c;
            do
            {
                id = r.Next(0, 100);
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
        }
        public static void addParcel()
        {
            Random r = new Random();
            int id = 0;
            Parcel p;
            do
            {
                id = r.Next(0, 1000);
                p = DalObject.DalObject.getParcelById(id);
            } while (!p.Equals(null)); //check if the id exist
            Console.WriteLine("Enter the sending costumer's id: ");
            int Serderid = Convert.ToInt32(Console.ReadLine());///////
            Console.WriteLine("Enter the receiving costumer's id: ");
            int TargetId = Convert.ToInt32(Console.ReadLine());////////
            WeightCategories Weight = (WeightCategories)r.Next(0, 3);
            Priorities Priority = (Priorities)r.Next(0, 3);
            //Datetime Requeasted = (Datatime)r.Next(0, 3);
            //int DroneId=111111;////////////////;////////////////;////////////////;////////////////
            //DateTime Scheduled = DateTime.Now;
            //DateTime PickUp = DateTime.Now;////////////////;////////////////;////////////////;//////////////// 0.0.0
            //DateTime Delivered = DateTime.Now;////////////////;////////////////;////////////////;////////////////0.0.0
            dalObject.AddParcelToDelivery(id, Serderid, TargetId, Weight, Priority/*,Requeasted,DroneId,Scheduled,PickUp,Delivered*/);
        }
        public static void findIfExist(int id)
        {

        }
    }
}



