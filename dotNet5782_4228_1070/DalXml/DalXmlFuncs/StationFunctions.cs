using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;


namespace Dal
{
    public sealed partial class DalXml : DalApi.Idal
    {

        /// <summary>
        /// Add the new station to Stations.
        /// </summary>
        /// <param name="newStation">The station to add.</param>
        public void AddStation(Station newStation)
        {
            XElement stationRoot = DL.XMLTools.LoadData(dir + stationFilePath);
            #region was checked
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
            stationRoot.Add(returnStationXElement(newStation));
            stationRoot.Save(dir + stationFilePath);
        }

        /// <summary>
        /// Receive a DO station and return a XElemnt station - copy information.
        /// </summary>
        /// <param name="newStation"></param>
        /// <returns></returns>
        private XElement returnStationXElement(DO.Station newStation)
        {
            XElement Id = new XElement("Id", newStation.Id);
            XElement Name = new XElement("Name", newStation.Name);
            XElement ChargeSlots = new XElement("ChargeSlots", newStation.ChargeSlots);
            XElement Latitude = new XElement("Latitude", newStation.Latitude);
            XElement Longitude = new XElement("Longitude", newStation.Longitude);
            XElement IsActive = new XElement("IsActive", true);
            return new XElement("Station", Id, Name, ChargeSlots, Latitude, Longitude, IsActive);
        }

        /// <summary>
        /// If station exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="stationToRemove">The station to remove. stationToRemove.IsActive = false</param>
        public void removeStation(Station stationToRemove)
        {
            IEnumerable<DO.Station> stationsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            try
            {
                Station station = getStationWithSpecificCondition(s => s.Id == stationToRemove.Id).First();
                if (station.IsActive)
                    station.IsActive = false;
                changeStationInfo(station);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Station), stationToRemove.Id, e1);
            }
            #region found better way
            //XElement stationRoot = DL.XMLTools.LoadData(dir + stationFilePath);
            //XElement stationXElemnt;
            //stationXElemnt = (from s in stationRoot.Elements()
            //                   where Convert.ToInt32(s.Element("Id").Value) == stationToRemove.Id
            //                   select s).FirstOrDefault();
            //if (stationXElemnt != null)
            //{
            //    stationXElemnt.Element("IsActive").Value = "false";
            //}
            #endregion
        }
        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations()
        {
            IEnumerable<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            return from item in studentsList
                   orderby item.Id
                   select item;
            #region found better way
            //XElement stationRoot = DL.XMLTools.LoadData(dir + stationFilePath);
            //List<DO.Station> stationList = new List<Station>();
            //try
            //{
            //    stationList = (from p in stationRoot.Elements()
            //                   select new DO.Station()
            //                   {
            //                       Id = Convert.ToInt32(p.Element("IdTest").Value),
            //                       Name = p.Element("Name").Value,
            //                       ChargeSlots = Convert.ToInt32(p.Element("ChargeSlots").Value),
            //                       //ChargeSlots = Convert.ToDateTime(p.Element("ChargeSlots").Value),
            //                       Latitude = Convert.ToInt32(p.Element("Latitude").Value),
            //                       Longitude = Convert.ToInt32(p.Element("Longitude").Value)
            //                   });
            //                       //IsActive = ((p.Element("IsActive").Value))
            //}
            //catch
            //{
            //    throw new Exception();
            //}
            //return stationList;
            #endregion
        }

        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            XElement stationRoot = DL.XMLTools.LoadData(dir + stationFilePath);
            XElement stationElemnt = (from p in stationRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == stationWithUpdateInfo.Id
                                      select p).FirstOrDefault();
            XElement xElementUpdateStation = returnStationXElement(stationWithUpdateInfo);
            stationElemnt = xElementUpdateStation;
            stationRoot.Save(dir + stationFilePath);
            #region LoadListFromXMLSerializer
            //List<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).ToList();
            //int index = studentsList.FindIndex(t => t.Id == stationWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Station), stationWithUpdateInfo.Id);

            //studentsList[index] = stationWithUpdateInfo;
            //DL.XMLTools.SaveListToXMLSerializer<DO.Station>(studentsList, dir + stationFilePath);
            #endregion
        }
    }
}
