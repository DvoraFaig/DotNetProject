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
            public ObjNotExistException(Type t, int id , Exception exception)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist." , exception))
            {
            }
            public ObjNotExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} doesn't exist."))
            {
            }
            //public ObjNotExistException(string message ,Exception exception)
            //    : base(String.Format($"The {message}"))
            //{
            //}

        }
        public class ObjExistException : Exception
        {
            public ObjExistException(Type t, int id , Exception exception)
                : base(String.Format($"The {t.Name} with id {id} exist."),exception)
            {
            }
            public ObjExistException(Type t, int id)
                : base(String.Format($"The {t.Name} with id {id} exist."))
            {
            }
            //public ObjExistException(string message, Exception exception)
            //    : base(String.Format($"The {message}", exception))
            //{
            //}
        }   
        public class NoMatchingData: Exception
        {
            public NoMatchingData(Type t ,int id ,Exception exception)
                : base(string.Format($"{t.Name} id: {id} exist but data doesn't match"), exception)
            {

            }

        }

        public class DataChanged : Exception
        {
            public DataChanged(Type t, int id, Exception exception)
                : base(string.Format($"{t.Name} id: {id} exist but data doesn't match"), exception)
            { 
            }

            public DataChanged(Type t, int id)
                : base(string.Format($"{t.Name} id: {id} exist but data doesn't match"))
            {
            }

        }


        //public class DalConfigException : Exception
        //{
        //    public DalConfigException(string s)
        //        : base(String.Format($"{s}"))
        //    {
        //    }
        //}

    }
}
