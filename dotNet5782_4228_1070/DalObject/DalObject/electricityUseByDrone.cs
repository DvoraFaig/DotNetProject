using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Returns electricity usage of drone.
        /// </summary>
        /// <returns></returns>
        public double[] electricityUseByDrone()
        {
            double[] arr = {
                DataSource.Config.empty,
                DataSource.Config.lightWeight,
                DataSource.Config.mediumWeight,
                DataSource.Config.heavyWeight,
                DataSource.Config.chargingRate };
            return arr;
        }
    }
}
