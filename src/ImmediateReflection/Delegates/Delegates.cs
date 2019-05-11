using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Getter delegate.
    /// </summary>
    /// <param name="target">Object instance to get a value.</param>
    /// <returns>Got value.</returns>
    public delegate object GetterDelegate([NotNull] object target);

    /// <summary>
    /// Setter delegate.
    /// </summary>
    /// <param name="target">Object instance to set a value.</param>
    /// <param name="value">Value to set.</param>
    public delegate void SetterDelegate([NotNull] object target, [CanBeNull] object value);
}