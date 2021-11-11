﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
/// Provides a method for performing a deep copy of an object.
/// Binary Serialization is used to perform the copy.
/// </summary>
//public static class ObjectCopier
//{
//    /// <summary>
//    /// Perform a deep copy of the object via serialization.
//    /// </summary>
//    /// <typeparam name="T">The type of object being copied.</typeparam>
//    /// <param name="source">The object instance to copy.</param>
//    /// <returns>A deep copy of the object.</returns>
//    public static T Clone<T>(T source)
//    {

//        return source;
//    }
//}


//[Serializable]
//public class X
//{
//    public string str;
//    public int i;
//}

public static class Extensions
{
    public static T DeepClone<T>(this T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }
}


        //X obj = new X();
        //obj.str = "Hello!";
        //obj.i = 5;

        //X copy = obj.DeepClone();
        //Console.WriteLine(copy.str + "  " + obj.i);
 