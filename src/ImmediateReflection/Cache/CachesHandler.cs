#if SUPPORTS_CACHING
using System;
using System.Reflection;
using JetBrains.Annotations;
#if SUPPORTS_MICROSOFT_CACHING
using Microsoft.Extensions.Caching.Memory;
#endif

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

#if SUPPORTS_SYSTEM_CACHING
        [NotNull]
        private volatile MemoryCache<ImmediateType> _cachedTypes = new MemoryCache<ImmediateType>(CacheConstants.TypesCacheName);
#else
        [NotNull]
        private volatile MemoryCache _cachedTypes = new MemoryCache(new MemoryCacheOptions());
#endif

        [NotNull]
        public ImmediateType GetImmediateType([NotNull] Type type, BindingFlags flags, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            string key = GetCacheKey();

#if SUPPORTS_SYSTEM_CACHING
            return _cachedTypes.GetOrCreate(key, entry =>
            {
                if (expirationTime.HasValue)
                {
                    entry.SlidingExpiration = expirationTime.Value.Offset;
                }

                return new ImmediateType(type, flags);
            });
#else
            return _cachedTypes.GetOrCreate(key, entry =>
            {
                if (expirationTime.HasValue)
                {
                    entry.SetOptions(new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expirationTime.Value.Offset
                    });
                }
                return new ImmediateType(type, flags);
            });
#endif

            #region Local function

            string GetCacheKey()
            {
                return $"{type.GetHashCode().ToString()};{flags.ToString()}";
            }

            #endregion
        }

        #endregion

        #region Attributes cache

#if SUPPORTS_SYSTEM_CACHING
        [NotNull]
        private volatile MemoryCache<AttributesCache> _cachedAttributes = new MemoryCache<AttributesCache>(CacheConstants.AttributesCacheName);
#else
        [NotNull]
        private volatile MemoryCache _cachedAttributes = new MemoryCache(new MemoryCacheOptions());
#endif

        [NotNull]
        public AttributesCache GetAttributesCache([NotNull] MemberInfo member, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            if (member is null)
                throw new ArgumentNullException(nameof(member));

            string key = GetCacheKey();

#if SUPPORTS_SYSTEM_CACHING
            return _cachedAttributes.GetOrCreate(key, entry =>
            {
                if (expirationTime.HasValue)
                {
                    entry.SlidingExpiration = expirationTime.Value.Offset;
                }

                return new AttributesCache(member);
            });
#else
            return _cachedAttributes.GetOrCreate(key, entry =>
            {
                if (expirationTime.HasValue)
                {
                    entry.SetOptions(new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expirationTime.Value.Offset
                    });
                }
                return new AttributesCache(member);
            });
#endif

            #region Local function

            string GetCacheKey()
            {
                return member.GetHashCode().ToString();
            }

            #endregion
        }

        #endregion
    }
}
#endif