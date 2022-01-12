using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;

namespace PO
{
    public class Station : DependencyObject
    {
        public Station(BO.Station d)
        {
            Id = d.Id;
            Name = d.Name;
            StationPosition = d.StationPosition != null ? d.StationPosition : null;
            DroneChargeAvailble = d.DroneChargeAvailble;
            DronesCharging = d.DronesCharging != null ? d.DronesCharging : null;
        }

        public Station()
        {

        }

        public BO.Station BO()
        {
            return new BO.Station()
            {
                Id = this.Id,
                Name = this.Name,
                StationPosition = this.StationPosition != null ? this.StationPosition : null,
                DroneChargeAvailble = this.DroneChargeAvailble,
                DronesCharging = this.DronesCharging != null ? this.DronesCharging : null,
            };
        }

        public void Update(BO.Station d)
        {
            Id = d.Id;
            Name = d.Name;
            StationPosition = d.StationPosition != null ? d.StationPosition : null;
            DroneChargeAvailble = d.DroneChargeAvailble;
            DronesCharging = d.DronesCharging != null ? d.DronesCharging : null;
        }

        /*public BO.Station BO()
        {
            return new BO.Station()
            {
            };
        }*/

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public Position StationPosition
        {
            get { return (Position)GetValue(StationPositionProperty); }
            set { SetValue(StationPositionProperty, value); }
        }
        public int DroneChargeAvailble
        {
            get { return (int)GetValue(DroneChargeAvailbleProperty); }
            set { SetValue(DroneChargeAvailbleProperty, value); }
        }
        public List<ChargingDrone> DronesCharging
        {
            get { return (List<ChargingDrone>)GetValue(DronesChargingProperty); }
            set { SetValue(DronesChargingProperty, value); }
        }
        //public object ChargingDrone { get; private set; }//??????????????????????????
        public override string ToString()
        {
            return $"station name: {Name}, station Id: {Id} , DroneChargeAvailble: {DroneChargeAvailble},\n\t{StationPosition.ToString()}, \tChargingDrone: { string.Join(", ", DronesCharging)}";
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty StationPositionProperty = DependencyProperty.Register("StationPosition", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DroneChargeAvailbleProperty = DependencyProperty.Register("DroneChargeAvailble", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DronesChargingProperty = DependencyProperty.Register("DronesCharging", typeof(object), typeof(Drone), new UIPropertyMetadata(0));

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

