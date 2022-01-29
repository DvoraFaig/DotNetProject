using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
//using DO;
using System.Threading;
using BO;
using static BL.BL;
using static BO.Exceptions;
using BlApi.IBL;

namespace BO
{
    class Simulation
    {
        Ibl BL;
        ISimulation Isimulation;
        Parcel parcel;
        Customer sender;
        Customer target;

        /// <summary>
        /// Ctor Simulation
        /// </summary>
        /// <param name="BL">Interface Ibl</param>
        public Simulation(Ibl BL)
        {
            this.BL = BL;
            distace = 0;
        }

        public enum DroneStatus { ToPickUp = 1, PickUp, ToDelivery, Delivery, ToCharge, NoAvailbleChargingSlots, NotEnoughBatteryForDelivery, DisFromDestination, HideTextBlock };


        private double distace { get; set; }

        /// <summary>
        /// Start simulation run occurding to drone status.
        /// DroneStatusAvailable() => PairParcelWithDrone.
        /// DroneStatusMaintenance() => Charging drone && freeing it.
        /// DroneStatusDelivery() =>
        ///     case 1: Parcel wasn't pick up yet. 
        ///     case d: Parcel wasn't deliverd yet.
        ///     case 1: Parcel is delivered and drone destination is a station.
        /// </summary>
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        /// <param name="needToStop">Func to use to stop simulation</param>
        public void StartSimulation(Drone drone, Action<Drone, int , double> updateDrone, Func<bool> needToStop)
        {

            while (!needToStop())
            {
                switch (drone.Status)
                {
                    case BO.DroneStatus.Available:
                        DroneStatusAvailable(updateDrone, drone, needToStop);
                        break;

                    case BO.DroneStatus.Maintenance:
                        #region a differance way
                        //double BatteryLeftToFullCharge = 100 - drone.Battery;
                        //double percentFillBatteryForCharging = BL.requestElectricity(0);
                        ////double baterryToAdd = second.TotalMinutes * chargingRateOfDrone;
                        ////baterryToAdd/chargingRateOfDrone = second.TotalMinutes;
                        //double timeLeftToCharge = BatteryLeftToFullCharge / percentFillBatteryForCharging;
                        //double a = Math.Ceiling(timeLeftToCharge);//int
                        //while (a > 0 && drone.Battery < 100)
                        //{
                        //    drone.Battery += percentFillBatteryForCharging * 10;
                        //    updateDrone(drone, 1);
                        //    Thread.Sleep(100);
                        //    a -= 100*percentFillBatteryForCharging; //-10
                        //}
                        #endregion
                        DroneStatusMaintenance(updateDrone, drone);
                        freeDroneFromCharge(updateDrone, drone);
                        break;

                    case BO.DroneStatus.Delivery:
                        DroneStatusDelivery(updateDrone, drone);
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Simulation when drone.Status == DroneStatus.Available
        /// Try to PairParcelWithDrone;
        /// If not try to send to charge
        /// If Not enough battery to fly to station: drone will be a parcel and another drone will pick him up.  
        /// </summary>
        /// <param name="updateDrone"></param>
        /// <param name="drone"></param>
        private void DroneStatusAvailable(Action<Drone, int , double> updateDrone, Drone drone, Func<bool> needToStop)
        {
            try
            {
                BO.Drone droneWithParcel = BL.PairParcelWithDrone(drone.Id);
                drone.Status = BO.DroneStatus.Delivery;
                drone.ParcelInTransfer = droneWithParcel.ParcelInTransfer;
                updateDrone(drone, (int) DroneStatus.HideTextBlock, distace); //if in the begining their were no available charging slots. hide the text block
                BL.changeDroneInfo(drone);
            }
            catch (ObjNotAvailableException)
            {
                try //not enough battery
                {
                    BL.SendDroneToCharge(drone.Id);
                    drone.Status = BO.DroneStatus.Maintenance;
                    updateDrone(drone, (int)DroneStatus.NotEnoughBatteryForDelivery, distace);
                    BL.changeDroneInfo(drone);
                }
                catch (ObjNotExistException) //No charging slots available
                {
                    Thread.Sleep(5000);
                }
                catch (ObjNotAvailableException) //Not enough battery to fly to station
                {
                    needToStop();
                    //Parcel parcel = BL.getParcelByStatus(Scheduled)//??????????????
                    //Not supposed to happen:
                    //If hapens Drone Will be like a parcel and another parcel will pick him
                    //up and deliver it to the neerest station with an empty charge slots.
                    // Not implemented becuase need to do other actions
                    // in the next interation on the while loop
                }

            }
            // Do: If drone not has enough buttery to go to the near station.... do something
            catch (Exception e)
            {
                Thread.Sleep(2000);
                ///to stop
            }
        }


        /// <summary>
        /// Simulation when drone.Status == DroneStatus.Maintenance
        /// Charging drone and than drone.Status == DroneStatus.Available
        /// </summary>
        /// <param name="updateDrone"></param>
        /// <param name="drone"></param>
        private void DroneStatusMaintenance(Action<Drone, int , double> updateDrone, Drone drone)
        {
            DateTime now = DateTime.Now;
            TimeSpan second;
            double baterryToAdd;//= //second.TotalMinutes * BL.requestElectricity(0);
            double batteryPerTime = BL.requestElectricity(0);
            //= //second.TotalMinutes * BL.requestElectricity(0);
            //TimeSpan second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 100;
            //double baterryToAdd = second.TotalMinutes * BL.requestElectricity(0);
            while (drone.Battery < 100)
            {
                second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 1000;//*1000
                drone.SartToCharge = DateTime.Now;
                baterryToAdd = second.TotalMinutes * batteryPerTime;
                drone.Battery += baterryToAdd;
                drone.Battery = Math.Min(100, drone.Battery);
                drone.Battery = Math.Max(0, drone.Battery);
                drone.Battery = Math.Round(drone.Battery, 1);
                updateDrone(drone, 0, 0);
                Thread.Sleep(100);//100
            }
        }

        /// <summary>
        /// freeing Drone From Charge.
        /// Changing drone status from DroneStatus.Maintenance to drone.Status. = DroneStatus.Available;
        /// </summary>
        /// <param name="updateDrone"></param>
        /// <param name="drone"></param>
        private void freeDroneFromCharge(Action<Drone, int , double> updateDrone, Drone drone)
        {
            bool succeedFreeDroneFromCharge = false;
            do
            {
                try
                {
                    BL.removeDroneChargeByDroneId(drone.Id); //BL.FreeDroneFromCharging(drone.Id);
                    drone.Status = BO.DroneStatus.Available;
                    BL.changeDroneInfo(drone);
                    updateDrone(drone, 0 , distace);
                    succeedFreeDroneFromCharge = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
            while (!succeedFreeDroneFromCharge);
        }



        /// <summary>
        /// Simulation when drone.Status == DroneStatus.Delivery
        /// case 1: Parcel wasn't pick up yet.
        /// case d: Parcel wasn't deliverd yet .
        /// case 1: Parcel is delivered and drone destination is a station with an available charging slot.
        /// </summary>
        /// <param name="updateDrone"></param>
        /// <param name="drone"></param>
        private void DroneStatusDelivery(Action<Drone, int , double> updateDrone, Drone drone)
        {

            DeliveryStatusAction droneStatus = BL.GetfromEnumDroneStatusInDelivery(drone);
            switch ((int)droneStatus)
            {
                #region Parcel wasn't pick up. (parcel.Scheduled != null && parcel.PickUp ==null.)
                case (1)://AsignedParcel
                    {
                        initializeObjectsWhenDroneInDelivery(drone);
                        updateDrone(drone, (int)Simulation.DroneStatus.ToPickUp, distace);
                        Thread.Sleep(1000);
                        drone = calcDisAndSimulateDlivery(updateDrone, drone, sender.CustomerPosition, BL.requestElectricity(0));
                        #region export this
                        ////////////////////////////////==========================================================
                        //#region declare and implement variables
                        //Position senderA = sender.CustomerPosition;
                        //double distanceDroneToSender = distance(drone.DronePosition, senderA);
                        //double batteryUsageByWeight = BL.requestElectricity(0);//(int)parcel.Weight
                        //double Incline = 0;
                        //double b;
                        //double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;

                        //double changeXInDis = drone.DronePosition.Latitude > senderA.Latitude ? -1 : 1; //drone x > costomer x // go back by x
                        //double changeYInDis = drone.DronePosition.Longitude > senderA.Longitude ? -1 : 1; //drone y > costomer y // go back by x

                        //double x = drone.DronePosition.Latitude;
                        //double y = drone.DronePosition.Longitude;
                        //double dis;
                        //double batteryUsageByWeightForM;
                        //#endregion

                        ////implement variables that are different in cases.

                        //#region Latitude is equal between drone and currentTarget
                        //if (drone.DronePosition.Latitude == senderA.Latitude)
                        //{
                        //    changeXInDis = 0;
                        //    b = drone.DronePosition.Longitude;
                        //    dis = changeYInDis; //The differance is by Longitude
                        //}
                        //#endregion

                        //#region Longitude is equal between drone and currentTarget
                        //else if (drone.DronePosition.Longitude == senderA.Longitude)
                        //{
                        //    changeYInDis = 0;
                        //    b = drone.DronePosition.Longitude;
                        //    dis = changeXInDis; //The differance is by Latitude
                        //}
                        //#endregion

                        //#region Position is dirrerent between drone and currentTarget
                        //else  //x = Latitude, y = Longitude.
                        //{
                        //    Incline = ((drone.DronePosition.Longitude - senderA.Longitude)
                        //       / (drone.DronePosition.Latitude - senderA.Latitude));
                        //    #region calc
                        //    //y = Incline * x + b
                        //    //-b = Incline * x - y
                        //    #endregion
                        //    b = Incline * x - y;
                        //    b = -b;
                        //    double Equation = Incline * x + b;
                        //    changeYInDis = (Incline * (x + 1) + b)/*the Y2*/ - y/*Y1*/;
                        //    dis = distance(drone.DronePosition, new Position() { Latitude = x + 1, Longitude = changeYInDis });
                        //}
                        //#endregion

                        //Position now = new Position() { Latitude = x, Longitude = y };
                        //Position currentPos;
                        //batteryUsageByWeightForM = Math.Round(Math.Abs(batteryUsageByWeight * dis), 2);

                        //#region calc dis and display simulation
                        //while (drone.Battery > 0 && !(x == senderA.Latitude && y == senderA.Longitude))
                        //{
                        //    x += changeXInDis;
                        //    y += changeYInDis;

                        //    currentPos = new Position() { Latitude = x, Longitude = y };
                        //    #region changes of drone to update by display
                        //    drone.DronePosition.Latitude = x;
                        //    drone.DronePosition.Longitude = y;
                        //    drone.DronePosition = currentPos;
                        //    drone.Battery -= batteryUsageByWeightForM;
                        //    drone.Battery = Math.Round(drone.Battery, 1);

                        //    updateDrone(drone, 1);
                        //    //BL.changeDroneInfo(drone);
                        //    #endregion
                        //    Thread.Sleep(500);
                        //}
                        //#endregion
                        //////////////////////////////==========================================================
                        #endregion
                        #region export to a func calcAndSimulateDlivery
                        //double distanceDroneToSender = distance(drone.DronePosition, sender.CustomerPosition);
                        //double batteryUsageByWeight = BL.requestElectricity(0);//(int)parcel.Weight
                        //double Incline = 0;
                        //double b;
                        //double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;

                        //double changeXInDis = drone.DronePosition.Latitude > sender.CustomerPosition.Latitude ? -1 : 1; //drone x > costomer x // go back by x
                        //double changeYInDis = drone.DronePosition.Longitude > sender.CustomerPosition.Longitude ? -1 : 1; //drone y > costomer y // go back by x

                        //double x = drone.DronePosition.Latitude;
                        //double y = drone.DronePosition.Longitude;
                        //double dis;
                        //double batteryUsageByWeightForM; 

                        //if (drone.DronePosition.Latitude == sender.CustomerPosition.Latitude)
                        //{
                        //    changeXInDis = 0;
                        //    b = drone.DronePosition.Longitude;
                        //    dis = changeYInDis; //The differance is by Longitude
                        //}
                        //else if (drone.DronePosition.Longitude == sender.CustomerPosition.Longitude)
                        //{
                        //    changeYInDis = 0;
                        //    b = drone.DronePosition.Longitude;
                        //    dis = changeXInDis; //The differance is by Latitude
                        //}
                        //else  //x = Latitude, y = Longitude.
                        //{
                        //    Incline = ((drone.DronePosition.Longitude - sender.CustomerPosition.Longitude)
                        //       / (drone.DronePosition.Latitude - sender.CustomerPosition.Latitude));
                        //    #region calc
                        //    //y = Incline * x + b
                        //    //-b = Incline * x - y
                        //    #endregion
                        //    b = Incline * x - y;
                        //    b = -b;
                        //    double Equation = Incline * x + b;
                        //    changeYInDis = (Incline * (x+1) + b)/*the Y2*/ - y/*Y1*/;
                        //    dis = distance(drone.DronePosition, new Position() { Latitude = x + 1, Longitude = changeYInDis });
                        //}

                        //Position now = new Position() { Latitude = x, Longitude = y };
                        //Position currentPos;
                        //batteryUsageByWeightForM = Math.Round(Math.Abs(batteryUsageByWeight * dis), 2);

                        //while (drone.Battery > 0 && !(x == sender.CustomerPosition.Latitude && y == sender.CustomerPosition.Longitude))
                        //{
                        //    x += changeXInDis;
                        //    y += changeYInDis;
                        //    //if (changeYInDis != 0) // if the drone and customer Latitude is the same and the change is only by Y;
                        //    //{ y += changeYInDis; b += changeYInDis; } //y += changeYInDis; b=y

                        //    //y = Incline * x + b;

                        //    currentPos = new Position() { Latitude = x, Longitude = y };
                        //    //if (changeYInDis != 0) // if the change is 1/-1 the same Latitude
                        //    //    dis = changeYInDis;
                        //    //else if (b == drone.DronePosition.Longitude) // if the change is 1/-1 the same Longitude
                        //    //    dis = changeXInDis;
                        //    //else
                        //    //{
                        //    //    dis = distance(now, to);
                        //    //    now = to;
                        //    //}
                        //    #region changes of drone to update by display
                        //    drone.DronePosition.Latitude = x;
                        //    drone.DronePosition.Longitude = y;
                        //    drone.DronePosition = currentPos;
                        //    drone.Battery -= Math.Round(Math.Abs(batteryUsageByWeight * dis),2); //batteryUsageByWeight * 10;
                        //    updateDrone(drone, 1);
                        //    #endregion
                        //    //BL.changeDroneInfo(drone);
                        //    Thread.Sleep(1000);

                        //}
                        #endregion
                        updateDrone(drone, (int)Simulation.DroneStatus.PickUp, distace);
                        parcel.PickUp = DateTime.Now;
                        BL.changeParcelInfo(parcel);
                        break;
                    }
                #endregion

                #region Parcel is pick up but not delivered. (parcel.PickUp !=null.parcel.Delivered == null)
                case (2)://PickedParcel //parcel was pickup already
                    {
                        initializeObjectsWhenDroneInDelivery(drone);
                        updateDrone(drone, (int)Simulation.DroneStatus.ToDelivery, distace);
                        Thread.Sleep(1000);
                        drone = calcDisAndSimulateDlivery(updateDrone, drone, target.CustomerPosition, BL.requestElectricity((int)parcel.Weight));
                        drone.ParcelInTransfer = null;
                        parcel.Delivered = DateTime.Now;
                        BL.changeParcelInfo(parcel);
                        parcel = null;
                        sender = null;
                        target = null;
                        updateDrone(drone, (int)Simulation.DroneStatus.Delivery, distace);
                        break;
                    }
                #endregion

                #region Parcel is delivered. Drone destination is a station (parcel.Delivered != null)
                case (3): //delivered
                    try
                    {
                        DO.Station s = BL.findAvailbleAndClosestStationForDrone(drone.DronePosition, drone.Battery);
                        Position stationPos = new Position() { Latitude = s.Latitude, Longitude = s.Longitude };
                        Thread.Sleep(1000);
                        updateDrone(drone, (int)Simulation.DroneStatus.ToCharge, distace);
                        Thread.Sleep(1000);
                        drone.Status = BO.DroneStatus.Maintenance;
                        drone = calcDisAndSimulateDlivery(updateDrone, drone, stationPos, BL.requestElectricity(0));
                        drone.SartToCharge = DateTime.Now;
                        BL.changeDroneInfo(drone);
                        updateDrone(drone, (int)DroneStatus.HideTextBlock , distace);
                    }
                    catch (ObjNotExistException e)//no available station
                    {
                        Thread.Sleep(2000);
                    }
                    break;
                    #endregion
            }

            #region Drone delivered parcel but needs to get to station to erase!!!
            ////Drone delivered parcel but needs to get to station
            //if (drone.DronePosition.Latitude == target.CustomerPosition.Latitude
            //    && drone.DronePosition.Longitude == target.CustomerPosition.Longitude 
            //    && parcel.Delivered != null)
            //{
            //    //try
            //    //{
            //    //    DO.Station s = BL.findAvailbleAndClosestStationForDrone(drone.DronePosition , drone.Battery);
            //    //    Position stationPos = new Position() { Latitude = s.Latitude, Longitude = s.Longitude };
            //    //    updateDrone(drone, 5);
            //    //    Thread.Sleep(1000);
            //    //    drone = calcDisAndSimulateDlivery(updateDrone, drone, stationPos, BL.requestElectricity(0));
            //    //    drone.Status = DroneStatus.Maintenance;
            //    //    drone.SartToCharge = DateTime.Now;
            //    //    BL.changeDroneInfo(drone);
            //    //    updateDrone(drone, -2);
            //    //}
            //    //catch (ObjNotExistException e)//no available station
            //    //{
            //    //    Thread.Sleep(2000);
            //    //}

            //}
            #endregion

            Thread.Sleep(1000);

        }

        /// <summary>
        /// initialize parcel , parcels' sender and parcels' target when drone in delivery;
        /// </summary>
        private void initializeObjectsWhenDroneInDelivery(Drone drone)
        {
            parcel = BL.convertDalToBLParcelSimulation(dal.getParcelWithSpecificCondition(p => p.Id == drone.ParcelInTransfer.Id).First());
            sender = BL.convertDalToBLCustomer(dal.getCustomerWithSpecificCondition(c => parcel.Sender.Id == c.Id).First());
            target = BL.convertDalToBLCustomer(dal.getCustomerWithSpecificCondition(c => parcel.Target.Id == c.Id).First());
        }

        private Drone calcDisAndSimulateDlivery(Action<Drone, int , double> updateDrone, Drone droneA, Position destination, double batteryUsage)
        {
            #region declare and implement variables
            double distanceDroneToSender = distance(droneA.DronePosition, destination);
            double batteryUsageByWeight = batteryUsage;//(int)parcel.Weight
            //double Incline = 0;
            //double b;
            double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;

            double changeXInDis; //drone x > costomer x // go back by x
            double changeYInDis; //drone y > costomer y // go back by x

            double x = droneA.DronePosition.Latitude;
            double y = droneA.DronePosition.Longitude;
            double dis;
            double batteryUsageByWeightForM;
            #endregion

            double fullDis = distance(droneA.DronePosition, destination);
            double sumBattery = fullDis * batteryUsage;
            //implement variables that are different in cases.

            #region Latitude is equal between drone and currentTarget
            if (droneA.DronePosition.Latitude == destination.Latitude)
            {
                changeXInDis = 0;
                changeYInDis = droneA.DronePosition.Longitude > destination.Longitude ? -1 : 1; //drone y > costomer y // go back by x
                //b = droneA.DronePosition.Longitude;
                dis = changeYInDis; //The differance is by Longitude
            }
            #endregion

            #region Longitude is equal between drone and currentTarget
            else if (droneA.DronePosition.Longitude == destination.Longitude)
            {
                changeXInDis = droneA.DronePosition.Latitude > destination.Latitude ? -1 : 1; //drone x > costomer x // go back by x
                changeYInDis = 0;
                //b = droneA.DronePosition.Longitude;
                dis = changeXInDis; //The differance is by Latitude
            }
            #endregion

            #region Position is dirrerent between drone and currentTarget
            else  //x = Latitude, y = Longitude.
            {
                //Incline = ((droneA.DronePosition.Longitude - sender.Longitude)
                //   / (droneA.DronePosition.Latitude - sender.Latitude));
                //#region calc
                ////y = Incline * x + b
                ////-b = Incline * x - y
                //#endregion
                //b = Incline * x - y;
                //b = -b;
                //double Equation = Incline * x + b;
                //changeYInDis = (Incline * (x + 1) + b)/*the Y2*/ - y/*Y1*/;

                double disInYPerUnit = Math.Abs(destination.Longitude - droneA.DronePosition.Longitude);
                double disInXPerUnit = Math.Abs(destination.Latitude - droneA.DronePosition.Latitude);
                changeXInDis = droneA.DronePosition.Latitude > destination.Latitude ? -1 : 1; //drone x > costomer x // go back by x
                changeYInDis = disInYPerUnit / disInXPerUnit;
                changeYInDis = droneA.DronePosition.Longitude > destination.Longitude ? (-1 * changeYInDis) : changeYInDis; //drone y > costomer y // go back by x
                dis = Math.Pow(Math.Pow(disInXPerUnit, 2) + Math.Pow(disInYPerUnit, 2), 0.5);
                dis = distance(droneA.DronePosition, new Position() { Latitude = x + changeXInDis, Longitude = y + changeYInDis }); //sqrt(dis^2 - x^2 ) = y
                //changeYInDis = Math.Pow((Math.Pow(dis,2) - Math.Pow(1,2)),0.5); //dis^2 = x^2 + y^2   // dis^2 - x^2 = y^2 // sqrt(dis^2 - x^2 ) = y


            }
            #endregion

            Position now = new Position() { Latitude = x, Longitude = y };
            Position currentPos;
            batteryUsageByWeightForM = (batteryUsageByWeight * dis);

            #region calc dis and display simulation
            while (sumBattery > 0 && fullDis > 0 )
            {
                x += changeXInDis;
                y += changeYInDis;

                currentPos = new Position() { Latitude = Math.Round(x, 2), Longitude = Math.Round(y, 2) };
                #region changes of drone to update by display
                droneA.DronePosition.Latitude = Math.Round(x, 2);
                droneA.DronePosition.Longitude = Math.Round(y, 2);
                droneA.DronePosition = currentPos;
                droneA.Battery -= batteryUsageByWeightForM;
                droneA.Battery = Math.Round(droneA.Battery, 1);
                droneA.Battery = Math.Min(100, droneA.Battery);//to erase
                droneA.Battery = Math.Max(0, droneA.Battery);//to erase
                BL.changeDroneInfo(droneA);
                sumBattery -= batteryUsageByWeightForM;
                fullDis -= dis;
                updateDrone(droneA,(int)DroneStatus.DisFromDestination, fullDis);
                #endregion
                Thread.Sleep((int)distanceDroneToSender*10);//500
            }
            #endregion

            droneA.DronePosition = destination;
            updateDrone(droneA, -1 , distace);
            return droneA;
        }

    }
}
