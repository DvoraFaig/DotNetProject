﻿using System;
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

        /// <summary>
        /// Ctor display the add a station Form
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        public StationWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Ctor display the update/see info a specific station Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        /// <param name="station">The station to update/see info</param>
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
                try
                {
                    blObject.AddStation(newStation);
                }
                catch(Exceptions.DataOfOjectChanged e1) { PLFuncions.messageBoxResponseFromServer("Add a Station", e1.Message); }
                new StationListWindow(blObject).Show();
                this.Close();
            }
            #region catch exeptions
            catch (BO.Exceptions.ObjExistException e1)
            {
                PLFuncions.messageBoxResponseFromServer("Add a Station" , e1.Message);
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
            Drone drone = blObject.GetDroneById(chargingDrone.Id);/////
            new DroneWindow(blObject, drone).Show();
            this.Close();
        }

        /// <summary>
        /// Try to send a stations update info to a func.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                blObject.StationChangeDetails(station.Id, NameTextBox.Text, int.Parse(ChargingSlotsAvailbleTextBox.Text));
                new StationListWindow(blObject);
                this.Close();
            }
            catch (ArgumentNullException e1) { PLFuncions.messageBoxResponseFromServer("Change Station information", e1.Message); }
            catch (FormatException e2) { PLFuncions.messageBoxResponseFromServer("Change Station information", e2.Message); }
            catch (OverflowException e3) { PLFuncions.messageBoxResponseFromServer("Change Station information", e3.Message); }
        }

        #region TextBox OnlyNumbers PreviewKeyDown function
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PLFuncions.TextBox_OnlyNumbers_PreviewKeyDown(sender, e);
        }
        #endregion

        /// <summary>
        /// Try to send the station to be removed = not active.
        /// Occurding to instuctions the station will be removed and no drones can be sent.
        /// The charging Drones will be able to stay till they are freed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                blObject.removeStation(station);
                new StationListWindow(blObject).Show();
                this.Close();
            }
            catch (BO.Exceptions.ObjExistException e1)
            {
                PLFuncions.messageBoxResponseFromServer("Remove Station", e1.Message);
            }   
        }
    }
}
