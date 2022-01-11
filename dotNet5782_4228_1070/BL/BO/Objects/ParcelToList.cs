using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;



namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public DO.WeightCategories Weight { get; set; }
        public DO.Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatus { get; set; }
    }
}

