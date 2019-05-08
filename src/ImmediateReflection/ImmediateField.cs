using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a field and provides access to its metadata in a faster way.
    /// </summary>
    public class ImmediateField : IEquatable<ImmediateField>
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
        /// Constructor.
        /// </summary>
        /// <param name="field"><see cref="System.Reflection.FieldInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="field"/> is null.</exception>
        internal ImmediateField([NotNull] FieldInfo field)
        {
            FieldInfo = field ?? throw new ArgumentNullException(nameof(field));
            Name = field.Name;
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