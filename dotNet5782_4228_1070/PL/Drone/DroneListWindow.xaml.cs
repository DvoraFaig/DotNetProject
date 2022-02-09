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
using BO;
using BlApi;
using System.Collections.ObjectModel;
//using PO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>

    public partial class DroneListWindow : Window
    {
        private Ibl blObjectH;
        //CollectionView view;
        private PO.Drones currentDroneList;
        //private ObservableCollection<PO.DroneToList> currentDroneList2 = new ObservableCollection<PO.DroneToList>();
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public DroneListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            Loaded += ToolWindowLoaded;//The x button
            //DroneListView.ItemsSource = blObjectH.DisplayDronesToList();
            currentDroneList = new PO.Drones(blObjectH);
            DroneListView.DataContext = currentDroneList.DroneList;
            currentDroneList.getNewList(blObjectH.returnDronesToList());
            //new
            //DroneListView.DataContext = currentDroneList2;
            //currentDroneList2.Add(currentDroneList.DroneList[0]);
            //currentDroneList2.Add(currentDroneList.DroneList[1]);
            //currentDroneList2.Add(currentDroneList.DroneList[2]);
            //DroneListView.DataContext = (IEnumerable<PO.DroneToList>)currentDroneList.DroneList;
            //IEnumerable<DroneToList> dronesToList = blObject.returnDronesToList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ChosenStatus.Visibility = Visibility.Hidden;
            ChosenWeight.Visibility = Visibility.Hidden;
            //DataContext = dronesToList;
            //view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.DataContext);
        }

        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
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
            //currentDroneList2.Add(currentDroneList.DroneList[3]);
            
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
            IEnumerable<DroneToList> b = blObjectH.DisplayDroneToListByFilters((int)weight ,(int)status);
            //DroneListView.ItemsSource = b;
            currentDroneList.getNewList(b);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            //System.Windows.Application.Current.Shutdown();
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
                App.Current.Windows[intCounter].Close();

            new MainWindow(blObjectH).Show();
            this.Close();
            //Environment.Exit(0);
            //this.Close();
        }

        private void AddDroneButtonClick(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blObjectH).Show();
            //this.Close();
        }

        private void DroneSelection(object sender, MouseButtonEventArgs e)
        {
            PO.DroneToList droneToList = (PO.DroneToList)DroneListView.SelectedItem;
            Drone drone = blObjectH.GetDroneById(droneToList.Id);////changed frrom get with specific...
            new DroneWindow(blObjectH, drone).Show();
            //this.Close();
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
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /*private void sortDronesByStatus(object sender, RoutedEventArgs e)
        {
            string propertyToGroup = "droneStatus";
            view.GroupDescriptions.Clear();
            PropertyGroupDescription property = new PropertyGroupDescription($"{propertyToGroup}");
            view.GroupDescriptions.Add(property);
        }*/
    }
}