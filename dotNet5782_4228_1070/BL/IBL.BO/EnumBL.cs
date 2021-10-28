using System;

namespace IBL
{
    namespace BO
    {
        public enum WeightCategories { Light, Medium, Heavy }
        public enum DroneStatus { Available, Maintenance, Delivery }
        public enum Priorities { Regular, Fast, Emergency }

        //enum for the main
        enum Choices { Add = 1, Update, ShowWithId, ShowList, exit }
        enum objects { Station = 1, Drone, Customer, Parcel, FreeParcel, EmptyCharges }
        enum UpdateObj { DroneReceivesParcel = 1, DroneCollectsAParcel, CostumerGetsParcel, sendDroneToCharge, freeDroneFromCharge }
    }
}
