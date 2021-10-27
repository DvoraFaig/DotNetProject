using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace DalObject
{
    public partial class DalObject : IBL.Ibl
    {
        //Drone//
        public static int amountDrones()
        {
            return DataSource.Drones.Count();
        }
        public void AddDrone(int id, string Model, WeightCategories MaxWeight, DroneStatus Status, double Battery)
        {
            Drone d = new Drone(id, Model, MaxWeight, Status, Battery);
            DataSource.Drones.Add(d);
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
    }
}
