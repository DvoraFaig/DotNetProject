﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;



namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public int amountParcels()
        {
            return DataSource.Parcels.Count();
        }
        public void AddParcelToDelivery(int id, int Serderid, int TargetId, IDal.DO.WeightCategories Weight, IDal.DO.Priorities Priority, DateTime requestedTime)
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
        public void AddParcelToDelivery(Parcel parcel)
        {
            DataSource.Parcels.Add(parcel);
        }
        public Parcel getParcelByDroneId(int droneId)
        {
           return DataSource.Parcels.Find(p => p.DroneId == droneId );
        }
        public void changeParcelInfo(Parcel p)
        {
            Parcel pToChange = getParcelById(p.Id);
            pToChange = p;
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
        public static Parcel getParcelById(int id)
        {
            try {
                return DataSource.Parcels.FirstOrDefault(parcel => parcel.Id == id);
            }
            catch (Exception e)
            {
                throw new DalExceptions.ObjNotExistException<Parcel>("parcel", id);
            }
        }
    }
}
