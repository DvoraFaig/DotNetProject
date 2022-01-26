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
        public Simulation(Ibl BL)
        {
            this.BL = BL;
        }

        public void Start(Drone drone, Action<Drone, int> updateDrone, Func<bool> needToStop)
        {

            while (!needToStop())
            {
                switch (drone.Status)
                {
                    case DroneStatus.Available:
                        {
                            try
                            {
                                BO.Drone droneWithParcel = BL.PairParcelWithDrone(drone.Id);
                                drone.Status = DroneStatus.Delivery;
                                drone.ParcelInTransfer = droneWithParcel.ParcelInTransfer;
                                updateDrone(drone, 1);
                                BL.changeDroneInfo(drone);
                            }
                            catch (ObjNotAvailableException)
                            {
                                try
                                {
                                    BL.SendDroneToCharge(drone.Id);
                                    drone.Status = DroneStatus.Maintenance;
                                    updateDrone(drone, 1);
                                    BL.changeDroneInfo(drone);
                                }
                                catch (ObjNotExistException) //No charging slots available
                                {
                                    Thread.Sleep(5000);
                                }
                                catch (ObjNotAvailableException) //Not enough battery
                                {
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
                            Thread.Sleep(2000);
                            break;
                        }
                    case DroneStatus.Maintenance:
                        {
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

                            DateTime now = DateTime.Now;
                            TimeSpan second;
                            //TimeSpan second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 100;
                            //double baterryToAdd = second.TotalMinutes * BL.requestElectricity(0);
                            while (drone.Battery < 100)
                            {
                                second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 1000;//*1000
                                drone.SartToCharge = DateTime.Now;
                                double baterryToAdd = second.TotalMinutes * BL.requestElectricity(0);
                                drone.Battery += baterryToAdd;
                                drone.Battery = Math.Min(100, drone.Battery);
                                drone.Battery = Math.Max(0, drone.Battery);
                                drone.Battery = Math.Round(drone.Battery, 1);
                                updateDrone(drone, 1);
                                Thread.Sleep(100);
                            }

                            #region Free Drone From Charge.
                            bool succeedFreeDroneFromCharge = false;
                            do
                            {
                                try
                                { 
                                    BL.removeDroneChargeByDroneId(drone.Id); //BL.FreeDroneFromCharging(drone.Id);
                                    drone.Status = DroneStatus.Available;
                                    BL.changeDroneInfo(drone);
                                    updateDrone(drone, 1);
                                    succeedFreeDroneFromCharge = true;
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(1000);
                                }
                                Thread.Sleep(5000);
                            }
                            while (!succeedFreeDroneFromCharge);
                            #endregion
                            Thread.Sleep(1000);
                            break;
                        }
                    case DroneStatus.Delivery:
                        {
                            Parcel parcel = BL.convertDalToBLParcelSimulation(dal.getParcelWithSpecificCondition(p => p.Id == drone.ParcelInTransfer.Id).First());
                            Customer sender = BL.convertDalToBLCustomer(dal.getCustomerWithSpecificCondition(c => parcel.Sender.Id == c.Id).First());
                            Customer target = BL.convertDalToBLCustomer(dal.getCustomerWithSpecificCondition(c => parcel.Target.Id == c.Id).First());
                            DeliveryStatusAction droneStatus = BL.GetfromEnumDroneStatusInDelivery(drone.Id);
                            switch ((int)droneStatus)
                            {
                                case (1)://PickedParcel
                                    {
                                        drone = calcDisAndSimulateDlivery(updateDrone ,drone , sender.CustomerPosition, BL.requestElectricity(0));
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

                                        updateDrone(drone, 1);
                                        parcel.PickUp = DateTime.Now;
                                        BL.changeParcelInfo(parcel);
                                        //updateParcelInfo
                                        //BL.DronePicksUpParcel(drone.Id);
                                        break;
                                    }
                                case (2)://AsignedParcel //parcel was pickup already

                                    drone = calcDisAndSimulateDlivery(updateDrone, drone, target.CustomerPosition, BL.requestElectricity((int)parcel.Weight));
                                    parcel.Delivered = DateTime.Now;
                                    drone.ParcelInTransfer = null;
                                    BL.changeParcelInfo(parcel);
                                    updateDrone(drone, 1);
                                    Thread.Sleep(1000);

                                    //drone.DronePosition
                                    //BL.DeliveryParcelByDrone(drone.Id);
                                    break;
                            }

                            #region Drone delivered parcel but needs to get to station
                            //Drone delivered parcel but needs to get to station
                            if (drone.DronePosition.Latitude == target.CustomerPosition.Latitude
                                && drone.DronePosition.Longitude == target.CustomerPosition.Longitude 
                                && parcel.Delivered != null)
                            {
                                try
                                {
                                    DO.Station s = BL.findAvailbleAndClosestStationForDrone(drone.DronePosition , drone.Battery);
                                    Position stationPos = new Position() { Latitude = s.Latitude, Longitude = s.Longitude };
                                    drone = calcDisAndSimulateDlivery(updateDrone, drone, stationPos, BL.requestElectricity(0));
                                    drone.Status = DroneStatus.Maintenance;
                                    drone.SartToCharge = DateTime.Now;
                                    BL.changeDroneInfo(drone);
                                    updateDrone(drone, 1);
                                }
                                catch (ObjNotExistException e)//no available station
                                {
                                    Thread.Sleep(2000);
                                }

                            }
                            #endregion

                            Thread.Sleep(2000);
                            break;
                        }

                    default:
                        break;
                        Thread.Sleep(10000);

                }
            }
        }
        private Drone calcDisAndSimulateDlivery(Action<Drone, int> updateDrone, Drone droneA, Position sender , double batteryUsage)
        {
            #region declare and implement variables
            double distanceDroneToSender = distance(droneA.DronePosition, sender);
            double batteryUsageByWeight = batteryUsage;//(int)parcel.Weight
            double Incline = 0;
            double b;
            double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;

            double changeXInDis = droneA.DronePosition.Latitude > sender.Latitude ? -1 : 1; //drone x > costomer x // go back by x
            double changeYInDis = droneA.DronePosition.Longitude > sender.Longitude ? -1 : 1; //drone y > costomer y // go back by x

            double x = droneA.DronePosition.Latitude;
            double y = droneA.DronePosition.Longitude;
            double dis;
            double batteryUsageByWeightForM;
            #endregion

            //implement variables that are different in cases.

            #region Latitude is equal between drone and currentTarget
            if (droneA.DronePosition.Latitude == sender.Latitude)
            {
                changeXInDis = 0;
                b = droneA.DronePosition.Longitude;
                dis = changeYInDis; //The differance is by Longitude
            }
            #endregion

            #region Longitude is equal between drone and currentTarget
            else if (droneA.DronePosition.Longitude == sender.Longitude)
            {
                changeYInDis = 0;
                b = droneA.DronePosition.Longitude;
                dis = changeXInDis; //The differance is by Latitude
            }
            #endregion

            #region Position is dirrerent between drone and currentTarget
            else  //x = Latitude, y = Longitude.
            {
                Incline = ((droneA.DronePosition.Longitude - sender.Longitude)
                   / (droneA.DronePosition.Latitude - sender.Latitude));
                #region calc
                //y = Incline * x + b
                //-b = Incline * x - y
                #endregion
                b = Incline * x - y;
                b = -b;
                double Equation = Incline * x + b;
                changeYInDis = (Incline * (x + 1) + b)/*the Y2*/ - y/*Y1*/;
                dis = distance(droneA.DronePosition, new Position() { Latitude = x + 1, Longitude = changeYInDis });
            }
            #endregion

            Position now = new Position() { Latitude = x, Longitude = y };
            Position currentPos;
            batteryUsageByWeightForM = Math.Round(Math.Abs(batteryUsageByWeight * dis), 2);

            #region calc dis and display simulation
            while (droneA.Battery > 0 && !(x == sender.Latitude && y == sender.Longitude))
            {
                x += changeXInDis;
                y += changeYInDis;
          
                currentPos = new Position() { Latitude = x, Longitude = y };
                #region changes of drone to update by display
                droneA.DronePosition.Latitude = x;
                droneA.DronePosition.Longitude = y;
                droneA.DronePosition = currentPos;
                droneA.Battery -= batteryUsageByWeightForM;
                droneA.Battery = Math.Round(droneA.Battery, 1);
                droneA.Battery = Math.Min(100, droneA.Battery);//to erase
                droneA.Battery = Math.Max(0, droneA.Battery);//to erase
                updateDrone(droneA, 1);
                BL.changeDroneInfo(droneA);
                #endregion
                Thread.Sleep(500);
            }
            #endregion

            return droneA;
        }

    }
}
