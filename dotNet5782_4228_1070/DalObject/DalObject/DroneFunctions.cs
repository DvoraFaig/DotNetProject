﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {
        /// <summary>
        /// Add the new drone to Drones.
        /// </summary>
        /// <param name="newDrone">drone to add.</param>
        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add(newDrone);
        }

        /// <summary>
        /// Get all drone.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> GetDrones()
        {
            return from d in DataSource.Drones
                   where d.IsActive == true
                   select d;
        }

        /// <summary>
        /// Get a Drone/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a drone/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Drone> getDroneWithSpecificCondition(Predicate<Drone> predicate)
        {
            return (from drone in DataSource.Drones
                    where predicate(drone)
                    select drone);
        }

        /// <summary>
        /// Change specific drone info
        /// </summary>
        /// <param name="droneWithUpdateInfo">Drone with the changed info</param>
        public void changeDroneInfo(Drone droneWithUpdateInfo)
        {
            int index = DataSource.Drones.FindIndex(d => d.Id == droneWithUpdateInfo.Id);
            DataSource.Drones[index] = droneWithUpdateInfo;
        }

        /// <summary>
        /// if drone exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.</summary>
        /// <param name="droneToRemove">The drone to remove. droneToRemove.IsActive = false</param>
        public void removeDrone(Drone droneToRemove)
        {
            try
            {
                Drone drone = (from d in DataSource.Drones
                               where d.Id == droneToRemove.Id
                              && d.Model == droneToRemove.Model
                              && d.MaxWeight == droneToRemove.MaxWeight
                               select d).First();
                drone.IsActive = false;
                changeDroneInfo(drone);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Drone), droneToRemove.Id, e1);
            }
        }

        /// <summary>
        /// Returns electricity usage of drone.
        /// </summary>
        /// <returns></returns>
        public double[] electricityUseByDrone()
        {
            double[] arr = {
                DataSource.Config.empty,
                DataSource.Config.lightWeight,
                DataSource.Config.mediumWeight,
                DataSource.Config.heavyWeight,
                DataSource.Config.chargingRate };
            return arr;
        }

        /// <summary>
        /// If drone with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        public Boolean IsDroneById(int requestedId)
        {
            return DataSource.Drones.Any(d => d.Id == requestedId);
        }

        /// <summary>
        /// If drone with the requested id exist and active
        /// </summary>
        /// <param name="requestedId">Looking for drone with this id</param>
        /// <returns></returns>
        public Boolean IsDroneActive(int requestedId)
        {
            return DataSource.Drones.Any(d => d.Id == requestedId && d.IsActive);
        }
    }
}