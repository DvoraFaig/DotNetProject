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
    public partial class CustomerListWindow : Window
    {
        private Ibl blObjectH;
        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion
        public CustomerListWindow(Ibl blObject)
        {
            InitializeComponent();
            blObjectH = blObject;
            Loaded += ToolWindowLoaded;//The x button
            CustomerListView.ItemsSource = blObjectH.DisplayCustomersToList();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blObject"></param>
        /// <param name="client"></param>
        //public CustomerListWindow(Ibl blObject , BO.Customer client)
        //{
        //    InitializeComponent();
        //    blObjectH = blObject;
        //    Loaded += ToolWindowLoaded;//The x button
        //    CustomerListView.ItemsSource = blObjectH.GetCustomerById(client.Id);
        //}
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            new MainWindow(blObjectH).Show();
            this.Close();
        }

        private void CustomerSelection(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customerToList = (CustomerToList)CustomerListView.SelectedItem;
            Customer customer = blObjectH.GetCustomerById(customerToList.Id);////changed frrom get with specific...
            new CustomerWindow(blObjectH, customer , false ).Show();
            this.Close();
        }

        private void AddCustomerButtonClick(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObjectH).Show();
            this.Close();
        }
    }
}