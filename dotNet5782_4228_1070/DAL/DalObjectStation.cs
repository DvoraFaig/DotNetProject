using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalObject
{
    public partial class DalObject //: IDal.IDal
    {
        public static int amountStations()
        {
            return DataSource.Stations.Count();
        }
        public void AddStation(Station s)
        {
            DataSource.Stations.Add(s);
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
        public Station getStationById(int id)
        {
            return DataSource.Stations.FirstOrDefault(station => station.Id == id);
        }
    }
}
