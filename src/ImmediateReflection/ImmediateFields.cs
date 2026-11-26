using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using System.Runtime.Serialization;
using System.Security.Permissions;
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;
using static ImmediateReflection.Utils.SerializationHelpers;

namespace ImmediateReflection;

/// <summary>
/// Represents a collection of fields and provides access to its metadata in a faster way.
/// </summary>
[PublicAPI]
[Serializable]
public sealed class ImmediateFields
    : IEnumerable<ImmediateField>
    , IEquatable<ImmediateFields>
    , ISerializable
{
    private readonly Dictionary<string, ImmediateField> _fields;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="fields">Enumerable of <see cref="FieldInfo"/> to wrap.</param>
    internal ImmediateFields(IEnumerable<FieldInfo> fields)
    {
        // ReSharper disable PossibleMultipleEnumeration, Justification: Only checking count, not really enumerating whole enumerable
        _fields = new Dictionary<string, ImmediateField>(GetCapacity(fields), StringComparer.Ordinal);
        Init(fields);
        // ReSharper restore PossibleMultipleEnumeration

        #region Local function

        static int GetCapacity(IEnumerable<FieldInfo> fields)
        {
            return fields is ICollection<FieldInfo> collection ? collection.Count : 0;
        }

        #endregion
    }

    private void Init(IEnumerable<FieldInfo> fields)
    {
        // ReSharper disable PossibleMultipleEnumeration, Justification: Only in debug and not really enumerating whole enumerable
        AssertNotNull(fields);

        foreach (FieldInfo field in fields)
        // ReSharper restore PossibleMultipleEnumeration
        {
            AssertNotNull(field);

            _fields.Add(field.Name, CachesHandler.Instance.GetField(field));
        }
    }

    /// <summary>
    /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
    /// </summary>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Found <see cref="ImmediateField"/>, otherwise null.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
    [PublicAPI]
    public ImmediateField? this[string fieldName] =>
        _fields.TryGetValue(fieldName, out ImmediateField? field)
            ? field
            : null;

    /// <summary>
    /// Gets the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
    /// </summary>
    /// <param name="fieldName">Field name.</param>
    /// <returns>Found <see cref="ImmediateField"/>, otherwise null.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="fieldName"/> is null.</exception>
    [PublicAPI]
    [Pure]
    [ContractAnnotation("fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public ImmediateField? GetField(string fieldName) => this[fieldName];

    #region Equality / IEquatable<T>

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as ImmediateFields);

    /// <inheritdoc />
    public bool Equals(ImmediateFields? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (_fields.Count != other._fields.Count)
            return false;

        foreach (KeyValuePair<string, ImmediateField> pair in _fields)
        {
            if (!other._fields.TryGetValue(pair.Key, out ImmediateField? otherField))
                return false;

            if (!pair.Value.Equals(otherField))
                return false;
        }

        return true;
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
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region IEnumerable<T>

    /// <inheritdoc />
    public IEnumerator<ImmediateField> GetEnumerator() => _fields.Values.GetEnumerator();

    #endregion

    #region ISerializable

    private ImmediateFields(SerializationInfo info, StreamingContext context)
    {
        // Serialized data is untrusted: validated (count, nulls, duplicates, consistency) before use
        List<FieldInfo> fields = ReadMembers<FieldInfo>(info, "Field");

        _fields = new Dictionary<string, ImmediateField>(fields.Count, StringComparer.Ordinal);
        Init(fields);
    }

    /// <inheritdoc />
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Count", _fields.Count);

        int i = -1;
        foreach (FieldInfo field in _fields.Select(pair => pair.Value.FieldInfo))
        {
            info.AddValue($"Field{++i}", field);
        }
    }

    #endregion

    /// <inheritdoc />
    public override string ToString() => $"[{string.Join(", ", _fields.Values)}]";
}