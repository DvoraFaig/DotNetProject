using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        public void removeStation(Station stationToRemove)
        {
            try
            {
                Station station = (from s in DataSource.Stations
                                   where s.Id == stationToRemove.Id
                                   && s.Name == stationToRemove.Name
                                   && s.ChargeSlots == stationToRemove.ChargeSlots
                                   && s.Latitude == stationToRemove.Latitude
                                   && s.Longitude == stationToRemove.Longitude
                                   select s).First();
                station.IsActive = false;
            }
            catch(Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Station), stationToRemove.Id, e1);
            }
        }

        public void removeCustomer(Customer customerToRemove)
        {
            Customer customer = (from c in DataSource.Customers
                                where c.Id == customerToRemove.Id
                               && c.Name == customerToRemove.Name
                               && c.Phone == customerToRemove.Phone
                               && c.Latitude == customerToRemove.Latitude
                               && c.Longitude == customerToRemove.Longitude
                               select c).First();
            customer.IsActive = false;
        }
    }
}
