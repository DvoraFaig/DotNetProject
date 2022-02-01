﻿using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Linq;
using static BO.Exceptions;

namespace BL
{
    sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Instance of Idal interface.
        /// </summary>
        private readonly DalApi.Idal dal;

        /// <summary>
        /// Electicity usage of drone by weight
        /// and chargingRateOfDrone.
        /// </summary>
        private double electricityUsageWhenDroneIsEmpty { get; set; }
        private double electricityUsageWhenDroneILightWeight { get; set; }
        private double electricityUsageWhenDroneIsMediumWeight { get; set; }
        private double electricityUsageWhenDroneIsHeavyWeight { get; set; }
        private double chargingRateOfDrone { get; set; }


        /// <summary>
        /// BL ctor.
        /// Initialize the drones in this namespace accurding to data in database.
        /// </summary>
        private BL()
        {
            Random r = new Random();

            dronesList = new List<Drone>();
            dal = DalApi.DalFactory.factory(); //start one time an IDal.DO.IDal object.

            #region props implementation
            double[] electricityUsageDrone = dal.electricityUseByDrone();
            electricityUsageWhenDroneIsEmpty = electricityUsageDrone[0];
            electricityUsageWhenDroneILightWeight = electricityUsageDrone[1];
            electricityUsageWhenDroneIsMediumWeight = electricityUsageDrone[2];
            electricityUsageWhenDroneIsHeavyWeight = electricityUsageDrone[3];
            chargingRateOfDrone = electricityUsageDrone[4];
            #endregion

            #region Objects and variables declaration
            IEnumerable<DO.Drone> drones = dal.GetDrones();
            DO.Parcel parcel;
            DO.Station station;
            DO.Customer sender, target;
            Position senderPosition, targetPosition;
            Drone CurrentDrone = new Drone();
            int amountStations = dal.amountStations();
            List<DO.Customer> cWithDeliveredP = new List<DO.Customer>();
            try
            {
                cWithDeliveredP = findCustomersWithDeliveredParcel();
            }
            #region Exceptions
            catch (Exception)
            {
            }
            #endregion
            #endregion

            #region implement Drone.DroneStatus: Available, Maintenance, Delivery.
            foreach (DO.Drone drone in drones)
            {
                #region Objects and variables implemntation
                parcel = dal.getParcelWithSpecificCondition(parcel => parcel.DroneId == drone.Id).FirstOrDefault();
                station = new DO.Station();
                CurrentDrone = new Drone();
                CurrentDrone.Id = drone.Id; /*copyDalToBLDroneInfo(d);*/
                CurrentDrone.Model = drone.Model;
                CurrentDrone.MaxWeight = (BO.WeightCategories)drone.MaxWeight;
                #endregion

                #region A parcel was paired with Drone but not yet delivered. => parcel.status = to PickUp parcel / to Delivered parcel.
                if (!(parcel.Scheduled == (null)) && parcel.Delivered == (null) && !parcel.Equals(default(DO.Parcel)))
                {
                    CurrentDrone.Status = DroneStatus.Delivery;
                    DO.Station closestStationToSender = new DO.Station();
                    sender = dal.getCustomerWithSpecificCondition(customer => customer.Id == parcel.SenderId).First();
                    target = dal.getCustomerWithSpecificCondition(customer => customer.Id == parcel.TargetId).First();
                    CurrentDrone.ParcelInTransfer = returnAParcelInTransfer(parcel, sender, target);//.First();
                    senderPosition = CurrentDrone.ParcelInTransfer.SenderPosition;
                    targetPosition = CurrentDrone.ParcelInTransfer.TargetPosition;
                    if (parcel.PickUp == null) //position like the closest station to the sender of parcel.
                    {
                        closestStationToSender = findAvailbleAndClosestStationForDrone(senderPosition); //תחנה קרובה לשלוח במצב הטענה? //אם אינו נכנס למצב הטענה PositionFromClosestStation () //אם כן updateDroneCharge
                        CurrentDrone.DronePosition = new Position() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude };
                        CurrentDrone.ParcelInTransfer.isWaiting = false;
                        //isWaiting = p.PickUp == null ? true : false,

                    }
                    else if (parcel.Delivered == null) //else position sender of parcel.
                    {
                        CurrentDrone.ParcelInTransfer.isWaiting = true;
                        CurrentDrone.DronePosition = senderPosition;//new Position() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                    }
                    CurrentDrone.Battery = calcDroneBatteryForDroneDelivery(parcel, closestStationToSender, senderPosition, targetPosition);
                }
                #endregion

                #region Drone is not with a delivery status
                else
                {
                    CurrentDrone.Status = (DroneStatus)r.Next(0, 2); // Available / Maintenance

                    #region Drone is with maintenance status.
                    if (CurrentDrone.Status == DroneStatus.Maintenance)//Maintenance ////or if couldn't find a position for an availble drone
                    {
                        List<DO.Station> stationsToFindPlaceToCharge = dal.GetStations().Cast<DO.Station>().ToList();
                        #region instructions
                        //If drone is supposed to be in charging find an avilable station with empty charging slots.
                        //for not having all the drones in the same place:
                        //Try random station if station didn't have an empty place go threw all the stations
                        ////int amountStation = dal.amountStations();
                        #endregion
                        #region Find random station
                        int randomStation = r.Next(0, amountStations);
                        Drone updatedDroneWithStationInfoAndBattery = new Drone();
                        updatedDroneWithStationInfoAndBattery = findStationForDrone(CurrentDrone, stationsToFindPlaceToCharge[randomStation]);
                        if (updatedDroneWithStationInfoAndBattery != null)
                        {
                            CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                            CurrentDrone.Battery = r.Next(0, 20);
                            CurrentDrone.SartToCharge = DateTime.Now;
                        }
                        #endregion
                        #region go threw all the stations
                        else
                        {
                            foreach (DO.Station stationsObj in stationsToFindPlaceToCharge)
                            {
                                updatedDroneWithStationInfoAndBattery = findStationForDrone(CurrentDrone, stationsToFindPlaceToCharge[randomStation]);
                                if (updatedDroneWithStationInfoAndBattery != null)
                                {
                                    CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                                    CurrentDrone.Battery = r.Next(0, 20);
                                    CurrentDrone.SartToCharge = DateTime.Now;
                                    break;
                                }
                            }
                            if (updatedDroneWithStationInfoAndBattery == null)//stand in a station position
                            {
                                /*int rand = r.Next(0, amountStations);
                                CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                                CurrentDrone.Battery = r.Next(20, 100);*/
                                CurrentDrone.Status = DroneStatus.Available;
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region Drone is with available status.
                    if (CurrentDrone.Status == DroneStatus.Available) //DroneStatus.Available
                    {
                        if (cWithDeliveredP.Count > 0)
                        {
                            int index = r.Next(0, cWithDeliveredP.Count);
                            target = dal.getCustomerWithSpecificCondition(c => c.Id == cWithDeliveredP[index].Id).First();
                            CurrentDrone.DronePosition = new Position() { Longitude = target.Longitude, Latitude = target.Latitude };
                            station = findAvailbleAndClosestStationForDrone(CurrentDrone.DronePosition);
                            double distanceBetweenDroneAndStation = distance(new Position() { Latitude = station.Latitude, Longitude = station.Longitude }, CurrentDrone.DronePosition);
                            double batteryToGetToStation = distanceBetweenDroneAndStation * requestElectricity((int)drone.MaxWeight);
                            CurrentDrone.Battery = r.Next((int)batteryToGetToStation, 100);
                        }
                        else //couldn't find a delivered parcel.
                        {
                            DO.Station stationForAvaiableDrone;
                            int randomStation = r.Next(0, amountStations);
                            try
                            {
                                stationForAvaiableDrone = dal.getStationWithSpecificCondition(p => p.Id >= randomStation).First();
                            }
                            catch (Exception)
                            {
                                stationForAvaiableDrone = dal.getStationWithSpecificCondition(p => p.Id > 0).First(); //everyone
                            }
                            CurrentDrone.DronePosition = new Position() { Latitude = stationForAvaiableDrone.Latitude, Longitude = stationForAvaiableDrone.Longitude };
                            CurrentDrone.Battery = r.Next(20, 100);
                        }

                    }
                    #endregion
                }
                #endregion

                dronesList.Add(CurrentDrone);
            }
            #endregion
        }

        /// <summary>
        /// Find a station for a drone with empty charging slots
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private Drone findStationForDrone(Drone drone, DO.Station station)
        {
            Random r = new Random();
            int amountChargingDronesInStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id).Count();
            if (station.ChargeSlots > amountChargingDronesInStation)
            {
                dal.AddDroneToCharge(new DO.DroneCharge() { DroneId = drone.Id, StationId = station.Id });
                drone.DronePosition = new Position() { Latitude = station.Latitude, Longitude = station.Longitude };
                drone.Battery = r.Next(0, 20);
                return drone;
            }
            return null;
        }

        /// <summary>
        /// Calculate Drones' battery accurding to the distance of the delivery;
        /// Distance = From cstation to sender + from sender to customer + dis from target to closest avialble station.
        /// </summary>
        /// <param name="parcel">parcel in delivery</param>
        /// <param name="stationOfDrone">Station where drone is. Closest station to the sender of the parcel</param>
        /// <param name="senderPosition">sender position</param>
        /// <param name="targetPosition">target position</param>
        /// <returns></returns>
        private int calcDroneBatteryForDroneDelivery(DO.Parcel parcel, DO.Station stationOfDrone, Position senderPosition, Position targetPosition)
        {
            double disFromStationToSender = 0; // only for a parcel who wasnt picked up.
            double disFromSenderToCustomer = distance(senderPosition, targetPosition);

            //from target to closest station;
            DO.Station closestAvailbleStationFromTarget = findAvailbleAndClosestStationForDrone(targetPosition);
            double disFromTargetTostation = distance(targetPosition, new Position() { Latitude = closestAvailbleStationFromTarget.Latitude, Longitude = closestAvailbleStationFromTarget.Longitude });
            if (parcel.PickUp == new DateTime())
            {
                disFromStationToSender = distance(new Position() { Latitude = stationOfDrone.Latitude, Longitude = stationOfDrone.Longitude }, senderPosition);
            }
            double sumDisForDrone = disFromStationToSender + disFromSenderToCustomer + disFromTargetTostation;
            double sumBattery = sumDisForDrone * requestDroneElectricityUsage()[(int)parcel.Weight];
            return new Random().Next((int)sumBattery, 100);
        }

        /// <summary>
        /// Find availble and closest station for drone;
        /// </summary>
        /// <param name="dronePosition">Current Drone position</param>
        /// <returns></returns>
        private DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition)
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
            DO.Station availbleClosestStation = new DO.Station();
            double dis = -1;
            double minDis = -1;
            int fullChargingSlots;
            foreach (DO.Station station in stations)
            {
                fullChargingSlots = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
                if (station.ChargeSlots - fullChargingSlots > 0) //has empty charging slots
                {
                    dis = distance(dronePosition, new Position() { Latitude = station.Latitude, Longitude = station.Longitude });
                    if (minDis == -1) //Wasn't implemted by a specific drone.
                    {
                        minDis = dis;
                        availbleClosestStation = station;
                    }
                    else if (minDis > dis)
                    {
                        minDis = dis;
                        availbleClosestStation = station;
                    }
                    if (minDis == 0)
                        return availbleClosestStation;
                }
            }
            return availbleClosestStation;
        }

