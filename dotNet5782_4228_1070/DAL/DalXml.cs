//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using System.Xml.Serialization;
//using DO;


//namespace DalObject
//{
//    class DalXML : DalApi.Idal
//    {
//        //dir need to be up from bin
//        static string dir = @"..\..\..\..\xmlData\";
//        static DalXML()
//        {
//            if (!Directory.Exists(dir))
//                Directory.CreateDirectory(dir);
//        }

//        string stationFilePath = @"StationsList.xml";
//        string droneFilePath = @"DroneList.xml";
//        string droneChargeFilePath = @"DroneChargesList.xml";
//        string customerFilePath = @"CustomersList.xml";
//        string parcelFilePath = @"ParcelsList.xml";
//        string workerFilePath = @"WorkersList.xml";

//        public DalXML()
//        {
//            if (!File.Exists(dir + stationFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.Station>(DataSource.Stations, dir + stationFilePath);

//            if (!File.Exists(dir + droneFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(DataSource.Drones, dir + droneFilePath);

//            if (!File.Exists(dir + droneChargeFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(DataSource.DroneCharges, dir + droneChargeFilePath);

//            if (!File.Exists(dir + customerFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(DataSource.Customers, dir + customerFilePath);

//            if (!File.Exists(dir + parcelFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(DataSource.Parcels, dir + parcelFilePath);

//            if (!File.Exists(dir + workerFilePath))
//                DL.XMLTools.SaveListToXMLSerializer<DO.Worker>(DataSource.Workers, dir + workerFilePath);
//        }


//        #region ####

//        //public void DeleteParcel(int id)
//        //{
//        //    IEnumerable<DO.Parcel> parcelLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + par);
//        //    if (parcelLits.Any(t => t.Id == id))
//        //    {
//        //        throw new Exception("DL: Student with the same id not found...");
//        //        //throw new SomeException("DL: Student with the same id not found...");
//        //    }
//        //    DO.Parcel parcel = GetParcelById(id);
//        //    //IEnumerable<DO.Parcel> studentInCourseList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//        //    //if (!studentInCourseList.Any(s => s.Id == id))
//        //    //{
//        //    //    throw new Exception("DL: cannot delete student, courses for this student still exist !!!");
//        //    //    //    throw new SomeException("DL: cannot delete student, courses for this student still exist !!!");
//        //    //}
//        //    parcelLits.ToList().Remove(parcel);

//        //    DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelLits, dir + parcelFilePath);
//        //}
//        //public void UpdateStudent(DO.Student stu)
//        //{
//        //    List<DO.Student> studentList = DL.XMLTools.LoadListFromXMLSerializer<DO.Student>(dir + stationFilePath).ToList();

//        //    int index = studentList.FindIndex(t => t.StudentId == stu.StudentId);

//        //    if (index == -1)
//        //        throw new Exception("DAL: Student with the same id not found...");

//        //    studentList[index] = stu;

//        //    DL.XMLTools.SaveListToXMLSerializer<DO.Student>(studentList, dir + stationFilePath);
//        //}

//        //public DO.Parcel GetParcelById(int id)
//        //{
//        //    try
//        //    {
//        //        IEnumerable<DO.Parcel> parcelList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//        //        return (from parcel in parcelList
//        //                where parcel.Id == id
//        //                select parcel).First();
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        throw new Exception();
//        //    }
//        //}

//        //public IEnumerable<DO.Student> GetAllStudents(Predicate<DO.Student> predicat = null)
//        //{
//        //    predicat ??= ((st) => true);

//        //    IEnumerable<DO.Student> studentList = DL.XMLTools.LoadListFromXMLSerializer<DO.Student>(dir + stationFilePath);

//        //    return from item in studentList
//        //           where predicat(item)
//        //           orderby item.StudentId
//        //           select item;



//        //}



//        //public IEnumerable<DO.Test> getAllTests()
//        //{

