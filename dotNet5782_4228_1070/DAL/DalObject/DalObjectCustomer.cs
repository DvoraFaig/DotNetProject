using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;





namespace DalExceptions
{
    public partial class DalObject : IDal.DO.IDal
    {
        public int amountCustomers()
        {
            return DataSource.Customers.Count;
        }
        public void AddCustomer(int id, string Name, string Phone, double Longitude, double Latitude)
        {
            Customer customer = new Customer();
            customer.ID = id;
            customer.Name = Name;
            customer.Phone = Phone;
            customer.Longitude = Longitude;
            customer.Latitude = Latitude;
            DataSource.Customers.Add(customer);
        }
        public void AddCustomer(Customer customer)
        {
            DataSource.Customers.Add(customer);
        }
        public void changeCustomerInfo(Customer c)
        {
            Customer cToChange = getCustomerById(c.ID);
            cToChange = c;
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
        public Customer getCustomerById(int id)
        {
            try
            {
                return DataSource.Customers.First(customer => customer.ID == id);
            }
            catch (InvalidOperationException e)
            {
                throw new IDal.DO.DalExceptions.ObjNotExistException(typeof(Customer), id);
            }
        }
    }
}