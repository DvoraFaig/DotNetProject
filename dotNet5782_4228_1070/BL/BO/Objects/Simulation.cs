using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DO;


namespace BO
{
    public class Simulation
    {
        Ibl BL;
        public Simulation(Ibl BL)
        {
            this.BL = BL;
        }

        public void start(Drone std, Action<Drone, int> updateStudent, Func<bool> needToStop)
        {
            int index = 0;
            while (!needToStop())
            {
                
            }
        }
    }
}

