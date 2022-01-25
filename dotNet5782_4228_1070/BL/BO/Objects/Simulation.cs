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

        public void start(Drone drone, Action<Drone, int> updateStudent, Func<bool> needToStop)
        {
            int index = 0;
            while (!needToStop())
            {
                if (drone.Status == DroneStatus.Available)
                {
                    try
                    {
                        BL.PairParcelWithDrone(drone.Id);
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(1000);
                    }

                }

                if (drone.Status == DroneStatus.Maintenance)
                {
                    double BatteryLeftToFullCharge = 100 - drone.Battery;
                    double batteryFillForCharging = BL.requestElectricity(0);
                    double timeLeftToCharge = BatteryLeftToFullCharge / batteryFillForCharging;
                    while (drone.Battery <100 && timeLeftToCharge>0)
                    {
                        drone.Battery+=0.1;
                        updateStudent(drone, (int)batteryFillForCharging);
                        Thread.Sleep(10);
                        timeLeftToCharge--;
                        if(drone.Battery < 100 && timeLeftToCharge > 0)
                        {

                        }
                    }
                    Thread.Sleep(100);
                    Drone changeDrone = BL.GetDroneById(drone.Id);
                    changeDrone = drone;
                    bool succeedFreeDroneFromCharge = false;
                    do 
                    {
                        try
                        {
                            BL.FreeDroneFromCharging(drone.Id);
                            drone.Status = DroneStatus.Available;
                            updateStudent(drone, (int)batteryFillForCharging);
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
