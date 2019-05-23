using System;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="AttributesCache"/>.
    /// </summary>
    [TestFixture]
    internal class AttributesCacheTests
    {
        [Test]
        public void AttributeCache_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AttributesCache(null));
        }
    }
}