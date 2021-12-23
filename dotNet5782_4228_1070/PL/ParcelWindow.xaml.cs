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
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BlApi.Ibl blObject;
        private Parcel parcel;
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion

        public ParcelWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            initializeAdd();
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            this.parcel = parcel;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            //visibleUpdateForm.Visibility = Visibility;
            initializeDetails();
        }

        private void initializeDetails()
        {
            IdText.Text = $"{parcel.Id}";
            SenderText.Content = $"{parcel.Sender.name}";
            TargetText.Content = $"{parcel.Target.name}";
            WeightText.Text = $"{parcel.Weight}";
            PriorityText.Text = $"{parcel.Priority}";
            if (parcel.Drone != null) DroneText.Content = $"{parcel.Drone.Id}";
            else
            {
                DroneText.Content = "No Drone";
                DroneText.IsEnabled = false;
            }
        }

        private void initializeAdd()
        {
            //visibleAddForm.Visibility = Visibility;
            ParcelTitle.Content = "Add a Parcel";
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
            ParcelTargetSelector.ItemsSource = blObject.CustomerLimitedDisplay();
            ParcelSenderSelector.ItemsSource = blObject.CustomerLimitedDisplay();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            blObject.RemoveParcel(parcel);
            new ParcelListWindow_(blObject).Show();
            this.Close();
            MessageBox.Show("Parcel was remove");
        }

        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {
            int senderId = ((CustomerInParcel)ParcelSenderSelector.SelectedItem).Id;
            int targetId = ((CustomerInParcel)ParcelTargetSelector.SelectedItem).Id;
            DO.WeightCategories weight = (DO.WeightCategories)ParcelWeightSelector.SelectedItem;
            DO.Priorities priority = (DO.Priorities)ParcelPrioritySelector.SelectedItem;
            try
            {
                blObject.AddParcel(senderId, targetId, weight, priority);
            }
            catch (BO.Exceptions.ObjNotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }
            new ParcelListWindow_(blObject).Show();
            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow_(blObject).Show();
            this.Close();
        }

        private void DroneClick(object sender, RoutedEventArgs e)
        {
            Drone d = blObject.GetDroneById(parcel.Drone.Id);
            new DroneWindow(blObject, d).Show();
            this.Close();
        }

        private void CustomerButton(object sender, RoutedEventArgs e)
        {
            
            Customer c = blObject.GetCustomerById(((CustomerInParcel)(sender as Button).Content).Id);
            new CustomerWindow(blObject, c).Show();
            this.Close();
        }
    }
}
