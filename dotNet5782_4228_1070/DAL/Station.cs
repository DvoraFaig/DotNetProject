using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class Station
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public int ChargeStop { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
