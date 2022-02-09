using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;
using System.Runtime.CompilerServices;



namespace Dal
{
    public sealed partial class DalXml : DalApi.IDal
    {

        /// <summary>
        /// Add the new station to Stations.
        /// </summary>
        /// <param name="newStation">The station to add.</param>
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
                XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
                stationRoot.Add(returnStationXElement(newStation));
                stationRoot.Save(dir + stationFilePath);
            }

            #region with a check if station exist checked
            //XElement stationXElemnt;
            //if(IsStationById(newStation.Id))
            //    throw new DO.Exceptions.ObjExistException(typeof(DO.Station), newStation.Id);
            //stationXElemnt = (from s in stationRoot.Elements()
            //                  where Convert.ToInt32(s.Element("Id").Value) == newStation.Id
            //                  select s).FirstOrDefault();
            //if (stationXElemnt != null)
            //{
            //    throw new DO.Exceptions.ObjExistException(typeof(DO.Station), newStation.Id);
            //}
            #endregion 
        }

        /// <summary>
        /// Receive a DO station and return a XElemnt station - copy information.
        /// </summary>
        /// <param name="newStation"></param>
        /// <returns></returns>
        private XElement returnStationXElement(DO.Station newStation)
        {
            return newStation.ToXElement<Station>();
            //XElement Id = new XElement("Id", newStation.Id);
            //XElement Name = new XElement("Name", newStation.Name);
            //XElement ChargeSlots = new XElement("ChargeSlots", newStation.ChargeSlots);
            //XElement Latitude = new XElement("Latitude", newStation.Latitude);
            //XElement Longitude = new XElement("Longitude", newStation.Longitude);
            //XElement IsActive = new XElement("IsActive", newStation.IsActive);
            //return new XElement("Station", Id, Name, ChargeSlots, Latitude, Longitude, IsActive);
        }

        /// <summary>
        /// Receive a XElement station and return a DO station - copy information.
        /// </summary>
        /// <param name="newStation"></param>
        /// <returns></returns>
        private Station returnStation(XElement station)
        {
            return station.FromXElement<Station>();
            //return new DO.Station()
            //{
            //    Id = Convert.ToInt32(station.Element("Id").Value),
            //    Name = station.Element("Name").Value,
            //    ChargeSlots = Convert.ToInt32(station.Element("ChargeSlots").Value),
            //    Latitude = Convert.ToInt32(station.Element("Latitude").Value),
            //    Longitude = Convert.ToInt32(station.Element("Longitude").Value),
            //    IsActive = Convert.ToBoolean((station.Element("IsActive").Value))
            //};
        }

        /// <summary>
        /// If station exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="stationToRemove">The station to remove. stationToRemove.IsActive = false</param>
        public void removeStation(Station stationToRemove)
        {
            try
            {
                XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
                XElement stationXElemnt = (from s in stationRoot.Elements()
                                           where Convert.ToInt32(s.Element("Id").Value) == stationToRemove.Id
                                           select s).First();
                if (stationXElemnt != null)
                    stationXElemnt.Element("IsActive").Value = "false";
            }
            catch (Exception e1)
            {
                throw new Exceptions.ObjNotExistException(typeof(Station), stationToRemove.Id, e1);
            }

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Station> stationsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            //try
            //{
            //    Station station = getStationWithSpecificCondition(s => s.Id == stationToRemove.Id).First();
            //    if (station.IsActive)
            //        station.IsActive = false;
            //    changeStationInfo(station);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exceptions.NoMatchingData(typeof(Station), stationToRemove.Id, e1);
            //}
            #endregion
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations()
        {
            XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            return (from s in stationRoot.Elements()
                    orderby Convert.ToInt32(s.Element("Id").Value)
                    select returnStation(s));

            #region found better way
            //IEnumerable<DO.Station> studentsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            //return from item in studentsList
            //       orderby item.Id
            //       select item;            
            #endregion

        }

        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            XElement stationElemnt = (from p in stationRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == stationWithUpdateInfo.Id
                                      select p).FirstOrDefault();

            //if (stationElemnt == (default(XElement)))
            //    throw new Exceptions.ObjNotExistException(typeof(Parcel), stationWithUpdateInfo.Id);

            XElement xElementUpdateStation = returnStationXElement(stationWithUpdateInfo);
            stationElemnt = xElementUpdateStation;
            stationRoot.Save(dir + stationFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Station> studentsList = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).ToList();
            //int index = studentsList.FindIndex(t => t.Id == stationWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Station), stationWithUpdateInfo.Id);

            //studentsList[index] = stationWithUpdateInfo;
            //XMLTools.SaveListToXMLSerializer<DO.Station>(studentsList, dir + stationFilePath);
            #endregion
        }

        /// <summary>
        /// Get a Station/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a station/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate)
        {
            XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            return (from s in stationRoot.Elements()
                    where predicate(returnStation(s))
                    select returnStation(s));

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Station> stationList = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            //return (from station in stationList
            //        where predicate(station)
            //        select station);
            #endregion
        }

        /// <summary>
        /// If station with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsStationById(int requestedId)
        {
            XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            XElement stationXElemnt = (from s in stationRoot.Elements()
                                       where Convert.ToInt32(s.Element("Id").Value) == requestedId
                                       select s).FirstOrDefault();
            if (stationXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Station> stationsLists = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            //if (stationsLists.Any(s => s.Id == requestedId))
            //    return true;
            //return false;
            #endregion
        }

        /// <summary>
        /// If station with the requested id exist & active
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsStationActive(int requestedId)
        {
            XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            XElement stationXElemnt = (from s in stationRoot.Elements()
                                       where Convert.ToInt32(s.Element("Id").Value) == requestedId
                                       && Convert.ToBoolean(s.Element("IsActive").Value)
                                       select s).FirstOrDefault();
            if (stationXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Station> stationsLists = XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            //if (stationsLists.Any(s => s.Id == requestedId && s.IsActive))
            //    return true;
            //return false;
            #endregion
        }

        /// <summary>
        /// Return how much stations there is.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int amountStations()
        {
            return XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).Count();

            //XElement stationRoot = XMLTools.LoadData(dir + stationFilePath);
            //return (from p in stationRoot.Elements()
            //        select p).Count();
        }
    }
}
