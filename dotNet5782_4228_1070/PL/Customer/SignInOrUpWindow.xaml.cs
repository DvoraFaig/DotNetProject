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
        private void LogInCLick(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.Customer client = blObject.GetCustomerById(int.Parse(IdTextBox.Text));
                if (client != null)
                {
                    messageBoxResponseFromServer("Sign in Succesfully");
                    new CustomerWindow(blObject, client,true).Show();
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
    }
}
