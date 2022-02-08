using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL 
    {
        public Action<Parcel> ParcelChangeAction { get; set; }

        /// <summary>
        /// Add a new parcel. checks if this parcel exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// Didn't sent an object because most of the props are initialize in the function and not by the workers/customer(client).
        /// </summary>
        /// <param name="senderId">Parcels' sender(customer) id</param>
        /// <param name="targetId">Parcels' target(customer) id</param>
        /// <param name="weight">Parcels' weight</param>
        /// <param name="priority">Parcels' priority</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcelToAdd)
        {
            if (parcelToAdd.Sender.Id != parcelToAdd.Target.Id)
            {
                lock (dal)
                {
                    DO.Parcel p = new DO.Parcel()
                    {
                        Id = dal.amountParcels() + 1,
                        SenderId = parcelToAdd.Sender.Id,
                        TargetId = parcelToAdd.Target.Id,
                        Weight = (DO.WeightCategories)parcelToAdd.Weight,
                        Priority = (DO.Priorities)parcelToAdd.Priority,
                        Requeasted = DateTime.Now
                    };

                    dal.AddParcel(p);
                    ParcelChangeAction(convertDalToBLParcel(p));
                }
            }
            else
            {
                throw new Exceptions.ObjNotAvailableException("Sender and targe customer are equal.\nplease change target or sender customer.");
                //throw new ObjNotExistException($"sender customer {parcelToAdd.Sender.Id} or terget customer {parcelToAdd.Target.Id} not exsist");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelToList()//////
        {
            lock (dal)
            {
                IEnumerable<DO.Parcel> parcelsList = dal.GetParcels();
                return (from parcel in parcelsList
                        select convertParcelToParcelToList(parcel));

                //IEnumerable<BO.Parcel> parcels = (from parcel in parcelsList
                //                                  select convertDalToBLParcel(parcel));
                //return convertBLParcelToBLParcelsToList(parcels);
            }
        }

        /// <summary>
        /// Returns a IEnumerable<Parcels> by recieving parcels from dal and converting them to BO.Parcel.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> getParcels()
        {
            lock (dal)
            {
                IEnumerable<DO.Parcel> parcels = dal.GetParcels();
                return (from parcel in parcels
                        select convertDalToBLParcel(parcel));
            }
        }

        ////private static ParcelStatuses findParcelStatus(ParcelStatuses p)
        ////{
        ////    if (p.Delivered != null)
        ////        return ParcelStatuses.Delivered;
        ////    else if (p.PickUp != null)
        ////        return ParcelStatuses.PickedUp;

        ////    else if (p.Scheduled != null)
        ////        return ParcelStatuses.Scheduled;
        ////    else //if (p.Requeasted != null)
        ////        return ParcelStatuses.Requeasted;
        ////}

        /// <summary>
        /// Receive weight, status and priority and returns List<ParcelToList> accurding to the conditions 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> DisplayParcelToListByFilters(int weight, int status, int priority)
        {
            #region Objects and variables declaration & implementation
            DO.ParcelStatuses parcelStatuses = (DO.ParcelStatuses)status;
            DO.WeightCategories weightCategories = (DO.WeightCategories)weight;
            DO.Priorities parcelPriority = (DO.Priorities)priority;
            #endregion

            lock (dal)
            {
                IEnumerable<DO.Parcel> parcelsList = new List<DO.Parcel>();

                if (weight >= 0 && status >= 0 && priority >= 0)
                    parcelsList = dal.getParcelWithSpecificCondition(p => p.Weight == weightCategories && dal.findParcelStatus(p) == parcelStatuses && p.Priority == parcelPriority);
                else if (weight >= 0 && status >= 0 && priority == -1)
                    parcelsList = dal.getParcelWithSpecificCondition(p => p.Weight == weightCategories && dal.findParcelStatus(p) == parcelStatuses);
                else if (weight >= 0 && status == -1 && priority >= 0)
                    parcelsList = dal.getParcelWithSpecificCondition(p => p.Weight == weightCategories && p.Priority == parcelPriority);
                else if (weight >= 0 && status == -1 && priority == -1)
                    parcelsList = dal.getParcelWithSpecificCondition(p => p.Weight == weightCategories);
                else if (weight == -1 && status >= 0 && priority >= 0)
                    parcelsList = dal.getParcelWithSpecificCondition(p => dal.findParcelStatus(p) == parcelStatuses && p.Priority == parcelPriority);
                else if (weight == -1 && status >= 0 && priority == -1)
                    parcelsList = dal.getParcelWithSpecificCondition(p => dal.findParcelStatus(p) == parcelStatuses);
                else if (weight == -1 && status == -1 && priority >= 0)
                    parcelsList = dal.getParcelWithSpecificCondition(p => p.Priority == parcelPriority);
                else parcelsList = dal.GetParcels();

                return from parcel in parcelsList
                       select convertParcelToParcelToList(convertDalToBLParcel(parcel));

                #region to delete
                ///////////////the good????
                //List<BO.Parcel> list = new List<Parcel>();
                //IEnumerable<Parcel> IList;
                //if (weight >= 0 && status >= 0 && priority >= 0)
                //    IList = dal.getParcelWithSpecificCondition(
                //        p => p.Weight == (DO.WeightCategories)weight
                //        && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status
                //        && p.Priority == (DO.Priorities)priority);
                //else if (weight >= 0 && status >= 0 && priority == -1)
                //    IList = dal.getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status);
                //else if (weight >= 0 && status == -1 && priority >= 0)
                //    IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight && p.Priority == (DO.Priorities)priority);
                //else if (weight >= 0 && status == -1 && priority == -1)
                //    IList = getParcelWithSpecificCondition(p => p.Weight == (DO.WeightCategories)weight);
                //else if (weight == -1 && status >= 0 && priority >= 0)
                //    IList = getParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status && p.Priority == (DO.Priorities)priority);
                //else if (weight == -1 && status >= 0 && priority == -1)
                //    IList = getParcelWithSpecificCondition(p => findParcelStatus(convertBLToDalParcel(p)) == (ParcelStatuses)status);
                //else if (weight == -1 && status == -1 && priority >= 0)
                //    IList = getParcelWithSpecificCondition(p => p.Priority == (DO.Priorities)priority);
                //else IList = getParcels();
                ////foreach (var i in IList)
                ////{
                ////    list.Add(i);
                ////}
                ////return convertBLParcelToBLParcelsToList(list);
                //return from parcel in IList
                //       select convertParcelToParcelToList(parcel);
                ////////////////////////////
                #endregion
            }
        }

        ///// <summary>
        ///// Return a BO.Parcel/s(converted) with a specific condition = predicate from parcels = getParcels
        ///// </summary>
        ///// <param name="predicate"></param>
        ///// <returns></returns>
        //private IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
        //{
        //    IEnumerable<Parcel> parcels = getParcels();
        //    return (from parcel in parcels
        //            where predicate(parcel)
        //            select parcel);
        //}


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel getParcelByDrone(int droneId)
        {
            try
            {
                lock (dal)
                {
                    DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();

                    if (parcel.Equals(null))//????????????????
                        throw new Exceptions.ObjNotExistException(typeof(ParcelInTransfer), -1);//

                    return convertDalToBLParcel(parcel);
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(ParcelInTransfer), -1, e);
            }
        }

        ///// <summary>
        ///// Check a predicate to dal and check if drone is schedualed to Parcel.
        ///// </summary>
        ///// <param name="droneId"></param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //public bool checkIfExistParcelByDrone(int droneId)
        //{
        //    try
        //    {
        //        lock (dal)
        //        {
        //            {
        //                DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
        //                if (parcel.Equals(null))
        //                    return false;
        //                return true;
        //            }
        //        }
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Return a BO.Parcel(converted) that the func receives it by an id from dal.getParcelWithSpecificCondition
        /// </summary>
        /// <param name="parcelRequestedId">The id of the parcel that's requested</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcelById(int parcelRequestedId)
        {
            lock (dal)
            {
                DO.Parcel p = dal.getParcelWithSpecificCondition(p => p.Id == parcelRequestedId).First();
                Parcel BLparcel = convertDalToBLParcel(p);
                return BLparcel;
            }
        }

        /// <summary>
        /// Checks if the parcel to remove exist.
        /// If exist send to remove
        /// else throw an error
        /// </summary>
        /// <param name="parcel">The parcel </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int parcelId)
        {
            lock (dal)
            {
                Parcel parcel = GetParcelById(parcelId);
                if (parcel.Drone == null)
                {
                    try
                    {
                        dal.removeParcel(dal.getParcelWithSpecificCondition(p => p.Id == parcel.Id).First());
                        ParcelChangeAction(parcel);
                    }
                    catch (Exceptions.ObjNotExistException e) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id, e); }
                    catch (Exception) { throw new Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id); }
                }
                else throw new Exceptions.ObjNotAvailableException("Can't remove parcel. Parcel asign to drone.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DronePicksUpParcel(int droneId)// ParcelStatuses.PickedUp          
        {
            try
            {
                lock (dronesList)
                {
                    lock (dal)
                    {
                        Drone drone = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId && d.Status == DroneStatus.Delivery).First();
                        DO.Parcel parcel = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                        if (parcel.PickUp != null /*!parcel.PickUp.Equals(default(DO.Parcel).PickUp)*/)
                        {
                            throw new Exception("The parcel is collected already");
                        }
                        if (parcel.Scheduled == null /*parcel.Scheduled.Equals(default(DO.Parcel).Scheduled)*/)
                        {
                            throw new Exception("The parcel is not schedueld.");
                        }
                        DO.Customer senderP;
                        try
                        {
                            senderP = dal.getCustomerWithSpecificCondition(customer => customer.Id == parcel.SenderId).First();
                        }
                        catch (ObjNotExistException)
                        {
                            throw new Exception("Drone wasn't abale to pick up the parcel");
                        }
                        Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        double disDroneToSenderP = distance(drone.DronePosition, senderPosition);
                        if (disDroneToSenderP > drone.Battery)///not sure if we need it here. it was supposed to be checked in the pair a parcel with drone.
                            throw new ObjNotAvailableException("The battery usage will be bigger than the drones' battery ");
                        drone.Battery -= Math.Round(disDroneToSenderP * requestElectricity((int)parcel.Weight), 1);
                        drone.DronePosition = senderPosition;

                        updateBLDrone(drone);
                        parcel.PickUp = DateTime.Now;
                        dal.changeParcelInfo(parcel);
                        drone.ParcelInTransfer.isWaiting = false; //////////////////////
                        //ParcelChangeAction(convertDalToBLParcel(parcel));
                        DroneChangeAction(drone);
                        return drone;
                    }
                }
            }

            catch (Exception e)
            {
                throw new ObjNotExistException(e.Message);
            }
        }

        public void changeParcelInfo(Parcel parcel)
        {
            dal.changeParcelInfo(convertBLToDalParcel(parcel));
            //ParcelChangeAction(parcel);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone DeliveryParcelByDrone(int droneId) //ParcelStatuses.Delivered.
        {
            try
            {
                lock (dronesList)
                {
                    lock (dal)
                    {
                        Drone bLDroneToSuplly = getDroneWithSpecificConditionFromDronesList(d => d.Id == droneId).First();
                        DO.Parcel parcelToDelivery = dal.getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
                        if (parcelToDelivery.PickUp.Equals(default(DO.Parcel).PickUp) && !parcelToDelivery.Delivered.Equals(default(DO.Parcel).Delivered))
                        {
                            throw new Exception("Drone cann't deliver this parcel.");
                        }
                        DO.Customer senderP;
                        DO.Customer targetP;
                        senderP = dal.getCustomerWithSpecificCondition(c => c.Id == parcelToDelivery.SenderId).First();
                        targetP = dal.getCustomerWithSpecificCondition(c => c.Id == parcelToDelivery.TargetId).First();
                        Position senderPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        Position targetPosition = new Position() { Longitude = senderP.Longitude, Latitude = senderP.Latitude };
                        double disSenderToTarget = distance(senderPosition, targetPosition);
                        double electricity = requestElectricity((int)parcelToDelivery.Weight);
                        bLDroneToSuplly.Battery -= Math.Round(electricity * disSenderToTarget, 1);
                        bLDroneToSuplly.DronePosition = targetPosition;
                        bLDroneToSuplly.Status = DroneStatus.Available;
                        updateBLDrone(bLDroneToSuplly);
                        parcelToDelivery.Delivered = DateTime.Now;
                        dal.changeParcelInfo(parcelToDelivery);
                        //ParcelChangeAction(convertDalToBLParcel(parcelToDelivery));
                        DroneChangeAction(bLDroneToSuplly);
                        return bLDroneToSuplly;
                    }
                }
            }
            catch (ObjNotExistException)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
            catch (Exception)
            {
                throw new Exception("Can't deliver parcelby drone.");
            }
        }

        //private static ParcelStatuses findParcelStatus(DO.Parcel p)
        //{
        //    if (p.Delivered != null)
        //        return ParcelStatuses.Delivered;
        //    else if (p.PickUp != null)
        //        return ParcelStatuses.PickedUp;

        //    else if (p.Scheduled != null)
        //        return ParcelStatuses.Scheduled;
        //    else //if (p.Requeasted != null)
        //        return ParcelStatuses.Requeasted;
        //}


        /// <summary>
        /// Find parcels' status: { Available, AsignedParcel, PickedParcel , DeliveredParcel}
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static ParcelStatuses findParcelStatus(Parcel p)
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
