using System;
using System.Collections.Generic;
using System.Text;
using IDal.DO;
using IBL.IBL;

namespace IBL
{
    namespace BO
    {
        public static class Exceptions
        {   
            public class InvalidStringException : Exception
            {
                public InvalidStringException(string name)
                    : base(String.Format("Invalid string : {0}", name))
                {
                }
            }
            public class ObjNotExistException<T> : Exception
            {
                public ObjNotExistException( T obj )
                    : base(String.Format($"The {obj.GetType()} doesn't exist."))
                {
                }
            }
            public class ObjNotExistException<T , S> : Exception
            {
                public ObjNotExistException(T obj , S message)
                    : base(String.Format($"The {message.ToString()} {obj.GetType()} doesn't exist."))
                {
                }
            }
            public class ObjNotExistException : Exception
            {
                public ObjNotExistException( string message , int id)
                    : base(String.Format($"The {message} with id: {id} doesn't exist."))
                {
                }
            }
            public class ObjExistException<T> : Exception
            {
                public ObjExistException(T obj)
                    : base(String.Format($"The {obj.GetType()} exist."))
                {
                }
            }
            public class ObjExistException : Exception
            {
                public ObjExistException(string objType , int id)
                    : base(String.Format($"The {objType} exist."))
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

        }
    }
}
