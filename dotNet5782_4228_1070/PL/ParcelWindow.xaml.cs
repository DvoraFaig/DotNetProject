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
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BlApi.Ibl blObject;
        private Parcel parcel;
        public ParcelWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            initializeAdd();
        }

        public ParcelWindow(BlApi.Ibl blObject, Parcel parcel)
        {
            InitializeComponent();
            this.blObject = blObject;
            this.parcel = parcel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            blObject.RemoveParcel(parcel);
            MessageBox.Show("Parcel was remove");
            this.Close();
        }

        private void initializeAdd()
        {
            ParcelTitle.Content = "Add a Parcel";
            ParcelWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
            ParcelPrioritySelector.ItemsSource = Enum.GetValues(typeof(DO.Priorities));
            ParcelTargetSelector.ItemsSource = blObject.CustomerLimitedDisplay();
            ParcelSenderSelector.ItemsSource = blObject.CustomerLimitedDisplay();
        }
        
        private void ButtonClickAdd(object sender, RoutedEventArgs e)
        {

        }
    }
}
