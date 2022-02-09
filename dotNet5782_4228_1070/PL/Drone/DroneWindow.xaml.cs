﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BlApi;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBl blObject;
        //BO.Drone dr;
        PO.Drone currentDrone;
        public enum DeliveryStatusAction { Available, AsignedParcel, PickedParcel, DeliveredParcel };

        string[] deliveryButtonOptionalContent = { "Send To Delivery", "Pick Up Parcel", "Deliver by Target"};
        BO.Drone tempDrone;

        //simulation
        BO.Parcel parcel;
        BO.Customer senedrOfParcel;
        BO.Customer targetOfParcel;

        bool isReturnBtnClick = false;

        bool isSimulationWorking = false;

        #region the closing button
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion

        /// <summary>
        /// Ctor display the add a drone Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        public DroneWindow(IBl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            currentDrone = new PO.Drone(blObject);
            AddDroneDisplay.DataContext = currentDrone;
            visibleAddForm.Visibility = Visibility.Visible;
            visibleUpdateForm.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Ctor display the update/see info a specific drone Form.
        /// </summary>
        /// <param name="blObject">Instance of interface Ibl</param>
        /// <param name="drone">The drone to update/see info</param>
        public DroneWindow(IBl blObject, BO.Drone drone )
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded; //The x button
            this.blObject = blObject;
            visibleAddForm.Visibility = Visibility.Hidden;
            visibleUpdateForm.Visibility = Visibility.Visible;
            currentDrone = new PO.Drone(blObject, drone);
            //BatteryFill.Width =(int)drone.Battery*2;////////////////////////////////
            AddDroneDisplay.DataContext = currentDrone;
            IdTextBox.IsReadOnly = true;
            if (drone.ParcelInTransfer == null)
            {
                ParcelTextBoxLabel.Visibility = Visibility.Hidden;
                ParcelIdIdTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                parcel = blObject.getParcelByDrone(currentDrone.Id);
                senedrOfParcel = blObject.GetCustomerById(parcel.Sender.Id);
                targetOfParcel = blObject.GetCustomerById(parcel.Target.Id);
            }
            //ParcelIdIdTextBox.Text = $"{drone.ParcelInTransfer.Id}";
            //ParcelIdIdTextBox.Text = $"{drone.ParcelInTransfer.Id}";
            setChargeBtn();
            ChargeButton.Visibility = drone.Status == DroneStatus.Delivery ? Visibility.Hidden : Visibility.Visible;
            if (drone.Status == DroneStatus.Maintenance)
                DeliveryStatusButton.Visibility = Visibility.Hidden;
            else
            {
                try
                {
                    int contentIndex = this.blObject.GetDroneStatusInDelivery(currentDrone.BO());
                    if(contentIndex == 3)
                    {
                        setChargeBtn();
                        return;
                    }
                    DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
                    DeliveryStatusButton.Visibility = Visibility.Visible;
                }
                catch (Exception)
                {
                    DeliveryStatusButton.Visibility = Visibility.Hidden;
                }
            }
            if (drone.Status != DroneStatus.Maintenance)
            {
                ChargeDroneTimeGrid.Visibility = Visibility.Hidden;
            }
            visibilityDroneBtns();
            AutomationBtn.Content = "Start Automation";
            tempDrone = currentDrone.BO();/////////////////////////////////////////////////////////////////
        }

        
        /// <summary>
        /// Avoid the display of the X closing btn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Code to remove close box from window
            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        /// <summary>
        /// Send the drone to an addition func.
        /// If succeed: Go to DroneListWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneClickBtn(object sender, RoutedEventArgs e)
        {
            int weightCategory = Convert.ToInt32((WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            try
            {
                blObject.AddDrone(new Drone() { Id = int.Parse(IdTextBox.Text), Model = ModelTextBox.Text, MaxWeight = (BO.WeightCategories)(DroneWeightSelector.SelectedIndex+ 1) }, Convert.ToInt32(StationIdTextBox.Text));
                this.Close();

                // didn't sent an object Drone becuase most of the props values are filled in BL automatic.
                //blObjectD.AddDrone(int.Parse(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                //blObjectD.AddDrone(currentDrone.BO(),Convert.ToInt32(StationIdTextBox.Text));
                ////////new DroneListWindow(blObjectD).Show();
            }

            #region catch exeptions
            catch (BO.Exceptions.ObjExistException e1)
            {
                PLFunctions.messageBoxResponseFromServer("Add Drone", e1.Message);
            }
            //catch (BO.Exceptions.ObjNotAvailableException ee)
            //{
            //    PLFuncions.messageBoxResponseFromServer("Add Drone", ee.Message);
            //}
            //catch (ArgumentNullException)
            //{
            //    PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            //}
            //catch (FormatException)
            //{
            //    PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            //}
            //catch (OverflowException)
            //{
            //    PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            //}
            //catch (NullReferenceException)
            //{
            //    PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            //}
            catch (Exception)
            {
                PLFunctions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            #endregion
        }

        /// <summary>
        /// Restart textBox(es) and selector content in add drone form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// RestartTextBoxesAndSelectorBtnClick
        private void RestartTextBoxesAndSelectorBtnClick(object sender, RoutedEventArgs e)
        {
            PLFunctions.clearFormTextBox(IdTextBox, ModelTextBox, StationIdTextBox);
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// Return To DroneListWindow.
        /// Ensure if the worker wants to exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnToDroneListWindowBtnClick(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            //if (messageBoxClosing == MessageBoxResult.OK)
            //{
            ////new DroneListWindow(blObjectD).Show();
            if (!isSimulationWorking || !simIsAskedToStopButOperationNotCompleted)
            {
                this.Close();
                return;
            }

            if (isSimulationWorking) 
            {
                isReturnBtnClick = true;
                isProgressBarFromReturnBtn = true;
                simIsAskedToStopButOperationNotCompleted = true;
                worker.CancelAsync();
                ProgressBarForSimulation.Visibility = Visibility.Visible;
                //return;
            }

        }

        /// <summary>
        /// Try to send the drone with the new name to an update func.
        /// If succeed: go to DroneListWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //currentDrone.Model = ModelTextBox.Text;
                blObject.ChangeDroneModel(currentDrone.Id, currentDrone.Model);
                //new DroneListWindow(blObjectD).Show();
                this.Close();
            }
            catch (InvalidOperationException exeptionInvalid) { PLFunctions.messageBoxResponseFromServer("Change Drones' Model", exeptionInvalid.Message); };
            //catch (ObjNotExistException exeptionNotExist) { PLFuncions.messageBoxResponseFromServer("Change Drones' Model", exeptionInvalid.Message); };
        }

        /// <summary>
        /// Try to send a drone to charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ChargeButton.Content.ToString() == "Send Drone To Charge")
            {
                try
                {
                    blObject.SendDroneToCharge(currentDrone.Id);
                    //currentDrone.Update(blObject.SendDroneToCharge(currentDrone.Id));

                    //currentDrone = new PO.Drone(blObjectD.SendDroneToCharge(currentDrone.Id));
                    //currentDrone.Status = d.Status;
                    //currentDrone.Battery = d.Battery;
                    //AddDroneDisplay.DataContext = currentDrone;
                    setChargeBtn();
                    visibilityDroneBtns();
                    //ChargeDroneTimeGrid.Visibility = Visibility.Visible;
                }
                catch (BO.Exceptions.ObjNotExistException ex) { PLFunctions.messageBoxResponseFromServer("Charge Drone", $"{ex.Message} can't charge now."); }
                catch (BO.Exceptions.ObjNotAvailableException ex1) { PLFunctions.messageBoxResponseFromServer("Charge Drone", $"{ex1.Message}"); }
                catch (Exception ex2) { PLFunctions.messageBoxResponseFromServer("Charge Drone", $"The Drone can't charge now\n{ex2.Message}\nPlease try later....."); }
            }
            else
            {
                //if (TimeTocharge.Text == "")
                //{
                //    PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nEnter time to charge");
                //}
                //else
                //{ 
                try
                {
                    blObject.FreeDroneFromCharging(currentDrone.Id);
                    //currentDrone.Update(blObject.FreeDroneFromCharging(currentDrone.Id/*, int.Parse(TimeTocharge.Text)*/));
                    
                    //AddDroneDisplay.DataContext = currentDrone;
                    //currentDrone.Status = d.Status;
                    //currentDrone.Battery = d.Battery;
                    //StatusTextBox.Text = $"{dr.Status}";
                    //BatteryTextBox.Text = $"{dr.Battery}";
                    //ChargeDroneTimeGrid.Visibility = Visibility.Hidden;
                    //StatusTextBox.Text = $"{DroneStatus.Available}";
                    //blObject.RemoveDroneCharge(currentDrone.Id);
                    setChargeBtn();
                    DeliveryStatusButton.Visibility = Visibility.Visible;
                    DeliveryStatusButton.Content = deliveryButtonOptionalContent[0];
                    visibilityDroneBtns();
                }
                catch(Exceptions.ObjNotAvailableException e1)
                {
                    PLFunctions.messageBoxResponseFromServer("Sent Drone To Charge", $"{e1.Message}");
                }
                catch (Exception)
                {
                    PLFunctions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
                }
                //}




                #region need to delete
                //////////////
                /// if (TimeTocharge.Text == "")
                //{
                //    PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nEnter time to charge");
                //}
                //else
                //{
                //    try
                //    {
                //        currentDrone.Update(blObject.FreeDroneFromCharging(currentDrone.Id, int.Parse(TimeTocharge.Text)));
                //        //AddDroneDisplay.DataContext = currentDrone;
                //        //currentDrone.Status = d.Status;
                //        //currentDrone.Battery = d.Battery;
                //        //StatusTextBox.Text = $"{dr.Status}";
                //        //BatteryTextBox.Text = $"{dr.Battery}";
                //        setChargeBtn();
                //        ChargeDroneTimeGrid.Visibility = Visibility.Hidden;
                //        //StatusTextBox.Text = $"{DroneStatus.Available}";
                //        DeliveryStatusButton.Visibility = Visibility.Visible;
                //        DeliveryStatusButton.Content = deliveryButtonOptionalContent[0];
                //    }
                //    catch (Exception)
                //    {
                //        PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
                //    }
                //}
                /////
                #endregion
            }
        }

        /// <summary>
        /// The btn:
        /// Content is: 
        /// if "Send To Delivery" :
        /// Check if valid to send a specific drone to charge.
        /// if valid: send to a function that pairs drones with a parcel.
        /// if  "Pick Up Parcel":
        /// Try to send to a function where drone can pick up the parcel.
        /// if "Which Package Delivery" :
        /// Try to send to a function where drone will delivere the package.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDroneOccurdingToStatusBtnClick(object sender, RoutedEventArgs e)
        {
            string contentClickedButton = DeliveryStatusButton.Content.ToString();
            if (contentClickedButton == deliveryButtonOptionalContent[0]) // Send To Delivery = pair with a parcel
            {
                try
                {
                    //currentDrone = new PO.Drone(blObjectD.PairParcelWithDrone(currentDrone.Id));
                    //AddDroneDisplay.DataContext = currentDrone;

                    blObject.PairParcelWithDrone(currentDrone.Id);
                    //currentDrone.Update(blObject.PairParcelWithDrone(currentDrone.Id));
                }
                #region Exceptions
                catch (BO.Exceptions.ObjNotExistException e1) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (BO.Exceptions.ObjNotAvailableException e2) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                //catch (BO.Exceptions.ObjNotAvailableException e3) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e3.Message); }
                catch (Exception e2) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                #endregion
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[1]) // Pick Up Parcel
            {
                try
                {
                    //BatteryTextBox.Text = $"{currentDrone.Battery}";
                    //PositionDroneTextBox.Text = $"({currentDrone.DronePosition.Latitude},{currentDrone.DronePosition.Longitude})";
                    //currentDrone = new PO.Drone(blObjectD.GetDroneById(currentDrone.Id));
                    //currentDrone = new PO.Drone(blObjectD.DronePicksUpParcel(currentDrone.Id));
                    //AddDroneDisplay.DataContext = currentDrone;

                    blObject.DronePicksUpParcel(currentDrone.Id);
                    //currentDrone.Update(blObject.DronePicksUpParcel(currentDrone.Id));

                    findDroneStatusContentBtn();

                }
                #region Exceptions 
                catch (BO.Exceptions.ObjNotExistException e1) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                #endregion
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[2]) // Which Package Delivery - to delivere the package
            {
                try
                {
                    //currentDrone = new PO.Drone(blObjectD.DeliveryParcelByDrone(currentDrone.Id));
                    //AddDroneDisplay.DataContext = currentDrone;
                    //currentDrone.Update(blObject.DeliveryParcelByDrone(currentDrone.Id));
                    blObject.DeliveryParcelByDrone(currentDrone.Id);
                }
                #region Exceptions
                catch (BO.Exceptions.ObjNotExistException e1) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFunctions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                #endregion
            }
            //setDeliveryBtn
            visibilityDroneBtns();
            try
            {
                currentDrone.DronePosition = blObject.convertDalToBLStation(blObject.findAvailbleAndClosestStationForDrone(currentDrone.DronePosition, currentDrone.Battery)).StationPosition;
                //currentDrone.DronePosition = blObject.findAvailbleAndClosestStationForDrone(currentDrone.DronePosition, currentDrone.Battery);
                //status = maintenace
            }
            catch (Exceptions.ObjNotExistException e1)
            {
                PLFunctions.messageBoxResponseFromServer("Send Drone To Charge", $"{e1.Message}");
                //status = maintenace?????????????????????
            }
            //catch(BO.Exceptions.ObjNotExistException )
        }

        #region TextBox OnlyNumbers PreviewKeyDown function
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PLFunctions.TextBox_OnlyNumbers_PreviewKeyDown(sender, e);
        }
        #endregion

        /// <summary>
        /// Try to send the drone to be removed = not active.
        /// Occurding to instuctions the drone will be removed and no sending it to charge & pair it with a parcel.
        /// The current action will continue till it finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDroneBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                blObject.RemoveDrone(currentDrone.BO());
                this.Close();
            }
            catch(BO.Exceptions.ObjNotExistException ee)
            {
                PLFunctions.messageBoxResponseFromServer("Remove Drone", ee.Message);
            }
            //catch(BO.Exceptions.ObjNotAvailableException ee1)
            //{
            //    PLFunctions.messageBoxResponseFromServer("Remove Drone", ee1.Message);
            //}
        }

        /// <summary>
        /// worker to be used bt the simulator of drone
        /// </summary>
        //= new BackgroundWorker();

        private void changeVisibilityOfUpdateBtn(Visibility visibility)
        {
            UpdateButton.Visibility = visibility;
            ChargeButton.Visibility = visibility;
            RemoveDrone.Visibility = visibility;
            DeliveryStatusButton.Visibility = visibility;
        }
    }
}




