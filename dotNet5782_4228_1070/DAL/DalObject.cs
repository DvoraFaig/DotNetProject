using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;



namespace DalObject
{
     public class DalObject
     {
          
        public DalObject()
        {
            DataSource.Initialize();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Add//
        public void AddDrone(int id, string Model, WeightCategories MaxWeight,DroneStatus Status,double Battery)
        {
            Drone drone = new Drone();
            drone.Id = id;
            drone.Model=Model;
            drone.MaxWeight = MaxWeight;
            drone.Status = Status;
            drone.Battery = Battery;
            
        }
            
      
        public void AddStation(int id, string Name,int ChargeSlots,double Longitude,double Latitude)
        {
            Station station = new Station();
            station.ID = id;
            station.Name=Name; 
            station.ChargeSlots = ChargeSlots;
            station.Longitude =Longitude;
            station.Latitude = Latitude; 
                
        }
        public void AddCustomer(int id, string Name , string Phone , double Longitude,double Latitude )
        {
            Customer customer = new Customer();
            customer.ID = id;
            customer.Name=Name; 
            customer.Phone = Phone;
            customer.Longitude =Longitude;
            customer.Latitude = Latitude;
        }
        public void AddParcelToDelivery(int id,int Serderid,int TargetId,WeightCategories Weight,Priorities Priority,Datatime Requeasted,int DroneId,DateTime Scheduled,DateTime PickUp,DateTime Delivered)
        {
            Parcel parcel = new Parcel()
            parcel.Id = id;
            parcel.Serderid = Serderid;
            parcel.TargetId = TargetId;
            parcel.Weight=Weight;
            parcel.Priority =Priority;
            parcel.Requeasted = Requeasted;
            parcel.DroneId =DroneId;
            parcel.Scheduled=Scheduled;
            parcel.PickUp = PickUp;
            parcel.Delivered =Delivered;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Update//
        public var DroneReceivesParcel(Parcel parcel)
        {
            foreach(Drone drone in DataSource.drones)
            {
                if(drone.Status == DroneStatus.Available && (WeightCategories)drone.MaxWeight >= parcel.Weight )
                {
                   parcel.DroneId = drone.Id;
                   drone.Status = DroneStatus.Delivery;
                   return;
                }
            }
            return ("No drones available");
            
        }
        public void DroneCollectsAParcel(Drone drone,Parcel parcel)
        {
            drone.DroneStatus = DroneStatus.Delivery;
            parcel.PickedUp = DateTime.Now;
            parcel.DroneId = drone.Id; 

        }

        public void CostumerGetsParcel(Customer customer , Parcel parcel)
        {
            parcel.Delivered = DateTime.Now;
        }
        public void sendDroneToCharge(Drone drone)
        {
            //??????????????????????????????
            drone.Status = DroneStatus.Maintenance;
            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = drone.Id;
            foreach(Station station in DataSource.Stations)
            {
                if(station.)
            }
            
        }
        public void freeDroneFromCharge(Drone drone)
        {
            //??????????????????????????????
            drone.Status == DroneStatus.Available;
            drone.Battery = 100;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Display//
        public Station displayStations()
        {

        }
        public Drone displayDrone()
        {

        }
        public Parcel displayParcels()
        {

        }
        public Customer displayCustomers()
        {

        }


        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /*כאן יהיו מתודות הוספה, עדכון, וכו
             */
            public void addOptions() { }
        }
     }  
}

