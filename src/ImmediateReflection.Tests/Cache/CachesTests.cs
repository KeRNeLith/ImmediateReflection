using System;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to cached information.
    /// </summary>
    [TestFixture]
    internal class CachesTests : ImmediateAttributesTestsBase
    {
        [Test]
        public void AttributeCache_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new AttributesCache(null));
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetAttributesCache(null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void DefaultConstructor_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetDefaultConstructor(null));
        }

        [Test]
        public void GetProperty_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetProperty(null));
        }

        [Test]
        public void GetField_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetField(null));
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetField(PublicValueTypePublicFieldFieldsInfo, null));
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetField(null, typeof(TestEnum)));
            Assert.Throws<ArgumentNullException>(() => CachesHandler.Instance.GetField(null, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }
    }
}