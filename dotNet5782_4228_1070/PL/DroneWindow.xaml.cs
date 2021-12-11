using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.Ibl blObjectD;
        BLDrone dr;
        public DroneWindow(IBL.Ibl blObject)
        {
            InitializeComponent();
            UpdateDroneDisplay.Visibility = Visibility.Hidden;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IDal.DO.WeightCategories));
            IdTextBox.Text = "Drone id....";
            ModelTextBox.Text = "Model id....";
            blObjectD = blObject;

        }

        private void OnClosing1(object sender, CancelEventArgs e)
        {
           e.Cancel = true;
        }
        // TextChangedEventHandler delegate method.
        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            // Omitted Code: Insert code that does something whenever
            // the text changes...
        } // end textChangedEventHandler
        public DroneWindow(IBL.Ibl blObject, IBL.BO.BLDrone drone)
        {
            InitializeComponent();
            AddDroneDisplay.Visibility = Visibility.Hidden;
            blObjectD = blObject;
            dr = drone;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_ClickAdd(object sender, RoutedEventArgs e)
        {
            int weightCategory = Convert.ToInt32((IDal.DO.WeightCategories)WeightSelector.SelectedIndex);
            try
            {
                blObjectD.AddDrone(Convert.ToInt32(IdTextBox.Text), ModelTextBox.Text, weightCategory, Convert.ToInt32(StationIdTextBox.Text));
                TextBlock addedDrone= new TextBlock();
                addedDrone.TextDecorations = TextDecorations.Strikethrough;
                // Create an underline text decoration. Default is underline.
                TextDecoration myUnderline = new TextDecoration();

                // Create a solid color brush pen for the text decoration.
                myUnderline.Pen = new Pen(Brushes.Red, 1);
                myUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;

                // Set the underline decoration to a TextDecorationCollection and add it to the text block.
                TextDecorationCollection myCollection = new TextDecorationCollection();
                myCollection.Add(myUnderline);
                addedDrone.TextDecorations = myCollection;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Drone Error");
            }

        }

        private void Button_ClickCancel(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
            if (messageBoxClosing == MessageBoxResult.OK)
            {
                this.Close();
            }
            else
            {

            }
        }

        private void StationIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ModelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //private void onClosing1(object sender, EventArgs e)
        //{
        //}
    }
}
