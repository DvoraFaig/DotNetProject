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
        /// Add the new DroneCharge to DroneCharges.
        /// </summary>
        /// <param name="newDroneCharge">DroneCharge to add.</param>
        public void AddDroneToCharge(DroneCharge newDroneCharge)
        {
            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            List<DroneCharge> newList = new List<DroneCharge>();
            if (droneChargesList.Count() != 0)
            {
                newList = droneChargesList.Cast<DO.DroneCharge>().ToList();
            }
            newList.Add(newDroneCharge);
            DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(newList, dir + droneChargeFilePath);

            #region A better way but DroneCharge will be the one object who use LoadListFromXMLSerializer&SaveListToXMLSerializer funcs
            //XElement droneChargeRoot = DL.XMLTools.LoadData(dir + droneChargeFilePath);
            //droneChargeRoot.Add(returnDroneChargeXElement(newDroneCharge));
            //droneChargeRoot.Save(dir + droneChargeFilePath);
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
        /// Remove charging drone by drone id.
        /// <param name="droneId">The charging drone with droneId - to remove</param>
        public void removeDroneChargeByDroneId(int droneId)
        {
            try
            {
                List<DO.DroneCharge> dronesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath).ToList();
                int index = dronesList.FindIndex(d => d.DroneId == droneId);
                dronesList.RemoveAt(index);
                DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(dronesList, dir + droneChargeFilePath);
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
            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            return from item in droneChargesList
                   orderby item.StationId // or orderby item.DroneId
                   select item;
        }
    }
}
