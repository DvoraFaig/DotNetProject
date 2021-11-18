//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using IBL.BO;

//namespace DalObject
//{
//    public partial class DalObject 
//    {
//        //Station//
//        public static int amountStations()
//        {
//            return DataSource.Stations.Count();
//        }
//        public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude)
//        {
//            Station s = new Station(id, Name, ChargeSlots, Latitude, Longitude);
//            DataSource.Stations.Add(s);
//        }
//        public IEnumerable<Station> displayStations()
//        {
//            foreach (Station station in DataSource.Stations)
//            {
//                if (station.Id != 0)
//                {
//                    yield return station;
//                }
//            }
//        }
//        public static Station getStationById(int id)
//        {
//            return DataSource.Stations.FirstOrDefault(station => station.Id == id);
//        }


//    }
//}
