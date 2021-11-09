using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        private List<BLDroneToList> dronesInBL;

        //static DalObject.DalObject d;
        static public IDal.DO.IDal dal;
        private BL()
        {
            dronesInBL = new List<BLDroneToList>();
            dal = IDal.DalFactory.factory("DalObject");
            /*
            //יש לבקש משכבת הנתונים ולשמור בשדות נפרדים את צריכת החשמל ע"י
            //הרחפנים ואת קצב טעינתם -בהתאם למה שרשום לעיל

            IDal.DO.Drone[] dronesInDal = (IDal.DO.Drone[])dal.displayDrone();
            foreach (IDal.DO.Drone dr in dronesInDal)
            {
                //BL הוראות בבנאי מופע של
                //כאן יש דברים שקשורים לבטריות ושטויות אחרות. להתייחס בהמשך
                Drone d = new Drone(dr.Id, dr.Model,*//*maxWeight ,Status,*//* dr.Battery);

            }*/
        }
        //public void addStation(int id, string name, int emptyChargeSlot, double longitude, double latitude)
        //{
        //    //Station s = new Station(id, name, emptyChargeSlot, longitude, latitude);
        //    d.AddStation(id, name, emptyChargeSlot, longitude, latitude);
        //    //BLdataSource.BLStations.Add(s);
        //}
        public void addStation(int id, string name, BLPosition position, int chargeSlot)
        {
            IDal.DO.Station s = new IDal.DO.Station(){ Id = id, Name = name, Latitude = position.Latitude, Longitude = position.Longitude, ChargeSlots = chargeSlot };
            dal.AddStation(s);
        }
        public void addDrone(int id, string model, IDal.DO.WeightCategories maxWeight, int stationId)
        {
            Random r = new Random();
            int battery = r.Next(20, 40);
            IDal.DO.Station s = dal.getStationById(stationId);
            BLPosition p = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
            BLDroneToList dr = new BLDroneToList() { Id = id, Model = model, MaxWeight = maxWeight, droneStatus = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
            dronesInBL.Add(dr);
            dal.AddDrone(convertBLToDalStation(dr));          
        }
        public void AddCustomer(int id, string name, string phone, BLPosition position)
        {
            IDal.DO.Customer c = new IDal.DO.Customer() { ID = id, Name = name, Phone = phone, Latitude = position.Latitude, Longitude = position.Longitude };
            dal.AddCustomer(c);
        }

        //public void addDrone(int id, WeightCategories maxWeight, int stationId)
        //{
        //    if ((d.getStationById(stationId).ChargeSlots) > 0)
        //    {
        //        Random r = new Random();
        //        int battery = r.Next(20, 40);
        //        Drone dr = new Drone(id, /*model,*/ maxWeight, DroneStatus.Maintenance, battery);
        //        BLdataSource.BLDrones.Add(dr);
        //        d.AddDrone(id, /*model, */ maxWeight,/*delete ths argument , */ battery);
        //    }
        //    else
        //    {
        //        throw new Exception("Charge slot is full. The drone was not added.");
        //    }
        //}
        //public void addCustomer(int id, string name, string phone)
        //{
        //    Customer c = new Customer(id, name, phone /*,longitude, latitude*/);
        //    BLdataSource.BLCustomers.Add(c);
        //    d.AddCustomer(id, name, phone/*,longitude, latitude*/ );
        //}

        public static List<BLStation> displayStations()
        {
            IEnumerable<IDal.DO.Station> stations = dal.displayStations();
            List<IDal.DO.Station> sList = stations.Cast<IDal.DO.Station>().ToList();
            List<BLStation> arr = new List<BLStation>();
            sList.ForEach(s => arr.Add(new BLStation() { ID = s.Id, Name = s.Name, DroneChargeAvailble = s.ChargeSlots, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude } })) ;
            return arr;
        }
        public static List<BLDrone> displayDrones()
        {
            IEnumerable<IDal.DO.Drone> d = dal.displayDrone();
            List<IDal.DO.Drone> dList = d.Cast<IDal.DO.Drone>().ToList();
            List<BLDrone> arr = new List<BLDrone>();
            //dList.ForEach(s => arr.Add(new BLDrone() { ID = s.Id, Name = s.Name, DroneChargeAvailble = s.ChargeSlots, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude } }));
            return arr;
        }
        public static List<BLCustomer> displayCustomers()
        {
            IEnumerable<IDal.DO.Customer> c = dal.displayCustomers();
            List<IDal.DO.Customer> cList = c.Cast<IDal.DO.Customer>().ToList();
            List<BLCustomer> arr = new List<BLCustomer>();
            cList.ForEach(s => arr.Add(new BLCustomer() { ID = s.ID, Name = s.Name, Phone = s.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude } }));
            return arr;
        }
        public static List<BLParcel> displayParcel()
        {
            IEnumerable<IDal.DO.Parcel> p = dal.displayParcels();
            List<IDal.DO.Parcel> pList = p.Cast<IDal.DO.Parcel>().ToList();
            List<BLParcel> arr = new List<BLParcel>();
            pList.ForEach(s => arr.Add(new BLParcel() { Id = s.Id,/* SenderId = s.SenderId, TargetId = s.TargetId,*/ PickUp=s.PickUp,Priority = s.Priority , Weight = s.Weight}));
            return arr;
        }
        public static List<BLParcel>  displayFreeParcel()
        {
            IEnumerable<IDal.DO.Parcel> p = dal.displayFreeParcels();
            List<IDal.DO.Parcel> pList = p.Cast<IDal.DO.Parcel>().ToList();
            List<BLParcel> arr = new List<BLParcel>();
            pList.ForEach(s => arr.Add(new BLParcel() { Id = s.Id,/* SenderId = s.SenderId, TargetId = s.TargetId,*/ PickUp = s.PickUp, Priority = s.Priority, Weight = s.Weight }));
            return arr;
        }
    }
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