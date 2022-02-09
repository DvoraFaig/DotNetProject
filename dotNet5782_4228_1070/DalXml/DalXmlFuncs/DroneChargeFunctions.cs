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
        /// Add the new DroneCharge to DroneCharges.
        /// </summary>
        /// <param name="newDroneCharge">DroneCharge to add.</param>
        public void AddDroneToCharge(DroneCharge newDroneCharge)
        {

            XElement droneChargeRoot = XMLTools.LoadData(dir + droneChargeFilePath);
            droneChargeRoot.Add(returnDroneChargeXElement(newDroneCharge));
            droneChargeRoot.Save(dir + droneChargeFilePath);

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.DroneCharge> droneChargesList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            //List<DroneCharge> newList = new List<DroneCharge>();
            //if (droneChargesList.Count() != 0)
            //{
            //    newList = droneChargesList.Cast<DO.DroneCharge>().ToList();
            //}
            //newList.Add(newDroneCharge);
            //XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(newList, dir + droneChargeFilePath);
            #endregion
        }

        /// <summary>
        /// Receive a DO DroneCharge and return a XElemnt DroneCharge - copy information.
        /// </summary>
        /// <param name="newDroneCharge"></param>
        /// <returns></returns>
        private XElement returnDroneChargeXElement(DO.DroneCharge newDroneCharge)
        {
            XElement DroneId = new XElement("DroneId", newDroneCharge.DroneId);
            XElement StationId = new XElement("StationId", newDroneCharge.StationId);
            return new XElement("DroneCharge", DroneId, StationId);
        }

        /// <summary>
        /// Receive a  XElement DroneCharge and return a DO DroneCharge - copy information.
        /// </summary>
        /// <param name="newDroneCharge"></param>
        /// <returns></returns>
        private DroneCharge returnDroneCharge(XElement drone)
        {
            return new DO.DroneCharge()
            {
                DroneId = Convert.ToInt32(drone.Element("DroneId").Value),
                StationId = Convert.ToInt32(drone.Element("StationId").Value),
            };
        }

        /// <summary>
        /// Remove charging drone by drone id.
        /// <param name="droneId">The charging drone with droneId - to remove</param>
        public void removeDroneChargeByDroneId(int droneId)
        {
            try
            {
                List<DO.DroneCharge> dronesList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath).ToList();
                int index = dronesList.FindIndex(d => d.DroneId == droneId);
                dronesList.RemoveAt(index);
                XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(dronesList, dir + droneChargeFilePath);
            }
            catch (Exception e1)
            {
                throw new Exceptions.ObjNotExistException(typeof(Drone), droneId, e1);
            }
        }

        

        /// <summary>
        /// Get all droneCharge.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneCharges()
        {
            IEnumerable<DO.DroneCharge> droneChargesList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            return from item in droneChargesList
                   orderby item.StationId 
                   select item;

            #region LoadData
            //XElement droneChargeRoot = XMLTools.LoadData(dir + droneChargeFilePath);
            //return (from d in droneChargeRoot.Elements()
            //                 orderby Convert.ToInt32(d.Element("Id").Value)
            //                 select returnDroneCharge(d));
            #endregion
        }

        /// <summary>
        /// Get a DroneCharge/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone charge /s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate)
        {
            IEnumerable<DO.DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            return (from droneCharge in droneChargeList
                    where predicate(droneCharge)
                    select droneCharge);

            #region LoadData
            //XElement droneChargeRoot = XMLTools.LoadData(dir + droneChargeFilePath);
            //return (from d in droneChargeRoot.Elements()
            //        where predicate(returnDroneCharge(d))
            //        select returnDroneCharge(d));
            #endregion
        }

        /// <summary>
        /// If droneCharge with the DroneId exist
        /// </summary>
        /// <param name="requestedId">Looking for droneCharge with this DroneId</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean IsDroneChargeById(int droneId)
        {
            IEnumerable<DO.DroneCharge> dronesChargeLits = XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            if (dronesChargeLits.Any(d => d.DroneId == droneId))
                return true;

            return false;
        }
    }
}
