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
        public string ShowDronesList;
        Ibl blObjectH;
        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            ListBox droneList = new ListBox();
            droneList.Margin = new Thickness(154, 0, 0, 0);
            List<IBL.BO.BLDrone> drones = blObject.DisplayDrones();
            foreach(IBL.BO.BLDrone drone in drones)
            {
                droneList.Items.Add(drone);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsInitialized) return;

            ComboBoxItem item = StatusList.SelectedItem as ComboBoxItem;
            switch (item.Name)
            {
                case "Available":
                    ShowDronesList = "available drones";
                    //ShowDronesList = blObjectH.DisplayAvailableDrones();
                    break;
                case "Maintenance":
                    ShowDronesList = "Maintenance drones";
                    break;
                case "Delivery":
                    ShowDronesList = "Delivery drones";
                    //ShowDronesList = blObjectH.DisplayDeliveryDrones();
                    break;
                default:
                    break;
            }
        }
    }
}
