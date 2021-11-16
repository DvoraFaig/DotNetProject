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
        public class BLStation
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public BLPosition StationPosition { get; set; }
            public int DroneChargeAvailble { get; set; }
            public List<BLChargingDrone> DronesCharging { get; set; }
            public override string ToString()
            {
                return ($"station name: {Name}, station Id: {ID} , DroneChargeAvailble: {DroneChargeAvailble},\n station position: { StationPosition.ToString()}, ChargingDrone:Try...... "/* {ChargingDrone.ForEach(e => e.ToString())} */  );
            }

        }
        public class BLStationToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int DroneChargeAvailble { get; set; }
            public int DroneChargeOccupied { get; set; }
        }      
    }
}
