using System;
using System.Collections.Generic;
using System.Text;
using BO;


namespace BL
{
    sealed partial class BL : BlApi.Ibl
    {

        /// <summary>
        /// Drones List. Implemented by BL ctor.
        /// </summary>
        private List<Drone> dronesList { get; set; }

    }
}