#if SUPPORTS_CACHING
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
    [PublicAPI]
    public static class ImmediateAttributesExtensions
    {
        /// <summary>
        /// Check if there is a custom attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="member">Member to get its custom attributes.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDefinedImmediateAttribute<TAttribute>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif
            bool inherit = false)
            where TAttribute : Attribute
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="T:System.Attribute"/> type.</exception>
        [PublicAPI]
        [ContractAnnotation("member:null => halt;attributeType:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDefinedImmediateAttribute(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif
            [NotNull] Type attributeType,
            bool inherit = false)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [PublicAPI]
        [CanBeNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TAttribute GetImmediateAttribute<TAttribute>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif
            bool inherit = false)
            where TAttribute : Attribute
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> or the <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="T:System.Attribute"/> type.</exception>
        [PublicAPI]
        [CanBeNull]
        [ContractAnnotation("member:null => halt;attributeType:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static Attribute GetImmediateAttribute(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif 
            [NotNull] Type attributeType,
            bool inherit = false)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [PublicAPI]
        [NotNull, ItemNotNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<TAttribute> GetImmediateAttributes<TAttribute>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif
            bool inherit = false)
            where TAttribute : Attribute
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [PublicAPI]
        [NotNull, ItemNotNull]
        [ContractAnnotation("member:null => halt;attributeType:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<Attribute> GetImmediateAttributes(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
#endif
            [NotNull] Type attributeType,
            bool inherit = false)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

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
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="member"/> is null.</exception>
        [PublicAPI]
        [NotNull, ItemNotNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<Attribute> GetAllImmediateAttributes(
#if SUPPORTS_EXTENSIONS
            [NotNull] this MemberInfo member,
#else
            [NotNull] MemberInfo member,
# endif
            bool inherit = false)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            return CachesHandler.Instance
                .GetAttributesCache(member)
                .GetAllAttributes(inherit);
        }
    }
}
#endif