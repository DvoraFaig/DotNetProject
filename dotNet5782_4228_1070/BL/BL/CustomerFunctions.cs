using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {
        /// <summary>
        /// Add a new customer. checks if this customer exist already.
        /// If exist throw an error
        /// If doesn't exist send if it to a func to add
        /// </summary>
        /// <param name="customerToAdd">The new customer to add.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(BO.Customer customerToAdd)
        {
            lock (dal)
            {
                try
                {
                    DO.Customer customer = dal.getCustomerWithSpecificCondition(c => c.Id == customerToAdd.Id).First();
                    if (customer.IsActive)
                        throw new ObjExistException(typeof(BO.Customer), customer.Id);
                    customer.IsActive = true;
                    dal.changeCustomerInfo(customer);

                    string message = "";
                    if (customerToAdd.CustomerPosition.Latitude != customer.Latitude || customerToAdd.CustomerPosition.Longitude != customer.Longitude)
                        message = "Position";
                    if (customerToAdd.Name != customer.Name)
                        message += ", Name";
                    if (customerToAdd.Phone != customer.Phone)
                        message += "and Phone";
                    if (message != "")
                        throw new Exceptions.DataOfOjectChanged(typeof(Customer), customer.Id, $"Data changed: {message} was changed");
                    return;
                }
                catch (ArgumentNullException) { }
                catch (InvalidOperationException) { }
                dal.AddCustomer(convertBLToDalCustomer(customerToAdd));
            }
        }

        /// <summary>
        /// Returns a IEnumerable<CustomerToList> by recieving customers and converting them to CustomerToList
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomersToList()
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerToList> customerToLists = new List<CustomerToList>();
            return from customer in customers
                   select converteCustomerToList(customer);
        }

        /// <summary>
        /// Return a List<CustomerInParcel>
        /// </summary>
        /// <param name="customerInParcel"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<CustomerInParcel> GetLimitedCustomersList(CustomerInParcel customerInParcel = null)
        {
            IEnumerable<DO.Customer> customers = dal.GetCustomers();
            List<CustomerInParcel> customerToLists = new List<CustomerInParcel>();
            foreach (var customer in customers)
            {
                customerToLists.Add(convertDalToBLCustomerInParcel(customer));
            }
            if (customerInParcel != null)
            {
                customerInParcel = customerToLists.SingleOrDefault(c => c.Id == customerInParcel.Id);
                customerToLists.Remove(customerInParcel);
            }
            return customerToLists;
        }

        /// <summary>
        /// Return a BO.Customer(converted) by an id from dal.getCustomerWithSpecificCondition
        /// </summary>
        /// <param name="customerRequestedId">The id of the customer that's requested</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerById(int customerRequestedId)
        {
            DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId).First();
            Customer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        /// <summary>
        /// Return a BO.Customer(converted) by id and name from dal.getCustomerWithSpecificCondition
        /// </summary>
        /// <param name="customerRequestedId">The id of the customer that requested<</param>
        /// <param name="customerRequestedName">The name of the customer that requested<</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerByIdAndName(int customerRequestedId, string customerRequestedName)
        {
            DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId && c.Name == customerRequestedName).First();
            if (c.IsActive == false)
            {
                c.IsActive = true;
                dal.AddCustomer(c);
            }
            Customer BLcustomer = convertDalToBLCustomer(c);
            return BLcustomer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Customer UpdateCustomerDetails(int id, string name = null, string phone = null)
        {
            DO.Customer c;
            try
            {
                c = dal.getCustomerWithSpecificCondition(c => c.Id == id).First();
                if (name != null)
                    c.Name = name;
                if (phone != null && phone.Length >= 9 && phone.Length <= 10)
                    c.Phone = phone;
                dal.changeCustomerInfo(c);
                return (convertDalToBLCustomer(c));
            }
            catch (Exception e)
            {
                throw new ObjNotExistException(typeof(DO.Customer), id, e);
            }
        }

        /// <summary>
        /// Remove specific customer
        /// </summary>
        /// <param name="parcelToRemove">remove current customer</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(int customerId)
        {
            try
            {
                Customer customer = GetCustomerById(customerId);
                if (dal.IsCustomerActive(customer.Id))
                    lock (dal)
                    {
                        dal.removeCustomer(convertBLToDalCustomer(customer));
                    }
                else
                    throw new Exceptions.ObjExistException(typeof(Customer), customer.Id, "is active");
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
            catch (DO.Exceptions.NoMatchingData e1)
            {
                throw new Exceptions.NoDataMatchingBetweenDalandBL(e1.Message);
            }
        }
    }
}
