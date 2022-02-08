using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    public sealed partial class DalXml : DalApi.Idal
    {
        //dir need to be up from bin
        static string dir = @"DalXml\DataSource\";

        /// <summary>
        /// instance of DalXml and will be equal to DalApi
        /// </summary>
        private static DalXml Instance;

        /// <summary>
        /// Avoid reaching DalXml instance by the same time a few places.
        /// DalXml is supposed to be a Singelton
        /// </summary>
        private static readonly object padlock = new object();

        /// <summary>
        /// return one and only one instance of DalXml 
        /// </summary>
       // [MethodImpl(MethodImplOptions.Synchronized)]
        public static DalXml GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (Instance == null)
                        Instance = new DalXml();
                    return Instance;
                }
            }
        }

        string stationFilePath = @"StationsList.xml";
        string droneFilePath = @"DroneList.xml";
        string droneChargeFilePath = @"DroneChargesList.xml";
        string customerFilePath = @"CustomersList.xml";
        string parcelFilePath = @"ParcelsList.xml";
        string workerFilePath = @"WorkersList.xml";
        string configFilePath = @"Config.xml";

        private DalXml()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            DataSource.Initialize(); //////////

            if (!File.Exists(dir + stationFilePath))
                XMLTools.SaveListToXMLSerializer<DO.Station>(DataSource.Stations, dir + stationFilePath);

            if (!File.Exists(dir + droneFilePath))
                XMLTools.SaveListToXMLSerializer<DO.Drone>(DataSource.Drones, dir + droneFilePath);

            if (!File.Exists(dir + droneChargeFilePath))
                XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(DataSource.DroneCharges, dir + droneChargeFilePath);

            if (!File.Exists(dir + customerFilePath))
                XMLTools.SaveListToXMLSerializer<DO.Customer>(DataSource.Customers, dir + customerFilePath);

            if (!File.Exists(dir + parcelFilePath))
                XMLTools.SaveListToXMLSerializer<DO.Parcel>(DataSource.Parcels, dir + parcelFilePath);

            if (!File.Exists(dir + workerFilePath))
                XMLTools.SaveListToXMLSerializer<DO.Worker>(DataSource.Workers, dir + workerFilePath);

            if (!File.Exists(dir + configFilePath))
            {
                XElement configRoot = XMLTools.LoadData(dir + configFilePath);
                double[] DroneElectricityUsage = { 0.1, 0.2, 0.4, 0.6, 0.7 };
                XMLTools.SaveListToXMLSerializer<double>(DroneElectricityUsage, dir + configFilePath);
            }
        }
    }   
}
#region garbage

//public void DeleteParcel(int id)
//{
//    IEnumerable<DO.Parcel> parcelLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + par);
//    if (parcelLits.Any(t => t.Id == id))
//    {
//        throw new Exception("DL: Student with the same id not found...");
//        //throw new SomeException("DL: Student with the same id not found...");
//    }
//    DO.Parcel parcel = GetParcelById(id);
//    //IEnumerable<DO.Parcel> studentInCourseList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//    //if (!studentInCourseList.Any(s => s.Id == id))
//    //{
//    //    throw new Exception("DL: cannot delete student, courses for this student still exist !!!");
//    //    //    throw new SomeException("DL: cannot delete student, courses for this student still exist !!!");
//    //}
//    parcelLits.ToList().Remove(parcel);

//    DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelLits, dir + parcelFilePath);
//}
//public void UpdateStudent(DO.Student stu)
//{
//    List<DO.Student> studentList = DL.XMLTools.LoadListFromXMLSerializer<DO.Student>(dir + stationFilePath).ToList();

//    int index = studentList.FindIndex(t => t.StudentId == stu.StudentId);

//    if (index == -1)
//        throw new Exception("DAL: Student with the same id not found...");

//    studentList[index] = stu;

//    DL.XMLTools.SaveListToXMLSerializer<DO.Student>(studentList, dir + stationFilePath);
//}

//public DO.Parcel GetParcelById(int id)
//{
//    try
//    {
//        IEnumerable<DO.Parcel> parcelList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//        return (from parcel in parcelList
//                where parcel.Id == id
//                select parcel).First();
//    }
//    catch (Exception e)
//    {
//        throw new Exception();
//    }
//}

//public IEnumerable<DO.Student> GetAllStudents(Predicate<DO.Student> predicat = null)
//{
//    predicat ??= ((st) => true);

//    IEnumerable<DO.Student> studentList = DL.XMLTools.LoadListFromXMLSerializer<DO.Student>(dir + stationFilePath);

//    return from item in studentList
//           where predicat(item)
//           orderby item.StudentId
//           select item;



//}



//public IEnumerable<DO.Test> getAllTests()
//{

//    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//    IEnumerable<DO.Test> allTests;
//    try
//    {
//        allTests = (from p in testRoot.Elements()
//                    select new DO.Test()
//                    {
//                        IdTest = Convert.ToInt32(p.Element("IdTest").Value),
//                        CourseName = p.Element("CourseName").Value,
//                        TestDate = Convert.ToDateTime(p.Element("TestDate").Value)
//                    });
//    }
//    catch
//    {
//        // allTests = null;
//        throw new Exception();
//    }
//    return allTests;
//}

//public DO.Test GetTestById(int id)
//{
//    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//    DO.Test t = new DO.Test();
//    try
//    {
//        t = (from p in testRoot.Elements()
//             where Convert.ToInt32(p.Element("IdTest").Value) == id
//             select new DO.Test()
//             {
//                 IdTest = Convert.ToInt32(p.Element("IdTest").Value),
//                 CourseName = p.Element("CourseName").Value,
//                 TestDate = Convert.ToDateTime(p.Element("TestDate").Value)
//             }).First();
//    }
//    catch (InvalidOperationException e)
//    {
//        Console.WriteLine("no element");
//        throw new Exception("no test", e);
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//    }
//    return t;
//}

//public void AddTest(DO.Test test)
//{
//    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//    XElement testElement;
//    testElement = (from p in testRoot.Elements()
//                   where Convert.ToInt32(p.Element("IdTest").Value) == test.IdTest
//                   select p).FirstOrDefault();
//    if (testElement == null)
//    {
//        XElement IdTest = new XElement("IdTest", test.IdTest);
//        XElement CourseName = new XElement("CourseName", test.CourseName);
//        XElement TestDate = new XElement("TestDate", test.TestDate.ToString("O"));
//        testRoot.Add(new XElement("test", IdTest, CourseName, TestDate));
//        testRoot.Save(dir + droneFilePath);
//    }
//    else
//    {
//        Console.WriteLine("cannot adding test with id " + test.IdTest + "...");
//    }
//}

//public bool RemoveTest(int id)
//{
//    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//    XElement testElement;
//    try
//    {
//        testElement = (from p in testRoot.Elements()
//                       where Convert.ToInt32(p.Element("IdTest").Value) == id
//                       select p).FirstOrDefault();
//        if (testElement != null)
//        {
//            testElement.Remove();
//            testRoot.Save(dir + droneFilePath);
//            return true;
//        }
//        else return false;
//    }
//    catch
//    {
//        return false;
//    }
//}

//public void UpdateTest(DO.Test test)
//{
//    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//    XElement testElement = (from p in testRoot.Elements()
//                            where Convert.ToInt32(p.Element("IdTest").Value) == test.IdTest
//                            select p).FirstOrDefault();
//    testElement.Element("CourseName").Value = test.CourseName;
//    testElement.Element("TestDate").Value = test.TestDate.ToString();
//    testRoot.Save(dir + droneFilePath);
//}

#endregion