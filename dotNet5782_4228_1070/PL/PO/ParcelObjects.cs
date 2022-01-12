using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Windows;

namespace PO
{
    public class Parcel : DependencyObject
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        //public WeightCategories Weight { get; set; }
        //public Priorities Priority { get; set; }
        public DroneInParcel Drone { get; set; }
        public DateTime? Requeasted { get; set; } //prepare a parcel to delivery
        public DateTime? Scheduled { get; set; } //pair a parcel to drone
        public DateTime? PickUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            //string notFilled = "not filled";
            return ($"parcel ID: {Id}, \n\tSender: {Sender.ToString()}, \tTarget: {Target.ToString()}, \tparcel Priority: /*Priority*/, parcel weight: /*Weight*/,\n" +
                $"\n\tDrone: {(!Drone.Equals(default(DroneInParcel)) ? Drone.ToString() : (char)'-') } " +
                $"\n\tparcel Requeasted: {(Requeasted != null ? Requeasted : (char)'-')}, parcel scheduled: {(Scheduled != null ? Scheduled : (char)'-')}, parcel pickUp: {(PickUp != null ? PickUp : (char)'-')}, parcel delivered: {(Delivered != null ? Delivered : (char)'-')}\n");
        }
    }

    public class ParcelToList : DependencyObject
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public DO.WeightCategories Weight { get; set; }
        public DO.Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
    }

    public class ParcelAtCustomer : DependencyObject
    {
        public int Id { get; set; }
        //public WeightCategories Weight { get; set; }
        public DO.Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
        public CustomerInParcel SenderOrTargetCustomer { get; set; }
        public override string ToString()
        {
            return ($"Id: {Id} , Weight: /*Weight*/ , Priority: {Priority}, ParcelStatus: {ParcelStatus},{SenderOrTargetCustomer}\n");
        }
    }

    public class ParcelInTransfer : DependencyObject
    {
        public int Id { get; set; }
        /// <summary>
        /// Awaiting collection =false\ On the way to the destination=true
        /// </summary>
        public bool parcelStatus { get; set; }
        //public WeightCategories Weight { get; set; }
        //public Priorities Priority { get; set; }
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel TargetCustomer { get; set; }
        public Position SenderPosition { get; set; }
        public Position TargetPosition { get; set; }
        public double distance { get; set; }
        //public override string ToString()
        //{
         //   return ($"id: {Id} , parcelStatus: {parcelStatus},Weight: {Weight}, parcelPriority: {Priority},\n\tSenderCustomer:{SenderCustomer}, \tTargetCustomer :{TargetCustomer}, \tSenderPosition: {SenderPosition}, \tTargetPosition: {TargetPosition}, distance: {distance} ");
        //}
    }
}

