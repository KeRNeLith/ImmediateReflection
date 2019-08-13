#if !SUPPORTS_STRING_FULL_FEATURES
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Helper to replace string standard utilities when not available in target version.
    /// </summary>
    internal static class StringUtils
    {
        /// <summary>
        /// Concatenates collection members using the specified <paramref name="separator"/> between each member.
        /// </summary>
        /// <param name="separator">String to use as separator. It is included only if <paramref name="values"/> contains multiple values.</param>
        /// <param name="values">Enumerable of values to concatenate.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>String composed of elements from <paramref name="values"/> separated by <paramref name="separator"/>.</returns>
        [Pure]
        [NotNull]
        public static string Join<T>([NotNull] string separator, [NotNull, ItemNotNull] IEnumerable<T> values)
        {
            Debug.Assert(separator != null);
            Debug.Assert(values != null);

            bool firstValue = true;
            var stringBuilder = new StringBuilder();
            foreach (T value in values)
            {
                if (!firstValue)
                    stringBuilder.Append(separator);
                firstValue = false;

                stringBuilder.Append(value);
            }

            return stringBuilder.ToString();
        }
    }
}
#endif