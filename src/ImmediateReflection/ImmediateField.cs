using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a field and provides access to its metadata in a faster way.
    /// </summary>
    public sealed class ImmediateField : IEquatable<ImmediateField>
    {
        /// <summary>
        /// Gets the wrapped <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        [NotNull]
        public FieldInfo FieldInfo { get; }

        /// <summary>
        /// Gets the <see cref="System.Reflection.FieldInfo"/> name.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of this field.
        /// </summary>
        [NotNull]
        public Type FieldType { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="field"><see cref="System.Reflection.FieldInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="field"/> is null.</exception>
        internal ImmediateField([NotNull] FieldInfo field)
        {
            FieldInfo = field ?? throw new ArgumentNullException(nameof(field));
            Name = field.Name;
            FieldType = field.FieldType;
        }

        /// <summary>
        /// Returns the field value of the specified object.
        /// </summary>
        /// <param name="obj">Object that field value will be returned.</param>
        /// <returns>Field value of the specified object.</returns>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null.</exception>
        [Pure]
        public object GetValue([NotNull] object obj)
        {
            return FieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Sets the field value of the specified object.
        /// </summary>
        /// <param name="obj">Object that field value will be set.</param>
        /// <param name="value">New field value.</param>
        /// <exception cref="TargetException">If the given <paramref name="obj"/> is null.</exception>
        public void SetValue([NotNull] object obj, [CanBeNull] object value)
        {
            FieldInfo.SetValue(obj, value);
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