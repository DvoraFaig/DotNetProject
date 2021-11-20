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
        public int amountDrones()
        {
            return DataSource.Drones.Count();
        }
        public void AddDrone(int id, string Model, IDal.DO.WeightCategories MaxWeight)
        {
            Drone drone = new Drone();
            drone.Id = id;
            drone.Model = Model;
            drone.MaxWeight = MaxWeight;
            DataSource.Drones.Add(drone);
        }
        public void AddDrone(Drone drone)
        {
            DataSource.Drones.Add(drone);
        }
        public void changeDroneInfo(Drone d)
        {
            Drone dToChange = getDroneById(d.Id);
            dToChange = d;
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
        public Drone getDroneById(int id)
        {
            try
            {
                return DataSource.Drones.FirstOrDefault(drone => drone.Id == id);
            }
            catch (Exception e)
            {
                throw new DalExceptions.ObjNotExistException(typeof(Drone), id);
            }
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
