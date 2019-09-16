#if !SUPPORTS_SYSTEM_CORE
using System.Collections.Generic;
using NUnit.Framework;
using ImmediateReflection.Utils;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="EnumerableUtils"/>.
    /// </summary>
    [TestFixture]
    internal class EnumerableUtilsTests
    {
        private static IEnumerable<int> GetInts()
        {
            yield return 4;
            yield return 5;
            yield return 6;
            yield return 7;
            yield return 8;
            yield return 9;
            yield return 10;
        }

        [Test]
        public void ToArray()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var emptyList = new List<int>();
            var list = new List<int> { 1, 2, 3, 4 };

            CollectionAssert.AreEqual(new int[] { }, EnumerableUtils.ToArray(emptyList));
            CollectionAssert.AreEqual(new [] { 1, 2, 3, 4 }, EnumerableUtils.ToArray(list));

            CollectionAssert.AreEqual(new int[] { }, EnumerableUtils.ToArray(EnumerableUtils.Empty<int>()));
            CollectionAssert.AreEqual(new[] { 4, 5, 6, 7, 8, 9, 10 }, EnumerableUtils.ToArray(GetInts()));
        }
    }
}
#endif