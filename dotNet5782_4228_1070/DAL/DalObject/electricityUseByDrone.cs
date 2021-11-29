﻿using System;
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
        public double[] electricityUseByDrone(Drone drone)
        {
            double[] droneInfo = new double[5];
            droneInfo[0] = DataSource.Config.empty;
            droneInfo[1] = DataSource.Config.lightWeight;
            droneInfo[2] = DataSource.Config.mediumWeight;
            droneInfo[3] = DataSource.Config.heavyWeight;
            droneInfo[4] = DataSource.Config.chargingRate;
            return droneInfo;
        }

    }
}