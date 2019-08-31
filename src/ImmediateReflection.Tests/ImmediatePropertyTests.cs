using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateProperty"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediatePropertyTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        private class PrivateNestedClass
        {
            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int NestedTestValue { get; set; } = 25;
        }

        #region Test helpers

        // Properties //

        [NotNull]
        private static readonly PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
            typeof(PrivateNestedClass).GetProperty(nameof(PrivateNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        #endregion

        #endregion

        [Test]
        public void ImmediatePropertyInfo()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(nameof(PublicValueTypeTestClass.PublicPropertyGetSet), immediateProperty1.Name);
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateProperty1.DeclaringType);
            Assert.AreEqual(typeof(int), immediateProperty1.PropertyType);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo, immediateProperty1.PropertyInfo);

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.PropertyInfo, immediateProperty2.PropertyInfo);
        }

#if SUPPORTS_IMMEDIATE_MEMBER_TYPE
        #region PropertyImmediateType

        [Test]
        public void PropertyImmediateType()
        {
            CheckPropertyImmediateType(new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo));
            CheckPropertyImmediateType(new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo));
            CheckPropertyImmediateType(new ImmediateProperty(PublicObjectTypePublicGetSetPropertyPropertyInfo));

            #region Local function

            void CheckPropertyImmediateType(ImmediateProperty property)
            {
                ImmediateType immediateType = property.PropertyImmediateType;
                Assert.IsNotNull(immediateType);
                Assert.AreEqual(property.PropertyType, immediateType.Type);

                ImmediateType immediateType2 = property.PropertyImmediateType;
                Assert.AreSame(immediateType, immediateType2);
            }

            #endregion
        }

        #endregion
