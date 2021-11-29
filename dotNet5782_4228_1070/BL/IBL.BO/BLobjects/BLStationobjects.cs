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
            public object ChargingDrone { get; private set; }//??????????????????????????
            public override string ToString()
            {
                return $"station name: {Name}, station Id: {ID} , DroneChargeAvailble: {DroneChargeAvailble},\n\t{StationPosition.ToString()}, \tChargingDrone: { string.Join(", ", ChargingDrone)}";
            }

        }
        public class BLStationToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int DroneChargeAvailble { get; set; }
            public int DroneChargeOccupied { get; set; }
        }
        public class DistanceFromStation
        {
            public IDal.DO.Station Station_ { get; set; }
            public double DistanceFromGivenPosotion { get; set; }
        }
    }
}
