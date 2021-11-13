using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using BL;
using IBL.BO;
using static IBL.BO.Exceptions;

namespace BL_ConsoleUI
{
    enum Choices { Add = 1, Update, ShowWithId, ShowList, exit }
    enum objects { Station = 1, Drone, Customer, Parcel, FreeParcel, EmptyCharges }
    enum UpdateObj { DroneReceivesParcel = 1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }

    class mainBL_ConsoleUI
    {
        Random r = new Random();
        //static DalObject.DalObject dalObject;

        /// <summary>
        /// The user chooses what to do with the objects:
        /// 1.Add 2.update 3.display obj by id 4.display arr of obj 5.exit.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //dalObject = new DalObject.DalObject();
            Choices choice; //defaualt 0
            do
            {
                Console.WriteLine("Choose action:\n 1.Add\n 2.Update\n 3.Display an object by Id\n 4.display a list of objects\n 5.Exit");
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
                        chooseObjToAdd();
                        break;
                    case Choices.Update:
                        UpdateFunc();
                        break;
                    case Choices.ShowWithId:
                        receivesAndDisplaysObjById();
                        break;
                    case Choices.ShowList:
                        receivesArrObjs();
                        break;
                    case Choices.exit:
                        return;
                    default:
                        Console.WriteLine("== ERROR ==");
                        break;
                }
                
            } while ((int)choice != 5);
        }

        public static void chooseObjToAdd()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel ");
            objects obj;
            try
            {
                obj = (objects)(Convert.ToInt32(Console.ReadLine()));
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Catch ArgumentNullException");
                obj = (objects)(-1);
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
            Console.WriteLine("Enter your choice to update:\n 1.Drone receives parcel \n 2.Drone collects a parcel\n 3.Costumer gets parcel\n 4.send drone to charge\n 5.free drone from harge ");
            UpdateBL choice;
            try
            {
                choice = (UpdateBL)(Convert.ToInt32(Console.ReadLine()));
            }
            catch
            {
                choice = (UpdateBL)(-1);
            }
            switch (UpdateBL)
            {
                case UpdateBL.DronesInfo:
                    Console.WriteLine("Enter drones' id to change its' models' name: ");
                    int droneId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a new Model");
                    string model = Console.ReadLine();
                    BL.BL.droneChangeModel(droneId, model);
                    break;
                case UpdateBL.StationInfo:
                    Console.WriteLine("Enter station id: ");
                    int stationId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter a station Name:(optional) ");
                    string sName = Console.ReadLine();
                    Console.WriteLine("Enter amount of Availble charging slots:(optional) ");
                    int chargeSlot = Convert.ToInt32(Console.ReadLine());
                    BL.BLstationChangeDetails(stationId, sName, chargeSlot);
                    break;
                case UpdateBL.CustomerInfo:
                    Console.WriteLine("Enter costumer's id: ");
                    int customerId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter costumer's Name: ");
                    string customerName = Console.ReadLine();
                    Console.WriteLine("Enter costumer's Phone: ");
                    string phone = Console.ReadLine();
                    BL.BL.customerupdateDetails(customerId, customerName, phone);
                    break;
                case UpdateBL.sendDroneToCharge:
                    Console.WriteLine("Enter drones' id that's sent to charging: ");
                    int droneIdSentToCharge = Convert.ToInt32(Console.ReadLine());
                    BL.BL.sendDroneToCharge(droneIdSentToCharge);
                    break;
                case UpdateBL.freeDroneFromCharging:
                    Console.WriteLine("Enter drones' id that's freed from charging: ");
                    int droneIdFreeFromCharging = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter time period's on charge: ");
                    int timePeriodOnCharge = Convert.ToInt32(Console.ReadLine());
                    BL.BL.freeDroneFromCharging(droneIdFreeFromCharging, timePeriodOnCharge);
                    break;
                case UpdateBL.PairParcelWithDrone:
                    Console.WriteLine("Enter drones' id to pair a parcel with: ");
                    int droneIdToPairAParcelWith = Convert.ToInt32(Console.ReadLine());
                    BL.BL.PairParcelWithDrone(droneIdToPairAParcelWith);
                    break;
                case UpdateBL.DroneCollectsAParcel:
                    Console.WriteLine("Enter drones' id that collects a parcel: ");
                    int droneIdCollectsAParcel = Convert.ToInt32(Console.ReadLine());
                    BL.BL.DroneCollectsAParcel(droneIdToPairAParcelWith);
                    break;
                case UpdateBL.DronePicksUpParcel:
                    Console.WriteLine("Enter drones' id that pickes up a parcel: ");
                    int droneIdPickesUpAParcel = Convert.ToInt32(Console.ReadLine());
                    BL.BL.DronePicksUpParcel(droneIdPickesUpAParcel);
                    break;
                case UpdateBL.DeliveryParcelByDrone:
                    Console.WriteLine("Enter drones' id that deliverd a parcel: ");
                    int droneIdDeliveredAParcel = Convert.ToInt32(Console.ReadLine());
                    BL.BL.DeliveryParcelByDrone(droneIdDeliveredAParcel);
                    break;
                default:
                    break;
            }

            switch (choice)
            {
                case UpdateObj:
                   
                case UpdateObj.DroneCollectsAParcel:
                    //Console.WriteLine("Enter ID of parcel");
                    //int parcelIdToCollect = Convert.ToInt32(Console.ReadLine());
                    //Parcel parcelCollect = DalObject.DalObject.getParcelById(parcelIdToCollect);
                    //dalObject.DroneCollectsAParcel(parcelCollect);
                    break;
                case UpdateObj.CostumerGetsParcel:
                    //Console.WriteLine("Enter ID of parcel");
                    //int parcelIdGet = Convert.ToInt32(Console.ReadLine());
                    //Parcel parcelGet = DalObject.DalObject.getParcelById(parcelIdGet);
                    //Drone drone = DalObject.DalObject.getDroneById(parcelGet.DroneId);
                    //dalObject.CostumerGetsParcel(drone, parcelGet);
                    break;
                case UpdateObj.sendDroneToCharge:
                    //Console.WriteLine("Enter ID of drone");
                    //int droneId = Convert.ToInt32(Console.ReadLine());
                    //Drone droneToCharge = DalObject.DalObject.getDroneById(droneId);
                    //switch (droneToCharge.Status)
                    //{
                    //    case DroneStatus.Maintenance:
                    //        Console.WriteLine("== Drone is maintenance ==");
                    //        break;
                    //    case DroneStatus.Delivery:
                    //        Console.WriteLine("== Drone in Deliver ==");
                    //        break;
                    //    case DroneStatus.Available:
                    //        dalObject.sendDroneToCharge(droneToCharge);
                    //        break;
                    //}
                    break;
                case UpdateObj.freeDroneFromCharge:
                    //Console.WriteLine("Enter Id of drone to charge");
                    //int droneIdCharged = Convert.ToInt32(Console.ReadLine());
                    //Drone droneToFree = DalObject.DalObject.getDroneById(droneIdCharged);
                    //if (droneToFree.Status == DroneStatus.Maintenance)
                    //{
                    //    dalObject.freeDroneFromCharge(droneToFree);
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Error: Can't free Drone from Charge station.\nDrone is not on charging");
                    //}
                    break;
                default:
                    //Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void receivesAndDisplaysObjById()
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
            int id = new int();
            if ((int)obj > 0 && (int)obj < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                id = Convert.ToInt32(Console.ReadLine());
            }
            switch (obj)
            {
                case objects.Station:
                    //Station s = DalObject.DalObject.getStationById(id);
                    //Console.WriteLine(s.ToString());
                    break;
                case objects.Drone:
                    //Drone d = DalObject.DalObject.getDroneById(id);
                    //Console.WriteLine(d.ToString());
                    break;
                case objects.Customer:
                    //Customer c = DalObject.DalObject.getCustomerById(id);
                    //Console.WriteLine(c.ToString());
                    break;
                case objects.Parcel:
                    //Parcel p = DalObject.DalObject.getParcelById(id);
                    //Console.WriteLine(p.ToString());
                    break;
                default:
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void displayArrObjs<T>(T[] arrObjects)
        {
            foreach (T obj in arrObjects)
            {
                Console.WriteLine(obj.ToString());
            }
        }
        /// <summary>
        /// Receives an array the objects. (according to a switch)
        /// and send the array to displayArrObjs.
        /// </summary>
        public static void receivesArrObjs()
        {
            Console.WriteLine("Enter your choice to display:\n 1.Station \n 2.Drone\n 3.CLient\n 4.Parcel\n 5.Parcels Without drone\n 6.Station with empty Charging positions ");
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
                    List<IBL.BO.BLStation> stations = BL.BL.displayStations();
                    stations.ForEach(s => s.ToString());
                    break;
                case objects.Drone:
                    List<IBL.BO.BLDrone> drones = BL.BL.displayDrones();
                    drones.ForEach(s => s.ToString());
                    break;
                case objects.Customer:
                    List<IBL.BO.BLCustomer> customers = BL.BL.displayCustomers();
                    customers.ForEach(s => s.ToString());
                    break;
                case objects.Parcel:
                    List<IBL.BO.BLParcel> parcels = BL.BL.displayParcel();
                    parcels.ForEach(s => s.ToString());
                    break;
                case objects.FreeParcel:
                    List<IBL.BO.BLParcel> freeParcels = BL.BL.displayFreeParcel();
                    freeParcels.ForEach(s => s.ToString());
                    break;
                case objects.EmptyCharges:
                    //IEnumerable<DroneCharge> ChargeStand = dalObject.displayDroneCharge();
                    //DroneCharge[] ChargeStandArr = ChargeStand.Cast<DroneCharge>().ToArray();
                    //displayArrObjs(ChargeStandArr);
                    break;
                default:
                    Console.WriteLine("== ERROR ==");
                    break;
            }
        }
        public static void addStation()
        {
            bool flag = true;
            do
            {
                flag = true;
                Console.WriteLine("Enter station id: ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a station Name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Enter a Latitude");
                int latitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter a Longitude");
                int longitude = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter amount of Availble charging slots: ");
                int chargeSlot = Convert.ToInt32(Console.ReadLine());
                try
                {
                    BL.BL.addStation(id, name, chargeSlot, latitude, longitude);
                }
                catch(Exception e )
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Plese try again: ");
                    flag = false;
                }
            } while (flag = false);
        }
        public static void addDrone()
        {
            Console.WriteLine("Enter the manufacturer serial number: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a Model");
            string Model = Console.ReadLine();
            Console.WriteLine("Enter max weight of drone (category) (Light =1, Medium=2, Heavy=3): ");
            int MaxWeight = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter station id for charging drone for the initial charging:");
            int stationId = Convert.ToInt32(Console.ReadLine());
            BL.BL.addDrone(id, Model, MaxWeight, stationId);
        }
        public static void addParcel()
        {
            bool flag = true;
            do
            {
                flag = true;
                Console.WriteLine("Enter sender's id: ");
                int senderID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter target's id: ");
                int targetId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter weight of parcel (Light =1, Medium=2, Heavy=3): ");
                int weight = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter priority of parcel (Regular=1, Fast=2, Emergency=3 ): ");
                int priority = Convert.ToInt32(Console.ReadLine());
                try
                {
                    BL.BL.addParcel(senderID, targetId, weight, priority);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Plese try again: ");
                    flag = false;
                }
            } while (!flag);
        }

        public static void addCustomer()
        {
            Console.WriteLine("Enter costumer's id: ");
            int id = Convert.ToInt32(Console.ReadLine()); 
            Console.WriteLine("Enter costumer's Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter costumer's Phone: ");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter a Latitude: ");
            int latitude = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a Longitude: ");
            int longitude = Convert.ToInt32(Console.ReadLine());
            BL.BL.AddCustomer(id, name, phone, longitude, latitude);
        }


        //need sometimes to use parcel's detailes - return parcel.
        //public static Parcel addParcel()
        //{
        //    Random r = new Random();
        //    int id = 0;
        //    Parcel p = new Parcel();
        //    int amountP = DalObject.DalObject.amountParcels();
        //    if (amountP >= 1000)
        //    {
        //        Console.WriteLine("== Cann't add costumers ==");
        //        return p;
        //    }
        //    do
        //    {
        //        id = r.Next(1000, 10000);
        //        p = DalObject.DalObject.getParcelById(id);
        //    } while (!p.Equals(null)); //check if the id exist
        //    int Serderid;
        //    int TargetId;
        //    do
        //    {
        //        Console.WriteLine("Enter the sending costumer's id: ");
        //        Serderid = Convert.ToInt32(Console.ReadLine());///////
        //        if (DalObject.DalObject.getCustomerById(Serderid).Equals(null))
        //        {
        //            Console.WriteLine("Error: no found customer");
        //        }
        //    } while (DalObject.DalObject.getCustomerById(Serderid).Equals(null));
        //    do
        //    {
        //        Console.WriteLine("Enter the receiving costumer's id: ");
        //        TargetId = Convert.ToInt32(Console.ReadLine());
        //        if (DalObject.DalObject.getCustomerById(Serderid).Equals(null))
        //        {
        //            Console.WriteLine("Error: no found customer");
        //        }
        //    } while (DalObject.DalObject.getCustomerById(TargetId).Equals(null));
        //    Console.WriteLine("Enter WeightCategory (1, 2 or 3):");
        //    WeightCategories Weight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
        //    Console.WriteLine("Enter priority (1, 2 or 3):");
        //    Priorities Priority = (Priorities)Convert.ToInt32(Console.ReadLine());
        //    DateTime requestedTime = DateTime.Now;
        //    dalObject.AddParcelToDelivery(id, Serderid, TargetId, Weight, Priority, requestedTime);
        //    return p;
        //}
    }


}
