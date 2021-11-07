using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;

//namespace BL.IBL.BO.objects
namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public IDal.DO.Customer customer;
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
        }
    }
}