#endif

        #region CanRead

        private static IEnumerable<TestCaseData> CreateImmediatePropertyCanReadTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Struct

                yield return new TestCaseData(TestStructTestPropertyPropertyInfo, true);

                #endregion

                #region Value type

                // Value type
                yield return new TestCaseData(PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicValueTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicValueTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicValueTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicValueTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalValueTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalValueTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalValueTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalValueTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Reference type

                // Reference type
                yield return new TestCaseData(PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicReferenceTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicReferenceTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicReferenceTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicReferenceTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalReferenceTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalReferenceTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalReferenceTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalReferenceTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Object type

                // Object type
                yield return new TestCaseData(PublicObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicObjectTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicObjectTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicObjectTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicObjectTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalObjectTypePublicSetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalObjectTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalObjectTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalObjectTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Static

                yield return new TestCaseData(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypeStaticPublicGetSetPropertyPropertyInfo, true);

                yield return new TestCaseData(PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo, true);

                yield return new TestCaseData(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo, true);

                #endregion

                #region Nested types

                // Nested types
                yield return new TestCaseData(PublicNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(ProtectedNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PrivateNestedPublicGetSetPropertyPropertyInfo, true);

                #endregion

                #region Abstract

                yield return new TestCaseData(PublicValueTypePublicAbstractGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicConcreteGetSetPropertyPropertyInfo, true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateImmediatePropertyCanReadTestCases))]
        public void ImmediatePropertyCanRead([NotNull] PropertyInfo property, bool expectedCanRead)
        {
            var immediateProperty = new ImmediateProperty(property);
            Assert.AreEqual(expectedCanRead, immediateProperty.CanRead);
        }

        #endregion

        #region GetValue

        private static IEnumerable<TestCaseData> CreateImmediatePropertyGetValueTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Struct

                var testStruct = new TestStruct
                {
                    TestValue = 42
                };

                yield return new TestCaseData(testStruct, TestStructTestPropertyPropertyInfo, 42);

                #endregion

                #region Value type

                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass(3, 4)
                {
                    PublicPropertyGetSet = 1,
                    PublicVirtualPropertyGetSet = 2,
                    PublicPropertyPrivateGetSet = 5
                };

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicVirtualGetSetPropertyPropertyInfo, 2);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPropertyPropertyInfo, 3);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, 4);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, 5);    // Private Get but gettable via Reflection

                var internalValueTypeTestObject = new InternalValueTypeTestClass(8, 9)
                {
                    PublicPropertyGetSet = 6,
                    PublicVirtualPropertyGetSet = 7,
                    PublicPropertyPrivateGetSet = 10
                };

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetSetPropertyPropertyInfo, 6);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicVirtualGetSetPropertyPropertyInfo, 7);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPropertyPropertyInfo, 8);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, 9);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, 10);    // Private Get but gettable via Reflection

                #endregion

                #region Reference type

                // Reference type
                var testObject1 = new TestObject { TestValue = 1 };
                var testObject2 = new TestObject { TestValue = 2 };
                var testObject3 = new TestObject { TestValue = 3 };
                var testObject4 = new TestObject { TestValue = 4 };
                var testObject5 = new TestObject { TestValue = 5 };
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass(testObject3, testObject4)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicVirtualPropertyGetSet = testObject2,
                    PublicPropertyPrivateGetSet = testObject5
                };

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, testObject4);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, testObject5);  // Private Get but gettable via Reflection

                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass(testObject3, testObject4)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicVirtualPropertyGetSet = testObject2,
                    PublicPropertyPrivateGetSet = testObject5
                };

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, testObject4);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, testObject5);  // Private Get but gettable via Reflection

                #endregion

                #region Object type

                // Object type
                var publicObjectTypeTestObject1 = new PublicObjectTypeTestClass(3, 4)
                {
                    PublicPropertyGetSet = 1,
                    PublicVirtualPropertyGetSet = 2,
                    PublicPropertyPrivateGetSet = 5
                };

                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo, 2);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetPropertyPropertyInfo, 3);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, 4);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, 5);  // Private Get but gettable via Reflection

                var publicObjectTypeTestObject2 = new PublicObjectTypeTestClass(testObject3, testObject4)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicVirtualPropertyGetSet = testObject2,
                    PublicPropertyPrivateGetSet = testObject5
                };

                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, testObject4);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, testObject5);  // Private Get but gettable via Reflection

                var internalObjectTypeTestObject1 = new InternalObjectTypeTestClass(3, 4)
                {
                    PublicPropertyGetSet = 1,
                    PublicVirtualPropertyGetSet = 2,
                    PublicPropertyPrivateGetSet = 5
                };

                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo, 2);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetPropertyPropertyInfo, 3);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, 4);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, 5);  // Private Get but gettable via Reflection

                var internalObjectTypeTestObject2 = new InternalObjectTypeTestClass(testObject3, testObject4)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicVirtualPropertyGetSet = testObject2,
                    PublicPropertyPrivateGetSet = testObject5
                };

                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, testObject4);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, testObject5);  // Private Get but gettable via Reflection

                #endregion

                #region Nested types

                // Nested types
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { NestedTestValue = 1 };
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicGetSetPropertyPropertyInfo, 1);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { NestedTestValue = 2 };
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicGetSetPropertyPropertyInfo, 2);

                var protectedNestedTypeTestObject = new ProtectedNestedClass { NestedTestValue = 3 };
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicGetSetPropertyPropertyInfo, 3);

                var privateNestedTypeTestObject = new PrivateNestedClass { NestedTestValue = 4 };
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicGetSetPropertyPropertyInfo, 4);

                #endregion

                #region Abstract

                var concreteTestObject = new ConcretePublicValueTypeTestClass
                {
                    PublicAbstractGetSetProperty = 88
                };

                yield return new TestCaseData(concreteTestObject, PublicValueTypePublicAbstractGetSetPropertyPropertyInfo, 88);
                yield return new TestCaseData(concreteTestObject, PublicValueTypePublicConcreteGetSetPropertyPropertyInfo, 88);

                #endregion

                #region New keyword

                var baseObject = new BaseTestClass();
                yield return new TestCaseData(baseObject, BaseClassPublicGetPropertyPropertyInfo, "Parent");

                var childObject = new ChildTestClass();
                yield return new TestCaseData(childObject, ChildClassPublicGetPropertyPropertyInfo, "Child");
                yield return new TestCaseData(childObject, BaseClassPublicGetPropertyPropertyInfo, "Parent");

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateImmediatePropertyGetValueTestCases))]
        public void ImmediatePropertyGetValue([NotNull] object target, [NotNull] PropertyInfo property, [CanBeNull] object expectedValue)
        {
            var immediateProperty = new ImmediateProperty(property);

            object gotValue = immediateProperty.GetValue(target);
            if (expectedValue is null)
                Assert.IsNull(gotValue);
            else if (expectedValue.GetType().IsValueType)
                Assert.AreEqual(expectedValue, gotValue);
            else
                Assert.AreSame(expectedValue, gotValue);
        }

        [Test]
        public void ImmediatePropertyGetValue_Static()
        {
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };

            // Value type
            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 12;
            var immediateProperty = new ImmediateProperty(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(12, immediateProperty.GetValue(null));

            InternalValueTypeTestClass.PublicStaticPropertyGetSet = 24;
            immediateProperty = new ImmediateProperty(InternalValueTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(24, immediateProperty.GetValue(null));

            // Reference type
            PublicReferenceTypeTestClass.PublicStaticPropertyGetSet = testObject1;
            immediateProperty = new ImmediateProperty(PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject1, immediateProperty.GetValue(null));

            InternalReferenceTypeTestClass.PublicStaticPropertyGetSet = testObject2;
            immediateProperty = new ImmediateProperty(InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject2, immediateProperty.GetValue(null));

            // Object type
            PublicObjectTypeTestClass.PublicStaticPropertyGetSet = 48;
            immediateProperty = new ImmediateProperty(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(48, immediateProperty.GetValue(null));

            InternalObjectTypeTestClass.PublicStaticPropertyGetSet = 96;
            immediateProperty = new ImmediateProperty(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(96, immediateProperty.GetValue(null));

            PublicObjectTypeTestClass.PublicStaticPropertyGetSet = testObject1;
            immediateProperty = new ImmediateProperty(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(testObject1, immediateProperty.GetValue(null));

            InternalObjectTypeTestClass.PublicStaticPropertyGetSet = testObject2;
            immediateProperty = new ImmediateProperty(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(testObject2, immediateProperty.GetValue(null));
        }

        [Test]
        public void ImmediatePropertyGetValue_NonPublic()
        {
            // Value type
            var publicValueTypeTestObject = new PublicValueTypeTestClass
            {
                _publicField = 12
            };

            var immediateProperty = new ImmediateProperty(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreEqual(12, immediateProperty.GetValue(publicValueTypeTestObject));

            publicValueTypeTestObject._publicField = 24;
            immediateProperty = new ImmediateProperty(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreEqual(24, immediateProperty.GetValue(publicValueTypeTestObject));

            publicValueTypeTestObject._publicField = 48;
            immediateProperty = new ImmediateProperty(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(48, immediateProperty.GetValue(publicValueTypeTestObject));

            var internalValueTypeTestObject = new InternalValueTypeTestClass
            {
                _publicField = 12
            };

            immediateProperty = new ImmediateProperty(InternalValueTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreEqual(12, immediateProperty.GetValue(internalValueTypeTestObject));

            internalValueTypeTestObject._publicField = 24;
            immediateProperty = new ImmediateProperty(InternalValueTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreEqual(24, immediateProperty.GetValue(internalValueTypeTestObject));

            internalValueTypeTestObject._publicField = 48;
            immediateProperty = new ImmediateProperty(InternalValueTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(48, immediateProperty.GetValue(internalValueTypeTestObject));

            // Reference type
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass
            {
                _publicField = testObject1
            };

            immediateProperty = new ImmediateProperty(PublicReferenceTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject1, immediateProperty.GetValue(publicReferenceTypeTestObject));

            publicReferenceTypeTestObject._publicField = testObject2;
            immediateProperty = new ImmediateProperty(PublicReferenceTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject2, immediateProperty.GetValue(publicReferenceTypeTestObject));

            publicReferenceTypeTestObject._publicField = testObject3;
            immediateProperty = new ImmediateProperty(PublicReferenceTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject3, immediateProperty.GetValue(publicReferenceTypeTestObject));

            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass
            {
                _publicField = testObject1
            };

            immediateProperty = new ImmediateProperty(InternalReferenceTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject1, immediateProperty.GetValue(internalReferenceTypeTestObject));

            internalReferenceTypeTestObject._publicField = testObject2;
            immediateProperty = new ImmediateProperty(InternalReferenceTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject2, immediateProperty.GetValue(internalReferenceTypeTestObject));

            internalReferenceTypeTestObject._publicField = testObject3;
            immediateProperty = new ImmediateProperty(InternalReferenceTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject3, immediateProperty.GetValue(internalReferenceTypeTestObject));

            // Object type
            var publicObjectTypeTestObject = new PublicObjectTypeTestClass
            {
                _publicField = 12
            };

            immediateProperty = new ImmediateProperty(PublicObjectTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreEqual(12, immediateProperty.GetValue(publicObjectTypeTestObject));

            publicObjectTypeTestObject._publicField = 24;
            immediateProperty = new ImmediateProperty(PublicObjectTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreEqual(24, immediateProperty.GetValue(publicObjectTypeTestObject));

            publicObjectTypeTestObject._publicField = 48;
            immediateProperty = new ImmediateProperty(PublicObjectTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(48, immediateProperty.GetValue(publicObjectTypeTestObject));

            publicObjectTypeTestObject._publicField = testObject1;
            immediateProperty = new ImmediateProperty(PublicObjectTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject1, immediateProperty.GetValue(publicObjectTypeTestObject));

            publicObjectTypeTestObject._publicField = testObject2;
            immediateProperty = new ImmediateProperty(PublicObjectTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject2, immediateProperty.GetValue(publicObjectTypeTestObject));

            publicObjectTypeTestObject._publicField = testObject3;
            immediateProperty = new ImmediateProperty(PublicObjectTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject3, immediateProperty.GetValue(publicObjectTypeTestObject));


            var internalObjectTypeTestObject = new InternalObjectTypeTestClass
            {
                _publicField = 12
            };

            immediateProperty = new ImmediateProperty(InternalObjectTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreEqual(12, immediateProperty.GetValue(internalObjectTypeTestObject));

            internalObjectTypeTestObject._publicField = 24;
            immediateProperty = new ImmediateProperty(InternalObjectTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreEqual(24, immediateProperty.GetValue(internalObjectTypeTestObject));

            internalObjectTypeTestObject._publicField = 48;
            immediateProperty = new ImmediateProperty(InternalObjectTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(48, immediateProperty.GetValue(internalObjectTypeTestObject));

            internalObjectTypeTestObject._publicField = testObject1;
            immediateProperty = new ImmediateProperty(InternalObjectTypeInternalGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject1, immediateProperty.GetValue(internalObjectTypeTestObject));

            internalObjectTypeTestObject._publicField = testObject2;
            immediateProperty = new ImmediateProperty(InternalObjectTypeProtectedGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject2, immediateProperty.GetValue(internalObjectTypeTestObject));

            internalObjectTypeTestObject._publicField = testObject3;
            immediateProperty = new ImmediateProperty(InternalObjectTypePrivateGetSetPropertyPropertyInfo);
            Assert.AreSame(testObject3, immediateProperty.GetValue(internalObjectTypeTestObject));
        }

        [Test]
        public void ImmediatePropertyGetValue_NullInstance()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<TargetException>(() => immediateProperty.GetValue(null));
        }

        [Test]
        public void ImmediatePropertyGetValue_NoGetter()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicSetPropertyPropertyInfo);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => immediateProperty.GetValue(new PublicValueTypeTestClass()));
        }

        #endregion

        #region CanWrite

        private static IEnumerable<TestCaseData> CreateImmediatePropertyCanWriteTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Struct

                yield return new TestCaseData(TestStructTestPropertyPropertyInfo, true);

                #endregion

                #region Value type

                // Value type
                yield return new TestCaseData(PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicValueTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicValueTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalValueTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalValueTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Reference type

                // Reference type
                yield return new TestCaseData(PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicReferenceTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicReferenceTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalReferenceTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalReferenceTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Object type

                // Object type
                yield return new TestCaseData(PublicObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(PublicObjectTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(PublicObjectTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                yield return new TestCaseData(InternalObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypeInternalGetSetPropertyPropertyInfo, true); // Internal property
                yield return new TestCaseData(InternalObjectTypeProtectedGetSetPropertyPropertyInfo, true);// Protected property
                yield return new TestCaseData(InternalObjectTypePrivateGetSetPropertyPropertyInfo, true);  // Private property

                #endregion

                #region Static

                yield return new TestCaseData(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypeStaticPublicGetSetPropertyPropertyInfo, true);

                yield return new TestCaseData(PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo, true);

                yield return new TestCaseData(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo, true);

                #endregion

                #region Nested types

                // Nested types
                yield return new TestCaseData(PublicNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(ProtectedNestedPublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PrivateNestedPublicGetSetPropertyPropertyInfo, true);

                #endregion

                #region Abstract

                yield return new TestCaseData(PublicValueTypePublicAbstractGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicConcreteGetSetPropertyPropertyInfo, true);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateImmediatePropertyCanWriteTestCases))]
        public void ImmediatePropertyCanWrite([NotNull] PropertyInfo property, bool expectedCanWrite)
        {
            var immediateProperty = new ImmediateProperty(property);
            Assert.AreEqual(expectedCanWrite, immediateProperty.CanWrite);
        }

        #endregion

        #region SetValue

        [Test]
        public void ImmediatePropertySetValue_Struct()
        {
            var testStruct = new TestStruct();

            var immediateProperty = new ImmediateProperty(TestStructTestPropertyPropertyInfo);
            immediateProperty.SetValue(testStruct, 51);
            Assert.AreEqual(0, testStruct.TestValue);   // Not updated there (but on the shadow copy yes) since struct are immutable
                                                                      // Limitation is the same with classic PropertyInfo
        }

        [Test]
        public void ImmediatePropertySetValue_ValueType()
        {
            // Value type / Public
            var publicValueTypeTestObject = new PublicValueTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 12);
            Assert.AreEqual(12, publicValueTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 24);
            Assert.AreEqual(24, publicValueTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 48);          // Private Set but settable via Reflection
            Assert.AreEqual(48, publicValueTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 96);
            Assert.AreEqual(96, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicValueTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 192);
            Assert.AreEqual(192, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 384);
            Assert.AreEqual(384, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 728);
            Assert.AreEqual(728, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 1536);
            Assert.AreEqual(1536, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            // Value type / Internal
            var internalValueTypeTestObject = new InternalValueTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalValueTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 12);
            Assert.AreEqual(12, internalValueTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalValueTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 24);
            Assert.AreEqual(24, internalValueTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 48);          // Private Set but settable via Reflection
            Assert.AreEqual(48, internalValueTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 96);
            Assert.AreEqual(96, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalValueTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 192);
            Assert.AreEqual(192, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalValueTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 384);
            Assert.AreEqual(384, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalValueTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 728);
            Assert.AreEqual(728, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalValueTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 1536);
            Assert.AreEqual(1536, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)
        }

        [Test]
        public void ImmediatePropertySetValue_ReferenceType()
        {
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };
            var testObject5 = new TestObject { TestValue = 5 };
            var testObject6 = new TestObject { TestValue = 6 };
            var testObject7 = new TestObject { TestValue = 7 };
            var testObject8 = new TestObject { TestValue = 8 };

            // Reference type / Public
            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject2);
            Assert.AreSame(testObject2, publicReferenceTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject3);        // Private Set but settable via Reflection
            Assert.AreSame(testObject3, publicReferenceTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicReferenceTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject5);
            Assert.AreSame(testObject5, publicReferenceTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject6);
            Assert.AreSame(testObject6, publicReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject7);
            Assert.AreSame(testObject7, publicReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject8);
            Assert.AreSame(testObject8, publicReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            // Reference type / Internal
            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, internalReferenceTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject2);
            Assert.AreSame(testObject2, internalReferenceTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject3);           // Private Set but settable via Reflection
            Assert.AreSame(testObject3, internalReferenceTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject5);
            Assert.AreSame(testObject5, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalReferenceTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject6);
            Assert.AreSame(testObject6, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalReferenceTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject7);
            Assert.AreSame(testObject7, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalReferenceTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject8);
            Assert.AreSame(testObject8, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)
        }

        [Test]
        public void ImmediatePropertySetValue_ObjectType()
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

            var immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 1);
            Assert.AreEqual(1, publicObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 2);
            Assert.AreEqual(2, publicObjectTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 3);          // Private Set but settable via Reflection
            Assert.AreEqual(3, publicObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 4);
            Assert.AreEqual(4, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 5);
            Assert.AreEqual(5, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 6);
            Assert.AreEqual(6, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 7);
            Assert.AreEqual(7, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 8);
            Assert.AreEqual(8, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)


            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject2);
            Assert.AreSame(testObject2, publicObjectTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject3);        // Private Set but settable via Reflection
            Assert.AreSame(testObject3, publicObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject5);
            Assert.AreSame(testObject5, publicObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject6);
            Assert.AreSame(testObject6, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject7);
            Assert.AreSame(testObject7, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject8);
            Assert.AreSame(testObject8, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)


            // Object type / Internal
            var internalObjectTypeTestObject = new InternalObjectTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 1);
            Assert.AreEqual(1, internalObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 2);
            Assert.AreEqual(2, internalObjectTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 3);        // Private Set but settable via Reflection
            Assert.AreEqual(3, internalObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 4);
            Assert.AreEqual(4, internalObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 5);
            Assert.AreEqual(5, internalObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 6);
            Assert.AreEqual(6, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 7);
            Assert.AreEqual(7, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 8);
            Assert.AreEqual(8, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)


            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, internalObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject2);
            Assert.AreEqual(testObject2, internalObjectTypeTestObject.PublicVirtualPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject3);          // Private Set but settable via Reflection
            Assert.AreSame(testObject3, internalObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject4);
            Assert.AreSame(testObject4, internalObjectTypeTestObject._publicField);     // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject5);
            Assert.AreSame(testObject5, internalObjectTypeTestObject._publicField);     // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypeInternalGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject6);
            Assert.AreSame(testObject6, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypeProtectedGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject7);
            Assert.AreSame(testObject7, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject8);
            Assert.AreSame(testObject8, internalObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)
        }

        [Test]
        public void ImmediatePropertySetValue_Abstract()
        {
            var concreteTestObject = new ConcretePublicValueTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicValueTypePublicAbstractGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(concreteTestObject, 123456);
            Assert.AreEqual(123456, concreteTestObject.PublicAbstractGetSetProperty);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicConcreteGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(concreteTestObject, 456789);
            Assert.AreEqual(456789, concreteTestObject.PublicAbstractGetSetProperty);
        }

        [Test]
        public void ImmediatePropertySetValue_Static()
        {
            var testObject = new TestObject { TestValue = 1 };

            // Value type
            var immediateProperty = new ImmediateProperty(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, 12);
            Assert.AreEqual(12, PublicValueTypeTestClass.PublicStaticPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalValueTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, 24);
            Assert.AreEqual(24, InternalValueTypeTestClass.PublicStaticPropertyGetSet);

            // Reference type
            immediateProperty = new ImmediateProperty(PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, PublicReferenceTypeTestClass.PublicStaticPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, InternalReferenceTypeTestClass.PublicStaticPropertyGetSet);

            // Object type
            immediateProperty = new ImmediateProperty(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, 48);
            Assert.AreEqual(48, PublicObjectTypeTestClass.PublicStaticPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, 96);
            Assert.AreEqual(96, InternalObjectTypeTestClass.PublicStaticPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, PublicObjectTypeTestClass.PublicStaticPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(null, testObject);
            Assert.AreSame(testObject, InternalObjectTypeTestClass.PublicStaticPropertyGetSet);
        }

        [Test]
        public void ImmediatePropertySetValue_NestedTypes()
        {
            // Nested types
            var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { NestedTestValue = 1 };
            var immediateProperty = new ImmediateProperty(PublicNestedPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicNestedTypeTestObject, 12);
            Assert.AreEqual(12, publicNestedTypeTestObject.NestedTestValue);

            var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { NestedTestValue = 2 };
            immediateProperty = new ImmediateProperty(InternalNestedPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalNestedTypeTestObject, 24);
            Assert.AreEqual(24, internalNestedTypeTestObject.NestedTestValue);

            var protectedNestedTypeTestObject = new ProtectedNestedClass { NestedTestValue = 3 };
            immediateProperty = new ImmediateProperty(ProtectedNestedPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(protectedNestedTypeTestObject, 48);
            Assert.AreEqual(48, protectedNestedTypeTestObject.NestedTestValue);

            var privateNestedTypeTestObject = new PrivateNestedClass { NestedTestValue = 4 };
            immediateProperty = new ImmediateProperty(PrivateNestedPublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(privateNestedTypeTestObject, 96);
            Assert.AreEqual(96, privateNestedTypeTestObject.NestedTestValue);
        }

        [Test]
        public void ImmediatePropertySetValue_NullInstance()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<TargetException>(() => immediateProperty.SetValue(null, null));
        }

        [Test]
        public void ImmediatePropertySetValue_WrongInstance()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.Throws<InvalidCastException>(() => immediateProperty.SetValue(new PublicReferenceTypeTestClass(), null));
        }

        [Test]
        public void ImmediatePropertySetValue_WrongValue()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.Throws<InvalidCastException>(() => immediateProperty.SetValue(new PublicValueTypeTestClass(), new TestObject()));

            var immediateProperty2 = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            Assert.Throws<InvalidCastException>(() => immediateProperty2.SetValue(new PublicReferenceTypeTestClass(), 12));

            var immediateProperty3 = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            Assert.Throws<InvalidCastException>(() => immediateProperty3.SetValue(new PublicReferenceTypeTestClass(), new SmallObject()));
        }

        [Test]
        public void ImmediatePropertySetValue_NoSetter()
        {
            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.Throws<ArgumentException>(() => immediateProperty.SetValue(new PublicValueTypeTestClass(), 51));
        }

        #endregion

        #region Equals/HashCode/ToString

        [Test]
        public void ImmediatePropertyEquality()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.IsTrue(immediateProperty1.Equals(immediateProperty1));
            Assert.IsTrue(immediateProperty1.Equals(immediateProperty2));
            Assert.IsTrue(immediateProperty1.Equals((object)immediateProperty2));
            Assert.IsFalse(immediateProperty1.Equals(null));

            var immediateProperty3 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.IsFalse(immediateProperty1.Equals(immediateProperty3));
            Assert.IsFalse(immediateProperty1.Equals((object)immediateProperty3));
        }

        [Test]
        public void ImmediatePropertyHashCode()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.GetHashCode(), immediateProperty1.GetHashCode());
            Assert.AreEqual(immediateProperty1.GetHashCode(), immediateProperty2.GetHashCode());

            var immediateProperty3 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.GetHashCode(), immediateProperty3.GetHashCode());
        }

        [Test]
        public void ImmediatePropertyToString()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.ToString(), immediateProperty1.ToString());

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.ToString(), immediateProperty2.ToString());
        }

        #endregion
    }
}