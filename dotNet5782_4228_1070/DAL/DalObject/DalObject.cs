using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        static DalObject instance;
        public static DalObject GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new DalObject();
                return instance;
            }
        }
        public double[] electricityUseByDrone()
        {
            double[] arr = { DataSource.Config.empty, DataSource.Config.lightWeight, DataSource.Config.mediumWeight, DataSource.Config.heavyWeight, DataSource.Config.chargingRate };
            return arr;
        }
        /// <summary>
        /// Function get id of drone and return drone charge object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        
        /// <summary>
        /// Function get parcel object and find drone to take it.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public string PairAParcelWithADrone(Parcel parcel)
        {
            for (int i = 0; i < DataSource.Drones.Count(); i++)
            {
                //if (DataSource.Drones[i].Status == DroneStatus.Available && (WeightCategories)DataSource.Drones[i].MaxWeight >= parcel.Weight)
                //{
                //    Drone d = DataSource.Drones[i];
                //    parcel.DroneId = DataSource.Drones[i].Id;
                //    parcel.Scheduled = DateTime.Now; //pair a parcel to dron
                //    d.Status = DroneStatus.Delivery;
                //    DataSource.Drones[i] = d;
                //    return $"The Drone number {d.Id} is ready and will recieve parcel num {parcel.Id}.";
                //}
            }
            return ("No drones available.\n please try later.");
        }
        public void DroneCollectsAParcel(Parcel parcel)
        {
            if (parcel.DroneId == null) //doesn't have a Drone 
            {
                Console.WriteLine("Error! The parcel doesn't have a Drone.\n Please enter DroneReceivesParcel");
            }
            else
            {
                parcel.PickUp = DateTime.Now;
                Drone droneCollect = getDroneWithSpecificCondition(d => d.Id == (int)parcel.DroneId).First();
            }
        }
        public void CostumerGetsParcel(Drone drone, Parcel parcel)
        {
            parcel.Delivered = DateTime.Now;
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
            DroneCharge droneCharge = getDroneChargeWithSpecificCondition(d => d.StationId == choose).First();
            if (droneCharge.StationId != -1)
            {
                droneCharge.DroneId = drone.Id;
            }
        }
        public void freeDroneFromCharge(Drone drone)
        {
            DroneCharge chargeToFree = getDroneChargeWithSpecificCondition(d=> d.DroneId == drone.Id).First();
            chargeToFree.StationId = -1;
        }
    }
}
  


