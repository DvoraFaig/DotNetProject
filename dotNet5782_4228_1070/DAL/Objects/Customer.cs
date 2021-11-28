using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    namespace DO
    {
        public struct Customer
        {
            public int ID { get; set; }
            //{
            //    get { return ID; }
            //    set
            //    {
            //        if (value >= 100000000 && value < 10000000000)
            //            ID = value;
            //        //else throw new Exception("Id not in the right lenght");
            //    }
            //}
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, customer latitude: {Latitude}, customer Longitude: {Longitude}\n");
            }
        }
    }
}



