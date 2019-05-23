#if SUPPORTS_EXTENSIONS
using System;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to easily work with <see cref="ImmediateProperty"/>.
    /// </summary>
    public static class ImmediateMemberExtensions
    {
        /// <summary>
        /// Searches for the public <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>The corresponding <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="fieldName"/> is null.</exception>
#if !SUPPORTS_SYSTEM_CACHING && !SUPPORTS_MICROSOFT_CACHING
        [Pure]
#endif
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateField GetImmediateField([NotNull] this Type type, [NotNull] string fieldName)
        {
            return TypeAccessor.Get(type).GetField(fieldName);
        }

        // TODO Add with flags

        /// <summary>
        /// Searches for the public <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>The corresponding <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="propertyName"/> is null.</exception>
#if !SUPPORTS_SYSTEM_CACHING && !SUPPORTS_MICROSOFT_CACHING
        [Pure]
#endif
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateProperty GetImmediateProperty([NotNull] this Type type, [NotNull] string propertyName)
        {
            return TypeAccessor.Get(type).GetProperty(propertyName);
        }

        // TODO Add with flags
    }
}
#endif