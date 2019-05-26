#if SUPPORTS_EXTENSIONS && SUPPORTS_CACHING
using System;
using System.Collections.Generic;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to easily work with attributes.
    /// </summary>
    public static class ImmediateAttributesExtensions
    {
        /// <summary>
        /// Check if there is a custom attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDefinedImmediateAttribute<TAttribute>([NotNull] this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .IsDefined<TAttribute>(inherit);
        }

        /// <summary>
        /// Check if there is a custom attribute of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDefinedImmediateAttribute([NotNull] this MemberInfo member, [NotNull] Type attributeType, bool inherit = false)
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .IsDefined(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TAttribute GetImmediateAttribute<TAttribute>([NotNull] this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="member"/> or the <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Attribute GetImmediateAttribute([NotNull] this MemberInfo member, [NotNull] Type attributeType, bool inherit = false)
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAttribute(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves custom attributes of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<TAttribute> GetImmediateAttributes<TAttribute>([NotNull] this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAttributes<TAttribute>(inherit);
        }

        /// <summary>
        /// Retrieves custom attributes of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<Attribute> GetImmediateAttributes([NotNull] this MemberInfo member, [NotNull] Type attributeType, bool inherit = false)
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAttributes(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves all custom attributes.
        /// </summary>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>All attributes.</returns>
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<Attribute> GetAllImmediateAttributes([NotNull] this MemberInfo member, bool inherit = false)
        {
            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAllAttributes(inherit);
        }
    }
}
#endif