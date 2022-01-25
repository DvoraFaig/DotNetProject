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
            int index = 0;
            while (!needToStop())
            {
                Thread.Sleep(1000);
                if (drone.Status == DroneStatus.Available)
                {
                    try
                    {
                        BL.PairParcelWithDrone(drone.Id);
                        Drone changeDrone = BL.GetDroneById(drone.Id);
                        changeDrone = drone;
                    }
                    catch (ObjNotAvailableException)
                    {
                        try
                        {
                            BL.SendDroneToCharge(drone.Id);
                        }
                        catch (ObjNotAvailableException)
                        {
                            // Not implemented becuase need to do other actions
                            // in the next interation on the while loop
                        }
                    }
                    // Do: If drone not has enough buttery to go to the near station.... do something
                    catch (Exception e)
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (drone.Status == DroneStatus.Maintenance)
                {
                    double BatteryLeftToFullCharge = 100 - drone.Battery;
                    double percentFillBatteryForCharging = BL.requestElectricity(0);
                    double timeLeftToCharge = BatteryLeftToFullCharge / percentFillBatteryForCharging;
                    while (drone.Battery <100 && timeLeftToCharge>0)
                    {
                        drone.Battery += percentFillBatteryForCharging*10;
                        updateStudent(drone, 1);
                        Thread.Sleep(100);
                        timeLeftToCharge--;
                    }
                    Thread.Sleep(100);
                    //BL.changeDroneInfo(drone);

                    //Drone changeDrone = BL.GetDroneById(drone.Id);
                    //changeDrone = drone;
                    bool succeedFreeDroneFromCharge = false;
                    do
                    {
                        try
                        {
                            BL.FreeDroneFromCharging(drone.Id);
                            drone.Status = DroneStatus.Available;
                            updateStudent(drone, (int)percentFillBatteryForCharging);
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
