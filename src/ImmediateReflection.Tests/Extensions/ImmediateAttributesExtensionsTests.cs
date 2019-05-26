#if SUPPORTS_EXTENSIONS && SUPPORTS_CACHING
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

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributeTestCases))]
        public void IsDefinedAndGetAttribute([NotNull] MemberInfo member, [NotNull] Type attributeType, bool inherit)
        {
            ImmediateMember immediateMember = GetImmediateMember(member);
            Assert.AreEqual(immediateMember.IsDefined(attributeType, inherit), member.IsDefinedImmediateAttribute(attributeType, inherit));
            Assert.AreEqual(immediateMember.GetAttribute(attributeType, inherit), member.GetImmediateAttribute(attributeType, inherit));
        }

        [Test]
        public void TemplateIsDefinedAndGetAttribute()
        {
            #region ImmediateType

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassNoAttribute), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassNoAttribute), true);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttribute), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttribute), true);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttributes), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassWithAttributes), true);

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassWithAttribute), false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassWithAttribute), true);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassNoAttribute), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassNoAttribute), true);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1), true);

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2), true);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassMultiAttributes), false);
            CheckHasAndGetAttribute<TestClassAttribute>(typeof(TestClassMultiAttributes), true);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassMultiAttributes), false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(typeof(TestClassMultiAttributes), true);

            #endregion

            #region ImmediateField

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldNoAttributeFieldInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldNoAttributeFieldInfo, true);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributeFieldInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributeFieldInfo, true);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributesFieldInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldAttributesFieldInfo, true);

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldAttributeFieldInfo, false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldAttributeFieldInfo, true);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldMultiAttributesFieldInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestFieldMultiAttributesFieldInfo, true);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo, false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo, true);

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyNoAttributePropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyNoAttributePropertyInfo, true);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributePropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributePropertyInfo, true);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributesPropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyAttributesPropertyInfo, true);

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo, false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo, true);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo, true);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo, true);

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo, true);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, false);
            CheckHasAndGetAttribute<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, true);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, false);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, true);

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(MemberInfo member, bool inherit)
                where TAttribute : Attribute
            {
                ImmediateMember immediateMember = GetImmediateMember(member);
                Assert.AreEqual(immediateMember.IsDefined<TAttribute>(inherit), member.IsDefinedImmediateAttribute<TAttribute>(inherit));
                Assert.AreEqual(immediateMember.GetAttribute<TAttribute>(inherit), member.GetImmediateAttribute<TAttribute>(inherit));
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
            Assert.Throws<ArgumentException>(() => member.IsDefinedImmediateAttribute(attributeType, inherit));
            Assert.Throws<ArgumentException>(() => member.GetImmediateAttribute(attributeType, inherit));
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
            Assert.Throws<ArgumentNullException>(() => type.IsDefinedImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => type.IsDefinedImmediateAttribute(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttribute(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => field.IsDefinedImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => field.IsDefinedImmediateAttribute(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttribute(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => property.IsDefinedImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttribute(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => property.IsDefinedImmediateAttribute(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttribute(typeof(TestClassAttribute), true));
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
            Assert.Throws<ArgumentNullException>(() => type.IsDefinedImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => type.IsDefinedImmediateAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => field.IsDefinedImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => field.IsDefinedImmediateAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => property.IsDefinedImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttribute(null));
            Assert.Throws<ArgumentNullException>(() => property.IsDefinedImmediateAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttribute(null, true));
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
                member.GetImmediateAttributes(attributeType, inherit));
        }

        [Test]
        public void TemplateGetAttributes()
        {
            #region ImmediateType

            // No attribute
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassNoAttribute), false);
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassNoAttribute), true);

            // With attribute
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttribute), false);
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttribute), true);

            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttributes), false);
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassWithAttributes), true);

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassWithAttribute), false);
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassWithAttribute), true);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassNoAttribute), false);
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassNoAttribute), true);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1), false);
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute1), true);

            // Attribute inherited 2
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2), false);
            CheckGetAttributes<TestClassAttribute>(typeof(InheritedTestClassWithAttribute2), true);

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassMultiAttributes), false);
            CheckGetAttributes<TestClassAttribute>(typeof(TestClassMultiAttributes), true);
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassMultiAttributes), false);
            CheckGetAttributes<SecondTestClassAttribute>(typeof(TestClassMultiAttributes), true);

            #endregion

            #region ImmediateField

            // No attribute
            CheckGetAttributes<TestClassAttribute>(TestFieldNoAttributeFieldInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestFieldNoAttributeFieldInfo, true);

            // With attribute
            CheckGetAttributes<TestClassAttribute>(TestFieldAttributeFieldInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestFieldAttributeFieldInfo, true);

            CheckGetAttributes<TestClassAttribute>(TestFieldAttributesFieldInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestFieldAttributesFieldInfo, true);

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldAttributeFieldInfo, false);
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldAttributeFieldInfo, true);

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(TestFieldMultiAttributesFieldInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestFieldMultiAttributesFieldInfo, true);
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo, false);
            CheckGetAttributes<SecondTestClassAttribute>(TestFieldMultiAttributesFieldInfo, true);

            #endregion

            #region ImmediateProperty

            // No attribute
            CheckGetAttributes<TestClassAttribute>(TestPropertyNoAttributePropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyNoAttributePropertyInfo, true);

            // With attribute
            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributePropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributePropertyInfo, true);

            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributesPropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyAttributesPropertyInfo, true);

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo, false);
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyAttributePropertyInfo, true);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedNoAttributePropertyInfo, true);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyInheritedAttribute1PropertyInfo, true);

            // Attribute inherited 2
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo, false);
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyInheritedAttribute2PropertyInfo, true);

            // Several attributes
            CheckGetAttributes<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, false);
            CheckGetAttributes<TestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, true);
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, false);
            CheckGetAttributes<SecondTestClassAttribute>(TestPropertyMultiAttributesPropertyInfo, true);

            #endregion

            #region Local function

            void CheckGetAttributes<TAttribute>(MemberInfo member, bool inherit)
                where TAttribute : Attribute
            {
                CollectionAssert.AreEqual(
                    GetImmediateMember(member).GetAttributes<TAttribute>(inherit),
                    member.GetImmediateAttributes<TAttribute>(inherit));
            }

            #endregion
        }

        [TestCaseSource(nameof(CreateWrongAttributeTestCases))]
        public void GetAttributes_WrongType([NotNull] MemberInfo member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => member.GetImmediateAttribute(attributeType, inherit));
        }

        [Test]
        public void GetAttributes_Throws_NullMember()
        {
            Type type = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttributes(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttributes(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttributes(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttributes(typeof(TestClassAttribute), true));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttributes(typeof(TestClassAttribute)));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttributes(typeof(TestClassAttribute), true));
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
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttributes(null));
            Assert.Throws<ArgumentNullException>(() => type.GetImmediateAttributes(null, true));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttributes(null));
            Assert.Throws<ArgumentNullException>(() => field.GetImmediateAttributes(null, true));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttributes(null));
            Assert.Throws<ArgumentNullException>(() => property.GetImmediateAttributes(null, true));
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
                member.GetAllImmediateAttributes(inherit));
        }

        [Test]
        public void GetAllAttributes_Throws_NullMember()
        {
            Type type = null;
            FieldInfo field = null;
            PropertyInfo property = null;
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => type.GetAllImmediateAttributes());
            Assert.Throws<ArgumentNullException>(() => type.GetAllImmediateAttributes(true));
            Assert.Throws<ArgumentNullException>(() => field.GetAllImmediateAttributes());
            Assert.Throws<ArgumentNullException>(() => field.GetAllImmediateAttributes(true));
            Assert.Throws<ArgumentNullException>(() => property.GetAllImmediateAttributes());
            Assert.Throws<ArgumentNullException>(() => property.GetAllImmediateAttributes(true));
            // ReSharper restore once AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
#endif