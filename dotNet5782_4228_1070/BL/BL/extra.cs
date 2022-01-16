//public void AddStation(int id, string name, int latitude, int longitude, int chargeSlot)
//{
//    if (dal.IsStationById(id))
//    {
//        throw new ObjExistException("station", id);
//    }
//    else
//    {
//        DO.Station s = new DO.Station() { Id = id, Name = name, Longitude = longitude, Latitude = latitude, ChargeSlots = chargeSlot };
//        dal.AddStation(s);
//    }
//}

//public void AddCustomer(int id, string name, string phone, int latitude, int longitude)
//{
//    if (dal.IsCustomerById(id))
//    {
//        throw new ObjExistException(typeof(BO.Customer), id);
//    }
//    else
//    {
//        DO.Customer c = new DO.Customer() { Id = id, Name = name, Phone = phone, Latitude = latitude, Longitude = longitude };
//        dal.AddCustomer(c);
//    }
//}


//public List<CustomerToList> DisplayCustomersToList()
//{
//    IEnumerable<DO.Customer> customers = dal.displayCustomers();
//    List<CustomerToList> customerToLists = new List<CustomerToList>();


//    foreach (var customer in customers)
//    {
//        customerToLists.Add(converteCustomerToList(customer));
//    }
//    return customerToLists;
//}
//public List<ParcelToList> DisplayParcelToList(List<Parcel> parcels)
//{
//    return convertBLParcelToBLParcelsToList(parcels);
//}
//public IEnumerable<Station> GetStation()
//{
//    IEnumerable<DO.Station> stations = dal.GetStations();
//    return (from station in stations
//    select convertDalToBLStation(station));
//}
//public IEnumerable<BLStationToList> DisplayStationsWithFreeSlots()
//{
//    List<BLStationToList> stationToList = GetStationsToList();
//    return (from station in stationToList
//            where station.DroneChargeAvailble > 0
//            select station);
//}

//public List<Customer> DisplayCustomers()
//{
//    IEnumerable<DO.Customer> customers = dal.displayCustomers();//c.id>100000000
//    List<Customer> costomersWithMoreInfo = new List<Customer>();
//    foreach (var customer in customers)
//    {
//        costomersWithMoreInfo.Add(convertDalToBLCustomer(customer));
//    }
//    return costomersWithMoreInfo;
//}
//public List<DroneToList> DisplayDronesToList()
//{
//    return convertBLDroneToBLDronesToList(dronesInBL);
//}
//public IEnumerable<Parcel> DisplayFreeParcel()
//{
//    IEnumerable<DO.Parcel> freeParcels = dal.getParcelWithSpecificCondition(x => x.DroneId == null);
//    return (from parcel in freeParcels
//            select convertDalToBLParcel(parcel));
//}
//public List<Station> DisplayEmptyDroneCharge()
//{
//    IEnumerable<DO.Station> stations = dal.GetStations();
//    List<Station> stationsWithEmptySlots = new List<Station>();
//    foreach (DO.Station station in stations)
//    {
//        int amountDroneChargeFullInStation = dal.getDroneChargeWithSpecificCondition(droneCharge => droneCharge.StationId == station.Id).Count();
//        if (station.ChargeSlots > amountDroneChargeFullInStation)
//        {
//            stationsWithEmptySlots.Add(convertDalToBLStation(station));
//        }
//    }
//    return stationsWithEmptySlots;
//}


//public void DroneChangeModel(DroneToList newDrone)
//{
//    try
//    {
//        Drone d = getDroneWithSpecificConditionFromDronesList(drone => drone.Id == newDrone.Id).First();
//        droensList.Remove(d);
//        droensList.Add(d);
//        dal.changeDroneInfo(convertBLToDalDrone(d));
//        //dal.changeDroneInfo(d.Id, d.Model);
//    }
//    catch (Exception)
//    {
//        throw new InvalidStringException("Drones' Model");
//    }
//}




//////public static double RoundUp(double input, int places)
//////{
//////    double multiplier = Math.Pow(10, Convert.ToDouble(places));
//////    return Math.Ceiling(input * multiplier) / multiplier;
//////}
///
//public void GetParcelToDelivery(int senderId, int targetId, DO.WeightCategories weight, DO.Priorities priority)
//{
//    DO.Parcel p = new DO.Parcel() { SenderId = senderId, TargetId = targetId, Priority = priority, Requeasted = DateTime.Now, Weight = weight };
//    dal.AddParcel(p);
//}
