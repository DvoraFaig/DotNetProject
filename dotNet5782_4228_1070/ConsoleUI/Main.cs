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
                try
                {
                    choice = (Choices)(Convert.ToInt32(Console.ReadLine()));
                }
                catch
                {
                    choice = (Choices)(-1);
                }
                
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
                        Console.WriteLine("== ERROR ==");
                        break;
                }
            } while ((int)choice != 5);
        }

        public static void additionFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects obj;
            try
            {
                obj = (objects)(Convert.ToInt32(Console.ReadLine()));
            }
            catch
            {
                obj = (objects)(-1);
            }
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
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void UpdateFunc()
        {
            Console.WriteLine("Enter your choice to update:\n 1.DroneReceivesParcel \n 2.DroneCollectsAParcel\n 3.CostumerGetsParcel\n 4.sendDroneToCharge\n 5.freeDroneFromCharge ");
            UpdateObj choice;
            try
            {
                choice = (UpdateObj)(Convert.ToInt32(Console.ReadLine()));
            }
            catch
            {
                choice = (UpdateObj)(-1);
            }
            switch (choice)
            {
                case UpdateObj.DroneReceivesParcel:  //i worked on it
                    Customer sendingCstmr = addCustomer();
                    if (sendingCstmr.Equals(null))
                    {
                        Console.WriteLine("The service is not availble now (too much customers).\n Please try later");
                        break;
                    }
                    Parcel parcel = addParcel();
                    if (parcel.Equals(null))
                    {
                        Console.WriteLine("The service is not availble now (too much parcels).\n Please try later");

                    }
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
                            Console.WriteLine("== Drone is maintenance ==");
                        else
                            Console.WriteLine("== Drone is available ==");
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
                default:
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void ShowWithIdFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects obj;
            try
            {
                obj = (objects)(Convert.ToInt32(Console.ReadLine()));
            }
            catch
            {
                obj = (objects)(-1);
            }
            int id = new int();
            if ((int)obj > 0 && (int)obj < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                id = Convert.ToInt32(Console.ReadLine());
            }
            switch (obj)
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
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void DisplayListFunc()
        {
            Console.WriteLine("Enter your choice to display:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects obj;
            try
            {
                obj = (objects)(Convert.ToInt32(Console.ReadLine()));
            }
            catch
            {
                obj = (objects)(-1);
            }
            switch (obj)
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
                        Console.WriteLine(parcel.ToString());
                    }
                    break;
                default:
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }

        public static void addStation()
        {
            Random r = new Random();
            int amountS = DalObject.DalObject.amountStations();
            if(amountS >= 5)
            {
                Console.WriteLine("== Cann't add stations ==");
                return;
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
                Console.WriteLine("== Cann't add Drones ==");
                return;
            }
            Console.WriteLine("Enter a Model");
            string Model = Console.ReadLine();
            WeightCategories MaxWeight = (WeightCategories)(r.Next(0, 3));
            DroneStatus Status = (DroneStatus)(r.Next(0, 3));
            double Battery = 0;
            dalObject.AddDrone(amountD, Model, MaxWeight, Status, Battery);
        }
        //need sometimes to use customer's detailes - return customer
        public static Customer addCustomer()
        {
            Random r = new Random();
            Customer c = new Customer();
            int amountC = DalObject.DalObject.amountCustomers();
            if (amountC >= 100)
            {
                Console.WriteLine("== Cann't add costumers ==");
                return c;
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
        public static Parcel addParcel()
        {
            Random r = new Random();
            int id = 0;
            Parcel p = new Parcel();
            int amountP = DalObject.DalObject.amountParcels();
            if (amountP >= 1000)
            {
                Console.WriteLine("== Cann't add costumers ==");
                return p;
            }
            do
            {
                id = r.Next(1000, 10000);
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
            return p;
        }
       
    }
}



