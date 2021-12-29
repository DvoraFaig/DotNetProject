using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Add the new parcel to parcels.
        /// </summary>
        /// <param name="newParcel">parcel to add</param>
        public void AddParcel(Parcel newParcel)
        {
            DataSource.Parcels.Add(newParcel);
        }

        /// <summary>
        /// Add the new drone to Drones.
        /// </summary>
        /// <param name="newDrone">drone to add.</param>
        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add(newDrone);
        }

        /// <summary>
        /// Add the new customer to Customers.
        /// </summary>
        /// <param name="newCustomer">customer to add.</param>
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// Add the new DroneCharge to DroneCharges.
        /// </summary>
        /// <param name="newDroneCharge">DroneCharge to add.</param>
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        /// <summary>
        /// Add the new station to Stations.
        /// </summary>
        /// <param name="newStation">The station to add.</param>
        public void AddStation(Station newStation)
        {
            DataSource.Stations.Add(newStation);
        }

    }
}


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