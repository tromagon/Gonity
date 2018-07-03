using System;
using System.Collections.Generic;

namespace Gonity
{
    public static class EnumUtils
    {
        public static List<T> GetList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new List<T>(array);
            return list;
        }

        public static T[] GetArray<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}