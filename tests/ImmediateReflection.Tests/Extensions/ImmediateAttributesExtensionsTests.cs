using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateAttributesExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateAttributesExtensionsTests : ImmediateAttributesTestsBase
    {
        #region Test helpers

        [Pure]
        [NotNull]
        private static ImmediateMember GetImmediateMember([NotNull] MemberInfo member)
        {
            if (member is Type type)
                return new ImmediateType(type);
            if (member is FieldInfo field)
                return new ImmediateField(field);
            if (member is PropertyInfo property)
                return new ImmediateProperty(property);
            throw new InvalidOperationException("Invalid given member, cannot be converted to an Immediate type.");
        }

        #endregion

        private static IEnumerable<TestCaseData> CreateGetAttributeTestCases
        {
            [UsedImplicitly]
            get
            {
                #region ImmediateType

                // No attribute
                yield return new TestCaseData(
                    typeof(TestClassNoAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassNoAttribute),
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassWithAttributes),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttributes),
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(SecondTestClassAttribute),
                    true);

                // Attribute not inherited
                yield return new TestCaseData(
                    typeof(InheritedTestClassNoAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassNoAttribute),
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 1
                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute1),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute1),
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 2
                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute2),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute2),
                    typeof(TestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassInheritedAttribute),
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassInheritedAttribute),
                    typeof(TestBaseAttribute),
                    true);

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    TestFieldInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    true);

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Attribute not inherited
                yield return new TestCaseData(
                    TestPropertyInheritedNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 1
                yield return new TestCaseData(
                    TestPropertyInheritedAttribute1PropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedAttribute1PropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 2
                yield return new TestCaseData(
                    TestPropertyInheritedAttribute2PropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedAttribute2PropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributeTestCases))]
        public void IsDefinedAndGetAttribute([NotNull] MemberInfo member, [NotNull] Type attributeType, bool inherit)
        {
            ImmediateMember immediateMember = GetImmediateMember(member);
            Assert.AreEqual(immediateMember.IsDefined(attributeType, inherit), ImmediateAttributesExtensions.IsDefinedImmediateAttribute(member, attributeType, inherit));
            Assert.AreEqual(immediateMember.GetAttribute(attributeType, inherit), ImmediateAttributesExtensions.GetImmediateAttribute(member, attributeType, inherit));
        }

        [Test]
        public void TemplateIsDefinedAndGetAttribute()
        {
            #region ImmediateType

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassNoAttribute));

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttribute));
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttributes));

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassWithAttribute));

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassNoAttribute));

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1));

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2));

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassMultiAttributes));
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassMultiAttributes));

            #endregion

            #region ImmediateField

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldNoAttributeFieldInfo);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributeFieldInfo);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributesFieldInfo);

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldAttributeFieldInfo);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldMultiAttributesFieldInfo);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo);

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyNoAttributePropertyInfo);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributePropertyInfo);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributesPropertyInfo);

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo);

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo);

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(MemberInfo member)
                where TAttribute : Attribute
            {
                CheckHasAndGetAttributeHelper(false);
                CheckHasAndGetAttributeHelper(true);

                #region Local function

                void CheckHasAndGetAttributeHelper(bool inherit)
                {
                    ImmediateMember immediateMember = GetImmediateMember(member);
                    Assert.AreEqual(immediateMember.IsDefined<TAttribute>(inherit), ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TAttribute>(member, inherit));
                    Assert.AreEqual(immediateMember.GetAttribute<TAttribute>(inherit), ImmediateAttributesExtensions.GetImmediateAttribute<TAttribute>(member, inherit));
                }

                #endregion
            }

            #endregion
        }

        [Test]
        public void TemplateIsDefinedAndGetAttribute_Inherited()
        {
            #region ImmediateType

            CheckHasAndGetAttribute<TestBaseAttribute>(typeof(TestClassOnlyInheritedAttribute));
            CheckHasAndGetAttribute<Attribute>(typeof(TestClassOnlyInheritedAttribute));
            CheckHasAndGetAttribute<TestBaseAttribute>(typeof(TestClassInheritedAttribute));

            #endregion

            #region ImmediateField

            CheckHasAndGetAttribute<TestBaseAttribute>(TestFieldOnlyInheritingAttributeFieldInfo);
            CheckHasAndGetAttribute<Attribute>(TestFieldOnlyInheritingAttributeFieldInfo);
            CheckHasAndGetAttribute<TestBaseAttribute>(TestFieldInheritingAttributeFieldInfo);

            #endregion

            #region ImmediateProperty

            CheckHasAndGetAttribute<TestBaseAttribute>(TestPropertyOnlyInheritingAttributePropertyInfo);
            CheckHasAndGetAttribute<Attribute>(TestPropertyOnlyInheritingAttributePropertyInfo);
            CheckHasAndGetAttribute<TestBaseAttribute>(TestPropertyInheritingAttributePropertyInfo);

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(MemberInfo member)
                where TAttribute : Attribute
            {
                CheckHasAndGetAttributeHelper(false);
                CheckHasAndGetAttributeHelper(true);

                #region Local function

                void CheckHasAndGetAttributeHelper(bool inherit)
                {
                    ImmediateMember immediateMember = GetImmediateMember(member);
                    Assert.AreEqual(immediateMember.IsDefined<TAttribute>(inherit), ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TAttribute>(member, inherit));
                    Assert.AreEqual(immediateMember.GetAttribute<TAttribute>(inherit), ImmediateAttributesExtensions.GetImmediateAttribute<TAttribute>(member, inherit));
                }

                #endregion
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
                    typeof(TestClassNoAttribute),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassNoAttribute),
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(FakeTestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(FakeTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(FakeTestClassAttribute),
                    true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateWrongAttributeTestCases))]
        public void IsDefinedAndGetAttribute_WrongType([NotNull] MemberInfo member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(member, attributeType, inherit));
            Assert.Throws<ArgumentException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(member, attributeType, inherit));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void IsDefinedAndGetAttribute_Throws_NullMember()
        {
            Type type = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(type, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(type, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(type, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(type, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(field, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(field, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(field, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(field, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(property, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(property, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(property, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(property, typeof(TestClassAttribute), true));

            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(type));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(type));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(type, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(type, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(field));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(field));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(field, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(field, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(property));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(property));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute<TestClassAttribute>(property, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute<TestClassAttribute>(property, true));
            // ReSharper restore once AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void IsDefinedAndGetAttribute_Throws_NullType()
        {
            Type type = typeof(PublicValueTypeTestClass);
            FieldInfo field = PublicValueTypePublicFieldFieldsInfo;
            PropertyInfo property = PublicValueTypePublicGetSetPropertyPropertyInfo;

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(type, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(type, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(type, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(type, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(field, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(field, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(field, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(field, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(property, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(property, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.IsDefinedImmediateAttribute(property, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttribute(property, null, true));
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
                    typeof(TestClassNoAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassNoAttribute),
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassWithAttributes),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttributes),
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassWithAttribute),
                    typeof(SecondTestClassAttribute),
                    true);

                // Attribute not inherited
                yield return new TestCaseData(
                    typeof(InheritedTestClassNoAttribute),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassNoAttribute),
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 1
                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute1),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute1),
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 2
                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute2),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(InheritedTestClassWithAttribute2),
                    typeof(TestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassMultiAttributes),
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassOnlyInheritedAttribute),
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    typeof(TestClassInheritedAttribute),
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    typeof(TestClassInheritedAttribute),
                    typeof(TestBaseAttribute),
                    true);

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldNoAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldAttributeFieldInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldMultiAttributesFieldInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    TestFieldOnlyInheritingAttributeFieldInfo,
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    TestFieldInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestFieldInheritingAttributeFieldInfo,
                    typeof(TestBaseAttribute),
                    true);

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // With attribute
                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Without requested attribute
                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyAttributePropertyInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Attribute not inherited
                yield return new TestCaseData(
                    TestPropertyInheritedNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedNoAttributePropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 1
                yield return new TestCaseData(
                    TestPropertyInheritedAttribute1PropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedAttribute1PropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Attribute inherited 2
                yield return new TestCaseData(
                    TestPropertyInheritedAttribute2PropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritedAttribute2PropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                // Several attributes
                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(TestClassAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(SecondTestClassAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyMultiAttributesPropertyInfo,
                    typeof(SecondTestClassAttribute),
                    true);

                // Inheriting attribute
                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(Attribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyOnlyInheritingAttributePropertyInfo,
                    typeof(Attribute),
                    true);

                yield return new TestCaseData(
                    TestPropertyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    false);

                yield return new TestCaseData(
                    TestPropertyInheritingAttributePropertyInfo,
                    typeof(TestBaseAttribute),
                    true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributesTestCases))]
        public void GetAttributes(
            [NotNull] MemberInfo member,
            [NotNull] Type attributeType,
            bool inherit)
        {
            CollectionAssert.AreEqual(
                GetImmediateMember(member).GetAttributes(attributeType, inherit),
                ImmediateAttributesExtensions.GetImmediateAttributes(member, attributeType, inherit));
        }

        [Test]
        public void TemplateGetAttributes()
        {
            #region ImmediateType

            // No attribute
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassNoAttribute));

            // With attribute
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttribute));

            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttributes));

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassWithAttribute));

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassNoAttribute));

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1));

            // Attribute inherited 2
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2));

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassMultiAttributes));
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassMultiAttributes));

            #endregion

            #region ImmediateField

            // No attribute
            CheckGetAttributes<TestClassAttribute>(TestFieldNoAttributeFieldInfo);

            // With attribute
            CheckGetAttributes<TestClassAttribute>(TestFieldAttributeFieldInfo);

            CheckGetAttributes<TestClassAttribute>(TestFieldAttributesFieldInfo);

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldAttributeFieldInfo);

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(TestFieldMultiAttributesFieldInfo);
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo);

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckGetAttributes<TestClassAttribute>(TestPropertyNoAttributePropertyInfo);

            // With attribute
            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributePropertyInfo);

            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributesPropertyInfo);

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo);

            // Attribute inherited 2
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo);

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo);
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo);

            #endregion

            #region Local function

            void CheckGetAttributes<TAttribute>(MemberInfo member)
                where TAttribute : Attribute
            {
                CheckGetAttributesHelper(false);
                CheckGetAttributesHelper(true);

                void CheckGetAttributesHelper(bool inherit)
                {
                    CollectionAssert.AreEqual(
                        GetImmediateMember(member).GetAttributes<TAttribute>(inherit),
                        ImmediateAttributesExtensions.GetImmediateAttributes<TAttribute>(member, inherit));
                }
            }

            #endregion
        }

        [Test]
        public void TemplateGetAttributes_Inherited()
        {
            #region ImmediateType

            CheckGetAttributes<TestBaseAttribute>(typeof(TestClassOnlyInheritedAttribute));
            CheckGetAttributes<Attribute>(typeof(TestClassOnlyInheritedAttribute));
            CheckGetAttributes<TestBaseAttribute>(typeof(TestClassInheritedAttribute));

            #endregion

            #region ImmediateField

            CheckGetAttributes<TestBaseAttribute>(TestFieldOnlyInheritingAttributeFieldInfo);
            CheckGetAttributes<Attribute>(TestFieldOnlyInheritingAttributeFieldInfo);
            CheckGetAttributes<TestBaseAttribute>(TestFieldInheritingAttributeFieldInfo);

            #endregion

            #region ImmediateProperty

            CheckGetAttributes<TestBaseAttribute>(TestPropertyOnlyInheritingAttributePropertyInfo);
            CheckGetAttributes<Attribute>(TestPropertyOnlyInheritingAttributePropertyInfo);
            CheckGetAttributes<TestBaseAttribute>(TestPropertyInheritingAttributePropertyInfo);

            #endregion

            #region Local function

            void CheckGetAttributes<TAttribute>(MemberInfo member)
                where TAttribute : Attribute
            {
                CheckGetAttributesHelper(false);
                CheckGetAttributesHelper(true);

                #region Local function

                void CheckGetAttributesHelper(bool inherit)
                {
                    CollectionAssert.AreEqual(
                        GetImmediateMember(member).GetAttributes<TAttribute>(inherit),
                        ImmediateAttributesExtensions.GetImmediateAttributes<TAttribute>(member, inherit));
                }

                #endregion
            }

            #endregion
        }

        [TestCaseSource(nameof(CreateWrongAttributeTestCases))]
        public void GetAttributes_WrongType([NotNull] MemberInfo member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(member, attributeType, inherit));
        }

        [Test]
        public void GetAttributes_Throws_NullMember()
        {
            Type type = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(type, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(type, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(field, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(field, typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(property, typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(property, typeof(TestClassAttribute), true));

            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(type));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(type, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(field));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(field, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(property));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes<TestClassAttribute>(property, true));
            // ReSharper restore once AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void GetAttributes_Throws_NullType()
        {
            Type type = typeof(PublicValueTypeTestClass);
            FieldInfo field = PublicValueTypePublicFieldFieldsInfo;
            PropertyInfo property = PublicValueTypePublicGetSetPropertyPropertyInfo;

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(type, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(type, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(field, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(field, null, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(property, null));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetImmediateAttributes(property, null, true));
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
                yield return new TestCaseData(typeof(TestClassNoAttribute), false);
                yield return new TestCaseData(typeof(TestClassNoAttribute), true);

                // With attribute
                yield return new TestCaseData(typeof(TestClassWithAttribute), false);
                yield return new TestCaseData(typeof(TestClassWithAttribute), true);

                yield return new TestCaseData(typeof(TestClassWithAttributes), false);
                yield return new TestCaseData(typeof(TestClassWithAttributes), true);

                // Attribute not inherited
                yield return new TestCaseData(typeof(InheritedTestClassNoAttribute), false);
                yield return new TestCaseData(typeof(InheritedTestClassNoAttribute), true);

                // Attribute inherited 1
                yield return new TestCaseData(typeof(InheritedTestClassWithAttribute1), false);
                yield return new TestCaseData(typeof(InheritedTestClassWithAttribute1), true);

                // Attribute inherited 2
                yield return new TestCaseData(typeof(InheritedTestClassWithAttribute2), false);
                yield return new TestCaseData(typeof(InheritedTestClassWithAttribute2), true);

                // Several attributes
                yield return new TestCaseData(typeof(TestClassMultiAttributes), false);
                yield return new TestCaseData(typeof(TestClassMultiAttributes), true);

                yield return new TestCaseData(typeof(InheritedTestClassMultiAttributes), false);
                yield return new TestCaseData(typeof(InheritedTestClassMultiAttributes), true);

                // Inheriting attribute
                yield return new TestCaseData(typeof(TestClassOnlyInheritedAttribute), false);
                yield return new TestCaseData(typeof(TestClassOnlyInheritedAttribute), true);

                yield return new TestCaseData(typeof(TestClassInheritedAttribute), false);
                yield return new TestCaseData(typeof(TestClassInheritedAttribute), true);

                #endregion

                #region ImmediateField

                // No attribute
                yield return new TestCaseData(TestFieldNoAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldNoAttributeFieldInfo, true);

                // With attribute
                yield return new TestCaseData(TestFieldAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldAttributeFieldInfo, true);

                yield return new TestCaseData(TestFieldAttributesFieldInfo, false);
                yield return new TestCaseData(TestFieldAttributesFieldInfo, true);

                // Several attributes
                yield return new TestCaseData(TestFieldMultiAttributesFieldInfo, false);
                yield return new TestCaseData(TestFieldMultiAttributesFieldInfo, true);

                // Inheriting attribute
                yield return new TestCaseData(TestFieldOnlyInheritingAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldOnlyInheritingAttributeFieldInfo, true);

                yield return new TestCaseData(TestFieldInheritingAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldInheritingAttributeFieldInfo, true);

                #endregion

                #region ImmediateProperty

                // No attribute
                yield return new TestCaseData(TestPropertyNoAttributePropertyInfo, false);
                yield return new TestCaseData(TestPropertyNoAttributePropertyInfo, true);

                // With attribute
                yield return new TestCaseData(TestPropertyAttributePropertyInfo, false);
                yield return new TestCaseData(TestPropertyAttributePropertyInfo, true);

                yield return new TestCaseData(TestPropertyAttributesPropertyInfo, false);
                yield return new TestCaseData(TestPropertyAttributesPropertyInfo, true);

                // Attribute not inherited
                yield return new TestCaseData(TestPropertyInheritedNoAttributePropertyInfo, false);
                yield return new TestCaseData(TestPropertyInheritedNoAttributePropertyInfo, true);

                // Attribute inherited 1
                yield return new TestCaseData(TestPropertyInheritedAttribute1PropertyInfo, false);
                yield return new TestCaseData(TestPropertyInheritedAttribute1PropertyInfo, true);

                // Attribute inherited 2
                yield return new TestCaseData(TestPropertyInheritedAttribute2PropertyInfo, false);
                yield return new TestCaseData(TestPropertyInheritedAttribute2PropertyInfo, true);

                // Several attributes
                yield return new TestCaseData(TestPropertyMultiAttributesPropertyInfo, false);
                yield return new TestCaseData(TestPropertyMultiAttributesPropertyInfo, true);

                yield return new TestCaseData(TestPropertyInheritedMultiAttributesPropertyInfo, false);
                yield return new TestCaseData(TestPropertyInheritedMultiAttributesPropertyInfo, true);

                // Inheriting attribute
                yield return new TestCaseData(TestFieldOnlyInheritingAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldOnlyInheritingAttributeFieldInfo, true);

                yield return new TestCaseData(TestFieldInheritingAttributeFieldInfo, false);
                yield return new TestCaseData(TestFieldInheritingAttributeFieldInfo, true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAllAttributesTestCases))]
        public void GetAllAttributes(
            [NotNull] MemberInfo member,
            bool inherit)
        {
            CollectionAssert.AreEqual(
                GetImmediateMember(member).GetAllAttributes(inherit),
                ImmediateAttributesExtensions.GetAllImmediateAttributes(member, inherit));
        }

        [Test]
        public void GetAllAttributes_Throws_NullMember()
        {
            Type type = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(type));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(type, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(field));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(field, true));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(property));
            Assert.Throws<ArgumentNullException>(() => ImmediateAttributesExtensions.GetAllImmediateAttributes(property, true));
            // ReSharper restore once AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}