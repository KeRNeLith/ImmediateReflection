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
        protected static PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
            typeof(PrivateNestedClass).GetProperty(nameof(PrivateNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        #endregion

        #endregion

        [Test]
        public void ImmediatePropertyInfo()
        {
            var immediateProperty1 = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.Name, immediateProperty1.Name);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo.PropertyType, immediateProperty1.PropertyType);
            Assert.AreEqual(PublicValueTypePublicGetSetPropertyPropertyInfo, immediateProperty1.PropertyInfo);

            var immediateProperty2 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1.PropertyInfo, immediateProperty2.PropertyInfo);

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ImmediateProperty(null));
        }

        #region CanRead

        private static IEnumerable<TestCaseData> CreateImmediatePropertyCanReadTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Value type

                // Value type
                yield return new TestCaseData(PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicValueTypePublicSetPropertyPropertyInfo, false);

                yield return new TestCaseData(InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalValueTypePublicSetPropertyPropertyInfo, false);

                #endregion

                #region Reference type

                // Reference type
                yield return new TestCaseData(PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicReferenceTypePublicSetPropertyPropertyInfo, false);

                yield return new TestCaseData(InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalReferenceTypePublicSetPropertyPropertyInfo, false);

                #endregion

                #region Object type

                // Object type
                yield return new TestCaseData(PublicObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(PublicObjectTypePublicSetPropertyPropertyInfo, false);

                yield return new TestCaseData(InternalObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(InternalObjectTypePublicSetPropertyPropertyInfo, false);

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
                #region Value type

                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass(2, 3)
                {
                    PublicPropertyGetSet = 1,
                    PublicPropertyPrivateGetSet = 4
                };

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPropertyPropertyInfo, 2);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, 3);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, 4);    // Private Get but gettable via Reflection

                var internalValueTypeTestObject = new InternalValueTypeTestClass(5, 6)
                {
                    PublicPropertyGetSet = 4,
                    PublicPropertyPrivateGetSet = 7
                };

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetSetPropertyPropertyInfo, 4);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPropertyPropertyInfo, 5);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, 6);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, 7);    // Private Get but gettable via Reflection

                #endregion

                #region Reference type

                // Reference type
                var testObject1 = new TestObject { TestValue = 1 };
                var testObject2 = new TestObject { TestValue = 2 };
                var testObject3 = new TestObject { TestValue = 3 };
                var testObject4 = new TestObject { TestValue = 4 };
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass(testObject2, testObject3)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicPropertyPrivateGetSet = testObject4
                };

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, testObject4);  // Private Get but gettable via Reflection

                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass(testObject2, testObject3)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicPropertyPrivateGetSet = testObject4
                };

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, testObject4);  // Private Get but gettable via Reflection

                #endregion

                #region Object type

                // Object type
                var publicObjectTypeTestObject1 = new PublicObjectTypeTestClass(2, 3)
                {
                    PublicPropertyGetSet = 1,
                    PublicPropertyPrivateGetSet = 4
                };

                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetPropertyPropertyInfo, 2);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, 3);
                yield return new TestCaseData(publicObjectTypeTestObject1, PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, 4);  // Private Get but gettable via Reflection

                var publicObjectTypeTestObject2 = new PublicObjectTypeTestClass(testObject2, testObject3)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicPropertyPrivateGetSet = testObject4
                };

                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(publicObjectTypeTestObject2, PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, testObject4);  // Private Get but gettable via Reflection

                var internalObjectTypeTestObject1 = new InternalObjectTypeTestClass(2, 3)
                {
                    PublicPropertyGetSet = 1,
                    PublicPropertyPrivateGetSet = 4
                };

                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetSetPropertyPropertyInfo, 1);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetPropertyPropertyInfo, 2);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, 3);
                yield return new TestCaseData(internalObjectTypeTestObject1, InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, 4);  // Private Get but gettable via Reflection

                var internalObjectTypeTestObject2 = new InternalObjectTypeTestClass(testObject2, testObject3)
                {
                    PublicPropertyGetSet = testObject1,
                    PublicPropertyPrivateGetSet = testObject4
                };

                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetSetPropertyPropertyInfo, testObject1);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetPropertyPropertyInfo, testObject2);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, testObject3);
                yield return new TestCaseData(internalObjectTypeTestObject2, InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, testObject4);  // Private Get but gettable via Reflection

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
                #region Value type

                // Value type
                yield return new TestCaseData(PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicValueTypePublicSetPropertyPropertyInfo, true);

                yield return new TestCaseData(InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalValueTypePublicSetPropertyPropertyInfo, true);

                #endregion

                #region Reference type

                // Reference type
                yield return new TestCaseData(PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicReferenceTypePublicSetPropertyPropertyInfo, true);

                yield return new TestCaseData(InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalReferenceTypePublicSetPropertyPropertyInfo, true);

                #endregion

                #region Object type

                // Object type
                yield return new TestCaseData(PublicObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(PublicObjectTypePublicSetPropertyPropertyInfo, true);

                yield return new TestCaseData(InternalObjectTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(InternalObjectTypePublicSetPropertyPropertyInfo, true);

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
        public void ImmediatePropertySetValue_ValueType()
        {
            // Value type / Public
            var publicValueTypeTestObject = new PublicValueTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 12);
            Assert.AreEqual(12, publicValueTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 24);          // Private Set but settable via Reflection
            Assert.AreEqual(24, publicValueTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 48);
            Assert.AreEqual(48, publicValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicValueTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicValueTypeTestObject, 96);
            Assert.AreEqual(96, publicValueTypeTestObject._publicField2);   // => Setter set the public field value (for check)

            // Value type / Internal
            var internalValueTypeTestObject = new InternalValueTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalValueTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 12);
            Assert.AreEqual(12, internalValueTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalValueTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 24);          // Private Set but settable via Reflection
            Assert.AreEqual(24, internalValueTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalValueTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 48);
            Assert.AreEqual(48, internalValueTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalValueTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalValueTypeTestObject, 96);
            Assert.AreEqual(96, internalValueTypeTestObject._publicField2);   // => Setter set the public field value (for check)
        }

        [Test]
        public void ImmediatePropertySetValue_ReferenceType()
        {
            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject2);        // Private Set but settable via Reflection
            Assert.AreSame(testObject2, publicReferenceTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject3);
            Assert.AreSame(testObject3, publicReferenceTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicReferenceTypeTestObject._publicField2);  // => Setter set the public field value (for check)

            // Reference type / Internal
            var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, internalReferenceTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject2);           // Private Set but settable via Reflection
            Assert.AreSame(testObject2, internalReferenceTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject3);
            Assert.AreSame(testObject3, internalReferenceTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalReferenceTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, internalReferenceTypeTestObject._publicField2);   // => Setter set the public field value (for check)
        }

        [Test]
        public void ImmediatePropertySetValue_ObjectType()
        {
            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };

            // Object type / Public
            var publicObjectTypeTestObject = new PublicObjectTypeTestClass();

            var immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 1);
            Assert.AreEqual(1, publicObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 2);          // Private Set but settable via Reflection
            Assert.AreEqual(2, publicObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 3);
            Assert.AreEqual(3, publicObjectTypeTestObject._publicField);    // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, 4);
            Assert.AreEqual(4, publicObjectTypeTestObject._publicField2);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject2);        // Private Set but settable via Reflection
            Assert.AreSame(testObject2, publicObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject3);
            Assert.AreSame(testObject3, publicObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicObjectTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicObjectTypeTestObject._publicField2);  // => Setter set the public field value (for check)

            // Object type / Internal
            var internalObjectTypeTestObject = new InternalObjectTypeTestClass();

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 1);
            Assert.AreEqual(1, internalObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 2);        // Private Set but settable via Reflection
            Assert.AreEqual(2, internalObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 3);
            Assert.AreEqual(3, internalObjectTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, 4);
            Assert.AreEqual(4, internalObjectTypeTestObject._publicField2);  // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject1);
            Assert.AreSame(testObject1, internalObjectTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject2);          // Private Set but settable via Reflection
            Assert.AreSame(testObject2, internalObjectTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject3);
            Assert.AreSame(testObject3, internalObjectTypeTestObject._publicField);     // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(InternalObjectTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(internalObjectTypeTestObject, testObject4);
            Assert.AreSame(testObject4, internalObjectTypeTestObject._publicField2);    // => Setter set the public field value (for check)
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
    }
}