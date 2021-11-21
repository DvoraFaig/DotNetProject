using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;




namespace DalExceptions
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
        public void changeStationInfo(Station goodStation)
        {
            Station sToErase = getStationById(goodStation.Id);
            DataSource.Stations.Remove(sToErase);
            DataSource.Stations.Add(goodStation);
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
                return DataSource.Stations.First(station => station.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Station), id);
            }
        }
    }
}
