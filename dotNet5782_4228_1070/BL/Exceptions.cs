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
            public InvalidStringException(string name , Exception exception)
                : base(String.Format("Invalid string : {0}", name), exception)
            {
            }
        }

        public class ObjNotExistException : Exception
        {
            public ObjNotExistException(Type t, int id , Exception exception)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist."), exception)
            {
            }
            public ObjNotExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist."))
            {
            }
            public ObjNotExistException( string messageFromDal , Exception exception)
                : base(String.Format($"{messageFromDal}"), exception)
            {
            }
            public ObjNotExistException(string messageFromDal)
                : base(String.Format($"{messageFromDal}"))
            {
            }
        }

        public class ObjExistException : Exception
        {
            public ObjExistException(Type objType, int id , Exception exception)
                : base(String.Format($"The {objType.GetType()} with id: {id} exist."), exception)
            {
            }
            public ObjExistException(Type objType, int id)
                : base(String.Format($"The {objType.GetType()} with id: {id} exist."))
            {
            }
            public ObjExistException(string objType, int id , Exception exception)
                : base(String.Format($"The {objType} with id: {id} exist."), exception)
            {
            }
        }

        public class NoDataMatchingBetweenDalandBL<T> : Exception
        {
            public NoDataMatchingBetweenDalandBL(T obj , Exception exception)
                : base(String.Format($"The {obj.GetType()} doesn't exist.\n BL and Dal data are not matching"), exception)
            {
            }
        }

        public class SerialNumbeExistException<T> : Exception
        {
            public SerialNumbeExistException(T obj , Exception exception)
                : base(String.Format($"The {obj.GetType()} serial number exist.\n"), exception)
            {
            }
        }

        public class ObjNotAvailableException : Exception
        {
            public ObjNotAvailableException(string message , Exception exception)
                : base(string.Format($"ERROR: {message}"), exception)
            {
            }
            public ObjNotAvailableException(string message)
                : base(string.Format($"ERROR: {message}"))
            {
            }
        }
    }
}
