#if !SUPPORTS_SYSTEM_CORE

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value of the
    /// type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
    /// <returns>The return value of the method that this delegate encapsulates.</returns>
    internal delegate TResult Func<out TResult>();
}

#endif