using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public partial class DalExceptions:Exception
    {
        public class ObjNotExistException : Exception
        {
            public ObjNotExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist."))
            {
            }
            public ObjNotExistException(string message )
                : base(String.Format($"The {message}"))
            {
            }

        }

    }
}
