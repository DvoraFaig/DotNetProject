using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.IBL;

namespace IBL.IBL
{
    public static class BLFactory 
    {
        public static Ibl Factory(string objName)
        {
            switch (objName)
            {
                case "BL":
                    return BL.BL.GetInstance;
                default:
                    throw new Exception();
            }
        }
    }
}
