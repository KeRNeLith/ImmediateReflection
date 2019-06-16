using System;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="MethodExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class MethodExtensionsTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        private struct TestMethodStruct
        {
            public int Property { get; set; }
        }

        private class TestMethodClass
        {
            public int Property { get; set; }
        }

        private struct MethodsTestStruct
        {
            public event EventHandler MethodCalled;

            public void VoidMethod()
            {
                MethodCalled?.Invoke(this, EventArgs.Empty);
            }
        }

        private class MethodsTestClass
        {
            public int VoidMethodCalls { get; set; }
            public int IntMethodCalls { get; set; }
            public int TestStructMethodCalls { get; set; }
            public int TestObjectMethodCalls { get; set; }
            public int VoidIntMethodCalls { get; set; }
            public int VoidIntTestStructMethodCalls { get; set; }
            public int VoidIntRefTestStructMethodCalls { get; set; }
            public int VoidIntOutTestStructMethodCalls { get; set; }
            public int VoidIntTestObjectMethodCalls { get; set; }
            public int VoidIntRefTestObjectMethodCalls { get; set; }
            public int VoidIntOutTestObjectMethodCalls { get; set; }
            public int VoidIntFloatsMethodCalls { get; set; }
            public int VoidOverload1MethodCalls { get; set; }
            public int VoidOverload2MethodCalls { get; set; }
            public int VoidVirtualMethodCalls { get; set; }
            public int VoidThrowMethodCalls { get; set; }

            public int VoidInternalMethodCalls { get; set; }

            public void VoidMethod()
            {
                ++VoidMethodCalls;
            }

            public int IntMethod()
            {
                ++IntMethodCalls;
                return 12;
            }

            public TestMethodStruct TestStructMethod()
            {
                ++TestStructMethodCalls;
                return new TestMethodStruct { Property = 12 };
            }

            public TestMethodClass TestObjectMethod()
            {
                ++TestObjectMethodCalls;
                return new TestMethodClass { Property = 12 };
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidIntMethod(int param)
            {
                ++VoidIntMethodCalls;
            }

            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            public void VoidIntTestStructMethod(int param1, TestMethodStruct param2)
            {
                Assert.IsNotNull(param2);
                ++VoidIntTestStructMethodCalls;
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidIntRefTestStructMethod(int param1, ref TestMethodStruct param2)
            {
                ++VoidIntRefTestStructMethodCalls;
                Assert.IsNotNull(param2);
                param2.Property = 42;
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidIntOutTestStructMethod(int param1, out TestMethodStruct param2)
            {
                ++VoidIntOutTestStructMethodCalls;
                param2 = new TestMethodStruct { Property = 45 };
            }

            // ReSharper disable UnusedParameter.Local
            public void VoidIntTestObjectMethod(int param1, TestMethodClass param2)
            // ReSharper restore UnusedParameter.Local
            {
                ++VoidIntTestObjectMethodCalls;
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidIntRefTestObjectMethod(int param1, ref TestMethodClass param2)
            {
                ++VoidIntRefTestObjectMethodCalls;
                if (param2 is null)
                    param2 = new TestMethodClass { Property = 33 };
                else
                    param2.Property = 51;
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidIntOutTestObjectMethod(int param1, out TestMethodClass param2)
            {
                ++VoidIntOutTestObjectMethodCalls;
                param2 = new TestMethodClass { Property = 52 };
            }

            public void VoidOverloadMethod()
            {
                ++VoidOverload1MethodCalls;
            }

            // ReSharper disable once UnusedParameter.Local
            public void VoidOverloadMethod(int param)
            {
                ++VoidOverload2MethodCalls;
            }

            // ReSharper disable UnusedParameter.Local
            public void VoidIntParamsMethod(int param, params float[] floats)
            // ReSharper restore UnusedParameter.Local
            {
                ++VoidIntFloatsMethodCalls;
            }

            public virtual void VoidVirtualMethod()
            {
                ++VoidVirtualMethodCalls;
            }

            public void VoidThrowMethod()
            {
                ++VoidThrowMethodCalls;
                throw new InvalidOperationException("Method throws.");
            }

            internal void VoidInternalMethod()
            {
                ++VoidInternalMethodCalls;
            }

            public static int VoidStaticMethodCalls { get; set; }

            public static void VoidStaticMethod()
            {
                ++VoidStaticMethodCalls;
            }
        }

        private class MethodsTestInheritedClass : MethodsTestClass
        {
            public int VoidVirtualOverrideMethodCalls { get; set; }

            public override void VoidVirtualMethod()
            {
                ++VoidVirtualOverrideMethodCalls;
            }
        }

        // ReSharper disable once UnusedTypeParameter
        private class MethodsTestTemplateClass<TTemplate>
        {
            public int VoidMethodCalls { get; set; }

            public void VoidMethod()
            {
                ++VoidMethodCalls;
            }
        }

        private static class MethodsTestStaticClass
        {
            public static int VoidMethodCalls { get; set; }

            public static void VoidMethod()
            {
                ++VoidMethodCalls;
            }
        }

        #endregion

        #region Test helpers

        #region MethodsTestStruct

        [NotNull]
        private static readonly MethodInfo TestStructVoidMethodInfo = typeof(MethodsTestStruct).GetMethod(nameof(MethodsTestStruct.VoidMethod)) ?? throw new AssertionException("Cannot find method.");

        #endregion

        #region MethodsTestClass

        [NotNull]
        private static readonly MethodInfo TestClassVoidMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassIntMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.IntMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassTestStructMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.TestStructMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassTestObjectMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.TestObjectMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntTestStructMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntTestStructMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntRefTestStructMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntRefTestStructMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntOutTestStructMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntOutTestStructMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntTestObjectMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntTestObjectMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntRefTestObjectMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntRefTestObjectMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntOutTestObjectMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntOutTestObjectMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidIntParamsMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidIntParamsMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidOverload1MethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidOverloadMethod), Type.EmptyTypes) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidOverload2MethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidOverloadMethod), new []{ typeof(int) }) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidVirtualMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidVirtualMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidThrowMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidThrowMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidInternalMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidInternalMethod), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestClassVoidStaticMethodInfo = typeof(MethodsTestClass).GetMethod(nameof(MethodsTestClass.VoidStaticMethod)) ?? throw new AssertionException("Cannot find method.");

        #endregion

        #region MethodsTestInheritedClass

        [NotNull]
        private static readonly MethodInfo TestInheritedClassVoidVirtualOverrideMethodInfo = typeof(MethodsTestInheritedClass).GetMethod(nameof(MethodsTestClass.VoidVirtualMethod)) ?? throw new AssertionException("Cannot find method.");

        #endregion

        #region MethodsTestTemplateClass

        [NotNull]
        private static readonly MethodInfo TestOpenTemplateClassVoidMethodInfo = typeof(MethodsTestTemplateClass<>).GetMethod(nameof(MethodsTestTemplateClass<object>.VoidMethod)) ?? throw new AssertionException("Cannot find method.");

        [NotNull]
        private static readonly MethodInfo TestTemplateClassVoidMethodInfo = typeof(MethodsTestTemplateClass<int>).GetMethod(nameof(MethodsTestTemplateClass<object>.VoidMethod)) ?? throw new AssertionException("Cannot find method.");

        #endregion

        #region MethodsTestStaticClass

        [NotNull]
        private static readonly MethodInfo TestStaticClassVoidStaticMethodInfo = typeof(MethodsTestStaticClass).GetMethod(nameof(MethodsTestStaticClass.VoidMethod)) ?? throw new AssertionException("Cannot find method.");

        #endregion

        #endregion

        [Test]
        public void CreateMethod_ReferenceType()
        {
            var testObject = new MethodsTestClass();

            MethodDelegate methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidMethodCalls);
            Assert.IsNull(methodDelegate(testObject, MethodExtensions.EmptyArgs));
            Assert.AreEqual(2, testObject.VoidMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassIntMethodInfo);
            Assert.IsNotNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.IntMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassTestStructMethodInfo);
            Assert.IsNotNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.TestStructMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassTestObjectMethodInfo);
            Assert.IsNotNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.TestObjectMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntMethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12 }));
            Assert.AreEqual(1, testObject.VoidIntMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntTestStructMethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, new TestMethodStruct() }));
            Assert.AreEqual(1, testObject.VoidIntTestStructMethodCalls);

            Assert.IsNull(methodDelegate(testObject, new object[] { 12, null /* struct can't be null but... */ }));
            Assert.AreEqual(2, testObject.VoidIntTestStructMethodCalls);


            var args = new object[] { 12, new TestMethodStruct { Property = 12 } };
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntRefTestStructMethodInfo);
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(1, testObject.VoidIntRefTestStructMethodCalls);
            Assert.AreEqual(42, ((TestMethodStruct)args[1]).Property);

            args = new object[] { 12, null /* struct can't be null but... */ };
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(2, testObject.VoidIntRefTestStructMethodCalls);
            Assert.AreEqual(42, ((TestMethodStruct)args[1]).Property);


            args = new object[] { 12, null };
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntOutTestStructMethodInfo);
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(1, testObject.VoidIntOutTestStructMethodCalls);
            Assert.AreEqual(45, ((TestMethodStruct)args[1]).Property);

            args = new object[] { 12, new TestMethodStruct { Property = 25 } /* initialized out parameter is useless but... */ };
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(2, testObject.VoidIntOutTestStructMethodCalls);
            Assert.AreEqual(45, ((TestMethodStruct)args[1]).Property);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntTestObjectMethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, new TestMethodClass() }));
            Assert.AreEqual(1, testObject.VoidIntTestObjectMethodCalls);

            Assert.IsNull(methodDelegate(testObject, new object[] { 12, null }));
            Assert.AreEqual(2, testObject.VoidIntTestObjectMethodCalls);


            var testParamRefClass = new TestMethodClass();
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntRefTestObjectMethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, testParamRefClass }));
            Assert.AreEqual(1, testObject.VoidIntRefTestObjectMethodCalls);
            Assert.AreEqual(51, testParamRefClass.Property);

            args = new object[] { 12, null };
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(2, testObject.VoidIntRefTestObjectMethodCalls);
            Assert.AreEqual(33, ((TestMethodClass)args[1]).Property);


            args = new object[] { 12, null };
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntOutTestObjectMethodInfo);
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(1, testObject.VoidIntOutTestObjectMethodCalls);
            Assert.AreEqual(52, ((TestMethodClass)args[1]).Property);

            args = new object[] { 12, new TestMethodClass { Property = 25 } /* initialized out parameter is useless but... */ };
            Assert.IsNull(methodDelegate(testObject, args));
            Assert.AreEqual(2, testObject.VoidIntOutTestObjectMethodCalls);
            Assert.AreEqual(52, ((TestMethodClass)args[1]).Property);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntParamsMethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, new float[] {} }));
            Assert.AreEqual(1, testObject.VoidIntFloatsMethodCalls);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, new[] { 12.5f } }));
            Assert.AreEqual(2, testObject.VoidIntFloatsMethodCalls);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12, new[] { 12.5f, 24.5f, 48.5f } }));
            Assert.AreEqual(3, testObject.VoidIntFloatsMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidOverload1MethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidOverload1MethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidOverload2MethodInfo);
            Assert.IsNull(methodDelegate(testObject, new object[] { 12 }));
            Assert.AreEqual(1, testObject.VoidOverload2MethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidVirtualMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidVirtualMethodCalls);


            var testObject2 = new MethodsTestInheritedClass();
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestInheritedClassVoidVirtualOverrideMethodInfo);
            Assert.IsNull(methodDelegate(testObject2));
            Assert.AreEqual(0, testObject2.VoidVirtualMethodCalls);
            Assert.AreEqual(1, testObject2.VoidVirtualOverrideMethodCalls);


            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidInternalMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidInternalMethodCalls);


            MethodsTestClass.VoidStaticMethodCalls = 0;
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidStaticMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, MethodsTestClass.VoidStaticMethodCalls);

            Assert.IsNull(methodDelegate(null));
            Assert.AreEqual(2, MethodsTestClass.VoidStaticMethodCalls);
        }

        [Test]
        public void CreateMethod_ValueType()
        {
            int methodCalls = 0;
            var testObject = new MethodsTestStruct();
            testObject.MethodCalled += (sender, args) => ++methodCalls;

            MethodDelegate methodDelegate = MethodExtensions.CreateMethodDelegate(TestStructVoidMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, methodCalls);
        }

        [Test]
        public void CreateMethod_Template()
        {
            var testObject = new MethodsTestTemplateClass<int>();

            MethodDelegate methodDelegate = MethodExtensions.CreateMethodDelegate(TestTemplateClassVoidMethodInfo);
            Assert.IsNull(methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidMethodCalls);
        }

        [Test]
        public void CreateMethod_Static()
        {
            MethodsTestStaticClass.VoidMethodCalls = 0;
            MethodDelegate methodDelegate = MethodExtensions.CreateMethodDelegate(TestStaticClassVoidStaticMethodInfo);
            Assert.IsNull(methodDelegate(null));
            Assert.AreEqual(1, MethodsTestStaticClass.VoidMethodCalls);

            Assert.IsNull(methodDelegate(new TestStruct()));
            Assert.AreEqual(2, MethodsTestStaticClass.VoidMethodCalls);
        }

        [Test]
        public void CreateMethod_Throws()
        {
            // Arguments not match signature
            MethodDelegate methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntMethodInfo);
            Assert.Throws<ArgumentException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12.5f }));

            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntParamsMethodInfo);
            Assert.Throws<ArgumentException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12, 24 }));
            Assert.Throws<ArgumentException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12, new object[] { 24.5f } }));
            Assert.Throws<ArgumentException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12, new object[] { 24.5 } }));

            // Open template
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestOpenTemplateClassVoidMethodInfo);
            Assert.Throws<InvalidOperationException>(() => methodDelegate(new MethodsTestTemplateClass<int>()));
            
            // Wrong target type
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidMethodInfo);
            Assert.Throws<TargetException>(() => methodDelegate(new MethodsTestTemplateClass<int>()));

            // Method throws
            var testObject = new MethodsTestClass();
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidThrowMethodInfo);
            Assert.Throws<TargetInvocationException>(() => methodDelegate(testObject));
            Assert.AreEqual(1, testObject.VoidThrowMethodCalls);

            // Wrong argument number
            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidMethodInfo);
            Assert.Throws<TargetParameterCountException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12 }));

            methodDelegate = MethodExtensions.CreateMethodDelegate(TestClassVoidIntParamsMethodInfo);
            Assert.Throws<TargetParameterCountException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12.5f }));
            Assert.Throws<TargetParameterCountException>(() => methodDelegate(new MethodsTestClass(), new object[] { 12.5f, new[] { 12.5f }, 48.5f }));
        }
    }
}