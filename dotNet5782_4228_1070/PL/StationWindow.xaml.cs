using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class StationWindow : Window
    {
        private BlApi.Ibl blObject;
        Station station;
        string[] deliveryButtonOptionalContent = { "Send To Delivery", "Pick Up Parcel", "Which Package Delivery" };
        private bool updateOrAddWindow { get; set; }//true = add drone
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public StationWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            updateOrAddWindow = true;
            displayWindowAddOrUpdate();
            IdTextBox.Text = "Id...";
            NameTextBox.Text = "Name....";
            ChargingSlotsTextBox.Text = "Amount...";
            StationLatitudeTextBox.Text = "Latitude....";
            StationLongitudeTextBox.Text = "Longitude....";
        }
        public StationWindow(BlApi.Ibl blObject, BO.Station station)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            updateOrAddWindow = false;
            displayWindowAddOrUpdate();
            this.blObject = blObject;
            this.station = station;
           
            IdTextBox.Text = $"{station.ID}";
            NameTextBox.Text = $"{ station.Name}";
            ChargingSlotsAvailbleTextBox.Text = $"{ station.DroneChargeAvailble}";
            PositionTextBox.Text = $"( {station.StationPosition.Longitude} , {station.StationPosition.Latitude} )";
            ChargingDronesInStationListView.ItemsSource = station.DronesCharging;
            if (station.DronesCharging.Count() == 0)
                ChargingDronesInStationListView.Visibility = Visibility.Hidden;
            else
                NoDronesInCharge.Visibility = Visibility.Hidden;
        }

        private void setDeliveryButton()
        {
            
        }

        /// <summary>
        /// Display DroneWindow Add or Update
        /// false == show update window
        /// true == show add window
        /// </summary>
        private void displayWindowAddOrUpdate()
        {
            Visibility visibility;
            visibility = (updateOrAddWindow == false) ? Visibility.Hidden : Visibility.Visible;
            IdLabel.Visibility = visibility;
            IdTextBox.Visibility = visibility;
            NameLabel.Visibility = visibility;
            NameTextBox.Visibility = visibility;
            ChargingSlotsLabel.Visibility = visibility;
            ChargingSlotsTextBox.Visibility = visibility;
            StationLatitudeLabel.Visibility = visibility;
            StationLatitudeTextBox.Visibility = visibility;
            StationLongitudeLabel.Visibility = visibility;
            StationLongitudeTextBox.Visibility = visibility;
            labelAddStation.Visibility = visibility;
            RestartButton.Visibility = visibility;
   

            visibility = (visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
            if (updateOrAddWindow == false)//if Add Drone don't go in
            {
                NameLabel.Visibility = visibility;
                NameTextBox.Visibility = visibility;
                IdLabel.Visibility = visibility;
                IdTextBox.Visibility = visibility;
                IdTextBox.IsReadOnly = true;
            }
            NoDronesInCharge.Visibility = visibility;
            PositionLabel.Visibility = visibility;
            PositionTextBox.Visibility = visibility;
            PositionTextBox.IsReadOnly = true;
            ChargingSlotsAvailbleLabel.Visibility = visibility;
            ChargingSlotsAvailbleTextBox.Visibility = visibility;
            ChargingDronesLabel.Visibility = visibility;
            ChargingDronesInStationListView.Visibility = visibility;

        }
        /// <summary>
        /// Code to remove close box from window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void BtnAddStation(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "Id...";
            NameTextBox.Text = "Name....";
            ChargingSlotsTextBox.Text = "Amount...";
            StationLatitudeTextBox.Text = "Latitude....";
            StationLongitudeTextBox.Text = "Longitude....";
        }
        private void ButtonClickReturnToPageStationListWindow(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                new StationListWindow(blObject).Show();
                this.Close();
            }
        } 
        
        private void DroneChargeSelection(object sender, MouseButtonEventArgs e)
        {
            ChargingDrone chargingDrone = ((ChargingDrone)ChargingDronesInStationListView.SelectedItem);
            Drone drone = blObject.GetDroneById(chargingDrone.Id);////
            new DroneWindow(blObject, drone).Show();
            this.Close();
        }

        private void UpdateBtnClick(object sender, RoutedEventArgs e)
        {
            //if (NameTextBox.Text != station.Name)
            //{
                
            //}

            //if (int.Parse(ChargingSlotsAvailbleTextBox.Text) != station.DroneChargeAvailble)
            //{

            //}
            blObject.StationChangeDetails(station.ID, NameTextBox.Text, int.Parse(ChargingSlotsAvailbleTextBox.Text));
        }
    }
}
