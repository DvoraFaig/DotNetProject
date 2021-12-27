using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;

namespace BL
{
    public sealed partial class BL : BlApi.Ibl
    {

        public bool ifWorkerExist(Worker worker)
        {
            try
            {
                DO.Worker worker1 = dal.getWorkerWithSpecificCondition(w => w.Id == worker.Id && w.Password == worker.Password).First();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new ObjNotExistException(typeof(Worker), worker.Id);
            }
        }
    }
}
