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
        public class Station
        {
            IDal.DO.Station station;
            public Station (int id, string Name, int ChargeSlots, double Longitude, double Latitude)
            {
                station = new IDal.DO.Station();
                station.Id = id;
                station.Name = Name;
                station.ChargeSlots = ChargeSlots;
                station.Longitude = Longitude;
                station.Latitude = Latitude;
            }
            public override string ToString()
            {
                return ($"station name: {station.Name}, station Id: {station.Id}, station latitude: {station.Latitude}, station longitude: {station.Longitude}\n");
            }
                //this.Id = id;
                //this.Name = Name;
                //this.ChargeSlots = ChargeSlots;
                //this.Longitude = Longitude;
                //this.Latitude = Latitude;
            //public int Id { get; set; }
            //public string Name { set; get; }
            //public int ChargeSlots { get; set; }
            //public double Longitude { get; set; }
            //public double Latitude { get; set; }
        }
    }
}
