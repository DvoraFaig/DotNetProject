using System;


namespace BO
{
    public static partial class Exceptions
    {
        public class ObjNotExistException<T> : Exception
        {
            public ObjNotExistException(T obj, string message)
                : base(String.Format($"The {message} {obj.GetType()} doesn't exist."))
            {
            }
        }

    }
}

