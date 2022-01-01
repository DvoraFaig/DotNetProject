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
    public partial class DroneWindow : Window
    {
        private BlApi.Ibl blObjectD;
        BO.Drone dr;
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
        /// Ctor display the add a drone Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        public DroneWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            blObjectD = blObject;
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Ctor display the update/see info a specific drone Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        /// <param name="drone">The drone to update/see info</param>
        public DroneWindow(BlApi.Ibl blObject, BO.Drone drone)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            blObjectD = blObject;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
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
            setDeliveryButton();

            ChargeButton.Visibility = drone.Status == DroneStatus.Delivery ? Visibility.Hidden : Visibility.Visible;
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
        /// Content of a btn in the update form occurding to the drones' status.
        /// </summary>
        private void setDeliveryButton()
        {
            switch (dr.Status)
            {
                case DroneStatus.Available:
                    ChargeButton.Content = "Send Drone To Charge";
                    break;
                case DroneStatus.Maintenance:
                    ChargeButton.Content = "Free Drone From Charge";
                    break;
            }
        }

        /// <summary>
        /// Avoid the display of the X closing btn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        /// <summary>
        /// Send the drone to an addition func.
        /// If succeed: Go to DroneListWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneClickBtn(object sender, RoutedEventArgs e)
        {
            int weightCategory = Convert.ToInt32((DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            try
            {
                // didn't sent an object Drone becuase most of the props values are filled in BL automatic.
                blObjectD.AddDrone(int.Parse(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }
            #region catch exeptions
            catch (BO.Exceptions.ObjExistException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data or enter a different Id ==\nPlease try again");
            }
            catch (ArgumentNullException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone","== ERROR receiving data ==\nPlease try again");
            }
            catch (FormatException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (OverflowException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (NullReferenceException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (Exception)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "Cann't add a drone");
            }
            #endregion
        }

        /// <summary>
        /// Restart textBox(es) and selector content in add drone form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// RestartTextBoxesAndSelectorBtnClick
        private void RestartTextBoxesAndSelectorBtnClick(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "";
            ModelTextBox.Text = "";
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(DO.WeightCategories));
            StationIdTextBox.Text = "";
        }

        /// <summary>
        /// Return To DroneListWindow.
        /// Ensure if the worker wants to exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToDroneListWindowBtnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }
        }

        /// <summary>
        /// Try to send the drone with the new name to an update func.
        /// If succeed: go to DroneListWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                dr.Model = ModelTextBox.Text;
                blObjectD.ChangeDroneModel(dr);
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }
            catch (InvalidOperationException exeptionInvalid) { PLFuncions.messageBoxResponseFromServer("Change Drones' Model", exeptionInvalid.Message); };
        }

        /// <summary>
        /// Try to send a drone to charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ChargeButton.Content.ToString() == "Send Drone To Charge")
            {
                try
                {
                    blObjectD.SendDroneToCharge(dr.Id);
                    StatusTextBox.Text = $"{dr.Status}";
                    setDeliveryButton();
                }
                catch (BO.Exceptions.ObjNotExistException ex) { PLFuncions.messageBoxResponseFromServer("Charge Drone", $"{ex.Message} can't charge now."); }
                catch (BO.Exceptions.ObjNotAvailableException) { PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
                catch (Exception){ PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
            }
            else
            {
                if (TimeTocharge.Text == "")
                {
                   PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge","ERROR\nEnter time to charge");
                }
                else
                {
                    try
                    {
                        blObjectD.FreeDroneFromCharging(dr.Id, int.Parse(TimeTocharge.Text));
                        StatusTextBox.Text = $"{dr.Status}";
                        setDeliveryButton();
                        TimeTocharge.Text = "";
                    }
                    catch (Exception)
                    {
                        PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
                    }
                }
            }
        }

        /// <summary>
        /// The btn:
        /// Content is: 
        /// if "Send To Delivery" :
        /// Check if valid to send a specific drone to charge.
        /// if valid: send to a function that pairs drones with a parcel.
        /// if  "Pick Up Parcel":
        /// Try to send to a function where drone can pick up the parcel.
        /// if "Which Package Delivery" :
        /// Try to send to a function where drone will delivere the package.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneOccurdingToStatusBtnClick(object sender, RoutedEventArgs e)
        {
            string contentClickedButton = DeliveryStatusButton.Content.ToString();
            if (contentClickedButton == deliveryButtonOptionalContent[0]) // Send To Delivery = pair with a parcel
            {
                try { blObjectD.PairParcelWithDrone(dr.Id); }
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone",e1.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone" , e2.Message); }
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[1]) // Pick Up Parcel
            {
                try { blObjectD.DronePicksUpParcel(dr.Id); }
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone" ,e2.Message); }
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[2]) // Which Package Delivery - to delivere the package
            {
                try { blObjectD.DeliveryParcelByDrone(dr.Id); }
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
            }
        }

        #region TextBox OnlyNumbers PreviewKeyDown function
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PLFuncions.TextBox_OnlyNumbers_PreviewKeyDown(sender, e);
        }
        #endregion
    }
}
