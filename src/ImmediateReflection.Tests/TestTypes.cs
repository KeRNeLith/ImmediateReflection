// ReSharper disable UnusedMember.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

#pragma warning disable CS0649

namespace ImmediateReflection.Tests
{
    public class TestObject
    {
        private int TestValue { get; set; } = 42;
    }

    public class PublicValueTypeTestClass
    {
        private int _privateField;
        protected int _protectedField;
        internal int _internalField;
        public int _publicField;
        public static int _publicStaticField;

        public int PublicPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get; set; }
        protected int ProtectedPropertyGetSet { get; set; }
        private int PrivatePropertyGetSet { get; set; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _privateField = value; }

        public static int PublicStaticPropertyGetSet { get; set; }
    }

    public class PublicReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public static TestObject _publicStaticField;

        public TestObject PublicPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get; set; }
        protected TestObject ProtectedPropertyGetSet { get; set; }
        private TestObject PrivatePropertyGetSet { get; set; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _privateField = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }
    }

    internal class InternalValueTypeTestClass
    {
        private int _privateField;
        protected int _protectedField;
        internal int _internalField;
        public int _publicField;
        public static int _publicStaticField;

        public int PublicPropertyGetSet { get; set; }
        internal int InternalPropertyGetSet { get; set; }
        protected int ProtectedPropertyGetSet { get; set; }
        private int PrivatePropertyGetSet { get; set; }

        public int PublicPropertyGet { get; }
        public int PublicPropertyGetPrivateSet { get; private set; }
        public int PublicPropertySet { set => _privateField = value; }

        public static int PublicStaticPropertyGetSet { get; set; }
    }

    internal class InternalReferenceTypeTestClass
    {
        private TestObject _privateField;
        protected TestObject _protectedField;
        internal TestObject _internalField;
        public TestObject _publicField;
        public static TestObject _publicStaticField;

        public TestObject PublicPropertyGetSet { get; set; }
        internal TestObject InternalPropertyGetSet { get; set; }
        protected TestObject ProtectedPropertyGetSet { get; set; }
        private TestObject PrivatePropertyGetSet { get; set; }

        public TestObject PublicPropertyGet { get; }
        public TestObject PublicPropertyGetPrivateSet { get; private set; }
        public TestObject PublicPropertySet { set => _privateField = value; }

        public static TestObject PublicStaticPropertyGetSet { get; set; }
    }

    public class PublicTestClass
    {
        public class PublicNestedClass
        {
            public int NestedTestValue { get; set; } = 12;
        }

        internal class InternalNestedClass
        {
            public int NestedTestValue { get; set; } = 12;
        }

        protected class ProtectedNestedClass
        {
            public int NestedTestValue { get; set; } = 12;
        }

        private class PrivateNestedClass
        {
            public int NestedTestValue { get; set; } = 12;
        }
    }
}

#pragma warning restore CS0649