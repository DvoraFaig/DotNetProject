using System;
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
        //private BO.Parcel parcel;
        private PO.Parcel currentParcel;
        private BO.Customer clientCustomer;
        private bool isClientAndNotAdmin = false;
        private bool clientIsSender = false;
        private bool returnToParcelListWindow = false;
        private Window returnBackToUnupdateWindow;
        private bool customerUpdateHisParcel = false;
        private BO.Customer sender;
        private BO.Customer target;
        private BO.Drone drone;

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
            currentParcel = new PO.Parcel(blObject);
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
            returnToParcelListWindow = true;
        }
        public ParcelWindow(BlApi.Ibl blObject, int senderCustomerId)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            initializeDetailsAddForm();
            isClientAndNotAdmin = true;
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
            Customer senderCustomer = blObject.GetCustomerById(senderCustomerId);
            ParcelTargetSelector.ItemsSource = blObject.GetLimitedCustomersList(new CustomerInParcel() { Id = senderCustomer.Id, Name = senderCustomer.Name });
            ParcelSenderSelector.Visibility = Visibility.Hidden;
            SenderText.Visibility = Visibility.Visible;
            SenderText.Content = senderCustomer.Name;
            clientIsSender = true;
            returnToParcelListWindow = false;
            clientCustomer = senderCustomer;
            initializeObjAndSetConfirm();
            initializeDrone();

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
            //this.parcel = parcel;
            currentParcel = new PO.Parcel(blObject, parcel);
            AddParcelDisplay.DataContext = currentParcel;
            returnToParcelListWindow = cameFromPageParcelList;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            initializeDetailsUpdateForm();
            initializeObjAndSetConfirm();
            initializeDrone();
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
            //this.parcel = parcel;
            currentParcel = new PO.Parcel(blObject, parcel);
            AddParcelDisplay.DataContext = currentParcel;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            if (clientIsSender)
                this.clientCustomer = blObject.GetCustomerById(currentParcel.Sender.Id);
            else
                this.clientCustomer = blObject.GetCustomerById(currentParcel.Target.Id);
            initializeDetailsUpdateForm();
            if (isClientAndNotAdmin)
            {

                if (currentParcel.Drone != null) // parcel was schedualed
                    RemoveBtn.Visibility = Visibility.Hidden;
                if (!isSender)
                    RemoveBtn.Visibility = Visibility.Hidden;
            }
            initializeObjAndSetConfirm();
            initializeDrone();
        }



        private void initializeObjAndSetConfirm()
        {
            sender = blObject.GetCustomerById(currentParcel.Sender.Id);
            target = blObject.GetCustomerById(currentParcel.Target.Id);
            if (currentParcel.Drone != null)
                drone = blObject.GetDroneById(currentParcel.Drone.Id);
            setConfirmBtn();
        }

        private void initializeDrone()
        {
            if (isClientAndNotAdmin)
            {
                if (currentParcel.PickUp != null && currentParcel.Delivered == null)
                {
                    List<BO.Drone> d = new List<BO.Drone>();
                    d.Add(blObject.GetDroneById(currentParcel.Drone.Id));
                    DroneListView.ItemsSource = new List<BO.Drone>();
                    DroneText.Visibility = Visibility.Hidden;
                    return;
                }
            }
            ExpenderTarget.Visibility = Visibility.Hidden;
            DroneText.Visibility = Visibility.Visible;
            //DroneText.Content = $"{currentParcel.Drone.ToString()}";
        }

        private void setConfirmBtn()
        {
            if (currentParcel.Requeasted != null)
            {
                ConfirmButton.Visibility = Visibility.Hidden;
                if (currentParcel.PickUp == null && currentParcel.Scheduled != null)
                {
                    if ((drone.DronePosition.Latitude == sender.CustomerPosition.Latitude //parcel was pick up now.
                        && drone.DronePosition.Longitude == sender.CustomerPosition.Longitude)
                        && ((!isClientAndNotAdmin) //Admin
                        || (isClientAndNotAdmin && clientIsSender))) //customer is sender 
                    {
                        ConfirmButton.Visibility = Visibility.Visible;
                        ConfirmButton.Content = "Confirm pickUp";
                    }
                }
                if (currentParcel.PickUp != null && currentParcel.Delivered == null)
                {
                    bool a = ((!isClientAndNotAdmin) //Admin
                        || (isClientAndNotAdmin && !clientIsSender));
                    bool b = (isClientAndNotAdmin && !clientIsSender);
                    bool c = (drone.DronePosition.Latitude == target.CustomerPosition.Latitude //parcel was delivered now.
                        && drone.DronePosition.Longitude == target.CustomerPosition.Longitude);
                    if ((drone.DronePosition.Latitude == target.CustomerPosition.Latitude //parcel was delivered now.
                        && drone.DronePosition.Longitude == target.CustomerPosition.Longitude)
                        && ((!isClientAndNotAdmin) //Admin
                        || (isClientAndNotAdmin && !clientIsSender))) //customer is target 
                    {
                        ConfirmButton.Visibility = Visibility.Visible;
                        ConfirmButton.Content = "Confirm delivery";
                    }
                }
            }
            initializeDrone();
        }


        /// <summary>
        /// initialize update form details of parcels' textBoxes.
        /// </summary>
        private void initializeDetailsUpdateForm()
        {
            /*IdText.Text = $"{parcel.Id}";
            SenderText.Content = parcel.Sender;
            TargetText.Content = parcel.Target;
            WeightText.Text = $"{parcel.Weight}";
            PriorityText.Text = $"{parcel.Priority}";*/
            if (currentParcel.Drone == null) //DroneText.Content = $"{parcel.Drone.Id}";
            {
                //DroneText.Content = "No Drone";
                DroneText.IsEnabled = false;
            }
            /*if (parcel.Drone != null) DroneText.Content = $"{parcel.Drone.Id}";
            else
            {
                DroneText.Content = "No Drone";
                DroneText.IsEnabled = false;
            }*/
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
                    blObject.RemoveParcel(currentParcel.Id);
                    customerUpdateHisParcel = true;
                }
                catch (ArgumentNullException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {currentParcel.Id} wasn't found"); }
                catch (InvalidOperationException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {currentParcel.Id} wasn't found"); }
                catch (Exceptions.ObjNotExistException) { PLFuncions.messageBoxResponseFromServer("Remove Parcel", $"The requested parcel with id {currentParcel.Id} wasn't found"); }

                if (isClientAndNotAdmin && !customerUpdateHisParcel)//if customer der=tailes are not updated. return to the window(without creating a new window).
                    returnBackToUnupdateWindow.Show();
                else if (isClientAndNotAdmin)
                    new CustomerWindow(blObject, clientCustomer, true).Show();
                else
                    new ParcelListWindow_(blObject).Show();
                this.Close();
                PLFuncions.messageBoxResponseFromServer("Parcel Remove", "Parcel was removed succesfully");
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
                CustomerInParcel senderCustomer;
                if (isClientAndNotAdmin)
                    senderCustomer = new CustomerInParcel() { Id = clientCustomer.Id, Name = clientCustomer.Name };
                else
                    senderCustomer = ((CustomerInParcel)ParcelSenderSelector.SelectedItem);
                CustomerInParcel targetCustomer = ((CustomerInParcel)ParcelTargetSelector.SelectedItem);
                DO.WeightCategories weight = (DO.WeightCategories)ParcelWeightSelector.SelectedItem;
                DO.Priorities priority = (DO.Priorities)ParcelPrioritySelector.SelectedItem;
                try
                {
                    blObject.AddParcel(new Parcel() { Sender = senderCustomer, Target = targetCustomer, Weight = weight, Priority = priority });
                    if (isClientAndNotAdmin)
                        new CustomerWindow(blObject, clientCustomer, true).Show();
                    else
                    {
                        new ParcelListWindow_(blObject).Show();
                        this.Close();
                    }
                }
                #region Exceptions
                catch (BO.Exceptions.ObjNotAvailableException ex)
                {
                    MessageBox.Show(ex.Message);
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
                #endregion
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
                    customer = (currentParcel == null) ? clientCustomer : blObject.GetCustomerById(currentParcel.Sender.Id);
                }
                else
                {
                    customer = blObject.GetCustomerById(currentParcel.Target.Id);
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
                        new CustomerWindow(blObject, blObject.GetCustomerById(currentParcel.Sender.Id), false).Show();
                    else
                        new CustomerWindow(blObject, blObject.GetCustomerById(currentParcel.Target.Id), false).Show();
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
            Drone d = blObject.GetDroneById(currentParcel.Drone.Id);
            new DroneWindow(blObject, d).Show();
            //this.Close();
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
            CustomerInParcel customerClicked = ((sender as Button).Name == "TargetText") ? currentParcel.Target : currentParcel.Sender;
            Customer customer = blObject.GetCustomerById(customerClicked.Id);
            new CustomerWindow(blObject, customer, false).Show();
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

        private void confirmParcelBtn(object sender, RoutedEventArgs e)
        {
            ///////////////////
            ///setConfirmBtn
            if (ConfirmButton.Content == "Confirm pickUp")
            {
                blObject.DronePicksUpParcel(drone.Id); //currentParcel.Drone.Id;
            }
            //takes time from pick up to delivery.
            else if (ConfirmButton.Content == "Confirm delivery")
            {
                blObject.DeliveryParcelByDrone(drone.Id);
            }
            setConfirmBtn();
        }


        /// <summary>
        /// Change background of drone info when there is a click on colla[s to close the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBackGroundExpenderCollapsedDroneExpender(object sender, RoutedEventArgs e)
        {
            DroneListView.Background = null;
        }

        /// <summary>
        /// Change background of drone info when there is a click on expender to close the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBackGroundExpenderExpandedDroneExpender(object sender, RoutedEventArgs e)
        {
            DroneListView.Background = Brushes.White;
        }
    }
}
