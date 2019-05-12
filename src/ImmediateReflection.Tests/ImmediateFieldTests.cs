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
                #region Value type

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

                #endregion

                #region Reference type

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

                #endregion

                #region Object type

                // Object type
                var publicObjectTypeTestObject1 = new PublicObjectTypeTestClass
                {
                    _publicField = 12
                };

                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicFieldFieldsInfo, 12);

                var publicObjectTypeTestObject2 = new PublicObjectTypeTestClass
                {
                    _publicField = testObject1
                };

                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicFieldFieldsInfo, testObject1);

                var internalObjectTypeTestObject1 = new InternalObjectTypeTestClass
                {
                    _publicField = 24
                };

                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicFieldFieldsInfo, 24);

                var internalObjectTypeTestObject2 = new InternalObjectTypeTestClass
                {
                    _publicField = testObject2
                };

                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicFieldFieldsInfo, testObject2);

                #endregion

                #region Nested types

                // Nested types
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { _nestedTestValue = 1 };
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicFieldFieldInfo, 1);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { _nestedTestValue = 2 };
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicFieldFieldInfo, 2);

                var protectedNestedTypeTestObject = new ProtectedNestedClass { _nestedTestValue = 3 };
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicFieldFieldInfo, 3);

                var privateNestedTypeTestObject = new PrivateNestedClass { _nestedTestValue = 4 };
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicFieldFieldInfo, 4);

                #endregion
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
        public void ImmediateFieldGetValue_Static()
        {
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };

            // Value type
            PublicValueTypeTestClass._publicStaticField = 12;
            var immediateField = new ImmediateField(PublicValueTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(12, immediateField.GetValue(null));

            InternalValueTypeTestClass._publicStaticField = 24;
            immediateField = new ImmediateField(InternalValueTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(24, immediateField.GetValue(null));

            // Reference type
            PublicReferenceTypeTestClass._publicStaticField = testObject1;
            immediateField = new ImmediateField(PublicReferenceTypeStaticPublicFieldFieldsInfo);
            Assert.AreSame(testObject1, immediateField.GetValue(null));

            InternalReferenceTypeTestClass._publicStaticField = testObject2;
            immediateField = new ImmediateField(InternalReferenceTypeStaticPublicFieldFieldsInfo);
            Assert.AreSame(testObject2, immediateField.GetValue(null));

            // Object type
            PublicObjectTypeTestClass._publicStaticField = 48;
            immediateField = new ImmediateField(PublicObjectTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(48, immediateField.GetValue(null));

            InternalObjectTypeTestClass._publicStaticField = 96;
            immediateField = new ImmediateField(InternalObjectTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(96, immediateField.GetValue(null));

            PublicObjectTypeTestClass._publicStaticField = testObject1;
            immediateField = new ImmediateField(PublicObjectTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(testObject1, immediateField.GetValue(null));

            InternalObjectTypeTestClass._publicStaticField = testObject2;
            immediateField = new ImmediateField(InternalObjectTypeStaticPublicFieldFieldsInfo);
            Assert.AreEqual(testObject2, immediateField.GetValue(null));
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
        public void ImmediateFieldSetValue_ValueType()
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
        }

        [Test]
        public void ImmediateFieldSetValue_ReferenceType()
        {
            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            var immediateField = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject._publicField);

            // Reference type / Internal
            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

            immediateField = new ImmediateField(InternalReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject2);
            Assert.AreSame(testObject2, internalReferenceTypeTestObject._publicField);
        }

        [Test]
        public void ImmediateFieldSetValue_ObjectType()
        {
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };

            // Object type / Public
            var publicObjectTypeTestObject = new PublicObjectTypeTestClass();

            var immediateField = new ImmediateField(PublicObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, 1);
            Assert.AreEqual(1, publicObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(PublicObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicObjectTypeTestObject._publicField);

            // Object type / Internal
            var internalObjectTypeTestObject = new InternalObjectTypeTestClass();

            immediateField = new ImmediateField(InternalObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, 1);
            Assert.AreEqual(1, internalObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(InternalObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, testObject2);
            Assert.AreSame(testObject2, internalObjectTypeTestObject._publicField);
        }

        [Test]
        public void ImmediateFieldSetValue_Static()
        {
            var testObject = new TestObject { TestValue = 1 };

            // Value type
            var immediateProperty = new ImmediateField(PublicValueTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, 12);
            Assert.AreEqual(12, PublicValueTypeTestClass._publicStaticField);

            immediateProperty = new ImmediateField(InternalValueTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, 24);
            Assert.AreEqual(24, InternalValueTypeTestClass._publicStaticField);

            // Reference type
            immediateProperty = new ImmediateField(PublicReferenceTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, PublicReferenceTypeTestClass._publicStaticField);

            immediateProperty = new ImmediateField(InternalReferenceTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, InternalReferenceTypeTestClass._publicStaticField);

            // Object type
            immediateProperty = new ImmediateField(PublicObjectTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, 48);
            Assert.AreEqual(48, PublicObjectTypeTestClass._publicStaticField);

            immediateProperty = new ImmediateField(InternalObjectTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, 96);
            Assert.AreEqual(96, InternalObjectTypeTestClass._publicStaticField);

            immediateProperty = new ImmediateField(PublicObjectTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, PublicObjectTypeTestClass._publicStaticField);

            immediateProperty = new ImmediateField(InternalObjectTypeStaticPublicFieldFieldsInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, InternalObjectTypeTestClass._publicStaticField);
        }

        [Test]
        public void ImmediateFieldSetValue_NestedTypes()
        {
            var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { _nestedTestValue = 1 };
            var immediateField = new ImmediateField(PublicNestedPublicFieldFieldInfo);
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
            Assert.Throws<InvalidCastException>(() => immediateField.SetValue(new PublicReferenceTypeTestClass(), null));
        }

        [Test]
        public void ImmediateFieldSetValue_WrongValue()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.Throws<InvalidCastException>(() => immediateField1.SetValue(new PublicValueTypeTestClass(), new TestObject()));

            var immediateField2 = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            Assert.Throws<InvalidCastException>(() => immediateField2.SetValue(new PublicReferenceTypeTestClass(), 12));

            var immediateField3 = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            Assert.Throws<InvalidCastException>(() => immediateField3.SetValue(new PublicReferenceTypeTestClass(), new SmallObject()));
        }

        #endregion

        [Test]
        public void ImmediateFieldEquality()
        {
            var immediateField1 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            var immediateField2 = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            Assert.IsTrue(immediateField1.Equals(immediateField1));
            Assert.IsTrue(immediateField1.Equals(immediateField2));
            Assert.IsTrue(immediateField1.Equals((object)immediateField2));
            Assert.IsFalse(immediateField1.Equals(null));

            var immediateField3 = new ImmediateField(PublicValueTypePublicField2FieldsInfo);
            Assert.IsFalse(immediateField1.Equals(immediateField3));
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