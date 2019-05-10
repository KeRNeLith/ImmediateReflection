// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

using System;

#pragma warning disable CS0649
#pragma warning disable 169

namespace ImmediateReflection.Tests
{
    public class TestObject
    {
        public int TestValue { get; set; } = 42;
    }

    public class SmallObject
    {
        public int _testField1 = 12;
        public TestObject _testField2 = new TestObject { TestValue = 12 };

        public int TestProperty1 { get; set; } = 21;
        public TestObject TestProperty2 { get; set; } = new TestObject();
    }

    public class SecondSmallObject
    {
        public int _testField1 = 12;
        public TestObject _testField2 = new TestObject { TestValue = 12 };

        public int TestProperty1 { get; set; } = 21;
        public TestObject TestProperty2 { get; set; } = new TestObject();
    }

    public class PublicValueTypeTestClass
    {
        private int _privateField;
        protected int _protectedField;
        internal int _internalField;
        public int _publicField;
        public int _publicField2;
        public static int _publicStaticField;

        public PublicValueTypeTestClass()
        {
        }

        // Constructor for non publicly initializable members
        public PublicValueTypeTestClass(int publicGetProperty = 0, int publicGetPrivateSetProperty = 0)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public int PublicPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get; set; }
        protected int ProtectedPropertyGetSet { get; set; }
        private int PrivatePropertyGetSet { get; set; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _publicField2 = value; }

        public static int PublicStaticPropertyGetSet { get; set; }
    }

    public class PublicReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public TestObject _publicField2;
        public static TestObject _publicStaticField;

        public PublicReferenceTypeTestClass()
        {
        }

        // Constructor for non publicly initializable members
        public PublicReferenceTypeTestClass(TestObject publicGetProperty = null, TestObject publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public TestObject PublicPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get; set; }
        protected TestObject ProtectedPropertyGetSet { get; set; }
        private TestObject PrivatePropertyGetSet { get; set; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _publicField2 = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }
    }

    internal class InternalValueTypeTestClass
    {
        private int _privateField;
        protected int _protectedField;
        internal int _internalField;
        public int _publicField;
        public int _publicField2;
        public static int _publicStaticField;

        public InternalValueTypeTestClass()
        {
        }

        // Constructor for non publicly initializable members
        public InternalValueTypeTestClass(int publicGetProperty = 0, int publicGetPrivateSetProperty = 0)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public int PublicPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get; set; }
        protected int ProtectedPropertyGetSet { get; set; }
        private int PrivatePropertyGetSet { get; set; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _publicField2 = value; }

        public static int PublicStaticPropertyGetSet { get; set; }
    }

    internal class InternalReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public TestObject _publicField2;
        public static TestObject _publicStaticField;

        public InternalReferenceTypeTestClass()
        {
        }

        // Constructor for non publicly initializable members
        public InternalReferenceTypeTestClass(TestObject publicGetProperty = null, TestObject publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public TestObject PublicPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get; set; }
        protected TestObject ProtectedPropertyGetSet { get; set; }
        private TestObject PrivatePropertyGetSet { get; set; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _publicField2 = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }
    }

    public class PublicTestClass
    {
        public class PublicNestedClass
        {
            public int _nestedTestValue;

            public int NestedTestValue { get; set; } = 12;
        }

        internal class InternalNestedClass
        {
            public int _nestedTestValue;

            public int NestedTestValue { get; set; } = 12;
        }

        protected class ProtectedNestedClass
        {
            public int _nestedTestValue;

            public int NestedTestValue { get; set; } = 12;
        }

        private class PrivateNestedClass
        {
            public int _nestedTestValue;

            public int NestedTestValue { get; set; } = 12;
        }
    }
}

#pragma warning restore 169
#pragma warning restore CS0649