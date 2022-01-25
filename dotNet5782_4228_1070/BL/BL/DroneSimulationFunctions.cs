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
        public void StartSimulation(Drone std, Action<Drone, int> updateStudent, Func<bool> needToStop)
        {
            var sim = new Simulation(this);
            sim.start(std, updateStudent, needToStop);

        }
    }
}
