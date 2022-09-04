using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;
using static ImmediateReflection.Utils.ReflectionHelpers;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a property and provides access to property metadata in a faster way.
    /// </summary>
    [PublicAPI]
    [Serializable]
    public sealed class ImmediateProperty
        : ImmediateMember
        , IEquatable<ImmediateProperty>
        , ISerializable
    {
        /// <summary>
        /// Gets the wrapped <see cref="T:System.Reflection.PropertyInfo"/>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> owning this property (declaring it).
        /// </summary>
        [PublicAPI]
        [NotNull]
        public Type DeclaringType { get; }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> of this property.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public Type PropertyType { get; }

        [NotNull]
        private readonly Lazy<ImmediateType> _propertyImmediateType;

        /// <summary>
        /// Gets the <see cref="ImmediateType"/> of this property.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public ImmediateType PropertyImmediateType => _propertyImmediateType.Value;

        /// <summary>
        /// Gets the readable state of this property.
        /// </summary>
        [PublicAPI]
        public bool CanRead { get; }

        [NotNull]
        private readonly GetterDelegate _getter;

        /// <summary>
        /// Gets the writable state of this property.
        /// </summary>
        [PublicAPI]
        public bool CanWrite { get; }

        [NotNull]
        private readonly SetterDelegate _setter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="property"><see cref="T:System.Reflection.PropertyInfo"/> to wrap.</param>
        internal ImmediateProperty([NotNull] PropertyInfo property)
            : base(property)
        {
            Debug.Assert(!IsIndexed(property), $"Cannot initialize an {nameof(ImmediateProperty)} with an indexed property.");

            // General property info
            PropertyInfo = property;
            PropertyType = property.PropertyType;
            _propertyImmediateType = new Lazy<ImmediateType>(() => TypeAccessor.Get(PropertyType));

            // ReSharper disable once AssignNullToNotNullAttribute, Justification: A property is always declared inside a type.
            DeclaringType = property.DeclaringType;

            CanRead = property.CanRead;
            CanWrite = property.CanWrite;

            // Getter / Setter
            _getter = ConfigureGetter();
            _setter = ConfigureSetter();

            #region Local functions

            GetterDelegate ConfigureGetter()
            {
                GetterDelegate getter = null;
                MethodInfo getMethod = property.GetGetMethod(true);
                if (getMethod != null)
                    getter = DelegatesFactory.CreateGetter(property, getMethod);

                if (getter is null)
                    return target => throw new ArgumentException($"No getter for property {Name}.");

                return getter;
            }

            SetterDelegate ConfigureSetter()
            {
                SetterDelegate setter = null;
                MethodInfo setMethod = property.GetSetMethod(true);
                if (setMethod != null)
                    setter = DelegatesFactory.CreateSetter(property, setMethod);

                if (setter is null)
                    return (target, value) => throw new ArgumentException($"No setter for property {Name}.");

                return setter;
            }

            #endregion
        }

        /// <summary>
        /// Returns the property value of the specified object.
        /// </summary>
        /// <param name="obj">Object that property value will be returned.</param>
        /// <returns>Property value of the specified object.</returns>
        /// <exception cref="T:System.ArgumentException">If this property has no getter.</exception>
        /// <exception cref="T:System.InvalidCastException">If the <paramref name="obj"/> is not the owner of this property.</exception>
        /// <exception cref="T:System.Reflection.TargetException">If the given <paramref name="obj"/> is null and the property to get is not static.</exception>
        [PublicAPI]
        [Pure]
        public object GetValue([CanBeNull] object obj)
        {
            return _getter(obj);
        }

        /// <summary>
        /// Sets the property value of the specified object.
        /// </summary>
        /// <param name="obj">Object that property value will be set.</param>
        /// <param name="value">New property value.</param>
        /// <exception cref="T:System.ArgumentException">If this property has no setter.</exception>
        /// <exception cref="T:System.InvalidCastException">If the <paramref name="obj"/> is not the owner of this property or if the <paramref name="value"/> is of the wrong type.</exception>
        /// <exception cref="T:System.Reflection.TargetException">If the given <paramref name="obj"/> is null and the property to set is not static.</exception>
        [PublicAPI]
        public void SetValue([CanBeNull] object obj, [CanBeNull] object value)
        {
            _setter(obj, value);
        }

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateProperty);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateProperty other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return PropertyInfo == other.PropertyInfo;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PropertyInfo.GetHashCode();
        }

        #endregion

        #region ISerializable

        private ImmediateProperty(SerializationInfo info, StreamingContext context)
            : this((PropertyInfo)info.GetValue("Property", typeof(PropertyInfo)))
        {
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Property", PropertyInfo);
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return PropertyInfo.ToString();
        }
    }
}