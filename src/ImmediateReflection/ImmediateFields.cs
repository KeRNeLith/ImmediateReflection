using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
#if SUPPORTS_SYSTEM_CORE
using System.Linq;
#endif
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
#if !SUPPORTS_STRING_FULL_FEATURES || !SUPPORTS_SYSTEM_CORE
using ImmediateReflection.Utils;
#endif

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a collection of fields and provides access to its metadata in a faster way.
    /// </summary>
    [PublicAPI]
    public sealed class ImmediateFields : IEnumerable<ImmediateField>, IEquatable<ImmediateFields>
    {
        [NotNull]
        private readonly Dictionary<string, ImmediateField> _fields;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fields">Enumerable of <see cref="FieldInfo"/> to wrap.</param>
        internal ImmediateFields([NotNull, ItemNotNull] IEnumerable<FieldInfo> fields)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Debug.Assert(fields != null);
#if SUPPORTS_SYSTEM_CORE
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Debug.Assert(fields.All(field => field != null));
#else
            Debug.Assert(EnumerableUtils.All(fields, field => field != null));
#endif

#if SUPPORTS_SYSTEM_CORE
            _fields = fields.ToDictionary(
                field => field.Name,
#if SUPPORTS_CACHING
                field => CachesHandler.Instance.GetField(field));
#else
                field => new ImmediateField(field));
#endif
#else
            _fields = new Dictionary<string, ImmediateField>();
            foreach (FieldInfo field in fields)
            {
#if SUPPORTS_CACHING
                _fields.Add(field.Name, CachesHandler.Instance.GetField(field));
#else
                _fields.Add(field.Name, new ImmediateField(field));
#endif
            }
#endif
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="enumType"><see cref="Type"/> of the enumeration.</param>
        /// <param name="enumValue">Field corresponding to the current enumeration value.</param>
        /// <param name="enumValues">Enumerable of <see cref="FieldInfo"/> corresponding to enum values.</param>
        internal ImmediateFields([NotNull] Type enumType, [NotNull] FieldInfo enumValue, [NotNull, ItemNotNull] IEnumerable<FieldInfo> enumValues)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Debug.Assert(enumType != null);
            Debug.Assert(enumType.IsEnum);
            Debug.Assert(enumValue != null);
            Debug.Assert(enumValues != null);
#if SUPPORTS_SYSTEM_CORE
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            Debug.Assert(enumValues.All(value => value != null));
#else
            Debug.Assert(EnumerableUtils.All(enumValues, value => value != null));
#endif

#if SUPPORTS_SYSTEM_CORE
            _fields = enumValues.ToDictionary(
                field => field.Name,
#if SUPPORTS_CACHING
                field => CachesHandler.Instance.GetField(field, enumType));
#else
                field => new ImmediateField(field, enumType));
#endif
#else
            _fields = new Dictionary<string, ImmediateField>();
            foreach (FieldInfo field in enumValues)
            {
#if SUPPORTS_CACHING
                _fields.Add(field.Name, CachesHandler.Instance.GetField(field, enumType));
#else
                _fields.Add(field.Name, new ImmediateField(field, enumType));
#endif
            }
#endif
            _fields[enumValue.Name] = new ImmediateField(enumValue);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Found <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
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
        [PublicAPI]
        [Pure]
        [CanBeNull]
        [ContractAnnotation("fieldName:null => halt")]
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

#if SUPPORTS_SYSTEM_CORE
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