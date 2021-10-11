using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class Parcel
    {
        public int Id { get; set; }
        public int Serderid { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public Datatime Requeasted { get; set; }
        public int DroneId { get; set; }
        public Datatime Scheduled { get; set; }
        public Datatime PickUp { get; set; }
        public Datatime Delivered { get; set; }
    }
}
