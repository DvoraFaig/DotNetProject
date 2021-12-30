﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BlApi.Ibl blObject;
        private BO.Parcel parcel;
        private BO.Customer clientCustomer;
        private bool isClientAndNotAdmin = false;
        private bool clientIsSender = false;
        private bool returnToParcelListWindow = false;
        private Window returnBackToUnupdateWindow;
        private bool customerUpdateHisParcel = false;

        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion


        public ParcelWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            initializeDetailsAddForm();
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
            returnToParcelListWindow = true;
        }
        public ParcelWindow(BlApi.Ibl blObject, Customer senderCustomer)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            initializeDetailsAddForm();
            isClientAndNotAdmin = true;
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
            ParcelTargetSelector.ItemsSource = blObject.GetLimitedCustomersList(new CustomerInParcel() { Id = senderCustomer.Id , Name = senderCustomer.Name});
            ParcelSenderSelector.Visibility = Visibility.Hidden;
            SenderText.Visibility = Visibility.Visible;
            SenderText.Content = senderCustomer.Name;
            clientIsSender = true;
            returnToParcelListWindow = false;
            clientCustomer = senderCustomer;
        }

        /// <summary>
        /// Ctor display the update / see info a specific parcel Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        /// <param name="parcel">The parcel to update / see info</param>
        /// <param name="cameFromPageParcelList">To know were to return back. if ture = parcelList, if false = return to a specific customer </param>
        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel, bool cameFromPageParcelList = true)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            this.parcel = parcel;
            returnToParcelListWindow = cameFromPageParcelList;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            initializeDetailsUpdateForm();
        }

        /// <summary>
        /// Ctor display the update / see info a specific parcel Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        /// <param name="parcel">The parcel to update / see info </param>
        /// <param name="isSender">to know to witch specific customer to return back. sender / target</param>
        /// <param name="window"></param>
        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel, bool isSender, Window window)
        {
            InitializeComponent();
            isClientAndNotAdmin = true;
            clientIsSender = isSender;
            returnBackToUnupdateWindow = window;
            Loaded += ToolWindowLoaded; //The x button
            this.blObject = blObject;
            this.parcel = parcel;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            if(clientIsSender)
                this.clientCustomer = blObject.GetCustomerById(parcel.Sender.Id);
            else
                this.clientCustomer = blObject.GetCustomerById(parcel.Target.Id);
            initializeDetailsUpdateForm();
            if (isClientAndNotAdmin)
            {
                if(parcel.Drone != null) // parcel was schedualed
                    RemoveBtn.Visibility = Visibility.Hidden;
                if(!isSender)
                    RemoveBtn.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// initialize update form details of parcels' textBoxes.
        /// </summary>
        private void initializeDetailsUpdateForm()
        {
            IdText.Text = $"{parcel.Id}";
            SenderText.Content = parcel.Sender;
            TargetText.Content = parcel.Target;
            WeightText.Text = $"{parcel.Weight}";
            PriorityText.Text = $"{parcel.Priority}";
            if (parcel.Drone != null) DroneText.Content = $"{parcel.Drone.Id}";
            else
            {
                DroneText.Content = "No Drone";
                DroneText.IsEnabled = false;
            }
        }

        /// <summary>
        /// initialize add form details of parcels' textBoxes.
        /// </summary>
        private void initializeDetailsAddForm()
        {
            ParcelTitle.Content = "Add a Parcel";
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
            ParcelTargetSelector.ItemsSource = blObject.GetLimitedCustomersList();
            ParcelSenderSelector.ItemsSource = blObject.GetLimitedCustomersList();
        }

        /// <summary>
        /// Try sending the parcel to remove.
        /// if isClientAndNotAdmin = true go to CustomerWindow
        /// If Admin go back to parcelListWIndow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeParcelBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!clientIsSender)//////
                    return;
                try
                {
                    blObject.RemoveParcel(parcel);
                    customerUpdateHisParcel = true;
                }
                catch(ArgumentNullException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {parcel.Id} wasn't found"); }
                catch (InvalidOperationException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {parcel.Id} wasn't found"); }
                catch (Exceptions.ObjNotExistException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {parcel.Id} wasn't found"); }

                if (isClientAndNotAdmin && !customerUpdateHisParcel)//if customer der=tailes are not updated. return to the window(without creating a new window).
                    returnBackToUnupdateWindow.Show();
                else if(isClientAndNotAdmin)
                    new CustomerWindow(blObject, clientCustomer , true).Show();
                else
                    new ParcelListWindow_(blObject).Show();
                this.Close();
                PLFuncions.messageBoxResponseFromServer("Parcel Remove","Parcel was removed succesfully");
            }
            catch (Exceptions.ObjNotAvailableException ex)
            {
               PLFuncions.messageBoxResponseFromServer("Remove Parcel", ex.Message);
            }
        }

        ///// <summary>
        ///// Message from the server. like errors
        ///// </summary>
        ///// <param name="header">the name of the header of the messageBox</param>
        ///// <param name="message">The message</param>
        //private void messageBoxResponseFromServer(String header,String message)
        //{
        //    MessageBox.Show(header , message);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// addParcelBtnClick
        private void addParcelBtnClick(object sender, RoutedEventArgs e)
        {

            if ((isComboBoxesFieldsFull(ParcelWeightSelector, ParcelPrioritySelector, ParcelSenderSelector, ParcelTargetSelector) && !isClientAndNotAdmin)
                || (isComboBoxesFieldsFull(ParcelWeightSelector, ParcelPrioritySelector, ParcelTargetSelector) && isClientAndNotAdmin))
            {
                int senderId;
                if (isClientAndNotAdmin)
                    senderId = clientCustomer.Id;
                else
                    senderId = ((CustomerInParcel)ParcelSenderSelector.SelectedItem).Id;
                int targetId = ((CustomerInParcel)ParcelTargetSelector.SelectedItem).Id;
                DO.WeightCategories weight = (DO.WeightCategories)ParcelWeightSelector.SelectedItem;
                DO.Priorities priority = (DO.Priorities)ParcelPrioritySelector.SelectedItem;
                try
                {
                    blObject.AddParcel(senderId, targetId, weight, priority);
                }
                catch (BO.Exceptions.ObjNotExistException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (BO.Exceptions.ObjExistException)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data or enter a different Id ==\nPlease try again");
                }
                catch (ArgumentNullException)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data ==\nPlease try again");
                }
                catch (FormatException)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data ==\nPlease try again");
                }
                catch (OverflowException)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data ==\nPlease try again");
                }
                catch (NullReferenceException)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data ==\nPlease try again");
                }
                catch (Exception)
                {
                    PLFuncions.messageBoxResponseFromServer("Add Parcel", "== ERROR receiving data ==\nPlease try again");
                }
                if (isClientAndNotAdmin)
                    new CustomerWindow(blObject, clientCustomer, true).Show();
                else
                    new ParcelListWindow_(blObject).Show();
                this.Close();
            }
            else PLFuncions.messageBoxResponseFromServer("Add a parcel", "Missinig Details");
        }

        /// <summary>
        /// Return true if the comboBoxes' fields are full
        /// </summary>
        /// <param name="comboBoxes">The comboBoxes to check their fields.</param>
        /// <returns></returns>
        private bool isComboBoxesFieldsFull(params ComboBox[] comboBoxes)
        {
            foreach (ComboBox item in comboBoxes)
            {
                if (item.SelectedItem == null) return false;
            }
            return true;
        }

        /// <summary>
        /// return back:
        /// if as a client = CustomerWindow with client privilages
        /// else: parcelListWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            //returnBackTo.Show();
            if (isClientAndNotAdmin)
            {
                BO.Customer customer;

                if (clientIsSender)
                {
                    customer = (parcel == null)? clientCustomer : blObject.GetCustomerById(parcel.Sender.Id);
                }
                else
                {
                    customer = blObject.GetCustomerById(parcel.Target.Id);
                }
                new CustomerWindow(blObject, customer, true).Show();
                this.Close();
            }
            else
            {
                if (returnToParcelListWindow)
                    new ParcelListWindow_(blObject).Show();
                else
                {
                    if (clientIsSender)
                        new CustomerWindow(blObject, blObject.GetCustomerById(parcel.Sender.Id), false).Show();
                    else
                        new CustomerWindow(blObject, blObject.GetCustomerById(parcel.Target.Id), false).Show();
                }
                this.Close();
            }
        }

        /// <summary>
        /// If Admin/worker go to the droneWindow and dispaly the parcels' drone.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneClick(object sender, RoutedEventArgs e)
        {
            if (isClientAndNotAdmin)
                return;
            Drone d = blObject.GetDroneById(parcel.Drone.Id);
            new DroneWindow(blObject, d).Show();
            this.Close();
        }

        /// <summary>
        /// If Admin/worker go to the CustomerWindow and dispaly the parcels' customer (sender / target).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerButton(object sender, RoutedEventArgs e)
        {
            if (isClientAndNotAdmin)
                return;
            CustomerInParcel customerClicked = ((sender as Button).Name == "TargetText") ? parcel.Target : parcel.Sender;
            Customer customer = blObject.GetCustomerById(customerClicked.Id);
            new CustomerWindow(blObject, customer,false).Show();
            this.Close();
        }

        private void ParcelCustomerSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CustomerInParcel customerSelected = (sender as ComboBox).SelectedItem;
            //var customerSelected = (sender as ComboBox).SelectedIndex;
            // ParcelTargetSelector.SelectedIndex = -1;
            //ParcelTargetSelector.Items.Remove(ParcelTargetSelector.SelectedItem);
            //ParcelSenderSelector.Items.RemoveAt(customerSelected);
            //ParcelTargetSelector.SelectedItem = customerSelected;
            //ParcelTargetSelector.ItemsSource = blObject.CustomerLimitedDisplay(customerSelected);
            //ParcelSenderSelector.ItemsSource = blObject.CustomerLimitedDisplay(customerSelected);
        }

        private void updateParcelInfoBtnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
