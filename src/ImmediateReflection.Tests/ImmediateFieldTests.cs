using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateField"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateFieldTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        private class PrivateNestedClass
        {
#pragma warning disable 649
            // ReSharper disable once InconsistentNaming
            public int _nestedTestValue;
#pragma warning restore 649
        }

        #region Test helpers

        // Fields //

        [NotNull]
        protected static FieldInfo PrivateNestedPublicFieldFieldInfo =
            typeof(PrivateNestedClass).GetField(nameof(PrivateNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion

        [Test]
        public void ImmediateFieldInfo()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo.Name, immediateField1.Name);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo.FieldType, immediateField1.FieldType);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo, immediateField1.FieldInfo);

            var immediateField2 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1.FieldInfo, immediateField2.FieldInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateField(null));
        }

        #region GetValue

        private static IEnumerable<TestCaseData> CreateImmediateFieldGetValueTestCases
        {
            [UsedImplicitly]
            get
            {
                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass
                {
                    _publicField = 1
                };

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicFieldFieldsInfo, 1);

                var internalValueTypeTestObject = new InternalValueTypeTestClass
                {
                    _publicField = 2
                };

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicFieldFieldsInfo, 2);

                // Reference type
                var testObject1 = new TestObject { TestValue = 1 };
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass
                {
                    _publicField = testObject1
                };

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicFieldFieldsInfo, testObject1);

                var testObject2 = new TestObject { TestValue = 2 };
                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass
                {
                    _publicField = testObject2
                };

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicFieldFieldsInfo, testObject2);

                // Nested type
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { _nestedTestValue = 1 };
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicFieldFieldInfo, 1);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { _nestedTestValue = 2 };
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicFieldFieldInfo, 2);

                var protectedNestedTypeTestObject = new ProtectedNestedClass { _nestedTestValue = 3 };
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicFieldFieldInfo, 3);

                var privateNestedTypeTestObject = new PrivateNestedClass { _nestedTestValue = 4 };
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicFieldFieldInfo, 4);
            }
        }

        [TestCaseSource(nameof(CreateImmediateFieldGetValueTestCases))]
        public void ImmediateFieldGetValue([NotNull] object target, [NotNull] FieldInfo field, [CanBeNull] object expectedValue)
        {
            var immediateField = new ImmediateField(field);

            object gotValue = immediateField.GetValue(target);
            if (expectedValue is null)
                Assert.IsNull(gotValue);
            else if (expectedValue.GetType().IsValueType)
                Assert.AreEqual(expectedValue, gotValue);
            else
                Assert.AreSame(expectedValue, gotValue);
        }

        [Test]
        public void ImmediateFieldGetValue_NullInstance()
        {
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<TargetException>(() => immediateField.GetValue(null));
        }

        #endregion

        #region SetValue

        [Test]
        public void ImmediateFieldSetValue()
        {
            // Value type / Public
            var publicValueTypeTestObject = new PublicValueTypeTestClass();

            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicValueTypeTestObject, 12);
            Assert.AreEqual(12, publicValueTypeTestObject._publicField);

            // Value type / Internal
            var internalValueTypeTestObject = new InternalValueTypeTestClass();

            immediateField = new ImmediateField(InternalValueTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalValueTypeTestObject, 24);
            Assert.AreEqual(24, internalValueTypeTestObject._publicField);


            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            immediateField = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject._publicField);

            // Reference type / Internal
            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

            immediateField = new ImmediateField(InternalReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject2);
            Assert.AreSame(testObject2, internalReferenceTypeTestObject._publicField);


            // Nested type
            var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { _nestedTestValue = 1 };
            immediateField = new ImmediateField(PublicNestedPublicFieldFieldInfo);
            immediateField.SetValue(publicNestedTypeTestObject, 12);
            Assert.AreEqual(12, publicNestedTypeTestObject._nestedTestValue);

            var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { _nestedTestValue = 2 };
            immediateField = new ImmediateField(InternalNestedPublicFieldFieldInfo);
            immediateField.SetValue(internalNestedTypeTestObject, 24);
            Assert.AreEqual(24, internalNestedTypeTestObject._nestedTestValue);

            var protectedNestedTypeTestObject = new ProtectedNestedClass { _nestedTestValue = 3 };
            immediateField = new ImmediateField(ProtectedNestedPublicFieldFieldInfo);
            immediateField.SetValue(protectedNestedTypeTestObject, 48);
            Assert.AreEqual(48, protectedNestedTypeTestObject._nestedTestValue);

            var privateNestedTypeTestObject = new PrivateNestedClass { _nestedTestValue = 4 };
            immediateField = new ImmediateField(PrivateNestedPublicFieldFieldInfo);
            immediateField.SetValue(privateNestedTypeTestObject, 96);
            Assert.AreEqual(96, privateNestedTypeTestObject._nestedTestValue);
        }

        [Test]
        public void ImmediateFieldSetValue_NullInstance()
        {
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<TargetException>(() => immediateField.SetValue(null, null));
        }

        [Test]
        public void ImmediateFieldSetValue_WrongInstance()
        {
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.Throws<ArgumentException>(() => immediateField.SetValue(new PublicReferenceTypeTestClass(), null));
        }

        [Test]
        public void ImmediateFieldSetValue_WrongValue()
        {
            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.Throws<ArgumentException>(() => immediateField.SetValue(new PublicValueTypeTestClass(), new TestObject()));
        }

        #endregion

        [Test]
        public void ImmediateFieldEquality()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateField2 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(immediateField1, immediateField1);
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
            var immediateField2 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.AreEqual(PublicValueTypePublicFieldFieldsInfo.GetHashCode(), immediateField1.GetHashCode());
            Assert.AreEqual(immediateField1.GetHashCode(), immediateField2.GetHashCode());

            var immediateField3 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.AreNotEqual(immediateField1.GetHashCode(), immediateField3.GetHashCode());
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