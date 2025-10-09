using System;
using System.Collections;
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;

namespace ImmediateReflection;

/// <summary>
/// Represents a type that implements a memory cache.
/// </summary>
/// <typeparam name="TKey">Cache key type.</typeparam>
/// <typeparam name="TValue">Cache value type.</typeparam>
internal sealed class MemoryCache<TKey, TValue>
    where TValue : class
{
    private readonly Hashtable _cache;

    public MemoryCache(IEqualityComparer? comparer = null)
    {
        _cache = new Hashtable(comparer);
    }

    /// <summary>
    /// Gets the cached value corresponding to the given <paramref name="key"/> if already cached, or creates
    /// a new entry if not available.
    /// </summary>
    /// <param name="key">Cache key.</param>
    /// <param name="valueFactory">Factory method to create the value if it does not exist.</param>
    /// <returns>The value.</returns>
    [ContractAnnotation("key:null => halt;valueFactory:null => halt")]
    public TValue GetOrCreate(TKey key, [InstantHandle] Func<TValue> valueFactory)
    {
        AssertNotNull(key);
        AssertNotNull(valueFactory);

        // ReSharper disable once InconsistentlySynchronizedField, Justification: HashTable is thread safe for reading
        var cachedValue = (TValue?)_cache[key!];
        if (cachedValue is not null)
            return cachedValue;

        lock (_cache)
        {
            // Double check (init during lock wait)
            cachedValue = (TValue?)_cache[key!];
            if (cachedValue is not null)
                return cachedValue;

            cachedValue = valueFactory();
            _cache[key!] = cachedValue;
            return cachedValue;
        }
    }
}