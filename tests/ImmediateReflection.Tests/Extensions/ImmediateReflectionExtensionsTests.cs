using System;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateReflectionExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateReflectionExtensionsTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void GetImmediateTypeFromObject()
        {
            object testObject1 = new PublicValueTypeTestClass();
            ImmediateType immediateType = ImmediateReflectionExtensions.GetImmediateType(testObject1);
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateType.Type);

            PublicValueTypeTestClass testObject2 = new PublicValueTypeTestClass();
            immediateType = ImmediateReflectionExtensions.GetImmediateType(testObject2);
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateType.Type);

            TestStruct testObject3 = new TestStruct();
            immediateType = ImmediateReflectionExtensions.GetImmediateType(testObject3);
            Assert.AreEqual(typeof(TestStruct), immediateType.Type);

            int testObject4 = 12;
            immediateType = ImmediateReflectionExtensions.GetImmediateType(testObject4);
            Assert.AreEqual(typeof(int), immediateType.Type);
        }

        [Test]
        public void GetImmediateTypeFromObject_Throws()
        {
            object testObject = null;

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ImmediateReflectionExtensions.GetImmediateType(testObject));
        }

#if SUPPORTS_EXTENSIONS
        [Test]
        public void GetImmediateTypeFromType()
        {
            Type testType = typeof(PublicValueTypeTestClass);
            ImmediateType immediateType = testType.GetImmediateType();
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateType.Type);

            testType = typeof(object);
            immediateType = testType.GetImmediateType();
            Assert.AreEqual(typeof(object), immediateType.Type);

            testType = typeof(TestStruct);
            immediateType = testType.GetImmediateType();
            Assert.AreEqual(typeof(TestStruct), immediateType.Type);

            testType = typeof(int);
            immediateType = testType.GetImmediateType();
            Assert.AreEqual(typeof(int), immediateType.Type);
        }

        [Test]
        public void GetImmediateTypeFromType_Throws()
        {
            Type testType = null;

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => testType.GetImmediateType());
        }
#endif
    }
}