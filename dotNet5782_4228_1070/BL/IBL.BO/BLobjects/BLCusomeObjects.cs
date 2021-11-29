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
        public class BLCustomer
        {
            public int ID
            {
                get;set;
                //{
                //    if (value > 99999999 && value < 1000000000)
                //        ID = value;
                //    else 
                //        throw new Exception("Id not in the right lenght");
                //}
            }
            public string Name { get; set; }
            public string Phone { get; set; }
            public BLPosition CustomerPosition { get; set; }
            public List<BLParcelAtCustomer> CustomerAsSender { get; set; }
            public List<BLParcelAtCustomer> CustomerAsTarget { get; set; }
            public override string ToString()
            {
                return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, \n\tCustomerPosition: {CustomerPosition.ToString()}"+
                  $"\tCustomerAsSenderAmount:  { CustomerAsSender.Count()}\n\tCustomerAsTargetAmount: {CustomerAsTarget.Count()}\n" );
            }
        }

        public class BLCustomerToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int AmountSendingDeliveredParcels { get; set; }
            public int AmountSendingUnDeliveredParcels { get; set; }
            public int AmountReceivingParcels { get; set; }
            public int AmountReceivingUnDeliveredParcels { get; set; }
            //public BLPosition CustomerPosition { get; set; }
            //public List<BLDeliveryAtCustomer> deliveryfromCustomers { get; set; }
            //public List<BLDeliveryAtCustomer> deliveryToCustomers { get; set; }
        }

        public class BLCustomerInParcel //targetId in parcel
        {
            public int Id { get; set; }
            public string name { get; set; }
            public override string ToString()
            {
                return ($"Id: {Id} , Name: {name} \n");
            }
        }
    }
}
