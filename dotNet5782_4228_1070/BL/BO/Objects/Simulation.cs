using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DO;
using System.Threading;
using BO;
using static BL.BL;
using static BO.Exceptions;
using BlApi.Ibl;
using DalApi;


namespace BO
{
    class Simulation
    {
        /// <summary>
        /// Instance of Ibl interface.
        /// </summary>
        IBl Ibl;

        /// <summary>
        /// Instance of Idal interface.
        /// </summary>
        IDal Idal;

        /// <summary>
        /// Instance of Isimulation interface.
        /// </summary>
        ISimulation Isimulation;

        /// <summary>
        /// Drones' parcel when drone.Status = DroneStatus.Delivery.
        /// </summary>
        Parcel parcel;

        /// <summary>
        /// Drone parcels' sender customer when drone.Status = DroneStatus.Delivery.
        /// </summary>
        Customer sender;

        /// <summary>
        /// Drone parcels' target customer when drone.Status = DroneStatus.Delivery.
        /// </summary>
        Customer target;


        /// <summary>
        /// <param name="drone">The Drone</param>
        /// </summary>
        Drone drone;

        /// <summary>
        /// Func to update info in PL
        /// </summary>
        Action<Drone, DroneStatusInSim, double> updateDrone;


        /// <summary>
        /// Ctor Simulation
        /// </summary>
        /// <param name="BL">Interface Ibl</param>
        public Simulation(IBl BL, IDal idal , Drone drone, Action<Drone, DroneStatusInSim, double> updateDrone, Func<bool> needToStop)
        {
            this.Ibl = BL;
            this.Idal = idal;
            this.Isimulation = BlApi.Isimulation.ISimFactory.GetSimulation();
            distace = 0;
            StartSim(drone, updateDrone, needToStop);
        }

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
        public void StartSim(Drone drone, Action<Drone, DroneStatusInSim, double> updateDrone, Func<bool> needToStop)
        {
            this.drone = drone;
            this.updateDrone = updateDrone;
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
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        private void DroneStatusAvailable(Action<Drone, DroneStatusInSim, double> updateDrone, Drone drone, Func<bool> needToStop)
        {
            try
            {
                BO.Drone droneWithParcel = Ibl.PairParcelWithDrone(drone.Id);
                drone.Status = BO.DroneStatus.Delivery;
                drone.ParcelInTransfer = droneWithParcel.ParcelInTransfer;
                updateDrone(drone, DroneStatusInSim.HideTextBlock, distace); 
                Ibl.changeDroneInfoInDroneList(drone);
            }
            catch (ObjNotAvailableException)
            {
                try //not enough battery
                {
                    if (drone.Battery != 100)
                    {
                        sendDroneToCharge(updateDrone, drone);
                        DroneStatusMaintenance(updateDrone, drone);
                    }
                    else
                        Thread.Sleep(5000);
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
            catch (Exception)
            {
                Thread.Sleep(2000);
                ///to stop
            }
        }


        /// <summary>
        /// Simulation when drone.Status == DroneStatus.Maintenance
        /// Charging drone and than drone.Status == DroneStatus.Available
        /// </summary>
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        private void DroneStatusMaintenance(Action<Drone, DroneStatusInSim, double> updateDrone, Drone drone)
        {
            DateTime now = DateTime.Now;
            TimeSpan second;
            double baterryToAdd;
            double batteryPerTime = Ibl.requestElectricity(0);
           
            while (drone.Battery < 100)
            {
                second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 1000;
                drone.SartToCharge = DateTime.Now;
                baterryToAdd = second.TotalMinutes * batteryPerTime;
                drone.Battery += baterryToAdd;
                drone.Battery = Math.Min(100, drone.Battery);
                drone.Battery = Math.Max(0, drone.Battery);
                drone.Battery = Math.Round(drone.Battery, 1);
                updateDrone(drone, 0, 0);
                Ibl.changeDroneInfoInDroneList(drone);
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// freeing Drone From Charge.
        /// Changing drone status from DroneStatus.Maintenance to drone.Status. = DroneStatus.Available;
        /// </summary>
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        private void freeDroneFromCharge(Action<Drone, DroneStatusInSim, double> updateDrone, Drone drone)
        {
            bool succeedFreeDroneFromCharge = false;
            do
            {
                try
                {
                    Isimulation.removeDroneChargeByDroneId(drone.Id); //BL.FreeDroneFromCharging(drone.Id);
                    drone.Status = BO.DroneStatus.Available;
                    Ibl.changeDroneInfoInDroneList(drone);
                    updateDrone(drone, 0, distace);
                    succeedFreeDroneFromCharge = true;
                }
                catch (Exception)
                {
                    //addDroneCharge?? if falls by changeDroneInfoInDroneList
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
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        private void DroneStatusDelivery(Action<Drone, DroneStatusInSim, double> updateDrone, Drone drone)
        {
            DeliveryStatusAction droneStatus = Ibl.returnDroneStatusInDelivery(drone);
            switch ((int)droneStatus)
            {
                #region Parcel wasn't pick up. (parcel.Scheduled != null && parcel.PickUp ==null.)
                case (1):
                    {
                        initializeObjectsWhenDroneInDelivery(drone);
                        updateDrone(drone, DroneStatusInSim.ToPickUp, distace);
                        Thread.Sleep(1000);
                        drone = calcDisAndSimulateDlivery(updateDrone, drone, sender.CustomerPosition, Ibl.requestElectricity(0));
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
                        updateDrone(drone, DroneStatusInSim.PickUp, distace);
                        parcel.PickUp = DateTime.Now;
                        Isimulation.changeParcelInfo(parcel);
                        break;
                    }
                #endregion

                #region Parcel is pick up but not delivered. (parcel.PickUp !=null.parcel.Delivered == null)
                case (2): 
                    {
                        initializeObjectsWhenDroneInDelivery(drone);
                        updateDrone(drone, DroneStatusInSim.ToDelivery, distace);
                        Thread.Sleep(2000);
                        drone = calcDisAndSimulateDlivery(updateDrone, drone, target.CustomerPosition, Ibl.requestElectricity((int)parcel.Weight));
                        updateDrone(drone, DroneStatusInSim.Delivery, distace);
                        Thread.Sleep(2000);
                        drone.ParcelInTransfer = null;
                        parcel.Delivered = DateTime.Now;
                        updateDrone(drone, DroneStatusInSim.HideTextBlock, distace);
                        drone.Status = BO.DroneStatus.Available;
                        Ibl.changeDroneInfoInDroneList(drone);
                        Isimulation.changeParcelInfo(parcel);
                        Thread.Sleep(1000);
                        parcel = null;
                        sender = null;
                        target = null;
                        break;
                    }
                #endregion
            }
            Thread.Sleep(1000);

        }

        /// <summary>
        /// case 1: Parcel is delivered and drone destination is a station with an available charging slot.
        /// </summary>
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        private void sendDroneToCharge(Action<Drone, DroneStatusInSim, double> updateDrone, Drone drone)
        {
            DO.Station s = Ibl.findAvailbleAndClosestStationForDrone(drone.DronePosition, drone.Battery);
            Idal.AddDroneToCharge(new DO.DroneCharge() { DroneId = drone.Id, StationId = s.Id });
            drone.Status = BO.DroneStatus.Maintenance;
            Ibl.changeDroneInfoInDroneList(drone);
            updateDrone(drone, DroneStatusInSim.ToCharge, distace);
            Position stationPos = new Position() { Latitude = s.Latitude, Longitude = s.Longitude };
            drone = calcDisAndSimulateDlivery(updateDrone, drone, stationPos, Ibl.requestElectricity(0));
            drone.SartToCharge = DateTime.Now;
            Ibl.changeDroneInfoInDroneList(drone);
            updateDrone(drone, DroneStatusInSim.HideTextBlock, distace);
            DroneStatusMaintenance(updateDrone, drone);
            freeDroneFromCharge(updateDrone, drone);
        }

        /// <summary>
        /// initialize parcel , parcels' sender and parcels' target when drone in delivery;
        /// </summary>
        private void initializeObjectsWhenDroneInDelivery(Drone drone)
        {
            parcel = Isimulation.convertDalToBLParcelSimulation(Idal.getParcelWithSpecificCondition(p => p.Id == drone.ParcelInTransfer.Id).First());
            sender = Isimulation.convertDalToBLCustomer(Idal.getCustomerWithSpecificCondition(c => parcel.Sender.Id == c.Id).First());
            target = Isimulation.convertDalToBLCustomer(Idal.getCustomerWithSpecificCondition(c => parcel.Target.Id == c.Id).First());
        }

        /// <summary>
        /// Calc Distance for each movment. Send to dispaly.
        /// </summary>
        /// <param name="updateDrone"></param>
        /// <param name="droneA"></param>
        /// <param name="destination"></param>
        /// <param name="batteryUsage"></param>
        /// <returns></returns>
        private Drone calcDisAndSimulateDlivery(Action<Drone, DroneStatusInSim, double> updateDrone, Drone droneA, Position destination, double batteryUsage)
        {
            #region declare and implement variables
            double distanceDroneToSender = distance(droneA.DronePosition, destination);
            double batteryUsageByWeight = batteryUsage;
            double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;
            double changeXInDis; //drone x > costomer x // go back by x
            double changeYInDis; //drone y > costomer y // go back by x
            double x = droneA.DronePosition.Latitude;
            double y = droneA.DronePosition.Longitude;
            double dis;
            double batteryUsageByWeightForM;
            double fullDis = distance(droneA.DronePosition, destination);
            double sumBattery = fullDis * batteryUsage;
            #endregion

            #region Latitude is equal between drone and currentTarget
            if (droneA.DronePosition.Latitude == destination.Latitude)
            {
                changeXInDis = 0;
                changeYInDis = droneA.DronePosition.Longitude > destination.Longitude ? -1 : 1; 
                dis = changeYInDis; 
            }
            #endregion

            #region Longitude is equal between drone and currentTarget
            else if (droneA.DronePosition.Longitude == destination.Longitude)
            {
                changeXInDis = droneA.DronePosition.Latitude > destination.Latitude ? -1 : 1; 
                changeYInDis = 0;
                dis = changeXInDis; 
            }
            #endregion

            #region Position is dirrerent between drone and currentTarget
            else  //x = Latitude, y = Longitude.
            {
                double disInYPerUnit = Math.Abs(destination.Longitude - droneA.DronePosition.Longitude);
                double disInXPerUnit = Math.Abs(destination.Latitude - droneA.DronePosition.Latitude);
                changeXInDis = droneA.DronePosition.Latitude > destination.Latitude ? -1 : 1; 
                changeYInDis = disInYPerUnit / disInXPerUnit;
                changeYInDis = droneA.DronePosition.Longitude > destination.Longitude ? (-1 * changeYInDis) : changeYInDis;
                dis = Math.Pow(Math.Pow(disInXPerUnit, 2) + Math.Pow(disInYPerUnit, 2), 0.5);
                dis = distance(droneA.DronePosition, new Position() { Latitude = x + changeXInDis, Longitude = y + changeYInDis }); 
            }
            #endregion

            Position now = new Position() { Latitude = x, Longitude = y };
            Position currentPos;
            batteryUsageByWeightForM = (batteryUsageByWeight * dis);

            #region calc dis and display simulation
            while (sumBattery > 0 && fullDis > 0)
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
                Ibl.changeDroneInfoInDroneList(droneA);
                sumBattery -= batteryUsageByWeightForM;
                fullDis -= dis;
                updateDrone(droneA, DroneStatusInSim.DisFromDestination, fullDis);
                #endregion
                Thread.Sleep((int)distanceDroneToSender*2);
            }
            #endregion

            droneA.DronePosition = destination;
            updateDrone(droneA, DroneStatusInSim.HideTextBlock, distace);
            return droneA;
        }
    }
}
