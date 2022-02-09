using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public class DalFactory
    {

        public static IDal factory() //global::DalApi.Idal
        {
            string dalType = DalConfig.DalName;
            string dalPkg = DalConfig.DalPackages[dalType];

            if (dalPkg == null) throw new DalConfigException($"Package {dalType} is not fount in package list in dal-config.xml");

            try { Assembly.Load(dalPkg); }
            catch (Exception) { throw new DalConfigException("Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");

            if (type == null) throw new DalConfigException($"Class {dalPkg} was not fount in the {dalPkg}.dll");

            DalApi.IDal dal = (DalApi.IDal)type.GetProperty("GetInstance", BindingFlags.Public | BindingFlags.Static).GetValue(null);

            if (dal == null) throw new DalConfigException($"Class {dalPkg} is not a singelton or wrong property name for Instance");

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

    //string nameOfProj = "DalObject";
    //Assembly assembly = Assembly.ReflectionOnlyLoadFrom(nameOfProj);
    //object instance = assembly.CreateInstance("DalObject");

    ////
    //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
    //UriBuilder uri = new UriBuilder(codeBase);
    //string path = Uri.UnescapeDataString(uri.Path);
    //string MyDLLPath = Path.GetDirectoryName(path);
    //////string startupPath = System.IO.Directory.GetCurrentDirectory();
    //////string startupPatha = Environment.CurrentDirectory;
    //////string a = "C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\DalObject.csproj";
    //////bool yes = File.Exists(a);
    ////////C:\Users\HP\Pictures\C#TheMainProject\dotNet5782_4228_1070\dotNet5782_4228_1070\DalObject\DalObject.csproj
    ////////C:\Users\HP\Pictures\C#TheMainProject\dotNet5782_4228_1070\dotNet5782_4228_1070\DAL\DAL.csproj
    //////////////System.Reflection.Assembly myDllAssembly =  System.Reflection.Assembly.LoadFile("..\\..\\..\\..\\DalObject\\DalObject.csproj");
    ////////System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\DalObject.csproj");
    ////System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.Load("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\bin\\Debug\\net5.0\\ref");
    //System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.Load("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\bin\\Debug\\net5.0\\ref\\DalObject.dll");
    ////System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.Load("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\DalObject.csproj");
    //Type type = myDllAssembly.GetType("DalObject");
    //object instanceOfMyType = Activator.CreateInstance(type);
    //DalApi.Idal idal = (DalApi.Idal)instanceOfMyType;
    //        return idal;
    //        ///
    //        //Form MyDLLFormInstance = (Form)myDllAssembly.CreateInstance("MyDLLNamespace.MyDLLForm");

    //        //switch ()
    //        //{
    //        //    case "DalObject":
    //        //        return DalObject.DalObject.GetInstance;
    //        //    case "DalXml":
    //        //        return DalObject.DalObject.GetInstance;
    //        //    default:
    //        //        throw new Exception();
    //        //}

    //        ////// Use the file name to load the assembly into the current
    //        ////// application domain.
    //        ////Assembly a = Assembly.Load("DalXml");
    //        ////// Get the type to use.
    //        ////Type myType = a.GetType("DalXml");
    //        ////// Get the method to call.
    //        ////MethodInfo myMethod = myType.GetMethod("MethodA");
    //        ////// Create an instance.
    //        ////object obj = Activator.CreateInstance(myType);
    //        ////// Execute the method.
    //        ////myMethod.Invoke(obj, null);
    //        ///
    //        //Assembly assembly = Assembly.LoadFrom("C:\\Users\\HP\\Pictures\\C#TheMainProject\\dotNet5782_4228_1070\\dotNet5782_4228_1070\\DalObject\\DalObject.dll");

    //        //Type type = assembly.GetType("DalObject");

    //        //object instanceOfMyType = Activator.CreateInstance(type);

    //        //DalApi.Idal idal = (DalApi.Idal)instanceOfMyType;

    //        //return idal;
}

