using System;
using System.Collections.Generic;
using System.IO;
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
            //string nameOfProj = "DalObject";
            //Assembly assembly = Assembly.ReflectionOnlyLoadFrom(nameOfProj);
            //object instance = assembly.CreateInstance("DalObject");

            ////
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string MyDLLPath = Path.GetDirectoryName(path);
            //C:\Users\HP\Pictures\C#TheMainProject\dotNet5782_4228_1070\dotNet5782_4228_1070\DalObject\DalObject.csproj
            //C:\Users\HP\Pictures\C#TheMainProject\dotNet5782_4228_1070\dotNet5782_4228_1070\DAL\DAL.csproj
            ////////System.Reflection.Assembly myDllAssembly =  System.Reflection.Assembly.LoadFile("..\\..\\..\\..\\DalObject\\DalObject.csproj");
            System.Reflection.Assembly myDllAssembly =  System.Reflection.Assembly.LoadFile("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\DalObject.csproj");
            Type type = myDllAssembly.GetType("DalObject");
            object instanceOfMyType = Activator.CreateInstance(type);
            DalApi.Idal idal = (DalApi.Idal)instanceOfMyType;
            return idal;
            ///
            //Form MyDLLFormInstance = (Form)myDllAssembly.CreateInstance("MyDLLNamespace.MyDLLForm");

            //switch ()
            //{
            //    case "DalObject":
            //        return DalObject.DalObject.GetInstance;
            //    case "DalXml":
            //        return DalObject.DalObject.GetInstance;
            //    default:
            //        throw new Exception();
            //}

            ////// Use the file name to load the assembly into the current
            ////// application domain.
            ////Assembly a = Assembly.Load("DalXml");
            ////// Get the type to use.
            ////Type myType = a.GetType("DalXml");
            ////// Get the method to call.
            ////MethodInfo myMethod = myType.GetMethod("MethodA");
            ////// Create an instance.
            ////object obj = Activator.CreateInstance(myType);
            ////// Execute the method.
            ////myMethod.Invoke(obj, null);
            ///
            //Assembly assembly = Assembly.LoadFrom("DalObject.dll");

            //Type type = assembly.GetType("DalObject");

            //object instanceOfMyType = Activator.CreateInstance(type);
        }
    }
}
