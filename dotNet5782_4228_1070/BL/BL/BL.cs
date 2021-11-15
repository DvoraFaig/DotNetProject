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
        //==================================
        // Add
        //==================================
        public void addStation(int id, string name, int latitude, int longitude, int chargeSlot)
        {
            if (!dal.getStationById(id).Equals(null))
            {
                throw new ObjExistException("station", id);
            }
            IDal.DO.Station s = new IDal.DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
            dal.AddStation(s);
        }
        public void addDrone(int id, string model, int maxWeight, int stationId)
        {  //==//

            if (!dal.getDroneById(id).Equals(null))
            {
                throw new ObjExistException("drone", id);
            }
            IDal.DO.WeightCategories maxWeightconvertToEnum = (IDal.DO.WeightCategories)maxWeight;

            Random r = new Random();
            int battery = r.Next(20, 40);
            IDal.DO.Station s = dal.getStationById(stationId);
            BLPosition p = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
            BLDrone dr = new BLDrone() { Id = id, Model = model, MaxWeight = maxWeightconvertToEnum, Status = DroneStatus.Maintenance, Battery = battery, DronePosition = p };
            dronesInBL.Add(dr);
            dal.AddDrone(convertBLToDalDrone(dr));
        }
        public void AddCustomer(int id, string name, string phone, int longitude, int latitude)
        {
            if (!dal.getCustomerById(id).Equals(null))
            {
                throw new ObjExistException("customer", id);
            }
            IDal.DO.Customer c = new IDal.DO.Customer() { ID = id, Name = name, Phone = phone, Latitude = latitude, Longitude = longitude };
            dal.AddCustomer(c);
        }
        public void addParcel(int senderId, int targetId, int weight, int priority)
        {
            IDal.DO.Customer dalCustomer;
            IDal.DO.Parcel p = new IDal.DO.Parcel();
            p.Id =  dal.amountParcels() + 1;
            try
            {
                dalCustomer = dal.getCustomerById(senderId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            try
            {
                dalCustomer = dal.getCustomerById(targetId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            p.SenderId = senderId;
            p.TargetId = targetId;
            p.Weight = (IDal.DO.WeightCategories)weight;
            p.Priority = (IDal.DO.Priorities)priority;
            p.Requeasted = DateTime.Now;
            p.Scheduled = new DateTime(0, 0, 0);
            p.PickUp = new DateTime(0, 0, 0);
            p.Delivered = new DateTime(0, 0, 0);
            dal.AddParcel(p);
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
        public void stationChangeDetails(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
        {
            IDal.DO.Station s = dal.getStationById(id);
            if (name != null)
                s.Name = name;
            if (s.ChargeSlots <= ChargeSlots)
                s.ChargeSlots = ChargeSlots;
            if (s.ChargeSlots > ChargeSlots)
            {
                List<IDal.DO.DroneCharge> droneCharges = dal.displayDrone().Cast<IDal.DO.DroneCharge>().ToList();
                int amountDroneChargesFull = droneCharges.Count(station => station.StationId == s.Id);
                if (amountDroneChargesFull < ChargeSlots)
                    s.ChargeSlots = ChargeSlots;
                else
                    throw new Exception($"The amount Charging slots you want to change is smaller than the amount of drones that are charging now in station number {id}");
            }

        }
        public void updateCustomerDetails(int id, string name = null, string phone = null)
        {
            IDal.DO.Customer c = dal.getCustomerById(id);
            if (c.Equals(null))
            {
                throw new Exceptions.ObjNotExistException<IDal.DO.Customer>(c);
            }
            if (name != null)
                c.Name = name;
            if (phone != null)
                c.Phone = phone;
        }
        public void sendDroneToCharge(int droneId)
        {
            BLDrone drone = dronesInBL.Find(d => d.Id == droneId);
            if (drone.Status == DroneStatus.Available)
            {

            }
            else
            {

            }
        }


        public void updateFreeDroneFromeCharge(int droneId, double time)
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
            blDrone.Battery += (double)time * requestElectricity()[4];
            IDal.DO.DroneCharge droneChargeByStation = dal.getDroneChargeByDroneId(blDrone.Id);
            IDal.DO.Station s = dal.getStationById(droneChargeByStation.StationId);
            s.ChargeSlots++;
            stationChangeDetails(s.Id, null, s.ChargeSlots);
            //dal.removeDroneFromCharge();
        }
        public void PairParcelWithDrone(int droneId)
        {
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            IDal.DO.Customer senderMaxParcel;
            BLDrone droneToParcel = GetBLDroneById(droneId);
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            IDal.DO.Parcel maxParcel = new IDal.DO.Parcel() { Weight = 0, };//parcels.First(); //check if weght is good=====================
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
                IDal.DO.Station MinDisFromTargetTostation = getMinDistanceFromStation(targetPosition);
                double disTargetToStation = distance(targetPosition, new BLPosition() { Longitude = MinDisFromTargetTostation.Longitude, Latitude = MinDisFromTargetTostation.Latitude });
                double disMaxPToSender = distance(droneToParcel.DronePosition, new BLPosition() { Longitude = senderMaxParcel.Longitude, Latitude = senderMaxParcel.Latitude });
                double droneElectricity = requestElectricity()[(int)p.Weight];
                if (droneToParcel.Battery - (int)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity) > 0) //[4]
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
            if (maxParcel.Weight == 0) //in the beggining parcels' weight = 0 to start compairing ;
            {
                throw new Exception("Drone with the parcels' conditions wasn't found.");
            }
            droneToParcel.Status = DroneStatus.Delivery;
            updateBLDrone(droneToParcel);
            maxParcel.DroneId = droneToParcel.Id;
            maxParcel.Scheduled = DateTime.Now;
            updateParcel(maxParcel);
        }
        public void DroneCollectsAParcel(int droneId)
        {
            BLDrone bLDrone = dronesInBL.Find(d => d.Id == droneId && d.Status == DroneStatus.Delivery);
            IDal.DO.Parcel p = dal.getParcelByDroneId(droneId);
            if (!p.PickUp.Equals(null))
            {
                throw new Exception("The parcel is collected already");
            }
            if (p.Scheduled.Equals(null))
            {
                throw new Exception("The parcel is not schedueld.");
            }
            IDal.DO.Customer senderP = dal.getCustomerById(p.SenderId);
            BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            double disDroneToSenderP = distance(bLDrone.DronePosition, senderPosition);

            bLDrone.Battery -= disDroneToSenderP * requestElectricity()[(int)p.Weight];
            bLDrone.DronePosition = senderPosition;
            updateBLDrone(bLDrone);
            p.PickUp = DateTime.Now;
            updateParcel(maxParcel);
        }

        public void updateBLDrone(BLDrone d)
        {
            BLDrone findDrone = dronesInBL.Find(e => e.Id == d.Id);
            findDrone = d;
        }



        public IDal.DO.Station getMinDistanceFromStation(BLPosition p)
        {
            return new IDal.DO.Station();
        }
        public void freeDroneFromCharging(int id, int timeCharging)
        {

        }

        public void DronePicksUpParcel(int droneId)
        {
        }
        //public void DeliveryParcelByDrone(int droneId)
        //{
        //}

        internal static string checkNullforPrint<T>(T t)
        {
            if (t.Equals(null))
                return $"--field {t.GetType()} not filled yet.--";
            return t.ToString();
        }
        public void deliveryParcelByDrone(int idDrone)
        {
            BLDrone bLDroneToSuplly = GetBLDroneById(idDrone);
            IDal.DO.Parcel parcelToDelivery = dal.getParcelByDroneId(idDrone);
            if (parcelToDelivery.PickUp.Equals(null) && !parcelToDelivery.Delivered.Equals(null))
            {
                throw new Exception("Drone cann't deliver this parcel.");
            }
            IDal.DO.Customer senderP;
            IDal.DO.Customer targetP;
            senderP = dal.getCustomerById(parcelToDelivery.SenderId);
            targetP = dal.getCustomerById(parcelToDelivery.TargetId);
            BLPosition senderPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            BLPosition targetPosition = new BLPosition() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
            double disSenderToTarget = distance(senderPosition, targetPosition);
            double ectricity = dal.requestElectricity()[(int)parcelToDelivery.Weight];
            bLDroneToSuplly.Battery -= ectricity * disSenderToTarget;
            bLDroneToSuplly.DronePosition = targetPosition;
            bLDroneToSuplly.Status = DroneStatus.Available;
            updateBLDrone(bLDroneToSuplly);
            parcelToDelivery.Delivered = DateTime.Now;
            updateParcel(parcelToDelivery);

        }
        private static bool checkNull<T>(T t)
        {
            if (t.Equals(null))
                return false;
            return true;
        } //not in Ibl
        private static ParcelStatuses findParcelStatus(IDal.DO.Parcel p)
        {
            if (p.Requeasted.Equals(null))
                return (ParcelStatuses)0;
            else if (p.Scheduled.Equals(null))
                return (ParcelStatuses)1;
            else if (p.PickUp.Equals(null))
                return (ParcelStatuses)2;
            else // if (p.Delivered.Equals(null))
                return (ParcelStatuses)3;
        } //not in Ibl
          // Throw match exception - "drone not available to charge"

        //==================================
        // Finding a drone in the BL array
        //==================================
        private static BLDrone GetBLDroneById(int id)/////////////////////////////
        {
            return new BLDrone();
            //return dronesInBL.Find(d => d.Id == id);

        }
        //==================================
        // Conversions BL to DAL.
        //==================================
        private  IDal.DO.Station convertBLToDalStation(BLStation s)
        {
            return new IDal.DO.Station() { Id = s.ID, Name = s.Name, ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(), Longitude = s.StationPosition.Longitude, Latitude = s.StationPosition.Latitude };
        }
        private  IDal.DO.Drone convertBLToDalDrone(BLDrone d)
        {
            return new IDal.DO.Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight,/* Battery = d.Battery , Status = d.Status*/};
        }
        private  IDal.DO.Customer convertBLToDalCustomer(BLCustomer c)
        {
            return new IDal.DO.Customer() { ID = c.ID, Name = c.Name, Phone = c.Phone, Longitude = c.CustomerPosition.Longitude, Latitude = c.CustomerPosition.Latitude, };
        }
        private IDal.DO.Parcel convertBLToDalParcel(BLParcel p)
        {
            //IDal.DO.Customer sender = convertBLToDalCustomer(p.Sender);
            //IDal.DO.Customer target = convertBLToDalCustomer(p.Target);
            //IDal.DO.Drone drone = convertBLToDalDrone(p.Drone);
            return new IDal.DO.Parcel() { Id = p.Id, SenderId = p.Sender.Id, TargetId = p.Target.Id, Weight = p.Weight, Priority = p.Priority, DroneId = p.Drone.Id, Requeasted = p.Requeasted, Scheduled = p.Scheduled, PickUp = p.PickUp, Delivered = p.Delivered };
        }
        //==================================
        // Conversions DAL to BL.
        //==================================

        private static BLStation convertDalToBLStation(IDal.DO.Station s)
        {
            List<BLChargingDrone> blDroneChargingByStation = new List<BLChargingDrone>();
            List<IDal.DO.DroneCharge> droneChargesByStation = dal.displayDroneCharge().ToList();
            droneChargesByStation = droneChargesByStation.FindAll(d => d.StationId == s.Id);
            droneChargesByStation.ForEach(d =>
                {
                    blDroneChargingByStation.Add(new BLChargingDrone() { Id = d.DroneId, Battery = GetBLDroneById(d.DroneId).Battery });
                });
            return new BLStation() { ID = s.Id, Name = s.Name, StationPosition = new IBL.BO.BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude }, DroneChargeAvailble = s.ChargeSlots, DronesCharging = blDroneChargingByStation };
        }
        private static BLCustomer convertDalToBLCustomer(IDal.DO.Customer c)
        {
            List<IDal.DO.Parcel> parcels = dal.displayParcels().Cast<IDal.DO.Parcel>().ToList();
            List<IDal.DO.Parcel> sendingParcels = parcels.FindAll(p => p.SenderId == c.ID);
            List<IDal.DO.Parcel> targetParcels = parcels.FindAll(p => p.TargetId == c.ID);
            List<BLParcelAtCustomer> customerAsSender = new List<BLParcelAtCustomer>();
            List<BLParcelAtCustomer> customerAsTarget = new List<BLParcelAtCustomer>();
            sendingParcels.ForEach(p => customerAsSender.Add(createtDalParcelToBLParcelAtCustomer(p, c)));
            targetParcels.ForEach(p => customerAsTarget.Add(createtDalParcelToBLParcelAtCustomer(p, c)));
            return new BLCustomer() { ID = c.ID, Name = c.Name, Phone = c.Phone, CustomerPosition = new IBL.BO.BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude }, CustomerAsSender = customerAsSender, CustomerAsTarget = customerAsSender };
        }
        private static BLDrone convertDalToBLDrone(IDal.DO.Drone d)//////////////////////////////////////////////////////////////////////////////
        {
            BLDrone BLDrone = GetBLDroneById(d.Id);// = dronesInBL.Find(e => e.Id == d.Id);
            //try
            //{
            //    return GetBLDroneById(d.Id); //if there is no such BLdrone -> there is an error becuase the obj is in DAL Obj
            //}
            //catch
            //{
            //    throw new NoDataMatchingBetweenDalandBL<IDal.DO.Drone>(d);
            //}
            Random r = new Random();
            return new BLDrone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
        }
        private static BLChargingDrone convertDalToBLChargingDrone(IDal.DO.DroneCharge d) //convertDalToBLChargingDrone the opposite BL to DAL
        {
            return new BLChargingDrone() { Id = d.DroneId, Battery = GetBLDroneById(d.DroneId).Battery };
        }
        private static BLParcel convertDalToBLParcel(IDal.DO.Parcel p)
        {
            BLDroneInParcel drone = null;
            if (!p.Scheduled.Equals(null)) //if the parcel is paired with a drone
            {
                drone = createBLDroneInParcel(p, GetBLDroneById(p.DroneId).Id);
            }
            return new BLParcel()
            {
                Id = p.Id,
                Sender = new BLCustomerInParcel() { Id = p.SenderId, name = dal.getCustomerById(p.SenderId).Name },
                Target = new BLCustomerInParcel() { Id = p.TargetId, name = dal.getCustomerById(p.TargetId).Name },
                Weight = p.Weight,
                Drone = createBLDroneInParcel(p, p.DroneId),
                Requeasted = p.Requeasted,
                Scheduled = p.Scheduled,
                PickUp = p.PickUp,
                Delivered = p.Delivered
            };
        }
        private static BLDroneInParcel createBLDroneInParcel(IDal.DO.Parcel p, int droneId)
        {
            BLDrone d = GetBLDroneById(droneId);
            return new BLDroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithparcel = d.DronePosition };
        }
        private static BLParcelAtCustomer createtDalParcelToBLParcelAtCustomer(IDal.DO.Parcel p, IDal.DO.Customer c)
        {
            //if(p.senderId == c.Id || p.TargetId == c.Id)
            return new BLParcelAtCustomer() { Id = p.Id, Weight = p.Weight, Priority = p.Priority, ParcelStatus = findParcelStatus(p), SenderOrTargetCustomer = new BLCustomerInParcel() { Id = c.ID, name = c.Name } };
        }
        //==================================
        // Get object by ID
        //==================================
        public void getParcelToDelivery(int senderId, int targetId, IDal.DO.WeightCategories weight, IDal.DO.Priorities priority)
        {
            IDal.DO.Parcel p = new IDal.DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Delivered = new DateTime(), Requeasted = DateTime.Now, Scheduled = new DateTime(), Weight = weight, PickUp = new DateTime() };
            dal.AddParcel(p);
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
        public IDal.DO.Drone getDalDroneById(int id)
        {
            return dal.getDroneById(id);
        }
        public IDal.DO.Station getDalStationById(int id)
        {
            return dal.getStationById(id);
        }
        public IDal.DO.Customer getDalCustomerById(int id)
        {
            return dal.getCustomerById(id);
        }
        public IDal.DO.Parcel getDalParcelById(int id)
        {
            return dal.getParcelById(id);
        }
        public IDal.DO.DroneCharge getDalDroneChargeById(int id)
        {
            return dal.getDroneChargeById(id);
        }


        //==================================
        // Get list of objects
        //==================================
        public static List<BLStation> displayStations()
        {
            IEnumerable<IDal.DO.Station> dalStations = dal.displayStations();
            List<BLStation> arr = new List<BLStation>();
            foreach (var s in dalStations)
            {
                arr.Add(convertDalToBLStation(s));
            }
            return arr;
        }
        public static List<BLCustomer> displayCustomers()
        {
            IEnumerable<IDal.DO.Customer> cList = dal.displayCustomers();
            List<BLCustomer> arr = new List<BLCustomer>();
            foreach (var c in cList)
            {
                arr.Add(convertDalToBLCustomer(c));
            }
            return arr;
        }
        public static List<BLDrone> displayDrones()
        {
            IEnumerable<IDal.DO.Drone> dList = dal.displayDrone();
            List<BLDrone> arr = new List<BLDrone>();
            foreach (var d in dList)
            {
                arr.Add(convertDalToBLDrone(d));
            }
            return arr;
        }
        public static List<BLParcel> displayParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
            return arr;
        }
        public static List<BLParcel> displayFreeParcel()
        {
            IEnumerable<IDal.DO.Parcel> pList = dal.displayFreeParcels();
            List<BLParcel> arr = new List<BLParcel>();
            foreach (var p in pList)
            {
                arr.Add(convertDalToBLParcel(p));
            }
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
