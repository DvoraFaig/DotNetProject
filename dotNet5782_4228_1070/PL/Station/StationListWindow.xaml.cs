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
using BL;
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>

    public partial class StationListWindow : Window
    {
        private IBl blObject;
        public enum ShowObjects { Drone, Station };
        private int ShowWindow;
        CollectionView view;

        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public StationListWindow(IBl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            IEnumerable<StationToList> stationToLists = blObject.GetStationsToList();
            StationListView.ItemsSource = stationToLists;//.Cast<BLStationToList>().ToList();
            DataContext = stationToLists;
            view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
        }

        /// <summary>
        /// Code to remove close box from window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            new MainWindow(blObject).Show();
            this.Close();
        }

        private void AddStationBtnClick(object sender, RoutedEventArgs e)
        {
            new StationWindow(blObject).Show();
            this.Close();
        }

        private void DroneSelection(object sender, MouseButtonEventArgs e)
        {
            //Drone drone = blObjectH.convertDroneToListToDrone((DroneToList)StationListView.SelectedItem);
            //new DroneWindow(blObjectH, drone).Show();
            this.Close();
        }

        //private void ChangeStatusToNull(object sender, MouseButtonEventArgs e)
        //{
        //    //StatusSelector.SelectedItem = null;
        //    //ChosenStatus.Visibility = Visibility.Hidden;
        //    //StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
        //}

        /// <summary>
        /// Find availble charging slots with int amountAvilableSlots;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //private void CheckedAvailbleChargingSlots(object sender, RoutedEventArgs e)
        //{
        //    //AvailbleChargingSlots();
        //}

        /// <summary>
        /// Find availble charging slots with int amountAvilableSlots occurding to the checked box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedAvailbleChargingSlots(object sender, TextChangedEventArgs e)
        {
            //AvailbleChargingSlots();
            //if(AvailbleChargingSlotsChecked.IsChecked == true)
            //    AvailbleChargingSlots();
        }

        ///// <summary>
        ///// Change amount avilable charging slots to null
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ChangeAmountToNull(object sender, MouseButtonEventArgs e)
        //{
        //    amountChargingSlots.Text = "";
        //    StationListView.ItemsSource = blObject.GetStationsWithFreeSlots(); //.Cast<StationToList>().ToList();
        //}

        private void AvailbleChargingSlots(object sender, TextChangedEventArgs e)
        {
            int amountAvilableSlots = 0;
            try
            {
                if (amountChargingSlots.Text.Length == 0)
                    amountAvilableSlots = 0;
                else if (int.Parse(amountChargingSlots.Text) > 0)
                    amountAvilableSlots = int.Parse(amountChargingSlots.Text);
                StationListView.ItemsSource = blObject.GetStationsWithFreeSlots(amountAvilableSlots);
            }
            catch (Exception) { }
        }

        private void StationSelection(object sender, MouseButtonEventArgs e)
        {
            StationToList stationToList = (StationToList)StationListView.SelectedItem;
            Station station = blObject.GetStationById(stationToList.Id);
            new StationWindow(blObject, station).Show();
            this.Close();
        }

        private void sortStationByAvailableSlotsClick(object sender, RoutedEventArgs e)
        {
            string propertyToGroup = "DroneChargeAvailble";
            view.GroupDescriptions.Clear();
            PropertyGroupDescription property = new PropertyGroupDescription($"{propertyToGroup}");
            view.GroupDescriptions.Add(property);
        }

        /// <summary>
        /// Avoid writing chars in amount of available charging slots textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PLFuncions.TextBox_OnlyNumbers_PreviewKeyDown(sender, e);
        }
    }
}