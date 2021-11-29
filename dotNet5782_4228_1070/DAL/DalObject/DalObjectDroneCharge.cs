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
        public int amountDroneCharges()
        {
            return DataSource.DroneCharges.Count();
        }
        public void changeDroneChargeInfo(DroneCharge d)
        {
           
        }
        public void AddDroneCharge(DroneCharge parcel)
        {
            DataSource.DroneCharges.Add(parcel);
        }
        public IEnumerable<DroneCharge> displayDroneCharge()
        {
            foreach (DroneCharge droneCharge in DataSource.DroneCharges)
            {
                if (droneCharge.StationId != 0 && droneCharge.DroneId == 0)
                {
                    yield return droneCharge;
                }
            }
        }
        public DroneCharge getDroneChargeByDroneId(int id)
        {
            return DataSource.DroneCharges.FirstOrDefault(charge => charge.DroneId == id);
        }
        public DroneCharge getDroneChargeByStationId(int id)
        {
            try
            {
                return DataSource.DroneCharges.Find(charge => charge.StationId == id);
            }
            catch (Exception)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(DroneCharge), id);
            }
        }
    }
}
