using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface Idal
    {
        //===============
        //Add functions//
        //===============
        void AddDrone(Drone d);
        void AddStation(Station s);
        void AddCustomer(Customer c);
        void AddParcel(Parcel parcel);
        void AddDroneToCharge(DroneCharge parcel);

        //==================
        //Remove functions
        //==================
        public void removeStation(Station stationToRemove);
        public void removeCustomer(Customer customerToRemove);
        public void removeDrone(Drone droneToRemove);
        public void removeParcel(Parcel parcel);
        public void removeDroneChargeByDroneId(int droneId);

        int amountParcels();
        int amountStations();    

        //==================
        //display lists
        //==================
        IEnumerable<Station> GetStations();
        IEnumerable<Drone> GetDrones();
        IEnumerable<Parcel> GetParcels();
        IEnumerable<DroneCharge> GetDroneCharges();
        IEnumerable<Customer> GetCustomers();

        //==================
        //change info
        //==================
        void changeStationInfo(Station s);
        void changeParcelInfo(Parcel p);
        void changeDroneInfo(Drone d);
        void changeCustomerInfo(Customer c);

        //======================
        //check if object exist
        //======================
        public Boolean IsCustomerById(int id);
        public Boolean IsParcelById(int id); 
        public Boolean IsDroneById(int id);
        public Boolean IsStationById(int id);
        public Boolean IsDroneChargeById(int id);




        //================================
        //check if object exsist & active
        //================================
        public Boolean IsStationActive(int requestedId);
        public Boolean IsCustomerActive(int requestedId);
        public Boolean IsDroneActive(int requestedId);

        //======================
        //Get Object/s with specific condition = Predicate
        //======================
        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate);
        public IEnumerable<Parcel> getParcelWithSpecificCondition(Predicate<Parcel> predicate);
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate);
        public IEnumerable<Station> getStationWithSpecificCondition(Predicate<Station> predicate);
        public IEnumerable<DroneCharge> getDroneChargeWithSpecificCondition(Predicate<DroneCharge> predicate);
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate);

        double[] electricityUseByDrone();

        ParcelStatuses findParcelStatus(DO.Parcel p);

    }
}

///maybe will be used later

//function to use in the futer
//======================================================
//void removeDrone(Drone drone);
//void removeDroneCharge(DroneCharge droneCharge);
//void removeStation(Station station);
//void removeCustomer(Customer customer);

//public void removeStation(Station station)
//{
//    DataSource.Stations.Remove(station);
//}
//public void removeCustomer(Customer customer)
//{
//    DataSource.Customers.Remove(customer);
//}
//public void removeDrone(Drone drone)
//{
//    DataSource.Drones.Remove(drone);
//}
//public void removeDroneCharge(DroneCharge droneCharge)
//{
//    DataSource.DroneCharges.Remove(droneCharge);
//}
//===========================================================
//==============================================================================================================================================

// void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude);
//void AddParcelToDelivery(int id, int Serderid, int TargetId, WeightCategories Weight, Priorities Priority, DateTime requestedTime) { }

//public void AddDrone(int id, string Model, WeightCategories MaxWeight)
//{
//    Drone drone = new Drone();
//    drone.Id = id;
//    drone.Model = Model;
//    drone.MaxWeight = MaxWeight;
//    DataSource.Drones.Add(drone);
//}

//public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
//{
//    Customer customer = new Customer();
//    customer.Id = id;
//    customer.Name = Name;
//    customer.Phone = Phone;
//    customer.Longitude = Longitude;
//    customer.Latitude = Latitude;
//    DataSource.Customers.Add(customer);
//}


//public void AddParcelToDelivery(int id, int Serderid, int TargetId, WeightCategories Weight, Priorities Priority, DateTime requestedTime)
//{
//    Parcel parcel = new Parcel();
//    parcel.Id = id;
//    parcel.SenderId = Serderid;
//    parcel.TargetId = TargetId;
//    parcel.Weight = Weight;
//    parcel.Priority = Priority;
//    parcel.Requeasted = requestedTime;
//    DataSource.Parcels.Add(parcel);
//}
//==============================================================================================================================================


