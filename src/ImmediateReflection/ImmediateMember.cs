using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents information about the attributes of a member or <see cref="Type"/> and provides access to its metadata in a faster way.
    /// </summary>
    [PublicAPI]
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public abstract class ImmediateMember
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public string Name { get; }

        [NotNull]
        private readonly AttributesCache _attributeCache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="member"><see cref="MemberInfo"/> to wrap.</param>
        protected ImmediateMember([NotNull] MemberInfo member)
        {
            Debug.Assert(member != null);

            Name = member.Name;

            // Attributes
#if SUPPORTS_CACHING
            _attributeCache = CachesHandler.Instance.GetAttributesCache(member);
#else
            _attributeCache = new AttributesCache(member);
#endif
        }

        /// <summary>
        /// Check if there is a custom attribute of type <typeparamref name="TAttribute"/> that is applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        [PublicAPI]
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsDefined<TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            return _attributeCache.IsDefined<TAttribute>(inherit);
        }

        /// <summary>
        /// Check if there is a custom attribute of type <paramref name="attributeType"/> that is applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("attributeType:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsDefined([NotNull] Type attributeType, bool inherit = false)
        {
            return _attributeCache.IsDefined(attributeType, inherit);
        }

        /// <summary>
        /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/> that is applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        [PublicAPI]
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
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("attributeType:null => halt")]
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
        [PublicAPI]
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
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
        [ContractAnnotation("attributeType:null => halt")]
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
        [PublicAPI]
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