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
        public void CreateDefaultConstructor_NullType()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateDefaultConstructor(null, out _));
        }

        [Test]
        public void CreateFieldGetter_NullField()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateGetter(null));
        }

        [Test]
        public void CreateFieldSetter_NullField()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateSetter(null));
        }

        [Test]
        public void CreatePropertyGetter_NullPropertyOrMethod()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateGetter(null, PublicValueTypePublicGetSetPropertyPropertyInfo.GetGetMethod()));
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, null));
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateGetter(null, null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void CreatePropertyGetter_CanRead()
        {
            Assert.IsNotNull(DelegatesFactory.CreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetGetMethod()));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.IsNull(DelegatesFactory.CreateGetter(PublicValueTypePublicSetPropertyPropertyInfo, null));
        }

        [Test]
        public void CreatePropertySetter_NullPropertyOrMethod()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateSetter(null, PublicValueTypePublicGetSetPropertyPropertyInfo.GetSetMethod()));
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, null));
            Assert.Throws<ArgumentNullException>(() => DelegatesFactory.CreateSetter(null, null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Test]
        public void CreatePropertySetter_CanWrite()
        {
            Assert.IsNotNull(DelegatesFactory.CreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetSetMethod()));
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.IsNull(DelegatesFactory.CreateSetter(PublicValueTypePublicGetPropertyPropertyInfo, null));
        }
    }
}