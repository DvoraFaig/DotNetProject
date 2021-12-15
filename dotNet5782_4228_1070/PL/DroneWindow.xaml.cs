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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.Ibl blObjectD;
        //BLDroneToList dr;
        Drone dr;
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
            Loaded += ToolWindow_Loaded;//The x button
            blObjectD = blObject;
            updateOrAddWindow = true;
            displayWindowAddOrUpdate();
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            IdTextBox.Text = "Drone id....";
            ModelTextBox.Text = "Model id....";
            StationIdTextBox.Text = "Station Id...";
            blObjectD = blObject;
        }

        public DroneWindow(IBL.Ibl blObject, IBL.BO.Drone drone)
        {
            InitializeComponent();
            blObjectD = blObject;
            dr = drone;
            Loaded += ToolWindow_Loaded; //The x button
            updateOrAddWindow = false;
            displayWindowAddOrUpdate();
            IdTextBox.Text = $"{drone.Id}";
            ModelTextBox.Text = $"{ drone.Model}";
            DroneWeightUpdate.Text = $"{drone.MaxWeight}";
            BatteryTextBox.Text = $"{drone.Battery}";
            //StatusTextBox.Text = $"{drone.Status}";
            //PositionDroneTextBox.Text = $"({drone.DronePosition.Latitude},{drone.DronePosition.Longitude})";

            this.DataContext = drone;
            if (drone.ParcelInTransfer == null)
            {
                ParcelTextBoxLabel.Visibility = Visibility.Hidden;
                ParcelIdIdTextBox.Visibility = Visibility.Hidden;
            }
            // ======================================
            // set type of buttons
            // ======================================
            // charge Button
            ChargeButton.Visibility = Visibility.Visible;
            ChargeButton.Content = drone.Status == DroneStatus.Maintenance ? "Free Drone From Charge" : "Send Drone To Charge";
            if ((drone.Status == DroneStatus.Delivery)) ChargeButton.Visibility = Visibility.Hidden;
            ChargeButton.IsEnabled = drone.Status == DroneStatus.Maintenance ? false : true;
            // Delivery status Button
            DeliveryStatusButton.IsEnabled = drone.Status == DroneStatus.Maintenance ? false : true;
            //For what it?
            /*ParcelTextBoxLabel.Visibility = Visibility.Hidden;
            ParcelIdIdTextBox.Visibility = Visibility.Hidden;*/
            DeliveryStatusButton.Content = actionAlowedDrone(drone).ToString();
            // ======================================
        }

        private DeliveryStatusAction actionAlowedDrone(Drone drone)
        {
            // Before asign parcel to drone
            if (drone.Status == DroneStatus.Available) { return DeliveryStatusAction.SendToDelivery; }
            else if (drone.ParcelInTransfer != null && drone.Status == DroneStatus.Delivery)
            {
                // Before customer get the parcel
                if (drone.DronePosition == drone.ParcelInTransfer.SenderPosition) { return DeliveryStatusAction.SendToDelivery; }
                // Before pick parcel
                else { return DeliveryStatusAction.PickUpParcel; }
            }
            // Do not happen: (Need for the correct function return value)
            return DeliveryStatusAction.other;
        }

        private void displayWindowAddOrUpdate()
        {
            Visibility visibility;
            visibility = (updateOrAddWindow == false) ? Visibility.Hidden : visibility = Visibility.Visible;
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

            visibility = (visibility == Visibility.Hidden) ? Visibility.Visible : Visibility.Hidden;
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
        /// <summary>
        /// Removes close box = X from window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        // TextChangedEventHandler delegate method.
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            // Omitted Code: Insert code that does something whenever
            // the text changes...
        } // end textChangedEventHandler

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// Add a drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                int weightCategory = Convert.ToInt32((IDal.DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
                blObjectD.AddDrone(Convert.ToInt32(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                TextBlock addedDrone = new TextBlock();
                new DroneListWindow(blObjectD).Show();
                this.Close();

            }
            #region Expetions
            catch (IBL.BO.Exceptions.ObjExistException ex)
            {
                MessageBox.Show(ex.Message, "Exist exeption");

            }
            catch (FormatException)
            {
                MessageBox.Show("== ERROR receiving data ==");
            }
            catch (OverflowException)
            {
                MessageBox.Show("== ERROR receiving data ==");
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("== ERROR receiving data ==");
            }
            catch (Exception)
            {
                MessageBox.Show("Cann't add a drone", "Drone Error");
            }
            #endregion

        }
        /// <summary>
        /// Restart TextBox in AddDrone form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "Id...";
            ModelTextBox.Text = "Model....";
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            StationIdTextBox.Text = "Station id...";
        }

        /// <summary>
        /// Return to page DroneListWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ClickReturnToPageDroneListWindow(object sender, RoutedEventArgs e)
        {

            /*MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }*/
            new DroneListWindow(blObjectD).Show();
            this.Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            dr.Model = ModelTextBox.Text;
            blObjectD.DroneChangeModel(dr);
            new DroneListWindow(blObjectD).Show();
            this.Close();
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blObjectD.SendDroneToCharge(dr.Id);
                StatusTextBox.Text = $"{dr.Status}";
                MessageBox.Show("Update");
            }
            catch (Exception)
            {
                MessageBox.Show("Exception");
            }
        }

        private void FreeChargeButton_Click(object sender, RoutedEventArgs e)
        {
            //if The text was changed send to the function
            /*if (TimeTocharge)
            try
            {
                blObjectD.FreeDroneFromCharging(dr.Id, int.Parse(TimeTocharge.Text));
            }
            catch (Exception)
            {

            }*/
        }

        private void SendDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
            //DeliveryStatusAction contentClickedButton;
            Enum.TryParse(DeliveryStatusButton.Content.ToString(), out DeliveryStatusAction contentClickedButton);
            switch (((int)contentClickedButton))
            {
                case 1:
                    try { blObjectD.PairParcelWithDrone(dr.Id); }
                    catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                    catch (Exception e2) { MessageBox.Show(e2.Message); }
                    break;
                case 2:
                    try { blObjectD.DronePicksUpParcel(dr.Id); }
                    catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                    catch (Exception e2) { MessageBox.Show(e2.Message); }
                    break;
                case 3:
                    try
                    { blObjectD.DeliveryParcelByDrone(dr.Id); }
                    catch (IBL.BO.Exceptions.ObjNotExistException e1) { MessageBox.Show(e1.Message); }
                    catch (Exception e2) { MessageBox.Show(e2.Message); }
                    break;
            }

        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObjectD).Show();
            this.Close();
        }
    }
}

//button click Add
//addedDrone.TextDecorations = TextDecorations.Strikethrough;
//// Create an underline text decoration. Default is underline.
//TextDecoration myUnderline = new TextDecoration();

//// Create a solid color brush pen for the text decoration.
//myUnderline.Pen = new Pen(Brushes.Red, 1);
//myUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;

//// Set the underline decoration to a TextDecorationCollection and add it to the text block.
//TextDecorationCollection myCollection = new TextDecorationCollection();
//myCollection.Add(myUnderline);
//addedDrone.TextDecorations = myCollection;
//new DroneListWindow(blObjectD).Show();