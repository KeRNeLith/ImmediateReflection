using System;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateField"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateFieldTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void ImmediateFieldInfo()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo, immediateField1.FieldInfo);

            var immediateField2 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1.FieldInfo, immediateField2.FieldInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateField(null));
        }

        [Test]
        public void ImmediateFieldEquality()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateField2 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(immediateField1, immediateField2);
            Assert.IsTrue(immediateField1.Equals((object)immediateField2));
            Assert.IsFalse(immediateField1.Equals(null));

            var immediateField3 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1, immediateField3);
            Assert.IsFalse(immediateField1.Equals((object)immediateField3));
        }

        [Test]
        public void ImmediateFieldHashCode()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo.GetHashCode(), immediateField1.GetHashCode());

            var immediateField2 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1.GetHashCode(), immediateField2.GetHashCode());
        }

        [Test]
        public void ImmediateFieldToString()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo.ToString(), immediateField1.ToString());

            var immediateField2 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1.ToString(), immediateField2.ToString());
        }
    }
}