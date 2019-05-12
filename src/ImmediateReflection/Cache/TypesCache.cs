#if SUPPORTS_SYSTEM_CACHING || SUPPORTS_MICROSOFT_CACHING
using System;
using System.Reflection;
using JetBrains.Annotations;
#if SUPPORTS_SYSTEM_CACHING
using System.Runtime.Caching;
#else
using Microsoft.Extensions.Caching.Memory;
#endif

namespace ImmediateReflection
{
    /// <summary>
    /// Cache storing all ready to use Immediate Reflection data.
    /// </summary>
    /// <remarks>This is a singleton implementation.</remarks>
    internal sealed class TypesCache
    {
        [Pure]
        [NotNull]
        private static string GetCacheKey([NotNull] Type type, BindingFlags flags)
        {
            return $"{type.AssemblyQualifiedName ?? type.FullName ?? $"{type.Namespace}.{type.Name}"};{flags.ToString()}";
        }

#if SUPPORTS_SYSTEM_CACHING
        private volatile MemoryCache _cachedTypes = new MemoryCache("ImmediateTypesCache");
#else
        private volatile MemoryCache _cachedTypes = new MemoryCache(new MemoryCacheOptions());
#endif

        #region Singleton management

        private TypesCache()
        {
        }

        /// <summary>
        /// Gets the cache instance.
        /// </summary>
        public static TypesCache Instance { get; } = InstanceHandler.InternalInstance;

        private sealed class InstanceHandler
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static InstanceHandler()
            {
            }

            internal static readonly TypesCache InternalInstance = new TypesCache();
        }

        #endregion

        [NotNull]
        public ImmediateType GetImmediateType([NotNull] Type type, BindingFlags flags, [CanBeNull] DateTimeOffset? expirationTime = null)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            string key = GetCacheKey(type, flags);

#if SUPPORTS_SYSTEM_CACHING
            if (_cachedTypes.Contains(key))
            {
                object cachedEntry = _cachedTypes.Get(key);
                if (cachedEntry is null)
                    throw new InvalidOperationException($"Cache must contains an entry for key {key}.");
                return (ImmediateType) cachedEntry;
            }

            var immediateType = new ImmediateType(type, flags);
            _cachedTypes.Add(key, immediateType, expirationTime ?? DateTimeOffset.MaxValue);
            return immediateType;
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
        }
    }
}
#endif