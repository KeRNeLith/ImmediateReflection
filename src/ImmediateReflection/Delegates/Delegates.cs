using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    #region Constructors

    /// <summary>
    /// Default constructor delegate.
    /// </summary>
    /// <returns>Newly created object.</returns>
    [PublicAPI]
    [NotNull]
    public delegate object DefaultConstructorDelegate();

    #endregion

    #region Getters

    /// <summary>
    /// Static getter delegate.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <returns>Got value.</returns>
    [PublicAPI]
    public delegate TValue StaticGetterDelegate<out TValue>();

    /// <summary>
    /// Getter delegate.
    /// </summary>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    [PublicAPI]
    public delegate object GetterDelegate([CanBeNull] object target);

    /// <summary>
    /// Template getter delegate.
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    [PublicAPI]
    public delegate object GetterDelegate<in TOwner>([CanBeNull] TOwner target);

    /// <summary>
    /// Template getter delegate.
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    [PublicAPI]
    public delegate TValue GetterDelegate<in TOwner, out TValue>([CanBeNull] TOwner target);

    /// <summary>
    /// Template getter delegate (ref).
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    internal delegate TValue RefGetterDelegate<TOwner, out TValue>([CanBeNull] ref TOwner target);

    #endregion

    #region Setters

    /// <summary>
    /// Static setter delegate.
    /// </summary>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="value">Value to set.</param>
    [PublicAPI]
    public delegate void StaticSetterDelegate<in TValue>([CanBeNull] TValue value);

    /// <summary>
    /// Setter delegate.
    /// </summary>
    /// <param name="target">Object instance to set a value, null if static.</param>
    /// <param name="value">Value to set.</param>
    [PublicAPI]
    public delegate void SetterDelegate([CanBeNull] object target, [CanBeNull] object value);

    /// <summary>
    /// Template setter delegate.
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <param name="target">Object instance to set a value, null if static.</param>
    /// <param name="value">Value to set.</param>
    [PublicAPI]
    public delegate void SetterDelegate<in TOwner>([CanBeNull] TOwner target, [CanBeNull] object value);

    /// <summary>
    /// Template setter delegate.
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="target">Object instance to set a value, null if static.</param>
    /// <param name="value">Value to set.</param>
    [PublicAPI]
    public delegate void SetterDelegate<in TOwner, in TValue>([CanBeNull] TOwner target, [CanBeNull] TValue value);

    #endregion

    #region Methods

    /// <summary>
    /// Method delegate.
    /// </summary>
    /// <param name="target">Object instance to call a method on, null if static.</param>
    /// <param name="arguments">Method arguments</param>
    /// <returns>Method return if there is one, otherwise null.</returns>
    /// <exception cref="ArgumentException">If <paramref name="arguments"/> do not match method signature.</exception>
    /// <exception cref="TargetException">If the given <paramref name="target"/> is null and the method is not static, or if its not the owner of the method.</exception>
    /// <exception cref="TargetInvocationException">The called method raises an exception.</exception>
    /// <exception cref="TargetParameterCountException">If the wrong number of arguments was given to the method.</exception>
    /// <exception cref="InvalidOperationException">
    /// The declaring type of the method is an open generic type.
    /// Meaning its <see cref="Type.ContainsGenericParameters"/> property returns true.
    /// </exception>
    [PublicAPI]
    public delegate object MethodDelegate([CanBeNull] object target, [CanBeNull, ItemCanBeNull] object[] arguments = null);

    #endregion
}