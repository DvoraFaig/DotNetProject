using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static global::DalApi.Idal factory()
        {
            string nameOfProj = "DalXml";
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(nameOfProj);
            object instance = assembly.CreateInstance("DalXml");
            //switch ()
            //{
            //    case "DalObject":
            //        return DalObject.DalObject.GetInstance;
            //    case "DalXml":
            //        return DalObject.DalObject.GetInstance;
            //    default:
            //        throw new Exception();
            //}

            // Use the file name to load the assembly into the current
            // application domain.
            Assembly a = Assembly.Load("DalXml");
            // Get the type to use.
            Type myType = a.GetType("DalXml");
            // Get the method to call.
            MethodInfo myMethod = myType.GetMethod("MethodA");
            // Create an instance.
            object obj = Activator.CreateInstance(myType);
            // Execute the method.
            myMethod.Invoke(obj, null);
        }
    }
}
