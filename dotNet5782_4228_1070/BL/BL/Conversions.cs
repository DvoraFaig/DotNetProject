//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BO;
//using static BO.Exceptions;

//namespace BL
//{
//    sealed partial class BL : BlApi.Ibl
//    {
       
        

        

        

        

       

        

        

        

        

        
//    }
//}

////private ChargingDrone convertDalToBLChargingDrone(DO.DroneCharge droneCharge) //convertDalToBLChargingDrone the opposite BL to DAL
////{
////    return new ChargingDrone()
////    {
////        Id = droneCharge.DroneId,
////        Battery = getBLDroneWithSpecificCondition(d => d.Id == droneCharge.DroneId).First().Battery
////    };
////}
////=======================================================
////private ParcelInTransfer convertDalToParcelInTranspare(DO.Parcel parcel)
////{
////    return new ParcelInTransfer()
////    {
////        Id = parcel.Id,

////    };
////}
////private DO.Station convertBLToDalStation(Station s)
////{
////    return new DO.Station()
////    {
////        Id = s.Id,
////        Name = s.Name,
////        ChargeSlots = s.DroneChargeAvailble + s.DronesCharging.Count(),
////        Longitude = s.StationPosition.Longitude,
////        Latitude = s.StationPosition.Latitude
////    };
////}

////=======================================================
////=======================================================
////private Drone convertDalToBLDrone(DO.Drone d)//////////////////////////////////////////////////////////////////////////////
////{
////    Drone BLDrone = getBLDroneById(d.Id);
////    return new Drone() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, Status = (DroneStatus)r.Next(0, 3), Battery = r.Next(20, 100)/*DronePosition ++++++++++++++++++++*/};
////}
////private List<DroneToList> convertBLDroneToBLDronesToList(List<Drone> drones)
////{
////    List<DroneToList> listDrones = new List<DroneToList>();
////    DroneToList toAdd = new DroneToList();
////    foreach (Drone d in drones)
////    {
////        if (d.ParcelInTransfer == (null))
////            toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition };
////        else
////            toAdd = new DroneToList() { Id = d.Id, Model = d.Model, MaxWeight = d.MaxWeight, droneStatus = d.Status, Battery = d.Battery, DronePosition = d.DronePosition, IdParcel = d.ParcelInTransfer.Id };
////        listDrones.Add(toAdd);
////    }
////    return listDrones;
////}
////=======================================================
////=======================================================
//        //private List<ParcelToList> convertBLParcelToBLParcelsToList()
//        //{
//        //    IEnumerable<DO.Parcel> parcels = dal.GetParcels();
//        //    List<ParcelToList> listParcels = new List<ParcelToList>();
//        //    ParcelToList toAdd = new ParcelToList();
//        //    foreach (DO.Parcel p in parcels)
//        //    {
//        //        toAdd = new ParcelToList()
//        //        {
//        //            Id = p.Id,
//        //            //SenderName = p.Sender.name,
//        //            //TargetName = p.Target.name,
//        //            SenderName = dal.getCustomerWithSpecificCondition(c => c.Id == p.SenderId).First().Name,
//        //            TargetName = dal.getCustomerWithSpecificCondition(c => c.Id == p.TargetId).First().Name,
//        //            Weight = p.Weight,
//        //            Priority = p.Priority,
//        //            ParcelStatus = findParcelStatus(p)
//        //        };
//        //        listParcels.Add(toAdd);
//        //    }
//        //    return listParcels;
//        //}
////=======================================================
