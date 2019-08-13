using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_SYSTEM_CORE
using System.Linq;
#else
using static ImmediateReflection.Utils.EnumerableUtils;
#endif
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Helpers to work with fields.
    /// </summary>
    internal static class FieldHelpers
    {
        [NotNull]
        private const string BackingFieldName = "BackingField";

        /// <summary>
        /// Checks if the given <see cref="FieldInfo"/> corresponds to a backing field.
        /// </summary>
        /// <param name="field">The <see cref="FieldInfo"/>.</param>
        /// <returns>True if the <see cref="FieldInfo"/> is a backing field, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("field:null => halt")]
        internal static bool IsBackingField([NotNull] FieldInfo field)
        {
            Debug.Assert(field != null);

            return field.Name.Contains(BackingFieldName);
        }

        /// <summary>
        /// Gets an enumerable of <see cref="FieldInfo"/> without backing fields.
        /// </summary>
        /// <param name="fields">Enumerable of <see cref="FieldInfo"/> to filter.</param>
        /// <returns>Filtered <see cref="FieldInfo"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        [ContractAnnotation("fields:null => halt")]
        internal static IEnumerable<FieldInfo> IgnoreBackingFields([NotNull, ItemNotNull] IEnumerable<FieldInfo> fields)
        {
            Debug.Assert(fields != null);

#if SUPPORTS_SYSTEM_CORE
            return fields.Where(field => !IsBackingField(field));
#else
            return Where(fields, field => !IsBackingField(field));
#endif
        }
    }
}