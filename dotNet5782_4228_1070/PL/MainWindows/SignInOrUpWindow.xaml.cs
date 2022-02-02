﻿using System;
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
        public SignInOrUpWindow()
        {
            InitializeComponent();
            this.blObject = BlApi.IBL.BLFactory.Factory();
            //Response.Visibility = Visibility.Hidden;
        }

        public SignInOrUpWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            this.blObject = blObject;
            //GetPassword.Visibility = Visibility.Hidden;
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
            }
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
                BO.Customer client = blObject.GetCustomerByIdAndName(int.Parse(IdTextBox.Text), passwordBoxCustomer.Password.ToString());
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
                customer.CustomerAsSender = new List<BO.ParcelAtCustomer>();
                customer.CustomerAsTarget = new List<BO.ParcelAtCustomer>();
                try
                {
                    blObject.AddCustomer(customer);
                }
                catch (BO.Exceptions.DataChanged e1) { PLFuncions.messageBoxResponseFromServer("Add a Customer", $"Customer was added successfully\n{e1.Message}"); }

                bool isClient = true;
                new CustomerWindow(blObject, customer, isClient).Show();
                this.Close();
            }
            #region catch Exceptions
            //catch (ArgumentNullException) { messageBoxResponseFromServer("ArgumentNullException"); clearFormTextBox(); }
            //catch (FormatException) { messageBoxResponseFromServer("FormatException"); clearFormTextBox(); }
            //catch (OverflowException) { messageBoxResponseFromServer("OverflowException"); clearFormTextBox(); }
            //catch (BO.Exceptions.ObjNotExistException serverException) { messageBoxResponseFromServer(serverException.Message); }
            catch (BO.Exceptions.ObjExistException serverException) { messageBoxResponseFromServer(serverException.Message); clearFormTextBox(); }
            catch (Exception exception) { messageBoxResponseFromServer(exception.Message); clearFormTextBox(); }
            #endregion
        }

        /// <summary>
        /// clear form textBoxes from text.
        /// </summary>
        private void clearFormTextBox()
        {
            PLFuncions.clearFormTextBox(SignUpIdTextBox, SignUpNameTextBox, SignUpPhoneTextBox, SignUpLatitudeTextBox, SignUpLongitudeTextBox);
        }

        private void signUpAsAWorker(object sender, RoutedEventArgs e)
        {
            try
            {
                bool ifWorkwerExist = blObject.ifWorkerExist(new BO.Worker()
                {
                    Id = int.Parse(WorkerIdTextBox.Text),
                    Password = passwordBox.Password.ToString()//WorkerPasswordTextBox.Text
                });
                if (ifWorkwerExist)
                {
                    new MainWindow().Show();
                    this.Close();
                }
                else
                    messageBoxResponseFromServer("Sorry...\nYou don't exist");
            }
            catch (ArgumentNullException) { messageBoxResponseFromServer("ArgumentNullException"); clearFormTextBox(); }
            catch (FormatException) { messageBoxResponseFromServer("FormatException"); clearFormTextBox(); }
            catch (OverflowException) { messageBoxResponseFromServer("OverflowException"); clearFormTextBox(); }
            //catch (BO.Exceptions.ObjNotExistException serverException) { messageBoxResponseFromServer(serverException.Message); }
            catch (BO.Exceptions.ObjExistException serverException) { messageBoxResponseFromServer(serverException.Message); clearFormTextBox(); }
            catch (Exception exception) { messageBoxResponseFromServer($"{exception.Message}\nOr {passwordBox.Password.ToString()} doesn't match id: {WorkerIdTextBox.Text}." ); clearFormTextBox(); }
        }

        private void getPassword(object sender, MouseButtonEventArgs e)
        {

            //passwordBox.Password = passwordBox.Password.ToString();
            passwordTextBox.Text = passwordBox.Password.ToString();
            passwordTextBox.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Hidden;
            HidePassword.Visibility = Visibility.Hidden;
            GetPassword.Visibility = Visibility.Visible;
        }

        private void hidePassword(object sender, MouseButtonEventArgs e)
        {
            //passwordBox.Password = passwordBox.Password.ToString();
            passwordTextBox.Visibility = Visibility.Hidden;
            passwordBox.Visibility = Visibility.Visible;
            HidePassword.Visibility = Visibility.Visible;
            GetPassword.Visibility = Visibility.Hidden;
        }

        private void hidePasswordCustomer(object sender, MouseButtonEventArgs e)
        {
            //passwordBox.Password = passwordBoxCustomer.Password.ToString();
            passwordTextBoxCustomer.Visibility = Visibility.Hidden;
            passwordBoxCustomer.Visibility = Visibility.Visible;
            HidePasswordCustomer.Visibility = Visibility.Visible;
            GetPasswordCustomer.Visibility = Visibility.Hidden;
        }
        private void getPasswordCustomer(object sender, MouseButtonEventArgs e)
        {
            passwordTextBoxCustomer.Text = passwordBoxCustomer.Password.ToString();
            passwordTextBoxCustomer.Visibility = Visibility.Visible;
            passwordBoxCustomer.Visibility = Visibility.Hidden;
            HidePasswordCustomer.Visibility = Visibility.Hidden;
            GetPasswordCustomer.Visibility = Visibility.Visible;
        }
    }
}
