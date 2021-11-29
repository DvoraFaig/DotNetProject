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
        public Ibl blObject;
        public MainWindow()
        {
            InitializeComponent();
            blObject = IBL.IBL.BLFactory.Factory("BL");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win2 = new DroneListWindow(blObject);
            win2.Show();
            this.Close();
        }
    }
}
