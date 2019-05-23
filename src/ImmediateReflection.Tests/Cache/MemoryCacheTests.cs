#if SUPPORTS_SYSTEM_CACHING
using System;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="MemoryCache{TKey,TValue}"/>.
    /// </summary>
    [TestFixture]
    internal class MemoryCacheTests
    {
        [Test]
        public void MemoryCache_CtorThrows()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new MemoryCache<int>(null));
        }

        [Test]
        public void MemoryCacheGetOrCreate_Throws()
        {
            var cache = new MemoryCache<object>("TestCache");
            Assert.Throws<InvalidOperationException>(() => cache.GetOrCreate("MyKey", policy => null));
        }
    }
}
#endif