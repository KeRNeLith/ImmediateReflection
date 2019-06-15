#if SUPPORTS_CACHING
#if SUPPORTS_SYSTEM_CORE
using System;
#else
using ImmediateReflection.Utils;
#endif
using System.Collections;
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
        private readonly Hashtable _cache = new Hashtable();

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
            // ReSharper disable once InconsistentlySynchronizedField, Justification: HashTable is thread safe for reading
            var cachedValue = (TValue)_cache[key];
            if (cachedValue != null)
                return cachedValue;

            lock (_cache)
            {
                // Double check (init during lock wait)
                cachedValue = (TValue)_cache[key];
                if (cachedValue != null)
                    return cachedValue;

                cachedValue = valueFactory();
                _cache[key] = cachedValue;
                return cachedValue;
            }
        }
    }
}
#endif