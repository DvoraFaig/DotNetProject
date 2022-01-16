using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Send a predicate to receive a BO.Drone from BL.DroneList. 
        /// </summary>
        /// <param name="droneRequestedId">The drone with this id</param>
        /// <returns></returns>
        private Drone getDroneByIdFromDronesList(int droneRequestedId)
        {
            try
            {
                return getDroneWithSpecificConditionFromDronesList(d => d.Id == droneRequestedId).First();
            }
            #region Exceptions
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(Drone), droneRequestedId, e);
            }
            #endregion
        }

        /// <summary>
        /// Return a drone from droneList from BL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private IEnumerable<Drone> getDroneWithSpecificConditionFromDronesList(Predicate<Drone> predicate)
        {
            return (from drone in dronesList
                    where predicate(drone)
                    select drone);
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
