using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents information about the attributes of a member or <see cref="Type"/> and provides access to its metadata in a faster way.
    /// </summary>
    public abstract class ImmediateMember
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        [NotNull]
        public string Name { get; }

        [NotNull]
        private readonly AttributesCache _attributeCache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="member"/> is null.</exception>
        protected ImmediateMember([NotNull] MemberInfo member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            Name = member.Name;

            // Attributes
            _attributeCache = new AttributesCache(member);
        }

        /// <summary>
        /// Check if there is a custom attribute of type <typeparamref name="TAttribute"/> that is applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool HasAttribute<TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            return _attributeCache.HasAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Check if there is a custom attribute of type <paramref name="attributeType"/> that is applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool HasAttribute([NotNull] Type attributeType, bool inherit = false)
        {
            return _attributeCache.HasAttribute(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/> that is applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        [Pure]
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TAttribute GetAttribute<TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            return _attributeCache.GetAttribute<TAttribute>(inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of type <paramref name="attributeType"/> that is applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public Attribute GetAttribute([NotNull] Type attributeType, bool inherit = false)
        {
            return _attributeCache.GetAttribute(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves custom attributes of type <typeparamref name="TAttribute"/> that are applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            return _attributeCache.GetAttributes<TAttribute>(inherit);
        }

        /// <summary>
        /// Retrieves custom attributes of type <paramref name="attributeType"/> that are applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<Attribute> GetAttributes([NotNull] Type attributeType, bool inherit = false)
        {
            return _attributeCache.GetAttributes(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves all custom attributes that are applied to this member.
        /// </summary>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>All attributes.</returns>
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<Attribute> GetAllAttributes(bool inherit = false)
        {
            return _attributeCache.GetAllAttributes(inherit);
        }
    }
}