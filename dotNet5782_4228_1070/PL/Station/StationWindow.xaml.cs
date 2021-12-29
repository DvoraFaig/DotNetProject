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
        BO.Station station;
        string[] deliveryButtonOptionalContent = { "Send To Delivery", "Pick Up Parcel", "Which Package Delivery" };

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
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        public StationWindow(BlApi.Ibl blObject, BO.Station station)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            this.blObject = blObject;
            this.station = station;
            IdTextBox.Text = $"{station.Id}";
            NameTextBox.Text = $"{ station.Name}";
            ChargingSlotsAvailbleTextBox.Text = $"{ station.DroneChargeAvailble}";
            PositionTextBox.Text = $"( {station.StationPosition.Latitude} , {station.StationPosition.Longitude} )";
            ChargingDronesInStationListView.ItemsSource = station.DronesCharging;
            if (station.DronesCharging.Count() == 0)
                ChargingDronesInStationListView.Visibility = Visibility.Hidden;
            else
                NoDronesInCharge.Visibility = Visibility.Hidden;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
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

        private void addStationBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Station newStation = new BO.Station()
                {
                    Id = int.Parse(IdTextBox.Text),
                    Name = NameTextBox.Text,
                    DroneChargeAvailble = int.Parse(ChargingSlotsTextBox.Text),
                    StationPosition = new BO.Position()
                    {
                        Latitude = double.Parse(StationLatitudeTextBox.Text),
                        Longitude = double.Parse(StationLongitudeTextBox.Text)
                    }
                };
                blObject.AddStation(newStation);
                new StationListWindow(blObject).Show();
                this.Close();
            }
            #region catch exeptions
            catch (BO.Exceptions.ObjExistException)
            {
                MessageBox.Show("== ERROR receiving data or enter a different Id ==\nPlease try again");
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (FormatException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (OverflowException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("== ERROR receiving data ==\nPlease try again");
            }
            catch (Exception)
            {
                MessageBox.Show("Cann't add a station", "Station Error");
            }
            #endregion

        }

        private void ButtonClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "";
            NameTextBox.Text = "";
            ChargingSlotsTextBox.Text = "";
            StationLatitudeTextBox.Text = "";
            StationLongitudeTextBox.Text = "";
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
            //   
            //}

            //if (int.Parse(ChargingSlotsAvailbleTextBox.Text) != station.DroneChargeAvailble)
            //{

            //}
            blObject.StationChangeDetails(station.Id, NameTextBox.Text, int.Parse(ChargingSlotsAvailbleTextBox.Text));
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