//public double[] electricityUseByDrone(Drone drone);
//public double[] electricityUseByDrone(Drone drone)
//{
//    double[] droneInfo = new double[5];
//    droneInfo[0] = DataSource.Config.empty;
//    droneInfo[1] = DataSource.Config.lightWeight;
//    droneInfo[2] = DataSource.Config.mediumWeight;
//    droneInfo[3] = DataSource.Config.heavyWeight;
//    droneInfo[4] = DataSource.Config.chargingRate;
//    return droneInfo;
//}
//=========================================================================================================

///update===========================
///maybe will be used later
///===============================
///
////==================
////Update functions//
////==================
//string PairAParcelWithADrone(Parcel parcel);
//void DroneCollectsAParcel(Parcel parcel);
//void CostumerGetsParcel(Drone drone, Parcel parcel);
//void sendDroneToCharge(Drone drone);
//void freeDroneFromCharge(Drone drone);

///// <summary>
///// Function get parcel object and find drone to take it.
///// </summary>
///// <param name="parcel"></param>
///// <returns></returns>
//public string PairAParcelWithADrone(Parcel parcel)
//{
//    for (int i = 0; i < DataSource.Drones.Count(); i++)
//    {
//        //if (DataSource.Drones[i].Status == DroneStatus.Available && (WeightCategories)DataSource.Drones[i].MaxWeight >= parcel.Weight)
//        //{
//        //    Drone d = DataSource.Drones[i];
//        //    parcel.DroneId = DataSource.Drones[i].Id;
//        //    parcel.Scheduled = DateTime.Now; //pair a parcel to dron
//        //    d.Status = DroneStatus.Delivery;
//        //    DataSource.Drones[i] = d;
//        //    return $"The Drone number {d.Id} is ready and will recieve parcel num {parcel.Id}.";
//        //}
//    }
//    return ("No drones available.\n please try later.");
//}

//public void DroneCollectsAParcel(Parcel parcel)
//{
//    if (parcel.DroneId == null) //doesn't have a Drone 
//    {
//        Console.WriteLine("Error! The parcel doesn't have a Drone.\n Please enter DroneReceivesParcel");
//    }
//    else
//    {
//        parcel.PickUp = DateTime.Now;
//        Drone droneCollect = getDroneWithSpecificCondition(d => d.Id == (int)parcel.DroneId).First();
//    }
//}
//public void CostumerGetsParcel(Drone drone, Parcel parcel)
//{
//    parcel.Delivered = DateTime.Now;
//}
//public void sendDroneToCharge(Drone drone)
//{
//    IEnumerable<DroneCharge> droneCharges = GetDroneCharge();
//    Console.WriteLine("Choose id of Station to charge The drone");
//    foreach (DroneCharge charge in droneCharges)
//    {
//        Console.WriteLine(charge.ToString());
//    }
//    int choose = Convert.ToInt32(Console.ReadLine());
//    DroneCharge droneCharge = getDroneChargeWithSpecificCondition(d => d.StationId == choose).First();
//    if (droneCharge.StationId != -1)
//    {
//        droneCharge.DroneId = drone.Id;
//    }
//}
//public void freeDroneFromCharge(Drone drone)
//{
//    DroneCharge chargeToFree = getDroneChargeWithSpecificCondition(d=> d.DroneId == drone.Id).First();
//    chargeToFree.StationId = -1;
//}

//======================================================================================
//Change info
//======================================================================================
//void changeDroneInfo(int id, string newModel);
///// <summary>
///// hange specific drone info
///// </summary>
///// <param name="id">The Drones' id who is needed to change info</param>
///// <param name="newModel">Drone.Model = newModel</param>
//public void changeDroneInfo(int id, string newModel)
//{
//    Drone dToChange = getDroneWithSpecificCondition(d => d.Id == id).First();
//    DataSource.Drones.Remove(dToChange);
//    dToChange.Model = newModel;
//    DataSource.Drones.Add(dToChange);
//}

//======================================================================================


//======================================================================================
//Amount of Obj
//======================================================================================
//public int amountDrones()
//{
//    return DataSource.Drones.Count();
//}
//public int amountCustomers()
//{
//    return DataSource.Customers.Count;
//}
//public int amountDroneCharges()
//{
//    return DataSource.DroneCharges.Count();
//}
//======================================================================================
