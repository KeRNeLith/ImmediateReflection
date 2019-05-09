using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

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

            _fields = fields.ToDictionary(
                field => field?.Name ?? throw new ArgumentNullException(nameof(field), "A field is null."),
                field => new ImmediateField(field));
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
            return _fields.Count == other._fields.Count
                   && !_fields.Except(other._fields).Any();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _fields
                .Aggregate(
                    0,
                    (hashCode, pair) =>
                    {
                        hashCode = (hashCode * 397) ^ pair.Key.GetHashCode();
                        hashCode = (hashCode * 397) ^ pair.Value.GetHashCode();
                        return hashCode;
                    });
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
            return $"[{string.Join(", ", _fields.Values)}]";
        }
    }
}