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
        public class BLCustomerInDelivery //targetId in parcel
        {
            public int TargetId { get; set; }
            public string name { get; set; }
        }
        public class BLDeliveryInTransfer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public bool parcelStatus { get; set; } //ממתין לאיסוף =false\ בדרך ליעד=true
            public BLPosition SenderPosition { get; set; }
            public BLPosition TargetPosition { get; set; }
            public double distance { get; set; } //.............. } //sqrt(pow(SenderPosition.Latitude+TargetPosition.Latitude,2)+ pow(SenderPosition.Longitude+TargetPosition.Longitude,2),2)}
        }
        public class BLDeliveryAtCustomer
        {
            public int Id { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses ParcelStatus { get; set; }
            public BLCustomerInDelivery SenderOrTargetCustomer { get; set; }
        }
        public class BLParcelInTransfer
        {
            public int Id { get; set; }
            public Priorities Priority { get; set; }
            public BLCustomerInDelivery SenderCustomer { get; set; }
            public BLCustomerInDelivery TargetCustomer { get; set; }
        }

        public class BLParcelInDrone //drone in pacel
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
        public class BLCustomer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public BLPosition CustomerPosition { get; set; }
            public List<BLDeliveryAtCustomer> deliveryfromCustomers { get; set; }
            public List<BLDeliveryAtCustomer> deliveryToCustomers { get; set; }
            public override string ToString()
            {
                return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, CustomerPosition: {CustomerPosition.ToString()}\n");
            }
        }

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
        public class BLDrone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; } //DroneStatus ?? the same name
            public BLDeliveryInTransfer DeliveryInTransfer { get; set; }
            public BLPosition DronePosition { get; set; }
            public override string ToString()
            {
                return ($"drone id: {Id}, drone model{Model}, drone MaxWeight {MaxWeight},drone battery {Battery} , drone status{Status}, drone battery {Battery}\n DeliveryInTransferDeliveryInTransferDeliveryInTransfer\n DronePosition : {DronePosition.ToString()}");
            }
        }
        public class BLParcel
        {
            public int Id { get; set; }
            public BLCustomer Sender { get; set; }
            public BLCustomer Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public BLDrone Drone { get; set; }
            public DateTime Requeasted { get; set; } //prepare a parcel to delivery
            public DateTime Scheduled { get; set; } //pair a parcel to drone
            public DateTime PickUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return ($"parcel ID: {Id}, Sender {Sender.ToString()}, Target: {Target.ToString()}, parcel Priority: {Priority}, parcel weight: {Weight}, parcel Requeasted: {Requeasted}, Drone of parcel {Drone.ToString}, parcel scheduled {Scheduled}, parcel pickUp{PickUp}, parcel delivered: {Delivered}\n");
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
            public BLPosition CustomerPosition { get; set; }
            public List<BLDeliveryAtCustomer> deliveryfromCustomers { get; set; }
            public List<BLDeliveryAtCustomer> deliveryToCustomers { get; set; }
        }
        public class BLStationToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int DroneChargeAvailble { get; set; }
            public int DroneChargeOccupied { get; set; }
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
        public class BLParcelToList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string TargetName { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatuses ParcelStatus { get; set; }
        }
    }
}
