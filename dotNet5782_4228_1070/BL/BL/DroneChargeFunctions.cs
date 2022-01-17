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
        /// Remove specific drone
        /// </summary>
        /// <param name="droneId">remove current drone with droneId</param>
        public void RemoveDroneCharge(int droneId)
        {
            try
            {
                if (dal.IsDroneChargeById(droneId))
                    dal.removeDroneChargeByDroneId(droneId);
                #region Exceptions
                else
                    throw new Exceptions.ObjNotExistException(typeof(ChargingDrone), droneId);
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
            #endregion
        }
    }
}
