﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    public partial class DalObject : DalApi.IDal
    {
        /// <summary>
        /// Add the new DroneCharge to DroneCharges.
        /// </summary>
        /// <param name="newDroneCharge">DroneCharge to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneToCharge(DroneCharge newDroneCharge)
        {
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        /// <summary>
        /// Get a DroneCharge/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone charge /s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate)
        {
            return (from DroneCharge in DataSource.DroneCharges
                    where predicate(DroneCharge)
                    select DroneCharge);
        }

        /// <summary>
        /// Remove charging drone by drone id.
        /// <param name="droneId">The charging drone with droneId</param>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void removeDroneChargeByDroneId(int droneId)
        {
            int index = DataSource.DroneCharges.FindIndex(d => d.DroneId == droneId);
            DataSource.DroneCharges.RemoveAt(index);
            //try
            //{
            //slice??
            //   DroneCharge droneCharge = (from d in DataSource.DroneCharges
            //                   where d.DroneId == droneId
            //                   select d).First();
            //    DataSource.DroneCharges.Remove(droneCharge);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exceptions.NoMatchingData(typeof(Drone), droneId, e1);
            //}
        }
    }
}

///// <summary>
///// If droneCharge with the DroneId exist
///// </summary>
///// <param name="requestedId">Looking for droneCharge with this DroneId</param>
///// <returns></returns>
//[MethodImpl(MethodImplOptions.Synchronized)]
//public Boolean IsDroneChargeById(int droneId)
//{
//    return DataSource.DroneCharges.Any(d => d.DroneId == droneId);
//}
///// <summary>
///// Get all droneCharge.
///// </summary>
///// <returns></returns>
//[MethodImpl(MethodImplOptions.Synchronized)]
//public IEnumerable<DroneCharge> GetDroneCharges()
//{
//    return from d in DataSource.DroneCharges select d;
//}
