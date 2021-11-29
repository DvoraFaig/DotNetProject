using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static IBL.BO.Exceptions;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public sealed partial class BL : IBL.Ibl
    {
        static BL instance;
        public static BL GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new BL();
                return instance;
            }
        }
        
        private static double distance(BLPosition p1, BLPosition p2)
        {
            double d = Math.Pow((Math.Pow(p1.Longitude - p2.Longitude, 2) + Math.Pow(p1.Latitude - p2.Latitude, 2)), 0.5);
            return d;
        }
    }
}
