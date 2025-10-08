using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ImmediateReflection;

/// <summary>
/// General helpers for Immediate Reflection code.
/// </summary>
internal static class GeneralHelpers
{
    /// <summary>
    /// Calls <see cref="M:System.Diagnostics.Debug.Assert"/> in a way that it does not raise CS8602/CS8604
    /// in caller code and makes static analysis happy while conserving the runtime assert for Debug configuration.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <param name="value">Value to assert not null.</param>
    [Conditional("DEBUG")]
    public static void AssertNotNull<T>([NotNullIfNotNull("value")] T? value)
    {
        Debug.Assert(value is not null);
    }
}