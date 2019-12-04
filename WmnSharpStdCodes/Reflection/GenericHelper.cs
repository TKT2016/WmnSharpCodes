using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WmnSharpStdCodes.Reflection
{
    public static class GenericHelper
    {
        /// <summary>
        /// 判断subType是否是由泛型topType创建的子泛型,比如IsInstanceOfGenericType（List<string> ，List<T> ）==true
        /// </summary>
        public static bool IsInstanceOfGenericType(Type subType,Type topType)
        {
            if (subType.IsGenericType == false) return false;
            if (topType.IsGenericType == false) return false;
            if (topType.IsGenericTypeDefinition == false) return false;
            Type subTopType = subType.GetGenericTypeDefinition();
            return subTopType == topType;
        }

        public static bool IsGenericType(Type type)
        {
            if (type == null) throw new NullReferenceException();
            return type.IsGenericType;
        }

        public static Type GetMakeGenericType(Type type, Type baseType)
        {
            Type temp = type;
            while (true)
            {
                if (temp == typeof(object)) break;
                if (temp.BaseType == null) break;
                if (temp.IsGenericType)
                {
                    if (temp.GetGenericTypeDefinition() == baseType)
                        break;
                }
                temp = temp.BaseType;
            }
            return temp;
        }

        public static Type[] GetInstanceGenriceType(Type type, Type baseType)
        {
            if (baseType.IsInterface)
            {
                Type[] intefaceTypes = type.GetInterfaces();
                foreach (var intfType in intefaceTypes)
                {
                    if (intfType.IsGenericType)
                    {
                        if (intfType.GetGenericTypeDefinition() == baseType)
                        {
                            Type[] types = intfType.GetGenericArguments();
                            return types;
                        }
                    }
                }
                return new Type[] { };
            }
            else
            {
                Type temp = GetMakeGenericType(type, baseType);
                if (temp.IsGenericType)
                {
                    Type[] types = type.GetGenericArguments();
                    return types;
                }
                return new Type[] { };
            }
        }

        public static string GetGenericTypeShortName(Type type)
        {
            string name = type.Name;
            int index = name.IndexOf("`");
            if (index == -1) return name;
            return name.Substring(0, index);
        }

        public static int GetGenericTypeArgCount(Type type)
        {
            string name = type.Name;
            int index = name.IndexOf("`");
            if (index == -1) return 0;
            string str = name.Substring(index + 1);
            return int.Parse(str);
        }

    }
}
