﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject : IDal.IDal
    {
        //DroneCharge
        public static int amountDroneCharges()
        {
            return DataSource.DroneCharges.Count();
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
            return DataSource.DroneCharges.FirstOrDefault(charge => charge.StationId == id);
        }

    }
}