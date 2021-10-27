using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject : IDal.IDal
    {
        public static int amountStations()
        {
            return DataSource.Stations.Count();
        }
        public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude)
        {
            Station station = new Station();
            station.Id = id;
            station.Name = Name;
            station.ChargeSlots = ChargeSlots;
            station.Longitude = Longitude;
            station.Latitude = Latitude;
            DataSource.Stations.Add(station);

        }
        public IEnumerable<Station> displayStations()
        {
            foreach (Station station in DataSource.Stations)
            {
                if (station.Id != 0)
                {
                    yield return station;
                }
            }
        }
        public static Station getStationById(int id)
        {
            return DataSource.Stations.FirstOrDefault(station => station.Id == id);
        }


    }
}
