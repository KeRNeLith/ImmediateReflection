using System;
using System.Collections;
using System.Collections.Generic;
#if SUPPORTS_LINQ
using System.Linq;
#endif
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
#if !SUPPORTS_STRING_FULL_FEATURES || !SUPPORTS_LINQ
using ImmediateReflection.Utils;
#endif

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a collection of fields and provides access to its metadata in a faster way.
    /// </summary>
    public sealed class ImmediateFields : IEnumerable<ImmediateField>, IEquatable<ImmediateFields>
    {
        [NotNull]
        private readonly Dictionary<string, ImmediateField> _fields;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fields">Enumerable of <see cref="FieldInfo"/> to wrap.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="fields"/> enumerable is null,
        /// or if it contains a null <see cref="FieldInfo"/>.</exception>
        internal ImmediateFields([NotNull, ItemNotNull] IEnumerable<FieldInfo> fields)
        {
            if (fields is null)
                throw new ArgumentNullException(nameof(fields));

#if SUPPORTS_LINQ
            _fields = fields.ToDictionary(
                field => field?.Name ?? throw new ArgumentNullException(nameof(field), "A field is null."),
                field => new ImmediateField(field));
#else
            _fields = new Dictionary<string, ImmediateField>();
            foreach (FieldInfo field in fields)
            {
                string name = field?.Name ?? throw new ArgumentNullException(nameof(field), "A field is null.");
                _fields[name] = new ImmediateField(field);
            }
#endif
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumType"><see cref="Type"/> of the enumeration.</param>
        /// <param name="enumValue">Field corresponding to the current enumeration value.</param>
        /// <param name="enumValues">Enumerable of <see cref="FieldInfo"/> corresponding to enum values.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="enumType"/> or <paramref name="enumValue"/> or the <paramref name="enumValues"/> enumerable
        /// is null, or if it contains a null <see cref="FieldInfo"/>.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="enumType"/> is not an enumeration type.</exception>
        internal ImmediateFields([NotNull] Type enumType, [NotNull] FieldInfo enumValue, [NotNull, ItemNotNull] IEnumerable<FieldInfo> enumValues)
        {
            if (enumType is null)
                throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum)
                throw new ArgumentException($"{nameof(enumType)} be must an {nameof(Enum)} type.");
            if (enumValue is null)
                throw new ArgumentNullException(nameof(enumValue));
            if (enumValues is null)
                throw new ArgumentNullException(nameof(enumValues));

#if SUPPORTS_LINQ
            _fields = enumValues.ToDictionary(
                field => field?.Name ?? throw new ArgumentNullException(nameof(field), "An enum field is null."),
                field => new ImmediateField(field, enumType));
#else
            _fields = new Dictionary<string, ImmediateField>();
            foreach (FieldInfo field in enumValues)
            {
                string name = field?.Name ?? throw new ArgumentNullException(nameof(field), "An enum field is null.");
                _fields[name] = new ImmediateField(field, enumType);
            }
#endif
            _fields[enumValue.Name] = new ImmediateField(enumValue);
        }

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Found <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [CanBeNull]
        public ImmediateField this[[NotNull] string fieldName] =>
            _fields.TryGetValue(fieldName, out ImmediateField field)
                ? field
                : null;

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Found <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [Pure]
        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public ImmediateField GetField([NotNull] string fieldName) => this[fieldName];

        #region Equality / IEquatable<T>

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as ImmediateFields);
        }

        /// <inheritdoc />
        public bool Equals(ImmediateFields other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_fields.Count != other._fields.Count)
                return false;

#if SUPPORTS_LINQ
            return !_fields.Except(other._fields).Any();
#else
            return !EnumerableUtils.Except(_fields, other._fields)
                .GetEnumerator()
                .MoveNext();
#endif
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (KeyValuePair<string, ImmediateField> pair in _fields)
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
        public IEnumerator<ImmediateField> GetEnumerator()
        {
            return _fields.Values.GetEnumerator();
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
#if SUPPORTS_STRING_FULL_FEATURES
            return $"[{string.Join(", ", _fields.Values)}]";
#else
            return $"[{StringUtils.Join(", ", _fields.Values)}]";
#endif
        }
    }
}