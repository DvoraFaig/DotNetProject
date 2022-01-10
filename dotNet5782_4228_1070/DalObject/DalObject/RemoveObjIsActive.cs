using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// If station exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="stationToRemove">The station to remove. stationToRemove.IsActive = false</param>
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
                changeStationInfo(station);
            }
            catch(Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Station), stationToRemove.Id, e1);
            }
        }

        /// <summary>
        /// if customer exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="customerToRemove">The customer to remove. customerToRemove.IsActive = false</param>
        public void removeCustomer(Customer customerToRemove)
        {
            try
            {
                Customer customer = (from c in DataSource.Customers
                                     where c.Id == customerToRemove.Id
                                    && c.Name == customerToRemove.Name
                                    && c.Phone == customerToRemove.Phone
                                    && c.Latitude == customerToRemove.Latitude
                                    && c.Longitude == customerToRemove.Longitude
                                     select c).First();
                customer.IsActive = false;
                changeCustomerInfo(customer);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Customer), customerToRemove.Id, e1);
            }
        }

        /// <summary>
        /// if drone exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.</summary>
        /// <param name="droneToRemove">The drone to remove. droneToRemove.IsActive = false</param>
        public void removeDrone(Drone droneToRemove)
        {
            try
            {
                Drone drone = (from d in DataSource.Drones
                                     where d.Id == droneToRemove.Id
                                    && d.Model == droneToRemove.Model
                                    && d.MaxWeight == droneToRemove.MaxWeight
                                     select d).First();
                drone.IsActive = false;
                changeDroneInfo(drone);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Drone), droneToRemove.Id, e1);
            }
        }
    }
}
