using System;
using System.Collections.Generic;
using System.Text;
using IBL;
using IBL.BO;
using DalObject;
using IDal.DO;


namespace BL
{
    public partial class BL //: IBL
    {
        internal static List<IBL.BO.Drone> Drones = new List<IBL.BO.Drone>();
        internal static List<IBL.BO.Station> Stations= new List<IBL.BO.Station>();
        internal static List<IBL.BO.Customer> Customers = new List<IBL.BO.Customer>();
        internal static List<IBL.BO.Parcel> Parcels  = new List<IBL.BO.Parcel>();
        internal static List<IBL.BO.DroneCharge> DroneCharges  = new List<IBL.BO.DroneCharge>();
    }
}