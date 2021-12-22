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
        Customer customer;// = new Customer();
        private bool updateOrAddWindow { get; set; }//true = add drone

     
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public CustomerWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            blObjectD = blObject;
            updateOrAddWindow = true;
            IdTextBox.Text = "Id...";
            NameTextBox.Text = "Name...";
            PhoneTextBox.Text = "Phone...";
            LatitudeTextBox.Text = "latitude...";
            LongitudeTextBox.Text = "longitude...";
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }
        public CustomerWindow(BlApi.Ibl blObject, Customer customerInCtor)
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
            CustomerAsTargetListView.ItemsSource = customerInCtor.CustomerAsSender;
            CustomerAsSenderListView.ItemsSource = customerInCtor.CustomerAsTarget;
            CustomerAsTargetListView.Visibility = Visibility.Hidden;

        }

        private void setDeliveryButton()
        {
            //switch (customer.Status)
            //{
            //    case DroneStatus.Available:
            //        ChargeButton.Content = "Send Drone To Charge";
            //        break;
            //    case DroneStatus.Maintenance:
            //        ChargeButton.Content = "Free Drone From Charge";
            //        break;
            //}
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
            //int weightCategory = Convert.ToInt32((DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            //try
            //{
            //    blObjectD.AddDrone(Convert.ToInt32(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
            //    TextBlock addedDrone = new TextBlock();
            //    new DroneListWindow(blObjectD).Show();
            //    this.Close();

            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("== ERROR receiving data ==");
            //}
            //catch (OverflowException)
            //{
            //    Console.WriteLine("== ERROR receiving data ==");
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Cann't add a drone", "Drone Error");
            //}

        }

        private void ButtonClickRestart(object sender, RoutedEventArgs e)
        {
            //IdTextBox.Text = "Id...";
            //ModelTextBox.Text = "Model....";
            //DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(DO.WeightCategories));
            //StationIdTextBox.Text = "Station id...";
        }
        private void ButtonClickReturnToPageDroneListWindow(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            //customer.Model = ModelTextBox.Text;
            //blObjectD.DroneChangeModel(customer);
            new CustomerListWindow(blObjectD).Show();
            this.Close();
        }

        private void ChargeButtonClick(object sender, RoutedEventArgs e)
        {
            //if (ChargeButton.Content.ToString() == "Send Drone To Charge")
            //{
            //    //try
            //    //{
            //    //    blObjectD.SendDroneToCharge(customer.Id);
            //    //    StatusTextBox.Text = $"{customer.Status}";
            //    //    setDeliveryButton();
            //    //}
            //    //catch (Exception)
            //    //{

            //    //}
            //}
            //else
            //{
            //    if (TimeTocharge.Text == "")
            //    {
            //        MessageBox.Show("ERROR\nEnter time to charge");
            //    }
            //    else
            //    {
            //        try
            //        {
            //            blObjectD.FreeDroneFromCharging(customer.Id, int.Parse(TimeTocharge.Text));
            //            StatusTextBox.Text = $"{customer.Status}";
            //            setDeliveryButton();
            //            TimeTocharge.Text = "";
            //        }
            //        catch (Exception)
            //        {

            //        }
            //    }
            //}
        }

        private void FreeChargeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SendDroneToChargeClick(object sender, RoutedEventArgs e)
        {
            //string contentClickedButton = DeliveryStatusButton.Content.ToString();
            //if (contentClickedButton == deliveryButtonOptionalContent[0])
            //{
            //    try { blObjectD.PairParcelWithDrone(customer.Id); }
            //    catch (BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
            //    catch (Exception e2) { MessageBox.Show(e2.Message); }
            //}
            //else if (contentClickedButton == deliveryButtonOptionalContent[1])
            //{
            //    try { blObjectD.DronePicksUpParcel(customer.Id); }
            //    catch (BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
            //    catch (Exception e2) { MessageBox.Show(e2.Message); }
            //}
            //else if (contentClickedButton == deliveryButtonOptionalContent[2])
            //{
            //    try { blObjectD.DeliveryParcelByDrone(customer.Id); }
            //    catch (BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
            //    catch (Exception e2) { MessageBox.Show(e2.Message); }
            //}
        }

        private void show(object sender, MouseButtonEventArgs e)
        {
            CustomerAsTargetListView.Visibility = Visibility.Visible;
        }
    }
}
