using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private BlApi.Ibl blObjectD;
        BO.Customer customer;// = new Customer();
        private bool updateOrAddWindow { get; set; }//true = add drone
        bool isClient = false;

        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion

        /// <summary>
        /// Ctor Add form
        /// </summary>
        /// <param name="blObject"></param>
        public CustomerWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            blObjectD = blObject;
            updateOrAddWindow = true;
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            PhoneTextBox.Text = "";
            LatitudeTextBox.Text = "";
            LongitudeTextBox.Text = "";
            //IdTextBox.Text = "Id...";
            //NameTextBox.Text = "Name...";
            //PhoneTextBox.Text = "Phone...";
            //LatitudeTextBox.Text = "latitude...";
            //LongitudeTextBox.Text = "longitude...";
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Ctor Update form
        /// </summary>
        /// <param name="blObject">Ibl instance</param>
        /// <param name="customerInCtor">Recieve the customer to update</param>
        public CustomerWindow(BlApi.Ibl blObject, BO.Customer customerInCtor)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            updateOrAddWindow = false;
            blObjectD = blObject;
            customer = customerInCtor;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            IdTextBox.Text = $"{customerInCtor.Id}";
            NameTextBox.Text = $"{customerInCtor.Name}";
            PhoneTextBox.Text = $"{customerInCtor.Phone}";
            PositionTextBox.Text = $"( {customer.CustomerPosition.Latitude} , {customer.CustomerPosition.Longitude} )";
            CustomerAsTargetParcelsListView.ItemsSource = customerInCtor.CustomerAsTarget;
            CustomerAsSenderParcelsListView.ItemsSource = customerInCtor.CustomerAsSender;

        }

        public CustomerWindow(BlApi.Ibl blObject, BO.Customer client , bool isClient)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            updateOrAddWindow = false;
            this.isClient = true;
            blObjectD = blObject;
            customer = client;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            IdTextBox.Text = $"{client.Id}";
            NameTextBox.Text = $"{client.Name}";
            PhoneTextBox.Text = $"{client.Phone}";
            PositionTextBox.Text = $"( {customer.CustomerPosition.Latitude} , {customer.CustomerPosition.Longitude} )";
            CustomerAsTargetParcelsListView.ItemsSource = client.CustomerAsTarget;
            CustomerAsSenderParcelsListView.ItemsSource = client.CustomerAsSender;
        }

        /// <summary>
        /// Display DroneWindow Add or Update
        /// false == show update window
        /// true == show add window
        /// </summary>
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void ButtoClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                blObjectD.AddCustomer(Convert.ToInt32(IdTextBox.Text), NameTextBox.Text, PhoneTextBox.Text, Convert.ToInt32(LatitudeTextBox.Text) , Convert.ToInt32(LongitudeTextBox.Text));
                new CustomerListWindow(blObjectD).Show();
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (OverflowException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (Exception)
            {
                MessageBox.Show("Cann't add a drone", "Drone Error");
            }

        }

        /// <summary>
        /// Reset all text boxes in the Add form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            PhoneTextBox.Text = "";
            LatitudeTextBox.Text = "";
            LongitudeTextBox.Text = "";
        }

        private void ButtonClickReturnToPageCustomerListWindow(object sender, RoutedEventArgs e)
        {
            if (isClient)
            {
                MessageBoxResult messageBoxClosing = MessageBox.Show("Are you Sure you wan't to exit", "GoodBy", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxClosing == MessageBoxResult.OK)
                {
                    new SignInOrUpWindow(blObjectD).Show();
                    this.Close();
                }
            }
            else
            {
                MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxClosing == MessageBoxResult.OK)
                {
                    new CustomerListWindow(blObjectD).Show();
                    this.Close();
                }
            }
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            blObjectD.UpdateCustomerDetails(customer.Id, NameTextBox.Text, PhoneTextBox.Text);
            new CustomerListWindow(blObjectD).Show();
            this.Close();
        }

        private void SelectParcelOfSender(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelAtCustomer parcelAtCustomer = (BO.ParcelAtCustomer)CustomerAsSenderParcelsListView.SelectedItem;
            BO.Parcel parcel = blObjectD.GetParcelById(parcelAtCustomer.Id);
            if (isClient)
                new ParcelWindow(blObjectD, parcel, true,this).Show();
            else
                new ParcelWindow(blObjectD, parcel,false).Show();
            this.Close();
        }

        private void SelectParcelOfTarget(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelAtCustomer parcelAtCustomer = (BO.ParcelAtCustomer)CustomerAsTargetParcelsListView.SelectedItem;
            BO.Parcel parcel = blObjectD.GetParcelById(parcelAtCustomer.Id);
            if(isClient)
                new ParcelWindow(blObjectD, parcel, false,this).Show();
            else
                new ParcelWindow(blObjectD, parcel, false).Show();
            this.Close();
        }

        private void changeBackGroundCollapsed(object sender, RoutedEventArgs e)
        {
            CustomerAsSenderParcelsListView.Background = null;
        }

        private void changeBackGroundExpanded(object sender, RoutedEventArgs e)
        {
            CustomerAsSenderParcelsListView.Background = Brushes.White;
        }

        #region TextBox OnlyNumbers PreviewKeyDown function
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home || e.Key == Key.End ||
                e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
        #endregion
    }
}
