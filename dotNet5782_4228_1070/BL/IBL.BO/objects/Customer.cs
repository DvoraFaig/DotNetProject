using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBL
{
    namespace BO
    {
        public class Customer
        {
        
            //public Customer(Customer cu) 
            //{
            //    IDal.DO.Costumer costumer;


            //}

            public Customer(int id, string Name, string Phone, double Longitude, double Latitude)
            {
                this.ID = id;
                this.Name = Name;
                this.Phone = Phone;
                this.Longitude = Longitude;
                this.Latitude = Latitude;
            }
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return ($"customer id: {ID}, customer name: {Name}, customer phone: {Phone}, customer latitude: {Latitude}, customer Longitude: {Longitude}\n");
            }
        }
    }
}



