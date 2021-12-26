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
        private BO.Parcel parcel;
        private bool isClientAndNotAdmin = false;
        private bool clientIsSender = false;
        private bool returnToParcelListWindow = false;
        private Window returnBackTo;




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
            initializeAdd();
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
            returnToParcelListWindow = true;
        }

        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel , bool cameFromPageParcelList = true)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            this.parcel = parcel;
            returnToParcelListWindow = cameFromPageParcelList;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            //visibleUpdateForm.Visibility = Visibility;
            initializeDetails();
        }
        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel, bool isSender , Window window)
        {
            InitializeComponent();
            isClientAndNotAdmin = true;
            clientIsSender = isSender;
            returnBackTo = window;
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            this.parcel = parcel;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            initializeDetails();
            if (isClientAndNotAdmin)
            {
                if (!clientIsSender || parcel.Drone != null)//parcel was squedueld already
                    RemoveBtn.Visibility = Visibility.Hidden;
            }
        }

        private void initializeDetails()
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

        private void initializeAdd()
        {
            //visibleAddForm.Visibility = Visibility;
            ParcelTitle.Content = "Add a Parcel";
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
            ParcelTargetSelector.ItemsSource = blObject.CustomerLimitedDisplay();
            ParcelSenderSelector.ItemsSource = blObject.CustomerLimitedDisplay();
        }
        private void RemoveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                blObject.RemoveParcel(parcel);
                new ParcelListWindow_(blObject).Show();
                this.Close();
                MessageBox.Show("Parcel was remove");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            int senderId = ((CustomerInParcel)ParcelSenderSelector.SelectedItem).Id;
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
            new ParcelListWindow_(blObject).Show();
            this.Close();
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
                    customer = blObject.GetCustomerById(parcel.Sender.Id);
                }
                else
                {
                    customer = blObject.GetCustomerById(parcel.Target.Id);
                }
                new CustomerWindow(blObject, customer, clientIsSender).Show();
                //returnBackTo(blObject, customer, clientIsSender).Show();
                this.Close();
            }
            else
            {
                if (returnToParcelListWindow)
                    new ParcelListWindow_(blObject).Show();
                else
                {
                    if (clientIsSender)
                        new CustomerWindow(blObject, blObject.GetCustomerById(parcel.Sender.Id)).Show();
                    else
                        new CustomerWindow(blObject, blObject.GetCustomerById(parcel.Target.Id)).Show();
                }
                this.Close();
            }
        }

        private void DroneClick(object sender, RoutedEventArgs e)
        {
            Drone d = blObject.GetDroneById(parcel.Drone.Id);
            new DroneWindow(blObject, d).Show();
            this.Close();
        }

        private void CustomerButton(object sender, RoutedEventArgs e)
        {
            CustomerInParcel customerClicked = ((sender as Button).Name == "TargetText") ? parcel.Target : parcel.Sender;
            Customer customer = blObject.GetCustomerById(customerClicked.Id);
            new CustomerWindow(blObject, customer).Show();
            this.Close();
        }

        private void ParcelCustomerSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerInParcel customerSelected = (CustomerInParcel)(sender as ComboBox).SelectedItem;
           // ParcelTargetSelector.SelectedIndex = -1;
            //ParcelTargetSelector.Items.Remove(customerSelected);
            //ParcelTargetSelector.ItemsSource = blObject.CustomerLimitedDisplay(customerSelected);
            //ParcelSenderSelector.ItemsSource = blObject.CustomerLimitedDisplay(customerSelected);
        }
    }
}
