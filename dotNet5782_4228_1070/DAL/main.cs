using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DAL
{
    enum choices { Add = 1, Update, ShowWithId, ShowList }
    enum objects { Station = 1, Drone, CLient, Parcel }
    Random r = new Random();

    class main
    {


        public void additionFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            int choice = Console.WriteLine();
            switch (choice)
            {
                case objects.Station:
                    //int amountStations = r.Next(2, 5);
                    //Console.WriteLine($"Enter {amountStations} stations information");
                    //Station StationsTemp = new Station;
                    //for (int i = 0; i < amountStations; i++)
                    //{
                    //    StationsTemp.Id = r.Next(0, 10);
                    //    Console.WriteLine("Enter a Latitude");
                    //    StationsTemp.Latitude = Convert.ToInt32(Console.ReadLine());
                    //    Console.WriteLine("Enter a Longitude");
                    //    StationsTemp.Longitude = Convert.ToInt32(Console.ReadLine());
                    //    StationsTemp.ChargeStop = r.Next(0, 200);
                    //    Inilize(i, choice, StationsTemp);
                    //}
                    break;
                case objects.Drone:
                    break;
                case objects.CLient:
                    break;
                case objects.Parcel:
                    break;
                default:
                    break;
            }
        }
        public void UpdateFunc() { }
        public void ShowWithIdFunc()
        {
            Console.WriteLine("Enter your choice to add:\n 1.Station \n2.Drone\n 3.CLient\n 4.Parcel ");
            int choice = Console.WriteLine();
            if (choice > 0 && choice < 5)
            {
                Console.WriteLine("Enter the Id of the object");
                int id = Console.ReadLine();
            }
            switch (choice)
            {
                case objects.Station:
                    break;
                case objects.Drone:
                    break;
                case objects.CLient:
                    break;
                case objects.Parcel:
                    break;
                default:
                    Console.WriteLine("eroor");
                    break;
            }
        }
        public void ShowListFunc() { }

        public void nav()
        {
            Console.WriteLine("Enter your choice");
            int choice = Console.ReadLine();
            do
            {
                Console.WriteLine("Enter your choice to add:\n 1.Add \n2.Update\n 3.Show object occurding to an Id\n 4.Show list of an object ");
                int choice = Console.WriteLine();
                switch (choices)
                {
                    case choices.Add:
                        additionFunc();
                        break;
                    case choices.Update:
                        UpdateFunc();
                        break;
                    case choices.ShowWithId:
                        ShowWithIdFunc();
                        break;
                    case choices.ShowList:
                        ShowListFunc();
                        break;
                    default:
                        if (choice != 5)
                            Console.WriteLine("Error");
                        break;
                }
            } while (choice != 5);
        }
    }
}



