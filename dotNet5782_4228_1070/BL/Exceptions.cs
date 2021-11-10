using System;
using System.Collections.Generic;
using System.Text;
using IDal.DO;

namespace IBL
{
    namespace BO
    {
        class Exceptions
        {
            public class InvalidStringException : Exception
            {
                //public InvalidStringException() { }
                public InvalidStringException(string name)
                    : base(String.Format("Invalid string : {0}", name))
                {
                }
            }
            public class ObjNotExistException<T> : Exception
            {
                //public ObjNotExistException(){ }
                public ObjNotExistException( T obj )
                    : base(String.Format($"The {obj.GetType()} doesn't exist."))
                {
                }
            }
            public class NoDataMatchingBetweenDalandBL<T> : Exception
            {
                //public NoDataMatchingBetweenDalandBL() { }
                public NoDataMatchingBetweenDalandBL(T obj)
                    : base(String.Format($"The {obj.GetType()} doesn't exist.\n BL and Dal data are not matching"))
                {
                }
            }

        }
    }
}
