using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;



namespace BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int AmountAsSendingDeliveredParcels { get; set; }
        public int AmountAsSendingUnDeliveredParcels { get; set; }
        public int AmountAsTargetDeliveredParcels { get; set; }
        public int AmountAsTargetUnDeliveredParcels { get; set; }
    }
 
}

