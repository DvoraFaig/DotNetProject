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
        
        public class Customer
        {
            IDal.DO.Customer customer;
            public Customer(int id, string Name, string Phone, double Longitude, double Latitude)
            {
                customer = new IDal.DO.Customer();
                customer.ID = id;
                customer.Name = Name;
                customer.Phone = Phone;
                customer.Longitude = Longitude;
                customer.Latitude = Latitude;
                //upadate the list
                //רשימת משלוחים אצל לקוח מהלקוח
            }

            public override string ToString()
            {
                return ($"customer id: {customer.ID}, customer name: {customer.Name}, customer phone: {customer.Phone}, customer latitude: {customer.Latitude}, customer Longitude: {customer.Longitude}\n");
            }

            //public int ID { get; set; }
            //public string Name { get; set; }
            //public string Phone { get; set; }
            //public double Longitude { get; set; }
            //public double Latitude { get; set; }

            //public Customer(int id, string Name, string Phone, double Longitude, double Latitude)
            //{
            //    this.ID = id;
            //    this.Name = Name;
            //    this.Phone = Phone;
            //    this.Longitude = Longitude;
            //    this.Latitude = Latitude;
            //}
        }
    }
}



