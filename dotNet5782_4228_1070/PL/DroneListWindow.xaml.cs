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
    public partial class DroneListWindow : Window
    {
        private Ibl blObjectH;

        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            DroneListView.ItemsSource = blObjectH.DisplayDronesToList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            ChosenStatus.Visibility = Visibility.Hidden;
            ChosenWeight.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Display DroneToList occurding to both conditions: 
        /// StatusSelector.SelectedItem and WeightSelector.SelectedItem;
        /// if they are null = -1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelectorANDWeightSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object status = StatusSelector.SelectedItem;
            object weight = WeightSelector.SelectedItem;
            if (weight!=null)
            {
                weight = WeightSelector.SelectedItem;
                ChosenWeight.Visibility = Visibility.Visible;
                ChosenWeightText.Text = WeightSelector.SelectedItem.ToString();
            }
            else
            {
                weight = -1;
                ChosenWeight.Visibility = Visibility.Hidden;
            }
            if (status != null)
            {
                status = StatusSelector.SelectedItem;
                ChosenStatus.Visibility = Visibility.Visible;
                ChosenStatusText.Text = StatusSelector.SelectedItem.ToString();
            }
            else
            {
                status = -1;
                ChosenStatus.Visibility = Visibility.Hidden;
            }
            List<DroneToList> b = blObjectH.DisplayDroneToListByWeightAndStatus((int)weight ,(int)status);
            DroneListView.ItemsSource = b;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            new MainWindow(blObjectH).Show();
            this.Close();
        }

        private void AddDroneButtonClick(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObjectH).Show();
            this.Close();
        }

        private void DroneSelection(object sender, MouseButtonEventArgs e)
        {
            Drone drone = blObjectH.convertDroneToListToDrone((DroneToList)DroneListView.SelectedItem);
            new DroneWindow(blObjectH, drone).Show();
            this.Close();
        }

        private void DroneListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ChangeStatusToNull(object sender, MouseButtonEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            ChosenStatus.Visibility = Visibility.Hidden;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
        }
        private void ChangeWeightToNull(object sender, MouseButtonEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            ChosenWeight.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
        }
    }
}