using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PO
{
    public class Station : DependencyObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Position StationPosition { get; set; }
        public int DroneChargeAvailble { get; set; }
        public List<ChargingDrone> DronesCharging { get; set; }
        //public object ChargingDrone { get; private set; }//??????????????????????????
        public override string ToString()
        {
            return $"station name: {Name}, station Id: {ID} , DroneChargeAvailble: {DroneChargeAvailble},\n\t{StationPosition.ToString()}, \tChargingDrone: { string.Join(", ", DronesCharging)}";
        }

    }
    
    public class BLStationToList : DependencyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DroneChargeAvailble { get; set; }
        public int DroneChargeOccupied { get; set; }
    }
    
    public class DistanceFromStation : DependencyObject
    {
        public DO.Station Station_ { get; set; }
        public double DistanceFromGivenPosotion { get; set; }
    }
}

