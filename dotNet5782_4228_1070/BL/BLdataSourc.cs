using System;
using System.Collections.Generic;
using System.Text;
using IBL;
using IBL.BO;
using DalObject;

namespace BL
{
    public partial class BL //: IBL
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Station> Stations = new List<Station>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

    }
}