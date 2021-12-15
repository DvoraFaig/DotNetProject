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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.Ibl blObjectD;
        Drone dr;
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
        public DroneWindow(IBL.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            blObjectD = blObject;
            updateOrAddWindow = true;
            displayWindowAddOrUpdate();
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            IdTextBox.Text = "Drone id....";
            ModelTextBox.Text = "Model id....";
            StationIdTextBox.Text = "Station Id...";
        }
        public DroneWindow(IBL.Ibl blObject, IBL.BO.Drone drone)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            updateOrAddWindow = false;
            displayWindowAddOrUpdate();
            blObjectD = blObject;
            dr = drone;
            IdTextBox.Text = $"{drone.Id}";
            ModelTextBox.Text = $"{ drone.Model}";
            DroneWeightUpdate.Text = $"{drone.MaxWeight}";
            BatteryTextBox.Text = $"{drone.Battery}";
            StatusTextBox.Text = $"{drone.Status}";
            PositionDroneTextBox.Text = $"({drone.DronePosition.Latitude},{drone.DronePosition.Longitude})";
            
            if (drone.ParcelInTransfer == null)
            {
                ParcelTextBoxLabel.Visibility = Visibility.Hidden;
                ParcelIdIdTextBox.Visibility = Visibility.Hidden;
            }
            // ======================================
            // set type of buttons
            // ======================================
            // charge Button
            ChargeButton.Content = drone.Status == DroneStatus.Maintenance ? "Free Drone From Charge" : "Send Drone To Charge" ;    
            ChargeButton.Visibility = drone.Status == DroneStatus.Maintenance ? Visibility.Visible : Visibility.Hidden;
            // Delivery status Button
            //DeliveryStatusButton.IsEnabled = drone.Status == DroneStatus.Maintenance ? false : true;

            if (drone.Status == DroneStatus.Maintenance) 
                DeliveryStatusButton.Visibility = Visibility.Hidden;
            else
            {
                try
                {
                    int contentIndex = blObjectD.GetDroneStatusInDelivery(dr);
                    DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
                    DeliveryStatusButton.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    DeliveryStatusButton.Visibility = Visibility.Hidden;
                }
            }
        }
        /// <summary>
        /// Display DroneWindow Add or Update
        /// false == show update window
        /// true == show add window
        /// </summary>
        private void displayWindowAddOrUpdate()
        {
            Visibility visibility;
            visibility = (updateOrAddWindow == false) ? Visibility.Hidden: Visibility.Visible;
            labelAddADrone.Visibility = visibility;
            IdTextBoxLabel.Visibility = visibility;
            IdTextBox.Visibility = visibility;
            ModelTextBoxLabel.Visibility = visibility;
            ModelTextBox.Visibility = visibility;
            DroneWeightSelectorLabel.Visibility = visibility;
            DroneWeightSelector.Visibility = visibility;
            StationIdTextBoxLabel.Visibility = visibility;
            StationIdTextBox.Visibility = visibility;
            RestartButton.Visibility = visibility;
            AddlButton.Visibility = visibility;

            visibility = (visibility == Visibility.Hidden) ? Visibility.Visible: Visibility.Hidden;
            if (updateOrAddWindow == false)//if Add Drone don't go in
            {
                IdTextBoxLabel.Visibility = visibility;
                IdTextBox.Visibility = visibility;
                IdTextBox.IsReadOnly = true;
                ModelTextBoxLabel.Visibility = visibility;
                ModelTextBox.Visibility = visibility;                
            }
            DroneWeightLabel.Visibility = visibility;
            DroneWeightUpdate.Visibility = visibility;
            BatteryTextBoxLabel.Visibility = visibility;
            BatteryTextBox.Visibility = visibility;
            StatusTextBoxLabel.Visibility = visibility;
            StatusTextBox.Visibility = visibility;
            PositionDroneTLabel.Visibility = visibility;
            PositionDroneTextBox.Visibility = visibility;
            ParcelTextBoxLabel.Visibility = visibility;
            ParcelIdIdTextBox.Visibility = visibility;
            UpdateButton.Visibility = visibility;
            ChargeButton.Visibility = visibility;
            TimeTochargeText.Visibility = visibility;
            TimeTocharge.Visibility = visibility;
            DeliveryStatusButton.Visibility = visibility;
        }

        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

    

        private void ButtoClickAdd(object sender, RoutedEventArgs e)
        {
            int weightCategory = Convert.ToInt32((IDal.DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            try
            {
                blObjectD.AddDrone(Convert.ToInt32(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                TextBlock addedDrone = new TextBlock();
                new DroneListWindow(blObjectD).Show();
                this.Close();

            }
            catch (FormatException)
            {
                Console.WriteLine("== ERROR receiving data ==");
            }
            catch (OverflowException)
            {
                Console.WriteLine("== ERROR receiving data ==");
            }
            catch (Exception)
            {
                MessageBox.Show("Cann't add a drone", "Drone Error");
            }

        }

        private void ButtonClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "Id...";
            ModelTextBox.Text = "Model....";
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            StationIdTextBox.Text = "Station id...";
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
            dr.Model = ModelTextBox.Text;
            blObjectD.DroneChangeModel(dr);
            new DroneListWindow(blObjectD).Show();
            this.Close();
        }

        private void ChargeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ChargeButton.Content.ToString() == "Send Drone To Charge")
            {
                try
                {
                    blObjectD.SendDroneToCharge(dr.Id);
                    StatusTextBox.Text = $"{dr.Status}";
                }
                catch (Exception)
                {

                }
            }
            else
            {
                if (TimeTocharge.Text != null)
                {
                    try
                    {
                        blObjectD.FreeDroneFromCharging(dr.Id, int.Parse(TimeTocharge.Text));
                        StatusTextBox.Text = $"{dr.Status}";
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void FreeChargeButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SendDroneToChargeClick(object sender, RoutedEventArgs e)
        {
            string contentClickedButton = DeliveryStatusButton.Content.ToString();
            if (contentClickedButton == deliveryButtonOptionalContent[0])
            {
                try { blObjectD.PairParcelWithDrone(dr.Id); }
                catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                catch (Exception e2) { MessageBox.Show(e2.Message); }
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[1])
            {
                try { blObjectD.DronePicksUpParcel(dr.Id); }
                catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                catch (Exception e2) { MessageBox.Show(e2.Message); }
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[2])
            {
                try { blObjectD.DeliveryParcelByDrone(dr.Id); }
                catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                catch (Exception e2) { MessageBox.Show(e2.Message); }
            }
        }
    }
}
