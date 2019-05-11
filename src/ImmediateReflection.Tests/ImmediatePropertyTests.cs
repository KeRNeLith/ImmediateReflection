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
                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass();

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicSetPropertyPropertyInfo, false);

                var internalValueTypeTestObject = new InternalValueTypeTestClass();

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);    // Private Get but gettable via Reflection
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicSetPropertyPropertyInfo, false);

                // Reference type
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicSetPropertyPropertyInfo, false);

                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);  // Private Get but gettable via Reflection
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicSetPropertyPropertyInfo, false);

                // Nested type
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass();
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicGetSetPropertyPropertyInfo, true);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass();
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicGetSetPropertyPropertyInfo, true);

                var protectedNestedTypeTestObject = new ProtectedNestedClass();
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicGetSetPropertyPropertyInfo, true);

                var privateNestedTypeTestObject = new PrivateNestedClass();
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicGetSetPropertyPropertyInfo, true);
            }
        }

        [TestCaseSource(nameof(CreateImmediatePropertyCanReadTestCases))]
        public void ImmediatePropertyCanRead([NotNull] object target, [NotNull] PropertyInfo property, bool expectedCanRead)
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

                // Nested type
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { NestedTestValue = 1 };
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicGetSetPropertyPropertyInfo, 1);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass { NestedTestValue = 2 };
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicGetSetPropertyPropertyInfo, 2);

                var protectedNestedTypeTestObject = new ProtectedNestedClass { NestedTestValue = 3 };
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicGetSetPropertyPropertyInfo, 3);

                var privateNestedTypeTestObject = new PrivateNestedClass { NestedTestValue = 4 };
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicGetSetPropertyPropertyInfo, 4);
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
                // Value type
                var publicValueTypeTestObject = new PublicValueTypeTestClass();

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicSetPropertyPropertyInfo, true);

                var internalValueTypeTestObject = new InternalValueTypeTestClass();

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicSetPropertyPropertyInfo, true);

                // Reference type
                var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(publicReferenceTypeTestObject, PublicReferenceTypePublicSetPropertyPropertyInfo, true);

                var internalReferenceTypeTestObject = new InternalReferenceTypeTestClass();

                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPropertyPropertyInfo, false);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo, true); // Private Set but settable via Reflection
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo, true);
                yield return new TestCaseData(internalReferenceTypeTestObject, InternalReferenceTypePublicSetPropertyPropertyInfo, true);

                // Nested type
                var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass();
                yield return new TestCaseData(publicNestedTypeTestObject, PublicNestedPublicGetSetPropertyPropertyInfo, true);

                var internalNestedTypeTestObject = new PublicTestClass.InternalNestedClass();
                yield return new TestCaseData(internalNestedTypeTestObject, InternalNestedPublicGetSetPropertyPropertyInfo, true);

                var protectedNestedTypeTestObject = new ProtectedNestedClass();
                yield return new TestCaseData(protectedNestedTypeTestObject, ProtectedNestedPublicGetSetPropertyPropertyInfo, true);

                var privateNestedTypeTestObject = new PrivateNestedClass();
                yield return new TestCaseData(privateNestedTypeTestObject, PrivateNestedPublicGetSetPropertyPropertyInfo, true);
            }
        }

        [TestCaseSource(nameof(CreateImmediatePropertyCanWriteTestCases))]
        public void ImmediatePropertyCanWrite([NotNull] object target, [NotNull] PropertyInfo property, bool expectedCanWrite)
        {
            var immediateProperty = new ImmediateProperty(property);
            Assert.AreEqual(expectedCanWrite, immediateProperty.CanWrite);
        }

        #endregion

        #region SetValue

        [Test]
        public void ImmediatePropertySetValue()
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


            // Reference type / Public
            var testObject1 = new TestObject { TestValue = 1 };
            var testObject2 = new TestObject { TestValue = 2 };
            var testObject3 = new TestObject { TestValue = 3 };
            var testObject4 = new TestObject { TestValue = 4 };

            var publicReferenceTypeTestObject = new PublicReferenceTypeTestClass();

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject1);
            Assert.AreSame(testObject1, publicReferenceTypeTestObject.PublicPropertyGetSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject2);          // Private Set but settable via Reflection
            Assert.AreSame(testObject2, publicReferenceTypeTestObject.PublicPropertyGetPrivateSet);

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject3);
            Assert.AreSame(testObject3, publicReferenceTypeTestObject._publicField);   // => Setter set the public field value (for check)

            immediateProperty = new ImmediateProperty(PublicReferenceTypePublicSetPropertyPropertyInfo);
            immediateProperty.SetValue(publicReferenceTypeTestObject, testObject4);
            Assert.AreSame(testObject4, publicReferenceTypeTestObject._publicField2);   // => Setter set the public field value (for check)

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


            // Nested type
            var publicNestedTypeTestObject = new PublicTestClass.PublicNestedClass { NestedTestValue = 1 };
            immediateProperty = new ImmediateProperty(PublicNestedPublicGetSetPropertyPropertyInfo);
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
            Assert.AreEqual(immediateProperty1, immediateProperty1);
            Assert.AreEqual(immediateProperty1, immediateProperty2);
            Assert.IsTrue(immediateProperty1.Equals((object)immediateProperty2));
            Assert.IsFalse(immediateProperty1.Equals(null));

            var immediateProperty3 = new ImmediateProperty(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreNotEqual(immediateProperty1, immediateProperty3);
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