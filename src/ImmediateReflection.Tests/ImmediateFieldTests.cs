using System;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateField"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateFieldTests
    {
        [Test]
        public void ImmediateFieldInfo()
        {
            FieldInfo fieldInfo1 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField));
            var immediateField1 = new ImmediateField(fieldInfo1);
            Assert.AreEqual(fieldInfo1, immediateField1.FieldInfo);

            FieldInfo fieldInfo2 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2));
            var immediateField2 = new ImmediateField(fieldInfo2);
            Assert.AreNotEqual(immediateField1.FieldInfo, immediateField2.FieldInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => { var _ = new ImmediateField(null); });
        }

        [Test]
        public void ImmediateFieldEquality()
        {
            FieldInfo fieldInfo1 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField));
            var immediateField1 = new ImmediateField(fieldInfo1);
            var immediateField2 = new ImmediateField(fieldInfo1);
            Assert.AreEqual(immediateField1, immediateField2);
            Assert.IsTrue(immediateField1.Equals((object)immediateField2));
            Assert.IsFalse(immediateField1.Equals(null));

            FieldInfo fieldInfo2 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2));
            var immediateField3 = new ImmediateField(fieldInfo2);
            Assert.AreNotEqual(immediateField1, immediateField3);
            Assert.IsFalse(immediateField1.Equals((object)immediateField3));
        }

        [Test]
        public void ImmediateFieldHashCode()
        {
            FieldInfo fieldInfo1 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField));
            var immediateField1 = new ImmediateField(fieldInfo1);
            Assert.AreEqual(fieldInfo1.GetHashCode(), immediateField1.GetHashCode());

            FieldInfo fieldInfo2 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2));
            var immediateField2 = new ImmediateField(fieldInfo2);
            Assert.AreNotEqual(immediateField1.GetHashCode(), immediateField2.GetHashCode());
        }

        [Test]
        public void ImmediateFieldToString()
        {
            FieldInfo fieldInfo1 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField));
            var immediateField1 = new ImmediateField(fieldInfo1);
            Assert.AreEqual(fieldInfo1.ToString(), immediateField1.ToString());

            FieldInfo fieldInfo2 = typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2));
            var immediateField2 = new ImmediateField(fieldInfo2);
            Assert.AreNotEqual(immediateField1.ToString(), immediateField2.ToString());
        }
    }
}