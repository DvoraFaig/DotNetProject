using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {

        public void RemoveStation(Station station)
        {
            try
            {
                if (dal.IsStationActive(station.Id))
                    dal.removeStation(convertBLToDalStation(station));
                else
                    throw new Exceptions.ObjExistException(typeof(Station), station.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
        }
        public void RemoveCustomer(Customer customer)
        {
            try
            {
                if (dal.IsCustomerActive(customer.Id))
                    dal.removeCustomer(convertBLToDalCustomer(customer));
                else
                    throw new Exceptions.ObjExistException(typeof(Customer), customer.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
        }

        public void RemoveDrone(Drone drone)
        {
            try
            {
                if (dal.IsDroneById(drone.Id))
                    dal.removeDrone(convertBLToDalDrone(drone));
                else
                    throw new Exceptions.ObjExistException(typeof(Drone), drone.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
        }
    }
}
