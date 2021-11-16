using System;
using System.Collections.Generic;
using System.Text;
using IDal;
using IBL.BO;
using System.Linq;
using static IBL.BO.Exceptions;
/*ChargeSlots = s.DroneChargeAvailble ????????????*/



namespace BL
{
    public sealed partial class BL //: IBL.Ibl
    {
        static public IDal.DO.IDal dal;
        private BL()
        {
            dronesInBL = new List<BLDrone>();
            dal = IDal.DalFactory.factory("DalObject"); //start one time an IDal.DO.IDal object.
            Random r = new Random();
            List<IDal.DO.Drone> drones = dal.displayDrone().Cast<IDal.DO.Drone>().ToList();
            List<IDal.DO.Parcel> parcels = dal.displayDrone().Cast<IDal.DO.Parcel>().ToList();
            BLDrone BLd;
            drones.ForEach(d =>
            {
                BLd = convertDalToBLDrone(d);

                parcels.ForEach(p =>
                {
                    if (!p.Scheduled.Equals(null) && p.Delivered.Equals(null))// pair a parcel to drone but not yed delivered.
                    {
                        IDal.DO.Customer c;
                        if (p.PickUp.Equals(null)) //if parcel wasn't picked up
                        {
                            c = dal.getCustomerById(p.SenderId);
                        }
                        else //if parcel was picked up but wasn't delivered
                        {
                            c = dal.getCustomerById(p.TargetId);
                        }
                        BLd.DronePosition = new BLPosition() { Longitude = c.Longitude, Latitude = c.Latitude };
                        BLd.Battery = r.Next(20, 100); // מצב סוללה יוגרל בין טעינה מינימלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לטעינה לתחנה הקרובה ליעד המשלוח לבין טעינה מלאה
                    }
                });
                if ((parcels.Find(p => p.DroneId == d.Id)).Equals(null)) //if the drone is not requested to a parcel - 
                {
                    BLd.Status = (DroneStatus)r.Next(0, 2);
                }
                //if(d.Status == DroneStatus.Maintenance) // doesn't have this by the dal
                //{
                //     //להגריל מספ תחנת בסיס 
                //    d.Battery = r.Next(0,20)
                //}
                //if(d.Status == DroneStatus.Availble) // doesn't have this by the dal
                //{
                //    
                //}
            });
            //add drones to BLDrone arr

            /*
            //יש לבקש משכבת הנתונים ולשמור בשדות נפרדים את צריכת החשמל ע"י
            //הרחפנים ואת קצב טעינתם -בהתאם למה שרשום לעיל

            IDal.DO.Drone[] dronesInDal = (IDal.DO.Drone[])dal.displayDrone();
            foreach (IDal.DO.Drone dr in dronesInDal)
            {
                //BL הוראות בבנאי מופע של
                //כאן יש דברים שקשורים לבטריות ושטויות אחרות. להתייחס בהמשך
                Drone d = new Drone(dr.Id, dr.Model,*//*maxWeight ,Status,*//* dr.Battery);

            }*/
        }
        public double[] requestElectricity()
        {
            return dal.electricityUseByDrone();
        }
    }
}

