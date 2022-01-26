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
                                BL.PairParcelWithDrone(drone.Id);
                                drone.Status = DroneStatus.Delivery;
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
                                Thread.Sleep(5000);
                                ///to stop
                            }
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
                                second = (TimeSpan)(DateTime.Now - drone.SartToCharge) * 1000;
                                drone.SartToCharge = DateTime.Now;
                                double baterryToAdd = second.TotalMinutes * BL.requestElectricity(0);
                                drone.Battery += baterryToAdd;
                                drone.Battery = Math.Round(drone.Battery, 1);
                                drone.Battery = Math.Min(100, drone.Battery);
                                updateDrone(drone, 1);
                                Thread.Sleep(500);
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
                            }
                            while (!succeedFreeDroneFromCharge);
                            #endregion
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
                                        double distanceDroneToSender = distance(drone.DronePosition, sender.CustomerPosition);
                                        double batteryUsageByWeight = BL.requestElectricity((int)parcel.Weight);
                                        //double batteryToRemove = distanceDroneToSender * batteryUsageByWeight(per meters per minute...)
                                        double batteryToRemove = distanceDroneToSender * batteryUsageByWeight;
                                        //x = Latitude, y = Longitude.
                                        double Incline = (Math.Abs(drone.DronePosition.Longitude - sender.CustomerPosition.Longitude)
                                            / Math.Abs(drone.DronePosition.Latitude - sender.CustomerPosition.Latitude));
                                        double x=drone.DronePosition.Latitude;
                                        double y=drone.DronePosition.Longitude;
                                        #region calc
                                        //y = Incline * x + b
                                        //-b = Incline * x - y
                                        #endregion
                                        double b = Incline * x - y;
                                        b = -b;
                                        double Equation = Incline * x + b;


                                        while (batteryToRemove > 0 && drone.Battery < 100 && !(x == sender.CustomerPosition.Latitude))
                                        {
                                            x++;
                                            y = Incline * x + b;
                                            drone.DronePosition.Latitude = x;
                                            drone.DronePosition.Longitude = y;
                                            drone.Battery -= batteryUsageByWeight * 10;
                                            updateDrone(drone, 1);
                                            Thread.Sleep(100);
                                            batteryToRemove -= batteryUsageByWeight * 10;
                                            BL.changeDroneInfo(drone);
                                        }
                                        BL.DronePicksUpParcel(drone.Id);
                                        break;
                                    }
                                case (2)://AsignedParcel
                                    BL.DeliveryParcelByDrone(drone.Id);
                                    break;
                            }
                            break;
                        }

                    default:
                        break;
                        Thread.Sleep(10000);

                }
            }
        }
    }
}
