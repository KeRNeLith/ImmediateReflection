using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateFields"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateFieldsTests
    {
        [Test]
        public void ImmediateFieldsInfo()
        {
            var immediateFields1 = new ImmediateFields(typeof(SmallObject).GetFields());
            CollectionAssert.AreEquivalent(
                new []
                {
                    // ReSharper disable AssignNullToNotNullAttribute
                    new ImmediateField(typeof(SmallObject).GetField(nameof(SmallObject._testField1))),
                    new ImmediateField(typeof(SmallObject).GetField(nameof(SmallObject._testField2)))
                    // ReSharper restore AssignNullToNotNullAttribute
                },
                immediateFields1);

            var immediateFields2 = new ImmediateFields(typeof(SecondSmallObject).GetFields());
            CollectionAssert.AreNotEquivalent(immediateFields1, immediateFields2);

            var immediateFields3 = new ImmediateFields(new FieldInfo[]{});
            CollectionAssert.AreEqual(
                Enumerable.Empty<ImmediateProperty>(),
                immediateFields3);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateFields(null));
            Assert.Throws<ArgumentNullException>(() => new ImmediateFields(new []{ typeof(SmallObject).GetField(nameof(SmallObject._testField1)), null }));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void GetField()
        {
            var immediateFields = new ImmediateFields(typeof(SmallObject).GetFields());
            Assert.AreEqual(
                new ImmediateField(typeof(SmallObject).GetField(nameof(SmallObject._testField1))), 
                immediateFields[nameof(SmallObject._testField1)]);

            Assert.IsNull(immediateFields["NotExists"]);

            // ReSharper disable once InconsistentNaming
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => { var _ = immediateFields[null]; });
        }

        [Test]
        public void ImmediateFieldsEquality()
        {
            var fields = typeof(SmallObject).GetFields();
            var immediateFields1 = new ImmediateFields(fields);
            var immediateFields2 = new ImmediateFields(fields);
            Assert.AreEqual(immediateFields1, immediateFields2);
            Assert.IsTrue(immediateFields1.Equals((object)immediateFields2));
            Assert.IsFalse(immediateFields1.Equals(null));

            var immediateFields3 = new ImmediateFields(typeof(SecondSmallObject).GetFields());
            Assert.AreNotEqual(immediateFields1, immediateFields3);
            Assert.IsFalse(immediateFields1.Equals((object)immediateFields3));
        }

        [Test]
        public void ImmediateFieldsHashCode()
        {
            var fields = typeof(SmallObject).GetFields();
            var immediateFields1 = new ImmediateFields(fields);
            var immediateFields2 = new ImmediateFields(fields);
            Assert.AreEqual(immediateFields1.GetHashCode(), immediateFields2.GetHashCode());

            var immediateFields3 = new ImmediateFields(typeof(SecondSmallObject).GetFields());
            Assert.AreNotEqual(immediateFields1.GetHashCode(), immediateFields3.GetHashCode());
        }

        [Test]
        public void ImmediateFieldsToString()
        {
            var fields = typeof(SmallObject).GetFields();
            var immediateFields1 = new ImmediateFields(fields);
            string expectedToString = $"[{string.Join(", ", fields.Select(p => p.ToString()))}]";
            Assert.AreEqual(expectedToString, immediateFields1.ToString());

            var immediateFields2 = new ImmediateFields(new FieldInfo[]{});
            Assert.AreEqual("[]", immediateFields2.ToString());
        }
    }
}