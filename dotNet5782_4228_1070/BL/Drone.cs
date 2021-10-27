using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatus Status { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return ($"drone id: {Id}, drone model{Model}, drone MaxWeight {MaxWeight}, drone status{Status}, drone battery {Battery}\n");
        }
    }
}
