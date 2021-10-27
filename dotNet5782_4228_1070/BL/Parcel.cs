﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime Requeasted { get; set; } //prepare a parcel to delivery
        public DateTime Scheduled { get; set; } //pair a parcel to drone
        public DateTime PickUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            return ($"parcel ID: {Id}, parcel SenderId: {SenderId}, parcel TargetId: {TargetId}, parcel Priority: {Priority}, parcel weight: {Weight}, parcel Requeasted: {Requeasted}, parcel DroneId {DroneId}, parcel scheduled {Scheduled}, parcel pickUp{PickUp}, parcel delivered: {Delivered}\n");
        }
    }
}