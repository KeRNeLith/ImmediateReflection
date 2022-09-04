using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;
using static ImmediateReflection.Utils.ReflectionHelpers;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a collection of properties and provides access to property metadata in a faster way.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class ImmediateProperties
        : IEnumerable<ImmediateProperty>
        , IEquatable<ImmediateProperties>
        , ISerializable
    {
        [NotNull]
        private readonly Dictionary<string, ImmediateProperty> _properties 
            = new Dictionary<string, ImmediateProperty>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="properties">Enumerable of <see cref="T:System.Reflection.PropertyInfo"/> to wrap.</param>
        internal ImmediateProperties([NotNull, ItemNotNull] IEnumerable<PropertyInfo> properties)
        {
            Init(properties);
        }

        private void Init([NotNull, ItemNotNull] IEnumerable<PropertyInfo> properties)
        {
            Debug.Assert(properties != null);

            foreach (PropertyInfo property in properties.Where(IsNotIndexed))
            {
                ImmediateProperty currentImmediateProperty = CachesHandler.Instance.GetProperty(property);

                if (_properties.TryGetValue(property.Name, out ImmediateProperty immediateProperty))
                {
                    // Keep the property from the most derived type
                    if (ShouldReplacePropertyWith(immediateProperty, currentImmediateProperty))
                    {
                        _properties[property.Name] = currentImmediateProperty;
                    }
                }
                else
                {
                    _properties.Add(property.Name, currentImmediateProperty);
                }
            }

            #region Local functions

            bool IsNotIndexed(PropertyInfo property)
            {
                Debug.Assert(property != null);

                return !IsIndexed(property);
            }

            bool ShouldReplacePropertyWith(ImmediateProperty property1, ImmediateProperty property2)
            {
                Debug.Assert(property1 != null);
                Debug.Assert(property2 != null);

                Type initialType = property1.DeclaringType;
                Type currentType = property2.DeclaringType;
                while (currentType != null)
                {
                    // If property2 is a property of a derived object of property1 declaring type
                    // => Prefer derived property
                    if (initialType == currentType)
                        return true;

                    currentType = currentType.BaseType;
                }

                return false;
            }

            #endregion
        }

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
        [CanBeNull]
        public ImmediateProperty this[[NotNull] string propertyName] =>
            _properties.TryGetValue(propertyName, out ImmediateProperty property)
                ? property
                : null;

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateProperty GetProperty([NotNull] string propertyName) => this[propertyName];

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
            if (ReferenceEquals(this, other))
                return true;
            if (_properties.Count != other._properties.Count)
                return false;
            return !_properties.Except(other._properties).Any();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (KeyValuePair<string, ImmediateProperty> pair in _properties)
            {
                hashCode = (hashCode * 397) ^ pair.Key.GetHashCode();
                hashCode = (hashCode * 397) ^ pair.Value.GetHashCode();
            }

            return hashCode;
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

        #region ISerializable

        private ImmediateProperties(SerializationInfo info, StreamingContext context)
        {
            Init(ExtractProperties());

            #region Local function

            IEnumerable<PropertyInfo> ExtractProperties()
            {
                int count = (int)info.GetValue("Count", typeof(int));
                for (int i = 0; i < count; ++i)
                {
                    yield return (PropertyInfo)info.GetValue($"Property{i}", typeof(PropertyInfo));
                }
            }

            #endregion
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Count", _properties.Count);

            int i = -1;
            foreach (PropertyInfo property in _properties.Select(pair => pair.Value.PropertyInfo))
            {
                info.AddValue($"Property{++i}", property);
            }
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{string.Join(", ", _properties.Values)}]";
        }
    }
}