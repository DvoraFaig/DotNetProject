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
    public sealed partial class DalXml : DalApi.Idal
    {
        /// <summary>
        /// Add the new drone to Drones.
        /// </summary>
        /// <param name="newDrone">drone to add.</param>
        public void AddDrone(DO.Drone newDrone)
        {
            Drone drone;
            try
            {
                drone = getDroneWithSpecificCondition(d => d.Id == newDrone.Id).First();
                if (drone.IsActive)
                    throw new Exceptions.ObjExistException(typeof(Drone), newDrone.Id);

                changeDroneInfo(newDrone);
                throw new Exceptions.DataChanged(typeof(Drone), newDrone.Id);

            }
            catch (Exception)
            {
                XElement DroneRoot = XMLTools.LoadData(dir + droneFilePath);
                DroneRoot.Add(returnDroneXElement(newDrone));
                DroneRoot.Save(dir + droneFilePath);
            }

            #region LoadListFromXMLSerializer
            //Drone drone;
            //try
            //{
            //    drone = getDroneWithSpecificCondition(d => d.Id == newDrone.Id).First();
            //    if (drone.IsActive)
            //        throw new Exceptions.ObjExistException(typeof(Drone), newDrone.Id);

            //    changeDroneInfo(newDrone);
            //    throw new Exceptions.DataChanged(typeof(Drone), newDrone.Id);
            //}
            //catch (Exception)
            //{
            //  IEnumerable<DO.Drone> droneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //  droneList.ToList().Add(newDrone);
            //  DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);            //    throw new Exceptions.ObjNotExistException(typeof(Drone), newDrone.Id);
            //}
            #endregion
        }

        /// <summary>
        /// Receive a DO drone and return a XElemnt drone - copy information.
        /// </summary>
        /// <param name="newDrone"></param>
        /// <returns></returns>
        private XElement returnDroneXElement(DO.Drone newDrone)
        {
            XElement Id = new XElement("Id", newDrone.Id);
            XElement Model = new XElement("Model", newDrone.Model);
            XElement MaxWeight = new XElement("MaxWeight", newDrone.MaxWeight);
            XElement IsActive = new XElement("IsActive", true);
            return new XElement("Drone", Id, Model, MaxWeight, MaxWeight, IsActive);
        }

        /// <summary>
        /// Receive a XElement drone and return a DO drone - copy information.
        /// </summary>
        /// <param name="newDrone"></param>
        /// <returns></returns>
        private Drone returnDrone(XElement drone)
        {
            return new DO.Drone()
            {
                Id = Convert.ToInt32(drone.Element("Id").Value),
                Model = drone.Element("Model").Value,
                MaxWeight = (WeightCategories)Convert.ToInt32(drone.Element("MaxWeight").Value),
                IsActive = Convert.ToBoolean((drone.Element("IsActive").Value))
            };
        }

        /// <summary>
        /// if drone exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.</summary>
        /// <param name="droneToRemove">The drone to remove. droneToRemove.IsActive = false</param>
        public void removeDrone(Drone droneToRemove)
        {
            try
            {
                XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
                XElement droneXElemnt = (from d in droneRoot.Elements()
                                         where Convert.ToInt32(d.Element("Id").Value) == droneToRemove.Id
                                         select d).FirstOrDefault();

                if (droneXElemnt != null)
                    droneXElemnt.Element("IsActive").Value = "false";
            }
            catch (Exception)
            {
                throw new Exceptions.ObjNotExistException(typeof(Drone), droneToRemove.Id);
            }

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Drone> dronesList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //try
            //{
            //    Drone drone = getDroneWithSpecificCondition(d => d.Id == droneToRemove.Id).First();
            //    if (drone.IsActive)
            //        drone.IsActive = false;
            //    changeDroneInfo(drone);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exceptions.NoMatchingData(typeof(Drone), droneToRemove.Id, e1);
            //}


            //public void removeDrone(int index)
            //{
            //    XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
            //    int i = 0;
            //    XElement droneXElemnt = (from d in droneRoot.Elements()
            //                             where i++ == index
            //                             select d).FirstOrDefault();

            //    if (droneXElemnt != null)
            //        droneXElemnt.Element("IsActive").Value = "false";

            //    #region LoadListFromXMLSerializer
            //    //IEnumerable<DO.Drone> dronesList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //    //try
            //    //{
            //    //    Drone drone = getDroneWithSpecificCondition(d => d.Id == droneToRemove.Id).First();
            //    //    if (drone.IsActive)
            //    //        drone.IsActive = false;
            //    //    changeDroneInfo(drone);
            //    //}
            //    //catch (Exception e1)
            //    //{
            //    //    throw new Exceptions.NoMatchingData(typeof(Drone), droneToRemove.Id, e1);
            //    //}
            //    #endregion
            //}
            #endregion
        }




        /// <summary>
        /// Get all drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);

            return (from d in droneRoot.Elements()
                    orderby Convert.ToInt32(d.Element("Id").Value)
                    select returnDrone(d));

            #region found a better way
            //IEnumerable<DO.Drone> dronesList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //return from item in dronesList
            //       orderby item.Id
            //       select item;
            #endregion
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
            XElement droneElement = (from p in droneRoot.Elements()
                                    where Convert.ToInt32(p.Element("Id").Value) == droneWithUpdateInfo.Id
                                    select p).FirstOrDefault();

            //if (droneElement == (default(XElement)))
            //    throw new Exceptions.ObjNotExistException(typeof(Parcel), droneWithUpdateInfo.Id);

            XElement xElementUpdateDrone = returnDroneXElement(droneWithUpdateInfo);
            droneElement = xElementUpdateDrone;
            droneRoot.Save(dir + droneFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Drone> droneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath).ToList();
            //int index = droneList.FindIndex(t => t.Id == droneWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Drone), droneWithUpdateInfo.Id);

            //droneList[index] = droneWithUpdateInfo;
            //XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);
            #endregion
        }

        /// <summary>
        /// Get a Drone/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate)
        {
            XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
            return (from d in droneRoot.Elements()
                    where predicate(returnDrone(d))
                    select returnDrone(d));

            #region LoadListFromXMLSerializer
            //    IEnumerable<DO.Drone> droneList = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //    return (from drone in droneList
            //            where predicate(drone)
            //            select drone);
            #endregion
        }

        /// <summary>
        /// If drone with the requested id exist & active
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsDroneActive(int requestedId)
        {
            XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
            XElement droneXElemnt = (from d in droneRoot.Elements()
                                     where Convert.ToInt32(d.Element("Id").Value) == requestedId
                                     && Convert.ToBoolean(d.Element("IsActive").Value)
                                     select d).FirstOrDefault();

            if (droneXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Drone> dronesLits = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //if (dronesLits.Any(d => d.Id == requestedId && d.IsActive))
            //    return true;
            //return false;
            #endregion
        }

        /// <summary>
        /// If drone with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsDroneById(int requestedId)
        {
            XElement droneRoot = XMLTools.LoadData(dir + droneFilePath);
            XElement droneXElemnt;
            droneXElemnt = (from d in droneRoot.Elements()
                            where Convert.ToInt32(d.Element("Id").Value) == requestedId
                            select d).FirstOrDefault();

            if (droneXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Drone> dronesLits = XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //if (dronesLits.Any(d => d.Id == requestedId))
            //    return true;
            //return false;
            #endregion
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
        public double[] electricityUseByDrone()
        {
            return XMLTools.LoadListFromXMLSerializer<double>(dir + configFilePath).ToArray();
        }
    }
}
