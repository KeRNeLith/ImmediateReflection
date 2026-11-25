using System;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection;

/// <summary>
/// Extensions to easily work with Immediate Reflection.
/// </summary>
[PublicAPI]
public static class ImmediateReflectionExtensions
{
    /// <summary>
    /// Gets the <see cref="ImmediateType"/> corresponding to this instance.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    /// <param name="obj">Object instance.</param>
    /// <returns>The corresponding <see cref="ImmediateType"/>.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="obj"/>is null.</exception>
    [PublicAPI]
    [ContractAnnotation("obj:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType GetImmediateType<T>(this T obj)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));
        return TypeAccessor.Get(obj.GetType());
    }

    /// <summary>
    /// Gets the <see cref="ImmediateType"/> corresponding to this <see cref="T:System.Type"/>.
    /// </summary>
    /// <param name="type">A <see cref="T:System.Type"/>.</param>
    /// <returns>The corresponding <see cref="ImmediateType"/>.</returns>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/>is null.</exception>
    [PublicAPI]
    [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType GetImmediateType(this Type type)
    {
        return TypeAccessor.Get(type);
    }
}