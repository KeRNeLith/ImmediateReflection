using System;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
    /// <see cref="ImmediateType"/> gives access to Reflection features in a faster way than standard stuff.
    /// </summary>
    [PublicAPI]
    public static class TypeAccessor
    {
        internal const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        [PublicAPI]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType Get<T>()
        {
            return Get(typeof(T));
        }

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// It gives access to all public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType Get([NotNull] Type type)
        {
            return Get(type, DefaultFlags);
        }

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        [PublicAPI]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType Get<T>(bool includeNonPublicMembers)
        {
            return Get(typeof(T), includeNonPublicMembers);
        }

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// If <paramref name="includeNonPublicMembers"/> is set to true it gives access to all public and not public instance members.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="includeNonPublicMembers">Indicates if non public members should be taken into account.</param>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType Get([NotNull] Type type, bool includeNonPublicMembers)
        {
            return !includeNonPublicMembers 
                ? Get(type) 
                : Get(type, DefaultFlags | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <typeparam name="T"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</typeparam>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        [PublicAPI]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateType Get<T>(BindingFlags flags)
        {
            return Get(typeof(T), flags);
        }

        /// <summary>
        /// Provides an access to a <see cref="T:System.Type"/> Reflection information via an <see cref="ImmediateType"/>.
        /// </summary>
        /// <remarks>Returned <see cref="ImmediateType"/> is cached within the library.</remarks>
        /// <param name="type"><see cref="T:System.Type"/> to get a corresponding <see cref="ImmediateType"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static ImmediateType Get([NotNull] Type type, BindingFlags flags)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return CachesHandler.Instance.GetImmediateType(type, flags);
        }
    }
}