using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;


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
            XElement customerRoot = DL.XMLTools.LoadData(dir + customerFilePath);
            customerRoot.Add(returnCustomerXElement(newCustomer));
            customerRoot.Save(dir + customerFilePath);
            #region found better way
            //IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            //if (customersList.Any(c => c.Id == newCustomer.Id))
            //{
            //    throw new DO.Exceptions.ObjExistException(typeof(DO.Customer), newCustomer.Id);
            //}
            //List<DO.Customer> newList = customersList.Cast<DO.Customer>().ToList();
            //newList.Add(newCustomer);
            //DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(newList, dir + customerFilePath);
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
        /// if customer exist: IsActive = false + change its info (In DataSource)
        /// If doesn't exist throw NoMatchingData exception.
        /// </summary>
        /// <param name="customerToRemove">The customer to remove. customerToRemove.IsActive = false</param>
        public void removeCustomer(Customer customerToRemove)
        {
            IEnumerable<DO.Customer> customersList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            try
            {
                Customer customer = getCustomerWithSpecificCondition(s => s.Id == customerToRemove.Id).First();
                if (customer.IsActive)
                    customer.IsActive = false;
                changeCustomerInfo(customer);
            }
            catch (Exception e1)
            {
                throw new Exceptions.NoMatchingData(typeof(Customer), customerToRemove.Id, e1);
            }
            #region found better way
            //XElement customerRoot = DL.XMLTools.LoadData(dir + customerFilePath);
            //XElement customerXElemnt;
            //customerXElemnt = (from c in customerRoot.Elements()
            //                where Convert.ToInt32(c.Element("Id").Value) == customerToRemove.Id
            //                select c).FirstOrDefault();
            //if (customerXElemnt != null)
            //{
            //    customerXElemnt.Element("IsActive").Value = "false";
            //}
            #endregion
        }

        /// <summary>
        /// Get all customer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomers()
        {
            IEnumerable<DO.Customer> parceslList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath);
            return from item in parceslList
                   orderby item.Id
                   select item;
        }

        /// <summary>
        ///  Change specific customer info
        /// </summary>
        /// <param name="customerWithUpdateInfo">DrThe customer with the changed info</param>
        public void changeCustomerInfo(Customer customerWithUpdateInfo)
        {
            XElement customerRoot = DL.XMLTools.LoadData(dir + customerFilePath);
            XElement customerElemnt = (from c in customerRoot.Elements()
                                       where Convert.ToInt32(c.Element("Id").Value) == customerWithUpdateInfo.Id
                                       select c).FirstOrDefault();
            XElement xElementUpdateDrone = returnCustomerXElement(customerWithUpdateInfo);
            customerElemnt = xElementUpdateDrone;
            customerRoot.Save(dir + customerFilePath);

            #region LoadListFromXMLSerializer
            //List<DO.Customer> parcelsList = DL.XMLTools.LoadListFromXMLSerializer<DO.Customer>(dir + customerFilePath).ToList();
            //int index = parcelsList.FindIndex(t => t.Id == customerWithUpdateInfo.Id);
            //if (index == -1)
            //    throw new DO.Exceptions.ObjNotExistException(typeof(Customer), customerWithUpdateInfo.Id);

            //parcelsList[index] = customerWithUpdateInfo;
            //DL.XMLTools.SaveListToXMLSerializer<DO.Customer>(parcelsList, dir + customerFilePath);
            #endregion
        }
    }
}
