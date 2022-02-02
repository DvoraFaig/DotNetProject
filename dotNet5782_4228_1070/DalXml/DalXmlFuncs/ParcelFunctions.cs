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
        /// Add the new parcel to parcels.
        /// </summary>
        /// <param name="newParcel">parcel to add</param>
        public void AddParcel(Parcel newParcel)
        {
            XElement ParcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            newParcel.IsActive = true;
            ParcelRoot.Add(returnParcelXElement(newParcel));
            ParcelRoot.Save(dir + parcelFilePath);

            //Parcel parcel;
            //try
            //{
            //    parcel = getParcelWithSpecificCondition(c => c.Id == newParcel.Id).First();
            //    if (parcel.IsActive)
            //        throw new Exceptions.ObjExistException(typeof(Parcel), newParcel.Id);

            //    changeParcelInfo(newParcel);
            //    throw new Exceptions.DataChanged(typeof(Parcel), newParcel.Id);
            //}
            //catch (Exception)
            //{
            //    XElement ParcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            //    ParcelRoot.Add(returnParcelXElement(newParcel));
            //    ParcelRoot.Save(dir + parcelFilePath);
            //}

            #region LoadListFromXMLSerializer 
            //Without the new chaeck if exist
            //List<DO.Parcel> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
            //newParcel.IsActive = true;
            //parcelsList.Add(newParcel);
            //XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
            #endregion
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

        private Parcel returnParcel(XElement station)
        {
            return new DO.Parcel()
            {
                Id = Convert.ToInt32(station.Element("Id").Value),
                SenderId = Convert.ToInt32(station.Element("SenderId").Value),
                TargetId = Convert.ToInt32(station.Element("TargetId").Value),
                Weight = (WeightCategories)Convert.ToInt32(station.Element("Weight").Value),
                Priority = (Priorities)Convert.ToInt32(station.Element("Priority").Value),
                DroneId = Convert.ToInt32(station.Element("DroneId").Value),
                Requeasted = Convert.ToDateTime(station.Element("Requeasted").Value),
                Scheduled = Convert.ToDateTime(station.Element("Scheduled").Value),
                PickUp = Convert.ToDateTime(station.Element("PickUp").Value),
                Delivered = Convert.ToDateTime(station.Element("Delivered").Value),
                IsActive = Convert.ToBoolean((station.Element("IsActive").Value))
            };
        }

        /// <summary>
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcelToRemove">remove current parcel</param>
        public void removeParcel(Parcel parcelToRemove)
        {
            XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            XElement parcelXElemnt = (from p in parcelRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == parcelToRemove.Id
                                      select p).FirstOrDefault();

            if (parcelXElemnt != null)
                parcelXElemnt.Element("IsActive").Value = "false";

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Parcel> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
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
            XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            return (from p in parcelRoot.Elements()
                    orderby Convert.ToInt32(p.Element("Id").Value)
                    select returnParcel(p));

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Parcel> parceslList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            //return from item in parceslList
            //       orderby item.Id
            //       select item;
            #endregion
        }

        /// <summary>
        ///  Change specific parcel info
        /// </summary>
        /// <param name="parcelWithUpdateInfo">The parcel with the changed info</param>
        public void changeParcelInfo(Parcel parcelWithUpdateInfo)
        {
            XElement ParcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            XElement parcelElement = (from p in ParcelRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == parcelWithUpdateInfo.Id
                                      select p).FirstOrDefault();

            //if (parcelElement == (default(XElement)))
            //    throw new Exceptions.ObjNotExistException(typeof(Parcel), parcelWithUpdateInfo.Id);

            XElement xElementUpdateParcel = returnParcelXElement(parcelWithUpdateInfo);
            parcelElement = xElementUpdateParcel;
            ParcelRoot.Save(dir + parcelFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Parcel> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
            //int index = parcelsList.FindIndex(t => t.Id == parcelWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Parcel), parcelWithUpdateInfo.Id);

            //parcelsList[index] = parcelWithUpdateInfo;
            //XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
            #endregion
        }

        /// <summary>
        /// Get a Parcel/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a parcel/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
        {
            XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            return (from p in parcelRoot.Elements()
                    where predicate(returnParcel(p))
                    select returnParcel(p));

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Parcel> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            //return (from parcel in parcelsList
            //        where predicate(parcel)
            //        select parcel);
            #endregion
        }

        /// <summary>
        /// If parcel with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for parcel with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsParcelById(int requestedId)
        {
            XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            XElement parcelXElemnt = (from p in parcelRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == requestedId
                                      select p).FirstOrDefault();

            if (parcelXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Parcel> parcelsLists = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            //if (parcelsLists.Any(p => p.Id == requestedId))
            //    return true;

            //return false;
            #endregion
        }

        /// <summary>
        /// If parcel with the requested id exist & active
        /// </summary>
        /// <param name="requestedId">Looking for parcel with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsParcelActive(int requestedId)
        {
            XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            XElement parcelXElemnt = (from p in parcelRoot.Elements()
                                      where Convert.ToInt32(p.Element("Id").Value) == requestedId
                                      && Convert.ToBoolean(p.Element("IsActive").Value)
                                      select p).FirstOrDefault();

            if (parcelXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Parcel> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            //if (parcelsList.Any(s => s.Id == requestedId && s.IsActive))
            //    return true;
            //return false;
            #endregion
        }

        /// <summary>
        /// Return how much parcels there is.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int amountParcels()
        {
            return XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).Count();

            //XElement parcelRoot = XMLTools.LoadData(dir + parcelFilePath);
            //return (from p in parcelRoot.Elements()
            //        select p).Count();
        }

        /// <summary>
        /// Findcparcel status occurding to it's DateTime
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
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
