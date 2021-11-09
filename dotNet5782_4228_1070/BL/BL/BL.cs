using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;
/*ChargeSlots = s.DroneChargeAvailble ????????????*/



namespace BL
{
    public sealed partial class BL : IBL.IBL
    {
        //static DalObject.DalObject d;
        static public IDal.DO.IDal dal;
        private BL()
        {
            dal = IDal.DalFactory.factory("DalObject");
            //יש לבקש משכבת הנתונים ולשמור בשדות נפרדים את צריכת החשמל ע"י
            //הרחפנים ואת קצב טעינתם -בהתאם למה שרשום לעיל

            IDal.DO.Drone[] dronesInDal = (IDal.DO.Drone[])dal.displayDrone();
            foreach (IDal.DO.Drone dr in dronesInDal)
            {
                //BL הוראות בבנאי מופע של
                //כאן יש דברים שקשורים לבטריות ושטויות אחרות. להתייחס בהמשך
                Drone d = new Drone(dr.Id, dr.Model,/*maxWeight ,Status,*/dr.Battery);

            }
        }
        //public void addStation(int id, string name, int emptyChargeSlot, double longitude, double latitude)
        //{
        //    //Station s = new Station(id, name, emptyChargeSlot, longitude, latitude);
        //    d.AddStation(id, name, emptyChargeSlot, longitude, latitude);
        //    //BLdataSource.BLStations.Add(s);
        //}
        public static void AddStation(BLStation s)
        {
            
        }
        public static void AddCustomer(BLCustomer c)
        {

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
        public static IDal.DO.Station convertBLToDalStation(BLStation s)
        {
            return new IDal.DO.Station() { Id = s.ID, Name = s.Name, ChargeSlots = s.DroneChargeAvailble + s.ChargingDrone.Count(), Longitude = s.StationPosition.Longitude , Latitude = s.StationPosition.Latitude};
        }
        public static IDal.DO.Drone convertBLToDalDrone(BLDrone d)
        {
            /*BLDeliveryInTransfer , BLPosition*/
            return new IDal.DO.Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight};
        }
        public static IDal.DO.Customer convertBLToDalCustomer(BLCustomer c)
        {
            return new IDal.DO.Customer() { ID = c.ID, Name = c.Name, Phone = c.Phone, Longitude = c.CustomerPosition.Longitude , Latitude =  c.CustomerPosition.Latitude, };
        }
        public static IDal.DO.Parcel convertBLToDalParcel(BLParcel p)
        {
            //IDal.DO.Customer sender = convertBLToDalCustomer(p.Sender);
            //IDal.DO.Customer target = convertBLToDalCustomer(p.Target);
            //IDal.DO.Drone drone = convertBLToDalDrone(p.Drone);
            return new IDal.DO.Parcel() { Id = p.Id, SenderId = p.Sender.ID, TargetId = p.Target.ID, Weight = p.Weight, Priority = p.Priority, DroneId = p.Drone.Id, Requeasted = p.Requeasted, Scheduled = p.Scheduled, PickUp = p.PickUp , Delivered = p.Delivered };
        }
        public static BLStation convertDalToBLStation(IDal.DO.Station s)
        {
            return  new BLStation() { ID = s.Id, Name = s.Name, DroneChargeAvailble = s.ChargeSlots, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude } };
        }
        public static BLCustomer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            return new BLCustomer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude } };
        }
        public static BLDrone convertDalToBLDrone(IDal.DO.Drone d)////////////////////////////////////////////
        {
            return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight /*++++++++++++++++++++*/
        };
        }
        public static BLParcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            BLCustomer sender = convertDalToBLCustomer(dal.getCustomerById(p.SenderId));
            BLCustomer target = convertDalToBLCustomer(dal.getCustomerById(p.TargetId));
            BLDrone drone = convertDalToBLDrone(dal.getDroneById(p.DroneId));
            return new BLParcel() { Id = p.Id, Sender = sender, Target = target, Weight = p.Weight, PickUp = p.PickUp, Drone = drone , Requeasted = p.Requeasted , Scheduled = p.Scheduled , /* PickUp = p.PickUp,*/ Delivered = p.Delivered};
        }
        public static BLStation getStationById(int id)
        {
            IDal.DO.Station s = dal.getStationById(id);
            BLStation BLstation = convertDalToBLStation(s);
            return BLstation;
        }
        public static BLCustomer getCustomerById(int id)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            BLCustomer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }
        public static BLDrone getDroneById(int id)
        {
            IDal.DO.Drone d = dal.getDroneById(id);
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }
        public static BLParcel getParcelById(int id)
        {
            IDal.DO.Parcel p = dal.getParcelById(id);
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
        public static List<BLStation> displayStations()
        {
            List<IDal.DO.Station> sList = dal.displayStations().Cast<IDal.DO.Station>().ToList();
            List<BLStation> arr = new List<BLStation>();
            sList.ForEach(s => arr.Add(convertDalToBLStation(s)));
            return arr;
        }
        public static List<BLCustomer> displayCustomers()
        {
            List<IDal.DO.Customer> cList = dal.displayCustomers().Cast<IDal.DO.Customer>().ToList();
            List<BLCustomer> arr = new List<BLCustomer>();
            cList.ForEach(c => arr.Add(convertDalToBLCustomer(c)));
            return arr;
        }
        public static List<BLDrone> displayDrones()
        {
            List<IDal.DO.Drone> dList = dal.displayDrone().Cast<IDal.DO.Drone>().ToList();
            List<BLDrone> arr = new List<BLDrone>();
            dList.ForEach(d => arr.Add(convertDalToBLDrone(d)));
            return arr;
        }
        public static List<BLParcel> displayParcel()
        {
            List<IDal.DO.Parcel> pList = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            List<BLParcel> arr = new List<BLParcel>();
            pList.ForEach(s => arr.Add(new BLParcel() { Id = s.Id,/* SenderId = s.SenderId, TargetId = s.TargetId,*/ PickUp=s.PickUp,Priority = s.Priority , Weight = s.Weight}));
            return arr;
        }
        public static List<BLParcel>  displayFreeParcel()
        {
            List<IDal.DO.Parcel> pList = dal.displayFreeParcels().Cast<IDal.DO.Parcel>().ToList();
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