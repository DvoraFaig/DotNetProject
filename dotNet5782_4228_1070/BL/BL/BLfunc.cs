using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static BO.Exceptions;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        static BL instance;
        public static BL GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new BL();
                return instance;
            }
        }
        
        private static double distance(Position p1, Position p2)
        {
            double d = Math.Pow((Math.Pow(p1.Longitude - p2.Longitude, 2) + Math.Pow(p1.Latitude - p2.Latitude, 2)), 0.5);
            return d;
        }

        public int GetDroneStatusInDelivery(int droneId)
        {
            Drone drone = GetDroneById(droneId);
            if (drone.Status == DroneStatus.Available)
            {
                return (int)DeliveryStatusAction.Available;
            }
            else if (drone.Status == DroneStatus.Delivery)
            {
                if (drone.DronePosition.Latitude == drone.ParcelInTransfer.SenderPosition.Latitude &&
                    drone.DronePosition.Longitude == drone.ParcelInTransfer.SenderPosition.Longitude ) // i erased else if
                {
                    return (int)DeliveryStatusAction.PickedParcel;
                }
                if (drone.ParcelInTransfer != null)
                {
                    return (int)DeliveryStatusAction.AsignedParcel;
                }
            }
            throw new Exception("No macthing status");
        }
    }
}
