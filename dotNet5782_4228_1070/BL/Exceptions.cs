using System;
using System.Collections.Generic;
using System.Text;
using IDal.DO;

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
