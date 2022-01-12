using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;



namespace Dal
{
    public partial class DalObject : DalApi.Idal
    {

        /// <summary>
        /// instance of DalObject and will be equal to DalApi
        /// </summary>
        static DalObject Instance;
        //private static DalObject Instance = null;

        //    private static readonly object padlock = new object();

        /// <summary>
        /// Ctor - calls Initialize  = Initialize info of the program
        /// </summary>
        public DalObject()
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
                //lock (padlock)
                //{
                    if (Instance == null)
                        Instance = new DalObject();
                    return Instance;
                //}
            }
        }
        

    }
}
