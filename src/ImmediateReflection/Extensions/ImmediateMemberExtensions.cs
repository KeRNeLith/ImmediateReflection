#if SUPPORTS_EXTENSIONS
using System;
using System.Collections.Generic;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to easily work with <see cref="ImmediateProperty"/>.
    /// </summary>
    [PublicAPI]
    public static class ImmediateMemberExtensions
    {
        #region Field

        /// <summary>
        /// Searches for the public <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>The corresponding <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [CanBeNull]
        [ContractAnnotation("type:null => halt;fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateField GetImmediateField([NotNull] this Type type, [NotNull] string fieldName)
        {
            return TypeAccessor.Get(type).GetField(fieldName);
        }

        /// <summary>
        /// Searches for the <see cref="ImmediateField"/> corresponding to the given <paramref name="fieldName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="fieldName">Field name.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <returns>The corresponding <see cref="ImmediateField"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="fieldName"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [CanBeNull]
        [ContractAnnotation("type:null => halt;fieldName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateField GetImmediateField([NotNull] this Type type, [NotNull] string fieldName, BindingFlags flags)
        {
            return TypeAccessor.Get(type, flags).GetField(fieldName);
        }

        /// <summary>
        /// Gets all the public fields of this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <returns>All <see cref="ImmediateField"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull, ItemNotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<ImmediateField> GetImmediateFields([NotNull] this Type type)
        {
            return TypeAccessor.Get(type).GetFields();
        }

        /// <summary>
        /// Gets all the fields of this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <returns>All <see cref="ImmediateField"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull, ItemNotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<ImmediateField> GetImmediateFields([NotNull] this Type type, BindingFlags flags)
        {
            return TypeAccessor.Get(type, flags).GetFields();
        }

        #endregion

        #region Property

        /// <summary>
        /// Searches for the public <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>The corresponding <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [CanBeNull]
        [ContractAnnotation("type:null => halt;propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateProperty GetImmediateProperty([NotNull] this Type type, [NotNull] string propertyName)
        {
            return TypeAccessor.Get(type).GetProperty(propertyName);
        }

        /// <summary>
        /// Searches for the <see cref="ImmediateProperty"/> corresponding to the given <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <returns>The corresponding <see cref="ImmediateProperty"/>, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="propertyName"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [CanBeNull]
        [ContractAnnotation("type:null => halt;propertyName:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static ImmediateProperty GetImmediateProperty([NotNull] this Type type, [NotNull] string propertyName, BindingFlags flags)
        {
            return TypeAccessor.Get(type, flags).GetProperty(propertyName);
        }

        /// <summary>
        /// Gets all the public properties of this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <returns>All <see cref="ImmediateProperty"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull, ItemNotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<ImmediateProperty> GetImmediateProperties([NotNull] this Type type)
        {
            return TypeAccessor.Get(type).GetProperties();
        }

        /// <summary>
        /// Gets all the properties of this <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/>.</param>
        /// <param name="flags">Flags that must be taken into account to get members.</param>
        /// <returns>All <see cref="ImmediateProperty"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
#if !SUPPORTS_CACHING
        [Pure]
#endif
        [NotNull, ItemNotNull]
        [ContractAnnotation("type:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<ImmediateProperty> GetImmediateProperties([NotNull] this Type type, BindingFlags flags)
        {
            return TypeAccessor.Get(type, flags).GetProperties();
        }

        #endregion
    }
}
#endif