using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Getter delegate.
    /// </summary>
    /// <param name="target">Object instance to get a value, null if static.</param>
    /// <returns>Got value.</returns>
    public delegate object GetterDelegate([CanBeNull] object target);

    /// <summary>
    /// Setter delegate.
    /// </summary>
    /// <param name="target">Object instance to set a value, null if static.</param>
    /// <param name="value">Value to set.</param>
    public delegate void SetterDelegate([CanBeNull] object target, [CanBeNull] object value);
}