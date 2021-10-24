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
        //returnnig amount functions//
        public static int amountStations()
        {
            return DataSource.Config.indexStations;
        }
        public static int amountDrones()
        {
            return DataSource.Config.indexDrones;
        }
        public static int amountParcels()
        {
            return DataSource.Config.indexParcels;
        }
        public static int amountCustomers()
        {
            return DataSource.Config.indexCustomers;
        }
        public static int amountDroneCharges()
        {
            return DataSource.Config.indexDroneCharges;
        }

        //Add functions//
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
        public void AddParcelToDelivery(int id, int Serderid, int TargetId,IDAL.DO.WeightCategories Weight, IDAL.DO.Priorities Priority, DateTime requestedTime)
        {
            Parcel parcel = new Parcel();
            parcel.Id = id;
            parcel.SenderId = Serderid;
            parcel.TargetId = TargetId;
            parcel.Weight = Weight;
            parcel.Priority = Priority;
            parcel.Requeasted = requestedTime;
            parcel.DroneId = -1;
            DataSource.Parcels[DataSource.Config.indexParcels++] = parcel;

        }

        //Update functions//
        //find an availeble drone and send it to the sending costumer.
        //Pair between a customer and parcel to a drone;
        public void assignParcelToDrone(int parcelId)
        {
            Parcel p = getParcelById(parcelId);
            foreach(Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available)
                {
                    
                }
            }
            
        }

        private bool isFreeDrone(Drone d)
        {
            if (d.Status == DroneStatus.Available)
            {
                return true;
            }
            return false;
        }
        public string PairAParcelWithADrone(Parcel parcel)
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available && (WeightCategories)drone.MaxWeight >= parcel.Weight)
                {
                    parcel.DroneId = drone.Id;
                    parcel.Scheduled = DateTime.Now; //pair a parcel to dron
                    int indexDrone = drone.Id;
                    DataSource.Drones[indexDrone].Status = DroneStatus.Delivery; // can't change info by foreach - drone.Status = DroneStatus.Delivery;
                    return $"The Drone number{drone.Id} is ready and will receive parcel num {parcel.Id}.";
                }
            }
            return ("No drones available.\n please try later.");
        }

        public void DroneCollectsAParcel(Parcel parcel)
        {
            if(parcel.DroneId == 0) //doesn't have a Drone
            {
                Console.WriteLine("Error! The parcel doesn't have a Drone.\n Please enter DroneReceivesParcel");
            }
            else
            {
                parcel.PickUp = DateTime.Now;
                Drone droneCollect = DalObject.getDroneById(parcel.DroneId);
                droneCollect.Status = DroneStatus.Delivery;
            }
        }
        public void CostumerGetsParcel(Drone drone, Parcel parcel)
        {
            parcel.Delivered = DateTime.Now;
            drone.Status = DroneStatus.Available;
        }
        public void sendDroneToCharge(Drone drone)
        {
            IEnumerable<DroneCharge> droneCharges = displayDroneCharge();
            Console.WriteLine("Choose id of Station to charge The drone");
            foreach (DroneCharge charge in droneCharges)
            {
                Console.WriteLine(charge.ToString());
            }
            int choose = Convert.ToInt32(Console.ReadLine());
            DroneCharge droneCharge = getDroneChargeById(choose);
            if (droneCharge.StationId != -1)
            {
                drone.Status = DroneStatus.Maintenance;
                droneCharge.DroneId = drone.Id;
            }
        }
        public void freeDroneFromCharge(Drone drone)
        {
            drone.Status = DroneStatus.Available;
            drone.Battery = 100;
            DroneCharge chargeToFree = getDroneChargeByDroneId(drone.Id);
            chargeToFree.StationId = -1;
        }
        
        // Display functions//
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
                if (droneCharge.StationId == id)
                {
                    return droneCharge;
                }
            }
            DroneCharge m_droneCharge = new DroneCharge();
            return m_droneCharge;
        }
        public static DroneCharge getDroneChargeByDroneId(int id)
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
        // Display all functions //
        public IEnumerable<Station> displayStations()
        {
            foreach (Station station in DataSource.Stations)
            {
                if (station.Id != 0)
                {
                    yield return station;
                }
            }
        }
        public IEnumerable<Drone> displayDrone()
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id != 0)
                {
                    yield return drone;
                }
            }
        }
        public IEnumerable<Parcel> displayParcels()
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id != 0)
                {
                    yield return parcel;
                }
            }
        }
        public IEnumerable<Parcel> displayFreeParcels()
        {
            foreach (Parcel parcel in DataSource.Parcels)
            {
                if (parcel.Id != 0 && parcel.DroneId == -1)
                {
                    yield return parcel;
                }
            }
        }
        public IEnumerable<DroneCharge> displayDroneCharge()
        {
            foreach (DroneCharge droneCharge in DataSource.DroneCharges)
            {
                if (droneCharge.StationId != 0 && droneCharge.DroneId == 0)
                {
                    yield return droneCharge;
                }
            }
        }
        public IEnumerable<Customer> displayCustomers()
        {
            foreach (Customer customer in DataSource.Customers)
            {
                if (customer.ID != 0)
                {
                    yield return customer;
                }
            }
        }
    }
}
  


