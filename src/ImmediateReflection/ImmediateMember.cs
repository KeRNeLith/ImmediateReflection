using System;
using System.Collections.Generic;
#if SUPPORTS_LINQ
using System.Linq;
#else
using static ImmediateReflection.Utils.EnumerableUtils;
#endif
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a member of a <see cref="Type"/> and provides access to its metadata in a faster way.
    /// </summary>
    public abstract class ImmediateMember
    {
        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        [NotNull]
        public string Name { get; }

        [NotNull]
        private readonly Dictionary<Type, List<Attribute>> _attributesWithInherited;

        [NotNull]
        private readonly Dictionary<Type, List<Attribute>> _attributesWithoutInherited;

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
#if SUPPORTS_LINQ
            Attribute[] attributesNotInherited = member.GetCustomAttributes(false).OfType<Attribute>().ToArray();
            Attribute[] attributesInherited = member.GetCustomAttributes(true).OfType<Attribute>().ToArray();
#else
            Attribute[] attributesNotInherited = ToArray(OfType<Attribute>(member.GetCustomAttributes(false)));
            Attribute[] attributesInherited = ToArray(OfType<Attribute>(member.GetCustomAttributes(true)));
#endif

            _attributesWithoutInherited = new Dictionary<Type, List<Attribute>>(attributesNotInherited.Length);
            FillAttributesDictionary(_attributesWithoutInherited, attributesNotInherited);

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
        /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/> that is applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        [Pure]
        [CanBeNull]
        public TAttribute GetAttribute<TAttribute>(bool inherit = false)
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
        /// Retrieves a custom attribute of type <paramref name="attributeType"/> that is applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>The first attribute matching requested type, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="attributeType"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given <paramref name="attributeType"/> is not an <see cref="Attribute"/> type.</exception>
        [Pure]
        [CanBeNull]
        public Attribute GetAttribute([NotNull] Type attributeType, bool inherit = false)
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
        /// Retrieves custom attributes of type <typeparamref name="TAttribute"/> that are applied to this member.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type.</typeparam>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [CanBeNull]
        public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            if (inherit)
                return FindAttributes(_attributesWithInherited);
            return FindAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<TAttribute> FindAttributes(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(typeof(TAttribute), out List<Attribute> attributes))
#if SUPPORTS_LINQ
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
        /// Retrieves custom attributes of type <paramref name="attributeType"/> that are applied to this member.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to search.</param>
        /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
        /// <returns>Attributes matching requested type.</returns>
        [Pure]
        [CanBeNull]
        public IEnumerable<Attribute> GetAttributes([NotNull] Type attributeType, bool inherit = false)
        {
            if (inherit)
                return FindAttributes(_attributesWithInherited);
            return FindAttributes(_attributesWithoutInherited);

            #region Local function

            IEnumerable<Attribute> FindAttributes(Dictionary<Type, List<Attribute>> attributesDictionary)
            {
                if (attributesDictionary.TryGetValue(attributeType, out List<Attribute> attributes))
#if SUPPORTS_LINQ
                    return attributes.AsEnumerable();
                return Enumerable.Empty<Attribute>();
#else
                    return AsEnumerable(attributes);
                return Empty<Attribute>();
#endif
            }

            #endregion
        }
    }
}