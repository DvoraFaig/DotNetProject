using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public static class DalFactory
    {
        public static IDal.DO.IDal factory(string objName)
        {
            switch (objName)
            {
                case "DalObject":
                    return DalObject.DalObject.GetInstance; 
                default:
                    throw new Exception();
            }
        }
    }
}
