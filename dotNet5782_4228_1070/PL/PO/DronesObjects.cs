using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Windows;

namespace PO
{
    public class Drone : DependencyObject
    {
        public Drone(BO.Drone d)
        {
            Id = d.Id;
            Model = d.Model;
            Battery = d.Battery;
            Status = d.Status;
            /*ParcelInTransfer = d.ParcelInTransfer;
            DronePosition = d.DronePosition;*/
        }
        public BO.Drone BO()
        {
            return new BO.Drone()
            {
                Id = this.Id,
                Model = this.Model,
                Battery = this.Battery,
                Status = this.Status
                /*ParcelInTransfer = d.ParcelInTransfer,
                DronePosition = d.DronePosition*/
            };
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public string Model { get; set; }
        //public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatus Status { get; set; } //DroneStatus ?? the same name
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public Position DronePosition { get; set; }

        public override string ToString()
        {
            if (DronePosition == null)
                return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: /*MaxWeight*/, drone battery: {Battery} , drone status: {Status}");
            return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: /* MaxWeight*/ drone battery: {Battery} , drone status: {Status}\n\tDronePosition : {DronePosition}");
        }

        public static readonly DependencyProperty IdProperty =
        DependencyProperty.Register("Id",
                                    typeof(object),
                                    typeof(Drone),
                                    new UIPropertyMetadata(0));
    }

    public class DroneToList : DependencyObject
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public DO.WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatus droneStatus { get; set; }
        public Position DronePosition { get; set; }
        public int IdParcel { get; set; } //if there is
                                          //public override string ToString()
                                          //{
                                          //if (DronePosition == null)
                                          //return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: {MaxWeight}, drone battery: {Battery} , drone status: {droneStatus}");
                                          //if (IdParcel == 0)
                                          //return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: {MaxWeight},drone battery: {Battery} , drone status: {droneStatus}\n\tDronePosition : {DronePosition} , Parcel Id: -- no parcel -- ");
                                          //return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: {MaxWeight},drone battery: {Battery} , drone status: {droneStatus}\n\tDronePosition : {DronePosition}");
                                          //}
    }

    public class DroneInParcel : DependencyObject //drone in pacel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Position droneWithParcel { get; set; }
        public override string ToString()
        {
            return ($"id: {Id} , battery: {Battery},\n\tdrone position: {droneWithParcel} \n");
        }
    }

    public class ChargingDrone : DependencyObject
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return ($"ChargingDrone Id: {Id}, ChargingDrone Battery: {Battery}\n");
        }
    }
}