///// <summary>
///// Return To DroneListWindow.
///// Ensure if the worker wants to exit.
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//private void ReturnToDroneListWindowBtnClick(object sender, RoutedEventArgs e)
//{
//    //MessageBoxResult messageBoxClosing = MessageBox.Show("If you close the next window without saving, your changes will be lost.", "Configuration", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
//    //if (messageBoxClosing == MessageBoxResult.OK)
//    //{
//    ////new DroneListWindow(blObjectD).Show();
//    if (AutomationBtn.Content == "Manual")
//    {
//        //worker.CancelAsync();
//    }
//    this.Close();
//    //}
//}

///// <summary>
///// Try to send the drone with the new name to an update func.
///// If succeed: go to DroneListWindow.
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//private void UpdateButtonClick(object sender, RoutedEventArgs e)
//{
//    try
//    {
//        //currentDrone.Model = ModelTextBox.Text;
//        blObject.ChangeDroneModel(currentDrone.Id, currentDrone.Model);
//        //new DroneListWindow(blObjectD).Show();
//        this.Close();
//    }
//    catch (InvalidOperationException exeptionInvalid) { PLFuncions.messageBoxResponseFromServer("Change Drones' Model", exeptionInvalid.Message); };
//}

///// <summary>
///// Try to send a drone to charge.
///// </summary>
///// <param name="sender"></param>
///// <param name="e"></param>
//private void ChargeButtonClick(object sender, RoutedEventArgs e)
//{
//    if (ChargeButton.Content.ToString() == "Send Drone To Charge")
//    {
//        try
//        {
//            //currentDrone = new PO.Drone(blObjectD.SendDroneToCharge(currentDrone.Id));
//            currentDrone.Update(blObject.SendDroneToCharge(currentDrone.Id));
//            //currentDrone.Status = d.Status;
//            //currentDrone.Battery = d.Battery;
//            //AddDroneDisplay.DataContext = currentDrone;
//            setChargeBtn();
//            removeDroneBtn();
//            //ChargeDroneTimeGrid.Visibility = Visibility.Visible;
//        }
//        catch (BO.Exceptions.ObjNotExistException ex) { PLFuncions.messageBoxResponseFromServer("Charge Drone", $"{ex.Message} can't charge now."); }
//        catch (BO.Exceptions.ObjNotAvailableException) { PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
//        catch (Exception) { PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
//    }
//    else
//    {
//        //if (TimeTocharge.Text == "")
//        //{
//        //    PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nEnter time to charge");
//        //}
//        //else
//        //{ 
//        try
//        {
//            currentDrone.Update(blObject.FreeDroneFromCharging(currentDrone.Id/*, int.Parse(TimeTocharge.Text)*/));
//            //AddDroneDisplay.DataContext = currentDrone;
//            //currentDrone.Status = d.Status;
//            //currentDrone.Battery = d.Battery;
//            //StatusTextBox.Text = $"{dr.Status}";
//            //BatteryTextBox.Text = $"{dr.Battery}";
//            //ChargeDroneTimeGrid.Visibility = Visibility.Hidden;
//            //StatusTextBox.Text = $"{DroneStatus.Available}";
//            blObject.RemoveDroneCharge(currentDrone.Id);
//            setChargeBtn();
//            DeliveryStatusButton.Visibility = Visibility.Visible;
//            DeliveryStatusButton.Content = deliveryButtonOptionalContent[0];
//            removeDroneBtn();
//        }
//        catch (Exception)
//        {
//            PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
//        }
//        //}




//        #region need to delete
//        //////////////
//        /// if (TimeTocharge.Text == "")
//        //{
//        //    PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nEnter time to charge");
//        //}
//        //else
//        //{
//        //    try
//        //    {
//        //        currentDrone.Update(blObject.FreeDroneFromCharging(currentDrone.Id, int.Parse(TimeTocharge.Text)));
//        //        //AddDroneDisplay.DataContext = currentDrone;
//        //        //currentDrone.Status = d.Status;
//        //        //currentDrone.Battery = d.Battery;
//        //        //StatusTextBox.Text = $"{dr.Status}";
//        //        //BatteryTextBox.Text = $"{dr.Battery}";
//        //        setChargeBtn();
//        //        ChargeDroneTimeGrid.Visibility = Visibility.Hidden;
//        //        //StatusTextBox.Text = $"{DroneStatus.Available}";
//        //        DeliveryStatusButton.Visibility = Visibility.Visible;
//        //        DeliveryStatusButton.Content = deliveryButtonOptionalContent[0];
//        //    }
//        //    catch (Exception)
//        //    {
//        //        PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
//        //    }
//        //}
//        /////
//        #endregion
//    }
//}