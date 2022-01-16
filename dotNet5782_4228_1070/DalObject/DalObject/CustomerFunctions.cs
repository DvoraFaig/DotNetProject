using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {

        /// <summary>
        /// Add the new customer to Customers.
        /// </summary>
        /// <param name="newCustomer">customer to add.</param>
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            return from c in DataSource.Customers
                   where c.IsActive == true
                   select c;
        }

        /// <summary>
        /// Get a Customer/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a customer/s that meeets the condition</param>
        /// <returns></returns>
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate)
        {
            return (from Customer in DataSource.Customers
                    where predicate(Customer)
                    select Customer);
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer customerWithUpdateInfo)
        {
            int index = DataSource.Customers.FindIndex(d => d.Id == customerWithUpdateInfo.Id);
            DataSource.Customers[index] = customerWithUpdateInfo;
        }

        /// <summary>
        /// if customer exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="customerToRemove">The customer to remove. customerToRemove.IsActive = false</param>
        public void removeCustomer(Customer customerToRemove)
        {
            try
            {
                Customer customer = (from c in DataSource.Customers
                                     where c.Id == customerToRemove.Id
                                    && c.Name == customerToRemove.Name
                                    && c.Phone == customerToRemove.Phone
                                    && c.Latitude == customerToRemove.Latitude
                                    && c.Longitude == customerToRemove.Longitude
                                     select c).First();
                customer.IsActive = false;
                changeCustomerInfo(customer);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Customer), customerToRemove.Id, e1);
            }
        }

        /// <summary>
        /// If customer with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for customer with this id</param>
        /// <returns></returns>
        public Boolean IsCustomerById(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId);
        }

        /// <summary>
        /// If customer with the requested id exist and Active
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns></returns>
        public Boolean IsCustomerActive(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId && c.IsActive == true);
        }
    }
}


