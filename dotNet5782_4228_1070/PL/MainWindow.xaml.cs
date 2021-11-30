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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using IBL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Ibl blObject;
        public MainWindow()
        {
            InitializeComponent();
            blObject = IBL.IBL.BLFactory.Factory("BL");
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("https://he.wikipedia.org/wiki/%D7%A8%D7%97%D7%A4%D7%9F#/media/%D7%A7%D7%95%D7%91%D7%A5:Quadcopter_camera_drone_in_flight.jpg"));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blObject).Show();
            //this.Close();
        }
    }
}
