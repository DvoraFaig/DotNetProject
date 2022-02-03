using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using System.Windows;
using System.ComponentModel;

namespace PO
{
    public class Drones : DependencyObject
    {
        public Drones(BlApi.IBl blObject)
        {
            DroneList = new List<DroneToList>();
            blObject.DroneListChangeAction += Update;
        }


        public List<DroneToList> DroneList
        {
            get { return (List<DroneToList>)GetValue(DroneListProperty); }
            set { SetValue(DroneListProperty, value); }
        }
        public static readonly DependencyProperty DroneListProperty = DependencyProperty.Register("DroneList", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));

        /// <summary>
        /// Update DroneList, add new drone to list or update exist drone - according to isNew boolean param.
        /// </summary>
        /// <param name="updatedDrone"></param>
        /// <param name="isNew">To add new drone to list send 'True', to update send 'False'.</param>
        public void Update(BO.DroneToList updatedDrone, bool isNew)
        {
            if (!updatedDrone.Equals(null))
            {
                if (isNew)
                {
                    DroneList.Add(new DroneToList(updatedDrone));
                }
                else
                {
                    int index = DroneList.FindIndex(d => d.Id == updatedDrone.Id);
                    DroneList[index].Update(updatedDrone);
                }
            }
        }

        internal void getNewList(IEnumerable<BO.DroneToList> drones)
        {
            if (!drones.Equals(null))
            {
                DroneList.Clear();
                foreach (BO.DroneToList drone in drones)
                {
                    DroneList.Add(new DroneToList(drone));
                }
            }
        }
    }

    //[Serializable]
    public class Drone : DependencyObject //, ICloneable
    {
        public Drone(BlApi.IBl blObject, BO.Drone d)
        {
            Id = d.Id;
            Model = d.Model;
            Battery = d.Battery;
            Status = d.Status;
            MaxWeight = d.MaxWeight;
            ParcelInTransfer = d.ParcelInTransfer;
            DronePosition = d.DronePosition;
            SartToCharge = d.SartToCharge;
            blObject.DroneChangeAction += Update;
        }


        public Drone(BlApi.IBl blObject)
        {
            //Id = -1;
            //MaxWeight = Enum.GetValues(typeof(DO.WeightCategories));
            //blObject.DroneChange 
            blObject.DroneChangeAction += Update;
        }

        /// <summary>
        /// Update drones info.
        /// </summary>
        /// <param name="updatedDrone"></param>
        public void Update(BO.Drone updatedDrone)
        {
            Id = updatedDrone.Id;
            Model = updatedDrone.Model;
            Battery = updatedDrone.Battery;
            Status = updatedDrone.Status;
            ParcelInTransfer = updatedDrone.ParcelInTransfer;
            DronePosition = updatedDrone.DronePosition;
            MaxWeight = updatedDrone.MaxWeight;
            //Update(senderDrone);
        }

        /// <summary>
        /// Ctor - copy info to PO.Drone
        /// </summary>
        /// <returns></returns>
        public BO.Drone BO()
        {
            return new BO.Drone()
            {
                Id = this.Id,
                Model = this.Model,
                Battery = this.Battery,
                Status = this.Status,
                MaxWeight = (BO.WeightCategories)this.MaxWeight,
                ParcelInTransfer = this.ParcelInTransfer,
                DronePosition = this.DronePosition,
                SartToCharge = this.SartToCharge
            };
        }

        /// <summary>
        /// Update PO.Drone - copy info to PO.Drone
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
            DronePosition.Latitude = d.DronePosition.Latitude;
            DronePosition.Longitude = d.DronePosition.Longitude;
            MaxWeight = (WeightCategories)d.MaxWeight;
            //Update(senderDrone);
        }*/


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
        public BO.DroneStatus Status
        {
            get { return (BO.DroneStatus)GetValue(StatusProperty); }
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
        public BO.WeightCategories MaxWeight
        {
            get { return (BO.WeightCategories)GetValue(MaxWeightProperty); }
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
        public DroneToList(BO.DroneToList drone)
        {
            Id = drone.Id;
            Model = drone.Model;
            MaxWeight = drone.MaxWeight;
            Battery = drone.Battery;
            Status = drone.droneStatus;
            DronePosition = drone.DronePosition;
            IdParcel = drone.IdParcel;
        }
        public void Update(BO.DroneToList drone)
        {
            Id = drone.Id;
            Model = drone.Model;
            MaxWeight = drone.MaxWeight;
            Battery = drone.Battery;
            Status = drone.droneStatus;
            DronePosition = drone.DronePosition;
            IdParcel = drone.IdParcel;
        }

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
        public BO.WeightCategories MaxWeight
        {
            get { return (BO.WeightCategories)GetValue(MaxWeightProperty); }
            set { SetValue(MaxWeightProperty, value); }
        }
        public double Battery
        {
            get { return (double)GetValue(BatteryProperty); }
            set { SetValue(BatteryProperty, value); }
        }
        public BO.DroneStatus Status
        {
            get { return (BO.DroneStatus)GetValue(droneStatusProperty); }
            set { SetValue(droneStatusProperty, value); }
        }
        public BO.Position DronePosition
        {
            get { return (BO.Position)GetValue(DronePositionProperty); }
            set { SetValue(droneStatusProperty, value); }
        }
        public int IdParcel { get; set; } //if there is

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("MaxWeight", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty droneStatusProperty = DependencyProperty.Register("Status", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DronePositionProperty = DependencyProperty.Register("DronePosition", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));
        public static readonly DependencyProperty IdParcelProperty = DependencyProperty.Register("IdParcel", typeof(object), typeof(DroneToList), new UIPropertyMetadata(0));

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
        public BO.Position droneWithParcel { get; set; }
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

