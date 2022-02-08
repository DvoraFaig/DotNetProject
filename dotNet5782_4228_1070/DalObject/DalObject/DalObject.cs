using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Runtime.CompilerServices;


namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {

        /// <summary>
        /// instance of DalObject and will be equal to DalApi
        /// </summary>
        private static DalObject Instance;
        //private static DalObject Instance = null;

        /// <summary>
        /// Avoid reaching DalXml instance by the same time a few places.
        /// DalXml is supposed to be a Singelton
        /// </summary>
        private static readonly object padlock = new object();

        /// <summary>
        /// Ctor - calls Initialize  = Initialize info of the program
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// return one and only one instance of DalObject 
        /// </summary>
        public static DalObject GetInstance
        {
            get
            {
                lock (padlock)
                {
                    if (Instance == null)
                        Instance = new DalObject();
                    return Instance;
                }
            }
        }
    }
}
