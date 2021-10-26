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
            return DataSource.Stations.Count();
        }
        public static int amountDrones()
        {
            return DataSource.Drones.Count();
        }
        public static int amountParcels()
        {
            return DataSource.Parcels.Count();
        }
        public static int amountCustomers()
        {
            return DataSource.Customers.Count;
        }
        public static int amountDroneCharges()
        {
            return DataSource.DroneCharges.Count();
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
            DataSource.Drones.Add(drone);
        }
        public void AddStation(int id, string Name, int ChargeSlots, double Longitude, double Latitude)
        {
            Station station = new Station();
            station.Id = id;
            station.Name = Name;
            station.ChargeSlots = ChargeSlots;
            station.Longitude = Longitude;
            station.Latitude = Latitude;
            DataSource.Stations.Add(station);

        }
        public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
        {
            Customer customer = new Customer();
            customer.ID = id;
            customer.Name = Name;
            customer.Phone = Phone;
            customer.Longitude = Longitude;
            customer.Latitude = Latitude;
            DataSource.Customers.Add(customer);

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
            DataSource.Parcels.Add(parcel);

        }

        //Update functions//
        //find an availeble drone and send it to the sending costumer.
        //Pair between a customer and parcel to a drone;
        /*public void assignParcelToDrone(int parcelId)
        {
            Parcel p = getParcelById(parcelId);
            for (int i = 0; i < DataSource.Drones.Count; i++)
            {
                if (DataSource.Drones[i].Status == DroneStatus.Available)
                {

                }
            }
            foreach(Drone drone in DataSource.Drones)
            {
                if (drone.Status == DroneStatus.Available)
                {
                    
                }
            }
            
        }*/

/*        private bool isFreeDrone(Drone d)
        {
            if (d.Status == DroneStatus.Available)
            {
                return true;
            }
            return false;
        }*/
        public string PairAParcelWithADrone(Parcel parcel)
        { 
            for (int i = 0; i < DataSource.Drones.Count())
            {
                if (DataSource.Drones[i].Status == DroneStatus.Available && (WeightCategories)DataSource.Drones[i].MaxWeight >= parcel.Weight)
                {

                }

            }
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
            return DataSource.Stations.FirstOrDefault(station => station.Id == id);
        }
        public static Drone getDroneById(int id)
        {
            return DataSource.Drones.FirstOrDefault(drone => drone.Id == id);
        }
        public static Customer getCustomerById(int id)
        {
            return DataSource.Customers.FirstOrDefault(customer => customer.ID == id);
        }
        public Parcel getParcelById(int id)
        {
            return DataSource.Parcels.FirstOrDefault(parcel => parcel.Id == id);
        }
        public static DroneCharge getDroneChargeById(int id)
        {
            return DataSource.DroneCharges.FirstOrDefault(charge => charge.StationId == id);
        }
        public static DroneCharge getDroneChargeByDroneId(int id)
        {
            return DataSource.DroneCharges.FirstOrDefault(charge => charge.DroneId == id);
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
  


