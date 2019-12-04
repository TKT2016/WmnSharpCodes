using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace WmnSharpStdCodes.Reflection
{
    public static class ILEmitHelper
    {
        public static void LE(ILGenerator IL)
        {
            IL.Emit(OpCodes.Cgt);  //  IL_0003:  cgt
            IL.Emit(OpCodes.Ldc_I4_0);  //IL_0005:  ldc.i4.0
            IL.Emit(OpCodes.Ceq);   //IL_0006:  ceq
        }

        public static void LoadVar(ILGenerator il, LocalBuilder local)
        {
            il.Emit(OpCodes.Ldloc, local);
        }

        public static void LoadVara(ILGenerator il, LocalBuilder local)
        {
            il.Emit(OpCodes.Ldloca, local);
        }

        public static void LoadDefaultValue(ILGenerator il, Type type)
        {
            if (type == typeof(bool))
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            else if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, (float)0.0);
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, (float)0.0);
            }
            else if (type == typeof(string))
            {
                il.Emit(OpCodes.Ldstr, "");
            }
            else if (type == typeof(char))
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            else
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { });
                if (constructor == null)
                {
                    il.Emit(OpCodes.Ldnull);
                }
                else
                {
                    ILEmitHelper.NewObj(il, constructor);
                }
            }
        }

        public static void LoadArga(ILGenerator il, int argIndex)
        {
            il.Emit(OpCodes.Ldarga, argIndex);
        }

        public static void LoadArg(ILGenerator il, int argIndex, bool isStruct)
        {
            if (!isStruct)
            {
                if (argIndex == 0)
                {
                    il.Emit(OpCodes.Ldarg_0);
                }
                else if (argIndex == 1)
                {
                    il.Emit(OpCodes.Ldarg_1);
                }
                else if (argIndex == 2)
                {
                    il.Emit(OpCodes.Ldarg_2);
                }
                else if (argIndex == 3)
                {
                    il.Emit(OpCodes.Ldarg_3);
                }
                else if (argIndex <= 255)
                {
                    il.Emit(OpCodes.Ldarg_S, argIndex);
                }
                else
                {
                    il.Emit(OpCodes.Ldarg, argIndex);
                }
            }

            else
                il.Emit(OpCodes.Ldarga, argIndex);
        }

        public static void StormField(ILGenerator il, FieldInfo field)
        {
            if (field.IsStatic)
            {
                il.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                il.Emit(OpCodes.Stfld, field);
            }
        }

        public static void LoadField(ILGenerator il, FieldInfo field)
        {
            if (field.IsLiteral)
            {
                object value = field.GetValue(null);
                if (value is int)
                {
                    ILEmitHelper.LoadInt(il, (int)value);
                }
                else if (value is float)
                {
                    il.Emit(OpCodes.Ldc_R4, (float)value);
                }
                else if (value is string)
                {
                    il.Emit(OpCodes.Ldstr, (string)value);
                }
                else if (value is bool)
                {
                    bool bv = (bool)value;
                    if (bv)
                    {
                        il.Emit(OpCodes.Ldc_I4_1);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                    }
                }
                else
                {
                    throw new Exception("编译器不支持" + field.FieldType.Name + "类型");
                }
            }
            else if (field.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                il.Emit(OpCodes.Ldfld, field);
            }
        }

        public static void LoadFielda(ILGenerator il, FieldInfo field)
        {
            if (field.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, field);
            }
            else
            {
                il.Emit(OpCodes.Ldflda, field);
            }
        }

        public static void StormVar(ILGenerator il, LocalBuilder local)
        {
            il.Emit(OpCodes.Stloc, local);
        }

        public static void StormArg(ILGenerator il, int argIndex)
        {
            il.Emit(OpCodes.Starg, argIndex);
        }

        public static void EmitBool(ILGenerator il, bool b)
        {
            if (b)
                il.Emit(OpCodes.Ldc_I4_1);
            else
                il.Emit(OpCodes.Ldc_I4_0);
        }

        public static void TypeOf(ILGenerator il, Type type)
        {
            il.Emit(OpCodes.Ldtoken, type);
            il.Emit(OpCodes.Call, typeof(System.Type).GetMethod("GetTypeFromHandle"));
        }

        public static void EmitThis(ILGenerator il, bool isStatic)
        {
            if (!isStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
        }

        public static void Inc(ILGenerator il, LocalBuilder local)
        {
            ILEmitHelper.LoadVar(il, local);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Add);
            ILEmitHelper.StormVar(il, local);
        }

        public static void Dec(ILGenerator il, LocalBuilder local)
        {
            ILEmitHelper.LoadVar(il, local);
            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Sub);
            ILEmitHelper.StormVar(il, local);
        }

        public static void LoadString(ILGenerator il, string str)
        {
            il.Emit(OpCodes.Ldstr, str);
        }

        public static void LoadInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }

        //public static void SetLocalArrayElementValue(ILGenerator il, LocalBuilder arrayLocal, int index, Action emitValue)
        //{
        //    EmitHelper.LoadVar(il, arrayLocal);
        //    EmitHelper.LoadInt(il, index);
        //    emitValue();
        //    il.Emit(OpCodes.Stelem_Ref);
        //}

        public static void NewArray(ILGenerator il, int length, Type type)
        {
            LoadInt(il, length);
            il.Emit(OpCodes.Newarr, type);
        }

        public static void NewObj(ILGenerator il, ConstructorInfo newMethod)
        {
            il.Emit(OpCodes.Newobj, newMethod);
        }

        public static void Call(ILGenerator il, MethodInfo method)
        {
            il.Emit(OpCodes.Call, method);
        }

        public static void CallDynamic(ILGenerator il, MethodInfo method)
        {
            if (method.IsStatic)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }
        }

        public static void EmitConv(ILGenerator il, Type targetType, Type curType)
        {
            //if (targetType == null) return;
            if (targetType == typeof(object) && curType.IsValueType)
            {
                il.Emit(OpCodes.Box, curType);
            }
            if (targetType == curType)
            {
                return;
            }
            OpCode op = GetConvOp(targetType, curType);
            if (op != OpCodes.Nop)
            {
                il.Emit(op);
            }
            //else if (targetType == typeof(float) && curType == typeof(int))
            //{
            //    il.Emit(OpCodes.Conv_R4);
            //}
            //else if (targetType == typeof(int) && curType == typeof(float))
            //{
            //    il.Emit(OpCodes.Conv_I4);
            //}
        }

        private static OpCode GetConvOp(Type targetType, Type curType)
        {
            if (targetType == typeof(double))//&& curType == typeof(int))
            {
                return OpCodes.Conv_R8;
            }
            else if (targetType == typeof(float)) //&& curType == typeof(float))
            {
                return OpCodes.Conv_R4;
            }
            else if (targetType == typeof(long)) //&& curType == typeof(float))
            {
                return OpCodes.Conv_I8;
            }
            else if (targetType == typeof(int)) //&& curType == typeof(float))
            {
                return OpCodes.Conv_I4;
            }
            return OpCodes.Nop;
        }
    }
}
