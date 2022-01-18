﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PO
{
    public class Customer : DependencyObject
    {
        public Customer()
        {
        }

        public Customer(BO.Customer c)
        {
            this.Update(c);
        }
        
        public void Update(BO.Customer c)
        {
            Id = c.Id;
            Name = c.Name;
            Phone = c.Phone;
            CustomerPosition = c.CustomerPosition;
            if (c.CustomerAsSender.Count > 0)
                CustomerAsSender = c.CustomerAsSender;
            if (c.CustomerAsTarget.Count > 0)
                CustomerAsTarget = c.CustomerAsTarget;
        }

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public string Name {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public string Phone {
            get { return (string)GetValue(PhoneProperty); }
            set { SetValue(PhoneProperty, value); }
        }
        public BO.Position CustomerPosition {
            get { return (BO.Position)GetValue(CustomerPositionProperty); }
            set { SetValue(CustomerPositionProperty, value); }
        }
        public List<BO.ParcelAtCustomer> CustomerAsSender {
            get { return (List<BO.ParcelAtCustomer>)GetValue(CustomerAsSenderProperty); }
            set { SetValue(CustomerAsSenderProperty, value); }
        }
        public List<BO.ParcelAtCustomer> CustomerAsTarget {
            get { return (List<BO.ParcelAtCustomer>)GetValue(CustomerAsTargetProperty); }
            set { SetValue(CustomerAsTargetProperty, value); }
        }

        public override string ToString()
        {
            return ($"customer id: {Id}, customer name: {Name}, customer phone: {Phone}, \n\tCustomerPosition: {CustomerPosition.ToString()}" +
              $"\tCustomerAsSenderAmount:  { CustomerAsSender.Count()}\n\tCustomerAsTargetAmount: {CustomerAsTarget.Count()}\n");
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(object), typeof(Customer), new UIPropertyMetadata(0));
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(object), typeof(Customer), new UIPropertyMetadata(0));
        public static readonly DependencyProperty PhoneProperty = DependencyProperty.Register("Phone", typeof(object), typeof(Customer), new UIPropertyMetadata(0));
        public static readonly DependencyProperty CustomerPositionProperty = DependencyProperty.Register("CustomerPosition", typeof(object), typeof(Customer), new UIPropertyMetadata(0));
        public static readonly DependencyProperty CustomerAsSenderProperty = DependencyProperty.Register("CustomerAsSender", typeof(object), typeof(Customer), new UIPropertyMetadata(0));
        public static readonly DependencyProperty CustomerAsTargetProperty = DependencyProperty.Register("CustomerAsTarget", typeof(object), typeof(Customer), new UIPropertyMetadata(0));


    }

    public class CustomerToList : DependencyObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int AmountAsSendingDeliveredParcels { get; set; }
        public int AmountAsSendingUnDeliveredParcels { get; set; }
        public int AmountAsTargetDeliveredParcels { get; set; }
        public int AmountAsTargetUnDeliveredParcels { get; set; }
    }

    public class CustomerInParcel : DependencyObject //targetId in parcel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return ($"Id: {Id} , Name: {name} \n");
        }
    }
}

