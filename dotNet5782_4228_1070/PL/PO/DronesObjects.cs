using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Windows;
using System.ComponentModel;

namespace PO
{
    //[Serializable]
    public class Drone : DependencyObject //, ICloneable
    {


        public Drone(BlApi.Ibl blObject, BO.Drone d)
        {
            Id = d.Id;
            Model = d.Model;
            Battery = d.Battery;
            Status = d.Status;
            MaxWeight = (WeightCategories)d.MaxWeight;
            ParcelInTransfer = d.ParcelInTransfer;
            DronePosition = d.DronePosition;
            blObject.DroneChange += Update;
        }
            SartToCharge = d.SartToCharge;
    }

        public Drone(BlApi.Ibl blObject)
        {
            //Id = -1;
            //MaxWeight = Enum.GetValues(typeof(DO.WeightCategories));
            //blObject.DroneChange 
            blObject.DroneChange += Update;
        }
        public void Update(BO.Drone d)
        {
            Id = d.Id;
            Model = d.Model;
            Battery = d.Battery;
            Status = d.Status;
            ParcelInTransfer = d.ParcelInTransfer;
            DronePosition = d.DronePosition;
            MaxWeight = (WeightCategories)d.MaxWeight;
            //Update(senderDrone);
        }

        public BO.Drone BO()
        {
            return new BO.Drone()
            {
                Id = this.Id,
                Model = this.Model,
                Battery = this.Battery,
                Status = this.Status,
                MaxWeight = (DO.WeightCategories)this.MaxWeight,
                ParcelInTransfer = this.ParcelInTransfer,
                DronePosition = this.DronePosition,
                SartToCharge = this.SartToCharge
            };
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        /*private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public string Model
        {
            get { return (string)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
        //public WeightCategories MaxWeight { get; set; }
        public double Battery
        {
            get { return (double)GetValue(BatteryProperty); }
            set { SetValue(BatteryProperty, value); }
        }
        public DroneStatus Status
        {
            get { return (DroneStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        } //DroneStatus ?? the same name
        public BO.ParcelInTransfer ParcelInTransfer
        {
            get { return (BO.ParcelInTransfer)GetValue(ParcelInTransferProperty); }
            set { SetValue(ParcelInTransferProperty, value); }
        }
        public BO.Position DronePosition
        {
            get { return (BO.Position)GetValue(DronePositionProperty); }
            set { SetValue(DronePositionProperty, value); }
        }
        public WeightCategories MaxWeight
        {
            get { return (WeightCategories)GetValue(MaxWeightProperty); }
            set { SetValue(MaxWeightProperty, value); }
        }
        public DateTime? SartToCharge { set; get; }

        public override string ToString()
        {
            if (DronePosition == null)
                return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: /*MaxWeight*/, drone battery: {Battery} , drone status: {Status}");
            return ($"drone id: {Id}, drone model: {Model}, drone MaxWeight: /* MaxWeight*/ drone battery: {Battery} , drone status: {Status}\n\tDronePosition : {DronePosition}");
        }

        /// <summary>
        /// Update drones info.
        /// </summary>
        /// <param name="d"></param>
        /*public void Update(BO.Drone d)
        {
            Id = d.Id;
            Model = d.Model;
            Battery = d.Battery;
            Status = d.Status;
            ParcelInTransfer = d.ParcelInTransfer;
            DronePosition = d.DronePosition;
            MaxWeight = (WeightCategories)d.MaxWeight;
        }*/

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ParcelInTransferProperty = DependencyProperty.Register("ParcelInTransfer", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DronePositionProperty = DependencyProperty.Register("DronePosition", typeof(object), typeof(Drone), new UIPropertyMetadata(0));
        public static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("MaxWeight", typeof(object), typeof(Drone), new UIPropertyMetadata(0));

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

    /*public class ChargingDrone : DependencyObject
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return ($"ChargingDrone Id: {Id}, ChargingDrone Battery: {Battery}\n");
        }
    }*/
}

