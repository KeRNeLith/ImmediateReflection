using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;

namespace ImmediateReflection.Utils;

/// <summary>
/// Helpers to use C# reflection.
/// </summary>
internal static class ReflectionHelpers
{
    /// <summary>
    /// Checks if the given parameter is a parameter using "params".
    /// </summary>
    /// <param name="param">A <see cref="ParameterInfo"/>.</param>
    /// <returns>True if the parameter correspond to a "params" parameter, false otherwise.</returns>
    [Pure]
    [ContractAnnotation("param:null => halt")]
    public static bool IsParams(ParameterInfo param)
    {
        return param.IsDefined(typeof(ParamArrayAttribute), false);
    }

    /// <summary>
    /// Checks if the given <paramref name="property"/> is an indexed one.
    /// </summary>
    /// <param name="property">A <see cref="T:System.Reflection.PropertyInfo"/>.</param>
    /// <returns>True if the <paramref name="property"/> is an indexed property, false otherwise.</returns>
    [Pure]
    [ContractAnnotation("property:null => halt")]
    public static bool IsIndexed(PropertyInfo property)
    {
        return property.GetIndexParameters().Length != 0;
    }

    /// <summary>
    /// Computes the hashcode for given <paramref name="property"/>.
    /// </summary>
    /// <param name="property">A <see cref="T:System.Reflection.PropertyInfo"/>.</param>
    /// <returns>A hashcode corresponding to <paramref name="property"/>.</returns>
    [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int GetPropertyInfoHashCode(PropertyInfo property)
    {
#if !NETFRAMEWORK
        if (property.ReflectedType is not null && property.DeclaringType is not null)
            return HashCode.Combine(property.MetadataToken, property.DeclaringType.GetHashCode(), property.ReflectedType.GetHashCode());
#endif
        return property.GetHashCode();
    }

    /// <summary>
    /// <see cref="IEqualityComparer"/> implementation for <see cref="PropertyInfo"/>.
    /// </summary>
    /// <remarks>
    /// Overrides the default equality comparer for <see cref="PropertyInfo"/> since it has a drastic performance issue under NET5.0+ target.
    /// as described here: https://github.com/dotnet/runtime/issues/114280.
    /// </remarks>
    internal sealed class PropertyInfoEqualityComparer : IEqualityComparer, IEqualityComparer<PropertyInfo>
    {
        /// <inheritdoc />
        bool IEqualityComparer.Equals(object? x, object? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return Equals((PropertyInfo)x!, (PropertyInfo)y!);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            //Debugger.Launch();
            return MemberHash.GetPropertyHash((PropertyInfo)obj); //GetPropertyInfoHashCode((PropertyInfo)obj);
        }

        /// <inheritdoc />
        public bool Equals(PropertyInfo? x, PropertyInfo? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return x!.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(PropertyInfo obj)
        {
            //Debugger.Launch();
            return MemberHash.GetPropertyHash(obj); //GetPropertyInfoHashCode(obj);
        }
    }

    /// <summary>
    /// <see cref="IEqualityComparer"/> implementation for <see cref="MemberInfo"/>.
    /// </summary>
    /// <remarks>
    /// Overrides the default equality comparer for <see cref="PropertyInfo"/> (similarly to <see cref="PropertyInfoEqualityComparer"/>)
    /// since it has a drastic performance issue under NET5.0+ target as described here:
    /// https://github.com/dotnet/runtime/issues/114280.
    /// </remarks>
    internal sealed class MemberInfoEqualityComparer : IEqualityComparer, IEqualityComparer<MemberInfo>
    {
        /// <inheritdoc />
        bool IEqualityComparer.Equals(object? x, object? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return Equals((MemberInfo)x!, (MemberInfo)y!);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode((MemberInfo)obj);
        }

        /// <inheritdoc />
        public bool Equals(MemberInfo? x, MemberInfo? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return x!.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(MemberInfo obj)
        {
            return MemberHash.Get(obj);
            //if (obj is PropertyInfo property)
            //    return GetPropertyInfoHashCode(property);
            //return obj.GetHashCode();
        }
    }


    public static class MemberHash
    {
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int Get(MemberInfo member)
        {
            //if (member is null)
            //    return 0;

            return member switch
            {
                Type t => t.GetHashCode(), // stable and fast
                //MethodBase m => GetMethodHash(m),
                PropertyInfo p => GetPropertyHash(p),
                FieldInfo f => GetFieldHash(f),
                //EventInfo e => GetEventHash(e),
                _ => Fallback(member)
            };
        }

        private static int GetMethodHash(MethodBase method)
        {
            unchecked
            {
                if (method.MetadataToken != 0)
                    return Combine(method.MetadataToken, method.Module.GetHashCode(), method.DeclaringType?.GetHashCode() ?? 0);

                // Dynamic case: fall back on signature-based identity
                int hash = 17;
                hash = hash * 31 + (method.Name?.GetHashCode() ?? 0);
                hash = hash * 31 + (method.DeclaringType?.GetHashCode() ?? 0);
                var parameters = method.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                    hash = hash * 31 + parameters[i].ParameterType.GetHashCode();
                return hash;
            }
        }
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static int GetPropertyHash(PropertyInfo property)
        {
            unchecked
            {
                if (property.MetadataToken != 0)
                    return Combine(property.MetadataToken, property.Module.GetHashCode(), property.DeclaringType?.GetHashCode() ?? 0);

                // Dynamic property fallback
                int hash = 17;
                hash = hash * 31 + (property.Name?.GetHashCode() ?? 0);
                hash = hash * 31 + (property.DeclaringType?.GetHashCode() ?? 0);
                hash = hash * 31 + (property.PropertyType?.GetHashCode() ?? 0);
                return hash;
            }
        }
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static int GetFieldHash(FieldInfo field)
        {
            unchecked
            {
                if (field.MetadataToken != 0)
                    return Combine(field.MetadataToken, field.Module.GetHashCode(), field.DeclaringType?.GetHashCode() ?? 0);

                // Dynamic field fallback
                int hash = 17;
                hash = hash * 31 + (field.Name?.GetHashCode() ?? 0);
                hash = hash * 31 + (field.DeclaringType?.GetHashCode() ?? 0);
                hash = hash * 31 + (field.FieldType?.GetHashCode() ?? 0);
                return hash;
            }
        }

        //private static int GetEventHash(EventInfo evt)
        //{
        //    unchecked
        //    {
        //        if (evt.MetadataToken != 0)
        //            return Combine(evt.MetadataToken, evt.Module.GetHashCode(), evt.DeclaringType?.GetHashCode() ?? 0);

        //        return Combine(evt.Name?.GetHashCode() ?? 0, evt.DeclaringType?.GetHashCode() ?? 0);
        //    }
        //}

        private static int Fallback(MemberInfo member)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (member.Name?.GetHashCode() ?? 0);
                hash = hash * 31 + (member.DeclaringType?.GetHashCode() ?? 0);
                hash = hash * 31 + (member.Module?.GetHashCode() ?? 0);
                return hash;
            }
        }

#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static int Combine(int h1, int h2, int h3)
        {
            unchecked
            {
                int hash = (h1 * 31 + h2);
                return hash * 31 + h3;
            }
        }
    }

    public sealed class MemberInfoComparer : IEqualityComparer<MemberInfo>
    {
        public static readonly MemberInfoComparer Instance = new();

        public bool Equals(MemberInfo? x, MemberInfo? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.Module != y.Module) return false;
            if (x.DeclaringType != y.DeclaringType) return false;

            // Handle both static and dynamic
            if (x.MetadataToken != 0 && y.MetadataToken != 0)
                return x.MetadataToken == y.MetadataToken;

            return x.Name == y.Name;
        }

        public int GetHashCode(MemberInfo obj) => MemberHash.Get(obj);
    }

}