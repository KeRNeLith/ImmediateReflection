using System;
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

        #region Strongly typed

        [Test]
        public void TryCreateGetter_StronglyTyped()
        {
            Assert.IsTrue(MemberExtensions.TryCreateGetter(TestStructTestPropertyPropertyInfo, out GetterDelegate<TestStruct, int> _));

            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeInternalGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeProtectedGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePrivateGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, int> _));
        }

        [Test]
        public void CreateGetter_StronglyTyped_ValueType()
        {
            var testObject = new TestStruct
            {
                TestValue = 12
            };

            GetterDelegate<TestStruct, int> getter = MemberExtensions.CreateGetter<TestStruct, int>(TestStructTestPropertyPropertyInfo);
            Assert.AreEqual(12, getter(testObject));

            getter = MemberExtensions.CreateGetter<TestStruct, int>(TestStructStaticTestPropertyPropertyInfo);
            TestStruct.TestStaticValue = 25;
            Assert.AreEqual(25, getter(default));
        }

        [Test]
        public void CreateGetter_StronglyTyped_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass(45, 12)
            {
                PublicPropertyGetSet = 51,
                PublicPropertyPrivateGetSet = 25
            };

            GetterDelegate<PublicValueTypeTestClass, int> getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(51, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreEqual(45, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(25, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            Assert.AreEqual(12, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePublicSetPropertyPropertyInfo);
            Assert.AreEqual(0, getter(testObject)); // No get

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 1;
            Assert.AreEqual(1, getter(null));

            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 2;
            Assert.AreEqual(2, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            testObject.InternalPropertyGetSet = 72;
            Assert.AreEqual(72, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            testObject._publicField = 92;
            Assert.AreEqual(92, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass, int>(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            testObject._publicField = 112;
            Assert.AreEqual(112, getter(testObject));
        }

        [Test]
        public void CreateGetter_StronglyTyped_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.CreateGetter<TestStruct, int>(null));
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.TryCreateGetter(null, out GetterDelegate<TestStruct, int> _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void CreateGetter_StronglyTyped_WrongType()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateGetter<PublicObjectTypeTestClass, int>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateGetter<PublicValueTypeTestClass, float>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateGetter<PublicObjectTypeTestClass, float>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass, float> _));
            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicObjectTypeTestClass, float> _));
        }

        #endregion

        #region Partially strongly typed

        [Test]
        public void TryCreateGetter()
        {
            Assert.IsTrue(MemberExtensions.TryCreateGetter(TestStructTestPropertyPropertyInfo, out GetterDelegate<TestStruct> _));

            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeInternalGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypeProtectedGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateGetter(PublicValueTypePrivateGetSetPropertyPropertyInfo, out GetterDelegate<PublicValueTypeTestClass> _));
        }

        [Test]
        public void CreateGetter_ValueType()
        {
            var testObject = new TestStruct
            {
                TestValue = 12
            };

            GetterDelegate<TestStruct> getter = MemberExtensions.CreateGetter<TestStruct>(TestStructTestPropertyPropertyInfo);
            Assert.AreEqual(12, getter(testObject));

            getter = MemberExtensions.CreateGetter<TestStruct>(TestStructStaticTestPropertyPropertyInfo);
            TestStruct.TestStaticValue = 25;
            Assert.AreEqual(25, getter(default));
        }

        [Test]
        public void CreateGetter_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass(45, 12)
            {
                PublicPropertyGetSet = 51,
                PublicPropertyPrivateGetSet = 25
            };

            GetterDelegate<PublicValueTypeTestClass> getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePublicGetSetPropertyPropertyInfo);
            Assert.AreEqual(51, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePublicGetPropertyPropertyInfo);
            Assert.AreEqual(45, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            Assert.AreEqual(25, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            Assert.AreEqual(12, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePublicSetPropertyPropertyInfo);
            Assert.AreEqual(0, getter(testObject)); // No get

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 1;
            Assert.AreEqual(1, getter(null));

            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 2;
            Assert.AreEqual(2, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            testObject.InternalPropertyGetSet = 72;
            Assert.AreEqual(72, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            testObject._publicField = 92;
            Assert.AreEqual(92, getter(testObject));

            getter = MemberExtensions.CreateGetter<PublicValueTypeTestClass>(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            testObject._publicField = 112;
            Assert.AreEqual(112, getter(testObject));

            // Reference Type
            var testObject2 = new PublicReferenceTypeTestClass();

            GetterDelegate<PublicReferenceTypeTestClass> getter2 = MemberExtensions.CreateGetter<PublicReferenceTypeTestClass>(PublicReferenceTypePublicSetPropertyPropertyInfo);
            Assert.AreEqual(null, getter2(testObject2)); // No get
        }

        [Test]
        public void CreateGetter_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.CreateGetter<TestStruct>(null));
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.TryCreateGetter(null, out GetterDelegate<TestStruct> _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void CreateGetter_WrongType()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateGetter<PublicObjectTypeTestClass>(PublicValueTypePublicGetSetPropertyPropertyInfo));

            Assert.IsFalse(MemberExtensions.TryCreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out GetterDelegate <PublicObjectTypeTestClass> _));
        }

        #endregion

        #endregion

        #region Setter

        #region Strongly typed

        [Test]
        public void TryCreateSetter_StronglyTyped()
        {
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeInternalGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeProtectedGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePrivateGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, int> _));
        }

        [Test]
        public void CreateSetter_StronglyTyped_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass();

            SetterDelegate<PublicValueTypeTestClass, int> setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetSetPropertyPropertyInfo);
            setter(testObject, 51);
            Assert.AreEqual(51, testObject.PublicPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetPropertyPropertyInfo);
            setter(testObject, 45);
            Assert.AreEqual(0, testObject.PublicPropertyGet);   // Not set

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            setter(testObject, 25);
            Assert.AreEqual(25, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            setter(testObject, 12);
            Assert.AreEqual(12, testObject.PublicPropertyGetPrivateSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePublicSetPropertyPropertyInfo);
            setter(testObject, 42);
            Assert.AreEqual(42, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            setter(null, 1);
            Assert.AreEqual(1, PublicValueTypeTestClass.PublicStaticPropertyGetSet);
            setter(testObject, 2);
            Assert.AreEqual(2, PublicValueTypeTestClass.PublicStaticPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            setter(testObject, 72);
            Assert.AreEqual(72, testObject.InternalPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            setter(testObject, 92);
            Assert.AreEqual(92, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            setter(testObject, 112);
            Assert.AreEqual(112, testObject._publicField);      // Store the result in the public field
        }

        [Test]
        public void CreateSetter_StronglyTyped_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.CreateSetter<PublicValueTypeTestClass, int>(null));
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.TryCreateSetter(null, out SetterDelegate<PublicValueTypeTestClass, int> _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void CreateSetter_StronglyTyped_WrongType()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateSetter<PublicObjectTypeTestClass, int>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateSetter<PublicValueTypeTestClass, float>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateSetter<PublicObjectTypeTestClass, float>(PublicValueTypePublicGetSetPropertyPropertyInfo));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicObjectTypeTestClass, int> _));
            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass, float> _));
            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicObjectTypeTestClass, float> _));
        }

        #endregion

        #region Partially strongly typed

        [Test]
        public void TryCreateSetter()
        {
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePublicSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeInternalGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypeProtectedGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
            Assert.IsTrue(MemberExtensions.TryCreateSetter(PublicValueTypePrivateGetSetPropertyPropertyInfo, out SetterDelegate<PublicValueTypeTestClass> _));
        }

        [Test]
        public void CreateSetter_ReferenceType()
        {
            var testObject = new PublicValueTypeTestClass();

            SetterDelegate<PublicValueTypeTestClass> setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePublicGetSetPropertyPropertyInfo);
            setter(testObject, 51);
            Assert.AreEqual(51, testObject.PublicPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePublicGetPropertyPropertyInfo);
            setter(testObject, 45);
            Assert.AreEqual(0, testObject.PublicPropertyGet);   // Not set

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePublicPrivateGetSetPropertyPropertyInfo);
            setter(testObject, 25);
            Assert.AreEqual(25, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePublicGetPrivateSetPropertyPropertyInfo);
            setter(testObject, 12);
            Assert.AreEqual(12, testObject.PublicPropertyGetPrivateSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePublicSetPropertyPropertyInfo);
            setter(testObject, 42);
            Assert.AreEqual(42, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypeStaticPublicGetSetPropertyPropertyInfo);
            setter(null, 1);
            Assert.AreEqual(1, PublicValueTypeTestClass.PublicStaticPropertyGetSet);
            setter(testObject, 2);
            Assert.AreEqual(2, PublicValueTypeTestClass.PublicStaticPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypeInternalGetSetPropertyPropertyInfo);
            setter(testObject, 72);
            Assert.AreEqual(72, testObject.InternalPropertyGetSet);

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypeProtectedGetSetPropertyPropertyInfo);
            setter(testObject, 92);
            Assert.AreEqual(92, testObject._publicField);       // Store the result in the public field

            setter = MemberExtensions.CreateSetter<PublicValueTypeTestClass>(PublicValueTypePrivateGetSetPropertyPropertyInfo);
            setter(testObject, 112);
            Assert.AreEqual(112, testObject._publicField);      // Store the result in the public field
        }

        [Test]
        public void CreateSetter_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.CreateSetter<PublicValueTypeTestClass>(null));
            Assert.Throws<ArgumentNullException>(() => MemberExtensions.TryCreateSetter(null, out SetterDelegate<PublicValueTypeTestClass> _));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Test]
        public void CreateSetter_WrongType()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => MemberExtensions.CreateSetter<PublicObjectTypeTestClass>(PublicValueTypePublicGetSetPropertyPropertyInfo));

            Assert.IsFalse(MemberExtensions.TryCreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, out SetterDelegate<PublicObjectTypeTestClass> _));
        }

        #endregion

        #endregion
    }
}