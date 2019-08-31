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

    /// <summary>
    /// Copy constructor delegate.
    /// </summary>
    /// <param name="other">Object to copy.</param>
    /// <returns>Newly created object.</returns>
    [PublicAPI]
    [NotNull]
    public delegate object CopyConstructorDelegate([CanBeNull] object other);

    /// <summary>
    /// Constructor delegate.
    /// </summary>
    /// <param name="arguments">Constructor arguments.</param>
    /// <returns>Newly created object.</returns>
    [PublicAPI]
    [NotNull]
    public delegate object ConstructorDelegate([CanBeNull, ItemCanBeNull] params object[] arguments);

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
}