using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;
using DalObject;

namespace DalXML
{
    class DalXml //: //DalApi.Idal
    {
        //dir need to be up from bin
        static string dir = @"..\..\..\..\xmlData\";
        static DalXml()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            DataSource.Initialize(); ////
        }

        string stationFilePath = @"StationsList.xml";
        string droneFilePath = @"DroneList.xml";
        string droneChargeFilePath = @"DroneChargesList.xml";
        string customerFilePath = @"CustomersList.xml";
        string parcelFilePath = @"ParcelsList.xml";
        string workerFilePath = @"WorkersList.xml";
        string configFilePath = @"Config.xml";

        public DalXml()
        {
            if (!File.Exists(dir + stationFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.Station>(DataSource.Stations, dir + stationFilePath);

            if (!File.Exists(dir + droneFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(DataSource.Drones, dir + droneFilePath);

            if (!File.Exists(dir + droneChargeFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(DataSource.DroneCharges, dir + droneChargeFilePath);

            if (!File.Exists(dir + customerFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(DataSource.Customers, dir + customerFilePath);

            if (!File.Exists(dir + parcelFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(DataSource.Parcels, dir + parcelFilePath);

            if (!File.Exists(dir + workerFilePath))
                DL.XMLTools.SaveListToXMLSerializer<DO.Worker>(DataSource.Workers, dir + workerFilePath);

            if (!File.Exists(dir + configFilePath))
            {
                //DL.XMLTools.SaveListToXMLSerializer(DataSource.Config.DalObjectOrDalXml, dir + configFilePath);
                XElement configRoot = DL.XMLTools.LoadData(dir + configFilePath);
                configRoot.Add(new XElement("DalObjectOrDalXml", "DalXml"));
                double[] DroneElectricityUsage = { 0.1, 0.2, 0.4, 0.6, 0.7 };
                //IEnumerable<double> DroneElectricityUsageList = DroneElectricityUsage;
                configRoot.Add(DroneElectricityUsage);                
                configRoot.Save(dir + configFilePath);
                //DL.XMLTools.SaveListToXMLSerializer<double>(DroneElectricityUsage, dir + configFilePath);
                //XElement empty = new XElement("empty", "0.1");
                //XElement lightWeight = new XElement("lightWeight", "0.2");
                //XElement mediumWeight = new XElement("mediumWeight", "0.4");
                //XElement heavyWeight = new XElement("heavyWeight", "0.6");
                //XElement chargingRate = new XElement("lightWeight", "0.7");
                //XElement DalObjectOrDalXml = new XElement("lightWeight", "DalXml");
                //configRoot.Add(new XElement("DroneElectricityUsage",empty, lightWeight, mediumWeight, heavyWeight ));
            }
        }

        /// <summary>
        /// Add the new drone to Drones.
        /// </summary>
        /// <param name="newDrone">drone to add.</param>
        public void AddDrone(DO.Drone newDrone)
        {
            IEnumerable<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            if (droneList.Any(d => d.Id == newDrone.Id))
            {
                throw new DO.Exceptions.ObjExistException(typeof(DO.Drone), newDrone.Id);
            }

            droneList.ToList().Add(newDrone);
            DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);
        }

        /// <summary>
        /// Add the new station to Stations.
        /// </summary>
        /// <param name="newStation">The station to add.</param>
        public void AddStation(Station newStation)
        {
            IEnumerable<DO.Station> stationsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            if (stationsList.Any(s => s.Id == newStation.Id))
            {
                throw new DO.Exceptions.ObjExistException(typeof(DO.Station), newStation.Id);
            }

            stationsList.ToList().Add(newStation);
            DL.XMLTools.SaveListToXMLSerializer<DO.Station>(stationsList, dir + stationFilePath);
        }

        /// <summary>
        /// Add the new customer to Customers.
        /// </summary>
        /// <param name="newCustomer">customer to add.</param>
        public void AddCustomer(Customer newCustomer)
        {
            IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            if (customersList.Any(c => c.Id == newCustomer.Id))
            {
                throw new DO.Exceptions.ObjExistException(typeof(DO.Customer), newCustomer.Id);
            }

            customersList.ToList().Add(newCustomer);
            DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(customersList, dir + customerFilePath);
        }

        /// <summary>
        /// Add the new parcel to parcels.
        /// </summary>
        /// <param name="newParcel">parcel to add</param>
        public void AddParcel(Parcel newParcel)
        {
            IEnumerable<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            if (parcelsList.Any(p => p.Id == newParcel.Id))
            {
                throw new DO.Exceptions.ObjExistException(typeof(DO.Parcel), newParcel.Id);
            }

            parcelsList.ToList().Add(newParcel);
            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
        }

        /// <summary>
        /// Add the new DroneCharge to DroneCharges.
        /// </summary>
        /// <param name="newDroneCharge">DroneCharge to add.</param>
        public void AddDroneToCharge(DroneCharge newDroneCharge)
        {
            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneFilePath);
            if (droneChargesList.Any(d => d.DroneId == newDroneCharge.DroneId))
            {
                throw new DO.Exceptions.ObjExistException(typeof(DO.DroneCharge), newDroneCharge.DroneId);
            }

            droneChargesList.ToList().Add(newDroneCharge);
            DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(droneChargesList, dir + droneFilePath);
        }

        /// <summary>
        /// Return how much parcels there is.
        /// </summary>
        /// <returns></returns>
        public int amountParcels()
        {
            return DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).Count();
        }

        /// <summary>
        /// Return how much stations there is.
        /// </summary>
        /// <returns></returns>
        public int amountStations()
        {
            return DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).Count();
        }

        /// <summary>
        /// Remove specific parcel
        /// </summary>
        /// <param name="parcel">remove current parcel</param>
        public void removeParcel(Parcel parcel)
        {
            IEnumerable<DO.Parcel> parcelLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            if (!parcelLits.Any(t => t.Id == parcel.Id))
            {
                throw new DO.Exceptions.ObjNotExistException(typeof(Parcel), parcel.Id);
            }
            DO.Parcel newParcel = getParcelWithSpecificCondition(p => p.Id == parcel.Id).First();
            parcelLits.ToList().Remove(newParcel);

            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelLits, dir + parcelFilePath);
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> GetStations()
        {
            IEnumerable<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            return from item in studentsList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        /// Get all drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            IEnumerable<DO.Drone> dronesList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            return from item in dronesList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        /// Get all stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels()
        {
            IEnumerable<DO.Parcel> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            return from item in parceslList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        /// Get all droneCharge.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneCharge> GetDroneCharge()
        {
            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
            return from item in droneChargesList
                   orderby item.StationId // or orderby item.DroneId
                   select item;
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            IEnumerable<DO.Customer> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            return from item in parceslList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        /// Change specific stations info
        /// </summary>
        /// <param name="stationWithUpdateInfo">Station with the changed info</param>
        public void changeStationInfo(Station stationWithUpdateInfo)
        {
            List<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).ToList();
            int index = studentsList.FindIndex(t => t.Id == stationWithUpdateInfo.Id);
            if (index == -1)
                throw new DO.Exceptions.ObjNotExistException(typeof(Station), stationWithUpdateInfo.Id);

            studentsList[index] = stationWithUpdateInfo;
            DL.XMLTools.SaveListToXMLSerializer<DO.Station>(studentsList, dir + stationFilePath);
        }

        /// <summary>
        ///  Change specific parcel info
        /// </summary>
        /// <param name="parcelWithUpdateInfo">The parcel with the changed info</param>
        public void changeParcelInfo(Parcel parcelWithUpdateInfo)
        {
            List<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
            int index = parcelsList.FindIndex(t => t.Id == parcelWithUpdateInfo.Id);
            if (index == -1)
                throw new DO.Exceptions.ObjNotExistException(typeof(Parcel), parcelWithUpdateInfo.Id);

            parcelsList[index] = parcelWithUpdateInfo;
            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            List<DO.Drone> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath).ToList();
            int index = parcelsList.FindIndex(t => t.Id == droneWithUpdateInfo.Id);
            if (index == -1)
                throw new DO.Exceptions.ObjNotExistException(typeof(Drone), droneWithUpdateInfo.Id);

            parcelsList[index] = droneWithUpdateInfo;
            DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(parcelsList, dir + droneFilePath);
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer customerWithUpdateInfo)
        {
            List<DO.Customer> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath).ToList();
            int index = parcelsList.FindIndex(t => t.Id == customerWithUpdateInfo.Id);
            if (index == -1)
                throw new DO.Exceptions.ObjNotExistException(typeof(Customer), customerWithUpdateInfo.Id);

            parcelsList[index] = customerWithUpdateInfo;
            DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(parcelsList, dir + customerFilePath);
        }

        /// <summary>
        /// If customer with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for customer with this id</param>
        /// <returns></returns>
        public bool IsCustomerById(int requestedId)
        {
            IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            if (customersList.Any(c => c.Id == requestedId))
                return true;

            return false;
        }

        /// <summary>
        /// If parcel with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for parcel with this id</param>
        /// <returns></returns>
        public bool IsParcelById(int requestedId)
        {
            IEnumerable<DO.Parcel> parcelsLists = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
            if (parcelsLists.Any(p => p.Id == requestedId))
                return true;

            return false;
        }

        /// <summary>
        /// If drone with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        public bool IsDroneById(int requestedId)
        {
            IEnumerable<DO.Drone> dronesLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
            if (dronesLits.Any(d => d.Id == requestedId))
                return true;

            return false;
        }

        /// <summary>
        /// If station with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for station with this id</param>
        /// <returns></returns>
        public bool IsStationById(int requestedId)
        {
            IEnumerable<DO.Station> stationsLists = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
            if (stationsLists.Any(s => s.Id == requestedId))
                return true;

            return false;
        }

        /// <summary>
        /// Get a Drone/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate)
        {
            try
            {
                IEnumerable<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
                return (from drone in droneList
                        where predicate(drone)
                        select drone);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a Parcel/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a parcel/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
        {
            try
            {
                IEnumerable<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
                return (from parcel in parcelsList
                        where predicate(parcel)
                        select parcel);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a Customer/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a customer/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate)
        {
            try
            {
                IEnumerable<DO.Customer> customerList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
                return (from customer in customerList
                        where predicate(customer)
                        select customer);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a Station/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a station/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate)
        {
            try
            {
                IEnumerable<DO.Station> stationList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
                return (from station in stationList
                        where predicate(station)
                        select station);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a DroneCharge/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone charge /s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate)
        {
            try
            {
                IEnumerable<DO.DroneCharge> droneChargeList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
                return (from droneCharge in droneChargeList
                        where predicate(droneCharge)
                        select droneCharge);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Get a Worker/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a worker/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate)
        {
            try
            {
                IEnumerable<DO.Worker> workerList = DL.XMLTools.LoadListFromXMLSerializer<DO.Worker>(dir + workerFilePath);
                return (from worker in workerList
                        where predicate(worker)
                        select worker);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public double[] electricityUseByDrone()
        {
            throw new NotImplementedException();
        }
    }

    class DL
    {

        public class XMLTools
        {
            #region SaveLoadWithXMLSerializer
            public static void SaveListToXMLSerializer<T>(IEnumerable<T> list, string filePath)
            {
                try
                {
                    FileStream file = new FileStream(filePath, FileMode.Create);
                    XmlSerializer x = new XmlSerializer(list.GetType());
                    x.Serialize(file, list);
                    file.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
                }
            }
            public static IEnumerable<T> LoadListFromXMLSerializer<T>(string filePath)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        IEnumerable<T> list;
                        XmlSerializer x = new XmlSerializer(typeof(List<T>));
                        FileStream file = new FileStream(filePath, FileMode.Open);
                        list = (IEnumerable<T>)x.Deserialize(file);
                        file.Close();
                        return list;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);  // DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
                }
                throw new Exception();
            }
            #endregion

            public static XElement LoadData(string filePath)
            {
                try
                {
                    return XElement.Load(filePath);
                }
                catch
                {
                    Console.WriteLine("File upload problem");
                    return null;
                }
            }
        }
    }
}
#region ####

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