//        //    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//        //    IEnumerable<DO.Test> allTests;
//        //    try
//        //    {
//        //        allTests = (from p in testRoot.Elements()
//        //                    select new DO.Test()
//        //                    {
//        //                        IdTest = Convert.ToInt32(p.Element("IdTest").Value),
//        //                        CourseName = p.Element("CourseName").Value,
//        //                        TestDate = Convert.ToDateTime(p.Element("TestDate").Value)
//        //                    });
//        //    }
//        //    catch
//        //    {
//        //        // allTests = null;
//        //        throw new Exception();
//        //    }
//        //    return allTests;
//        //}

//        //public DO.Test GetTestById(int id)
//        //{
//        //    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//        //    DO.Test t = new DO.Test();
//        //    try
//        //    {
//        //        t = (from p in testRoot.Elements()
//        //             where Convert.ToInt32(p.Element("IdTest").Value) == id
//        //             select new DO.Test()
//        //             {
//        //                 IdTest = Convert.ToInt32(p.Element("IdTest").Value),
//        //                 CourseName = p.Element("CourseName").Value,
//        //                 TestDate = Convert.ToDateTime(p.Element("TestDate").Value)
//        //             }).First();
//        //    }
//        //    catch (InvalidOperationException e)
//        //    {
//        //        Console.WriteLine("no element");
//        //        throw new Exception("no test", e);
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Console.WriteLine(e.Message);
//        //    }
//        //    return t;
//        //}

//        //public void AddTest(DO.Test test)
//        //{
//        //    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//        //    XElement testElement;
//        //    testElement = (from p in testRoot.Elements()
//        //                   where Convert.ToInt32(p.Element("IdTest").Value) == test.IdTest
//        //                   select p).FirstOrDefault();
//        //    if (testElement == null)
//        //    {
//        //        XElement IdTest = new XElement("IdTest", test.IdTest);
//        //        XElement CourseName = new XElement("CourseName", test.CourseName);
//        //        XElement TestDate = new XElement("TestDate", test.TestDate.ToString("O"));
//        //        testRoot.Add(new XElement("test", IdTest, CourseName, TestDate));
//        //        testRoot.Save(dir + droneFilePath);
//        //    }
//        //    else
//        //    {
//        //        Console.WriteLine("cannot adding test with id " + test.IdTest + "...");
//        //    }
//        //}

//        //public bool RemoveTest(int id)
//        //{
//        //    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//        //    XElement testElement;
//        //    try
//        //    {
//        //        testElement = (from p in testRoot.Elements()
//        //                       where Convert.ToInt32(p.Element("IdTest").Value) == id
//        //                       select p).FirstOrDefault();
//        //        if (testElement != null)
//        //        {
//        //            testElement.Remove();
//        //            testRoot.Save(dir + droneFilePath);
//        //            return true;
//        //        }
//        //        else return false;
//        //    }
//        //    catch
//        //    {
//        //        return false;
//        //    }
//        //}

//        //public void UpdateTest(DO.Test test)
//        //{
//        //    XElement testRoot = DL.XMLTools.LoadData(dir + droneFilePath);
//        //    XElement testElement = (from p in testRoot.Elements()
//        //                            where Convert.ToInt32(p.Element("IdTest").Value) == test.IdTest
//        //                            select p).FirstOrDefault();
//        //    testElement.Element("CourseName").Value = test.CourseName;
//        //    testElement.Element("TestDate").Value = test.TestDate.ToString();
//        //    testRoot.Save(dir + droneFilePath);
//        //}

//        #endregion

//        //====================================
//        public void AddDrone(DO.Drone drone)
//        {
//            IEnumerable<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
//            if (droneList.Any(d => d.Id == drone.Id))
//            {
//                throw new DO.Exceptions.ObjExistException(typeof(DO.Drone), drone.Id);
//            }

//            droneList.ToList().Add(drone);
//            DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(droneList, dir + droneFilePath);
//        }
//        public void AddStation(Station station)
//        {
//            IEnumerable<DO.Station> stationsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
//            if (stationsList.Any(s => s.Id == station.Id))
//            {
//                throw new DO.Exceptions.ObjExistException(typeof(DO.Station),station.Id);
//            }

//            stationsList.ToList().Add(station);
//            DL.XMLTools.SaveListToXMLSerializer<DO.Station>(stationsList, dir + stationFilePath);
//        }

