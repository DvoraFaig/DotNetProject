using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;


namespace IBL
{
    namespace BO
    {
        public class BLDrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; } //DroneStatus ?? the same name
            public BLParcelInTransfer ParcelInTransfer { get; set; }
            public BLPosition DronePosition { get; set; }
            public override string ToString()
            {
                if (DronePosition == null)
                    return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: {MaxWeight},drone battery: {Battery} , drone status{Status}");
                return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: {MaxWeight},drone battery: {Battery} , drone status{Status}\nDronePosition : {DronePosition}");
            }
        }

        public class BLDroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus droneStatus { get; set; } //DroneStatus ?? the same name
            public BLPosition DronePosition { get; set; }
            public int IdParcel { get; set; } //if there is
        }

        public class BLDroneInParcel //drone in pacel
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public BLPosition droneWithParcel { get; set; }
            public override string ToString()
            {
                return ($"id: {Id} , battery: {Battery},{droneWithParcel} ");
            }
        }

        public class BLChargingDrone
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return ($"ChargingDrone Id: {Id}, ChargingDrone Battery: {Battery}\n");
            }
        }
    }
}
