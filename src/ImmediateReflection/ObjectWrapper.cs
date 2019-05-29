using System;
using System.Collections.Generic;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Provides a wrapper over an <see cref="object"/> that gives access to its Reflection features in a faster way than standard stuff.
    /// </summary>
    [PublicAPI]
    public sealed class ObjectWrapper : IEquatable<ObjectWrapper>
    {
        /// <summary>
        /// Gets the wrapped object.
        /// </summary>
        [PublicAPI]
        public object Object { get; }

        /// <summary>
        /// Gets the wrapped object <see cref="System.Type"/>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public Type Type { get; }

        /// <summary>
        /// Gets the wrapped object corresponding <see cref="ImmediateType"/>.
        /// </summary>
        [PublicAPI]
        [NotNull]
        public ImmediateType ImmediateType { get; }

        /// <summary>
        /// Gets all the members of this <see cref="Object"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public IEnumerable<ImmediateMember> Members => ImmediateType.Members;

        /// <summary>
        /// Gets all the fields of this <see cref="Object"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public ImmediateFields Fields => ImmediateType.Fields;

        /// <summary>
        /// Gets all the properties of this <see cref="Object"/>.
        /// </summary>
        [PublicAPI]
        [NotNull, ItemNotNull]
        public ImmediateProperties Properties => ImmediateType.Properties;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="obj">Object to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="obj"/> is null.</exception>
        public ObjectWrapper([NotNull] object obj)
        {
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
            Type = obj.GetType();
            ImmediateType = TypeAccessor.Get(Type);
        }

        #region Members

        /// <summary>
        /// Gets all the members of this <see cref="System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateMember"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateMember> GetMembers() => ImmediateType.Members;

        /// <summary>
        /// Gets the <see cref="ImmediateMember"/> corresponding to the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">Member name.</param>
        /// <returns>Found <see cref="ImmediateMember"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
        [PublicAPI]
        [CanBeNull]
        public ImmediateMember this[[NotNull] string memberName] => ImmediateType[memberName];

        /// <summary>
        /// Gets the <see cref="ImmediateMember"/> corresponding to the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">Member name.</param>
        /// <returns>Found <see cref="ImmediateMember"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="memberName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("memberName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateMember GetMember([NotNull] string memberName) => ImmediateType[memberName];

        #endregion

        #region Fields

        /// <summary>
        /// Gets all the fields of this <see cref="Object"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateField"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateField> GetFields() => ImmediateType.Fields;

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateField GetField([NotNull] string fieldName) => ImmediateType.Fields[fieldName];

        /// <summary>
        /// Returns the field value of the wrapped <see cref="Object"/>.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>
        /// Field value of the <see cref="Object"/>.
        /// It can also returns null if there is no field corresponding to <paramref name="fieldName"/> in the object.
        /// </returns>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public object GetFieldValue([NotNull] string fieldName)
        {
            return GetField(fieldName)?.GetValue(Object);
        }

        /// <summary>
        /// Sets the field value of the wrapped <see cref="Object"/>.
        /// </summary>
        /// <remarks>It will set nothing if there is no field corresponding to <paramref name="fieldName"/> in the object.</remarks>
        /// <param name="fieldName">Field name.</param>
        /// <param name="value">New field value.</param>
        /// <exception cref="InvalidCastException">If the <paramref name="value"/> is of the wrong type.</exception>
        [PublicAPI]
        [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void SetFieldValue([NotNull] string fieldName, [CanBeNull] object value)
        {
            GetField(fieldName)?.SetValue(Object, value);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all the properties of this <see cref="Object"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateProperty"/>.</returns>
        [PublicAPI]
        [Pure]
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateProperty> GetProperties() => ImmediateType.Properties;

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
        public ImmediateProperty GetProperty([NotNull] string propertyName) => ImmediateType.Properties[propertyName];

        /// <summary>
        /// Returns the property value of the wrapped <see cref="Object"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>
        /// Property value of the <see cref="Object"/>.
        /// It can also returns null if there is no property corresponding to <paramref name="propertyName"/> in the object.
        /// </returns>
        /// <exception cref="ArgumentException">If this property has no getter.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public object GetPropertyValue([NotNull] string propertyName)
        {
            return GetProperty(propertyName)?.GetValue(Object);
        }

        /// <summary>
        /// Sets the property value of the wrapped <see cref="Object"/>.
        /// </summary>
        /// <remarks>It will set nothing if there is no property corresponding to <paramref name="propertyName"/> in the object.</remarks>
        /// <param name="propertyName">Property name.</param>
        /// <param name="value">New property value.</param>
        /// <exception cref="ArgumentException">If this property has no setter.</exception>
        /// <exception cref="InvalidCastException">If the <paramref name="value"/> is of the wrong type.</exception>
        [PublicAPI]
        [ContractAnnotation("propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void SetPropertyValue([NotNull] string propertyName, [CanBeNull] object value)
        {
            GetProperty(propertyName)?.SetValue(Object, value);
        }

        #endregion

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectWrapper);
        }

        /// <inheritdoc />
        public bool Equals(ObjectWrapper other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Object.Equals(other.Object);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Object.GetHashCode();
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return Object.ToString();
        }
    }
}