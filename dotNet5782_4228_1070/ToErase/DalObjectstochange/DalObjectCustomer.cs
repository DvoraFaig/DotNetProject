using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;


namespace DalObject
{
    public partial class DalObject 
    {
        
        //Customers//
        public static int amountCustomers()
        {
            return DataSource.Customers.Count;
        }
        public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
        {
            Customer c = new Customer(id, Name, Phone, Latitude, Longitude);
            DataSource.Customers.Add(c);
            //customer.ID = id;
            //customer.Name = Name;
            //customer.Phone = Phone;
            //customer.Longitude = Longitude;
            //customer.Latitude = Latitude;

        }
        public IEnumerable<Customer> displayCustomers()
        {
            foreach (Customer customer in DataSource.Customers)
            {
                if (customer.ID != 0)
                {
                    yield return customer;
                }
            }
        }
        public static Customer getCustomerById(int id)
        {
            return DataSource.Customers.FirstOrDefault(customer => customer.ID == id);
        }
    }
}
