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
        public class BLPosition
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
        //================================
        //Customer
        //================================
        public class BLCustomer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public BLPosition CustomerPosition { get; set; }
            public List<BLParcelAtCustomer> CustomerAsSender { get; set; }
            public List<BLParcelAtCustomer> CustomerAsTarget { get; set; }
            public override string ToString()
            {
                return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, CustomerPosition: {CustomerPosition.ToString()} ,\nCustomerAsSender: {CustomerAsSender} ,CustomerAsTarget: {CustomerAsTarget}\n");
            }
        }
        public class BLCustomerToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int AmountSendingDeliveredParcels { get; set; }
            public int AmountSendingUnDeliveredParcels { get; set; }
            public int AmountReceivingParcels { get; set; }
            public int AmountReceivingUnDeliveredParcels { get; set; }
            //public BLPosition CustomerPosition { get; set; }
            //public List<BLDeliveryAtCustomer> deliveryfromCustomers { get; set; }
            //public List<BLDeliveryAtCustomer> deliveryToCustomers { get; set; }
        }
        public class BLCustomerInParcel //targetId in parcel
        {
            public int Id { get; set; }
            public string name { get; set; }
        }
        //================================
        //Parcel
        //================================

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
                return ($"parcel ID: {Id}, Sender {Sender.ToString()}, Target: {Target.ToString()}, parcel Priority: {Priority}, parcel weight: {Weight},\nDrone: {BL.BL.checkNullforPrint(Drone)} parcel Requeasted: { BL.BL.checkNullforPrint(Requeasted) }, parcel scheduled {BL.BL.checkNullforPrint(Scheduled)}, parcel pickUp{BL.BL.checkNullforPrint(PickUp)}, parcel delivered: {BL.BL.checkNullforPrint(Delivered)}\n");
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
        }
        //================================
        // Drone
        //================================
        public class BLDrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; } //DroneStatus ?? the same name
            public BLParcelInTransfer ParcelInTransfer { get; set; }
            public BLPosition DronePosition { get; set; }
            public override string ToString()
            {
                return ($"drone id: {Id}, drone model{Model}, drone MaxWeight {MaxWeight},drone battery {Battery} , drone status{Status}, drone battery {Battery}\n DeliveryInTransferDeliveryInTransferDeliveryInTransfer\n DronePosition : {DronePosition.ToString()}");
            }
        }
        public class BLDroneToList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus droneStatus { get; set; } //DroneStatus ?? the same name
            public BLPosition DronePosition { get; set; }
            public int IdParcel { get; set; } //if there is
        }
        public class BLDroneInParcel //drone in pacel
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public BLPosition droneWithparcel { get; set; }
        }
        public class BLChargingDrone
        {
            public int Id { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return ($"ChargingDrone Id: {Id}, ChargingDrone Battery: {Battery}\n");
            }
        }
        //================================
        // Station
        //================================
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
