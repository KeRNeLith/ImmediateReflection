using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateMember"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateMemberTests : ImmediateAttributesTestsBase
    {
        private static IEnumerable<TestCaseData> CreateGetAttributeTestCases
        {
            [UsedImplicitly]
            get
            {
                #region ImmediateType

                // No attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(1));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(1));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(4));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(4));

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(1));

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(11));

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(11));

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(13));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(13));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    false,
                    new SecondTestClassAttribute(1));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    true,
                    new SecondTestClassAttribute(1));

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    false,
                    new TestInheritingAttribute(17));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    true,
                    new TestInheritingAttribute(17));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    false,
                    new TestBaseAttribute(22));

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    true,
                    new TestBaseAttribute(22));

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(2));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(2));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(7));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(7));

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(14));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(14));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new SecondTestClassAttribute(2));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new SecondTestClassAttribute(2));

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new TestInheritingAttribute(19));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new TestInheritingAttribute(19));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new TestBaseAttribute(20));

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new TestBaseAttribute(20));

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(3));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(3));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(9));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(9));

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(3));

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(12));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(12));

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute(15));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute(15));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new SecondTestClassAttribute(3));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new SecondTestClassAttribute(3));

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new TestInheritingAttribute(25));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new TestInheritingAttribute(25));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new TestBaseAttribute(26));

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new TestBaseAttribute(26));

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributeTestCases))]
        public void IsDefinedAndGetAttribute([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit, [CanBeNull] Attribute expectedAttribute)
        {
            if (expectedAttribute is null)
                Assert.IsFalse(member.IsDefined(attributeType, inherit));
            else
                Assert.IsTrue(member.IsDefined(attributeType, inherit));

            Assert.AreEqual(expectedAttribute, member.GetAttribute(attributeType, inherit));
        }

        [Test]
        public void TemplateIsDefinedAndGetAttribute()
        {
            #region ImmediateType

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), true, null);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, new TestClassAttribute(1));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, new TestClassAttribute(1));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttributes)), false, new TestClassAttribute(4));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttributes)), true, new TestClassAttribute(4));

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, null);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, null);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), true, null);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), true, new TestClassAttribute(1));

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), false, new TestClassAttribute(11));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), true, new TestClassAttribute(11));

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), false, new TestClassAttribute(13));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), true, new TestClassAttribute(13));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), false, new SecondTestClassAttribute(1));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), true, new SecondTestClassAttribute(1));

            #endregion

            #region ImmediateField

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), true, null);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, new TestClassAttribute(2));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, new TestClassAttribute(2));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributesFieldInfo), false, new TestClassAttribute(7));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributesFieldInfo), true, new TestClassAttribute(7));

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, null);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, null);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new TestClassAttribute(14));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new TestClassAttribute(14));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new SecondTestClassAttribute(2));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new SecondTestClassAttribute(2));

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyNoAttributePropertyInfo), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyNoAttributePropertyInfo), true, null);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), false, new TestClassAttribute(3));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), true, new TestClassAttribute(3));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyAttributesPropertyInfo), false, new TestClassAttribute(9));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyAttributesPropertyInfo), true, new TestClassAttribute(9));

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), false, null);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), true, null);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo), true, null);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo), true, new TestClassAttribute(3));

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo), false, new TestClassAttribute(12));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo), true, new TestClassAttribute(12));

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), false, new TestClassAttribute(15));
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), true, new TestClassAttribute(15));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), false, new SecondTestClassAttribute(3));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), true, new SecondTestClassAttribute(3));

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(ImmediateMember member, bool inherit, Attribute expectedAttribute)
                where TAttribute : Attribute
            {
                if (expectedAttribute is null)
                    Assert.IsFalse(member.IsDefined<TAttribute>(inherit));
                else
                    Assert.IsTrue(member.IsDefined<TAttribute>(inherit));
                Assert.AreEqual(expectedAttribute, member.GetAttribute<TAttribute>(inherit));
            }

            #endregion
        }

        [Test]
        public void TemplateIsDefinedAndGetAttribute_Inherited()
        {
            #region ImmediateType

            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateType(typeof(TestClassOnlyInheritedAttribute)), false, new TestInheritingAttribute(17));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateType(typeof(TestClassOnlyInheritedAttribute)), true, new TestInheritingAttribute(17));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateType(typeof(TestClassInheritedAttribute)), false, new TestBaseAttribute(22));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateType(typeof(TestClassInheritedAttribute)), true, new TestBaseAttribute(22));

            #endregion

            #region ImmediateField

            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo), false, new TestInheritingAttribute(19));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo), true, new TestInheritingAttribute(19));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateField(TestFieldInheritingAttributeFieldInfo), false, new TestBaseAttribute(20));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateField(TestFieldInheritingAttributeFieldInfo), true, new TestBaseAttribute(20));

            #endregion

            #region ImmediateProperty

            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo), false, new TestInheritingAttribute(25));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo), true, new TestInheritingAttribute(25));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo), false, new TestBaseAttribute(26));
            CheckHasAndGetAttribute<TestBaseAttribute>(new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo), true, new TestBaseAttribute(26));

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(ImmediateMember member, bool inherit, Attribute expectedAttribute)
                where TAttribute : Attribute
            {
                if (expectedAttribute is null)
                    Assert.IsFalse(member.IsDefined<TAttribute>(inherit));
                else
                    Assert.IsTrue(member.IsDefined<TAttribute>(inherit));
                Assert.AreEqual(expectedAttribute, member.GetAttribute<TAttribute>(inherit));
            }

            #endregion
        }

        private static IEnumerable<TestCaseData> CreateWrongAttributeTestCases
        {
            [UsedImplicitly]
            get
            {
                #region ImmediateType

                // No attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateWrongAttributeTestCases))]
        public void IsDefinedAndGetAttribute_WrongType([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => member.IsDefined(attributeType, inherit));
            Assert.Throws<ArgumentException>(() => member.GetAttribute(attributeType, inherit));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void IsDefinedAndGetAttribute_Throws_NullType()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.IsDefined(null));
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateType.IsDefined(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateField.IsDefined(null));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateField.IsDefined(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.IsDefined(null));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.IsDefined(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.GetAttribute(null, true));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        private static IEnumerable<TestCaseData> CreateGetAttributesTestCases
        {
            [UsedImplicitly]
            get
            {
                #region ImmediateType

                // No attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(4), new TestClassAttribute(5) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(4), new TestClassAttribute(5) });

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(1) });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(11) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(1), new TestClassAttribute(11) });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(13) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(13) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    false,
                    new[] { new SecondTestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    true,
                    new[] { new SecondTestClassAttribute(1) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestInheritingAttribute(17) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestInheritingAttribute(17) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(7), new TestClassAttribute(8) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(7), new TestClassAttribute(8) });

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(14) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(14) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new[] { new SecondTestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new[] { new SecondTestClassAttribute(2) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestInheritingAttribute(19) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestInheritingAttribute(19) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(9), new TestClassAttribute(10) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(9), new TestClassAttribute(10) });

                // Without requested attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    null);

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(3) });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(12) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(3), new TestClassAttribute(12) });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(15) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(15) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new[] { new SecondTestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new[] { new SecondTestClassAttribute(3) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestInheritingAttribute(25) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestInheritingAttribute(25) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    false,
                    new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    typeof(TestBaseAttribute),
                    true,
                    new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributesTestCases))]
        public void GetAttributes(
            [NotNull] ImmediateMember member,
            [NotNull] Type attributeType,
            bool inherit,
            [CanBeNull, ItemNotNull] IEnumerable<Attribute> expectedAttributes)
        {
            if (expectedAttributes is null)
                CollectionAssert.IsEmpty(member.GetAttributes(attributeType, inherit));
            else
                CollectionAssert.AreEquivalent(expectedAttributes, member.GetAttributes(attributeType, inherit));
        }

        [Test]
        public void TemplateGetAttributes()
        {
            #region ImmediateType

            // No attribute
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), true, null);

            // With attribute
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttribute)), false, new[] { new TestClassAttribute(1) });
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttribute)), true, new[] { new TestClassAttribute(1) });

            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttributes)), false, new[] { new TestClassAttribute(4), new TestClassAttribute(5) });
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttributes)), true, new[] { new TestClassAttribute(4), new TestClassAttribute(5) });

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, null);
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, null);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), true, null);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), false, null);
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), true, new[] { new TestClassAttribute(1) });

            // Attribute inherited 2
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), false, new[] { new TestClassAttribute(11) });
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), true, new[] { new TestClassAttribute(1), new TestClassAttribute(11) });

            // Several attributes
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), false, new[] { new TestClassAttribute(13) });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), true, new[] { new TestClassAttribute(13) });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), false, new[] { new SecondTestClassAttribute(1) });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), true, new[] { new SecondTestClassAttribute(1) });

            #endregion

            #region ImmediateField

            // No attribute
            CheckGetAttributes<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), true, null);

            // With attribute
            CheckGetAttributes(new ImmediateField(TestFieldAttributeFieldInfo), false, new[] { new TestClassAttribute(2) });
            CheckGetAttributes(new ImmediateField(TestFieldAttributeFieldInfo), true, new[] { new TestClassAttribute(2) });

            CheckGetAttributes(new ImmediateField(TestFieldAttributesFieldInfo), false, new[] { new TestClassAttribute(7), new TestClassAttribute(8) });
            CheckGetAttributes(new ImmediateField(TestFieldAttributesFieldInfo), true, new[] { new TestClassAttribute(7), new TestClassAttribute(8) });

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, null);
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, null);

            // Several attributes
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new[] { new TestClassAttribute(14) });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new[] { new TestClassAttribute(14) });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new[] { new SecondTestClassAttribute(2) });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new[] { new SecondTestClassAttribute(2) });

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckGetAttributes<TestClassAttribute>(new ImmediateProperty(TestPropertyNoAttributePropertyInfo), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateProperty(TestPropertyNoAttributePropertyInfo), true, null);

            // With attribute
            CheckGetAttributes(new ImmediateProperty(TestPropertyAttributePropertyInfo), false, new[] { new TestClassAttribute(3) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyAttributePropertyInfo), true, new[] { new TestClassAttribute(3) });

            CheckGetAttributes(new ImmediateProperty(TestPropertyAttributesPropertyInfo), false, new[] { new TestClassAttribute(9), new TestClassAttribute(10) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyAttributesPropertyInfo), true, new[] { new TestClassAttribute(9), new TestClassAttribute(10) });

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), false, null);
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateProperty(TestPropertyAttributePropertyInfo), true, null);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo), true, null);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo), false, null);
            CheckGetAttributes(new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo), true, new[] { new TestClassAttribute(3) });

            // Attribute inherited 2
            CheckGetAttributes(new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo), false, new[] { new TestClassAttribute(12) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo), true, new[] { new TestClassAttribute(3), new TestClassAttribute(12) });

            // Several attributes
            CheckGetAttributes(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), false, new[] { new TestClassAttribute(15) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), true, new[] { new TestClassAttribute(15) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), false, new[] { new SecondTestClassAttribute(3) });
            CheckGetAttributes(new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo), true, new[] { new SecondTestClassAttribute(3) });

            #endregion

            #region Local function

            void CheckGetAttributes<TAttribute>(ImmediateMember member, bool inherit, IEnumerable<TAttribute> expectedAttributes)
                where TAttribute : Attribute
            {
                if (expectedAttributes is null)
                    CollectionAssert.IsEmpty(member.GetAttributes<TAttribute>(inherit));
                else
                    CollectionAssert.AreEquivalent(expectedAttributes, member.GetAttributes<TAttribute>(inherit));
            }

            #endregion
        }

        [Test]
        public void TemplateGetAttributes_Inherited()
        {
            #region ImmediateType

            CheckGetAttributes<TestBaseAttribute>(new ImmediateType(typeof(TestClassOnlyInheritedAttribute)), false, new[] { new TestInheritingAttribute(17) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateType(typeof(TestClassOnlyInheritedAttribute)), true, new[] { new TestInheritingAttribute(17) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateType(typeof(TestClassInheritedAttribute)), false, new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateType(typeof(TestClassInheritedAttribute)), true, new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });

            #endregion

            #region ImmediateField

            CheckGetAttributes<TestBaseAttribute>(new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo), false, new[] { new TestInheritingAttribute(19) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo), true, new[] { new TestInheritingAttribute(19) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateField(TestFieldInheritingAttributeFieldInfo), false, new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateField(TestFieldInheritingAttributeFieldInfo), true, new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });

            #endregion

            #region ImmediateProperty

            CheckGetAttributes<TestBaseAttribute>(new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo), false, new[] { new TestInheritingAttribute(25) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo), true, new[] { new TestInheritingAttribute(25) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo), false, new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });
            CheckGetAttributes<TestBaseAttribute>(new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo), true, new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });

            #endregion

            #region Local function

            void CheckGetAttributes<TAttribute>(ImmediateMember member, bool inherit, IEnumerable<TAttribute> expectedAttributes)
                where TAttribute : Attribute
            {
                if (expectedAttributes is null)
                    CollectionAssert.IsEmpty(member.GetAttributes<TAttribute>(inherit));
                else
                    CollectionAssert.AreEquivalent(expectedAttributes, member.GetAttributes<TAttribute>(inherit));
            }

            #endregion
        }

        [TestCaseSource(nameof(CreateWrongAttributeTestCases))]
        public void GetAttributes_WrongType([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => member.GetAttributes(attributeType, inherit));
        }

        [Test]
        public void GetAttributes_Throws_NullType()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttributes(null));
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttributes(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttributes(null));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttributes(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.GetAttributes(null));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.GetAttributes(null, true));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        private static IEnumerable<TestCaseData> CreateGetAllAttributesTestCases
        {
            [UsedImplicitly]
            get
            {
                #region ImmediateType

                // No attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassNoAttribute)),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    false,
                    new[] { new TestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    true,
                    new[] { new TestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    false,
                    new[] { new TestClassAttribute(4), new TestClassAttribute(5) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    true,
                    new[] { new TestClassAttribute(4), new TestClassAttribute(5) });

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassNoAttribute)),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute1)),
                    true,
                    new[] { new TestClassAttribute(1) });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    false,
                    new[] { new TestClassAttribute(11) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    true,
                    new[] { new TestClassAttribute(1), new TestClassAttribute(11) });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    false,
                    new Attribute[] { new TestClassAttribute(13), new SecondTestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    true,
                    new Attribute[] { new TestClassAttribute(13), new SecondTestClassAttribute(1) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassMultiAttributes)),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassMultiAttributes)),
                    true,
                    new Attribute[] { new TestClassAttribute(13), new SecondTestClassAttribute(1) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    false,
                    new Attribute[] { new ThirdTestClassAttribute(16), new TestInheritingAttribute(17) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassOnlyInheritedAttribute)),
                    true,
                    new Attribute[] { new ThirdTestClassAttribute(16), new TestInheritingAttribute(17) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    false,
                    new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassInheritedAttribute)),
                    true,
                    new[] { new TestBaseAttribute(22), new TestInheritingAttribute(23) });

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateField(TestFieldNoAttributeFieldInfo),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    false,
                    new[] { new TestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    true,
                    new[] { new TestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    false,
                    new[] { new TestClassAttribute(7), new TestClassAttribute(8) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    true,
                    new[] { new TestClassAttribute(7), new TestClassAttribute(8) });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    false,
                    new Attribute[] { new TestClassAttribute(14), new SecondTestClassAttribute(2) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    true,
                    new Attribute[] { new TestClassAttribute(14), new SecondTestClassAttribute(2) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    false,
                    new Attribute[] { new ThirdTestClassAttribute(18), new TestInheritingAttribute(19) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldOnlyInheritingAttributeFieldInfo),
                    true,
                    new Attribute[] { new ThirdTestClassAttribute(18), new TestInheritingAttribute(19) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    false,
                    new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldInheritingAttributeFieldInfo),
                    true,
                    new[] { new TestBaseAttribute(20), new TestInheritingAttribute(21) });

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyNoAttributePropertyInfo),
                    true,
                    null);

                // With attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    false,
                    new[] { new TestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributePropertyInfo),
                    true,
                    new[] { new TestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    false,
                    new[] { new TestClassAttribute(9), new TestClassAttribute(10) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyAttributesPropertyInfo),
                    true,
                    new[] { new TestClassAttribute(9), new TestClassAttribute(10) });

                // Attribute not inherited
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedNoAttributePropertyInfo),
                    true,
                    null);

                // Attribute inherited 1
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute1PropertyInfo),
                    true,
                    new[] { new TestClassAttribute(3) });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    false,
                    new[] { new TestClassAttribute(12) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedAttribute2PropertyInfo),
                    true,
                    new[] { new TestClassAttribute(3), new TestClassAttribute(12) });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    false,
                    new Attribute[] { new TestClassAttribute(15), new SecondTestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyMultiAttributesPropertyInfo),
                    true,
                    new Attribute[] { new TestClassAttribute(15), new SecondTestClassAttribute(3) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedMultiAttributesPropertyInfo),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritedMultiAttributesPropertyInfo),
                    true,
                    new Attribute[] { new TestClassAttribute(15), new SecondTestClassAttribute(3) });

                // Inheriting attribute
                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    false,
                    new Attribute[] { new ThirdTestClassAttribute(24), new TestInheritingAttribute(25) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyOnlyInheritingAttributePropertyInfo),
                    true,
                    new Attribute[] { new ThirdTestClassAttribute(24), new TestInheritingAttribute(25) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    false,
                    new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });

                yield return new TestCaseData(
                    new ImmediateProperty(TestPropertyInheritingAttributePropertyInfo),
                    true,
                    new[] { new TestBaseAttribute(26), new TestInheritingAttribute(27) });

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAllAttributesTestCases))]
        public void GetAllAttributes(
            [NotNull] ImmediateMember member,
            bool inherit,
            [CanBeNull, ItemNotNull] IEnumerable<Attribute> expectedAttributes)
        {
            if (expectedAttributes is null)
                CollectionAssert.IsEmpty(member.GetAllAttributes(inherit));
            else
                CollectionAssert.AreEquivalent(expectedAttributes, member.GetAllAttributes(inherit));
        }
    }
}