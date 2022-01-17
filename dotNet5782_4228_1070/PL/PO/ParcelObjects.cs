﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Windows;

namespace PO
{
    public class Parcel : DependencyObject
    {
        public Parcel(BO.Parcel p)
        {
            Id = p.Id;
            Sender = p.Sender;
            Target = p.Target;
            Weight = (WeightCategories)p.Weight;
            Priority = (Priorities)p.Priority;
            Drone = p.Drone;
            Requeasted = p.Requeasted;
            Scheduled = p.Scheduled;
            PickUp = p.PickUp;
            Delivered = p.Delivered;
        }
        public Parcel()
        {
                
        }
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public BO.CustomerInParcel Sender
        {
            get { return (BO.CustomerInParcel)GetValue(SenderProperty); }
            set { SetValue(SenderProperty, value); }
        }
        public BO.CustomerInParcel Target
        {
            get { return (BO.CustomerInParcel)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        public WeightCategories Weight
        {
            get { return (WeightCategories)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }
        public Priorities Priority
        {
            get { return (Priorities)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }
        //public WeightCategories Weight { get; set; }
        //public Priorities Priority { get; set; }
        public BO.DroneInParcel Drone
        {
            get { return (BO.DroneInParcel)GetValue(DroneProperty); }
            set { SetValue(DroneProperty, value); }
        }
        public DateTime? Requeasted
        {
            get { return (DateTime?)GetValue(RequeastedProperty); }
            set { SetValue(RequeastedProperty, value); }
        } //prepare a parcel to delivery
        public DateTime? Scheduled
        {
            get { return (DateTime?)GetValue(ScheduledProperty); }
            set { SetValue(ScheduledProperty, value); }
        } //pair a parcel to drone
        public DateTime? PickUp
        {
            get { return (DateTime?)GetValue(PickUpProperty); }
            set { SetValue(PickUpProperty, value); }
        }
        public DateTime? Delivered
        {
            get { return (DateTime?)GetValue(DeliveredProperty); }
            set { SetValue(DeliveredProperty, value); }
        }
        public override string ToString()
        {
            //string notFilled = "not filled";
            return ($"parcel ID: {Id}, \n\tSender: {Sender.ToString()}, \tTarget: {Target.ToString()}, \tparcel Priority: /*Priority*/, parcel weight: /*Weight*/,\n" +
                $"\n\tDrone: {(!Drone.Equals(default(DroneInParcel)) ? Drone.ToString() : (char)'-') } " +
                $"\n\tparcel Requeasted: {(Requeasted != null ? Requeasted : (char)'-')}, parcel scheduled: {(Scheduled != null ? Scheduled : (char)'-')}, parcel pickUp: {(PickUp != null ? PickUp : (char)'-')}, parcel delivered: {(Delivered != null ? Delivered : (char)'-')}\n");
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty SenderProperty = DependencyProperty.Register("Sender", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DroneProperty = DependencyProperty.Register("Drone", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty RequeastedProperty = DependencyProperty.Register("Requeasted", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty ScheduledProperty = DependencyProperty.Register("Scheduled", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty PickUpProperty = DependencyProperty.Register("PickUp", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty DeliveredProperty = DependencyProperty.Register("Delivered", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty WeightProperty = DependencyProperty.Register("Weight", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register("Priority", typeof(object), typeof(Parcel), new UIPropertyMetadata(0));
    }

    public class ParcelToList : DependencyObject
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public DO.WeightCategories Weight { get; set; }
        public DO.Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
    }

    public class ParcelAtCustomer : DependencyObject
    {
        public int Id { get; set; }
        //public WeightCategories Weight { get; set; }
        public DO.Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
        public CustomerInParcel SenderOrTargetCustomer { get; set; }
        public override string ToString()
        {
            return ($"Id: {Id} , Weight: /*Weight*/ , Priority: {Priority}, ParcelStatus: {ParcelStatus},{SenderOrTargetCustomer}\n");
        }
    }

    public class ParcelInTransfer : DependencyObject
    {
        public int Id { get; set; }
        /// <summary>
        /// Awaiting collection =false\ On the way to the destination=true
        /// </summary>
        public bool parcelStatus { get; set; }
        //public WeightCategories Weight { get; set; }
        //public Priorities Priority { get; set; }
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel TargetCustomer { get; set; }
        public Position SenderPosition { get; set; }
        public Position TargetPosition { get; set; }
        public double distance { get; set; }
        //public override string ToString()
        //{
        //   return ($"id: {Id} , parcelStatus: {parcelStatus},Weight: {Weight}, parcelPriority: {Priority},\n\tSenderCustomer:{SenderCustomer}, \tTargetCustomer :{TargetCustomer}, \tSenderPosition: {SenderPosition}, \tTargetPosition: {TargetPosition}, distance: {distance} ");
        //}
    }
}

