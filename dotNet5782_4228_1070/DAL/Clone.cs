//using System;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using System.Reflection;

///// <summary>
///// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
///// Provides a method for performing a deep copy of an object.
///// Binary Serialization is used to perform the copy.
///// </summary>



////[Serializable]
////public class X
////{
////    public string str;
////    public int i;
////}

//namespace DO
//{
//    public static class Extensions
//    {
//        public static T DeepClone<T>(this T obj)
//        {
//            using (MemoryStream stream = new MemoryStream())
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                formatter.Serialize(stream, obj);
//                stream.Position = 0;

//                return (T)formatter.Deserialize(stream);
//            }
//        }
//    }
//}



//namespace DO
//{
//    static class Cloning
//    {
//        public static T Clone<T>(this T original) where T : new()
//        {
//            T newObj = new T();
//            foreach (PropertyInfo prop in typeof(T).GetProperties())
//                prop.SetValue(newObj, prop.GetValue(original, null), null);
//            return newObj;
//        }
//    }
//}

