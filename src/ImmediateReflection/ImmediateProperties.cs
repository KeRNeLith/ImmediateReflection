using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_SYSTEM_CORE
using System.Linq;
#endif
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
#if SUPPORTS_SERIALIZATION
using System.Runtime.Serialization;
#endif
using JetBrains.Annotations;
#if !SUPPORTS_SYSTEM_CORE
using static ImmediateReflection.Utils.EnumerableUtils;
#endif
using static ImmediateReflection.Utils.ReflectionHelpers;
#if !SUPPORTS_STRING_FULL_FEATURES
using static ImmediateReflection.Utils.StringUtils;
#endif

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a collection of properties and provides access to property metadata in a faster way.
    /// </summary>
    [PublicAPI]
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class ImmediateProperties
        : IEnumerable<ImmediateProperty>
        , IEquatable<ImmediateProperties>
#if SUPPORTS_SERIALIZATION
        , ISerializable
#endif
    {
        [NotNull]
        private readonly Dictionary<string, ImmediateProperty> _properties 
            = new Dictionary<string, ImmediateProperty>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="properties">Enumerable of <see cref="PropertyInfo"/> to wrap.</param>
        internal ImmediateProperties([NotNull, ItemNotNull] IEnumerable<PropertyInfo> properties)
        {
            Init(properties);
        }

        private void Init([NotNull, ItemNotNull] IEnumerable<PropertyInfo> properties)
        {
            Debug.Assert(properties != null);

#if SUPPORTS_SYSTEM_CORE
            foreach (PropertyInfo property in properties.Where(IsNotIndexed))
#else
            foreach (PropertyInfo property in Where(properties, IsNotIndexed))
#endif
            {
                _properties.Add(
                    property.Name,
#if SUPPORTS_CACHING
                    CachesHandler.Instance.GetProperty(property));
#else
                    new ImmediateProperty(property));
#endif
            }

            #region Local function

            bool IsNotIndexed(PropertyInfo property)
            {
                Debug.Assert(property != null);

                return !IsIndexed(property);
            }

            #endregion
        }

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
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
        /// <exception cref="ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
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

#if SUPPORTS_SYSTEM_CORE
            return !_properties.Except(other._properties).Any();
#else
            return !Except(_properties, other._properties)
                .GetEnumerator()
                .MoveNext();
#endif
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

#if SUPPORTS_SERIALIZATION
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
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Count", _properties.Count);

            int i = -1;
#if SUPPORTS_SYSTEM_CORE
            foreach (PropertyInfo property in _properties.Select(pair => pair.Value.PropertyInfo))
#else
            foreach (PropertyInfo property in Select(_properties, pair => pair.Value.PropertyInfo))
#endif
            {
                info.AddValue($"Property{++i}", property);
            }
        }

        #endregion
#endif

        /// <inheritdoc />
        public override string ToString()
        {
#if SUPPORTS_STRING_FULL_FEATURES
            return $"[{string.Join(", ", _properties.Values)}]";
#else
            return $"[{Join(", ", _properties.Values)}]";
#endif
        }
    }
}