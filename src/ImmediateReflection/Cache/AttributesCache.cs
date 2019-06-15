using System;
using System.Collections.Generic;
#if SUPPORTS_SYSTEM_CORE
using System.Linq;
#else
using static ImmediateReflection.Utils.EnumerableUtils;
#endif
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
    internal class AttributesCache
    {
        [NotNull]
        private readonly Dictionary<Type, List<Attribute>> _attributesWithInherited;

        [NotNull]
        private readonly Dictionary<Type, List<Attribute>> _attributesWithoutInherited;

        public AttributesCache([NotNull] MemberInfo member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            Attribute[] attributesNotInherited = Attribute.GetCustomAttributes(member, false);
            _attributesWithoutInherited = new Dictionary<Type, List<Attribute>>(attributesNotInherited.Length);
            FillAttributesDictionary(_attributesWithoutInherited, attributesNotInherited);

            Attribute[] attributesInherited = Attribute.GetCustomAttributes(member, true);
            _attributesWithInherited = new Dictionary<Type, List<Attribute>>(attributesInherited.Length);
            FillAttributesDictionary(_attributesWithInherited, attributesInherited);

            #region Local function

            void FillAttributesDictionary(Dictionary<Type, List<Attribute>> attributesDictionary, Attribute[] attributes)
            {
                foreach (Attribute attribute in attributes)
                {
                    Type attributeType = attribute.GetType();
                    if (attributesDictionary.TryGetValue(attributeType, out List<Attribute> currentAttributes))
                        currentAttributes.Add(attribute);
                    else
                        attributesDictionary[attributeType] = new List<Attribute> { attribute };
                }
            }

            #endregion
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
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

            TAttribute FindAttribute(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(typeof(TAttribute), out List<Attribute> attributes))
                    return (TAttribute)attributes[0];
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
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

            Attribute FindAttribute(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(attributeType, out List<Attribute> attributes))
                    return attributes[0];
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

            IEnumerable<TAttribute> FindAttributes(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(typeof(TAttribute), out List<Attribute> attributes))
#if SUPPORTS_SYSTEM_CORE
                    return attributes.OfType<TAttribute>();
                return Enumerable.Empty<TAttribute>();
#else
                    return OfType<TAttribute>(attributes);
                return Empty<TAttribute>();
#endif
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
            if (inherit)
                return FindAttributes(_attributesWithInherited);
            return FindAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<Attribute> FindAttributes(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(attributeType, out List<Attribute> attributes))
#if SUPPORTS_SYSTEM_CORE
                    return attributes.AsEnumerable();
                return Enumerable.Empty<Attribute>();
#else
                    return AsEnumerable(attributes);
                return Empty<Attribute>();
#endif
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

            IEnumerable<Attribute> GetAllAttributes(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                foreach (List<Attribute> attributes in attributesDictionary.Values)
                {
                    foreach (Attribute attribute in attributes)
                        yield return attribute;
                }
            }

            #endregion
        }
    }
}