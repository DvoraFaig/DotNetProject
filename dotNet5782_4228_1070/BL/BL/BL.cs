﻿using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;
using static IBL.BO.Exceptions;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        static public IDal.DO.IDal dal;
        private double empty { get; set; }
        private double lightWeight { get; set; }
        private double mediumWeight { get; set; }
        private double heavyWeight { get; set; }
        private double chargingRate { get; set; }
        private BL()
        {
            dronesInBL = new List<Drone>();
            dal = IDal.DalFactory.factory("DalObject"); //start one time an IDal.DO.IDal object.
            empty = dal.electricityUseByDrone()[0];
            lightWeight = dal.electricityUseByDrone()[1];
            mediumWeight = dal.electricityUseByDrone()[2];
            heavyWeight = dal.electricityUseByDrone()[3];
            chargingRate = dal.electricityUseByDrone()[4];

            IEnumerable<IDal.DO.Drone> drones = dal.displayDrone();
            IDal.DO.Parcel parcel;
            IDal.DO.Station station;
            IDal.DO.Customer sender, target;
            Position senderPosition, targetPosition;
            Drone CurrentDrone = new Drone();

            foreach(IDal.DO.Drone drone in drones )
            {
                parcel = dal.getParcelWithSpecificCondition(parcel=> parcel.DroneId == drone.Id).FirstOrDefault();
                station = new IDal.DO.Station();
                CurrentDrone = new Drone();
                CurrentDrone.Id = drone.Id; /*copyDalToBLDroneInfo(d);*/
                CurrentDrone.Model = drone.Model;
                CurrentDrone.MaxWeight =  drone.MaxWeight;

                if (!(parcel.Scheduled == (null)) && parcel.Delivered == (null) && !parcel.Equals(default(IDal.DO.Parcel)) )// pair a parcel to drone but not yed delivered.
                {
                    CurrentDrone.Status = DroneStatus.Delivery;
                    IDal.DO.Station closestStationToSender = new IDal.DO.Station();
                    sender = dal.getCustomerWithSpecificCondition(customer => customer.ID == parcel.SenderId).First();
                    target = dal.getCustomerWithSpecificCondition(customer => customer.ID == parcel.TargetId).First();
                    CurrentDrone.ParcelInTransfer = createParcelInTransfer(parcel, sender, target);//.First();
                    senderPosition = CurrentDrone.ParcelInTransfer.SenderPosition;
                    targetPosition = CurrentDrone.ParcelInTransfer.TargetPosition;
                    if (parcel.PickUp == null) //position like the closest station to the sender of parcel.
                    {
                        closestStationToSender = findAvailbleAndClosestStationForDrone(senderPosition); //תחנה קרובה לשלוח במצב הטענה? //אם אינו נכנס למצב הטענה PositionFromClosestStation () //אם כן updateDroneCharge
                        CurrentDrone.DronePosition = new Position() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude };
                    }
                    else if (parcel.Delivered == null) //else position sender of parcel.
                    {
                        CurrentDrone.DronePosition = new Position() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                    }
                    CurrentDrone.Battery = calcDroneBatteryForDroneDelivery(parcel, closestStationToSender, senderPosition, targetPosition);
                }

                else // if drone is not delivery status
                {
                    CurrentDrone.Status = (DroneStatus)r.Next(0, 2); // Available / Maintenance
                    bool AvailbeDroneWithPosition = false;
                    if (CurrentDrone.Status == DroneStatus.Available) //DroneStatus.Available
                    {
                        List<IDal.DO.Customer> cWithDeliveredP = findCustomersWithDeliveredParcel();
                        if (cWithDeliveredP.Count > 0)
                        {
                            AvailbeDroneWithPosition = true;
                            target = dal.getCustomerWithSpecificCondition(c => c.ID == cWithDeliveredP[r.Next(0, cWithDeliveredP.Count)].ID).First();
                            CurrentDrone.DronePosition = new Position() { Longitude = target.Longitude, Latitude = target.Latitude };
                            station = findAvailbleAndClosestStationForDrone(CurrentDrone.DronePosition);
                            double distanceBetweenDroneAndStation = distance(new Position() { Latitude = station.Latitude, Longitude = station.Longitude }, CurrentDrone.DronePosition);
                            CurrentDrone.Battery = r.Next(0, (int)distanceBetweenDroneAndStation);
                        }
                        else //couldn't find a delivered parcel.
                        {
                            List<IDal.DO.Station> stationsToFindPlaceToCharge = dal.displayStations().Cast<IDal.DO.Station>().ToList();
                            int amountStation = dal.amountStations();
                            int randomStation = r.Next(0, amountStation);
                            CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                            CurrentDrone.Battery = r.Next(20, 100);
                        }

                    }
                    if (CurrentDrone.Status == DroneStatus.Maintenance)//Maintenance or if couldn't find a position for an availble drone
                    {
                        List<IDal.DO.Station> stationsToFindPlaceToCharge  = dal.displayStations().Cast< IDal.DO.Station>().ToList();
                        //If drone is supposed to be in charging find an avilable station with empty charging slots.
                        //for not having all the drones in the same place:
                        //Try random station if station didn't have an empty place go threw all the stations
                        int amountStation = dal.amountStations();
                        #region Find random station
                        int randomStation = r.Next(0, amountStation);
                        Drone temp = new Drone();
                        temp = findStationForDrone(CurrentDrone, stationsToFindPlaceToCharge[randomStation]);
                        if (temp != null)
                        {
                            CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                            CurrentDrone.Battery = r.Next(0, 20);
                        }
                        #endregion
                        #region go threw all the stations
                        else
                        {
                            foreach (IDal.DO.Station stationsObj in stationsToFindPlaceToCharge)
                            {
                                temp = findStationForDrone(CurrentDrone, stationsToFindPlaceToCharge[randomStation]);
                                if (temp != null)
                                {
                                    CurrentDrone.DronePosition = new Position() { Latitude = stationsToFindPlaceToCharge[randomStation].Latitude, Longitude = stationsToFindPlaceToCharge[randomStation].Longitude };
                                    CurrentDrone.Battery = r.Next(0, 20);
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                }
                dronesInBL.Add(CurrentDrone);
            }
        }
        /// <summary>
        /// Find a station for a drone with empty charging slots
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private Drone findStationForDrone(Drone drone , IDal.DO.Station station)
        {
            int amountChargingDronesInStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id).Count();
            if (station.ChargeSlots > amountChargingDronesInStation)
            {
                dal.AddDroneCharge(new IDal.DO.DroneCharge() { DroneId = drone.Id, StationId = station.Id });
                drone.DronePosition = new Position() { Latitude = station.Latitude, Longitude = station.Longitude };
                drone.Battery = r.Next(0, 20);
                return drone;
            }
            return null;
        }
        /// <summary>
        /// Calculate Drones' battery accurding to the distance of the delivery;
        /// Distance = From station to sender + from sender to customer + dis from target to closest avialble station.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="closestStationToSender"></param>
        /// <param name="senderPosition"></param>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        private int calcDroneBatteryForDroneDelivery(IDal.DO.Parcel parcel, IDal.DO.Station closestStationToSender, Position senderPosition, Position targetPosition)
        {
            double disFromStationToSender = 0; // only for a parcel who wasnt picked up.
            double disFromSenderToCustomer = distance(senderPosition, targetPosition);
            //from target to closest station;
            IDal.DO.Station closestAvailbleStationFromTarget = findAvailbleAndClosestStationForDrone(targetPosition);
            double disFromTargetTostation = distance(targetPosition, new Position() { Latitude = closestAvailbleStationFromTarget.Latitude, Longitude = closestAvailbleStationFromTarget.Longitude });
            if (parcel.PickUp == new DateTime())
            {
                disFromStationToSender = distance(new Position() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude }, senderPosition);
            }
            double sumDisForDrone = disFromStationToSender + disFromSenderToCustomer + disFromTargetTostation;
            double sumBattery = sumDisForDrone * requestElectricity()[(int)parcel.Weight];
            return r.Next((int)sumBattery, 100);
        }
       
        /// <summary>
        /// Find availble and closest station for drone;
        /// </summary>
        /// <param name="dronePosition">Current Drone position</param>
        /// <returns></returns>
        private IDal.DO.Station findAvailbleAndClosestStationForDrone(Position dronePosition)
        {
            IEnumerable<IDal.DO.Station> stations = dal.displayStations();
            IDal.DO.Station availbleCLosestStation = new IDal.DO.Station();
            double dis = -1;
            double minDis = -1;
            int fullChargingSlots;
            foreach(IDal.DO.Station station in stations)
            {
                fullChargingSlots = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
                if (station.ChargeSlots - fullChargingSlots > 0) //has empty charging slots
                {
                    dis = distance(dronePosition, new Position() { Latitude = station.Latitude, Longitude = station.Longitude });
                    if (minDis == -1)
                    {
                        minDis = dis;
                    }
                    else if (minDis > dis)
                    {
                        minDis = dis;
                        availbleCLosestStation = station;
                    }
                }
            }
            return availbleCLosestStation;
        }
        /// <summary>
        /// Find a customer who received a parcel.
        /// </summary>
        /// <returns></returns>
        private List<IDal.DO.Customer> findCustomersWithDeliveredParcel()
        {
            IEnumerable<IDal.DO.Parcel> parcels = dal.getParcelWithSpecificCondition(p => (p.Delivered != null));
            List<IDal.DO.Customer> customersWithDeliveredParcels = new List<IDal.DO.Customer>();
            foreach(IDal.DO.Parcel parcel in parcels)
            {
                customersWithDeliveredParcels.Add(dal.getCustomerWithSpecificCondition(c => c.ID == parcel.TargetId).First());
            }
            return customersWithDeliveredParcels;
        }

        private double[] requestElectricity()
        {
            return dal.electricityUseByDrone();
        }
        /// <summary>
        /// Amount of power consumption of Drone
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        private double requestElectricity(int choice)
        {
            switch ((Electricity)choice)
            {
                case Electricity.Empty:
                    return empty;
                case Electricity.LightWeight:
                    return lightWeight;
                case Electricity.MediumWeight:
                    return mediumWeight;
                case Electricity.HeavyWeight:
                    return chargingRate;
                case Electricity.ChargingRate:
                    return chargingRate;
                default:
                    return 0;
            }
        }
    }
}