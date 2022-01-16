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
        /// Add a new station. checks if this station exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// </summary>
        /// <param name="stationToAdd">The new station to add.</param>
        public void AddStation(Station stationToAdd)
        {
            DO.Station station;
            try
            {
                station = dal.getStationWithSpecificCondition(s => s.Id == stationToAdd.Id).First();
                if (station.IsActive)
                    throw new ObjExistException(typeof(BO.Station), stationToAdd.Id);
                station.IsActive = true;
                dal.changeStationInfo(station); //convertBLToDalStation(stationToAdd)
                string message = "";
                if (stationToAdd.StationPosition.Latitude != station.Latitude || stationToAdd.StationPosition.Longitude != station.Longitude)
                    message += "Position";
                if (stationToAdd.DroneChargeAvailble != station.ChargeSlots)
                    message += ", Amount charge slots";
                if (message != "")
                    throw new Exceptions.DataOfOjectChanged(typeof(Station), station.Id, $"Data changed: {message} was changed");
                return;
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            dal.AddStation(convertBLToDalStation(stationToAdd));
        }

        /// <summary>
        /// Returns a IEnumerable<StationToList> by recieving staions and converting them to BLStationToList
        /// </summary>
        /// <returns></returns>
        public List<StationToList> GetStationsToList()
        {
            IEnumerable<DO.Station> stations = dal.GetStations();
            List<StationToList> stationToList = new List<StationToList>();
            foreach (var station in stations)
            {
                int occupiedChargeSlotsInStation = dal.getDroneChargeWithSpecificCondition(d => d.StationId == station.Id).Count();
                int avilableChargeSlotsInStation = station.ChargeSlots - occupiedChargeSlotsInStation;
                stationToList.Add(new StationToList() { Id = station.Id, Name = station.Name, DroneChargeAvailble = avilableChargeSlotsInStation, DroneChargeOccupied = occupiedChargeSlotsInStation });

            }
            return stationToList;
        }

        /// <summary>
        /// Return IEnumerable<StationToList> by receiving a converted list of station (one of BO.Station is availableChargeSlots).
        /// </summary>
        /// <param name="amountAvilableSlots"></param>
        /// <returns></returns>
        public IEnumerable<StationToList> GetStationsWithFreeSlots(int amountAvilableSlots = 0)
        {
            List<StationToList> stationToList = GetStationsToList();
            return (from station in stationToList
                    where station.DroneChargeAvailble >= amountAvilableSlots
                    select station);
        }

        /// <summary>
        /// Return a BO.Station(converted) by an id from dal.getStationWithSpecificCondition
        /// </summary>
        /// <param name="stationrequestedId">The id of the drone that's requested</param>
        /// <returns></returns>
        public Station GetStationById(int stationrequestedId)
        {
            DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == stationrequestedId).First();
            Station BLstation = convertDalToBLStation(s);
            return BLstation;
        }

        /// <summary>
        /// Change i=info of station
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="ChargeSlots"></param>
        public void changeInfoOfStation(int id, string name = null, int ChargeSlots = -1)//-1 is defualt value
        {
            DO.Station s = dal.getStationWithSpecificCondition(s => s.Id == id).First();
            if (name != null)
                s.Name = name;
            if (s.ChargeSlots <= ChargeSlots)
                s.ChargeSlots = ChargeSlots;
            if (s.ChargeSlots > ChargeSlots)
            {
                int amountDroneChargesFull = dal.getDroneChargeWithSpecificCondition(station => station.StationId == s.Id).Count();
                if (amountDroneChargesFull < ChargeSlots)
                    s.ChargeSlots = ChargeSlots;
                else
                    throw new Exception($"The amount Charging slots you want to change is smaller than the amount of drones that are charging now in station number {id}");
            }
            dal.changeStationInfo(s);
        }

        /// <summary>
        /// Remove specific station
        /// </summary>
        /// <param name="stationId">remove current station with stationId</param>
        public void RemoveStation(Station station)
        {
            try
            {
                if (dal.IsStationActive(station.Id))
                    dal.removeStation(convertBLToDalStation(station));
                else
                    throw new Exceptions.ObjExistException(typeof(Station), station.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
        }
    }
}
