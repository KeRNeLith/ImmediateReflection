#if SUPPORTS_SYSTEM_CACHING
using System;
using System.Runtime.Caching;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a type that implements a memory cache.
    /// </summary>
    /// <typeparam name="TValue">Cache value.</typeparam>
    internal class MemoryCache<TValue>
    {
        [NotNull]
        private readonly MemoryCache _cache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Cache name.</param>
        public MemoryCache([NotNull] string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            _cache = new MemoryCache(name);
        }

        /// <summary>
        /// Gets the cached value corresponding to the given <paramref name="key"/> if already cached, or creates
        /// a new entry if not available.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="valueFactory">Factory method to create the value if it does not exist.</param>
        /// <returns>The value.</returns>
        [NotNull]
        public TValue GetOrCreate([NotNull] string key, [NotNull, InstantHandle] Func<CacheItemPolicy, TValue> valueFactory)
        {
            if (_cache.Get(key) is TValue cachedValue)
                return cachedValue;

            var cacheItemPolicy = new CacheItemPolicy();
            TValue newValue = valueFactory(cacheItemPolicy);
            if (newValue == null)
                throw new InvalidOperationException("Cache must contains not null entries.");

            if (!_cache.Add(key, newValue, cacheItemPolicy))
                throw new InvalidOperationException($"Cache must contains an entry for key {key}.");
            return newValue;
        }
    }
}
#endif