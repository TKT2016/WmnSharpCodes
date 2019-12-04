using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WmnSharpStdCodes.Reflection
{
    public static class TypeHelper
    {
        public static ParameterInfo[] GetParameterInfos(PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetGetMethod() != null)
            {
                return propertyInfo.GetGetMethod().GetParameters();
            }
            else
            {
                return propertyInfo.GetSetMethod().GetParameters();
            }
        }

        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo another)
        {
            ParameterInfo[] paramInfos = another.GetParameters();
            //return GetConstructor(type, another.GetParameters());
            Type[] types = paramInfos.Select(P => P.ParameterType).ToArray();
            ConstructorInfo cc = type.GetConstructor(types);
            return cc;
        }

        public static MethodInfo GetMethod(Type type, MethodInfo another)
        {
            ParameterInfo[] paramInfos = another.GetParameters();
            Type[] types = paramInfos.Select(P => P.ParameterType).ToArray();
            return type.GetMethod(another.Name, types);
            //return GetMethod(type, another.Name, another.GetParameters());
        }


        public static bool  GetIsStatic(PropertyInfo SharpProperty)
        {
            bool IsStatic = false;
            if (SharpProperty.GetGetMethod() != null)
                IsStatic = SharpProperty.GetGetMethod().IsStatic;
            else
                IsStatic = SharpProperty.GetSetMethod().IsStatic;
            return IsStatic;
        }


        static Type[] NumberTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };

        static int indexOfNumber(Type type)
        {
            for(int i=0;i<NumberTypes.Length;i++)
            {
                if (NumberTypes[i] == type)
                    return i;
            }
            return -1;
        }

        public static bool IsStruct(Type atype)
        {
            if (!atype.IsValueType) return false;
            if (atype == typeof(bool) || atype == typeof(byte) || atype == typeof(char) || atype == typeof(short) || atype == typeof(int) || atype == typeof(long)
                  || atype == typeof(float) || atype == typeof(double) || atype == typeof(decimal))
                return false;
            return true;
        }


        public static bool IsValueType(Type atype)
        {
            return atype.IsValueType;
            //if (!atype.IsValueType) return false;
            //if (atype == typeof(bool) || atype == typeof(byte) || atype == typeof(char) || atype == typeof(short) || atype == typeof(int) || atype == typeof(long)
            //      || atype == typeof(float) || atype == typeof(double) || atype == typeof(decimal))
            //    return false;
            //return true;
        }

        public static bool IsNumberType(Type type)
        {
            //return type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(long) || type == typeof(byte) || type == typeof(short);
            return indexOfNumber(type) != -1;
        }

        //public static bool MoreEqNumberType(Type typea,Type typeb)
        //{
        //    int indexa = indexOfNumber(typea);
        //    int indexb = indexOfNumber(typeb);
        //    return indexa >= indexb;
        //}

        public static bool IsDeclare(Type type, FieldInfo property)
        {
            if (property.DeclaringType == type)
            {
                return true;
            }
            return false;
        }

        public static bool IsDeclare(Type type,PropertyInfo property)
        {
            if (property.DeclaringType == type)
             {
                 return true;
             }
             return false;
        }

        /// <summary>
        /// 是否是当前类定义的方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsDeclare(Type type, MethodInfo method)
        {
            if (method.DeclaringType == type)
            {
                return true;
            }
            return false;
        }

        public static bool IsExtends(Type subType, Type baseType)
        {
            if (subType == baseType)
            {
                return true;
            }
            if (subType.IsSubclassOf(baseType))
            {
                return true;
            }
            if (baseType.IsGenericType)
            {
                Type temp = GenericHelper.GetMakeGenericType(subType, baseType);
                if (temp.IsGenericType && temp.GetGenericTypeDefinition() == baseType)
                 {
                     return true;
                 }
            }
            else
            {
                if(baseType== typeof(object))
                {
                    return true;
                }
                else if (baseType == typeof(float) && subType == typeof(int))
                {
                    return true;
                }
                //if (subType.IsAssignableFrom(baseType))
                //{
                //    return true;
                //}
            }
            return false;
        }

        public static bool IsStatic(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static bool IsStatic(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsStatic) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsStatic) return true;
            return false;
        }

        public static bool IsPublic(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsPublic) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsPublic) return true;
            return false;
        }

        public static bool IsProtected(PropertyInfo property)
        {
            MethodInfo method = null;
            method = property.GetGetMethod();
            if (method != null && method.IsFamily) return true;
            method = property.GetSetMethod();
            if (method != null && method.IsFamily) return true;
            return false;
        }

        public static object NewInstance(Type type, params object[] args)
        {
            //List<object> argList = new List<object>();
            object obj = Activator.CreateInstance(type, args);
            if (obj == null)
            {
                throw new Exception("无法创建实例" + type.FullName);
            }
            return obj;
        }

        public static MethodInfo GetMethod(Type type, string methodName)
        {
            MethodInfo[] methods = type.GetMethods(
                BindingFlags.Public         //公共成员  
                | BindingFlags.Static        //为了获取返回值，必须指定 BindingFlags.Instance 或 BindingFlags.Static。  
                | BindingFlags.NonPublic     //非公共成员（即私有成员和受保护的成员）  
                | BindingFlags.Instance      //为了获取返回值，必须指定 BindingFlags.Instance 或 BindingFlags.Static。  
                //| BindingFlags.DeclaredOnly
                 );
            foreach (var method in methods)
            {
                if (method.Name == methodName)
                {
                    return method;
                }
            }
            return null;
        }

        public static bool IsRuntimeType(Type type)
        {
            if (type.IsPointer) return true;
            //if (IsExtends(type, typeof(object))) return false;
            return false;
            //return type.ToString() == "System.RuntimeType";
        }

        public static object CallStatic(Type type, string funcName, params object[] args)
        {
            object[] newArgs = args;
            MethodInfo func = GetMethodInfo(type, funcName, newArgs);
            object result = func.Invoke(null, newArgs);
            return result;
        }

        public static object CallInstance(object obj, string funcName, params object[] args)
        {
            object[] newArgs = args;
            object result = null;

            if (obj is Type)
            {
                //debug("is Type " + obj.ToString());
                Type type = (Type)obj;
                MethodInfo func = GetMethodInfo(type, funcName, newArgs);
                result = func.Invoke(null, newArgs);
            }
            else
            {
                //debug("not is Type " + obj.ToString());
                Type type = obj.GetType();
                MethodInfo func = GetMethodInfo(type, funcName, newArgs);
                result = func.Invoke(obj, newArgs);
            }
            //debug(" **** call result= " + result.ToString());
            return result;
        }

        public static MethodInfo GetMethodInfo(Type type, string funcName, params object[] args)
        {
            List<Type> types = new List<Type>();
            foreach (object arg in args)
            {
                types.Add(arg.GetType());
                //debug("arg = "+arg.ToString()+", type = " +arg.GetType().FullName);
            }

            MethodInfo func = type.GetMethod(funcName, types.ToArray());
            if (func == null)
            {
                throw new Exception(string.Format("没有找到类型{0}的方法{1}({2})", type.FullName, funcName, string.Join(",", types.Select(p => p.FullName))));
            }

            return func;
        }

        public static object GetValue(object obj, string memberName)
        {
            Type type = null;
            if (obj is Type)
            {
                type = (Type)obj;
            }
            else
            {
                type = obj.GetType();
            }

            var property = type.GetProperty(memberName);
            if (property != null)
            {
                return property.GetValue(obj is Type ? null : obj, null);
            }

            var field = type.GetField(memberName);
            if (field != null)
            {
                return field.GetValue(obj is Type ? null : obj);
            }

            throw new Exception("找不到类型" + type.FullName + "的" + memberName + "成员,无法取值");
        }
    }
}
