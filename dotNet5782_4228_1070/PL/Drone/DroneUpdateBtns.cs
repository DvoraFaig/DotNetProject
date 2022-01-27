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
            findDroneStatusContentBtn();
            setChargeBtn();
            removeDroneBtn();
        }

        /// <summary>
        /// Find drone status content btn.
        /// </summary>
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

        private void removeDroneBtn()
        {
            if (currentDrone.Status == DroneStatus.Available)
                RemoveDrone.Visibility = Visibility.Visible;
            else
                RemoveDrone.Visibility = Visibility.Hidden;
        }

        

    }
}