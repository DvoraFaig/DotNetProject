using System;
using System.Collections.Generic;
using System.Text;
using IBL;
using IBL.BO;
using DalObject;
using IDal.DO;


namespace IBL
{
    namespace BO
    {
        public partial class BL //: IBL
        {
            public static class BLdataSource
            {
                public static List<IBL.BO.Drone> BLDrones = new List<IBL.BO.Drone>();
                public static List<IBL.BO.Station> BLStations = new List<IBL.BO.Station>();
                public static List<IBL.BO.Customer> BLCustomers = new List<IBL.BO.Customer>();
                public static List<IBL.BO.Parcel> BLParcels = new List<IBL.BO.Parcel>();
                public static List<IBL.BO.DroneCharge> BLDroneCharges = new List<IBL.BO.DroneCharge>();
            }
        }
    }
}