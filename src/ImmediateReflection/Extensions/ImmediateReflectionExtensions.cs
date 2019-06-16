using System;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to easily work with Immediate Reflection.
    /// </summary>
    [PublicAPI]
    public static class ImmediateReflectionExtensions
    {
        /// <summary>
        /// Gets the <see cref="ImmediateType"/> corresponding to this instance.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object instance.</param>
        /// <returns>The corresponding <see cref="ImmediateType"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="obj"/>is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull]
        [ContractAnnotation("obj:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType GetImmediateType<T>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this T obj
#else
            [NotNull] T obj
#endif
            )
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return TypeAccessor.Get(obj.GetType());
        }

#if SUPPORTS_EXTENSIONS
        /// <summary>
        /// Gets the <see cref="ImmediateType"/> corresponding to this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <returns>The corresponding <see cref="ImmediateType"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/>is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType GetImmediateType([NotNull] this Type type)
        {
            return TypeAccessor.Get(type);
        }
#endif
    }
}