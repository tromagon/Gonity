using System;
using System.Collections.Generic;
using System.Reflection;

public class ReflectionUtils
{
    public static FieldInfo GetCustomAttributeField<T>(Type type)
    {
        FieldInfo[] fInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        int l = fInfos.Length;
        for (int i = 0; i < l; i++)
        {
            FieldInfo fInfo = fInfos[i];
            if (fInfo.GetCustomAttributes(typeof(T), false).Length == 0) continue;

            return fInfo;
        }

        return null;
    }

    public static List<FieldInfo> GetCustomAttributeFields<T>(Type type)
    {
        List<FieldInfo> attributes = new List<FieldInfo>();
        FieldInfo[] fInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        int l = fInfos.Length;
        for (int i = 0; i < l; i++)
        {
            FieldInfo fInfo = fInfos[i];
            if (fInfo.GetCustomAttributes(typeof(T), false).Length == 0) continue;

            attributes.Add(fInfo);
        }

        return attributes;
    }

    public static MethodInfo GetCustomAttributeMethod<T>(Type type)
    {
        MethodInfo[] fInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        int l = fInfos.Length;
        for (int i = 0; i < l; i++)
        {
            MethodInfo fInfo = fInfos[i];
            if (fInfo.GetCustomAttributes(typeof(T), false).Length == 0) continue;

            return fInfo;
        }

        return null;
    }
}