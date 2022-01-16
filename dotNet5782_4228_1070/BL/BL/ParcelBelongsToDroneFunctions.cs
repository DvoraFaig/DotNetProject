using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Find a parcel to delivery for drone.
        /// </summary>
        /// <param name="droneId">The drones' id</param>
        /// <returns></returns>
        public Drone PairParcelWithDrone(int droneId) //ParcelStatuses.Scheduled
        {
            try
            {
                DO.Customer senderParcel;
                DO.Customer targetParcel;
                DO.Customer senderMaxParcel;
                double disMaxPToSender = -1; // the biggest distance.....
                Drone droneToParcel = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                IEnumerable<DO.Parcel> parcels = dal.GetParcels();
                DO.Parcel maxParcel = new DO.Parcel();// = new IDal.DO.Parcel() { Weight = 0 };//parcels.First(); //check if weight is good=====================
                foreach (DO.Parcel p in parcels)
                {
                    if (p.Scheduled == null)//&& requested!=null .Equals(default(DO.Parcel).Scheduled)
                    {
                        senderParcel = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First();
                        targetParcel = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First();
                        Position senderPosition = new Position() { Longitude = senderParcel.Longitude, Latitude = senderParcel.Latitude };
                        Position targetPosition = new Position() { Longitude = targetParcel.Longitude, Latitude = targetParcel.Latitude };
                        double disDroneToSenderP = distance(droneToParcel.DronePosition, senderPosition);
                        double disSenderToTarget = distance(senderPosition, targetPosition);
                        double batteryAfterDeliveringByTarget = Math.Round(disDroneToSenderP * electricityUsageWhenDroneIsEmpty + disSenderToTarget * requestElectricity((int)p.Weight), 1);
                        DO.Station stationWithMinDisFromTarget = findAvailbleAndClosestStationForDrone(targetPosition, batteryAfterDeliveringByTarget);
                        double disTargetToStation = distance(targetPosition, new Position() { Longitude = stationWithMinDisFromTarget.Longitude, Latitude = stationWithMinDisFromTarget.Latitude });
                        double droneElectricity = requestElectricity((int)p.Weight);
                        double totalBatteryForDeliveryUsage = (double)(disDroneToSenderP * droneElectricity + disSenderToTarget * droneElectricity + disTargetToStation * droneElectricity);
                        totalBatteryForDeliveryUsage = Math.Round(totalBatteryForDeliveryUsage, 2);
                        #region find the most matching parcel
                        if (droneToParcel.Battery - totalBatteryForDeliveryUsage > 0) //[4]
                        {
                            if (p.Weight <= droneToParcel.MaxWeight) //BLParcel bLParcel = convertDalToBLParcel(p);
                            {
                                if (maxParcel.Equals(default(DO.Parcel)))
                                {
                                    maxParcel = p;
                                    senderMaxParcel = senderParcel;
                                    disMaxPToSender = disDroneToSenderP;
                                }
                                else
                                {
                                    if (maxParcel.Priority < p.Priority)
                                    {
                                        maxParcel = p;
                                        senderMaxParcel = senderParcel;
                                        disMaxPToSender = disDroneToSenderP;
                                    }
                                    else if (maxParcel.Priority == p.Priority /*&& p.Weight <= droneToParcel.MaxWeight && maxParcel.Weight <= droneToParcel.MaxWeight*/)
                                    {
                                        if (maxParcel.Weight < p.Weight)
                                            maxParcel = p;
                                        else if (maxParcel.Weight == p.Weight)
                                        {
                                            if (disDroneToSenderP < disMaxPToSender || disMaxPToSender == -1)
                                            {
                                                maxParcel = p;
                                                senderMaxParcel = senderParcel;
                                                disMaxPToSender = disDroneToSenderP;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                if (maxParcel.Equals(default(DO.Parcel)))
                {
                    throw new Exceptions.ObjNotAvailableException("No Parcel matching drones' conditions");
                }
                droneToParcel.Status = DroneStatus.Delivery;
                droneToParcel.ParcelInTransfer = returnAParcelInTransfer(
                    maxParcel, convertBLToDalCustomer(GetCustomerById(maxParcel.SenderId)),
                    convertBLToDalCustomer(GetCustomerById(maxParcel.TargetId)));
                updateBLDrone(droneToParcel);
                maxParcel.DroneId = droneToParcel.Id;
                maxParcel.Scheduled = DateTime.Now;
                dal.changeParcelInfo(maxParcel);
                return droneToParcel;
            }
            #region Exceptions
            catch (Exception e)
            {
                throw new ObjNotAvailableException(e.Message);
            }
            #endregion
        }



        /// <summary>
        /// return a DroneInParcel occurding to a parcel and drone id.
        /// </summary>
        /// <param name="p">a parcel in drone</param>
        /// <param name="droneId">drone that has a parcel</param>
        /// <returns></returns>
        private DroneInParcel createDroneInParcel(DO.Parcel p, int droneId)
        {
            Drone d = getDroneByIdFromDronesList(droneId);
            return new DroneInParcel() { Id = d.Id, Battery = d.Battery, droneWithParcel = d.DronePosition };
        }
    }
}
