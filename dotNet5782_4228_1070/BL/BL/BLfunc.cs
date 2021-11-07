using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public sealed partial class BL : IBL.IBL
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
        public static double distance(float x1, float y1, float x2, float y2)
        {
            double d = Math.Pow((Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)), 0.5);
            return d;
        }
    }
}
