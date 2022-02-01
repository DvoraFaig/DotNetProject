using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static BO.Exceptions;
using System.Threading.Tasks;
using BO;

namespace BL
{
    sealed partial class BL : BlApi.Ibl
    {
        static BL Instance;
        public static BL GetInstance
        {
            get
            {
                if (Instance == null)
                    Instance = new BL();
                return Instance;
            }
        }

    }
}