//        public void AddCustomer(Customer customer)
//        {
//            IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
//            if (customersList.Any(c => c.Id == customer.Id))
//            {
//                throw new DO.Exceptions.ObjExistException(typeof(DO.Customer), customer.Id);
//            }

//            customersList.ToList().Add(customer);
//            DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(customersList, dir + customerFilePath);
//        }

//        public void AddParcel(Parcel parcel)
//        {
//            IEnumerable<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//            if (parcelsList.Any(p => p.Id == parcel.Id))
//            {
//                throw new DO.Exceptions.ObjExistException(typeof(DO.Parcel), parcel.Id);
//            }

//            parcelsList.ToList().Add(parcel);
//            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
//        }

//        public void AddDroneToCharge(DroneCharge droneCharge)
//        {
//            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneFilePath);
//            if (droneChargesList.Any(d => d.DroneId == droneCharge.DroneId))
//            {
//                throw new DO.Exceptions.ObjExistException(typeof(DO.DroneCharge), droneCharge.DroneId);
//            }

//            droneChargesList.ToList().Add(droneCharge);
//            DL.XMLTools.SaveListToXMLSerializer<DO.DroneCharge>(droneChargesList, dir + droneFilePath);
//        }

//        public int amountParcels()
//        {
//            return DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).Count();
//        }

//        public int amountStations()
//        {
//           return DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).Count();

//        }

//        public void removeParcel(Parcel parcel)
//        {
//            IEnumerable<DO.Parcel> parcelLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//            if (!parcelLits.Any(t => t.Id == parcel.Id))
//            {
//                throw new DO.Exceptions.ObjNotExistException(typeof(Parcel),parcel.Id);
//            }
//            DO.Parcel newParcel = getParcelWithSpecificCondition(p=> p.Id == parcel.Id).First();
//            parcelLits.ToList().Remove(newParcel);

//            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelLits, dir + parcelFilePath);
//        }

//        public IEnumerable<Station> GetStations()
//        {
//            IEnumerable<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
//            return from item in studentsList
//                   orderby item.Id
//                   select item;
//        }

//        public IEnumerable<Drone> GetDrones()
//        {
//            IEnumerable<DO.Drone> dronesList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
//            return from item in dronesList
//                   orderby item.Id
//                   select item;
//        }

//        public IEnumerable<Parcel> GetParcels()
//        {
//            IEnumerable<DO.Parcel> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//            return from item in parceslList
//                   orderby item.Id
//                   select item;
//        }

//        public IEnumerable<DroneCharge> GetDroneCharge()
//        {
//            IEnumerable<DO.DroneCharge> droneChargesList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
//            return from item in droneChargesList
//                   orderby item.StationId // or orderby item.DroneId
//                   select item;
//        }

//        public IEnumerable<Customer> GetCustomers()
//        {
//            IEnumerable<DO.Customer> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
//            return from item in parceslList
//                   orderby item.Id
//                   select item;
//        }

//        public void changeStationInfo(Station s)
//        {
//            List<DO.Station> studentsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath).ToList();
//            int index = studentsList.FindIndex(t => t.Id == s.Id);
//            if (index == -1)
//                throw new DO.Exceptions.ObjNotExistException(typeof(Station),s.Id);

//            studentsList[index] = s;
//            DL.XMLTools.SaveListToXMLSerializer<DO.Station>(studentsList, dir + stationFilePath);
//        }

//        public void changeParcelInfo(Parcel p)
//        {
//            List<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath).ToList();
//            int index = parcelsList.FindIndex(t => t.Id == p.Id);
//            if (index == -1)
//                throw new DO.Exceptions.ObjNotExistException(typeof(Parcel), p.Id);

//            parcelsList[index] = p;
//            DL.XMLTools.SaveListToXMLSerializer<DO.Parcel>(parcelsList, dir + parcelFilePath);
//        }

//        public void changeDroneInfo(Drone d)
//        {
//            List<DO.Drone> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath).ToList();
//            int index = parcelsList.FindIndex(t => t.Id == d.Id);
//            if (index == -1)
//                throw new DO.Exceptions.ObjNotExistException(typeof(Drone), d.Id);

