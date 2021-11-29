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
using IBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        Ibl blObjectH;
        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            List<IBL.BO.BLDrone> drones = blObject.DisplayDrones();
            foreach(IBL.BO.BLDrone drone in drones)
            {
                DroneList.Items.Add(drone);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsInitialized) return;

            ComboBoxItem item = StatusList.SelectedItem as ComboBoxItem;
            switch (item.Name)
            {
                case "Available":
                    DroneList.Items.Clear();
                    //ShowDronesList = blObjectH.DisplayAvailableDrones();
                    break;
                case "Maintenance":
                    DroneList.Items.Clear();
                    //ShowDronesList = "Maintenance drones";
                    break;
                case "Delivery":
                    //this.Dispatcher.BeginInvoke();
                    //ShowDronesList = "Delivery drones";
                    //ShowDronesList = blObjectH.DisplayDeliveryDrones();
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new MainWindow();
            win2.Show();
            this.Close();
        }
    }
}
