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

namespace PL
{
    /// <summary>
    /// Interaction logic for ClientWeb.xaml
    /// </summary>
    public partial class SignInOrUpWindow : Window
    {
        private BlApi.Ibl blObject;
        BO.Customer customer = new BO.Customer();
        public SignInOrUpWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            //Response.Visibility = Visibility.Hidden;
        }
        private void messageBoxResponseFromServer(String message)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed) // X button
            {
                MessageBoxResult messageBoxClosing = MessageBox.Show(message, "Hi", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxClosing == MessageBoxResult.OK)
                {

                }
                //base.OnClosing(e);
                //else
                //e.Cancel = true;
            }
            //base.OnClosing(e);
        }

        /// <summary>
        /// Log in customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogInCLick(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Customer client = blObject.GetCustomerByIdAndName(int.Parse(IdTextBox.Text), NameTextBox.Text);
                if (client != null)
                {
                    messageBoxResponseFromServer("Sign in Succesfully");
                    new CustomerWindow(blObject, client, true).Show();
                    this.Close();

                }
                else
                    messageBoxResponseFromServer("Please Sign in");
            }
            catch (ArgumentNullException) { messageBoxResponseFromServer("ArgumentNullException"); }
            catch (FormatException) { messageBoxResponseFromServer("FormatException"); }
            catch (OverflowException) { messageBoxResponseFromServer("OverflowException"); }
            catch (BO.Exceptions.ObjNotExistException serverException) { messageBoxResponseFromServer(serverException.Message); }
            catch (Exception exception) { messageBoxResponseFromServer(exception.Message); }
        }

        /// <summary>
        /// SignUp customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignUpClick(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Customer customer = new BO.Customer()
                {
                    Id = int.Parse(SignUpIdTextBox.Text),
                    Name = SignUpNameTextBox.Text,
                    Phone = SignUpPhoneTextBox.Text,
                    CustomerPosition = new BO.Position() { Latitude = int.Parse(SignUpLatitudeTextBox.Text), Longitude = int.Parse(SignUpLongitudeTextBox.Text) }
                };
                blObject.AddCustomer(customer);
                bool isClient = true;
                new CustomerWindow(blObject, customer, isClient).Show();
                this.Close();
            }
            catch (ArgumentNullException) { messageBoxResponseFromServer("ArgumentNullException"); clearFormTextBox(); }
            catch (FormatException) { messageBoxResponseFromServer("FormatException"); clearFormTextBox(); }
            catch (OverflowException) { messageBoxResponseFromServer("OverflowException"); clearFormTextBox(); }
            //catch (BO.Exceptions.ObjNotExistException serverException) { messageBoxResponseFromServer(serverException.Message); }
            catch (BO.Exceptions.ObjExistException serverException) { messageBoxResponseFromServer(serverException.Message); clearFormTextBox(); }
            catch (Exception exception) { messageBoxResponseFromServer(exception.Message); clearFormTextBox();}
            
        }

        /// <summary>
        /// clear form textBoxes from text.
        /// </summary>
        private void clearFormTextBox()
        {
            SignUpIdTextBox.Text = "";
            SignUpNameTextBox.Text = "";
            SignUpPhoneTextBox.Text = "";
            SignUpLatitudeTextBox.Text = "";
            SignUpLongitudeTextBox.Text = "";
        }
    }
}
