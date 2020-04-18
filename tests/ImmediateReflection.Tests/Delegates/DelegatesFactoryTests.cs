using System;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="DelegatesFactory"/>.
    /// </summary>
    [TestFixture]
    internal class DelegatesFactoryTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void CreatePropertyGetter_CanRead()
        {
            Assert.IsNull(DelegatesFactory.CreateGetter(PublicValueTypePublicSetPropertyPropertyInfo, PublicValueTypePublicSetPropertyPropertyInfo.GetGetMethod()));
            Assert.IsNotNull(DelegatesFactory.CreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetGetMethod()));
        }

        [Test]
        public void CreatePropertySetter_CanWrite()
        {
            Assert.IsNull(DelegatesFactory.CreateSetter(PublicValueTypePublicGetPropertyPropertyInfo, PublicValueTypePublicGetPropertyPropertyInfo.GetSetMethod()));
            Assert.IsNotNull(DelegatesFactory.CreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetSetMethod()));
        }
    }
}