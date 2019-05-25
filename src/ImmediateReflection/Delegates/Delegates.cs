using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Default constructor delegate.
    /// </summary>
    /// <returns>Newly created object.</returns>
    [NotNull]
    public delegate object DefaultConstructorDelegate();

    /// <summary>
    /// Getter delegate.
    /// </summary>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    public delegate object GetterDelegate([CanBeNull] object target);

    /// <summary>
    /// Template getter delegate (ref).
    /// </summary>
    /// <typeparam name="TOwner">Owner object type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    public delegate TValue RefGetterDelegate<TOwner, out TValue>([CanBeNull] ref TOwner target);

    /// <summary>
    /// Setter delegate.
    /// </summary>
    /// <param name="target">Object instance to set a value, null if static.</param>
    /// <param name="value">Value to set.</param>
    public delegate void SetterDelegate([CanBeNull] object target, [CanBeNull] object value);
}