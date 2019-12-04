using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WmnSharpStdCodes.Reflection
{
    public static class AttributeHelper
    {
        public static bool HasAttributes(Type class1, params Type[] types)
        {
            foreach(var item in types)
            {
                if (HasAttribute(class1,item))
                    return true;
            }
            return false;
        }

        public static bool HasAttribute( Type class1,Type typ2) //where T : Attribute
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(class1);
            foreach (var attr in attrs)
            {
                if (attr.GetType() == typ2)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasAttribute<T>(MemberInfo member) where T : Attribute
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(member);
            foreach(var  attr in attrs)
            {
                if(attr is T)
                {
                    return true;
                }
            }
            return false;
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attr = Attribute.GetCustomAttribute(member, typeof(T));
            if (attr == null) return null;
            return attr as T;
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(member, typeof(T));
            if (attrs == null) return new T[] { };
            T[] attrs2 = new T[attrs.Length];
            for (int i = 0; i < attrs.Length; i++)
            {
                attrs2[i] = attrs[i] as T;
            }
            return attrs2;
        }
    }
}
