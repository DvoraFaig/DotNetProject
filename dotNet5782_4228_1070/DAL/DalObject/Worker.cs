using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    public partial class DalObject : DalApi.Idal
    {
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate)
        {
            return (from worker in DataSource.Workers
                    where predicate(worker)
                    select worker);
        }

    }
}
