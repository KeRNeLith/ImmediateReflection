#if !SUPPORTS_LINQ
using System;
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
        [Test]
        public void First()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var emptyList = new List<int>();
            var list = new List<int> { 1, 2, 3, 4 };

            Assert.AreEqual(3, EnumerableUtils.First(list, value => value > 2));
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => EnumerableUtils.First(list, value => value > 5));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<InvalidOperationException>(() => EnumerableUtils.First(emptyList, value => true));
        }
    }
}
#endif