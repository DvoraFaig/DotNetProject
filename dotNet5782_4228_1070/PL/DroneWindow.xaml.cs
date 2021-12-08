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
        public DroneWindow(IBL.Ibl blObject)
        {
            InitializeComponent();
            UpdateDroneDisplay.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            IdTextBox.Text = "Drone id....";
            ModelTextBox.Text = "Model id....";
            blObjectD = blObject;

        }

        private void OnClosing1(object sender, CancelEventArgs e)
        {
           e.Cancel = true;
        }
        // TextChangedEventHandler delegate method.
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            // Omitted Code: Insert code that does something whenever
            // the text changes...
        } // end textChangedEventHandler
        public DroneWindow(IBL.Ibl blObject, IBL.BO.BLDrone drone)
        {
            InitializeComponent();
            AddDroneDisplay.Visibility = Visibility.Hidden;
            blObjectD = blObject;
            dr = drone;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void onClosing1(object sender, EventArgs e)
        //{
        //}
    }
}
