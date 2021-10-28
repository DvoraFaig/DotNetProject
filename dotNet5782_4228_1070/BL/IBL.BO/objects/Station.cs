using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{ 
    namespace BO
    { 
        public class Station
        {
            public Station (int id, string Name, int ChargeSlots, double Longitude, double Latitude)
            {
                this.Id = id;
                this.Name = Name;
                this.ChargeSlots = ChargeSlots;
                this.Longitude = Longitude;
                this.Latitude = Latitude;
            }
	    
            public int Id { get; set; }
            public string Name { set; get; }
            public int ChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return ($"station name: {Name}, station Id: {Id}, station latitude: {Latitude}, station longitude: {Longitude}\n");
            }
        }
    }
}
