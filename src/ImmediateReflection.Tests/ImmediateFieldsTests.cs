using System;
using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateFields"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateFieldsTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void ImmediateFieldsInfo()
        {
            var immediateFields1 = new ImmediateFields(SmallObjectFieldInfos);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    new ImmediateField(SmallObjectTestField1FieldInfo),
                    new ImmediateField(SmallObjectTestField2FieldInfo)
                },
                immediateFields1);

            var immediateFields2 = new ImmediateFields(SecondSmallObjectFieldInfos);
            CollectionAssert.AreNotEquivalent(immediateFields1, immediateFields2);

            var immediateFields3 = new ImmediateFields(EmptyFieldInfo);
            CollectionAssert.AreEqual(
                Enumerable.Empty<ImmediateProperty>(),
                immediateFields3);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateFields(null));
            Assert.Throws<ArgumentNullException>(() => new ImmediateFields(new[] { SmallObjectTestField1FieldInfo, null }));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void GetField()
        {
            var immediateFields = new ImmediateFields(SmallObjectFieldInfos);
            var expectedField = new ImmediateField(SmallObjectTestField1FieldInfo);
            Assert.AreEqual(expectedField, immediateFields[nameof(SmallObject._testField1)]);
            Assert.AreEqual(expectedField, immediateFields.GetField(nameof(SmallObject._testField1)));

            Assert.IsNull(immediateFields["NotExists"]);
            Assert.IsNull(immediateFields.GetField("NotExists"));

            // ReSharper disable InconsistentNaming
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateFields[null]; });
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateFields.GetField(null); });
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore InconsistentNaming
        }

        [Test]
        public void ImmediateFieldsEquality()
        {
            var immediateFields1 = new ImmediateFields(SmallObjectFieldInfos);
            var immediateFields2 = new ImmediateFields(SmallObjectFieldInfos);
            Assert.IsTrue(immediateFields1.Equals(immediateFields1));
            Assert.IsTrue(immediateFields1.Equals(immediateFields2));
            Assert.IsTrue(immediateFields1.Equals((object)immediateFields2));
            Assert.IsFalse(immediateFields1.Equals(null));

            var immediateFields3 = new ImmediateFields(SecondSmallObjectFieldInfos);
            Assert.IsFalse(immediateFields1.Equals(immediateFields3));
            Assert.IsFalse(immediateFields1.Equals((object)immediateFields3));

            var immediateFields4 = new ImmediateFields(PublicNestedFieldInfos);
            Assert.IsFalse(immediateFields4.Equals(immediateFields1));
            Assert.IsFalse(immediateFields4.Equals((object)immediateFields1));
        }

        [Test]
        public void ImmediateFieldsHashCode()
        {
            var immediateFields1 = new ImmediateFields(SmallObjectFieldInfos);
            var immediateFields2 = new ImmediateFields(SmallObjectFieldInfos);
            Assert.AreEqual(immediateFields1.GetHashCode(), immediateFields2.GetHashCode());

            var immediateFields3 = new ImmediateFields(SecondSmallObjectFieldInfos);
            Assert.AreNotEqual(immediateFields1.GetHashCode(), immediateFields3.GetHashCode());
        }

        [Test]
        public void ImmediateFieldsToString()
        {
            var immediateFields1 = new ImmediateFields(SmallObjectFieldInfos);
            string expectedToString = $"[{string.Join(", ", SmallObjectFieldInfos.Select(p => p.ToString()).ToArray())}]";
            Assert.AreEqual(expectedToString, immediateFields1.ToString());

            var immediateFields2 = new ImmediateFields(EmptyFieldInfo);
            Assert.AreEqual("[]", immediateFields2.ToString());
        }
    }
}