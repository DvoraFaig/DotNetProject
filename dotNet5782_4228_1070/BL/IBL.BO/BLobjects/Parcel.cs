/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;


namespace IBL
{
    namespace BO
    { 
        public class Parcel
        {
            IDal.DO.Parcel parcel;
            public Parcel (int id, int Serderid, int TargetId, WeightCategories Weight, Priorities Priority, DateTime requestedTime)
            {
                parcel = new IDal.DO.Parcel();
                parcel.Id = id;
                parcel.SenderId = Serderid;
                parcel.TargetId = TargetId;
                parcel.Weight = Weight;
                parcel.Priority = Priority;
                parcel.Requeasted = requestedTime;
                parcel.DroneId = -1;
            }
	        
            public override string ToString()
            {
                return ($"parcel ID: {parcel.Id}, parcel SenderId: {parcel.SenderId}, parcel TargetId: {parcel.TargetId}, parcel Priority: {parcel.Priority}, parcel weight: {parcel.Weight}, parcel Requeasted: {parcel.Requeasted}, parcel DroneId {parcel.DroneId}, parcel scheduled {parcel.Scheduled}, parcel pickUp{parcel.PickUp}, parcel delivered: {parcel.Delivered}\n");
            }
        }
    }
}*/