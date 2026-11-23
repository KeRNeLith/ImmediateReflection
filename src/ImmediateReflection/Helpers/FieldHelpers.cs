using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;

namespace ImmediateReflection.Utils;

/// <summary>
/// Helpers to work with fields.
/// </summary>
internal static class FieldHelpers
{
    private const string BackingFieldName = "BackingField";

    /// <summary>
    /// Checks if the given <see cref="FieldInfo"/> corresponds to a backing field.
    /// </summary>
    /// <param name="field">The <see cref="FieldInfo"/>.</param>
    /// <returns>True if the <see cref="FieldInfo"/> is a backing field, false otherwise.</returns>
    [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [ContractAnnotation("field:null => halt")]
    private static bool IsBackingField(FieldInfo field)
    {
        AssertNotNull(field);

        return field.Name.Contains(BackingFieldName);
    }

    /// <summary>
    /// Gets an array of <see cref="FieldInfo"/> without backing fields.
    /// </summary>
    /// <param name="fields">Array of <see cref="FieldInfo"/> to filter.</param>
    /// <returns>Filtered <see cref="FieldInfo"/> (the given array itself if there is nothing to filter).</returns>
    [Pure]
    [ContractAnnotation("fields:null => halt")]
    internal static FieldInfo[] IgnoreBackingFields(FieldInfo[] fields)
    {
        AssertNotNull(fields);

        int backingFieldsCount = 0;
        foreach (FieldInfo field in fields)
        {
            if (IsBackingField(field))
            {
                ++backingFieldsCount;
            }
        }

        if (backingFieldsCount == 0)
            return fields;

        var filtered = new FieldInfo[fields.Length - backingFieldsCount];
        int index = 0;
        foreach (FieldInfo field in fields)
        {
            if (!IsBackingField(field))
            {
                filtered[index++] = field;
            }
        }

        return filtered;
    }
}