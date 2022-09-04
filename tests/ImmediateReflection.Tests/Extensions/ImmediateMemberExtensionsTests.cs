using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateMemberExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateMemberExtensionsTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        private class EmptyType
        {
        }

        #endregion

        [Test]
        public void GetImmediateField()
        {
            Type testType = typeof(PublicValueTypeTestClass);
            ImmediateField field = testType.GetImmediateField(nameof(PublicValueTypeTestClass._publicField));
            Assert.IsNotNull(field);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo, field.FieldInfo);

            field = testType.GetImmediateField("_privateField", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field);
            Assert.AreEqual(PublicValueTypePrivateFieldFieldsInfo, field.FieldInfo);

            field = testType.GetImmediateField("_notExists");
            Assert.IsNull(field);
        }

        [Test]
        public void GetImmediateFields()
        {
            TypeClassifiedMembers classifiedMember = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            Type testType = typeof(PublicValueTypeTestClass);
            IEnumerable<ImmediateField> fields = testType.GetImmediateFields();
            Assert.IsNotNull(fields);
            CollectionAssert.AreEquivalent(
                classifiedMember.PublicInstanceFields.Concat(classifiedMember.StaticFields).Concat(classifiedMember.ConstFields), 
                fields.Select(field => field.FieldInfo));

            fields = testType.GetImmediateFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(fields);
            CollectionAssert.AreEquivalent(
                classifiedMember.NonPublicInstanceFields,
                fields.Select(field => field.FieldInfo));

            testType = typeof(EmptyType);
            fields = testType.GetImmediateFields();
            Assert.IsNotNull(fields);
            CollectionAssert.IsEmpty(fields);
        }

        [Test]
        public void GetImmediateField_Throws()
        {
            Type testType = typeof(PublicValueTypeTestClass);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateField(null));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateField(null, BindingFlags.NonPublic));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void GetImmediateFieldFromType_Throws()
        {
            Type testType = null;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateField("Field"));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateField("Field", BindingFlags.NonPublic));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateFields());
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateFields(BindingFlags.NonPublic));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }


        [Test]
        public void GetImmediateProperty()
        {
            Type testType = typeof(PublicValueTypeTestClass);
            ImmediateProperty property = testType.GetImmediateProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet));
            Assert.IsNotNull(property);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo, property.PropertyInfo);

            property = testType.GetImmediateProperty("PrivatePropertyGetSet", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(property);
            Assert.AreEqual(PublicValueTypePrivateGetSetPropertyPropertyInfo, property.PropertyInfo);

            property = testType.GetImmediateProperty("NotExists");
            Assert.IsNull(property);
        }

        [Test]
        public void GetImmediateProperties()
        {
            TypeClassifiedMembers classifiedMember = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            Type testType = typeof(PublicValueTypeTestClass);
            IEnumerable<ImmediateProperty> properties = testType.GetImmediateProperties();
            Assert.IsNotNull(properties);
            CollectionAssert.AreEquivalent(
                classifiedMember.PublicInstanceProperties.Concat(classifiedMember.StaticProperties),
                properties.Select(property => property.PropertyInfo));

            properties = testType.GetImmediateProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(properties);
            CollectionAssert.AreEquivalent(
                classifiedMember.NonPublicInstanceProperties,
                properties.Select(property => property.PropertyInfo));

            testType = typeof(EmptyType);
            properties = testType.GetImmediateProperties();
            Assert.IsNotNull(properties);
            CollectionAssert.IsEmpty(properties);
        }

        [Test]
        public void GetImmediateProperty_Throws()
        {
            Type testType = typeof(PublicValueTypeTestClass);

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperty(null));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperty(null, BindingFlags.NonPublic));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void GetImmediatePropertyFromType_Throws()
        {
            Type testType = null;

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperty("Property"));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperty("Property", BindingFlags.NonPublic));
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperties());
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateProperties(BindingFlags.NonPublic));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}