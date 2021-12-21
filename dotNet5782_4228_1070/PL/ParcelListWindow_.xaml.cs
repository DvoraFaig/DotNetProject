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
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelListWindow_.xaml
    /// </summary>
    public partial class ParcelListWindow_ : Window
    {
        private Ibl blObject;
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public ParcelListWindow_(Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            Loaded += ToolWindowLoaded;//The x button
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
            ParcelListView.ItemsSource = blObject.DisplayParcelToList();
            ChosenStatus.Visibility = Visibility.Hidden;
            ChosenWeight.Visibility = Visibility.Hidden;
            ChosenPriority.Visibility = Visibility.Hidden;
        }
        #region ToolWindowLoaded funcion
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }
        #endregion

        /// <summary>
        /// Display ParcelToList occurding to both conditions: 
        /// StatusSelector.SelectedItem and WeightSelector.SelectedItem and PrioritSelector.SelectedItem;
        /// if they are null = -1
        /// </summary>
        public void SelectorsChanges(object sender, SelectionChangedEventArgs e)
        {
            object status = StatusSelector.SelectedItem;
            object weight = WeightSelector.SelectedItem;
            object priority = PrioritySelector.SelectedItem;
            if (weight != null)
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
            if (priority != null)
            {
                priority = PrioritySelector.SelectedItem;
                ChosenPriority.Visibility = Visibility.Visible;
                ChosenPriorityText.Text = PrioritySelector.SelectedItem.ToString();
            }
            else
            {
                priority = -1;
                ChosenPriority.Visibility = Visibility.Hidden;
            }
            List<ParcelToList> b = blObject.DisplayParcelToListByFilters((int)weight, (int)status, (int)priority);
            ParcelListView.ItemsSource = b;
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            new MainWindow(blObject).Show();
            this.Close();
        }

        private void AddDroneButtonClick(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject).Show();
            this.Close();
        }
        
        private void ParcelSelection(object sender, MouseButtonEventArgs e)
        {
            ParcelToList selectedParcel = (ParcelToList)ParcelListView.SelectedItems;
            Parcel parcel = blObject.GetParcelById(selectedParcel.Id);
            new ParcelWindow(blObject, parcel);
            this.Close();
        }

        private void ChangeWeightToNull(object sender, MouseButtonEventArgs e)
        {
            WeightSelector.SelectedItem = null;
            ChosenWeight.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
        }

        private void ChangeStatusToNull(object sender, MouseButtonEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            ChosenStatus.Visibility = Visibility.Hidden;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
        }
        private void ChangePriorityToNull(object sender, MouseButtonEventArgs e)
        {
            PrioritySelector.SelectedItem = null;
            ChosenPriority.Visibility = Visibility.Hidden;
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
        }
    }
}
