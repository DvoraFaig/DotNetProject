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
                                BL.changeDroneInfo(drone);
                            }
                            catch (ObjNotAvailableException)
                            {
                                //The Drone is ready to delivery but no matching parcels.
                                if (drone.Battery == 100)
                                {
                                    //cancle
                                }
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
                            catch (Exception )
                            {
                                Thread.Sleep(1000);
                                ///to stop
                            }
                            break;
                        }
                    case DroneStatus.Maintenance:
                        {

                            double BatteryLeftToFullCharge = 100 - drone.Battery;
                            double percentFillBatteryForCharging = BL.requestElectricity(0);
                            //double baterryToAdd = second.TotalMinutes * chargingRateOfDrone;
                            //baterryToAdd/chargingRateOfDrone = second.TotalMinutes;
                            double timeLeftToCharge = BatteryLeftToFullCharge / percentFillBatteryForCharging;
                            double a = Math.Ceiling(timeLeftToCharge);//int
                            while (a > 0 && drone.Battery < 100)
                            {
                                drone.Battery += percentFillBatteryForCharging * 10;
                                updateDrone(drone, 1);
                                Thread.Sleep(100);
                                a -= 100*percentFillBatteryForCharging; //-10
                            }

                            Thread.Sleep(100);

                            bool succeedFreeDroneFromCharge = false;
                            do
                            {
                                try
                                {
                                    BL.removeDroneChargeByDroneId(drone.Id); //BL.FreeDroneFromCharging(drone.Id);
                                    drone.Status = DroneStatus.Available;
                                    BL.changeDroneInfo(drone);
                                    updateDrone(drone, (int)percentFillBatteryForCharging);
                                    succeedFreeDroneFromCharge = true;
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(1000);
                                }
                            }
                            while (!succeedFreeDroneFromCharge);
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
                                        while (batteryToRemove > 0 && drone.Battery < 100)
                                        {
                                            drone.Battery -= batteryUsageByWeight *10;
                                            updateDrone(drone, 1);
                                            Thread.Sleep(100);
                                            batteryToRemove -= batteryUsageByWeight*10;
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
                        Thread.Sleep(10000);
                        break;

                }
            }
        }
    }
}
