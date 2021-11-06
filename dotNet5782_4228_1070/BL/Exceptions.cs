using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace BO
    {
        class Exceptions
        {
            class InvalidStringException : Exception
            {
                public InvalidStringException() { }
                public InvalidStringException(string name)
                    : base(String.Format("Invalid string : {0}", name))
                {
                }
            }
            class ObjNotExistException<T> : Exception
            {
                public ObjNotExistException(){ }
                public ObjNotExistException( T obj )
                    : base(String.Format($"The {obj.GetType()} doesn't exist."))
                {
                }
            }

        }
    }
}