        /// <summary>
        /// find availble & closest station for drone. (occurding to distance* weight < Drone.Battery
        /// </summary>
        /// <param name="dronePosition">To find the distance to a station </param>
        /// <param name="droneBattery">Drone.Batter: To check if could hover to station</param>
        /// <returns></returns>
        public DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition, double droneBattery)
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
            DO.Station availbleClosestStation = new DO.Station();
            double dis = -1;
            double minDis = -1;
            int fullChargingSlots;
            foreach (DO.Station station in stations)
            {
                fullChargingSlots = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
                if (station.ChargeSlots - fullChargingSlots > 0) //has empty charging slots
                {
                    dis = distance(dronePosition, new Position() { Latitude = station.Latitude, Longitude = station.Longitude });
                    if (droneBattery - dis * electricityUsageWhenDroneIsEmpty > 0)
                    {
                        if (minDis == -1)
                        {
                            minDis = dis;
                            availbleClosestStation = station;
                        }
                        else if (minDis > dis)
                        {
                            minDis = dis;
                            availbleClosestStation = station;
                        }
                        if (minDis == 0)
                            return availbleClosestStation;
                    }
                }
            }
            if (availbleClosestStation.Equals(typeof(Station)))
                throw new Exceptions.ObjNotExistException($"No station with empty charging slots, please free drone from {availbleClosestStation.Id} station.\n later the free try to send this drone again.", availbleClosestStation);
            return availbleClosestStation;
        }

        /// <summary>
        /// Find a customer who received a parcel.
        /// </summary>
        /// <returns></returns>
        private List<DO.Customer> findCustomersWithDeliveredParcel()
        {
            IEnumerable<DO.Parcel> parcels = dal.getParcelWithSpecificCondition(p => (p.Delivered != null));
            List<DO.Customer> customersWithDeliveredParcels = new List<DO.Customer>();
            foreach (DO.Parcel parcel in parcels)
            {
                customersWithDeliveredParcels.Add(dal.getCustomerWithSpecificCondition(c => c.Id == parcel.TargetId).First());
            }
            return customersWithDeliveredParcels;
        }

        /// <summary>
        /// returns an array of drones' electricity usage. 
        /// arr[] =
        /// empty,
        /// lightWeight,
        /// mediumWeight,
        /// heavyWeight,
        /// chargingRate
        /// </summary>
        /// <returns></returns>
        private double[] requestDroneElectricityUsage()
        {
            return dal.electricityUseByDrone();
        }

        /// <summary>
        /// Amount of power consumption of Drone
        /// returns the value of drones' electricity usage from the props.
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public double requestElectricity(int choice)
        {
            switch ((Electricity)choice)
            {
                case Electricity.Empty:
                    return electricityUsageWhenDroneIsEmpty;
                case Electricity.LightWeight:
                    return electricityUsageWhenDroneILightWeight;
                case Electricity.MediumWeight:
                    return electricityUsageWhenDroneIsMediumWeight;
                case Electricity.HeavyWeight:
                    return chargingRateOfDrone;
                case Electricity.ChargingRate:
                    return chargingRateOfDrone;
                default:
                    return 0;
            }
        }
    }
}

//using System;
//using BlApi;
//using DalApi;

//namespace BL
//{
//    sealed class BL : IBL
//    {
//        static readonly IBL instance = new BL();
//        public static IBL Instance { get => instance; }

//        internal IDal dal = DalFactory.GetDal();
//        BL() { }

//    }
//}