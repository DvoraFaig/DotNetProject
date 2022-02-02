using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Add the new station to Stations.
        /// </summary>
        /// <param name="newStation">The station to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            Station drone;
            try
            {
                drone = getStationWithSpecificCondition(d => d.Id == newStation.Id).First();
                if (drone.IsActive)
                    throw new Exceptions.ObjExistException(typeof(Station), newStation.Id);

                changeStationInfo(newStation);
                throw new Exceptions.DataChanged(typeof(Station), newStation.Id);

            }
            catch (Exception)
            {
                DataSource.Stations.Add(newStation);
            }
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations()
        {
            //return getStationWithSpecificCondition(s => s.IsActive == true);
            
            return from s in DataSource.Stations
                   where s.IsActive == true
                   orderby s.Id
                   select s;
        }

        /// <summary>
        /// Get a Station/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a station/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate)
        {
            return (from station in DataSource.Stations
                    where predicate(station)
                    orderby station.Id
                    select station);
        }

        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            int index = DataSource.Stations.FindIndex(s => s.Id == stationWithUpdateInfo.Id);
            DataSource.Stations[index] = stationWithUpdateInfo;
        }

        /// <summary>
        /// If station exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="stationToRemove">The station to remove. stationToRemove.IsActive = false</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void removeStation(Station stationToRemove)
        {
            try
            {
                Station station = (from s in DataSource.Stations
                                   where s.Id == stationToRemove.Id
                                   && s.Name == stationToRemove.Name
                                   && s.ChargeSlots == stationToRemove.ChargeSlots
                                   && s.Latitude == stationToRemove.Latitude
                                   && s.Longitude == stationToRemove.Longitude
                                   select s).First();
                station.IsActive = false;
                changeStationInfo(station);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Station), stationToRemove.Id, e1);
            }
        }

        /// <summary>
        /// Return how much stations there is.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int amountStations()
        {
            return DataSource.Stations.Count();
        }

        /// <summary>
        /// If station with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean IsStationById(int requestedId)
        {
            return DataSource.Stations.Any(s => s.Id == requestedId);
        }

        /// <summary>
        /// If station with the requested id exist and active.
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean IsStationActive(int requestedId)
        {
            return DataSource.Stations.Any(s => s.Id == requestedId && s.IsActive == true);
        }


    }
}
