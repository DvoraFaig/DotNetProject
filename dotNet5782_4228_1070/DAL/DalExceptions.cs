using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDal.DO
{
    public partial class DalExceptions:Exception
    {
        public class ObjNotExistException<T> : Exception
        {
            public ObjNotExistException(Type t, int id)
                : base(String.Format($"The {t.GetType()} with id {id} exist."))
            {
            }
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
