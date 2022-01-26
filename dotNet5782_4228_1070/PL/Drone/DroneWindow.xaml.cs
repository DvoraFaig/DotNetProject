using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;


namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private BlApi.Ibl blObject;
        //BO.Drone dr;
        PO.Drone currentDrone;
        string[] deliveryButtonOptionalContent = { "Send To Delivery", "Pick Up Parcel", "Deliver by Target" };
        BO.Drone tempDrone;

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
        public DroneWindow(BlApi.Ibl blObject)
        {
            InitializeComponent();
            Loaded += ToolWindowLoaded;//The x button
            this.blObject = blObject;
            DroneWeightSelector.ItemsSource = Enum.GetValues(typeof(DO.WeightCategories));
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
        public DroneWindow(BlApi.Ibl blObject, BO.Drone drone)
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
                    int contentIndex = this.blObject.GetDroneStatusInDelivery(currentDrone.Id);
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
            removeDroneBtn();
            AutomationBtn.Content = "Start Automation";
            tempDrone = currentDrone.BO();/////////////////////////////////////////////////////////////////

        }

        private void findDroneStatusContentBtn()
        {
            try
            {
                int contentIndex = blObject.GetDroneStatusInDelivery(currentDrone.Id);
                DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
                DeliveryStatusButton.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                DeliveryStatusButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Content of a btn in the update form occurding to the drones' status.
        /// </summary>
        private void setChargeBtn()
        {
            switch (currentDrone.Status)
            {
                case DroneStatus.Available:
                    ChargeButton.Content = "Send Drone To Charge";
                    break;
                case DroneStatus.Maintenance:
                    ChargeButton.Content = "Free Drone From Charge";
                    break;
            }
        }

        private void removeDroneBtn()
        {
            if (currentDrone.Status == DroneStatus.Available )
                RemoveDrone.Visibility = Visibility.Visible;
            else
                RemoveDrone.Visibility = Visibility.Hidden;
        }

        private void setDeliveryBtn()
        {
            int contentIndex = blObject.GetDroneStatusInDelivery(currentDrone.Id);
            DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
            DeliveryStatusButton.Visibility = Visibility.Visible;
            if (contentIndex != 0)
                ChargeButton.Visibility = Visibility.Hidden;
            if (contentIndex == 0)
                ChargeButton.Visibility = Visibility.Visible;
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
            int weightCategory = Convert.ToInt32((DO.WeightCategories)DroneWeightSelector.SelectedIndex + 1);
            try
            {
                // didn't sent an object Drone becuase most of the props values are filled in BL automatic.

                //blObjectD.AddDrone(int.Parse(IdTextBox.Text), ModelTextBox.Text, DroneWeightSelector.SelectedIndex + 1, Convert.ToInt32(StationIdTextBox.Text));
                blObject.AddDrone(new Drone() { Id = int.Parse(IdTextBox.Text), Model = ModelTextBox.Text, MaxWeight = (DO.WeightCategories)(DroneWeightSelector.SelectedIndex + 1) }, Convert.ToInt32(StationIdTextBox.Text));
                //blObjectD.AddDrone(currentDrone.BO(),Convert.ToInt32(StationIdTextBox.Text));
                ////////new DroneListWindow(blObjectD).Show();
                this.Close();
            }
            #region catch exeptions
            catch (BO.Exceptions.ObjExistException e1)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", e1.Message);
            }
            catch (ArgumentNullException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (FormatException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (OverflowException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (NullReferenceException)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "== ERROR receiving data ==\nPlease try again");
            }
            catch (Exception)
            {
                PLFuncions.messageBoxResponseFromServer("Add Drone", "Cann't add a drone");
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
            PLFuncions.clearFormTextBox(IdTextBox, ModelTextBox, StationIdTextBox);
            DroneWeightSelector.SelectedItem = Enum.GetValues(typeof(DO.WeightCategories));
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
            if (AutomationBtn.Content == "Manual")
            {
                worker.CancelAsync();
            }
            this.Close();
            //}
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
            catch (InvalidOperationException exeptionInvalid) { PLFuncions.messageBoxResponseFromServer("Change Drones' Model", exeptionInvalid.Message); };
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
                    removeDroneBtn();
                    //ChargeDroneTimeGrid.Visibility = Visibility.Visible;
                }
                catch (BO.Exceptions.ObjNotExistException ex) { PLFuncions.messageBoxResponseFromServer("Charge Drone", $"{ex.Message} can't charge now."); }
                catch (BO.Exceptions.ObjNotAvailableException) { PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
                catch (Exception) { PLFuncions.messageBoxResponseFromServer("Charge Drone", "The Drone can't charge now\nPlease try later....."); }
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
                    blObject.RemoveDroneCharge(currentDrone.Id);
                    setChargeBtn();
                    DeliveryStatusButton.Visibility = Visibility.Visible;
                    DeliveryStatusButton.Content = deliveryButtonOptionalContent[0];
                    removeDroneBtn();
                }
                catch (Exception)
                {
                    PLFuncions.messageBoxResponseFromServer("Sent Drone To Charge", "ERROR\nCan't charge the drone\nPlease try later....");
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
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (BO.Exceptions.ObjNotAvailableException e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                //catch (BO.Exceptions.ObjNotAvailableException e3) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e3.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
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
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                #endregion
            }
            else if (contentClickedButton == deliveryButtonOptionalContent[2]) // Which Package Delivery - to delivere the package
            {
                try
                {
                    //currentDrone = new PO.Drone(blObjectD.DeliveryParcelByDrone(currentDrone.Id));
                    //AddDroneDisplay.DataContext = currentDrone;
                    currentDrone.Update(blObject.DeliveryParcelByDrone(currentDrone.Id));
                }
                #region Exceptions
                catch (BO.Exceptions.ObjNotExistException e1) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e1.Message); }
                catch (Exception e2) { PLFuncions.messageBoxResponseFromServer("Pair a Prcel With a Drone", e2.Message); }
                #endregion
            }
            //setDeliveryBtn
            removeDroneBtn();
        }

        #region TextBox OnlyNumbers PreviewKeyDown function
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            PLFuncions.TextBox_OnlyNumbers_PreviewKeyDown(sender, e);
        }
        #endregion

        /// <summary>
        /// Try to send the drone to be removed = not active.
        /// Occurding to instuctions the drone will be removed and no sending it to charge & pair it with a parcel.
        /// The current action will continue till it finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                blObject.RemoveDrone(currentDrone.BO());
                new DroneListWindow(blObject).Show();
                this.Close();
            }
            catch (BO.Exceptions.ObjExistException e1)
            {
                PLFuncions.messageBoxResponseFromServer("Remove Drone", e1.Message);
            }
        }





        /// <summary>
        /// worker to be used bt the simulator of drone
        /// </summary>
        BackgroundWorker worker = new BackgroundWorker();

        private void changeVisibilityOfUpdateBtn(Visibility visibility)
        {
            UpdateButton.Visibility = visibility;
            ChargeButton.Visibility = visibility;
            RemoveDrone.Visibility = visibility;
        }
        /// <summary>
        /// Initialize obj worker for the simolator of drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeWorker(object sender, RoutedEventArgs e)
        {
            if(AutomationBtn.Content == "Manual")
            {
                AutomationBtn.Content = "Start Automation";
                worker.CancelAsync();
                changeVisibilityOfUpdateBtn(Visibility.Visible);
                return;
            }

            AutomationBtn.Content = "Manual";
            changeVisibilityOfUpdateBtn(Visibility.Hidden);

            worker.DoWork += (object? sender, DoWorkEventArgs e) =>
            {
                blObject.StartSimulation(
                    tempDrone,//currentDrone.BO(),
                    (tempDrone, i) => {worker.ReportProgress(i);  },
                    () => worker.CancellationPending);

            };
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += (object? sender, ProgressChangedEventArgs e) =>
            {
                currentDrone.Update(tempDrone);
                //currentDrone.Battery ++;
                //Student.MyAge++;
                //Student.Name = updateDrone.FirstName;
                //progress.Content = e.ProgressPercentage;
            };

            worker.RunWorkerCompleted += (object? sender, RunWorkerCompletedEventArgs e) =>
            {
                AutomationBtn.Content = "Start Automation";
                changeVisibilityOfUpdateBtn(Visibility.Visible);
                worker.CancelAsync();                
            };
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();

        }
    }
}
