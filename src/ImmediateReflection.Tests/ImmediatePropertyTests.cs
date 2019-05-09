using System;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateProperty"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediatePropertyTests
    {
        [Test]
        public void ImmediatePropertyInfo()
        {
            PropertyInfo propertyInfo1 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet));
            var immediateProperty1 = new ImmediateProperty(propertyInfo1 ?? throw new AssertionException("Property does not exists."));
            Assert.AreEqual(propertyInfo1, immediateProperty1.PropertyInfo);

            PropertyInfo propertyInfo2 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet));
            var immediateProperty2 = new ImmediateProperty(propertyInfo2 ?? throw new AssertionException("Property does not exists."));
            Assert.AreNotEqual(immediateProperty1.PropertyInfo, immediateProperty2.PropertyInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateProperty(null));
        }

        [Test]
        public void ImmediatePropertyEquality()
        {
            PropertyInfo propertyInfo1 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet));
            var immediateProperty1 = new ImmediateProperty(propertyInfo1 ?? throw new AssertionException("Property does not exists."));
            var immediateProperty2 = new ImmediateProperty(propertyInfo1);
            Assert.AreEqual(immediateProperty1, immediateProperty2);
            Assert.IsTrue(immediateProperty1.Equals((object)immediateProperty2));
            Assert.IsFalse(immediateProperty1.Equals(null));

            PropertyInfo propertyInfo2 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet));
            var immediateProperty3 = new ImmediateProperty(propertyInfo2 ?? throw new AssertionException("Property does not exists."));
            Assert.AreNotEqual(immediateProperty1, immediateProperty3);
            Assert.IsFalse(immediateProperty1.Equals((object)immediateProperty3));
        }

        [Test]
        public void ImmediatePropertyHashCode()
        {
            PropertyInfo propertyInfo1 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet));
            var immediateProperty1 = new ImmediateProperty(propertyInfo1 ?? throw new AssertionException("Property does not exists."));
            Assert.AreEqual(propertyInfo1.GetHashCode(), immediateProperty1.GetHashCode());

            PropertyInfo propertyInfo2 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet));
            var immediateProperty2 = new ImmediateProperty(propertyInfo2 ?? throw new AssertionException("Property does not exists."));
            Assert.AreNotEqual(immediateProperty1.GetHashCode(), immediateProperty2.GetHashCode());
        }

        [Test]
        public void ImmediatePropertyToString()
        {
            PropertyInfo propertyInfo1 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet));
            var immediateProperty1 = new ImmediateProperty(propertyInfo1 ?? throw new AssertionException("Property does not exists."));
            Assert.AreEqual(propertyInfo1.ToString(), immediateProperty1.ToString());

            PropertyInfo propertyInfo2 = typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet));
            var immediateProperty2 = new ImmediateProperty(propertyInfo2 ?? throw new AssertionException("Property does not exists."));
            Assert.AreNotEqual(immediateProperty1.ToString(), immediateProperty2.ToString());
        }
    }
}