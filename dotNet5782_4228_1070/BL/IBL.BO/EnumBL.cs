﻿using System;

namespace IBL
{
    namespace BO
    {
        public enum DroneStatus { Available, Maintenance, Delivery }

        //enum for the main
        enum Choices { Add = 1, Update, ShowWithId, ShowList, exit }
        enum objects { Station = 1, Drone, Customer, Parcel, FreeParcel, EmptyCharges }
        enum UpdateObj { DroneReceivesParcel = 1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }
        public enum ParcelStatuses { Requeasted, Scheduled, PickedUp, Delivered  };
    }
}
