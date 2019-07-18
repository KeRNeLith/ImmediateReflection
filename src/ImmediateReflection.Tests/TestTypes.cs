// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local

using System;

#pragma warning disable CS0649
#pragma warning disable 169

namespace ImmediateReflection.Tests
{
    public enum TestEnum
    {
        EnumValue1,
        EnumValue2
    }

    public enum TestEnumULong : ulong
    {
        EnumValue1 = 10,
        EnumValue2 = 20
    }

    [Flags]
    public enum TestEnumFlags
    {
        EnumValue1,
        EnumValue2,
        EnumValue3
    }

    public struct TestStruct
    {
        public int _testValue;

        public int TestValue { get; set; }

        public static int TestStaticValue { get; set; }
    }

    public class TestObject
    {
        public int TestValue { get; set; }
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
        public static readonly int _publicStaticReadonlyField = 112;
        public const int _publicConstField = 221;

        public PublicValueTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public PublicValueTypeTestClass(int internalField = 0, int protectedField = 0, int privateField = 0)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public PublicValueTypeTestClass(int publicGetProperty = 0, int publicGetPrivateSetProperty = 0)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public int PublicPropertyGetSet { get; set; }
        public virtual int PublicVirtualPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected int ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private int PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _publicField = value; }

        public static int PublicStaticPropertyGetSet { get; set; }

        // Test getters
        public int GetProtectedFieldValue() => _protectedField;
        public int GetPrivateFieldValue() => _privateField;
    }

    public abstract class AbstractPublicValueTypeTestClass : PublicValueTypeTestClass
    {
        public abstract int PublicAbstractGetSetProperty { get; set; }
    }

    public class ConcretePublicValueTypeTestClass : AbstractPublicValueTypeTestClass
    {
        public override int PublicAbstractGetSetProperty { get; set; }
    }

    public class PublicReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public TestObject _publicField2;
        public static TestObject _publicStaticField;
        public static readonly TestObject _publicStaticReadonlyField = new TestObject { TestValue = 112 };

        public PublicReferenceTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public PublicReferenceTypeTestClass(TestObject internalField = null, TestObject protectedField = null, TestObject privateField = null)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public PublicReferenceTypeTestClass(TestObject publicGetProperty = null, TestObject publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public TestObject PublicPropertyGetSet { get; set; }
        public virtual TestObject PublicVirtualPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected TestObject ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private TestObject PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _publicField = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }

        // Test getters
        public TestObject GetProtectedFieldValue() => _protectedField;
        public TestObject GetPrivateFieldValue() => _privateField;
    }

    public class PublicObjectTypeTestClass
    {
        private object _privateField;
        protected object _protectedField;
        internal object _internalField;
        public object _publicField;
        public object _publicField2;
        public static object _publicStaticField;
        public static readonly object _publicStaticReadonlyField = 112;
        public const object _publicConstField = null;

        public PublicObjectTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public PublicObjectTypeTestClass(object internalField = null, object protectedField = null, object privateField = null)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public PublicObjectTypeTestClass(object publicGetProperty = null, object publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public object PublicPropertyGetSet { get; set; }
        public virtual object PublicVirtualPropertyGetSet { get; set; }
        internal object InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected object ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private object PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public object PublicPropertyGet { get; }
        public object PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public object PublicPropertyGetPrivateSet { get; private set; }
        public object PublicPropertySet { set => _publicField = value; }

        public static object PublicStaticPropertyGetSet { get; set; }

        // Test getters
        public object GetProtectedFieldValue() => _protectedField;
        public object GetPrivateFieldValue() => _privateField;
    }

    internal class InternalValueTypeTestClass
    {
        private int _privateField;
        protected int _protectedField;
        internal int _internalField;
        public int _publicField;
        public int _publicField2;
        public static int _publicStaticField;
        public static readonly int _publicStaticReadonlyField = 112;
        public const int _publicConstField = 221;

        public InternalValueTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public InternalValueTypeTestClass(int internalField = 0, int protectedField = 0, int privateField = 0)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public InternalValueTypeTestClass(int publicGetProperty = 0, int publicGetPrivateSetProperty = 0)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public int PublicPropertyGetSet { get; set; }
        public virtual int PublicVirtualPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected int ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private int PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _publicField = value; }

        public static int PublicStaticPropertyGetSet { get; set; }


        // Test getters
        public int GetProtectedFieldValue() => _protectedField;
        public int GetPrivateFieldValue() => _privateField;
    }

    internal class InternalReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public TestObject _publicField2;
        public static TestObject _publicStaticField;
        public static readonly TestObject _publicStaticReadonlyField = new TestObject { TestValue = 221 };

        public InternalReferenceTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public InternalReferenceTypeTestClass(TestObject internalField = null, TestObject protectedField = null, TestObject privateField = null)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public InternalReferenceTypeTestClass(TestObject publicGetProperty = null, TestObject publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public TestObject PublicPropertyGetSet { get; set; }
        public virtual TestObject PublicVirtualPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected TestObject ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private TestObject PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _publicField = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }


        // Test getters
        public TestObject GetProtectedFieldValue() => _protectedField;
        public TestObject GetPrivateFieldValue() => _privateField;
    }

    internal class InternalObjectTypeTestClass
    {
        private object _privateField;
        protected object _protectedField;
        internal object _internalField;
        public object _publicField;
        public object _publicField2;
        public static object _publicStaticField;
        public static readonly object _publicStaticReadonlyField = new TestObject();
        public const object _publicConstField = null;

        public InternalObjectTypeTestClass()
        {
        }

        // Constructor for non publicly initializable fields
        public InternalObjectTypeTestClass(object internalField = null, object protectedField = null, object privateField = null)
        {
            _internalField = internalField;
            _protectedField = protectedField;
            _privateField = privateField;
        }

        // Constructor for non publicly initializable properties
        public InternalObjectTypeTestClass(object publicGetProperty = null, object publicGetPrivateSetProperty = null)
        {
            PublicPropertyGet = publicGetProperty;
            PublicPropertyGetPrivateSet = publicGetPrivateSetProperty;
        }

        public object PublicPropertyGetSet { get; set; }
        public virtual object PublicVirtualPropertyGetSet { get; set; }
        internal object InternalPropertyGetSet { get => _publicField; set => _publicField = value; }
        protected object ProtectedPropertyGetSet { get => _publicField; set => _publicField = value; }
        private object PrivatePropertyGetSet { get => _publicField; set => _publicField = value; }

        public object PublicPropertyGet { get; }
        public object PublicPropertyPrivateGetSet { private get => _publicField; set => _publicField = value; }
        public object PublicPropertyGetPrivateSet { get; private set; }
        public object PublicPropertySet { set => _publicField = value; }

        public static object PublicStaticPropertyGetSet { get; set; }


        // Test getters
        public object GetProtectedFieldValue() => _protectedField;
        public object GetPrivateFieldValue() => _privateField;
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

    public class BaseTestClass
    {
        public string Property { get; } = "Parent";
    }

    public class ChildTestClass : BaseTestClass
    {
        public new string Property { get; } = "Child";
    }
}

#pragma warning restore 169
#pragma warning restore CS0649