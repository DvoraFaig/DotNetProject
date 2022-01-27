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
    public partial class DroneWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();
        public enum droneStatusInDelivery { ToPickUp = 1, PickUp, ToDelivery, Delivery, ToCharge, NoAvailbleChargingSlots, HideTextBlock };


        /// <summary>
        /// Initialize obj worker for the simolator of drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitializeWorker(object sender, RoutedEventArgs e)
        {

            if (AutomationBtn.Content == "Manual")
            {
                AutomationBtn.Content = "Start Automation";
                worker.CancelAsync();
                //changeVisibilityOfUpdateBtn(Visibility.Visible);
                setVisibilityAndContentBtn();
                return;
            }

            int i = 10;
            int droneCase = -1;
            AutomationBtn.Content = "Manual";
            changeVisibilityOfUpdateBtn(Visibility.Hidden);

            worker.DoWork += (object? sender, DoWorkEventArgs e) =>
            {
                blObject.StartSimulation(
                    tempDrone,//currentDrone.BO(),
                    (tempDrone, i) => { worker.ReportProgress(i); droneCase = i; },
                    () => worker.CancellationPending);

            };
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += (object? sender, ProgressChangedEventArgs e) =>
            {
                currentDrone.Update(tempDrone);
                if (droneCase != -1 && droneCase != 0)
                { //if droneCase == -1 it already used the switch and their in no point using it and wasting time; 0 = not in delivery status
                    StatusTextBoxLabelSimulation.Visibility = Visibility.Visible;
                    switch ((droneStatusInDelivery)droneCase)
                    {
                        case droneStatusInDelivery.ToPickUp:
                            StatusTextBoxLabelSimulation.Content = "Destination\nSender Customer";//"Drone on the way to pick up the parcel";
                            break;
                        case droneStatusInDelivery.PickUp:
                            StatusTextBoxLabelSimulation.Content = "Picking up the parcel";
                            break;
                        case droneStatusInDelivery.ToDelivery:
                            StatusTextBoxLabelSimulation.Content = "Destination\nReceiving Customer";//"Drone on the way to deliver the parcel";
                            break;
                        case droneStatusInDelivery.Delivery:
                            StatusTextBoxLabelSimulation.Content = "Delivering the parcel";
                            break;
                        case droneStatusInDelivery.ToCharge:
                            StatusTextBoxLabelSimulation.Content = "Destination\nStation";
                            break;
                        case droneStatusInDelivery.NoAvailbleChargingSlots:
                            StatusTextBoxLabelSimulation.Content = "No charging slots";
                            break;
                        default:
                            StatusTextBoxLabelSimulation.Visibility = Visibility.Hidden;
                            break;
                    }
                }
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
