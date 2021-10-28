using System;
using System.Collections.Generic;
using System.Text;
using IBL;
using IBL.BO;
using DalObject;

namespace BL
{
    public partial class BL //: IBL.Ibl
    {
        DalObject.DalObject dalObject;
        public BL()
        {
            dalObject = new DalObject.DalObject();
        }
        n 
        public void addStation()
        {
            Random r = new Random();
            int amountS = DalObject.DalObject.amountStations();
            if (amountS >= 5)
            {
                Console.WriteLine("== Cann't add stations ==");
                return;
            }
            Console.WriteLine("Enter a station Name: ");
            string Name = Console.ReadLine();
            int ChargeSlots = r.Next(0, 5);
            Console.WriteLine("Enter a Latitude");
            int Latitude = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a Longitude");
            int Longitude = Convert.ToInt32(Console.ReadLine());
            dalObject.AddStation(amountS, Name, ChargeSlots, Longitude, Latitude);
        }
    }
}
