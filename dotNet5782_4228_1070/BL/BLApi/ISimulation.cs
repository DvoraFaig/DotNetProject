using System;
using System.Collections.Generic;
using System.Text;
using BO;
namespace BlApi
{
    public interface ISimulation
    {
        Parcel convertDalToBLParcelSimulation(DO.Parcel p);
        void removeDroneChargeByDroneId(int droneId);

    }
}
