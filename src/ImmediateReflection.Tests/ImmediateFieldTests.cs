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
                #region Struct

                var testStruct = new TestStruct
                {
                    _testValue = 12
                };

                yield return new TestCaseData(testStruct, TestStructTestFieldFieldInfo, 12);

                #endregion

                #region Value type

                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass(2, 3, 4)
                {
                    _publicField = 1
                };

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicFieldFieldsInfo, 1);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypeInternalFieldFieldsInfo, 2);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypeProtectedFieldFieldsInfo, 3);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePrivateFieldFieldsInfo, 4);

                var internalValueTypeTestObject = new InternalValueTypeTestClass(6, 7, 8)
                {
                    _publicField = 5
                };

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicFieldFieldsInfo, 5);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypeInternalFieldFieldsInfo, 6);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypeProtectedFieldFieldsInfo, 7);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePrivateFieldFieldsInfo, 8);

                #endregion

                #region Reference type

                // Reference type
                var testObject1 = new TestObject { TestValue = 1 };
                var testObject2 = new TestObject { TestValue = 2 };
                var testObject3 = new TestObject { TestValue = 3 };
                var testObject4 = new TestObject { TestValue = 4 };
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass(testObject2, testObject3, testObject4)
                {
                    _publicField = testObject1
                };

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicFieldFieldsInfo, testObject1);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypeInternalFieldFieldsInfo, testObject2);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypeProtectedFieldFieldsInfo, testObject3);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePrivateFieldFieldsInfo, testObject4);

                var testObject5 = new TestObject { TestValue = 5 };
                var testObject6 = new TestObject { TestValue = 6 };
                var testObject7 = new TestObject { TestValue = 7 };
                var testObject8 = new TestObject { TestValue = 8 };
                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass(testObject6, testObject7, testObject8)
                {
                    _publicField = testObject5
                };

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicFieldFieldsInfo, testObject5);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypeInternalFieldFieldsInfo, testObject6);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypeProtectedFieldFieldsInfo, testObject7);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePrivateFieldFieldsInfo, testObject8);

                #endregion

                #region Object type

                // Object type
                var publicObjectTypeTestObject1 = new PublicObjectTypeTestClass(24, 48, 96)
                {
                    _publicField = 12
                };

                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicFieldFieldsInfo, 12);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypeInternalFieldFieldsInfo, 24);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypeProtectedFieldFieldsInfo, 48);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePrivateFieldFieldsInfo, 96);

                var publicObjectTypeTestObject2 = new PublicObjectTypeTestClass(testObject2, testObject3, testObject4)
                {
                    _publicField = testObject1
                };

                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicFieldFieldsInfo, testObject1);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypeInternalFieldFieldsInfo, testObject2);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypeProtectedFieldFieldsInfo, testObject3);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePrivateFieldFieldsInfo, testObject4);

                var internalObjectTypeTestObject1 = new InternalObjectTypeTestClass(384, 768, 1536)
                {
                    _publicField = 192
                };

                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicFieldFieldsInfo, 192);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypeInternalFieldFieldsInfo, 384);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypeProtectedFieldFieldsInfo, 768);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePrivateFieldFieldsInfo, 1536);

                var internalObjectTypeTestObject2 = new InternalObjectTypeTestClass(testObject6, testObject7, testObject8)
                {
                    _publicField = testObject5
                };

                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicFieldFieldsInfo, testObject5);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypeInternalFieldFieldsInfo, testObject6);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypeProtectedFieldFieldsInfo, testObject7);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePrivateFieldFieldsInfo, testObject8);

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
        public void ImmediateFieldGetValue_Enum()
        {
            // Simple TestEnum
            const TestEnum testEnum1 = TestEnum.EnumValue2;

            var immediateField = new ImmediateField(TestEnumFieldValueFieldInfo);
            Assert.AreEqual((int)TestEnum.EnumValue2, immediateField.GetValue(testEnum1));

            immediateField = new ImmediateField(TestEnumField2FieldInfo, typeof(TestEnum));
            Assert.AreEqual(TestEnum.EnumValue2, immediateField.GetValue(null));

            // TestEnum (inherit ulong)
            const TestEnumULong testEnum2 = TestEnumULong.EnumValue1;

            immediateField = new ImmediateField(TestEnumULongFieldValueFieldInfo);
            Assert.AreEqual((ulong)TestEnumULong.EnumValue1, immediateField.GetValue(testEnum2));

            immediateField = new ImmediateField(TestEnumULongField1FieldInfo, typeof(TestEnumULong));
            Assert.AreEqual(TestEnumULong.EnumValue1, immediateField.GetValue(null));

            // TestEnumFlags
            const TestEnumFlags testEnum3 = TestEnumFlags.EnumValue1 | TestEnumFlags.EnumValue2;

            immediateField = new ImmediateField(TestEnumFlagsFieldValueFieldInfo);
            Assert.AreEqual((int)(TestEnumFlags.EnumValue1 | TestEnumFlags.EnumValue2), immediateField.GetValue(testEnum3));

            immediateField = new ImmediateField(TestEnumFlagsField3FieldInfo, typeof(TestEnumFlags));
            Assert.AreEqual(TestEnumFlags.EnumValue3, immediateField.GetValue(null));
        }

        [Test]
        public void ImmediateFieldGetValue_EnumThrows()
        {
            // ReSharper disable ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new ImmediateField(null, typeof(TestEnum)));
            Assert.Throws<ArgumentException>(() => new ImmediateField(TestEnumField1FieldInfo, typeof(PublicValueTypeTestClass)));
            // ReSharper restore ObjectCreationAsStatement

            var immediateField = new ImmediateField(TestEnumFieldValueFieldInfo);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<TargetException>(() => immediateField.GetValue(null));
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
        public void ImmediateFieldSetValue_Struct()
        {
            var testStruct = new TestStruct();

            var immediateField = new ImmediateField(TestStructTestFieldFieldInfo);
            immediateField.SetValue(testStruct, 45);
            Assert.AreEqual(0, testStruct._testValue);  // Not updated there (but on the shadow copy yes) since struct are immutable
                                                                      // Limitation is the same with classic FieldInfo
        }

        [Test]
        public void ImmediateFieldSetValue_ValueType()
        {
            // Value type / Public
            var publicValueTypeTestObject = new PublicValueTypeTestClass();

            var immediateField = new ImmediateField(PublicValueTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicValueTypeTestObject, 12);
            Assert.AreEqual(12, publicValueTypeTestObject._publicField);

            immediateField = new ImmediateField(PublicValueTypeInternalFieldFieldsInfo);
            immediateField.SetValue(publicValueTypeTestObject, 24);
            Assert.AreEqual(24, publicValueTypeTestObject._internalField);

            immediateField = new ImmediateField(PublicValueTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(publicValueTypeTestObject, 48);
            Assert.AreEqual(48, publicValueTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(PublicValueTypePrivateFieldFieldsInfo);
            immediateField.SetValue(publicValueTypeTestObject, 96);
            Assert.AreEqual(96, publicValueTypeTestObject.GetPrivateFieldValue());

            // Value type / Internal
            var internalValueTypeTestObject = new InternalValueTypeTestClass();

            immediateField = new ImmediateField(InternalValueTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalValueTypeTestObject, 192);
            Assert.AreEqual(192, internalValueTypeTestObject._publicField);

            immediateField = new ImmediateField(InternalValueTypeInternalFieldFieldsInfo);
            immediateField.SetValue(internalValueTypeTestObject, 384);
            Assert.AreEqual(384, internalValueTypeTestObject._internalField);

            immediateField = new ImmediateField(InternalValueTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(internalValueTypeTestObject, 728);
            Assert.AreEqual(728, internalValueTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(InternalValueTypePrivateFieldFieldsInfo);
            immediateField.SetValue(internalValueTypeTestObject, 1536);
            Assert.AreEqual(1536, internalValueTypeTestObject.GetPrivateFieldValue());
        }

        [Test]
        public void ImmediateFieldSetValue_ReferenceType()
        {
            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };
            var testObject5 = new TestObject { TestValue = 5 };
            var testObject6 = new TestObject { TestValue = 6 };
            var testObject7 = new TestObject { TestValue = 7 };
            var testObject8 = new TestObject { TestValue = 8 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            var immediateField = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject._publicField);

            immediateField = new ImmediateField(PublicReferenceTypeInternalFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject2);
            Assert.AreSame(testObject2, publicReferenceTypeTestObject._internalField);

            immediateField = new ImmediateField(PublicReferenceTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject3);
            Assert.AreSame(testObject3, publicReferenceTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(PublicReferenceTypePrivateFieldFieldsInfo);
            immediateField.SetValue(publicReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicReferenceTypeTestObject.GetPrivateFieldValue());

            // Reference type / Internal
            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

            immediateField = new ImmediateField(InternalReferenceTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject5);
            Assert.AreSame(testObject5, internalReferenceTypeTestObject._publicField);

            immediateField = new ImmediateField(InternalReferenceTypeInternalFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject6);
            Assert.AreSame(testObject6, internalReferenceTypeTestObject._internalField);

            immediateField = new ImmediateField(InternalReferenceTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject7);
            Assert.AreSame(testObject7, internalReferenceTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(InternalReferenceTypePrivateFieldFieldsInfo);
            immediateField.SetValue(internalReferenceTypeTestObject, testObject8);
            Assert.AreSame(testObject8, internalReferenceTypeTestObject.GetPrivateFieldValue());
        }

        [Test]
        public void ImmediateFieldSetValue_ObjectType()
        {
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };
            var testObject5 = new TestObject { TestValue = 5 };
            var testObject6 = new TestObject { TestValue = 6 };
            var testObject7 = new TestObject { TestValue = 7 };
            var testObject8 = new TestObject { TestValue = 8 };

            // Object type / Public
            var publicObjectTypeTestObject = new PublicObjectTypeTestClass();

            var immediateField = new ImmediateField(PublicObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, 1);
            Assert.AreEqual(1, publicObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(PublicObjectTypeInternalFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, 2);
            Assert.AreEqual(2, publicObjectTypeTestObject._internalField);

            immediateField = new ImmediateField(PublicObjectTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, 3);
            Assert.AreEqual(3, publicObjectTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(PublicObjectTypePrivateFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, 4);
            Assert.AreEqual(4, publicObjectTypeTestObject.GetPrivateFieldValue());


            immediateField = new ImmediateField(PublicObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(PublicObjectTypeInternalFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, testObject2);
            Assert.AreSame(testObject2, publicObjectTypeTestObject._internalField);

            immediateField = new ImmediateField(PublicObjectTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, testObject3);
            Assert.AreSame(testObject3, publicObjectTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(PublicObjectTypePrivateFieldFieldsInfo);
            immediateField.SetValue(publicObjectTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicObjectTypeTestObject.GetPrivateFieldValue());


            // Object type / Internal
            var internalObjectTypeTestObject = new InternalObjectTypeTestClass();

            immediateField = new ImmediateField(InternalObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, 1);
            Assert.AreEqual(1, internalObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(InternalObjectTypeInternalFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, 2);
            Assert.AreEqual(2, internalObjectTypeTestObject._internalField);

            immediateField = new ImmediateField(InternalObjectTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, 3);
            Assert.AreEqual(3, internalObjectTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(InternalObjectTypePrivateFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, 4);
            Assert.AreEqual(4, internalObjectTypeTestObject.GetPrivateFieldValue());


            immediateField = new ImmediateField(InternalObjectTypePublicFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, testObject5);
            Assert.AreSame(testObject5, internalObjectTypeTestObject._publicField);

            immediateField = new ImmediateField(InternalObjectTypeInternalFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, testObject6);
            Assert.AreSame(testObject6, internalObjectTypeTestObject._internalField);

            immediateField = new ImmediateField(InternalObjectTypeProtectedFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, testObject7);
            Assert.AreSame(testObject7, internalObjectTypeTestObject.GetProtectedFieldValue());

            immediateField = new ImmediateField(InternalObjectTypePrivateFieldFieldsInfo);
            immediateField.SetValue(internalObjectTypeTestObject, testObject8);
            Assert.AreSame(testObject8, internalObjectTypeTestObject.GetPrivateFieldValue());
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
        public void ImmediateFieldSetValue_Enum()
        {
            // Simple TestEnum
            TestEnum testEnum = TestEnum.EnumValue2;

            var immediateField = new ImmediateField(TestEnumFieldValueFieldInfo);
            immediateField.SetValue(testEnum, TestEnum.EnumValue1);
            Assert.AreEqual(TestEnum.EnumValue2, testEnum);     // Enum internal value cannot be set
        }

        [Test]
        public void ImmediateFieldSetValue_EnumThrows()
        {
            var immediateField = new ImmediateField(TestEnumFieldValueFieldInfo);
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<TargetException>(() => immediateField.SetValue(null, TestEnum.EnumValue2));

            immediateField = new ImmediateField(TestEnumField1FieldInfo);
            Assert.Throws<FieldAccessException>(() => immediateField.SetValue(null, TestEnum.EnumValue2));
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