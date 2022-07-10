using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Cache storage for attributes.
    /// </summary>
    internal sealed class AttributesCache
    {
        [NotNull, ItemNotNull]
        private readonly Attribute[] _attributesWithInherited;

        [NotNull, ItemNotNull]
        private readonly Attribute[] _attributesWithoutInherited;

        public AttributesCache([NotNull] MemberInfo member)
        {
            Debug.Assert(member != null);

            _attributesWithoutInherited = Attribute.GetCustomAttributes(member, false);
            _attributesWithInherited = Attribute.GetCustomAttributes(member, true);
        }

        /// <summary>
        /// Check if there is a custom attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        [Pure]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsDefined<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            return GetAttribute<TAttribute>(inherit) != null;
        }

        /// <summary>
        /// Check if there is a custom attribute of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>True if an attribute matches requested type, otherwise false.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
        [ContractAnnotation("attributeType:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsDefined([NotNull] Type attributeType, bool inherit)
        {
            return GetAttribute(attributeType, inherit) != null;
        }

        /// <summary>
        /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        [Pure]
        [CanBeNull]
        public TAttribute GetAttribute<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            if (inherit)
                return FindAttribute(_attributesWithInherited);
            return FindAttribute(_attributesWithoutInherited);

            #region Local function

            TAttribute FindAttribute(Attribute[] attributes)
            {
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is TAttribute attr)
                        return attr;
                }

                return null;
            }

            #endregion
        }

        /// <summary>
        /// Retrieves a custom attribute of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
        [CanBeNull]
        [ContractAnnotation("attributeType:null => halt")]
        public Attribute GetAttribute([NotNull] Type attributeType, bool inherit)
        {
            if (attributeType is null)
                throw new ArgumentNullException(nameof(attributeType));
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException($"{nameof(attributeType)} must be an {nameof(Attribute)} type.");

            if (inherit)
                return FindAttribute(_attributesWithInherited);
            return FindAttribute(_attributesWithoutInherited);

            #region Local function

            Attribute FindAttribute(Attribute[] attributes)
            {
                foreach (Attribute attribute in attributes)
                {
                    if (attributeType.IsInstanceOfType(attribute))
                        return attribute;
                }

                return null;
            }

            #endregion
        }

        /// <summary>
        /// Retrieves custom attributes of type <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool inherit)
            where TAttribute : Attribute
        {
            if (inherit)
                return FindAttributes(_attributesWithInherited);
            return FindAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<TAttribute> FindAttributes(Attribute[] attributes)
            {
                return attributes.OfType<TAttribute>();
            }

            #endregion
        }

        /// <summary>
        /// Retrieves custom attributes of type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        [ContractAnnotation("attributeType:null => halt")]
        public IEnumerable<Attribute> GetAttributes([NotNull] Type attributeType, bool inherit)
        {
            if (attributeType is null)
                throw new ArgumentNullException(nameof(attributeType));
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException($"{nameof(attributeType)} must be an {nameof(Attribute)} type.");

            if (inherit)
                return FindAttributes(_attributesWithInherited);
            return FindAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<Attribute> FindAttributes(Attribute[] attributes)
            {
                return attributes.Where(attributeType.IsInstanceOfType);
            }

            #endregion
        }

        /// <summary>
        /// Retrieves all custom attributes.
        /// </summary>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>All attributes.</returns>
        [Pure]
        [NotNull, ItemNotNull]
        public IEnumerable<Attribute> GetAllAttributes(bool inherit)
        {
            if (inherit)
                return GetAllAttributes(_attributesWithInherited);
            return GetAllAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<Attribute> GetAllAttributes(Attribute[] attributes)
            {
                foreach (Attribute attribute in attributes)
                {
                    yield return attribute;
                }
            }

            #endregion
        }
    }
}