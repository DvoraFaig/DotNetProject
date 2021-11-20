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
    public sealed partial class BL : IBL.Ibl
    {
        static public IDal.DO.IDal dal;
        private double empty { get; set; }
        private double lightWeight { get; set; }
        private double mediumWeight { get; set; }
        private double heavyWeight { get; set; }
        private double chargingRate { get; set; }
        private BL()
        {
            dronesInBL = new List<BLDrone>();
            dal = IDal.DalFactory.factory("DalObject"); //start one time an IDal.DO.IDal object.
            empty = dal.electricityUseByDrone()[0];
            lightWeight = dal.electricityUseByDrone()[1];
            mediumWeight = dal.electricityUseByDrone()[2];
            heavyWeight = dal.electricityUseByDrone()[3];
            chargingRate = dal.electricityUseByDrone()[4];

            List<IDal.DO.Drone> drones = dal.displayDrone().Cast<IDal.DO.Drone>().ToList();
            IDal.DO.Parcel p;
            IDal.DO.Station s;
            IDal.DO.Customer sender, target;
            BLPosition senderPosition, targetPosition;
            BLDrone blDrone = new BLDrone();
            List<IDal.DO.Parcel> ppppp = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            Random r = new Random();

            drones.ForEach(d =>
            {
                blDrone = copyDalToBLDroneInfo(d);
                try
                {
                    p = dal.getParcelByDroneId(d.Id);
                    IDal.DO.Station closestStationToSender = new IDal.DO.Station();
                    sender = dal.getCustomerById(p.SenderId);
                    target = dal.getCustomerById(p.TargetId);
                    blDrone.ParcelInTransfer = createBLParcelInTransfer(p, sender, target);
                    senderPosition = blDrone.ParcelInTransfer.SenderPosition;
                    targetPosition = blDrone.ParcelInTransfer.TargetPosition;
                    //if (p.Scheduled != null && p.Delivered == null)
                    //{
                    if (p.PickUp == default(DateTime)) //position like the closest station to the sender of parcel.
                    {
                        closestStationToSender = findAvailbleAndClosestStationForDrone(senderPosition); //תחנה קרובה לשלוח במצב הטענה? //אם אינו נכנס למצב הטענה PositionFromClosestStation () //אם כן updateDroneCharge
                        blDrone.DronePosition = new BLPosition() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude };
                    }
                    else if (p.Delivered == default(DateTime)) //else position sender of parcel.
                    {
                        blDrone.DronePosition = new BLPosition() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                    }
                    blDrone.Battery = calcDroneBatteryForDroneDelivery(p, closestStationToSender, senderPosition, targetPosition);
                }
                //}
                catch (IDal.DO.DalExceptions.ObjNotExistException e) // if drone is not delivery status
                {
                    //else
                    //{
                    blDrone.Status = (DroneStatus)r.Next(0, 1); // Available / Maintenance
                    if (blDrone.Status == DroneStatus.Maintenance)
                    {
                        s = findAvailbleAndClosestStationForDrone(blDrone.DronePosition);
                        blDrone.DronePosition = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
                        blDrone.Battery = r.Next(0, 20);
                    }
                    else  //DroneStatus.Available
                    {
                        List<IDal.DO.Customer> cWithDeliveredP = findCustomersWithDeliveredParcel();
                        if (cWithDeliveredP.Count > 0)
                        {
                            target = dal.getCustomerById(cWithDeliveredP[r.Next(0, cWithDeliveredP.Count)].ID);
                            blDrone.DronePosition = new BLPosition() { Longitude = target.Longitude, Latitude = target.Latitude };
                        }
                    }
                }
                dronesInBL.Add(blDrone);
            });
        }
        private int calcDroneBatteryForDroneDelivery(IDal.DO.Parcel p, IDal.DO.Station closestStationToSender, BLPosition senderPosition, BLPosition targetPosition)
        {
            Random r = new Random();
            double disFromStationToSender = 0; // only for a parcel who wasnt picked up.
            double disFromSenderToCustomer = distance(senderPosition, targetPosition);
            //from target to closest station;
            IDal.DO.Station closestAvailbleStationFromTarget = findAvailbleAndClosestStationForDrone(targetPosition);
            double disFromTargetTostation = distance(targetPosition, new BLPosition() { Latitude = closestAvailbleStationFromTarget.Latitude, Longitude = closestAvailbleStationFromTarget.Longitude });
            if (p.PickUp == null)
            {
                disFromStationToSender = distance(new BLPosition() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude }, senderPosition);
            }
            double sumDisForDrone = disFromStationToSender + disFromSenderToCustomer + disFromTargetTostation;
            double sumBattery = sumDisForDrone * requestElectricity()[(int)p.Weight];
            return r.Next((int)sumBattery, 100);

        }
        private List<IDal.DO.Station> findAvailbleStationForDrone()
        {
            List<IDal.DO.DroneCharge> droneCharges = dal.displayDroneCharge().Cast<IDal.DO.DroneCharge>().ToList();
            List<IDal.DO.Station> stations = dal.displayStations().Cast<IDal.DO.Station>().ToList();
            List<IDal.DO.Station> availbleStations = new List<IDal.DO.Station>();
            int busyChargingSlots;
            stations.ForEach(s =>
            {
                busyChargingSlots = droneCharges.Count(droneCharge => droneCharge.StationId == s.Id);
                if (s.ChargeSlots - busyChargingSlots > 0)//has empty charging slots
                {
                    availbleStations.Add(s);
                }
            });
            return availbleStations;
        }
        private IDal.DO.Station findAvailbleAndClosestStationForDrone(BLPosition d)
        {
            List<IDal.DO.DroneCharge> droneCharges = dal.displayDroneCharge().Cast<IDal.DO.DroneCharge>().ToList();
            List<IDal.DO.Station> stations = dal.displayStations().Cast<IDal.DO.Station>().ToList();
            IDal.DO.Station availbleCLosestStation = new IDal.DO.Station();
            double dis = -1;
            double minDis = -1;
            int busyChargingSlots;
            stations.ForEach(s =>
            {
                busyChargingSlots = droneCharges.Count(droneCharge => droneCharge.StationId == s.Id);
                if (s.ChargeSlots - busyChargingSlots > 0)//has empty charging slots
                {
                    dis = distance(d, new BLPosition() { Latitude = s.Latitude, Longitude = s.Longitude });
                    if (minDis == -1)
                    {
                        minDis = dis;
                    }
                    else if (minDis > dis) ;
                    {
                        minDis = dis;
                        availbleCLosestStation = s;
                    }
                }
            });
            return availbleCLosestStation;
        }

        private List<IDal.DO.Customer> findCustomersWithDeliveredParcel()
        {
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            parcels = parcels.FindAll(p => p.Delivered != null);
            List<IDal.DO.Customer> customersWithDeliveredParcels = new List<IDal.DO.Customer>();
            parcels.ForEach(p =>
            {
                customersWithDeliveredParcels.Add(dal.getCustomerById(p.TargetId));
            });
            return customersWithDeliveredParcels;
        }

        public double[] requestElectricity()
        {
            return dal.electricityUseByDrone();
        }
        public double requestElectricity(int choice)
        {
            switch ((Electricity)choice)
            {
                case Electricity.empty:
                    return empty;
                    break;
                case Electricity.lightWeight:
                    return lightWeight;
                    break;
                case Electricity.mediumWeight:
                    return mediumWeight;
                    break;
                case Electricity.heavyWeight:
                    return chargingRate;
                    break;
                case Electricity.chargingRate:
                    return chargingRate;
                    break;
                default://///////////////////////////////////////
                    return 0;
            }
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
