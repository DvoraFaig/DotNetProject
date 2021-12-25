using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public partial class Exceptions
    {
        public class InvalidStringException : Exception
        {
            public InvalidStringException(string name)
                : base(String.Format("Invalid string : {0}", name))
            {
            }
        }
        public class ObjNotExistException : Exception
        {
            public ObjNotExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist."))
            {
            }
            public ObjNotExistException( string messageFromDal)
                : base(String.Format($"{messageFromDal}"))
            {
            }
        }

        public class ObjExistException : Exception
        {
            public ObjExistException(Type objType, int id)
                : base(String.Format($"The {objType.GetType()} with id: {id} exist."))
            {
            }
            public ObjExistException(string objType, int id)
                : base(String.Format($"The {objType} with id: {id} exist."))
            {
            }
        }

        public class NoDataMatchingBetweenDalandBL<T> : Exception
        {
            public NoDataMatchingBetweenDalandBL(T obj)
                : base(String.Format($"The {obj.GetType()} doesn't exist.\n BL and Dal data are not matching"))
            {
            }
        }

        public class SerialNumbeExistException<T> : Exception
        {
            public SerialNumbeExistException(T obj)
                : base(String.Format($"The {obj.GetType()} serial number exist.\n"))
            {
            }
        }

        public class ObjNotAvailableException : Exception
        {
            public ObjNotAvailableException(string message)
                : base(string.Format($"ERROR: {message}"))
            {
            }
        }
    }
}
