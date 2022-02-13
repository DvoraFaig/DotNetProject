using System;
using System.ComponentModel;
using System.Windows;
using BO;
using DO;

namespace PL
{
    public partial class DroneWindow : Window
    {
        /// <summary>
        /// BackgroundWorker for simulation
        /// </summary>
        BackgroundWorker worker = new BackgroundWorker();
        //Private WithEvents Worker As BackgroundWorker;


        /// <summary>
        /// Drone status in delivery: for changing text occurding to status
        /// </summary>
        //public enum droneStatusInDelivery { ToPickUp = 1, PickUp, ToDelivery, Delivery, ToCharge, NoAvailbleChargingSlots, NotEnoughBatteryForDelivery, DisFromDestination, HideTextBlock };

        /// <summary>
        /// If Simulator is asked to stop but operation is not Completed yet = true/false.
        /// For the progress bar.
        /// </summary>
        bool simIsAskedToStop = false;

        /// <summary>
        /// Is ProgressBar from click Return btn = true;
        /// </summary>
        bool isProgressBarFromReturnBtn = false;

        /// <summary>
        /// Enum of drone status in simulation.
        /// </summary>
        public DroneStatusInSim droneCase { get; set; }

        /// <summary>
        /// Saves the distence of drone from it's destination when drone.Status == DroneStatus.Delivery;
        /// </summary>
        public double droneDisFromDes { get; set; }


        /// <summary>
        /// Checks if simulation is working.
        /// </summary>
        private void checkIfEnableToCloseSim()
        {
            if (currentDrone.Battery == 100)
            {
                AutomationBtn.Content = "Start Automation";
                ProgressBarForSimulation.Visibility = Visibility.Hidden;
                visibilityDroneBtns();
                int contentIndex = blObject.GetDroneStatusInDelivery(currentDrone.BO());
                if (contentIndex != 3)
                    setDeliveryBtn();
                else
                    setChargeBtn();
                simIsAskedToStop = false;
            }
        }

        /// <summary>
        /// Initialize obj worker for the simolator of drone
        /// And when manual btn is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeWorker(object sender, RoutedEventArgs e)
        {
            if (isSimulationWorking && simIsAskedToStop) //first time?
                return;

            if (isSimulationWorking)//AutomationBtn.Content == "Manual")
            {
                simIsAskedToStop = true;
                worker.CancelAsync();
                ProgressBarForSimulation.Visibility = Visibility.Visible;
                return;
            }

            if (!isSimulationWorking)//AutomationBtn.Content == "Start Automation")
                worker = new BackgroundWorker();

            droneCase = 0;
            droneDisFromDes = 0;
            AutomationBtn.Content = "Manual";
            changeVisibilityOfUpdateBtn(Visibility.Hidden);
            isSimulationWorking = true;
            worker.DoWork += new DoWorkEventHandler(DoWork); //worker.DoWork += (DoWork);
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted); //worker.RunWorkerCompleted += (RunWorkerCompleted);
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(worker);
            worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
        }

        /// <summary>
        /// DoWork of BackgroundWorker worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DoWork(object? sender, DoWorkEventArgs e)
        {
            blObject.StartSimulation(
                tempDrone,  //currentDrone.BO(),
                (tempDrone, i, des) =>
                {
                    worker.ReportProgress((int)i);
                    droneCase = i;
                    droneDisFromDes = des;
                },
                () => worker.CancellationPending);
        }

        /// <summary>
        /// RunWorkerCompleted of BackgroundWorker worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            worker.CancelAsync();

            if (isReturnBtnClick)
                this.Close();

            AutomationBtn.Content = "Start Automation";
            ProgressBarForSimulation.Visibility = Visibility.Hidden;
            setChargeBtn();
            visibilityDroneBtns();
            setDeliveryBtn();
            simIsAskedToStop = false;
        }

        /// <summary>
        /// ProgressChanged of BackgroundWorker worker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            currentDrone.Update(tempDrone);
            //if (droneCase != -1 && droneCase != 0)
            //{ //if droneCase == -1 it already used the switch and their in no point using it and wasting time; 0 = not in delivery status
            StatusTextBoxLabelSimulation.Visibility = Visibility.Visible;
            DisDroneFromDes.Visibility = Visibility.Hidden;
            switch (droneCase)
            {
                case DroneStatusInSim.ToPickUp:
                    deliveryVisibility(Visibility.Visible);
                    StatusTextBoxLabelSimulation.Content = "Destination\nSender Customer";//"Drone on the way to pick up the parcel";
                    break;
                case DroneStatusInSim.PickUp:
                    deliveryVisibility(Visibility.Visible);
                    StatusTextBoxLabelSimulation.Content = "Picking up parcel";
                    break;
                case DroneStatusInSim.ToDelivery:
                    deliveryVisibility(Visibility.Visible);
                    StatusTextBoxLabelSimulation.Content = "Destination\nReceiving Customer";//"Drone on the way to deliver the parcel";
                    break;
                case DroneStatusInSim.Delivery:
                    deliveryVisibility(Visibility.Hidden);
                    StatusTextBoxLabelSimulation.Content = "Delivering parcel";
                    break;
                case DroneStatusInSim.ToCharge:
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    StatusTextBoxLabelSimulation.Content = "Destination\nStation";
                    break;
                case DroneStatusInSim.NoAvailbleChargingSlots:
                    StatusTextBoxLabelSimulation.Content = "No charging slots";
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case DroneStatusInSim.NotEnoughBatteryForDelivery:
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case DroneStatusInSim.DisFromDestination:
                    DisDroneFromDes.Visibility = Visibility.Visible;
                    if (droneDisFromDes >= 0)
                        DisDroneFromDes.Content = $"Distance from\ndestination: {Math.Round(droneDisFromDes, 1)}";
                    else
                        DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case DroneStatusInSim.HideTextBlock:
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case DroneStatusInSim.completeSim:
                    //RunWorkerCompleted(sender, null);
                    //worker.CancelAsync();
                    break;
                default:
                    //StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    //DisDroneFromDes.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void deliveryVisibility(Visibility visibility)
        {
            ParcelIdIdTextBox.Text = visibility == Visibility.Visible ? $"{currentDrone.ParcelInTransfer.Id}" : "";
            ParcelIdIdTextBox.Visibility = visibility;
            ParcelTextBoxLabel.Visibility = visibility;
        }
    }
}
