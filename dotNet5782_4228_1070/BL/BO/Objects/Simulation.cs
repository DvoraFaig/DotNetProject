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

namespace BO
{
    class Simulation
    {
        Ibl BL;
        public Simulation(Ibl BL)
        {
            this.BL = BL;
        }

        public void Start(Drone drone, Action<Drone, int> updateStudent, Func<bool> needToStop)
        {

            while (!needToStop())
            {
                Thread.Sleep(1000);
                if (drone.Status == DroneStatus.Available)
                {
                    try
                    {
                        BL.PairParcelWithDrone(drone.Id);
                        drone.Status = DroneStatus.Delivery;
                        updateStudent(drone, 1);
                        BL.changeDroneInfo(drone);
                    }
                    catch (ObjNotAvailableException)
                    {
                        try
                        {
                            BL.SendDroneToCharge(drone.Id);
                            drone.Status = DroneStatus.Maintenance;
                            updateStudent(drone, 1);
                            BL.changeDroneInfo(drone);
                        }
                        catch (ObjNotExistException) //No charging slots available
                        {
                            Thread.Sleep(5000);
                        }
                        catch (ObjNotAvailableException) //Not enough battery
                        {
                            
                            // Not implemented becuase need to do other actions
                            // in the next interation on the while loop
                        }
                    }
                    // Do: If drone not has enough buttery to go to the near station.... do something
                    catch (Exception e)
                    {
                        Thread.Sleep(1000);
                        ///to stop
                    }
                }

                if (drone.Status == DroneStatus.Maintenance)
                {

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
                        updateStudent(drone, 1);
                        Thread.Sleep(500);
                    }

                    #region a differance way to impelement;
                    ////double BatteryLeftToFullCharge = 100 - drone.Battery;
                    ////double percentFillBatteryForCharging = BL.requestElectricity(0);
                    ////double timeLeftToCharge = BatteryLeftToFullCharge / percentFillBatteryForCharging;
                    ////while (drone.Battery < 100 && timeLeftToCharge > 0)
                    ////{
                    ////    drone.Battery += percentFillBatteryForCharging * 10;
                    ////    updateStudent(drone, 1);
                    ////    Thread.Sleep(100);
                    ////    timeLeftToCharge--;
                    ////}
                    ////Thread.Sleep(100);

                    //Drone changeDrone = BL.GetDroneById(drone.Id);
                    //changeDrone = drone;
                    #endregion
                    bool succeedFreeDroneFromCharge = false;
                    do
                    {
                        try
                        {
                            //BL.FreeDroneFromCharging(drone.Id);
                            BL.removeDroneChargeByDroneId(drone.Id);
                            drone.Status = DroneStatus.Available;
                            BL.changeDroneInfo(drone);
                            updateStudent(drone, 1);//(int)percentFillBatteryForCharging
                            succeedFreeDroneFromCharge = true;
                        }
                        catch (Exception) { }
                    }
                    while (!succeedFreeDroneFromCharge);

                }

                if (drone.Status == DroneStatus.Delivery)
                {
                    DeliveryStatusAction droneStatus = BL.GetEnumDroneStatusInDelivery(drone.Id);
                    switch ((int)droneStatus)
                    {
                        case (1)://PickedParcel
                            BL.DronePicksUpParcel(drone.Id);
                            break;
                        case (2)://AsignedParcel
                            BL.DeliveryParcelByDrone(drone.Id);
                            break;
                    }
                }
            }
        }
    }
}
