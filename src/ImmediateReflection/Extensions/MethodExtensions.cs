using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="MethodInfo"/>.
    /// </summary>
    [PublicAPI]
    public static class MethodExtensions
    {
        /// <summary>
        /// Empty parameters.
        /// </summary>
        public static readonly object[] EmptyArgs = { };

        /// <summary>
        /// Creates a delegate method to call the target <paramref name="method"/> in a faster way.
        /// </summary>
        /// <param name="method">Method for which creating a delegate.</param>
        /// <returns>The corresponding <see cref="MethodDelegate"/> delegate.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="method"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("method:null => halt")]
        public static MethodDelegate CreateMethodDelegate(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MethodInfo method
#else
            [NotNull] MethodInfo method
#endif
            )
        {
            return method.Invoke;
        }
    }
}