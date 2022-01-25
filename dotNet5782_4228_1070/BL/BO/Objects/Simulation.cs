using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DO;
using System.Threading;


namespace BO
{
    public class Simulation
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
                if (drone.Status == DroneStatus.Available)
                {
                    try
                    {
                        BL.PairParcelWithDrone(drone.Id);
                        Drone changeDrone = BL.GetDroneById(drone.Id);
                        changeDrone = drone;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(1000);
                        ///to stop
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

                    //Drone changeDrone = BL.GetDroneById(drone.Id);
                    //changeDrone = drone;
                    bool succeedFreeDroneFromCharge = false;
                    do 
                    {
                        try
                        {
                            //BL.FreeDroneFromCharging(drone.Id);
                            BL.removeDroneChargeByDroneId(drone.Id);
                            drone.Status = DroneStatus.Available;
                            BL.changeDroneInfo(drone);
                            updateStudent(drone, (int)percentFillBatteryForCharging);
                            succeedFreeDroneFromCharge = true;
                        }
                        catch (Exception) { }
                    }
                    while (!succeedFreeDroneFromCharge);
                    
                }
                if (drone.Status == DroneStatus.Delivery)
                {

                }
            }
        }
    }
}
