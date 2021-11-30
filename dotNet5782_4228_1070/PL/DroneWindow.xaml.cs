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
        public DroneWindow(IBL.Ibl blObject)
        {
            InitializeComponent();
            blObjectD = blObject;
        }

        public DroneWindow(IBL.Ibl blObject, IBL.BO.BLDrone drone)
        {
            InitializeComponent();
            blObjectD = blObject;
            dr = drone;
            CancelButton.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
    }
}
