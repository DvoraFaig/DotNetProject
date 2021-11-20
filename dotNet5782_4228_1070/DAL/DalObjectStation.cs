using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public int amountStations()
        {
            return DataSource.Stations.Count();
        }
        public void AddStation(Station s)
        {
            DataSource.Stations.Add(s);
        }
        public void changeStationInfo(Station s)
        {
            Station sToChange = getStationById(s.Id);
            sToChange = s;
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
            try
            {
                return DataSource.Stations.FirstOrDefault(station => station.Id == id);
            }
            catch (Exception e)
            {
                throw new DalExceptions.ObjNotExistException(typeof(Station), id);
            }
        }
    }
}
