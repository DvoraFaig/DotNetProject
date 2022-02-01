﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Statrt the sumulation.
        /// </summary>
        /// <param name="drone">The Drone</param>
        /// <param name="updateDrone">Func to update info in PL</param>
        /// <param name="needToStop">Func to use to stop simulation</param>
        public void StartSimulation(Drone drone, Action<Drone, DroneStatusInSim, double> updateDrone, Func<bool> needToStop)
        {
            var sim = new Simulation(this , dal);
            sim.StartSim(drone, updateDrone, needToStop);
        }
    }
}
