using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Linq;
using static BO.Exceptions;

namespace BL
{
    sealed partial class BL 
    {
        public static double distance(Position p1, Position p2) //not private becuase of simulation
        {
            double d = Math.Abs(Math.Pow((Math.Pow(p1.Longitude - p2.Longitude, 2) + Math.Pow(p1.Latitude - p2.Latitude, 2)), 0.5));
            return d;
        }
    }
}
