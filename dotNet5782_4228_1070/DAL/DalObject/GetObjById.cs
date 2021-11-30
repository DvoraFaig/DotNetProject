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
        public Station getStationById(int id)
        {
            try
            {
                return getStationWithSpecificCondition(s => s.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Station), id);
            }
        }
        public Drone getDroneById(int id)
        {
            try
            {
                return getDroneWithSpecificCondition(drone => drone.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Drone), id);
            }
        }
        public Parcel getParcelById(int id)
        {
            try
            {
                return getParcelWithSpecificCondition(parcel => parcel.Id == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Parcel), id);
            }
        }
        public Parcel getParcelByDroneId(int droneId)
        {
            try
            {
                return getParcelWithSpecificCondition(p => p.DroneId == droneId).First();
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Parcel), droneId);
            }
        }
        public Customer getCustomerById(int id)
        {
            try
            {
                return getCustomerWithSpecificCondition(customer => customer.ID == id).First();
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Customer), id);
            }
        }
        public DroneCharge getDroneChargeByDroneId(int id)
        {
            try
            {
                return getDroneChargeWithSpecificCondition(charge => charge.DroneId == id).First();
            }
            catch (InvalidOperationException)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(DroneCharge), id);
            }

        }
        public DroneCharge getDroneChargeByStationId(int id)
        {
            try
            {
                return getDroneChargeWithSpecificCondition(charge => charge.StationId == id).First();
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(DroneCharge), id);
            }
        }

    }
}
