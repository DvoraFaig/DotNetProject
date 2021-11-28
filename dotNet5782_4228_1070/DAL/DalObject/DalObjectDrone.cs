using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;


namespace DalExceptions
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
        public void changeDroneInfo(int id , string newModel)
        {
            Drone dToChange = getDroneById(id);
            DataSource.Drones.Remove(dToChange);
            dToChange.Model = newModel;
            DataSource.Drones.Add(dToChange);
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
                return DataSource.Drones.First(drone => drone.Id == id);
            }
            catch (Exception )
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Drone), id);
            }
        }

        public void changeDroneInfo(Drone d)
        {
            DataSource.Drones.Remove(d);
            DataSource.Drones.Add(d);
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

        public Boolean IsDroneById(int id)
        {
            return DataSource.Drones.Any(d => d.Id == id);
        }
    }
}
