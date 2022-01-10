using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalApi;
using DO;



namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Return how much stations there is.
        /// </summary>
        /// <returns></returns>
        public int amountStations()
        {
            return DataSource.Stations.Count();
        }
        
        /// <summary>
        /// Return how much parcels there is.
        /// </summary>
        /// <returns></returns>
        public int amountParcels()
        {
            return DataSource.Parcels.Count();
        }
    }
}
