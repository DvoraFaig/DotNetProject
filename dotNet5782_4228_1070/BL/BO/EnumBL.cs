using System;


namespace BO
{
    public enum DroneStatus { Available, Maintenance, Delivery }
    public enum ParcelStatuses { Requeasted, Scheduled, PickedUp, Delivered };
    public enum Electricity { Empty, LightWeight, MediumWeight, HeavyWeight, ChargingRate }
    public enum DeliveryStatusAction { Available, AsignedParcel, PickedParcel , DeliveredParcel};
    public enum DroneStatusInSim { ToPickUp = 1, PickUp, ToDelivery, Delivery, ToCharge, NoAvailbleChargingSlots, NotEnoughBatteryForDelivery, DisFromDestination, HideTextBlock };

    //Enums for BL_ConsuoleUI
    public enum UpdateObj { DroneReceivesParcel = 1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }
    public enum Choices { Add = 1, Update, ShowWithId, ShowList, exit }
    public enum objects { Station = 1, Drone, Customer, Parcel, FreeParcel, EmptyCharges }
    public enum UpdateBL { DronesInfo = 1, StationInfo, CustomerInfo, sendDroneToCharge, freeDroneFromCharging, DroneScheduledWithAParcel, DronePicksUpParcel, DeliveryParcelByDrone }

}

