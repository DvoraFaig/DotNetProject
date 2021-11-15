﻿using System;
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
        private List<BLDrone> dronesInBL;
        //static DalObject.DalObject d;
        static public IDal.DO.IDal dal;

        private BL()
        {
            dronesInBL = new List<BLDrone>();
            dal = IDal.DalFactory.factory("DalObject");
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
                if((parcels.Find(p => p.DroneId == d.Id)).Equals(null)) //if the drone is not requested to a parcel - 
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
        public double requestElectricity()
        {
            return new double;
        }
        //==================================
        // Add
        //==================================
        public void addStation(int id, string name, BLPosition position, int chargeSlot)
        {
            IDal.DO.Station s = new IDal.DO.Station() { Id = id, Name = name, Latitude = position.Latitude, Longitude = position.Longitude, ChargeSlots = chargeSlot };
            dal.AddStation(s);
        }
        public void addDrone(int id, string model, IDal.DO.WeightCategories maxWeight, int stationId)
        {
            Random r = new Random();
            int battery = r.Next(20, 40);
            IDal.DO.Station s = dal.getStationById(stationId);
            BLPosition p = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
            BLDrone dr = new BLDrone() { Id = id, Model = model, MaxWeight = maxWeight, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
            dronesInBL.Add(dr);
            dal.AddDrone(convertBLToDalDrone(dr));
        }
        public void AddCustomer(int id, string name, string phone, BLPosition position)
        {
            IDal.DO.Customer c = new IDal.DO.Customer() { ID = id, Name = name, Phone = phone, Latitude = position.Latitude, Longitude = position.Longitude };
            dal.AddCustomer(c);
        }
        //==================================
        // Updates
        //==================================
        public void droneChangeModel(int id, string newModel)
        {
            BLDrone d = dronesInBL.Find(drone => drone.Id == id);
            _ = !d.Equals(null) ? d.Model = newModel : throw new Exception($"ERROR: Drone {id} not found");
            IDal.DO.Drone dr = dal.getDroneById(id);
            dr.Model = newModel;
            //check if it valid to do it. (the changing of dal - here)
        }
        public void stationChangeDetails(int id, string name = null, int chargeStand = -1)//-1 is defualt value
        {

        }
        public void customerupdateDetails(int id, string name = null, string phone = null)
        {

        }
        public void sendDroneToCharge(int droneId)
        {
            BLDrone drone = dronesInBL.Find(d => d.Id == droneId);
            if (drone.droneStatus == DroneStatus.Available)
            {

            }
            else
            {
                // Throw match exception - "drone not available to charge"
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void updateFreeDroneFromeCharge(int droneId , double time)
        {
            BLDrone blDrone;
            try
            {
                dronesInBL.First(d => d.Id == droneId && d.Status == DroneStatus.Maintenance);
            }
            catch (Exception e)
            {
                throw new Exceptions.ObjNotExistException<BLDrone>(blDrone);
            }

            blDrone.Status = DroneStatus.Available;
            blDrone.Battery += (double)time * requestElectricity(); //[4]
            IDal.DO.DroneCharge droneChargeByStation = dal.getDroneChargeByDroneId(blDrone.Id);
            IDal.DO.Station s = dal.getStationById(droneChargeByStation.StationId);
            s.ChargeSlots++;
            stationChangeDetails(s.Id, null,s.ChargeSlots);
            //dal.removeDroneFromCharge();
        }
        public void ScheduledParcelToDrone(int droneId)
        {
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            IDal.DO.Customer senderMaxParcel;
            BLDrone droneToParcel = GetBLDroneById(droneId);
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            IDal.DO.Parcel maxParcel = new IDal.DO.Parcel() { Weight = 0 , };//parcels.First(); //check if weght is good=====================
            foreach (IDal.DO.Parcel p in parcels)
            {
                if (p.Requeasted.Equals(null))
                    break;
                senderP = dal.getCustomerById(p.SenderId);
                targetP = dal.getCustomerById(p.TargetId);
                BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                BLPosition targetPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                senderMaxParcel = dal.getCustomerById(maxParcel.SenderId);
                double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                double disSenderToTarget = distance(senderPosition, targetPosition);
                IDal.DO.Station MinDisFromTargetTostation =  getMinDistanceFromStation(targetPosition);
                double disTargetToStation = distance(targetPosition, new BLPosition() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude })
                double disMaxPToSender = distance(droneToParcel.DronePosition, new BLPosition() { Longitude = senderMaxParcel.Longitude, Latitude = senderMaxParcel.Latitude });
                double droneElectricity = requestElectricity()[(IDal.DO.WeightCategories)p.Weight;
                if (droneToParcel.Battery - (int)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity) >0) //[4]
                {
                    //BLParcel bLParcel = convertDalToBLParcel(p);
                    if (p.Weight <= droneToParcel.MaxWeight)
                    {
                        if (maxParcel.Priority < p.Priority)
                        {
                            maxParcel = p;
                        }
                        else if (maxParcel.Priority == p.Priority /*&& p.Weight <= droneToParcel.MaxWeight && maxParcel.Weight <= droneToParcel.MaxWeight*/)
                        {
                            if (maxParcel.Weight < p.Weight)
                                maxParcel = p;
                            else if (maxParcel.Weight == p.Weight)
                            {

                                if (disDroneToSenderP < disMaxPToSender)
                                {
                                    maxParcel = p;
                                }
                            }
                        }
                    }
                
                }
            }
            if(maxParcel.Weight == 0) //in the beggining parcels' weight = 0 to start compairing ;
            {
                throw new Exception("Drone with the parcels' conditions wasn't found.");
            }
            droneToParcel.Status = DroneStatus.Delivery;
            updateBLDrone(droneToParcel);
            maxParcel.DroneId = droneToParcel.Id;
            maxParcel.Scheduled = DateTime.Now;
            updateParcel(maxParcel);///////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void pickUpParcelByDrone(int droneId)
        {
            BLDrone bLDrone = dronesInBL.Find(d => d.Id == droneId && d.Status == DroneStatus.Delivery);
            IDal.DO.Parcel p = dal.getParcelByDroneId(droneId);
            if(!p.PickUp.Equals(null))
            {
                throw new Exception("The parcel is collected already");
            }
            if (p.Scheduled.Equals(null))
            {
                throw new Exception("The parcel is not schedueld.");
            }
            IDal.DO.Customer senderP= dal.getCustomerById(p.SenderId);
            BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            double disDroneToSenderP = distance(bLDrone.DronePosition, senderPosition);

            bLDrone.Battery -= disDroneToSenderP * requestElectricity()[(int)p.Weight];
            bLDrone.DronePosition = senderPosition;
            updateBLDrone(bLDrone);
            p.PickUp = DateTime.Now;
            updateParcel(maxParcel);///////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public void updateBLDrone(BLDrone d)
        {
            BLDrone findDrone = dronesInBL.Find(e => e.Id == d.Id);
            findDrone = d;
        }



        public IDal.DO.Station getMinDistanceFromStation(BLPosition p)/////////////////////////////////////////////
        {
            return new IDal.DO.Station();
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        

        //==================================
        // Findong a drone in the BL array
        //==================================
        private BLDrone GetBLDroneById(int id)
        {
            return dronesInBL.Find(d => d.Id == id);
        }
        //==================================
        // Conversions
        //==================================
        private IDal.DO.Station convertBLToDalStation(BLStation s)
        {
            return new IDal.DO.Station() { Id = s.ID, Name = s.Name, ChargeSlots = s.DroneChargeAvailble + s.ChargingDrone.Count(), Longitude = s.StationPosition.Longitude, Latitude = s.StationPosition.Latitude };
        }
        private IDal.DO.Drone convertBLToDalDrone(BLDrone d)
        {
            return new IDal.DO.Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight ,/* Battery = d.Battery , Status = d.Status*/};
        }
        private IDal.DO.Customer convertBLToDalCustomer(BLCustomer c)
        {
            return new IDal.DO.Customer() { ID = c.ID, Name = c.Name, Phone = c.Phone, Longitude = c.CustomerPosition.Longitude, Latitude = c.CustomerPosition.Latitude, };
        }
        private IDal.DO.Parcel convertBLToDalParcel(BLParcel p)
        {
            //IDal.DO.Customer sender = convertBLToDalCustomer(p.Sender);
            //IDal.DO.Customer target = convertBLToDalCustomer(p.Target);
            //IDal.DO.Drone drone = convertBLToDalDrone(p.Drone);
            return new IDal.DO.Parcel() { Id = p.Id, SenderId = p.Sender.ID, TargetId = p.Target.ID, Weight = p.Weight, Priority = p.Priority, DroneId = p.Drone.Id, Requeasted = p.Requeasted, Scheduled = p.Scheduled, PickUp = p.PickUp, Delivered = p.Delivered };
        }
        private BLStation convertDalToBLStation(IDal.DO.Station s)
        {
            return new BLStation() { ID = s.Id, Name = s.Name, DroneChargeAvailble = s.ChargeSlots, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude } };
        }
        private BLCustomer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            return new BLCustomer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude } };
        }
        public BLDrone convertDalToBLDrone(IDal.DO.Drone d)////////////////////////////////////////////
        {
            try
            {
                return GetBLDroneById(d.Id); //if there is no such BLdrone -> there is an error becuase the obj is in DAL Obj
            }
            catch
            {
                throw new NoDataMatchingBetweenDalandBL<IDal.DO.Drone>(d);
            }
            //return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight ,/*Battery = d.Battery , Status = d.Status*//*++++++++++++++++++++*/
        }
        
        private BLParcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            BLCustomer sender = convertDalToBLCustomer(dal.getCustomerById(p.SenderId));
            BLCustomer target = convertDalToBLCustomer(dal.getCustomerById(p.TargetId));
            BLDrone drone = convertDalToBLDrone(dal.getDroneById(p.DroneId));
            return new BLParcel() { Id = p.Id, Sender = sender, Target = target, Weight = p.Weight, PickUp = p.PickUp, Drone = drone, Requeasted = p.Requeasted, Scheduled = p.Scheduled, /* PickUp = p.PickUp,*/ Delivered = p.Delivered };
        }
        //==================================
        // Get object by ID
        //==================================
        public void getParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority)
        {
            IDal.DO.Parcel p = new IDal.DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Delivered = new DateTime(), Requeasted = DateTime.Now, Scheduled = new DateTime(), Weight = weight, PickUp = new DateTime() };
            dal.AddParcelToDelivery(p);
        }
        public BLStation getStationById(int id)
        {
            IDal.DO.Station s = dal.getStationById(id);
            BLStation BLstation = convertDalToBLStation(s);
            return BLstation;
        }
        public BLCustomer getCustomerById(int id)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            BLCustomer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }
        public BLDrone getDroneById(int id)
        {
            IDal.DO.Drone d = dal.getDroneById(id);
            BLDrone BLdrone = convertDalToBLDrone(d);
            return BLdrone;
        }
        public BLParcel getParcelById(int id)
        {
            IDal.DO.Parcel p = dal.getParcelById(id);
            BLParcel BLparcel = convertDalToBLParcel(p);
            return BLparcel;
        }
        //==================================
        // Get list of objects
        //==================================
        public List<BLStation> displayStations()
        {
            List<IDal.DO.Station> sList = dal.displayStations().Cast<IDal.DO.Station>().ToList();
            List<BLStation> arr = new List<BLStation>();
            sList.ForEach(s => arr.Add(convertDalToBLStation(s)));
            return arr;
        }
        public List<BLCustomer> displayCustomers()
        {
            List<IDal.DO.Customer> cList = dal.displayCustomers().Cast<IDal.DO.Customer>().ToList();
            List<BLCustomer> arr = new List<BLCustomer>();
            cList.ForEach(c => arr.Add(convertDalToBLCustomer(c)));
            return arr;
        }
        public List<BLDrone> displayDrones()
        {
            List<IDal.DO.Drone> dList = dal.displayDrone().Cast<IDal.DO.Drone>().ToList();
            List<BLDrone> arr = new List<BLDrone>();
            dList.ForEach(d => arr.Add(convertDalToBLDrone(d)));
            return arr;
        }
        public List<BLParcel> displayParcel()
        {
            List<IDal.DO.Parcel> pList = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            List<BLParcel> arr = new List<BLParcel>();
            pList.ForEach(s => arr.Add(new BLParcel() { Id = s.Id,/* SenderId = s.SenderId, TargetId = s.TargetId,*/ PickUp = s.PickUp, Priority = s.Priority, Weight = s.Weight }));
            return arr;
        }
        public List<BLParcel> displayFreeParcel()
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
