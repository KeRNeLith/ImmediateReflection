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
    /// Represents type declarations: class types, interface types, array types, value types, enumeration types,
    /// type parameters, generic type definitions, and open or closed constructed generic types in a faster way.
    /// </summary>
    public sealed class ImmediateType : IEquatable<ImmediateType>
    {
        /// <summary>
        /// Gets the wrapped <see cref="System.Type"/>.
        /// </summary>
        [NotNull]
        public Type Type { get; }

        /// <summary>
        /// Gets the <see cref="System.Type"/> name.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary>
        /// Gets the fully qualified name of the <see cref="System.Type"/>, including its namespace but not its assembly.
        /// </summary>
        /// <remarks>Fallback on the type name if full name is null.</remarks>
        [NotNull]
        public string FullName { get; }

        /// <summary>
        /// Gets all the fields of this <see cref="System.Type"/>.
        /// </summary>
        [NotNull, ItemNotNull]
        public ImmediateFields Fields { get; }

        /// <summary>
        /// Gets all the properties of this <see cref="System.Type"/>.
        /// </summary>
        [NotNull, ItemNotNull]
        public ImmediateProperties Properties { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type"><see cref="System.Type"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="type"/> is null.</exception>
        internal ImmediateType([NotNull] Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = type.Name;
            FullName = type.FullName ?? Name;

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            Fields = new ImmediateFields(type.GetFields(flags));
            Properties = new ImmediateProperties(type.GetProperties(flags));
        }

        /// <summary>
        /// Gets all the fields of this <see cref="System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateField"/>.</returns>
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateField> GetFields() => Fields;

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateField GetField([NotNull] string fieldName) => Fields[fieldName];

        /// <summary>
        /// Gets all the properties of this <see cref="System.Type"/>.
        /// </summary>
        /// <returns>All <see cref="ImmediateProperty"/>.</returns>
        [NotNull, ItemNotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IEnumerable<ImmediateProperty> GetProperties() => Properties;

        /// <summary>
        /// Gets the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Found <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="propertyName"/> is null.</exception>
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateProperty GetProperty([NotNull] string propertyName) => Properties[propertyName];

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateType);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateType other)
        {
            if (other is null)
                return false;
            return Type == other.Type;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return Type.ToString();
        }
    }
}