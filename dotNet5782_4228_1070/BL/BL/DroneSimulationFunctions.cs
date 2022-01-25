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
    public sealed partial class BL : BlApi.Ibl
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartSimulation(Drone drone, Action<Drone, int> updateDrone, Func<bool> needToStop)
        {
            var sim = new Simulation(this);
            sim.Start(drone, updateDrone, needToStop);
        }
    }
}
