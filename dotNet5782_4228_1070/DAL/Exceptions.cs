using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public partial class Exceptions:Exception
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
        public class ObjExistException : Exception
        {
            public ObjExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} exist."))
            {
            }
            public ObjExistException(string message)
                : base(String.Format($"The {message}"))
            {
            }

        }
        public class DalConfigException : Exception
        {
            public DalConfigException(string s)
                : base(String.Format($"{s}"))
            {
            }
        }

    }
}
