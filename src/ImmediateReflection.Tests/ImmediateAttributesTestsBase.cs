using System;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to attributes.
    /// </summary>
    [TestFixture]
    internal class ImmediateAttributesTestsBase : ImmediateReflectionTestsBase
    {
        #region Test classes

        #region Attributes

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        protected sealed class TestClassAttribute : Attribute, IEquatable<TestClassAttribute>
        {
            private readonly int _id;

            public TestClassAttribute(int id)
            {
                _id = id;
            }

            /// <inheritdoc />
            public bool Equals(TestClassAttribute other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return _id == other._id;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return Equals(obj as TestClassAttribute);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return (base.GetHashCode() * 397) ^ _id;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        protected sealed class SecondTestClassAttribute : Attribute, IEquatable<SecondTestClassAttribute>
        {
            private readonly int _id;

            public SecondTestClassAttribute(int id)
            {
                _id = id;
            }

            /// <inheritdoc />
            public bool Equals(SecondTestClassAttribute other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return _id == other._id;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return Equals(obj as SecondTestClassAttribute);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return (base.GetHashCode() * 397) ^ _id;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        protected sealed class ThirdTestClassAttribute : Attribute, IEquatable<ThirdTestClassAttribute>
        {
            private readonly int _id;

            public ThirdTestClassAttribute(int id)
            {
                _id = id;
            }

            /// <inheritdoc />
            public bool Equals(ThirdTestClassAttribute other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return _id == other._id;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return Equals(obj as ThirdTestClassAttribute);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return (base.GetHashCode() * 397) ^ _id;
                }
            }
        }

        protected sealed class FakeTestClassAttribute
        {
        }

        [AttributeUsage(AttributeTargets.All)]
        protected class TestBaseAttribute : Attribute
        {
            private readonly int _id;

            public TestBaseAttribute(int id)
            {
                _id = id;
            }

            protected bool Equals(TestBaseAttribute other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return _id == other._id;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return Equals(obj as TestBaseAttribute);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return (base.GetHashCode() * 397) ^ _id;
                }
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        protected sealed class TestInheritingAttribute : TestBaseAttribute
        {
            public TestInheritingAttribute(int id)
                : base(id)
            {
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                return Equals(obj as TestInheritingAttribute);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return base.GetHashCode() * 397;
            }
        }

        #endregion

        #region Classes

        // ReSharper disable InconsistentNaming

        protected class TestClassNoAttribute
        {
            public int _testField = 42;

            public virtual int TestProperty { get; set; } = 12;
        }

        [TestClass(1)]
        protected class TestClassWithAttribute
        {
            [TestClass(2)]
            public int _testField = 42;

            [TestClass(3)]
            public virtual int TestProperty { get; set; } = 12;
        }

        [TestClass(4)]
        [TestClass(5)]
        protected class TestClassWithAttributes
        {
            [TestClass(7)]
            [TestClass(8)]
            public int _testField = 42;

            [TestClass(9)]
            [TestClass(10)]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int TestProperty { get; } = 12;
        }

        protected class InheritedTestClassNoAttribute : TestClassNoAttribute
        {
            public override int TestProperty { get; set; } = 45;
        }

        protected class InheritedTestClassWithAttribute1 : TestClassWithAttribute
        {
            public override int TestProperty { get; set; } = 45;
        }

        [TestClass(11)]
        protected class InheritedTestClassWithAttribute2 : TestClassWithAttribute
        {
            [TestClass(12)]
            public override int TestProperty { get; set; } = 45;
        }

        [TestClass(13)]
        [SecondTestClass(1)]
        protected class TestClassMultiAttributes
        {
            [TestClass(14)]
            [SecondTestClass(2)]
            public int _testField = 42;

            [TestClass(15)]
            [SecondTestClass(3)]
            public virtual int TestProperty { get; set; } = 12;
        }

        protected class InheritedTestClassMultiAttributes : TestClassMultiAttributes
        {
            public override int TestProperty { get; set; } = 45;
        }

        [ThirdTestClass(16)]
        [TestInheriting(17)]
        protected class TestClassOnlyInheritedAttribute
        {
            [ThirdTestClass(18)]
            [TestInheriting(19)]
            public int _testFieldOnlyInheriting = 12;

            [TestBase(20)]
            [TestInheriting(21)]
            public int _testField = 42;
        }

        [TestBase(22)]
        [TestInheriting(23)]
        protected class TestClassInheritedAttribute
        {
            [ThirdTestClass(24)]
            [TestInheriting(25)]
            public int TestPropertyOnlyInheriting { get; set; }

            [TestBase(26)]
            [TestInheriting(27)]
            public int TestProperty { get; set; }
        }

        // ReSharper restore InconsistentNaming

        #endregion

        #region Fields & Property Info

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo TestPropertyNoAttributePropertyInfo =
            typeof(TestClassNoAttribute).GetProperty(nameof(TestClassNoAttribute.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyAttributePropertyInfo =
            typeof(TestClassWithAttribute).GetProperty(nameof(TestClassWithAttribute.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyAttributesPropertyInfo =
            typeof(TestClassWithAttributes).GetProperty(nameof(TestClassWithAttributes.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyInheritedNoAttributePropertyInfo =
            typeof(InheritedTestClassNoAttribute).GetProperty(nameof(InheritedTestClassNoAttribute.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyInheritedAttribute1PropertyInfo =
            typeof(InheritedTestClassWithAttribute1).GetProperty(nameof(InheritedTestClassWithAttribute1.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyInheritedAttribute2PropertyInfo =
            typeof(InheritedTestClassWithAttribute2).GetProperty(nameof(InheritedTestClassWithAttribute2.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyMultiAttributesPropertyInfo =
            typeof(TestClassMultiAttributes).GetProperty(nameof(TestClassMultiAttributes.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyInheritedMultiAttributesPropertyInfo =
            typeof(InheritedTestClassMultiAttributes).GetProperty(nameof(InheritedTestClassMultiAttributes.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyOnlyInheritingAttributePropertyInfo =
            typeof(TestClassInheritedAttribute).GetProperty(nameof(TestClassInheritedAttribute.TestPropertyOnlyInheriting)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo TestPropertyInheritingAttributePropertyInfo =
            typeof(TestClassInheritedAttribute).GetProperty(nameof(TestClassInheritedAttribute.TestProperty)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo TestFieldNoAttributeFieldInfo =
            typeof(TestClassNoAttribute).GetField(nameof(TestClassNoAttribute._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo TestFieldAttributeFieldInfo =
            typeof(TestClassWithAttribute).GetField(nameof(TestClassWithAttribute._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo TestFieldAttributesFieldInfo =
            typeof(TestClassWithAttributes).GetField(nameof(TestClassWithAttributes._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo TestFieldMultiAttributesFieldInfo =
            typeof(TestClassMultiAttributes).GetField(nameof(TestClassMultiAttributes._testField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo TestFieldOnlyInheritingAttributeFieldInfo =
            typeof(TestClassOnlyInheritedAttribute).GetField(nameof(TestClassOnlyInheritedAttribute._testFieldOnlyInheriting)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo TestFieldInheritingAttributeFieldInfo =
            typeof(TestClassOnlyInheritedAttribute).GetField(nameof(TestClassOnlyInheritedAttribute._testField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion
    }
}