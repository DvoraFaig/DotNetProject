using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static global::DalApi.Idal factory(string objName)
        {
            switch (objName)
            {
                case "DalObject":
                    return DalObject.DalObject.GetInstance;
                case "DalXml":
                    return DalObject.DalObject.GetInstance;
                default:
                    throw new Exception();
            }
        }
    }
}
