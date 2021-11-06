using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDal;

namespace DalObject
{
    public partial class DalObject 
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
      
        /// <summary>
        /// ???????????????????????
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DroneCharge getDroneChargeByDroneId(int id)
        {
            return DataSource.DroneCharges.FirstOrDefault(charge => charge.DroneId == id);
        }
        //Update functions//
        //find an availeble drone and send it to the sending costumer.
        //Pair between a customer and parcel to a drone;
        /*public void assignParcelToDrone(int parcelId)
        {
            Parcel p = getParcelById(parcelId);
            for (int i = 0; i < DataSource.Drones.Count; i++)
            {
                if (DataSource.Drones[i].Status == DroneStatus.Available)
                {

                }
            }
            foreach(Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available)
                {
                    
                }
            }
            
        }*/

        /*        private bool isFreeDrone(Drone d)
                {
                    if (d.Status == DroneStatus.Available)
                    {
                        return true;
                    }
                    return false;
                }*/
        public string PairAParcelWithADrone(Parcel parcel)
        {
            for (int i = 0; i < DataSource.Drones.Count();/* ??? לא היה כתוב i++ */ i++)
            {
                if (DataSource.Drones[i].Status == DroneStatus.Available && (WeightCategories)DataSource.Drones[i].MaxWeight >= parcel.Weight)
                {

                }

            }
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available && (WeightCategories)drone.MaxWeight >= parcel.Weight)
                {
                    parcel.DroneId = drone.Id;
                    parcel.Scheduled = DateTime.Now; //pair a parcel to dron
                    int indexDrone = drone.Id;
                    //?????????????????????????????????????????????????
                    //change to list........
                    //DataSource.Drones[indexDrone].Status = DroneStatus.Delivery; // can't change info by foreach - drone.Status = DroneStatus.Delivery;
                    //////////////////////////
                    return $"The Drone number{drone.Id} is ready and will receive parcel num {parcel.Id}.";
                }
            }
            return ("No drones available.\n please try later.");
        }

        public void DroneCollectsAParcel(Parcel parcel)
        {
            if (parcel.DroneId == 0) //doesn't have a Drone
            {
                Console.WriteLine("Error! The parcel doesn't have a Drone.\n Please enter DroneReceivesParcel");
            }
            else
            {
                parcel.PickUp = DateTime.Now;
                Drone droneCollect = DalObject.getDroneById(parcel.DroneId);
                droneCollect.Status = DroneStatus.Delivery;
            }
        }
        public void CostumerGetsParcel(Drone drone, Parcel parcel)
        {
            parcel.Delivered = DateTime.Now;
            drone.Status = DroneStatus.Available;
        }
        public void sendDroneToCharge(Drone drone)
        {
            IEnumerable<DroneCharge> droneCharges = displayDroneCharge();
            Console.WriteLine("Choose id of Station to charge The drone");
            foreach (DroneCharge charge in droneCharges)
            {
                Console.WriteLine(charge.ToString());
            }
            int choose = Convert.ToInt32(Console.ReadLine());
            DroneCharge droneCharge = getDroneChargeById(choose);
            if (droneCharge.StationId != -1)
            {
                drone.Status = DroneStatus.Maintenance;
                droneCharge.DroneId = drone.Id;
            }
        }
        public void freeDroneFromCharge(Drone drone)
        {
            drone.Status = DroneStatus.Available;
            drone.Battery = 100;
            DroneCharge chargeToFree = getDroneChargeByDroneId(drone.Id);
            chargeToFree.StationId = -1;
        }

    }
}
  


