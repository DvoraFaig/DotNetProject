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
        public static int amountDrones()
        {
            return DataSource.Drones.Count();
        }
        public void AddDrone(int id, string Model, IDal.DO.WeightCategories MaxWeight, IDal.DO.DroneStatus Status, double Battery)
        {
            Drone drone = new Drone();
            drone.Id = id;
            drone.Model = Model;
            //drone.MaxWeight = MaxWeight;
            //drone.Status = Status;
            drone.Battery = Battery;
            DataSource.Drones.Add(drone);
        }
        public IEnumerable<Drone> displayDrone()
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id != 0)
                {
                    yield return drone;
                }
            }
        }
        public static Drone getDroneById(int id)
        {
            return DataSource.Drones.FirstOrDefault(drone => drone.Id == id);
        }
        public double[] electricityUseByDrone(Drone drone)
        {
            double[] droneInfo = new double[5];
            droneInfo[0] = DataSource.Config.empty;
            droneInfo[1] = DataSource.Config.lightWeight;
            droneInfo[2] = DataSource.Config.mediumWeight;
            droneInfo[3] = DataSource.Config.heavyWeight;
            droneInfo[4] = DataSource.Config.chargingRate;
            return droneInfo;
        }
    }
}
