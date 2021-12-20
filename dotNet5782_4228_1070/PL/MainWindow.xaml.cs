using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Ibl blObject;
        public enum ShowObjects { Drone, Station };

        public MainWindow()
        {
            InitializeComponent();
            blObject = BlApi.IBL.BLFactory.Factory();
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("https://he.wikipedia.org/wiki/%D7%A8%D7%97%D7%A4%D7%9F#/media/%D7%A7%D7%95%D7%91%D7%A5:Quadcopter_camera_drone_in_flight.jpg"));
        }
        public MainWindow(Ibl bl)
        {
            InitializeComponent();
            blObject = bl;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("https://he.wikipedia.org/wiki/%D7%A8%D7%97%D7%A4%D7%9F#/media/%D7%A7%D7%95%D7%91%D7%A5:Quadcopter_camera_drone_in_flight.jpg"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObject).Show();
            this.Close();
        }
        private void ButtonClickStation(object sender, RoutedEventArgs e)
        {
            new StationListWindow(blObject).Show();
            this.Close();
        }
        private void ButtonClickParcel(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow_(blObject).Show();
            this.Close();
        }

        private void ChangeBackGround(object sender, MouseEventArgs e)
        {
            GoToDroneListWindow.Background = Brushes.Transparent;
        }

        private void ChangeBackTheBackGround(object sender, MouseEventArgs e)
        {
            GoToDroneListWindow.Background = Brushes.White;

        }

        /// <summary>
        /// Closing MainWindow
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed) // X button
            {
                MessageBoxResult messageBoxClosing = MessageBox.Show("Are you Sure you wan't to exit", "GoodBy", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxClosing == MessageBoxResult.OK)
                    base.OnClosing(e);
                else
                    e.Cancel = true;
            }
            base.OnClosing(e);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(blObject).Show();
            this.Close();
        }


    }
}
