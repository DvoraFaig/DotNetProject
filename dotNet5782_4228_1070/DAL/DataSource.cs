using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DALObject
{
    internal class DataSource
    {
        internal Drone[] Drones = new Drone[10];
        internal Station[] Stations = new Station[5];
        internal Customer[] Customers = new Customer[100];
        internal Parcel[] Parcels = new Parcel[1000];

        /*מערכים סטטים של ישויות הנתונים*/
        internal class Config
        {
            public static int indexDrones = 0;
            public static int indexStations = 0;
            public static int indexCustomers = 0;
            public static int indexParcels = 0;
            /*להוסיף שדה עבור יצירה של מזהה רץ עבור חבילות*/
        }

    }


}
