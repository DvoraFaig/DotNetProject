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
using IBL.BO;
using IBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        private Ibl blObjectH;
        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            DroneListView.ItemsSource = blObjectH.DisplayDronesToList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
        }

        /// <summary>
        /// Display DroneToList occurding to both conditions: 
        /// StatusSelector.SelectedItem and WeightSelector.SelectedItem;
        /// if they are null = -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged_AND_WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object status = StatusSelector.SelectedItem;
            object weight = WeightSelector.SelectedItem;
            weight = (weight == null )? -1 : WeightSelector.SelectedItem;
            status = (weight == null ) ? -1 : StatusSelector.SelectedItem;
            List<BLDroneToList> b = blObjectH.DisplayDroneToListByWeightAndStatus((int)weight ,(int)status);
            DroneListView.ItemsSource = b;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(blObjectH).Show();
            this.Close();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObjectH).Show();
            this.Close();
        }

        private void DroneSelection(object sender, MouseButtonEventArgs e)
        {
            //new DroneWindow(blObjectH, (BLDrone)DroneListView.SelectedItem).Show();
            //new DroneWindow(blObjectH, blObjectH.GetDroneById(DroneListView.SelectedItem)).Show();
            new DroneWindow(blObjectH, (BLDroneToList)DroneListView.SelectedItem).Show();
            this.Close();
        }

        private void DroneList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DisplayFullList(object sender, RoutedEventArgs e)
        {
            DroneListView.ItemsSource = blObjectH.DisplayDronesToList();
            StatusSelector.SelectedItem = -1;
            WeightSelector.SelectedItem = -1;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
        }
    }
}


//private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    object item = StatusSelector.SelectedItem;
//    List<BLDroneToList> b = blObjectH.DisplayDroneToListByStatus((DroneStatus)item);
//    DroneListView.ItemsSource = b;
//}
//private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
//{
//    object item = WeightSelector.SelectedItem;
//    List<BLDroneToList> b = blObjectH.DisplayDroneToListByWeight((IDal.DO.WeightCategories)item);
//    DroneListView.ItemsSource = b;
//}