#if SUPPORTS_CACHING
using System;
using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Represents a type that implements a memory cache.
    /// </summary>
    /// <typeparam name="TKey">Cache key type.</typeparam>
    /// <typeparam name="TValue">Cache value type.</typeparam>
    internal class MemoryCache<TKey, TValue>
    {
        [NotNull]
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _cache = new ConcurrentDictionary<TKey, Lazy<TValue>>();

        /// <summary>
        /// Gets the cached value corresponding to the given <paramref name="key"/> if already cached, or creates
        /// a new entry if not available.
        /// </summary>
        /// <param name="key">Cache key.</param>
        /// <param name="valueFactory">Factory method to create the value if it does not exist.</param>
        /// <returns>The value.</returns>
        [NotNull]
        [ContractAnnotation("key:null => halt;valueFactory:null => halt")]
        public TValue GetOrCreate([NotNull] TKey key, [NotNull] Func<TValue> valueFactory)
        {
            return _cache.GetOrAdd(
                key,
                k => new Lazy<TValue>(valueFactory)).Value;
        }
    }
}
#endif