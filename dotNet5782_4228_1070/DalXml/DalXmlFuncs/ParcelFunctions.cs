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
        /// Add the new parcel to parcels.
        /// </summary>
        /// <param name="newParcel">parcel to add</param>
        public void AddParcel(Parcel newParcel)
        {
            List<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
            parcelsList.Add(newParcel);
            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
        }
        /// <summary>
        /// Receive a DO parcel and return a XElemnt parcel - copy information.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        private XElement returnParcelXElement(DO.Parcel newParcel)
        {
            XElement Id = new XElement("Id", newParcel.Id);
            XElement SenderId = new XElement("SenderId", newParcel.SenderId);
            XElement TargetId = new XElement("TargetId", newParcel.TargetId);
            XElement Weight = new XElement("Weight", newParcel.Weight);
            XElement Priority = new XElement("Priority", newParcel.Priority);
            XElement DroneId = new XElement("DroneId", newParcel.DroneId);
            XElement Requeasted = new XElement("Requeasted", newParcel.Requeasted);
            XElement Scheduled = new XElement("Scheduled", newParcel.Scheduled);
            XElement PickUp = new XElement("PickUp", newParcel.PickUp);
            XElement Delivered = new XElement("Delivered", newParcel.Delivered);
            XElement IsActive = new XElement("IsActive", true);
            return new XElement("Parcel", Id, SenderId, TargetId, Weight, Priority, DroneId, Requeasted, Scheduled, PickUp, Delivered, IsActive);
        }

        /// <summary>
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcelToRemove">remove current parcel</param>
        public void removeParcel(Parcel parcelToRemove)
        {
            XElement parcelRoot = DL.XMLTools.LoadData(dir + parcelFilePath);
            XElement parcelXElemnt;
            parcelXElemnt = (from p in parcelRoot.Elements()
                             where Convert.ToInt32(p.Element("Id").Value) == parcelToRemove.Id
                             select p).FirstOrDefault();
            if (parcelXElemnt != null)
            {
                parcelXElemnt.Element("IsActive").Value = "false";
            }
            #region found better way
            //IEnumerable<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            //try
            //{
            //    Parcel parcel = getParcelWithSpecificCondition(s => s.Id == parcelToRemove.Id).First();
            //    if (parcel.IsActive)
            //        parcel.IsActive = false;
            //    changeParcelInfo(parcel);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exceptions.NoMatchingData(typeof(Parcel), parcelToRemove.Id, e1);
            //}
            #endregion
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels()
        {
            IEnumerable<DO.Parcel> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            return from item in parceslList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        ///  Change specific parcel info
        /// </summary>
        /// <param name="parcelWithUpdateInfo">The parcel with the changed info</param>
        public void changeParcelInfo(Parcel parcelWithUpdateInfo)
        {
            List<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
            int index = parcelsList.FindIndex(t => t.Id == parcelWithUpdateInfo.Id);
            if (index == -1)
                throw new DO.Exceptions.ObjNotExistException(typeof(Parcel), parcelWithUpdateInfo.Id);

            parcelsList[index] = parcelWithUpdateInfo;
            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
        }

        public ParcelStatuses findParcelStatus(DO.Parcel p)
        {
            if (p.Delivered != null)
                return ParcelStatuses.Delivered;
            else if (p.PickUp != null)
                return ParcelStatuses.PickedUp;

            else if (p.Scheduled != null)
                return ParcelStatuses.Scheduled;
            else //if (p.Requeasted != null)
                return ParcelStatuses.Requeasted;
        }
    }
}
