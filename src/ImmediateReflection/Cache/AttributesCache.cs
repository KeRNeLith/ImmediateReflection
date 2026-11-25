using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;

namespace ImmediateReflection;

/// <summary>
/// Cache storage for attributes.
/// </summary>
internal sealed class AttributesCache
{
    private readonly Attribute[] _attributesWithInherited;
    private readonly Attribute[] _attributesWithoutInherited;

    // Read only views exposed to callers (arrays are never exposed), created once to avoid allocating on each call
    private readonly ReadOnlyCollection<Attribute> _allAttributesWithInherited;
    private readonly ReadOnlyCollection<Attribute> _allAttributesWithoutInherited;

    public AttributesCache(MemberInfo member)
    {
        AssertNotNull(member);

        _attributesWithoutInherited = Attribute.GetCustomAttributes(member, false);
        _attributesWithInherited = Attribute.GetCustomAttributes(member, true);

        _allAttributesWithoutInherited = Array.AsReadOnly(_attributesWithoutInherited);
        _allAttributesWithInherited = Array.AsReadOnly(_attributesWithInherited);
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
        return GetAttribute<TAttribute>(inherit) is not null;
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
    public bool IsDefined(Type attributeType, bool inherit)
    {
        return GetAttribute(attributeType, inherit) is not null;
    }

    /// <summary>
    /// Retrieves a custom attribute of type <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TAttribute">Attribute type.</typeparam>
    /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
    /// <returns>The first attribute matching requested type, otherwise null.</returns>
    [Pure]
    public TAttribute? GetAttribute<TAttribute>(bool inherit)
        where TAttribute : Attribute
    {
        if (inherit)
            return FindAttribute(_attributesWithInherited);
        return FindAttribute(_attributesWithoutInherited);

        #region Local function

        static TAttribute? FindAttribute(Attribute[] attributes)
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
    [ContractAnnotation("attributeType:null => halt")]
    public Attribute? GetAttribute(Type attributeType, bool inherit)
    {
        if (attributeType is null)
            throw new ArgumentNullException(nameof(attributeType));
        if (!typeof(Attribute).IsAssignableFrom(attributeType))
            throw new ArgumentException($"{nameof(attributeType)} must be an {nameof(Attribute)} type.");

        if (inherit)
            return FindAttribute(_attributesWithInherited, attributeType);
        return FindAttribute(_attributesWithoutInherited, attributeType);

        #region Local function

        static Attribute? FindAttribute(Attribute[] attributes, Type type)
        {
            foreach (Attribute attribute in attributes)
            {
                if (type.IsInstanceOfType(attribute))
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
    public IEnumerable<TAttribute> GetAttributes<TAttribute>(bool inherit)
        where TAttribute : Attribute
    {
        if (inherit)
            return FindAttributes(_attributesWithInherited);
        return FindAttributes(_attributesWithoutInherited);

        #region Local function

        static IEnumerable<TAttribute> FindAttributes(Attribute[] attributes)
        {
            int count = 0;
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TAttribute)
                {
                    ++count;
                }
            }

            if (count == 0)
                return Enumerable.Empty<TAttribute>();

            // Exact size array filled in a single pass, not shared with the cache
            var result = new TAttribute[count];
            int index = 0;
            foreach (Attribute attribute in attributes)
            {
                if (attribute is TAttribute attr)
                {
                    result[index++] = attr;
                }
            }

            return result;
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
    [ContractAnnotation("attributeType:null => halt")]
    public IEnumerable<Attribute> GetAttributes(Type attributeType, bool inherit)
    {
        if (attributeType is null)
            throw new ArgumentNullException(nameof(attributeType));
        if (!typeof(Attribute).IsAssignableFrom(attributeType))
            throw new ArgumentException($"{nameof(attributeType)} must be an {nameof(Attribute)} type.");

        if (inherit)
            return FindAttributes(_attributesWithInherited, attributeType);
        return FindAttributes(_attributesWithoutInherited, attributeType);

        #region Local function

        static IEnumerable<Attribute> FindAttributes(Attribute[] attributes, Type type)
        {
            int count = 0;
            foreach (Attribute attribute in attributes)
            {
                if (type.IsInstanceOfType(attribute))
                {
                    ++count;
                }
            }

            if (count == 0)
                return Enumerable.Empty<Attribute>();

            // Exact size array filled in a single pass, not shared with the cache
            var result = new Attribute[count];
            int index = 0;
            foreach (Attribute attribute in attributes)
            {
                if (type.IsInstanceOfType(attribute))
                {
                    result[index++] = attribute;
                }
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Retrieves all custom attributes.
    /// </summary>
    /// <param name="inherit">Indicates if inherited attributes should be taken into account.</param>
    /// <returns>All attributes.</returns>
    [Pure]
    public IEnumerable<Attribute> GetAllAttributes(bool inherit)
    {
        return inherit
            ? _allAttributesWithInherited
            : _allAttributesWithoutInherited;
    }
}