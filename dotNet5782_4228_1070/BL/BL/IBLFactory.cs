using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;

namespace BlApi.IBL
{
    public static class IBLFactory
    {
        public static Ibl Factory()
        {
            return BL.BL.GetInstance;
        }
    }
}


namespace BlApi.Isimulation
{
    public static class SimFactory
    {
        public static ISimulation GetSimulation()
        {
            return BL.BL.GetInstance;
        }
    }
}
