using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    [ContractAnnotation("field:null => halt")]
    private static bool IsBackingField(FieldInfo field)
    {
        AssertNotNull(field);

        return field.Name.Contains(BackingFieldName);
    }

    /// <summary>
    /// Gets an enumerable of <see cref="FieldInfo"/> without backing fields.
    /// </summary>
    /// <param name="fields">Enumerable of <see cref="FieldInfo"/> to filter.</param>
    /// <returns>Filtered <see cref="FieldInfo"/>.</returns>
    [Pure]
    [ContractAnnotation("fields:null => halt")]
    internal static IEnumerable<FieldInfo> IgnoreBackingFields(IEnumerable<FieldInfo> fields)
    {
        // ReSharper disable PossibleMultipleEnumeration, Justification: Only in debug and not really enumerating whole enumerable
        AssertNotNull(fields);

        return fields.Where(field => !IsBackingField(field));
        // ReSharper restore PossibleMultipleEnumeration
    }
}