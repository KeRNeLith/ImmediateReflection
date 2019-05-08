using System;
using System.Linq;
using System.Reflection;
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
        /// <remarks>Fallback on the type name if fullname is null.</remarks>
        [NotNull]
        public string FullName { get; }

        /// <summary>
        /// Gets all the fields of this <see cref="System.Type"/>.
        /// </summary>
        [NotNull, ItemNotNull]
        public ImmediateField[] Fields { get; }

        /// <summary>
        /// Gets all the properties of this <see cref="System.Type"/>.
        /// </summary>
        [NotNull, ItemNotNull]
        public ImmediateProperty[] Properties { get; }

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
            Fields = type.GetFields(flags)
                .Select(field => new ImmediateField(field))
                .ToArray();

            Properties = type.GetProperties(flags)
                .Select(property => new ImmediateProperty(property))
                .ToArray();
        }

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