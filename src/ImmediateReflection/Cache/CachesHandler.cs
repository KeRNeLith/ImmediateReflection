#if SUPPORTS_CACHING
using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Cache storing all ready to use Immediate Reflection data.
    /// </summary>
    /// <remarks>This is a singleton implementation.</remarks>
    internal sealed class CachesHandler
    {
        #region Singleton management

        private CachesHandler()
        {
        }

        /// <summary>
        /// Gets the cache instance.
        /// </summary>
        public static CachesHandler Instance { get; } = InstanceHandler.InternalInstance;

        private static class InstanceHandler
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static InstanceHandler()
            {
            }

            internal static readonly CachesHandler InternalInstance = new CachesHandler();
        }

        #endregion

        #region ImmediateType cache

        private struct TypeCacheKey : IEquatable<TypeCacheKey>
        {
            [NotNull]
            private readonly Type _type;

            private readonly BindingFlags _flags;

            public TypeCacheKey([NotNull] Type type, BindingFlags flags)
            {
                _type = type;
                _flags = flags;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is null)
                    return false;
                return obj is TypeCacheKey other && Equals(other);
            }

            /// <inheritdoc />
            public bool Equals(TypeCacheKey other)
            {
                return _type == other._type && _flags == other._flags;
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return (_type.GetHashCode() * 397) ^ (int)_flags;
            }
        }

        [NotNull]
        private volatile MemoryCache<TypeCacheKey, ImmediateType> _cachedTypes = new MemoryCache<TypeCacheKey, ImmediateType>();

        [NotNull]
        public ImmediateType GetImmediateType([NotNull] Type type, BindingFlags flags)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _cachedTypes.GetOrCreate(
                new TypeCacheKey(type, flags),
                () => new ImmediateType(type, flags));
        }

        #endregion

        #region Attributes cache

        [NotNull]
        private volatile MemoryCache<MemberInfo, AttributesCache> _cachedAttributes = new MemoryCache<MemberInfo, AttributesCache>();

        [NotNull]
        public AttributesCache GetAttributesCache([NotNull] MemberInfo member)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            return _cachedAttributes.GetOrCreate(member, () => new AttributesCache(member));
        }

        #endregion
    }
}
#endif