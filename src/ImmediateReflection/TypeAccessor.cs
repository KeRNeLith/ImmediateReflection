using System;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection;

/// <summary>
/// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
/// <see cref="ImmediateType"/> gives access to Reflection features in a faster way than standard stuff.
/// </summary>
[PublicAPI]
public static class TypeAccessor
{
    internal const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// It gives access to all public instance members.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
    [PublicAPI]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType Get<T>()
    {
        return DefaultTypeHolder<T>.GetOrInitialize();
    }

    // Per type T shortcut to the default ImmediateType, it avoids the lookups in the caches.
    // Not a static initializer on purpose: nothing can fail in a type initializer and concurrent initializations
    // are harmless as they all get the same (cached) instance.
    private static class DefaultTypeHolder<T>
    {
        // ReSharper disable once StaticMemberInGenericType, Justification: This is a per-type cache, it is not a static member of the outer class
        private static ImmediateType? _instance;

#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType GetOrInitialize()
        {
            return _instance ?? Initialize();
        }

        private static ImmediateType Initialize()
        {
            return _instance = Get(typeof(T), DefaultFlags);
        }
    }

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// It gives access to all public instance members.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
    [PublicAPI]
    [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType Get(Type type)
    {
        return Get(type, DefaultFlags);
    }

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
    /// <param name="includeNonPublicMembers">Indicates if nonpublic members should be taken into account.</param>
    [PublicAPI]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType Get<T>(bool includeNonPublicMembers)
    {
        return includeNonPublicMembers
            ? Get(typeof(T), DefaultFlags | BindingFlags.NonPublic)
            : Get<T>();
    }

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
    /// <param name="includeNonPublicMembers">Indicates if nonpublic members should be taken into account.</param>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
    [PublicAPI]
    [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType Get(Type type, bool includeNonPublicMembers)
    {
        return includeNonPublicMembers
            ? Get(type, DefaultFlags | BindingFlags.NonPublic)
            : Get(type);
    }

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
    /// <param name="flags">Flags that must be taken into account to get members.</param>
    [PublicAPI]
#if SUPPORTS_AGGRESSIVE_INLINING
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ImmediateType Get<T>(BindingFlags flags)
    {
        return Get(typeof(T), flags);
    }

    /// <summary>
    /// Provides access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// </summary>
    /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
    /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
    /// <param name="flags">Flags that must be taken into account to get members.</param>
    /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
    [PublicAPI]
    [ContractAnnotation("type:null => halt")]
    public static ImmediateType Get(Type type, BindingFlags flags)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return CachesHandler.Instance.GetImmediateType(type, flags);
    }
}