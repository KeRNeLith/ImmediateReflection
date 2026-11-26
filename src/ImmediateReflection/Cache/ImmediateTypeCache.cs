using System;
using System.Reflection;

namespace ImmediateReflection;

/// <summary>
/// Cache of the <see cref="ImmediateType"/> instances created for a single <see cref="Type"/>, one per binding flags.
/// </summary>
/// <remarks>
/// A type is rarely used with more than a couple of binding flags, so a copy on write array is used.
/// It can be read without lock and does not require to build (and box) a composite key.
/// </remarks>
internal sealed class ImmediateTypeCache
{
    private sealed class Entry
    {
        public readonly BindingFlags Flags;
        public readonly ImmediateType ImmediateType;

        public Entry(BindingFlags flags, ImmediateType immediateType)
        {
            Flags = flags;
            ImmediateType = immediateType;
        }
    }

    private readonly Type _type;
    private readonly object _lock = new();

    // Never mutated, replaced as a whole when an entry is added
    private volatile Entry[] _entries = [];

    public ImmediateTypeCache(Type type)
    {
        _type = type;
    }

    /// <summary>
    /// Gets the <see cref="ImmediateType"/> corresponding to the given <paramref name="flags"/>
    /// or creates it if not already cached.
    /// </summary>
    public ImmediateType GetOrCreate(BindingFlags flags) => TryFind(_entries, flags) ?? Create(flags);

    private static ImmediateType? TryFind(Entry[] entries, BindingFlags flags)
    {
        // ReSharper disable once ForCanBeConvertedToForeach, Justification: Avoid enumerator in hot path
        for (int i = 0; i < entries.Length; ++i)
        {
            if (entries[i].Flags == flags)
                return entries[i].ImmediateType;
        }

        return null;
    }

    private ImmediateType Create(BindingFlags flags)
    {
        lock (_lock)
        {
            // Double check (init during lock wait)
            Entry[] entries = _entries;
            ImmediateType? type = TryFind(entries, flags);
            if (type is not null)
                return type;

            type = new ImmediateType(_type, flags);

            // Entries may have changed during the creation (nested creations)
            entries = _entries;
            ImmediateType? existing = TryFind(entries, flags);
            if (existing is not null)
                return existing;

            Entry[] newEntries = new Entry[entries.Length + 1];
            Array.Copy(entries, newEntries, entries.Length);
            newEntries[entries.Length] = new Entry(flags, type);
            _entries = newEntries;

            return type;
        }
    }
}
