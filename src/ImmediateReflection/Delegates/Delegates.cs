using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace ImmediateReflection;

#region Constructors

/// <summary>
/// Default constructor delegate.
/// </summary>
/// <returns>Newly created object.</returns>
[PublicAPI]
public delegate object DefaultConstructorDelegate();

/// <summary>
/// Copy constructor delegate.
/// </summary>
/// <param name="other">Object to copy.</param>
/// <returns>Newly created object.</returns>
[PublicAPI]
[return: NotNullIfNotNull("other")]
public delegate object? CopyConstructorDelegate(object? other);

/// <summary>
/// Constructor delegate.
/// </summary>
/// <param name="arguments">Constructor arguments.</param>
/// <returns>Newly created object.</returns>
[PublicAPI]
public delegate object ConstructorDelegate(params object?[]? arguments);

#endregion

#region Getters

/// <summary>
/// Static getter delegate.
/// </summary>
/// <typeparam name="TValue">Value type.</typeparam>
/// <returns>Got value.</returns>
[PublicAPI]
public delegate TValue? StaticGetterDelegate<out TValue>();

/// <summary>
/// Getter delegate.
/// </summary>
/// <param name="target">Object instance to get a value, null if static.</param>
/// <returns>Got value.</returns>
[PublicAPI]
public delegate object? GetterDelegate(object? target);

/// <summary>
/// Template getter delegate.
/// </summary>
/// <typeparam name="TOwner">Owner object type.</typeparam>
/// <param name="target">Object instance to get a value, null if static.</param>
/// <returns>Got value.</returns>
[PublicAPI]
public delegate object? GetterDelegate<in TOwner>(TOwner? target);

/// <summary>
/// Template getter delegate.
/// </summary>
/// <typeparam name="TOwner">Owner object type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
/// <param name="target">Object instance to get a value, null if static.</param>
/// <returns>Got value.</returns>
[PublicAPI]
public delegate TValue? GetterDelegate<in TOwner, out TValue>(TOwner? target);

/// <summary>
/// Template getter delegate (ref).
/// </summary>
/// <typeparam name="TOwner">Owner object type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
/// <param name="target">Object instance to get a value, null if static.</param>
/// <returns>Got value.</returns>
internal delegate TValue? RefGetterDelegate<TOwner, out TValue>(ref TOwner? target);

#endregion

#region Setters

/// <summary>
/// Static setter delegate.
/// </summary>
/// <typeparam name="TValue">Value type.</typeparam>
/// <param name="value">Value to set.</param>
[PublicAPI]
public delegate void StaticSetterDelegate<in TValue>(TValue? value);

/// <summary>
/// Setter delegate.
/// </summary>
/// <param name="target">Object instance to set a value, null if static.</param>
/// <param name="value">Value to set.</param>
[PublicAPI]
public delegate void SetterDelegate(object? target, object? value);

/// <summary>
/// Template setter delegate.
/// </summary>
/// <typeparam name="TOwner">Owner object type.</typeparam>
/// <param name="target">Object instance to set a value, null if static.</param>
/// <param name="value">Value to set.</param>
[PublicAPI]
public delegate void SetterDelegate<in TOwner>(TOwner? target, object? value);

/// <summary>
/// Template setter delegate.
/// </summary>
/// <typeparam name="TOwner">Owner object type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
/// <param name="target">Object instance to set a value, null if static.</param>
/// <param name="value">Value to set.</param>
[PublicAPI]
public delegate void SetterDelegate<in TOwner, in TValue>(TOwner? target, TValue? value);

#endregion