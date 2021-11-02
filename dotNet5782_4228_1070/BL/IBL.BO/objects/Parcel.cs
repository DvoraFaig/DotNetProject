using System;
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
	
                //this.Id = id;
                //this.SenderId = Serderid;
                //this.TargetId = TargetId;
                //this.Weight = Weight;
                //this.Priority = Priority;
                //this.Requeasted = requestedTime;
                //this.DroneId = -1;
            //public int Id { get; set; }
            //public int SenderId { get; set; }
            //public int TargetId { get; set; }
            //public WeightCategories Weight { get; set; }
            //public Priorities Priority { get; set; }
            //public int DroneId { get; set; }
            //public DateTime Requeasted { get; set; } //prepare a parcel to delivery
            //public DateTime Scheduled { get; set; } //pair a parcel to drone
            //public DateTime PickUp { get; set; }
            //public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return ($"parcel ID: {parcel.Id}, parcel SenderId: {parcel.SenderId}, parcel TargetId: {parcel.TargetId}, parcel Priority: {parcel.Priority}, parcel weight: {parcel.Weight}, parcel Requeasted: {parcel.Requeasted}, parcel DroneId {parcel.DroneId}, parcel scheduled {parcel.Scheduled}, parcel pickUp{parcel.PickUp}, parcel delivered: {parcel.Delivered}\n");
            }
        }
    }
}