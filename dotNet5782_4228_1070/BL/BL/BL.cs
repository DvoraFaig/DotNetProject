using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;
using static IBL.BO.Exceptions;
/*ChargeSlots = s.DroneChargeAvailble ????????????*/



namespace BL
{
    public sealed partial class BL //: IBL.Ibl
    {
        private List<BLDrone> dronesInBL { get; set; }
        //static DalObject.DalObject d;
        static public IDal.DO.IDal dal;

        private BL()
        {
            dronesInBL = new List<BLDrone>();
            dal = IDal.DalFactory.factory("DalObject"); //start one time an IDal.DO.IDal object.
            Random r = new Random();
            List<IDal.DO.Drone> drones = dal.displayDrone().Cast<IDal.DO.Drone>().ToList();
            List<IDal.DO.Parcel> parcels = dal.displayDrone().Cast<IDal.DO.Parcel>().ToList();
            BLDrone BLd;
            drones.ForEach(d =>
            {
                BLd = convertDalToBLDrone(d);

                parcels.ForEach(p =>
                {
                    if (!p.Scheduled.Equals(null) && p.Delivered.Equals(null))// pair a parcel to drone but not yed delivered.
                    {
                        IDal.DO.Customer c;
                        if (p.PickUp.Equals(null)) //if parcel wasn't picked up
                        {
                            c = dal.getCustomerById(p.SenderId);
                        }
                        else //if parcel was picked up but wasn't delivered
                        {
                            c = dal.getCustomerById(p.TargetId);
                        }
                        BLd.DronePosition = new BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude };
                        BLd.Battery = r.Next(20, 100); // מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה
                    }
                });
                if ((parcels.Find(p => p.DroneId == d.Id)).Equals(null)) //if the drone is not requested to a parcel - 
                {
                    BLd.Status = (DroneStatus)r.Next(0, 2);
                }
                //if(d.Status == DroneStatus.Maintenance) // doesn't have this by the dal
                //{
                //     //להגריל מספ תחנת בסיס 
                //    d.Battery = r.Next(0,20)
                //}
                //if(d.Status == DroneStatus.Availble) // doesn't have this by the dal
                //{
                //    
                //}
            });
            //add drones to BLDrone arr

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
        public double[] requestElectricity()
        {
            return dal.electricityUseByDrone();
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
