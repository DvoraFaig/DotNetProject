using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDal.DO
{
    public class DalExceptions:Exception
    {
        public class ObjNotExistException<T> : Exception
        {
            public ObjNotExistException(T obj)
                : base(String.Format($"The {obj.GetType()} doesn't exist."))
            {
            }
            public ObjNotExistException(string message , int id)
                : base(String.Format($"The {message}with id: {id} doesn't exist."))
            {
            }

        }

    }
}
