using System;
using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateProperties"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediatePropertiesTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void ImmediatePropertiesInfo()
        {
            var immediateProperties1 = new ImmediateProperties(SmallObjectPropertyInfos);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new ImmediateProperty(SmallObjectTestProperty1PropertyInfo),
                    new ImmediateProperty(SmallObjectTestProperty2PropertyInfo)
                },
                immediateProperties1);

            var immediateProperties2 = new ImmediateProperties(SecondSmallObjectPropertyInfos);
            CollectionAssert.AreNotEquivalent(immediateProperties1, immediateProperties2);

            var immediateProperties3 = new ImmediateProperties(EmptyPropertyInfo);
            CollectionAssert.AreEqual(
                Enumerable.Empty<ImmediateProperty>(),
                immediateProperties3);
        }

        [Test]
        public void ImmediatePropertiesInfoWithNew()
        {
            var immediateProperties = new ImmediateProperties(new[] { ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo, BaseClassPublicGetPropertyPropertyInfo });
            CollectionAssert.AreEquivalent(
                new[] { new ImmediateProperty(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo) },
                immediateProperties);

            immediateProperties = new ImmediateProperties(new[] { BaseClassPublicGetPropertyPropertyInfo, ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo });
            CollectionAssert.AreEquivalent(
                new[] { new ImmediateProperty(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo) },
                immediateProperties);
        }

        [Test]
        public void GetProperty()
        {
            var immediateProperties = new ImmediateProperties(SmallObjectPropertyInfos);
            var expectedProperty = new ImmediateProperty(SmallObjectTestProperty1PropertyInfo);
            Assert.AreEqual(expectedProperty, immediateProperties[nameof(SmallObject.TestProperty1)]);
            Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(SmallObject.TestProperty1)));

            Assert.IsNull(immediateProperties["NotExists"]);
            Assert.IsNull(immediateProperties.GetProperty("NotExists"));

            // ReSharper disable InconsistentNaming
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateProperties[null]; });
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateProperties.GetProperty(null); });
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore InconsistentNaming
        }

        [Test]
        public void GetPropertyWithNew()
        {
            var immediateProperties = new ImmediateProperties(new[] { ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo, BaseClassPublicGetPropertyPropertyInfo });
            var expectedProperty = new ImmediateProperty(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo);
            Assert.AreEqual(expectedProperty, immediateProperties[nameof(ChildTypeRedefinitionTestClass.Property)]);
            Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(ChildTypeRedefinitionTestClass.Property)));

            immediateProperties = new ImmediateProperties(new[] { BaseClassPublicGetPropertyPropertyInfo, ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo });
            Assert.AreEqual(expectedProperty, immediateProperties[nameof(ChildTypeRedefinitionTestClass.Property)]);
            Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(ChildTypeRedefinitionTestClass.Property)));
        }

        #region Equals/HashCode/ToString

        [Test]
        public void ImmediatePropertiesEquality()
        {
            var immediateProperties1 = new ImmediateProperties(SmallObjectPropertyInfos);
            var immediateProperties2 = new ImmediateProperties(SmallObjectPropertyInfos);
            Assert.IsTrue(immediateProperties1.Equals(immediateProperties1));
            Assert.IsTrue(immediateProperties1.Equals(immediateProperties2));
            Assert.IsTrue(immediateProperties1.Equals((object)immediateProperties2));
            Assert.IsFalse(immediateProperties1.Equals(null));

            var immediateProperties3 = new ImmediateProperties(SecondSmallObjectPropertyInfos);
            Assert.IsFalse(immediateProperties1.Equals(immediateProperties3));
            Assert.IsFalse(immediateProperties1.Equals((object)immediateProperties3));

            var immediateProperties4 = new ImmediateProperties(PublicNestedPropertyInfos);
            Assert.IsFalse(immediateProperties4.Equals(immediateProperties1));
            Assert.IsFalse(immediateProperties4.Equals((object)immediateProperties1));
        }

        [Test]
        public void ImmediatePropertiesHashCode()
        {
            var immediateProperties1 = new ImmediateProperties(SmallObjectPropertyInfos);
            var immediateProperties2 = new ImmediateProperties(SmallObjectPropertyInfos);
            Assert.AreEqual(immediateProperties1.GetHashCode(), immediateProperties2.GetHashCode());

            var immediateProperties3 = new ImmediateProperties(SecondSmallObjectPropertyInfos);
            Assert.AreNotEqual(immediateProperties1.GetHashCode(), immediateProperties3.GetHashCode());
        }

        [Test]
        public void ImmediatePropertiesToString()
        {
            var immediateProperties1 = new ImmediateProperties(SmallObjectPropertyInfos);
            string expectedToString = $"[{string.Join(", ", SmallObjectPropertyInfos.Select(p => p.ToString()).ToArray())}]";
            Assert.AreEqual(expectedToString, immediateProperties1.ToString());

            var immediateProperties2 = new ImmediateProperties(EmptyPropertyInfo);
            Assert.AreEqual("[]", immediateProperties2.ToString());
        }

        #endregion
    }
}