using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace MReplace
{
    /// <summary>
    /// by Ziad Elmalki (http://www.codeproject.com/Articles/37549/CLR-Injection-Runtime-Method-Replacer)
    /// </summary>
    public static class MethodReplaceLogic
    {
        public static object ReplaceWith(this MethodBase dest, MethodBase source)
        {
            if (!MethodSignaturesEqual(source, dest))
            {
                throw new ArgumentException("The method signatures are not the same.", "source");
            }
            return ReplaceMethod(GetMethodAddress(source), dest);
        }

        internal static object ReplaceMethod(IntPtr srcAdr, MethodBase dest)
        {
            IntPtr destAdr = GetMethodAddress(dest);
            object oldPointer;
            unsafe
            {
                if (IntPtr.Size == 8)
                {
                    ulong* d = (ulong*)destAdr.ToPointer();
                    oldPointer = *d;
                    *d = *((ulong*)srcAdr.ToPointer());
                }
                else
                {
                    uint* d = (uint*)destAdr.ToPointer();
                    oldPointer = *d;
                    *d = *((uint*)srcAdr.ToPointer());
                }
            }
            return oldPointer;
        }

        internal static object ReplaceMethodBack(MethodBase dest, object oldPointer)
        {
            IntPtr destAdr = GetMethodAddress(dest);
            unsafe
            {
                if (IntPtr.Size == 8)
                {
                    ulong* d = (ulong*)destAdr.ToPointer();
                    *d = (ulong)oldPointer;
                }
                else
                {
                    uint* d = (uint*)destAdr.ToPointer();
                    *d = (uint)oldPointer;
                }
            }
            return oldPointer;
        }

        private static IntPtr GetMethodAddress(MethodBase method)
        {
            if ((method is DynamicMethod))
            {
                return GetDynamicMethodAddress(method);
            }

            // Prepare the method so it gets jited
            RuntimeHelpers.PrepareMethod(method.MethodHandle);

            // If 3.5 sp1 or greater than we have a different layout in memory.
            if (IsNet20Sp2OrGreater())
            {
                return GetMethodAddress20SP2(method);
            }

            unsafe
            {
                // Skip these
                const int skip = 10;

                // Read the method index.
                UInt64* location = (UInt64*)(method.MethodHandle.Value.ToPointer());
                int index = (int)(((*location) >> 32) & 0xFF);

                if (IntPtr.Size == 8)
                {
                    // Get the method table
                    ulong* classStart = (ulong*)method.DeclaringType.TypeHandle.Value.ToPointer();
                    ulong* address = classStart + index + skip;
                    return new IntPtr(address);
                }
                else
                {
                    // Get the method table
                    uint* classStart = (uint*)method.DeclaringType.TypeHandle.Value.ToPointer();
                    uint* address = classStart + index + skip;
                    return new IntPtr(address);
                }
            }
        }

        private static IntPtr GetDynamicMethodAddress(MethodBase method)
        {
            unsafe
            {
                byte* ptr = (byte*)GetDynamicMethodRuntimeHandle(method).ToPointer();
                if (IsNet20Sp2OrGreater())
                {
                    if (IntPtr.Size == 8)
                    {
                        ulong* address = (ulong*)ptr;
                        address = (ulong*)*(address + 5);
                        return new IntPtr(address + 12);
                    }
                    else
                    {
                        uint* address = (uint*)ptr;
                        address = (uint*)*(address + 5);
                        return new IntPtr(address + 12);
                    }
                }
                else
                {
                    if (IntPtr.Size == 8)
                    {
                        ulong* address = (ulong*)ptr;
                        address += 6;
                        return new IntPtr(address);
                    }
                    else
                    {
                        uint* address = (uint*)ptr;
                        address += 6;
                        return new IntPtr(address);
                    }
                }
            }
        }

        private static IntPtr GetDynamicMethodRuntimeHandle(MethodBase method)
        {
            if (method is DynamicMethod)
            {
                FieldInfo fieldInfo = typeof(DynamicMethod).GetField("m_method", BindingFlags.NonPublic | BindingFlags.Instance);
                return ((RuntimeMethodHandle)fieldInfo.GetValue(method)).Value;
            }
            return method.MethodHandle.Value;
        }

        private static IntPtr GetMethodAddress20SP2(MethodBase method)
        {
            unsafe
            {
                return new IntPtr(((int*)method.MethodHandle.Value.ToPointer() + 2));
            }
        }

        private static bool MethodSignaturesEqual(MethodBase x, MethodBase y)
        {
            if (x.CallingConvention != y.CallingConvention)
            {
                return false;
            }
            Type returnX = GetMethodReturnType(x), returnY = GetMethodReturnType(y);
            if (returnX != returnY)
            {
                return false;
            }
            ParameterInfo[] xParams = x.GetParameters(), yParams = y.GetParameters();
            if (xParams.Length != yParams.Length)
            {
                return false;
            }
            for (int i = 0; i < xParams.Length; i++)
            {
                if (xParams[i].ParameterType != yParams[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }

        private static Type GetMethodReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
            {
                // Constructor info.
                throw new ArgumentException("Unsupported MethodBase : " + method.GetType().Name, "method");
            }
            return methodInfo.ReturnType;
        }

        private static bool IsNet20Sp2OrGreater()
        {
            return Environment.Version.Major > 2;
        }
    }
}