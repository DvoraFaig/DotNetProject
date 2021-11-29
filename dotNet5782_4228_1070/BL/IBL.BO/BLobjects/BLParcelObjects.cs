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
        public class BLParcel
        {
            public int Id { get; set; }
            public BLCustomerInParcel Sender { get; set; }
            public BLCustomerInParcel Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public BLDroneInParcel Drone { get; set; }
            public DateTime? Requeasted { get; set; } //prepare a parcel to delivery
            public DateTime? Scheduled { get; set; } //pair a parcel to drone
            public DateTime? PickUp { get; set; }
            public DateTime? Delivered { get; set; }
            public override string ToString()
            {
                string notFilled = "not filled";
                return ($"parcel ID: {Id}, \n\tSender: {Sender.ToString()}, \tTarget: {Target.ToString()}, \tparcel Priority: {Priority}, parcel weight: {Weight},\n" +
                    $"\n\tDrone: {(!Drone.Equals(default(BLDroneInParcel))? Drone.ToString() : (char)'-') } " +
                    $"\n\tparcel Requeasted: {(Requeasted != null? Requeasted : (char)'-' )}, parcel scheduled: {(Scheduled != null ? Scheduled :  (char)'-' )}, parcel pickUp: {(PickUp != null ? PickUp : (char)'-')}, parcel delivered: {(Delivered != null ? Delivered : (char)'-')}\n");
            }
        } 

        public class BLParcelToList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string TargetName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses ParcelStatus { get; set; }
        }

        public class BLParcelAtCustomer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses ParcelStatus { get; set; }
            public BLCustomerInParcel SenderOrTargetCustomer { get; set; }
            public override string ToString()
            {
                return ($"Id: {Id} , Weight: {Weight} , Priority: {Priority}, ParcelStatus: {ParcelStatus},{SenderOrTargetCustomer}\n");
            }
        }

        public class BLParcelInTransfer
        {
            public int Id { get; set; }
            /// <summary>
            /// Awaiting collection =false\ On the way to the destination=true
            /// </summary>
            public bool parcelStatus { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public BLCustomerInParcel SenderCustomer { get; set; }
            public BLCustomerInParcel TargetCustomer { get; set; }
            public BLPosition SenderPosition { get; set; }
            public BLPosition TargetPosition { get; set; }
            public double distance { get; set; } 
            public override string ToString()
            {
                return ($"id: {Id} , parcelStatus: {parcelStatus},Weight: {Weight}, parcelPriority: {Priority},\n\tSenderCustomer:{SenderCustomer}, \tTargetCustomer :{TargetCustomer}, \tSenderPosition: {SenderPosition}, \tTargetPosition: {TargetPosition}, distance: {distance} ");
            }
        }
    }
}
