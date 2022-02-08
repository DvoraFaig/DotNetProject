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
    sealed partial class BL
    {
        public Action<Customer> CustomerChangeAction { get; set; }

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
                DO.Customer cToChange = new DO.Customer();
                try
                {
                    #region If Customer.IsActive == false. What was changed when drone was added-> Customer.IsActive == true;
                    try
                    {
                        cToChange = dal.getCustomerWithSpecificCondition(c => c.Id == customerToAdd.Id).First();
                    }
                    catch (Exception) { }
                    #endregion

                    dal.AddCustomer(convertBLToDalCustomer(customerToAdd));
                }
                catch (DO.Exceptions.DataChanged)
                {
                    string message = messageDataChanged(cToChange, customerToAdd);

                    if (message != "")
                        throw new Exceptions.DataChanged(typeof(Customer), cToChange.Id, $"Data changed: {message} was changed");
                }
                catch (DO.Exceptions.ObjExistException)
                {
                    throw new ObjExistException(typeof(Customer), customerToAdd.Id);
                }
            }

            #region erase
            //try
            //{

            //    DO.Customer customer = dal.getCustomerWithSpecificCondition(c => c.Id == customerToAdd.Id).First();
            //    if (customer.IsActive)
            //        throw new ObjExistException(typeof(BO.Customer), customer.Id);
            //    customer.IsActive = true;
            //    dal.changeCustomerInfo(customer);

            //    string message = "";
            //    if (customerToAdd.CustomerPosition.Latitude != customer.Latitude || customerToAdd.CustomerPosition.Longitude != customer.Longitude)
            //        message = "Position";
            //    if (customerToAdd.Name != customer.Name)
            //        message += ", Name";
            //    if (customerToAdd.Phone != customer.Phone)
            //        message += "and Phone";
            //    if (message != "")
            //        throw new Exceptions.DataOfOjectChanged(typeof(Customer), customer.Id, $"Data changed: {message} was changed");
            //    return;
            //}
            //catch (ArgumentNullException) { }
            //catch (InvalidOperationException) { }
            //dal.AddCustomer(convertBLToDalCustomer(customerToAdd));
            //CustomerChangeAction?.Invoke(customerToAdd);
            #endregion
        }

        /// <summary>
        /// Return dif between the changed and unchanges customer
        /// </summary>
        /// <param name="cToChange"></param>
        /// <param name="cWithChange"></param>
        /// <returns></returns>
        private string messageDataChanged(DO.Customer cToChange, Customer cWithChange)
        {
            string message = "";
            if (cWithChange.CustomerPosition.Latitude != cToChange.Latitude || cWithChange.CustomerPosition.Longitude != cToChange.Longitude)
                message = "Position";
            if (cWithChange.Name != cToChange.Name)
                message += ", Name";
            if (cWithChange.Phone != cToChange.Phone)
                message += "and Phone";
            return message;
        }

        /// <summary>
        /// Returns a IEnumerable<CustomerToList> by recieving customers and converting them to CustomerToList
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomersToList()
        {
            lock (dal)
            {
                IEnumerable<DO.Customer> customers = dal.GetCustomers();
                return from customer in customers
                       select converteCustomerToList(customer);
            }
        }

        ///// <summary>
        ///// Return a List<CustomerInParcel>
        ///// </summary>
        ///// <param name="customerInParcel"></param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        //public List<CustomerInParcel> GetLimitedCustomersList(CustomerInParcel customerInParcel = null)
        //{
        //    lock (dal)
        //    {
        //        IEnumerable<DO.Customer> customers = dal.GetCustomers();
        //        customerInParcel = (customerInParcel == null) ? new CustomerInParcel() : customerInParcel;

        //        return (from c in customers
        //                where customerInParcel.Id != c.Id
        //                select convertDalToBLCustomerInParcel(c)).ToList();

        //        //List<CustomerInParcel> customerToLists = new List<CustomerInParcel>();
        //        //foreach (var customer in customers)
        //        //{
        //        //    customerToLists.Add(convertDalToBLCustomerInParcel(customer));
        //        //}             

        //        //if (customerInParcel != null)
        //        //{
        //        //    customerInParcel = customerToLists.SingleOrDefault(c => c.Id == customerInParcel.Id);
        //        //    customerToLists.Remove(customerInParcel);
        //        //}
        //        //return customerToLists;
        //    }
        //}


        /// <summary>
        /// Return a List<CustomerInParcel>
        /// </summary>
        /// <param name="customerInParcel"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerInParcel> GetLimitedCustomersList(int customerId = 0)
        {
            lock (dal)
            {
                IEnumerable<DO.Customer> customers = dal.GetCustomers();

                return (from c in customers
                        where customerId != c.Id
                        select convertDalToBLCustomerInParcel(c));
            }
        }

        /// <summary>
        /// Return a BO.Customer(converted) by an id from dal.getCustomerWithSpecificCondition
        /// </summary>
        /// <param name="customerRequestedId">The id of the customer that's requested</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomerById(int customerRequestedId)
        {
            lock (dal)
            {
                DO.Customer c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId).First();
                Customer customer = convertDalToBLCustomer(c);
                return customer;
            }
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
            lock (dal)
            {
                DO.Customer c;
                try
                {
                    c = dal.getCustomerWithSpecificCondition(c => c.Id == customerRequestedId && c.Name == customerRequestedName).First();
                }
                catch (Exception e) { throw new Exceptions.ObjNotExistException(typeof(Customer), customerRequestedId); }

                if (c.IsActive == false)
                    dal.AddCustomer(c);

                return convertDalToBLCustomer(c);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Customer UpdateCustomerDetails(int id, string name = null, string phone = null)
        {
            lock (dal)
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
                    Customer customer = convertDalToBLCustomer(c);
                    CustomerChangeAction?.Invoke(customer);
                    return (customer);
                }
                catch (Exception e)
                {
                    throw new ObjNotExistException(typeof(DO.Customer), id, e);
                }
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
                lock (dal)
                {
                    //Customer customer = GetCustomerById(customerId);
                    //if (dal.IsCustomerActive(customer.Id))
                    dal.removeCustomer(dal.getCustomerWithSpecificCondition(c => c.Id == customerId).First());
                    //else
                    //    throw new Exceptions.ObjExistException(typeof(Customer), customer.Id, "is active");
                }
            }
            catch (DO.Exceptions.ObjNotExistException e1)
            {
                throw new Exceptions.ObjNotExistException(typeof(Customer), customerId, e1);
            }
        }
    }
}
