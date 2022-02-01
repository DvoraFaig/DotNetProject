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
            public ObjNotExistException(string messageFromDal, object obj)
                : base(String.Format($"{messageFromDal}"))
            {
            }
        }
        public class DataOfOjectChanged : Exception
        {
            public DataOfOjectChanged(Type objType, int id,string message)
                : base(String.Format($"The {objType.Name} with id: {id} : Data Changed\n{message}."))
            {
            }
        }

            public class ObjExistException : Exception
        {
            public ObjExistException(Type objType, int id , Exception exception)
                : base(String.Format($"The {objType.Name} with id: {id} exist."), exception)
            {
            }
            public ObjExistException(Type objType, int id)
                : base(String.Format($"The {objType.Name} with id: {id} exist."))
            {
            }
            public ObjExistException(Type objType, int id , string message )
                : base(String.Format($"The {objType} with id: {id} {message}."))
            {
            }
        }

        public class NoDataMatchingBetweenDalandBL : Exception
        {
            public NoDataMatchingBetweenDalandBL(Type obj , Exception exception)
                : base(String.Format($"The {obj.GetType()} doesn't exist.\n BL and Dal data are not matching"), exception)
            {
            }
            public NoDataMatchingBetweenDalandBL(string message)
                : base(String.Format($"{message}"))
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
            public ObjNotAvailableException(Type objType, int id , string message)
                : base(string.Format($"ERROR: The {objType} with id: {id} \n{message}"))
            {
            }
            
        }
    }
}
