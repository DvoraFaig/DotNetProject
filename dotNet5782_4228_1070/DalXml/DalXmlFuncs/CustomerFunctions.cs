using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;
using System.Runtime.CompilerServices;



namespace Dal
{
    public sealed partial class DalXml : DalApi.Idal
    {

        /// <summary>
        /// Add the new customer to Customers.
        /// </summary>
        /// <param name="newCustomer">customer to add.</param>
        public void AddCustomer(Customer newCustomer)
        {
            newCustomer.IsActive = true;
            Customer customer;
            try
            {
                customer = getCustomerWithSpecificCondition(d => d.Id == newCustomer.Id).First();
                if (customer.IsActive)
                    throw new Exceptions.ObjExistException(typeof(Customer), newCustomer.Id);

                changeCustomerInfo(newCustomer);
                throw new Exceptions.DataChanged(typeof(Customer), newCustomer.Id);

            }
            catch (Exception)
            {
                XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
                customerRoot.Add(returnCustomerXElement(newCustomer));
                customerRoot.Save(dir + customerFilePath);
            }

            #region LoadListFromXMLSerializer
            //wasn't change to check if exist
            //IEnumerable<DO.Customer> customersList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //if (customersList.Any(c => c.Id == newCustomer.Id))
            //{
            //    throw new DO.Exceptions.ObjExistException(typeof(DO.Customer), newCustomer.Id);
            //}
            //List<DO.Customer> newList = customersList.Cast<DO.Customer>().ToList();
            //newList.Add(newCustomer);
            //XMLTools.SaveListToXMLSerializer<DO.Customer>(newList, dir + customerFilePath);
            #endregion
        }

        /// <summary>
        /// Receive a DO Customer and return a XElemnt Customer - copy information.
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        private XElement returnCustomerXElement(DO.Customer newCustomer)
        {
            XElement Id = new XElement("Id", newCustomer.Id);
            XElement Name = new XElement("Name", newCustomer.Name);
            XElement Phone = new XElement("Phone", newCustomer.Phone);
            XElement Latitude = new XElement("Latitude", newCustomer.Latitude);
            XElement Longitude = new XElement("Longitude", newCustomer.Longitude);
            XElement IsActive = new XElement("IsActive", true);
            return new XElement("Customer", Id, Name, Phone, Latitude, Longitude, IsActive);
        }

        /// <summary>
        /// Receive a XElement Customer and return a XElemnt DO.Customer - copy information.
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        private Customer returnCustomer(XElement customer)
        {
            return new DO.Customer()
            {
                Id = Convert.ToInt32(customer.Element("Id").Value),
                Name = customer.Element("Name").Value,
                Phone = customer.Element("Phone").Value,
                Latitude = Convert.ToInt32(customer.Element("Latitude").Value),
                Longitude = Convert.ToInt32(customer.Element("Longitude").Value),
                IsActive = Convert.ToBoolean((customer.Element("IsActive").Value))
            };
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
                XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
                XElement customerXElemnt = (from c in customerRoot.Elements()
                                            where Convert.ToInt32(c.Element("Id").Value) == customerToRemove.Id
                                            select c).FirstOrDefault();
                if (customerXElemnt != null)
                    customerXElemnt.Element("IsActive").Value = "false";
            }
            catch(Exceptions.ObjNotExistException)
            {
                throw new Exceptions.ObjNotExistException(typeof(Customer), customerToRemove.Id);
            }

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Customer> customersList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //try
            //{
            //    Customer customer = getCustomerWithSpecificCondition(s => s.Id == customerToRemove.Id).First();
            //    if (customer.IsActive)
            //        customer.IsActive = false;
            //    changeCustomerInfo(customer);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exceptions.NoMatchingData(typeof(Customer), customerToRemove.Id, e1);
            //}
            #endregion
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
            return (from c in customerRoot.Elements()
                    orderby Convert.ToInt32(c.Element("Id").Value)
                    select returnCustomer(c));

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Customer> parceslList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //return from item in parceslList
            //       orderby item.Id
            //       select item;
            #endregion

        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer customerWithUpdateInfo)
        {
            XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
            XElement customerElemnt = (from c in customerRoot.Elements()
                                       where Convert.ToInt32(c.Element("Id").Value) == customerWithUpdateInfo.Id
                                       select c).FirstOrDefault();

            //if (customerElemnt == (default(XElement)))
            //    throw new Exceptions.ObjNotExistException(typeof(Parcel), customerWithUpdateInfo.Id);

            XElement xElementUpdateDrone = returnCustomerXElement(customerWithUpdateInfo);
            customerElemnt = xElementUpdateDrone;
            customerRoot.Save(dir + customerFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Customer> parcelsList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath).ToList();
            //int index = parcelsList.FindIndex(t => t.Id == customerWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Customer), customerWithUpdateInfo.Id);

            //parcelsList[index] = customerWithUpdateInfo;
            //XMLTools.SaveListToXMLSerializer<DO.Customer>(parcelsList, dir + customerFilePath);
            #endregion
        }

        /// <summary>
        /// Get a Customer/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a customer/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> getCustomerWithSpecificCondition(Predicate<Customer> predicate)
        {
            XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
            return (from c in customerRoot.Elements()
                    where predicate(returnCustomer(c))
                    select returnCustomer(c));

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Customer> customerList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //return (from customer in customerList
            //        where predicate(customer)
            //        select customer);
            #endregion
        }

        /// <summary>
        /// If customer with the requested id exist & active
        /// </summary>
        /// <param name="requestedId">Looking for customer with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsCustomerActive(int requestedId)
        {
            XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
            XElement customerXElemnt = (from c in customerRoot.Elements()
                                        where Convert.ToInt32(c.Element("Id").Value) == requestedId
                                        && Convert.ToBoolean(c.Element("IsActive").Value)
                                        select c).FirstOrDefault();

            if (customerXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Customer> customersList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //if (customersList.Any(c => c.Id == requestedId))
            //    return true;
            //return false;
            #endregion
        }

        /// <summary>
        /// If customer with the requested id exist
        /// </summary>
        /// <param name="requestedId">Looking for customer with this id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IsCustomerById(int requestedId)
        {
            XElement customerRoot = XMLTools.LoadData(dir + customerFilePath);
            XElement customerXElemnt = (from c in customerRoot.Elements()
                                        where Convert.ToInt32(c.Element("Id").Value) == requestedId
                                        select c).FirstOrDefault();
            if (customerXElemnt != null)
                return true;
            return false;

            #region LoadListFromXMLSerializer
            //IEnumerable<DO.Customer> customersList = XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //if (customersList.Any(c => c.Id == requestedId))
            //    return true;
            //return false;
            #endregion
        }
    }
}
