using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;
using static IBL.BO.Exceptions;
/*ChargeSlots = s.DroneChargeAvailble ????????????*/



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
            dronesInBL = new List<BLDrone>();
            dal = IDal.DalFactory.factory("DalObject"); //start one time an IDal.DO.IDal object.
            empty = dal.electricityUseByDrone()[0];
            lightWeight = dal.electricityUseByDrone()[1];
            mediumWeight = dal.electricityUseByDrone()[2];
            heavyWeight = dal.electricityUseByDrone()[3];
            chargingRate = dal.electricityUseByDrone()[4];

            IEnumerable<IDal.DO.Drone> drones = dal.displayDrone();
            IDal.DO.Parcel p;
            IDal.DO.Station s;


            IDal.DO.Customer sender, target;
            BLPosition senderPosition, targetPosition;
            BLDrone blDrone = new BLDrone();

            foreach(IDal.DO.Drone d in drones )
            {
                blDrone = copyDalToBLDroneInfo(d);
                try
                {
                    p = dal.getParcelWithSpecificCondition(p=> p.DroneId == d.Id).First();
                    IDal.DO.Station closestStationToSender = new IDal.DO.Station();
                    sender = dal.getCustomerWithSpecificCondition(parcel => parcel.ID == p.SenderId).First();
                    target = dal.getCustomerWithSpecificCondition(parcel => parcel.ID == p.TargetId).First();
                    blDrone.ParcelInTransfer = createBLParcelInTransfer(p, sender, target);//.First();
                    senderPosition = blDrone.ParcelInTransfer.SenderPosition;
                    targetPosition = blDrone.ParcelInTransfer.TargetPosition;
                    if (p.PickUp == null) //position like the closest station to the sender of parcel.
                    {
                        closestStationToSender = findAvailbleAndClosestStationForDrone(senderPosition); //תחנה קרובה לשלוח במצב הטענה? //אם אינו נכנס למצב הטענה PositionFromClosestStation () //אם כן updateDroneCharge
                        blDrone.DronePosition = new BLPosition() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude };
                    }
                    else if (p.Delivered == null) //else position sender of parcel.
                    {
                        blDrone.DronePosition = new BLPosition() { Latitude = sender.Latitude, Longitude = sender.Longitude };
                    }
                    blDrone.Battery = calcDroneBatteryForDroneDelivery(p, closestStationToSender, senderPosition, targetPosition);
                }

                catch (IDal.DO.DalExceptions.ObjNotExistException ) // if drone is not delivery status
                {
                    blDrone.Status = (DroneStatus)r.Next(0, 1); // Available / Maintenance
                    if (blDrone.Status == DroneStatus.Maintenance)
                    {
                        s = findAvailbleAndClosestStationForDrone(blDrone.DronePosition);
                        blDrone.DronePosition = new BLPosition() { Longitude = s.Longitude, Latitude = s.Latitude };
                        blDrone.Battery = r.Next(0, 20);
                    }
                    else  //DroneStatus.Available
                    {
                        List<IDal.DO.Customer> cWithDeliveredP = findCustomersWithDeliveredParcel();
                        if (cWithDeliveredP.Count > 0)
                        {
                            target = dal.getCustomerWithSpecificCondition(c => c.ID == cWithDeliveredP[r.Next(0, cWithDeliveredP.Count)].ID).First();
                            blDrone.DronePosition = new BLPosition() { Longitude = target.Longitude, Latitude = target.Latitude };
                        }
                    }
                }
                dronesInBL.Add(blDrone);
            }
        }
        private int calcDroneBatteryForDroneDelivery(IDal.DO.Parcel p, IDal.DO.Station closestStationToSender, BLPosition senderPosition, BLPosition targetPosition)
        {
            double disFromStationToSender = 0; // only for a parcel who wasnt picked up.
            double disFromSenderToCustomer = distance(senderPosition, targetPosition);
            //from target to closest station;
            IDal.DO.Station closestAvailbleStationFromTarget = findAvailbleAndClosestStationForDrone(targetPosition);
            double disFromTargetTostation = distance(targetPosition, new BLPosition() { Latitude = closestAvailbleStationFromTarget.Latitude, Longitude = closestAvailbleStationFromTarget.Longitude });
            if (p.PickUp == new DateTime())
            {
                disFromStationToSender = distance(new BLPosition() { Latitude = closestStationToSender.Latitude, Longitude = closestStationToSender.Longitude }, senderPosition);
            }
            double sumDisForDrone = disFromStationToSender + disFromSenderToCustomer + disFromTargetTostation;
            double sumBattery = sumDisForDrone * requestElectricity()[(int)p.Weight];
            return r.Next((int)sumBattery, 100);
        }

        private List<IDal.DO.Station> findAvailbleStationForDrone()
        {
            IEnumerable<IDal.DO.Station> stations = dal.displayStations();
            List<IDal.DO.Station> availbleStations = new List<IDal.DO.Station>();
            int busyChargingSlots;
            foreach(IDal.DO.Station s in stations)
            {
                busyChargingSlots = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == s.Id).Count();
                if (s.ChargeSlots - busyChargingSlots > 0)//has empty charging slots
                {
                    availbleStations.Add(s);
                }
            }
            return availbleStations;
        }

        private IDal.DO.Station findAvailbleAndClosestStationForDrone(BLPosition d)
        {
            IEnumerable<IDal.DO.Station> stations = dal.displayStations();
            IDal.DO.Station availbleCLosestStation = new IDal.DO.Station();
            double dis = -1;
            double minDis = -1;
            int busyChargingSlots;
            foreach(IDal.DO.Station s in stations)
            {
                busyChargingSlots = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == s.Id).Count();
                if (s.ChargeSlots - busyChargingSlots > 0) //has empty charging slots
                {
                    dis = distance(d, new BLPosition() { Latitude = s.Latitude, Longitude = s.Longitude });
                    if (minDis == -1)
                    {
                        minDis = dis;
                    }
                    else if (minDis > dis)
                    {
                        minDis = dis;
                        availbleCLosestStation = s;
                    }
                }
            }
            return availbleCLosestStation;
        }

        private List<IDal.DO.Customer> findCustomersWithDeliveredParcel()
        {
            IEnumerable<IDal.DO.Parcel> parcels = dal.getParcelWithSpecificCondition(p => (p.Delivered != null));
            List<IDal.DO.Customer> customersWithDeliveredParcels = new List<IDal.DO.Customer>();
            foreach(IDal.DO.Parcel p in parcels)
            {
                customersWithDeliveredParcels.Add(dal.getCustomerWithSpecificCondition(c => c.ID == p.TargetId).First);
            }
            return customersWithDeliveredParcels;
        }

        private double[] requestElectricity()
        {
            return dal.electricityUseByDrone();
        }

        private double requestElectricity(int choice)
        {
            switch ((Electricity)choice)
            {
                case Electricity.empty:
                    return empty;
                case Electricity.lightWeight:
                    return lightWeight;
                case Electricity.mediumWeight:
                    return mediumWeight;
                case Electricity.heavyWeight:
                    return chargingRate;
                case Electricity.chargingRate:
                    return chargingRate;
                default:
                    return 0;
            }
        }
    }
}