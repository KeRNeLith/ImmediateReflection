using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ImmediateReflection.Tests;

/// <summary>
/// Contains some useful helpers for Immediate Reflection tests.
/// </summary>
internal static class TestHelpers
{
    /// <inheritdoc cref="ImmediateMember.GetAttributes{TAttribute}"/>
    public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ImmediateMember member, bool inherit = false)
        where TAttribute : Attribute
    {
        return member.GetAttributes<TAttribute>(inherit).Where(x => x is not NullableContextAttribute);
    }

    /// <inheritdoc cref="ImmediateMember.GetAttributes"/>
    public static IEnumerable<Attribute> GetAttributes(this ImmediateMember member, Type attributeType, bool inherit = false)
    {
        return member.GetAttributes(attributeType, inherit).Where(x => x is not NullableContextAttribute);
    }

    /// <inheritdoc cref="ImmediateMember.GetAllAttributes"/>
    public static IEnumerable<Attribute> GetAllAttributes(this ImmediateMember member, bool inherit = false)
    {
        return member.GetAllAttributes(inherit).Where(x => x is not NullableContextAttribute);
    }
}