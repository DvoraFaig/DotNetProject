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
        /// <summary>
        /// set visibility and content buttons.
        /// </summary>
        private void setVisibilityAndContentBtn()
        {
            if(currentDrone.Status == DroneStatus.Delivery)
                findDroneStatusContentBtn();
            if(currentDrone.Status != DroneStatus.Maintenance)
                setChargeBtn();
            visibilityDroneBtns();
        }

        /// <summary>
        /// Find drone status content btn.
        /// </summary>
        private void findDroneStatusContentBtn()
        {
            try
            {
                int contentIndex = blObject.GetDroneStatusInDelivery(currentDrone.BO());
                DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
                DeliveryStatusButton.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                DeliveryStatusButton.Visibility = Visibility.Hidden;
            }
        }

        private void setDeliveryBtn()
        {
            if (currentDrone.Status == DroneStatus.Maintenance || currentDrone.Status == DroneStatus.Available)
                //|| (currentDrone.Status == DroneStatus.Delivery && currentDrone.ParcelInTransfer == null )) //parcel is delivered
                return;
            int contentIndex = blObject.GetDroneStatusInDelivery(currentDrone.BO());
            //if(contentIndex >= deliveryButtonOptionalContent.Count())
            //    ChargeButton.Visibility = Visibility.Visible;
            //if (contentIndex == 3)
            //    return;

            DeliveryStatusButton.Content = deliveryButtonOptionalContent[contentIndex];
            DeliveryStatusButton.Visibility = Visibility.Visible;
            if (contentIndex != 0)
                ChargeButton.Visibility = Visibility.Hidden;
            if (contentIndex == 0)
                ChargeButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Content of a btn in the update form occurding to the drones' status.
        /// </summary>
        private void setChargeBtn()
        {
            ChargeButton.Visibility = Visibility.Hidden;
            switch (currentDrone.Status)
            {
                case DroneStatus.Available:
                    ChargeButton.Content = "Send Drone To Charge";
                    ChargeButton.Visibility = Visibility.Visible;
                    break;
                case DroneStatus.Maintenance:
                    ChargeButton.Content = "Free Drone From Charge";
                    ChargeButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// Manege Drone update form Visibility of btns.
        /// </summary>
        private void visibilityDroneBtns()
        {
            if (currentDrone.Status == DroneStatus.Available)
                RemoveDrone.Visibility = Visibility.Visible;
            else
                RemoveDrone.Visibility = Visibility.Hidden;
        }

        

    }
}
