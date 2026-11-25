using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace ImmediateReflection.Utils;

/// <summary>
/// Helpers to validate data coming from a <see cref="SerializationInfo"/> (considered as untrusted).
/// </summary>
internal static class SerializationHelpers
{
    /// <summary>
    /// Reads and validates a <see cref="Type"/> stored in the given <paramref name="info"/>.
    /// </summary>
    public static Type ReadType(SerializationInfo? info, string name)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        return (Type?)info.GetValue(name, typeof(Type))
               ?? throw new SerializationException("Serialized type is null.");
    }

    private static readonly BindingFlags AllowedFlags = ComputeAllowedFlags();

    private static BindingFlags ComputeAllowedFlags()
    {
        BindingFlags allowed = BindingFlags.Default;
        foreach (BindingFlags flag in (BindingFlags[])Enum.GetValues(typeof(BindingFlags)))
        {
            allowed |= flag;
        }

        return allowed;
    }

    /// <summary>
    /// Reads and validates the <see cref="BindingFlags"/> stored in the given <paramref name="info"/>.
    /// </summary>
    public static BindingFlags ReadFlags(SerializationInfo? info, string name)
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        var flags = (BindingFlags?)info.GetValue(name, typeof(BindingFlags));
        if (flags is null || (flags.Value & ~AllowedFlags) != 0)
            throw new SerializationException("Serialized binding flags are invalid.");

        return flags.Value;
    }

    /// <summary>
    /// Maximum number of members accepted in a serialized collection.
    /// </summary>
    private const int MaxMembersCount = 65535;

    /// <summary>
    /// Reads and validates members stored in the given <paramref name="info"/>.
    /// Members must be not null and have distinct names.
    /// Members are not required to come from the same type (collections can be built from any members).
    /// </summary>
    public static List<T> ReadMembers<T>(SerializationInfo? info, string memberKeyPrefix)
        where T : MemberInfo
    {
        if (info is null)
            throw new ArgumentNullException(nameof(info));

        int count = (int)info.GetValue("Count", typeof(int))!;
        if (count is < 0 or > MaxMembersCount)
            throw new SerializationException("Invalid members count in serialized data.");

        List<T> members = new(count);
        HashSet<string> names = new(StringComparer.Ordinal);
        for (int i = 0; i < count; ++i)
        {
            T member = (T?)info.GetValue($"{memberKeyPrefix}{i}", typeof(T))
                       ?? throw new SerializationException($"Serialized member {i} is null.");

            if (!names.Add(member.Name))
                throw new SerializationException($"Serialized member '{member.Name}' is duplicated.");

            members.Add(member);
        }

        return members;
    }
}
