using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }

        //public void DroneIfNeedToCharge(Drone drone)
        //{
        //    if(drone.Battery < 50)
        //    {
        //        findClosestStation(drone);
        //        //findEmptyDroneCharge()
        //    }
        //}
        //public void findClosestStation(Drone drone)
        //{
        //    //find customer's place
        //    //find parcel and the parcel has the customer's id
        //    Parcel parcel = Array.Find((Parcel p) => p.DroneId == drone.Id);
        //    Customer customer = Array.Find((Customer c) => c.ID == parcel.TargetId);
        //    calcStationsDis(c);
        //}
        //public int[] calcStationsDis(Customer c)
        //{
        //    int dis[/*indexStation*/5][2];
        //    int i = 0;
        //    foreach (Station s in StationArray)
        //    {
        //        dis[i] = haversine(c.Latitude, s.Latitude, c.Longitude, s.Longitude);
        //        i++
        //    }
        //    //מיון dis ולמצוא את הקטן אם לא פנוי ללכת להבא
        //}
        
        //static double haversine(double lat1, double lon1,double lat2, double lon2)
        //{
        //    // distance between latitudes and longitudes
        //    double dLat = (Math.PI / 180) * (lat2 - lat1);
        //    double dLon = (Math.PI / 180) * (lon2 - lon1);

        //    // convert to radians
        //    lat1 = (Math.PI / 180) * (lat1);
        //    lat2 = (Math.PI / 180) * (lat2);

        //    // apply formulae
        //    double a = Math.Pow(Math.Sin(dLat / 2), 2) +
        //               Math.Pow(Math.Sin(dLon / 2), 2) *
        //               Math.Cos(lat1) * Math.Cos(lat2);
        //    double rad = 6371;
        //    double c = 2 * Math.Asin(Math.Sqrt(a));
        //    return rad * c;
        //}
    }
}
