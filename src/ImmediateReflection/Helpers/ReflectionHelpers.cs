using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

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
    public static bool IsParams([NotNull] ParameterInfo param)
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
    public static bool IsIndexed([NotNull] PropertyInfo property)
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
    public static int GetPropertyInfoHashCode([NotNull] PropertyInfo property)
    {
#if !NETFRAMEWORK
            if (property.ReflectedType != null && property.DeclaringType != null)
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
        bool IEqualityComparer.Equals(object x, object y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);

            return Equals((PropertyInfo)x, (PropertyInfo)y);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetPropertyInfoHashCode((PropertyInfo)obj);
        }

        /// <inheritdoc />
        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);

            return x.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(PropertyInfo obj)
        {
            return GetPropertyInfoHashCode(obj);
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
        bool IEqualityComparer.Equals(object x, object y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);

            return Equals((MemberInfo)x, (MemberInfo)y);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetHashCode((MemberInfo)obj);
        }

        /// <inheritdoc />
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);

            return x.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(MemberInfo obj)
        {
            if (obj is PropertyInfo property)
                return GetPropertyInfoHashCode(property);
            return obj.GetHashCode();
        }
    }
}