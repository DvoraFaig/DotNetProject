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
        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            ListBox droneList = new ListBox();
            List<IBL.BO.BLDrone> drones = blObject.DisplayDrones();
            foreach(IBL.BO.BLDrone drone in drones)
            {
                droneList.Items.Add(drone);
            }
        }
    }
}
