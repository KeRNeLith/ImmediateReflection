using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a field and provides access to its metadata in a faster way.
    /// </summary>
    public sealed class ImmediateField : ImmediateMember, IEquatable<ImmediateField>
    {
        /// <summary>
        /// Gets the wrapped <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        [NotNull]
        public FieldInfo FieldInfo { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of this field.
        /// </summary>
        [NotNull]
        public Type FieldType { get; }

        [NotNull]
        private readonly GetterDelegate _getter;

        [NotNull]
        private readonly SetterDelegate _setter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="field"><see cref="System.Reflection.FieldInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="field"/> is null.</exception>
        internal ImmediateField([NotNull] FieldInfo field)
            : base(field)
        {
            FieldInfo = field;
            FieldType = field.FieldType;

            // Getter / Setter
            _getter = ConfigureGetter();
            _setter = ConfigureSetter();

            #region Local functions

            bool IsConstantField()
            {
                return field.IsLiteral || field.IsInitOnly;
            }

            GetterDelegate ConfigureGetter()
            {
                if (IsConstantField() && field.IsStatic)
                {
                    object fieldValue = field.GetValue(null);
                    return target => fieldValue;
                }

                return DelegatesFactory.CreateGetter(field);
            }

            SetterDelegate ConfigureSetter()
            {
                if (IsConstantField())
                    return (target, value) => throw new FieldAccessException($"Field {Name} cannot be set.");
                return DelegatesFactory.CreateSetter(field);
            }

            #endregion
        }

        /// <summary>
        /// Constructor for an enum value.
        /// </summary>
        /// <param name="field"><see cref="System.Reflection.FieldInfo"/> to wrap.</param>
        /// <param name="enumType"><see cref="Type"/> of the enumeration.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="field"/> is null.</exception>
        internal ImmediateField([NotNull] FieldInfo field, [NotNull] Type enumType)
            : base(field)
        {
            if (enumType is null)
                throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum)
                throw new ArgumentException($"{nameof(enumType)} be must an {nameof(Enum)} type.");

            FieldInfo = field;
            FieldType = field.FieldType;

            // Getter / No setter
            object enumValue = field.GetValue(null);
            _getter = target => enumValue;
            _setter = (target, value) => throw new FieldAccessException("Cannot set an enumeration value.");
        }

        /// <summary>
        /// Returns the field value of the specified object.
        /// </summary>
        /// <param name="obj">Object that field value will be returned.</param>
        /// <returns>Field value of the specified object.</returns>
        /// <exception cref="InvalidCastException">If the <paramref name="obj"/> is not the owner of this field.</exception>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null and the field to get is not static.</exception>
        [Pure]
        public object GetValue([CanBeNull] object obj)
        {
            return _getter(obj);
        }

        /// <summary>
        /// Sets the field value of the specified object.
        /// </summary>
        /// <param name="obj">Object that field value will be set.</param>
        /// <param name="value">New field value.</param>
        /// <exception cref="InvalidCastException">If the <paramref name="obj"/> is not the owner of this field or if the <paramref name="value"/> is of the wrong type.</exception>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null and the field to set is not static.</exception>
        public void SetValue([CanBeNull] object obj, [CanBeNull] object value)
        {
            _setter(obj, value);
        }

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateField);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateField other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return FieldInfo == other.FieldInfo;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return FieldInfo.GetHashCode();
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return FieldInfo.ToString();
        }
    }
}