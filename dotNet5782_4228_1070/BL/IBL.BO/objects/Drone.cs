using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;


namespace IBL
{
    namespace BO {
        public class Drone
        {
            IDal.DO.Drone drone;
            public Drone(int id, string Model, WeightCategories MaxWeight, DroneStatus Status, double Battery)
            {
                drone = new IDal.DO.Drone();
                drone.Id = id;
                drone.Model = Model;
                this.MaxWeight = MaxWeight;
                this.Status = Status;
                drone.Battery = Battery;
            }           
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }

            public override string ToString()
            {
                return ($"drone id: {drone.Id}, drone model{drone.Model}, drone MaxWeight {MaxWeight}, drone status{Status}, drone battery {drone.Battery}\n");
            }


            //   public Drone (int id, string Model, WeightCategories MaxWeight, DroneStatus Status, double Battery)
            //   {
            //       this.Id = id;
            //       this.Model = Model;
            //       this.MaxWeight = MaxWeight;
            //       this.Status = Status;
            //       this.Battery = Battery;
            //}
        }
    }
}
