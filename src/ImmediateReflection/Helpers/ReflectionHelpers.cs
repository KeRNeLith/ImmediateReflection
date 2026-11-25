using System;
using System.Collections;
using System.Collections.Generic;
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
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [ContractAnnotation("param:null => halt")]
    public static bool IsParams(ParameterInfo param)
    {
        return param.IsDefined(typeof(ParamArrayAttribute), inherit: false);
    }

    /// <summary>
    /// Checks if the given <paramref name="property"/> is an indexed one.
    /// </summary>
    /// <param name="property">A <see cref="T:System.Reflection.PropertyInfo"/>.</param>
    /// <returns>True if the <paramref name="property"/> is an indexed property, false otherwise.</returns>
    [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [ContractAnnotation("property:null => halt")]
    public static bool IsIndexed(PropertyInfo property)
    {
        return property.GetIndexParameters().Length != 0;
    }

    // Remarks on hashcodes of reflection members:
    // Under NET5.0+ the runtime creates one distinct instance per reflected type for inherited members
    // (properties, fields, events, methods, constructors), but their default hashcode only depends on the member
    // definition (metadata token and declaring type). Using such members as dictionary keys then leads to a massive
    // amount of hashcode collisions (one per reflected type), making lookups quadratic,
    // see https://github.com/dotnet/runtime/issues/114280.

    /// <summary>
    /// Computes the hashcode for given <paramref name="property"/>.
    /// </summary>
    /// <param name="property">A <see cref="T:System.Reflection.PropertyInfo"/>.</param>
    /// <returns>A hashcode corresponding to <paramref name="property"/>.</returns>
    [Pure]
    [ContractAnnotation("property:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int GetPropertyInfoHashCode(PropertyInfo property)
    {
#if !NETFRAMEWORK
        Type? declaringType = property.DeclaringType;
        Type? reflectedType = property.ReflectedType;
        if (declaringType is not null && reflectedType is not null)
            return HashCode.Combine(property.MetadataToken, declaringType.GetHashCode(), reflectedType.GetHashCode());
#endif
        return property.GetHashCode();
    }

    /// <summary>
    /// Computes the hashcode for given <paramref name="field"/>.
    /// </summary>
    /// <param name="field">A <see cref="T:System.Reflection.FieldInfo"/>.</param>
    /// <returns>A hashcode corresponding to <paramref name="field"/>.</returns>
    [Pure]
    [ContractAnnotation("field:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int GetFieldInfoHashCode(FieldInfo field)
    {
#if !NETFRAMEWORK
        Type? declaringType = field.DeclaringType;
        Type? reflectedType = field.ReflectedType;
        if (declaringType is not null && reflectedType is not null)
            return HashCode.Combine(field.MetadataToken, declaringType.GetHashCode(), reflectedType.GetHashCode());
#endif
        return field.GetHashCode();
    }

    /// <summary>
    /// Computes the hashcode for given <paramref name="member"/>. Properties and fields get a specialized hash, other kinds (including types) use the default one.
    /// </summary>
    /// <remarks>Prefer <see cref="GetPropertyInfoHashCode"/> and <see cref="GetFieldInfoHashCode"/> when the kind is known.</remarks>
    /// <param name="member">A <see cref="MemberInfo"/>.</param>
    /// <returns>A hashcode corresponding to <paramref name="member"/>.</returns>
    [Pure]
    [ContractAnnotation("member:null => halt")]
    public static int GetMemberInfoHashCode(MemberInfo member)
    {
        if (member is PropertyInfo property)
            return GetPropertyInfoHashCode(property);
        if (member is FieldInfo field)
            return GetFieldInfoHashCode(field);
        // Types hash by identity, efficient enough; other kinds (methods, constructors, events) are not targeted by the library
        return member.GetHashCode();
    }

    /// <summary>
    /// <see cref="IEqualityComparer"/> implementation for <see cref="PropertyInfo"/>.
    /// </summary>
    /// <remarks>
    /// Overrides the default equality comparer for <see cref="PropertyInfo"/> since it has a drastic performance issue
    /// under NET5.0+ target for inherited properties as described here: https://github.com/dotnet/runtime/issues/114280.
    /// </remarks>
    internal sealed class PropertyInfoEqualityComparer : IEqualityComparer, IEqualityComparer<PropertyInfo>
    {
        /// <inheritdoc />
        bool IEqualityComparer.Equals(object? x, object? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return ((PropertyInfo)x!).Equals(y);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetPropertyInfoHashCode((PropertyInfo)obj);
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
            return GetPropertyInfoHashCode(obj);
        }
    }

    /// <summary>
    /// <see cref="IEqualityComparer"/> implementation for <see cref="FieldInfo"/>.
    /// </summary>
    /// <remarks>
    /// Overrides the default equality comparer for <see cref="FieldInfo"/> since it has a drastic performance issue
    /// under NET5.0+ target for inherited fields (same as <see cref="PropertyInfoEqualityComparer"/>).
    /// </remarks>
    internal sealed class FieldInfoEqualityComparer : IEqualityComparer, IEqualityComparer<FieldInfo>
    {
        /// <inheritdoc />
        bool IEqualityComparer.Equals(object? x, object? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return ((FieldInfo)x!).Equals(y);
        }

        /// <inheritdoc />
        int IEqualityComparer.GetHashCode(object obj)
        {
            return GetFieldInfoHashCode((FieldInfo)obj);
        }

        /// <inheritdoc />
        public bool Equals(FieldInfo? x, FieldInfo? y)
        {
            AssertNotNull(x);
            AssertNotNull(y);

            return x!.Equals(y);
        }

        /// <inheritdoc />
        public int GetHashCode(FieldInfo obj)
        {
            return GetFieldInfoHashCode(obj);
        }
    }

    /// <summary>
    /// <see cref="IEqualityComparer"/> implementation for <see cref="MemberInfo"/> of any kind
    /// (used by caches mixing member kinds).
    /// </summary>
    /// <remarks>
    /// Overrides the default equality comparer for <see cref="MemberInfo"/> since it has a drastic performance issue
    /// under NET5.0+ target for inherited members as described here: https://github.com/dotnet/runtime/issues/114280.
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
            return GetMemberInfoHashCode((MemberInfo)obj);
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
            return GetMemberInfoHashCode(obj);
        }
    }
}
