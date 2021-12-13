﻿using System;
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
            DroneListView.ItemsSource = blObjectH.DisplayDronesToList();//blObjectH.DisplayDrones();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = StatusSelector.SelectedItem;
            List<BLDroneToList> b = blObjectH.DisplayDroneToListByStatus((DroneStatus)item);
            DroneListView.ItemsSource = b;
        }
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = WeightSelector.SelectedItem;
            List<BLDroneToList> b = blObjectH.DisplayDroneToListByWeight((IDal.DO.WeightCategories)item);
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
            new DroneWindow(blObjectH, (BLDrone)DroneListView.SelectedItem).Show();
            this.Close();
        }

        private void DroneList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
