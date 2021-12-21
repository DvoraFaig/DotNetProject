using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;



namespace BO
{
    public class Customer
    {
        public int ID
        {
            get; set;
        }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Position CustomerPosition { get; set; }
        public List<ParcelAtCustomer> CustomerAsSender { get; set; }
        public List<ParcelAtCustomer> CustomerAsTarget { get; set; }
        public override string ToString()
        {
            return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, \n\tCustomerPosition: {CustomerPosition.ToString()}" +
              $"\tCustomerAsSenderAmount:  { CustomerAsSender.Count()}\n\tCustomerAsTargetAmount: {CustomerAsTarget.Count()}\n");
        }
    }

    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int AmountAsSendingDeliveredParcels { get; set; }
        public int AmountASSendingUnDeliveredParcels { get; set; }
        public int AmountAsTargetHaveDeliveredParcels { get; set; }
        public int AmountAsTargetHaveUnDeliveredParcels { get; set; }
    }

    public class CustomerInParcel //targetId in parcel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return ($"Id: {Id} , Name: {name} \n");
        }
    }
}

