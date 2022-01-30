﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using DO;
using System.Runtime.CompilerServices;



namespace Dal
{
    public sealed partial class DalXml : DalApi.Idal
    {

        /// <summary>
        /// Get a Worker/s with a specific condition = predicate
        /// </summary>
        /// <param name="predicate">return a worker/s that meeets the condition</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Worker> getWorkerWithSpecificCondition(Predicate<Worker> predicate)
        {
            IEnumerable<DO.Worker> workerList = XMLTools.LoadListFromXMLSerializer<DO.Worker>(dir + workerFilePath);
            return (from worker in workerList
                    where predicate(worker)
                    select worker);
        }
    }
}
