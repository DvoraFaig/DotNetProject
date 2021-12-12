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
        BLDrone dr;
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
            droneListWindowForBacking = droneListWindow;
            blObjectD = blObject;
            updateOrAddWindow = true;
            displayWindowAddOrUpdate();
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            IdTextBox.Text = "Drone id....";
            ModelTextBox.Text = "Model id....";
            StationIdTextBox.Text = "Station Id...";
            blObjectD = blObject;
        }
        public DroneWindow(IBL.Ibl blObject, IBL.BO.BLDrone drone)
        {
            InitializeComponent();
            Loaded += ToolWindow_Loaded; //The x button
            droneListWindowForBacking = droneListWindow;
            updateOrAddWindow = false;
            displayWindowAddOrUpdate();
            IdTextBox.Text = $"{drone.Id}";
            ModelTextBox.Text = $"{ drone.Model}";
            DroneWeightUpdate.Text = $"{drone.MaxWeight}";
            BatteryTextBox.Text = $"{drone.Battery}";
            StatusTextBox.Text = $"{drone.Status}";
            //PositionDroneTextBox.Text = $"({drone.DronePosition.Latitude},{drone.DronePosition.Longitude})";
            
            if (drone.ParcelInTransfer == null)
            {

                // ParcelOfDroneInfo.Visibility = Visibility.Hidden;
            }
            ChargeButton.IsEnabled = drone.Status == DroneStatus.Available ? true : false;
            blObjectD = blObject;
            dr = drone;
        }
        private void displayWindowAddOrUpdate()
        {
            Visibility visibility;
            //false == show update window
            //true == show add window
            visibility = (updateOrAddWindow == false) ? Visibility.Hidden: visibility = Visibility.Visible;
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
        }

        private static void TrueOrFalseDisplay()
        {

        }

        void ToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        // TextChangedEventHandler delegate method.
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            // Omitted Code: Insert code that does something whenever
            // the text changes...
        } // end textChangedEventHandler


        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
*/
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void DroneWeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            int weightCategory = Convert.ToInt32((IDal.DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            try
            {
                blObjectD.AddDrone(Convert.ToInt32(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                TextBlock addedDrone = new TextBlock();
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

                // MessageBox.Show(e.Message, "Drone Error");
            }

        }

        private void Button_ClickRestart(object sender, RoutedEventArgs e)
        {
            IdTextBox.Text = "Id...";
            ModelTextBox.Text = "Model....";
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            StationIdTextBox.Text = "Station id...";
        }
        //private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //    //IdTextBox.Text = " ";
        //}
        //private void ModelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}
        //private void StationIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        private void Button_ClickReturnToPageDroneListWindow(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                new DroneListWindow(blObjectD).Show();
                this.Close();
            }
        }

        private void ModelTextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

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
            }
            catch(Exception)
            {
                
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

        }

        private void PickParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PackageDelivery_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
