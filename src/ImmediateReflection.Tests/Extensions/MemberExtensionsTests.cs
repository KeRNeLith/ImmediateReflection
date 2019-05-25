#if SUPPORTS_EXTENSIONS
using System;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="MemberExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class MemberExtensionsTests : ImmediateReflectionTestsBase
    {
        #region Getter

        [Test]
        public void TryCreateGetter()
        {
            Assert.IsTrue(TestStructTestPropertyPropertyInfo.TryCreateGetter(out Func<TestStruct, int> _));

            Assert.IsTrue(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicGetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeInternalGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeProtectedGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePrivateGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicValueTypeTestClass, int> _));
        }

        [Test]
        public void CreateGetter_ValueType()
        {
            var testObject = new TestStruct
            {
                TestValue = 12
            };

            Func<TestStruct, int> getter = TestStructTestPropertyPropertyInfo.CreateGetter<TestStruct, int>();
            Assert.AreEqual(12, getter(testObject));
        }

        [Test]
        public void CreateGetter_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass(45, 12)
            {
                PublicPropertyGetSet = 51,
                PublicPropertyPrivateGetSet = 25
            };

            Func<PublicValueTypeTestClass, int> getter = PublicValueTypePublicGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            Assert.AreEqual(51, getter(testObject));

            getter = PublicValueTypePublicGetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            Assert.AreEqual(45, getter(testObject));

            getter = PublicValueTypePublicPrivateGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            Assert.AreEqual(25, getter(testObject));

            getter = PublicValueTypePublicGetPrivateSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            Assert.AreEqual(12, getter(testObject));

            getter = PublicValueTypePublicSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            Assert.AreEqual(0, getter(testObject)); // No get

            getter = PublicValueTypeStaticPublicGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 1;
            Assert.AreEqual(1, getter(null));

            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 2;
            Assert.AreEqual(2, getter(testObject));

            getter = PublicValueTypeInternalGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            testObject.InternalPropertyGetSet = 72;
            Assert.AreEqual(72, getter(testObject));

            getter = PublicValueTypeProtectedGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            testObject._publicField = 92;
            Assert.AreEqual(92, getter(testObject));

            getter = PublicValueTypePrivateGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, int>();
            testObject._publicField = 112;
            Assert.AreEqual(112, getter(testObject));
        }

        [Test]
        public void CreateGetter_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null).CreateGetter<TestStruct, int>());
        }

        [Test]
        public void CreateGetter_WrongType()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateGetter<PublicObjectTypeTestClass, int>());
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateGetter<PublicValueTypeTestClass, object>());
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateGetter<PublicObjectTypeTestClass, object>());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateGetter(out Func<PublicObjectTypeTestClass, int> _));
        }

        #endregion

        #region Setter

        [Test]
        public void TryCreateSetter()
        {
            Assert.IsFalse(TestStructTestPropertyPropertyInfo.TryCreateSetter(out Action<TestStruct, int> _));  // Cannot set on struct

            Assert.IsTrue(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicGetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePublicSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeInternalGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypeProtectedGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(PublicValueTypePrivateGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicValueTypeTestClass, int> _));
        }

        [Test]
        public void CreateSetter_ValueType()
        {
            var testObject = new TestStruct
            {
                TestValue = 12
            };

            Action<TestStruct, int> setter = TestStructTestPropertyPropertyInfo.CreateSetter<TestStruct, int>();
            setter(testObject, 25);
            Assert.AreEqual(12, testObject.TestValue);  // Value can't be set on struct
        }

        [Test]
        public void CreateSetter_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass();

            Action<PublicValueTypeTestClass, int> setter = PublicValueTypePublicGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 51);
            Assert.AreEqual(51, testObject.PublicPropertyGetSet);

            setter = PublicValueTypePublicGetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 45);
            Assert.AreEqual(0, testObject.PublicPropertyGet);   // Not set

            setter = PublicValueTypePublicPrivateGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 25);
            Assert.AreEqual(25, testObject._publicField);       // Store the result in the public field

            setter = PublicValueTypePublicGetPrivateSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 12);
            Assert.AreEqual(12, testObject.PublicPropertyGetPrivateSet);

            setter = PublicValueTypePublicSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 42);
            Assert.AreEqual(42, testObject._publicField);       // Store the result in the public field

            setter = PublicValueTypeStaticPublicGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(null, 1);
            Assert.AreEqual(1, PublicValueTypeTestClass.PublicStaticPropertyGetSet);
            setter(testObject, 2);
            Assert.AreEqual(2, PublicValueTypeTestClass.PublicStaticPropertyGetSet);

            setter = PublicValueTypeInternalGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 72);
            Assert.AreEqual(72, testObject.InternalPropertyGetSet);

            setter = PublicValueTypeProtectedGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 92);
            Assert.AreEqual(92, testObject._publicField);       // Store the result in the public field

            setter = PublicValueTypePrivateGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, int>();
            setter(testObject, 112);
            Assert.AreEqual(112, testObject._publicField);      // Store the result in the public field
        }

        [Test]
        public void CreateSetter_Throws()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ((PropertyInfo)null).CreateSetter<TestStruct, int>());
        }

        [Test]
        public void CreateSetter_WrongType()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateSetter<PublicObjectTypeTestClass, int>());
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateSetter<PublicValueTypeTestClass, object>());
            Assert.Throws<ArgumentException>(() => PublicValueTypePublicGetSetPropertyPropertyInfo.CreateSetter<PublicObjectTypeTestClass, object>());
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(PublicValueTypePublicGetSetPropertyPropertyInfo.TryCreateSetter(out Action<PublicObjectTypeTestClass, int> _));
        }

        #endregion
    }
}
#endif