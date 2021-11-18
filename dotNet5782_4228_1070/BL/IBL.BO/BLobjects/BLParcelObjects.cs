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
            public DateTime Requeasted { get; set; } //prepare a parcel to delivery
            public DateTime Scheduled { get; set; } //pair a parcel to drone
            public DateTime PickUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return ($"parcel ID: {Id}, Sender {Sender.ToString()}, Target: {Target.ToString()}, parcel Priority: {Priority}, parcel weight: {Weight},\n" +
                    $"Drone: {BL.BL.checkNullforPrint(Drone)} parcel Requeasted: { BL.BL.checkNullforPrint(Requeasted) }, parcel scheduled {BL.BL.checkNullforPrint(Scheduled)}, parcel pickUp{BL.BL.checkNullforPrint(PickUp)}, parcel delivered: {BL.BL.checkNullforPrint(Delivered)}\n");
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
                return ($"Id: {Id} , Weight: {Weight} , Priority: {Priority}, ParcelStatus: {ParcelStatus},{SenderOrTargetCustomer}");
            }
        }

        public class BLParcelInTransfer
        {
            public int Id { get; set; }
            public bool parcelStatus { get; set; } //ממתין לאיסוף =false\ בדרך ליעד=true
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public BLCustomerInParcel SenderCustomer { get; set; }
            public BLCustomerInParcel TargetCustomer { get; set; }
            public BLPosition SenderPosition { get; set; }
            public BLPosition TargetPosition { get; set; }
            public double distance { get; set; } //.............. } //sqrt(pow(SenderPosition.Latitude+TargetPosition.Latitude,2)+ pow(SenderPosition.Longitude+TargetPosition.Longitude,2),2)}
            public override string ToString()
            {
                return ($"id: {Id} , parcelStatus: {parcelStatus},Weight: {Weight}, parcelPriority: {Priority},SenderCustomer:{SenderCustomer}, TargetCustomer :{TargetCustomer}, SenderPosition: {SenderPosition}, TargetPosition :{TargetPosition}, distance: {distance} ");
            }
        }
    }
}
