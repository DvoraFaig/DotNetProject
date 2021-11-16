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
        public static int amountDroneCharges()
        {
            return DataSource.DroneCharges.Count();
        }
        public void changeDroneChargeInfo(DroneCharge d)
        {
           
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
        public static DroneCharge getDroneChargeById(int id)
        {
            try
            {
                return DataSource.DroneCharges.Find(charge => charge.StationId == id);
            }
            catch (Exception e)
            {
                throw new DalExceptions.ObjNotExistException(typeof(DroneCharge), id);
            }
        }
    }
}
