using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;



namespace IBL
{
    namespace BO
    {
        public class Position
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public override string ToString()
            {
                return ($"({Latitude},{Longitude}).");
            }
        }
    }
}
