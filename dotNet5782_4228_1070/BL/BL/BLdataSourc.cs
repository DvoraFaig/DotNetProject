using System;
using System.Collections.Generic;
using System.Text;


namespace BL
{
    public sealed partial class BL : IBL.IBL
    {
        public static class BLdataSource
        {
            public static List<IBL.BO.BLDrone> BLDrones = new List<IBL.BO.BLDrone>();

        }
    }
}