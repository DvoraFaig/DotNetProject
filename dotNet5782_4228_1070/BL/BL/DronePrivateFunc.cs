using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    sealed partial class BL
    {
        

        /// <summary>
        /// Return a drone from droneList from BL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private IEnumerable<Drone> getDroneWithSpecificConditionFromDronesList(Predicate<Drone> predicate)
        {
            IEnumerable<Drone> drones = (from drone in dronesList
                                         where predicate(drone)
                                         select drone);
            return from d in drones
                   select d.Clone<Drone>();
        }

        /// <summary>
        /// Update drones detailes.
        /// </summary>
        /// <param name="droneWithUpdateInfo"></param>
        private void updateBLDrone(Drone droneWithUpdateInfo)
        {
            try
            {
                int index = dronesList.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
                dronesList[index] = droneWithUpdateInfo;
            }
            #region Exceptions
            catch (Exception)
            {
                throw new InvalidOperationException();
            }
            #endregion
        }
    }
}

///// <summary>
///// Add a drone to dronesList by Index
///// </summary>
///// <param name="dr"></param>
//private void AddDroneByIndex(Drone dr)
//{

//    ///////////////////////////
//    // sometimes drone.id != drone.index
//    /////////////////
//    if (dr.Id < dronesList.Count)
//    {
//        dronesList.Insert(dr.Id - 1, dr);
//        return;
//    }
//    dronesList.Add(dr);
//}
