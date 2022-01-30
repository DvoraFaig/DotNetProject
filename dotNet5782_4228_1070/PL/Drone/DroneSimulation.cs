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
        public enum droneStatusInDelivery { ToPickUp = 1, PickUp, ToDelivery, Delivery, ToCharge, NoAvailbleChargingSlots, NotEnoughBatteryForDelivery, DisFromDestination, HideTextBlock };

        /// <summary>
        /// If Simulator is asked to stop but operation is not Completed yet = true/false.
        /// For the progress bar.
        /// </summary>
        bool simIsAskedToStopButOperationNotCompleted = false;

        /// <summary>
        /// Is ProgressBar from click Return btn = true;
        /// 
        /// </summary>
        bool isProgressBarFromReturnBtn = false;


        public int droneCase { get; set; }
        public double droneDisFromDes { get; set; }



        private void checkIfSimIsWorking()
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
                simIsAskedToStopButOperationNotCompleted = false;
            }
        }

        /// <summary>
        /// Initialize obj worker for the simolator of drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeWorker(object sender, RoutedEventArgs e)
        {

            //worker = new BackgroundWorker();

            if (simIsAskedToStopButOperationNotCompleted)
            {
                checkIfSimIsWorking();
                return;///////////////////////////??????????????????
            }

            if (AutomationBtn.Content == "Manual")
            {
                simIsAskedToStopButOperationNotCompleted = true;
                worker.CancelAsync();
                ProgressBarForSimulation.Visibility = Visibility.Visible;
                return;
            }

            if (AutomationBtn.Content == "Start Automation")
            {
                worker = new BackgroundWorker();
            }

            droneCase = -1;
            droneDisFromDes = 0;

            AutomationBtn.Content = "Manual";
            changeVisibilityOfUpdateBtn(Visibility.Hidden);
            isSimulationWorking = true;
            //worker = new BackgroundWorker();
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
                tempDrone,//currentDrone.BO(),
                (tempDrone, i, des) =>
                {
                    worker.ReportProgress(i);
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
            if (isReturnBtnClick)
                this.Close();

            AutomationBtn.Content = "Start Automation";
            ProgressBarForSimulation.Visibility = Visibility.Hidden;
            setChargeBtn();
            visibilityDroneBtns();
            setDeliveryBtn();
            simIsAskedToStopButOperationNotCompleted = false;

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
            switch ((droneStatusInDelivery)droneCase)
            {
                case droneStatusInDelivery.ToPickUp:
                    StatusTextBoxLabelSimulation.Content = "Destination\nSender Customer";//"Drone on the way to pick up the parcel";
                    break;
                case droneStatusInDelivery.PickUp:
                    StatusTextBoxLabelSimulation.Content = "Picking up parcel";
                    break;
                case droneStatusInDelivery.ToDelivery:
                    StatusTextBoxLabelSimulation.Content = "Destination\nReceiving Customer";//"Drone on the way to deliver the parcel";
                    break;
                case droneStatusInDelivery.Delivery:
                    StatusTextBoxLabelSimulation.Content = "Delivering parcel";
                    break;
                case droneStatusInDelivery.ToCharge:
                    StatusTextBoxLabelSimulation.Content = "Destination\nStation";
                    break;
                case droneStatusInDelivery.NoAvailbleChargingSlots:
                    StatusTextBoxLabelSimulation.Content = "No charging slots";
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case droneStatusInDelivery.NotEnoughBatteryForDelivery:
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case droneStatusInDelivery.DisFromDestination:
                    DisDroneFromDes.Visibility = Visibility.Visible;
                    if (droneDisFromDes >= 0)
                        DisDroneFromDes.Content = $"Distance from\ndestination: {Math.Round(droneDisFromDes, 1)}";
                    else
                        DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                case droneStatusInDelivery.HideTextBlock:
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    DisDroneFromDes.Visibility = Visibility.Hidden;
                    break;
                default:
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                    DisDroneFromDes.Visibility = Visibility.Visible;
                    break;
                    //}
            }
        }
    }
}
