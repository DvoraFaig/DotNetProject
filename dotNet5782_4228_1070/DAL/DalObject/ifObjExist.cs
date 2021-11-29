using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDal.DO;
using IDal;


namespace DalObject
{
    public partial class DalObject : IDal.DO.IDal
    {
        public Boolean IsStationById(int id)
        {
            return DataSource.Stations.Any(s => s.Id == id);
        }

        public Boolean IsDroneById(int id)
        {
            return DataSource.Drones.Any(d => d.Id == id);
        }
        public Boolean IsCustomerById(int id)
        {
            return DataSource.Customers.Any(c => c.ID == id);
        }
        public Boolean IsParcelById(int id)
        {
            return DataSource.Parcels.Any(p => p.Id == id);
        }
    }
}
