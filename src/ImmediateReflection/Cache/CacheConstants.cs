#if SUPPORTS_SYSTEM_CACHING
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Constants related to cache.
    /// </summary>
    internal static class CacheConstants
    {
        [NotNull]
        public const string TypesCacheName = "ImmediateTypesCache";
    }
}
#endif