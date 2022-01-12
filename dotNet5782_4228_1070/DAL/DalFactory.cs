using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DalApi
{
    public static class DalFactory
    {

        public static global::DalApi.Idal factory(string objName)
        {
            string dalType = "DalObject";//DalConfig.DalName;
            string dalPkg = "DalObject";// DalConfig.DalPackages[dalType];

            if (dalPkg == null) throw new Exceptions.DalConfigException($"Package {dalType} is not fount in package list in dal-config.xml");

            try { Assembly.Load(dalPkg); }
            catch (Exception) { throw new Exceptions.DalConfigException("Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");

            if (type == null) throw new Exceptions.DalConfigException($"Class {dalPkg} was not fount in the {dalPkg}.dll");

            DalApi.Idal dal = (DalApi.Idal)type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null);

            if (dal == null) throw new Exceptions.DalConfigException($"Class {dalPkg} is not a singelton or wrong property name for Instance");

            return dal;
        }
    }

    //public static global::DalApi.Idal factory(string objName)
    //{
    //    switch (objName)
    //    {
    //        case "DalObject":
    //            return DalObject.DalObject.GetInstance;
    //        case "DalXml":
    //            return DalObject.DalObject.GetInstance;
    //        default:
    //            throw new Exception();
    //    }
    //}
}

