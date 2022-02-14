using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using static BO.Exceptions;
using System.Runtime.CompilerServices;

namespace BL
{
    sealed partial class BL
    {
        /// <summary>
        /// For worker to sign in check if he exist.
        /// </summary>
        /// <param name="worker">The worker who loged in</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool IfWorkerExist(Worker worker)
        {
            try
            {
                lock (dal)
                {
                    DO.Worker worker1 = dal.getWorkerWithSpecificCondition(w => w.Id == worker.Id && w.Password == worker.Password).First();
                    return true;
                }
            }
            catch (InvalidOperationException e)
            {
                throw new ObjNotExistException(typeof(Worker), worker.Id , e);
            }
        }
    }
}
