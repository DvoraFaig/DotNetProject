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
        /// Add the new drone to Drones.
        /// </summary>
        /// <param name="newDrone">drone to add.</param>
        public void AddDrone(DO.Drone newDrone)
        {
            XElement DroneRoot = DL.XMLTools.LoadData(dir + droneFilePath);
            DroneRoot.Add(returnDroneXElement(newDrone));
            DroneRoot.Save(dir + droneFilePath);
            #region found better way
            //IEnumerable<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            //if (droneList.Any(d => d.Id == newDrone.Id))
            //{
            //    throw new DO.Exceptions.ObjExistException(typeof(DO.Drone), newDrone.Id);
            //}
            //droneList.ToList().Add(newDrone);
            //DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);
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
        /// if drone exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.</summary>
        /// <param name="droneToRemove">The drone to remove. droneToRemove.IsActive = false</param>
        public void removeDrone(Drone droneToRemove)
        {
            IEnumerable<DO.Drone> dronesList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            try
            {
                Drone drone = getDroneWithSpecificCondition(d => d.Id == droneToRemove.Id).First();
                if (drone.IsActive)
                    drone.IsActive = false;
                changeDroneInfo(drone);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Drone), droneToRemove.Id, e1);
            }
            #region found better way
            //XElement droneRoot = DL.XMLTools.LoadData(dir + droneFilePath);
            //XElement droneXElemnt;
            //droneXElemnt = (from d in droneRoot.Elements()
            //                where Convert.ToInt32(d.Element("Id").Value) == droneToRemove.Id
            //                select d).FirstOrDefault();
            //if (droneXElemnt != null)
            //{
            //    droneXElemnt.Element("IsActive").Value = "false";
            //}
            #endregion
        }

        /// <summary>
        /// Get all drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            IEnumerable<DO.Drone> dronesList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            return from item in dronesList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            XElement droneRoot = DL.XMLTools.LoadData(dir + droneFilePath);
            XElement droneElemnt = (from p in droneRoot.Elements()
                                    where Convert.ToInt32(p.Element("Id").Value) == droneWithUpdateInfo.Id
                                    select p).FirstOrDefault();
            XElement xElementUpdateDrone = returnDroneXElement(droneWithUpdateInfo);
            droneElemnt = xElementUpdateDrone;
            droneRoot.Save(dir + droneFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath).ToList();
            //int index = droneList.FindIndex(t => t.Id == droneWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Drone), droneWithUpdateInfo.Id);

            //droneList[index] = droneWithUpdateInfo;
            //DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);
            #endregion
        }


    }
}
