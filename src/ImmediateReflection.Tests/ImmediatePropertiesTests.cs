using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateProperties"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediatePropertiesTests
    {
        [Test]
        public void ImmediatePropertiesInfo()
        {
            var immediateProperties1 = new ImmediateProperties(typeof(SmallObject).GetProperties());
            CollectionAssert.AreEquivalent(
                new []
                {
                    // ReSharper disable AssignNullToNotNullAttribute
                    new ImmediateProperty(typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1))),
                    new ImmediateProperty(typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty2)))
                    // ReSharper restore AssignNullToNotNullAttribute
                },
                immediateProperties1);

            var immediateProperties2 = new ImmediateProperties(typeof(SecondSmallObject).GetProperties());
            CollectionAssert.AreNotEquivalent(immediateProperties1, immediateProperties2);

            var immediateProperties3 = new ImmediateProperties(new PropertyInfo[]{});
            CollectionAssert.AreEqual(
                Enumerable.Empty<ImmediateProperty>(),
                immediateProperties3);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateProperties(null));
            Assert.Throws<ArgumentNullException>(() => new ImmediateProperties(new []{ typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1)), null }));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void GetProperty()
        {
            var immediateProperties = new ImmediateProperties(typeof(SmallObject).GetProperties());
            Assert.AreEqual(
                // ReSharper disable once AssignNullToNotNullAttribute
                new ImmediateProperty(typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1))), 
                immediateProperties[nameof(SmallObject.TestProperty1)]);

            Assert.IsNull(immediateProperties["NotExists"]);

            // ReSharper disable once InconsistentNaming
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateProperties[null]; });
        }

        [Test]
        public void ImmediatePropertiesEquality()
        {
            var properties = typeof(SmallObject).GetProperties();
            var immediateProperties1 = new ImmediateProperties(properties);
            var immediateProperties2 = new ImmediateProperties(properties);
            Assert.AreEqual(immediateProperties1, immediateProperties2);
            Assert.IsTrue(immediateProperties1.Equals((object)immediateProperties2));
            Assert.IsFalse(immediateProperties1.Equals(null));

            var immediateProperties3 = new ImmediateProperties(typeof(SecondSmallObject).GetProperties());
            Assert.AreNotEqual(immediateProperties1, immediateProperties3);
            Assert.IsFalse(immediateProperties1.Equals((object)immediateProperties3));
        }

        [Test]
        public void ImmediatePropertiesHashCode()
        {
            var properties = typeof(SmallObject).GetProperties();
            var immediateProperties1 = new ImmediateProperties(properties);
            var immediateProperties2 = new ImmediateProperties(properties);
            Assert.AreEqual(immediateProperties1.GetHashCode(), immediateProperties2.GetHashCode());

            var immediateProperties3 = new ImmediateProperties(typeof(SecondSmallObject).GetProperties());
            Assert.AreNotEqual(immediateProperties1.GetHashCode(), immediateProperties3.GetHashCode());
        }

        [Test]
        public void ImmediatePropertyToString()
        {
            var properties = typeof(SmallObject).GetProperties();
            var immediateProperties1 = new ImmediateProperties(properties);
            string expectedToString = $"[{string.Join(", ", properties.Select(p => p.ToString()))}]";
            Assert.AreEqual(expectedToString, immediateProperties1.ToString());

            var immediateProperties2 = new ImmediateProperties(new PropertyInfo[]{});
            Assert.AreEqual("[]", immediateProperties2.ToString());
        }
    }
}