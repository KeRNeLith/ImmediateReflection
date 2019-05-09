using System;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateProperty"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediatePropertyTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void ImmediatePropertyInfo()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo, immediateProperty1.PropertyInfo);

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.PropertyInfo, immediateProperty2.PropertyInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateProperty(null));
        }

        [Test]
        public void ImmediatePropertyEquality()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(immediateProperty1, immediateProperty2);
            Assert.IsTrue(immediateProperty1.Equals((object)immediateProperty2));
            Assert.IsFalse(immediateProperty1.Equals(null));

            var immediateProperty3 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1, immediateProperty3);
            Assert.IsFalse(immediateProperty1.Equals((object)immediateProperty3));
        }

        [Test]
        public void ImmediatePropertyHashCode()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.GetHashCode(), immediateProperty1.GetHashCode());

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.GetHashCode(), immediateProperty2.GetHashCode());
        }

        [Test]
        public void ImmediatePropertyToString()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.ToString(), immediateProperty1.ToString());

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.ToString(), immediateProperty2.ToString());
        }
    }
}