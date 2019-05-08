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
        /// Constructor.
        /// </summary>
        /// <param name="property"><see cref="System.Reflection.PropertyInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="property"/> is null.</exception>
        internal ImmediateProperty([NotNull] PropertyInfo property)
        {
            PropertyInfo = property ?? throw new ArgumentNullException(nameof(property));
            Name = property.Name;
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