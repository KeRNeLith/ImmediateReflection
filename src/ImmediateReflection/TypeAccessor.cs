using System;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// <see cref="ImmediateType"/> gives access to Reflection features in a faster way than standard stuff.
    /// </summary>
    public static class TypeAccessor
    {
        internal const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance;

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="expirationTime">Cache expiration time.</param>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
#endif
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get<T>([CanBeNull] DateTimeOffset? expirationTime = null)
        {
            return Get(typeof(T), expirationTime);
        }
#else
        public static ImmediateType Get<T>()
        {
            return Get(typeof(T));
        }
#endif

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#endif
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get([NotNull] Type type, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            return Get(type, DefaultFlags, expirationTime);
        }
#else
        public static ImmediateType Get([NotNull] Type type)
        {
            return Get(type, DefaultFlags);
        }
#endif

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
#endif
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get<T>(bool includeNonPublicMembers, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            return Get(typeof(T), includeNonPublicMembers, expirationTime);
        }
#else
        public static ImmediateType Get<T>(bool includeNonPublicMembers)
        {
            return Get(typeof(T), includeNonPublicMembers);
        }
#endif

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#endif
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get([NotNull] Type type, bool includeNonPublicMembers, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            return !includeNonPublicMembers
                ? Get(type, expirationTime)
                : Get(type, DefaultFlags | BindingFlags.NonPublic, expirationTime);
        }
#else
        public static ImmediateType Get([NotNull] Type type, bool includeNonPublicMembers)
        {
            return !includeNonPublicMembers 
                ? Get(type) 
                : Get(type, DefaultFlags | BindingFlags.NonPublic);
        }
#endif

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
#endif
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get<T>(BindingFlags flags, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            return Get(typeof(T), flags, expirationTime);
        }
#else
        public static ImmediateType Get<T>(BindingFlags flags)
        {
            return Get(typeof(T), flags);
        }
#endif

#if SUPPORTS_MICROSOFT_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#elif SUPPORTS_SYSTEM_CACHING
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <param name="expirationTime">Cache expiration time.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="InvalidOperationException">If the cache contains a null entry for the given type.</exception>
#else
        /// <summary>
        /// Provides an access to a <see cref="Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
#endif
        [NotNull]
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
        public static ImmediateType Get([NotNull] Type type, BindingFlags flags, [CanBeNull] DateTimeOffset? expirationTime = null)
#else
        public static ImmediateType Get([NotNull] Type type, BindingFlags flags)
#endif
        {
#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
            return TypesCache.Instance.GetImmediateType(type, flags, expirationTime);
#else
            return new ImmediateType(type, flags);
#endif
        }
    }
}