//            parcelsList[index] = d;
//            DL.XMLTools.SaveListToXMLSerializer<DO.Drone>(parcelsList, dir + droneFilePath);
//        }

//        public void changeCustomerInfo(Customer c)
//        {
//            List<DO.Customer> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath).ToList();
//            int index = parcelsList.FindIndex(t => t.Id == c.Id);
//            if (index == -1)
//                throw new DO.Exceptions.ObjNotExistException(typeof(Customer), c.Id);

//            parcelsList[index] = c;
//            DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(parcelsList, dir + customerFilePath);
//        }

//        public bool IsCustomerById(int id)
//        {
//            IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
//            if (customersList.Any(c => c.Id == id))
//                return true;
            
//            return false;
//        }

//        public bool IsParcelById(int id)
//        {
//            IEnumerable<DO.Parcel> parcelsLists = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//            if (parcelsLists.Any(p => p.Id == id))
//                return true;

//            return false;
//        }

//        public bool IsDroneById(int id)
//        {
//            IEnumerable<DO.Drone> dronesLits = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
//            if (dronesLits.Any(d => d.Id == id))
//                return true;

//            return false;
//        }

//        public bool IsStationById(int id)
//        {
//            IEnumerable<DO.Station> stationsLists = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
//            if (stationsLists.Any(s => s.Id == id))
//                return true;

//            return false;
//        }

//        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.Drone> droneList = DL.XMLTools.LoadListFromXMLSerializer<DO.Drone>(dir + droneFilePath);
//                return (from drone in droneList
//                        where predicate(drone)
//                        select drone);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.Parcel> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Parcel>(dir + parcelFilePath);
//                return (from parcel in parcelsList
//                        where predicate(parcel)
//                        select parcel);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.Customer> customerList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
//                return (from customer in customerList
//                        where predicate(customer)
//                        select customer);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.Station> stationList = DL.XMLTools.LoadListFromXMLSerializer<DO.Station>(dir + stationFilePath);
//                return (from station in stationList
//                        where predicate(station)
//                        select station);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.DroneCharge> droneChargeList = DL.XMLTools.LoadListFromXMLSerializer<DO.DroneCharge>(dir + droneChargeFilePath);
//                return (from droneCharge in droneChargeList
//                        where predicate(droneCharge)
//                        select droneCharge);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate)
//        {
//            try
//            {
//                IEnumerable<DO.Worker> workerList = DL.XMLTools.LoadListFromXMLSerializer<DO.Worker>(dir + workerFilePath);
//                return (from worker in workerList
//                        where predicate(worker)
//                        select worker);
//            }
//            catch (Exception e)
//            {
//                throw new Exception();
//            }
//        }

//        public double[] electricityUseByDrone()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    class DL
//    {

//        public class XMLTools
//        {
//            #region SaveLoadWithXMLSerializer
//            public static void SaveListToXMLSerializer<T>(IEnumerable<T> list, string filePath)
//            {
//                try
//                {
//                    FileStream file = new FileStream(filePath, FileMode.Create);
//                    XmlSerializer x = new XmlSerializer(list.GetType());
//                    x.Serialize(file, list);
//                    file.Close();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                    //throw new DO.XMLFileLoadCreateException(filePath, $"fail to create xml file: {filePath}", ex);
//                }
//            }
//            public static IEnumerable<T> LoadListFromXMLSerializer<T>(string filePath)
//            {
//                try
//                {
//                    if (File.Exists(filePath))
//                    {
//                        IEnumerable<T> list;
//                        XmlSerializer x = new XmlSerializer(typeof(List<T>));
//                        FileStream file = new FileStream(filePath, FileMode.Open);
//                        list = (IEnumerable<T>)x.Deserialize(file);
//                        file.Close();
//                        return list;
//                    }

//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);  // DO.XMLFileLoadCreateException(filePath, $"fail to load xml file: {filePath}", ex);
//                }
//                throw new Exception();
//            }
//            #endregion

//            public static XElement LoadData(string filePath)
//            {
//                try
//                {
//                    return XElement.Load(filePath);
//                }
//                catch
//                {
//                    Console.WriteLine("File upload problem");
//                    return null;
//                }
//            }
//        }
//    }
//}
