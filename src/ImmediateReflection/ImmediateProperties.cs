using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a collection of properties and provides access to property metadata in a faster way.
    /// </summary>
    public sealed class ImmediateProperties : IEnumerable<ImmediateProperty>, IEquatable<ImmediateProperties>
    {
        [NotNull]
        private readonly Dictionary<string, ImmediateProperty> _properties;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="properties">Enumerable of <see cref="PropertyInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="properties"/> enumerable is null,
        /// or if it contains a null <see cref="PropertyInfo"/>.</exception>
        internal ImmediateProperties([NotNull, ItemNotNull] IEnumerable<PropertyInfo> properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            _properties = properties.ToDictionary(
                property => property?.Name ?? throw new ArgumentNullException(nameof(property), "A property is null."), 
                property => new ImmediateProperty(property));
        }

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [CanBeNull]
        public ImmediateProperty this[[NotNull] string propertyName] =>
            _properties.TryGetValue(propertyName, out ImmediateProperty property) 
                ? property 
                : null;

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateProperties);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateProperties other)
        {
            if (other is null)
                return false;
            return _properties.Count == other._properties.Count
                && !_properties.Except(other._properties).Any();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _properties
                .Aggregate(
                    0, 
                    (hashCode, pair) =>
                    {
                        hashCode = (hashCode * 397) ^ pair.Key.GetHashCode();
                        hashCode = (hashCode * 397) ^ pair.Value.GetHashCode();
                        return hashCode;
                    });
        }

        #endregion

        #region IEnumerable

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<T>

        /// <inheritdoc />
        public IEnumerator<ImmediateProperty> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{string.Join(", ", _properties.Values)}]";
        }
    }
}