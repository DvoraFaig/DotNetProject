﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {

        /// <summary>
        /// Add the new customer to Customers.
        /// </summary>
        /// <param name="newCustomer">customer to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers.Add(newCustomer);
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers()
        {
            return from c in DataSource.Customers
                   where c.IsActive == true
                   orderby c.Id
                   select c;
        }

        /// <summary>
        /// Get a Customer/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a customer/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate)
        {
            return (from customer in DataSource.Customers
                    where predicate(customer)
                    orderby customer.Id
                    select customer);
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean IsCustomerById(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId);
        }

        /// <summary>
        /// If customer with the requested id exist and Active
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Boolean IsCustomerActive(int requestedId)
        {
            return DataSource.Customers.Any(c => c.Id == requestedId && c.IsActive == true);
        }
    }
}


