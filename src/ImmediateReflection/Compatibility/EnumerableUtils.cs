#if !SUPPORTS_LINQ
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Helper to replace Enumerable standard utilities when not available in target version.
    /// </summary>
    internal static class EnumerableUtils
    {
        /// <summary>
        /// Gets an enumerable of <paramref name="first"/>values excepting those in <paramref name="second"/>.
        /// </summary>
        /// <param name="first">Enumerable of values.</param>
        /// <param name="second">Enumerable of values to except.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>Source element that were not present in <paramref name="second"/>.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public static IEnumerable<T> Except<T>([NotNull, ItemNotNull] IEnumerable<T> first, [NotNull, ItemNotNull] IEnumerable<T> second)
        {
            var list = new List<T>(second);
            foreach (T source in first)
            {
                if (!list.Contains(source))
                    yield return source;
            }
        }
    }
}
#endif