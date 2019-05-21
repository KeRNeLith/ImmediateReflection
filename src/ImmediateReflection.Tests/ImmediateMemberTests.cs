using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateMember"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateMemberTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        #region Attributes

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        private class TestClassAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        private class SecondTestClassAttribute : Attribute
        {
        }

        private class FakeTestClassAttribute
        {
        }

        #endregion

        #region Classes

        // ReSharper disable InconsistentNaming

        private class TestClassNoAttribute
        {
            public int _testField = 42;
        }

        [TestClass]
        private class TestClassWithAttribute
        {
            [TestClass]
            public int _testField = 42;
        }

        [TestClass]
        [TestClass]
        private class TestClassWithAttributes
        {
            [TestClass]
            [TestClass]
            public int _testField = 42;
        }

        private class InheritedTestClassNoAttribute : TestClassNoAttribute
        {
        }

        private class InheritedTestClassWithAttribute1 : TestClassWithAttribute
        {
        }

        [TestClass]
        private class InheritedTestClassWithAttribute2 : TestClassWithAttribute
        {
        }

        [TestClass]
        [SecondTestClass]
        private class TestClassMultiAttributes
        {
            [TestClass]
            [SecondTestClass]
            public int _testField = 42;
        }

        private class InheritedTestClassMultiAttributes : TestClassMultiAttributes
        {
        }

        // ReSharper restore InconsistentNaming

        #endregion

        #region Fields & Property Info

        // Fields //

        [NotNull]
        private static readonly FieldInfo TestFieldNoAttributeFieldInfo =
            typeof(TestClassNoAttribute).GetField(nameof(TestClassNoAttribute._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        private static readonly FieldInfo TestFieldAttributeFieldInfo =
            typeof(TestClassWithAttribute).GetField(nameof(TestClassWithAttribute._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        private static readonly FieldInfo TestFieldAttributesFieldInfo =
            typeof(TestClassWithAttributes).GetField(nameof(TestClassWithAttributes._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        private static readonly FieldInfo TestFieldMultiAttributesFieldInfo =
            typeof(TestClassMultiAttributes).GetField(nameof(TestClassMultiAttributes._testField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion

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
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

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
                    new TestClassAttribute());

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    false,
                    new SecondTestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    true,
                    new SecondTestClassAttribute());

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
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

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
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new TestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new SecondTestClassAttribute());

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new SecondTestClassAttribute());

                #endregion

                #region ImmediateProperty

                // TODO

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateGetAttributeTestCases))]
        public void HasAndGetAttribute([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit, [CanBeNull] Attribute expectedAttribute)
        {
            if (expectedAttribute is null)
                Assert.IsFalse(member.HasAttribute(attributeType, inherit));
            else
                Assert.IsTrue(member.HasAttribute(attributeType, inherit));

            Assert.AreEqual(expectedAttribute, member.GetAttribute(attributeType, inherit));
        }

        [Test]
        public void TemplateHasAndGetAttribute()
        {
            #region ImmediateType

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassNoAttribute)), true, null);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, new TestClassAttribute());

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, null);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, null);

            // Attribute not inherited
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), true, null);

            // Attribute inherited 1
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), true, new TestClassAttribute());

            // Attribute inherited 2
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), true, new TestClassAttribute());

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), true, new TestClassAttribute());
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), false, new SecondTestClassAttribute());
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassMultiAttributes)), true, new SecondTestClassAttribute());

            #endregion

            #region ImmediateField

            // No attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), false, null);
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), true, null);

            // With attribute
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributesFieldInfo), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldAttributesFieldInfo), true, new TestClassAttribute());

            // Without requested attribute
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, null);
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, null);

            // Several attributes
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new TestClassAttribute());
            CheckHasAndGetAttribute<TestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new TestClassAttribute());
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new SecondTestClassAttribute());
            CheckHasAndGetAttribute<SecondTestClassAttribute>(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new SecondTestClassAttribute());

            #endregion

            #region ImmediateProperty

            // TODO

            #endregion

            #region Local function

            void CheckHasAndGetAttribute<TAttribute>(ImmediateMember member, bool inherit, Attribute expectedAttribute)
                where TAttribute : Attribute
            {
                if (expectedAttribute is null)
                    Assert.IsFalse(member.HasAttribute<TAttribute>(inherit));
                else
                    Assert.IsTrue(member.HasAttribute<TAttribute>(inherit));
                Assert.AreEqual(expectedAttribute, member.GetAttribute<TAttribute>(inherit));
            }

            #endregion
        }

        private static IEnumerable<TestCaseData> CreateWrongGetAttributeTestCases
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

                // TODO

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateWrongGetAttributeTestCases))]
        public void HasAndGetAttribute_WrongType([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => member.HasAttribute(attributeType, inherit));
            Assert.Throws<ArgumentException>(() => member.GetAttribute(attributeType, inherit));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void HasAndGetAttribute_Throws_NullType()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.HasAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateType.HasAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateType.GetAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateField.HasAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateField.HasAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateField.GetAttribute(null, true));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.HasAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.GetAttribute(null));
            Assert.Throws<ArgumentNullException>(() => immediateProperty.HasAttribute(null, true));
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
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

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
                    new[] { new TestClassAttribute() });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    false,
                    new[] { new SecondTestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    typeof(SecondTestClassAttribute),
                    true,
                    new[] { new SecondTestClassAttribute() });

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
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    false,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

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
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(TestClassAttribute),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    false,
                    new[] { new SecondTestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    typeof(SecondTestClassAttribute),
                    true,
                    new[] { new SecondTestClassAttribute() });

                #endregion

                #region ImmediateProperty

                // TODO

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
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttribute)), false, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttribute)), true, new[] { new TestClassAttribute() });

            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttributes)), false, new[] { new TestClassAttribute(), new TestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(TestClassWithAttributes)), true, new[] { new TestClassAttribute(), new TestClassAttribute() });

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), false, null);
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateType(typeof(TestClassWithAttribute)), true, null);

            // Attribute not inherited
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassNoAttribute)), true, null);

            // Attribute inherited 1
            CheckGetAttributes<TestClassAttribute>(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), false, null);
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute1)), true, new[] { new TestClassAttribute() });

            // Attribute inherited 2
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), false, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(InheritedTestClassWithAttribute2)), true, new[] { new TestClassAttribute(), new TestClassAttribute() });

            // Several attributes
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), false, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), true, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), false, new[] { new SecondTestClassAttribute() });
            CheckGetAttributes(new ImmediateType(typeof(TestClassMultiAttributes)), true, new[] { new SecondTestClassAttribute() });

            #endregion

            #region ImmediateField

            // No attribute
            CheckGetAttributes<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), false, null);
            CheckGetAttributes<TestClassAttribute>(new ImmediateField(TestFieldNoAttributeFieldInfo), true, null);

            // With attribute
            CheckGetAttributes(new ImmediateField(TestFieldAttributeFieldInfo), false, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateField(TestFieldAttributeFieldInfo), true, new[] { new TestClassAttribute() });

            CheckGetAttributes(new ImmediateField(TestFieldAttributesFieldInfo), false, new[] { new TestClassAttribute(), new TestClassAttribute() });
            CheckGetAttributes(new ImmediateField(TestFieldAttributesFieldInfo), true, new[] { new TestClassAttribute(), new TestClassAttribute() });

            // Without requested attribute
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), false, null);
            CheckGetAttributes<SecondTestClassAttribute>(new ImmediateField(TestFieldAttributeFieldInfo), true, null);

            // Several attributes
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new[] { new TestClassAttribute() });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), false, new[] { new SecondTestClassAttribute() });
            CheckGetAttributes(new ImmediateField(TestFieldMultiAttributesFieldInfo), true, new[] { new SecondTestClassAttribute() });

            #endregion

            #region ImmediateProperty

            // TODO

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

        private static IEnumerable<TestCaseData> CreateWrongGetAttributesTestCases
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

                // TODO

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateWrongGetAttributesTestCases))]
        public void GetAttributes_WrongType([NotNull] ImmediateMember member, [NotNull] Type attributeType, bool inherit)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => member.GetAttribute(attributeType, inherit));
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
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttribute)),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    false,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassWithAttributes)),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

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
                    new[] { new TestClassAttribute() });

                // Attribute inherited 2
                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    false,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassWithAttribute2)),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    false,
                    new Attribute[] { new TestClassAttribute(), new SecondTestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(TestClassMultiAttributes)),
                    true,
                    new Attribute[] { new TestClassAttribute(), new SecondTestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassMultiAttributes)),
                    false,
                    null);

                yield return new TestCaseData(
                    new ImmediateType(typeof(InheritedTestClassMultiAttributes)),
                    true,
                    new Attribute[] { new TestClassAttribute(), new SecondTestClassAttribute() });

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
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributeFieldInfo),
                    true,
                    new[] { new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    false,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldAttributesFieldInfo),
                    true,
                    new[] { new TestClassAttribute(), new TestClassAttribute() });

                // Several attributes
                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    false,
                    new Attribute[] { new TestClassAttribute(), new SecondTestClassAttribute() });

                yield return new TestCaseData(
                    new ImmediateField(TestFieldMultiAttributesFieldInfo),
                    true,
                    new Attribute[] { new TestClassAttribute(), new SecondTestClassAttribute() });

                #endregion

                #region ImmediateProperty

                // TODO

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