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
        public void AddDrone(int id, string Model, IDAL.DO.WeightCategories MaxWeight, IDAL.DO.DroneStatus Status, double Battery)
        {
            Drone drone = new Drone();
            drone.Id = id;
            drone.Model = Model;
            drone.MaxWeight = MaxWeight;
            drone.Status = Status;
            drone.Battery = Battery;
            DataSource.Drones[DataSource.Config.indexDrones++] = drone;
        }

        public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude)
        {
            Station station = new Station();
            station.Id = id;
            station.Name = Name;
            station.ChargeSlots = ChargeSlots;
            station.Longitude = Longitude;
            station.Latitude = Latitude;
            DataSource.Stations[DataSource.Config.indexStations++] = station;

        }
        public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
        {
            Customer customer = new Customer();
            customer.ID = id;
            customer.Name = Name;
            customer.Phone = Phone;
            customer.Longitude = Longitude;
            customer.Latitude = Latitude;
            DataSource.Customers[DataSource.Config.indexCustomers++] = customer;

        }
        public void AddParcelToDelivery(int id, int Serderid, int TargetId,IDAL.DO.WeightCategories Weight, IDAL.DO.Priorities Priority/*,Datatime Requeasted, int DroneId, DateTime Scheduled, DateTime PickUp, DateTime Delivered*/)
        {
            Parcel parcel = new Parcel();
            parcel.Id = id;
            parcel.SenderId = Serderid;
            parcel.TargetId = TargetId;
            parcel.Weight = Weight;
            parcel.Priority = Priority;
            //parcel.Requeasted = Requeasted;
            //parcel.DroneId = DroneId;
            //parcel.Scheduled = Scheduled;
            //parcel.PickUp = PickUp;
            //parcel.Delivered = Delivered;
            DataSource.Parcels[DataSource.Config.indexParcels++] = parcel;

        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Update//
        //find an availeble drone and send it to the sending costumer.
        public string PairAParcelWithADrone(Parcel parcel,Customer sendingCustomer)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available && (WeightCategories)drone.MaxWeight >= parcel.Weight)
                {
                    parcel.DroneId = drone.Id;
                    parcel.SenderId = sendingCustomer.ID;
                    parcel.Requeasted = DateTime.Today;//prepare a parcel to delivery
                    parcel.Scheduled = DateTime.Now; //pair a parcel to drone
                    //drone.Status = DroneStatus.Delivery;
                    return $"The Drone number{drone.Id} is ready and will receive parcel num {parcel.Id} frome costumer {sendingCustomer.ID}.";
                }
            }
            return ("No drones available.\n please try later.");
        }
        public void DroneCollectsAParcel( Parcel parcel)
        {
            if(parcel.DroneId == 0) //doesn't have a Drone
            {
                Console.WriteLine("Error! The parcel doesn't have a Drone.\n Please enter DroneReceivesParcel");
                return;
            }
            Drone drone = new Drone();//recieves Drone occurding to DroneId;
            //drone.DroneStatus = DroneStatus.Delivery;
            //parcel.PickedUp = DateTime.Now;
            parcel.DroneId = drone.Id;

        }

        public void CostumerGetsParcel(Customer customer, Parcel parcel)
        {
            parcel.Delivered = DateTime.Now;
        }
        public void sendDroneToCharge(Drone drone)
        {
            //DroneCharges;
            foreach (DroneCharge DroneCharge in DataSource.DroneCharges)
            {
                //if(DataSource.DroneCharges[DataSource.Config.indexDroneCharges] == 10)
                //{
                //    Console.WriteLine( "The DroneCharge is full!\nPleasetry later! ");
                //    return;
                //}
            }
            drone.Status = DroneStatus.Maintenance;
            DroneCharge droneCharge = new DroneCharge();
            droneCharge.DroneId = drone.Id;
            //foreach (Station station in DataSource.Stations)
            //{
            //}

        }
        public void freeDroneFromCharge(Drone drone)
        {
            //
            //??????????????????????????????
            //drone.Status == DroneStatus.Available;
            drone.Battery = 100;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Display//
        public static Station getStationById(int id)
        {
            foreach(Station state in DataSource.Stations)
            {
                if (state.Id == id)
                {
                    return state;
                }
            }
            Station m_station = new Station();
            return m_station;
        }
        public static Drone getDroneById(int id)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == id)
                {
                    return drone;
                }
            }
            Drone m_drone = new Drone();
            return m_drone;
        }
        public static Customer getCustomerById(int id)
        {
            foreach (Customer customer in DataSource.Customers)
            {
                if (customer.ID == id)
                {
                    return customer;
                }
            }
            Customer m_customer = new Customer();
            return m_customer;
        }
        public static Parcel getParcelById(int id)
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id == id)
                {
                    return parcel;
                }
            }
            Parcel m_parcel = new Parcel();
            return m_parcel;
        }
        public static DroneCharge getDroneChargeById(int id)
        {
            foreach (DroneCharge droneCharge in DataSource.DroneCharges)
            {
                if (droneCharge.DroneId == id)
                {
                    return droneCharge;
                }
            }
            DroneCharge m_droneCharge = new DroneCharge();
            return m_droneCharge;
        }
        //////////////////////////////////////////////////////////////////
        /// Display all
        //////////////////////////////////////////////////////////////////
        public IEnumerable<Station> displayStations()
        {
            foreach(Station station in DataSource.Stations)
            {
                yield return station;
            }
        }
        public IEnumerable<Drone> displayDrone()
        {
            foreach (Drone drone in DataSource.Drones)
            {
                yield return drone;
            }

        }
        public IEnumerable<Parcel> displayParcels()
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                yield return parcel;
            }
        }
        public IEnumerable<Customer> displayCustomers()
        {
            foreach (Customer customer in DataSource.Customers)
            {
                yield return customer;
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///*כאן יהיו מתודות הוספה, עדכון, וכו
        // */

    }
}
  


