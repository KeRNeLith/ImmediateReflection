using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a property and provides access to property metadata in a faster way.
    /// </summary>
    public sealed class ImmediateProperty : IEquatable<ImmediateProperty>
    {
        /// <summary>
        /// Gets the wrapped <see cref="System.Reflection.PropertyInfo"/>.
        /// </summary>
        [NotNull]
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets the <see cref="System.Reflection.PropertyInfo"/> name.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of this property.
        /// </summary>
        [NotNull]
        public Type PropertyType { get; }

        /// <summary>
        /// Gets the readable state of this property.
        /// </summary>
        public bool CanRead { get; }

        private readonly GetterDelegate _getter;

        /// <summary>
        /// Gets the writable state of this property.
        /// </summary>
        public bool CanWrite { get; }

        private readonly SetterDelegate _setter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="property"><see cref="System.Reflection.PropertyInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="property"/> is null.</exception>
        internal ImmediateProperty([NotNull] PropertyInfo property)
        {
            // General property info
            PropertyInfo = property ?? throw new ArgumentNullException(nameof(property));
            Name = property.Name;
            PropertyType = property.PropertyType;
            CanRead = property.CanRead;
            CanWrite = property.CanWrite;

            // Getter
            MethodInfo getMethod = property.GetGetMethod(true);
            if (getMethod != null)
            {
                _getter = DelegatesFactory.CreateGetter(property, getMethod);
            }

            if (_getter is null)
                _getter = target => throw new ArgumentException($"No getter for property {Name}.");

            // Setter
            MethodInfo setMethod = property.GetSetMethod(true);
            if (setMethod != null)
            {
                _setter = DelegatesFactory.CreateSetter(property, setMethod);
            }

            if (_setter is null)
                _setter = (target, value) => throw new ArgumentException($"No setter for property {Name}.");
        }

        /// <summary>
        /// Returns the property value of the specified object.
        /// </summary>
        /// <param name="obj">Object that property value will be returned.</param>
        /// <returns>Property value of the specified object.</returns>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null and the property to get is not static.</exception>
        [Pure]
        public object GetValue([CanBeNull] object obj)
        {
            if (obj is null)
                throw new TargetException();
            return _getter(obj);
        }

        /// <summary>
        /// Sets the property value of the specified object.
        /// </summary>
        /// <param name="obj">Object that property value will be set.</param>
        /// <param name="value">New property value.</param>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null and the property to set is not static.</exception>
        public void SetValue([CanBeNull] object obj, [CanBeNull] object value)
        {
            if (obj is null)
                throw new TargetException();
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

        /// <inheritdoc />
        public override string ToString()
        {
            return PropertyInfo.ToString();
        }
    }
}