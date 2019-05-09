using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateType"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateTypeTests : ImmediateReflectionTestsBase
    {
        protected class ProtectedNestedClass
        {
            public int NestedTestValue { get; set; } = 25;
        }

        private class PrivateNestedClass
        {
            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int NestedTestValue { get; set; } = 25;
        }

        [Test]
        public void ImmediateTypeValueType()
        {
            // Public class
            var immediateTypePublic = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicValueTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicValueTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicValueTypeTestClass._publicField),
                    nameof(PublicValueTypeTestClass._publicField2)
                },
                immediateTypePublic.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicValueTypeTestClass.PublicPropertyGetSet),
                    nameof(PublicValueTypeTestClass.PublicPropertyGet),
                    nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet),
                    nameof(PublicValueTypeTestClass.PublicPropertySet)
                },
                immediateTypePublic.Properties.Select(property => property.Name));

            // Internal class
            var immediateTypeInternal = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreEqual(typeof(InternalValueTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalValueTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalValueTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalValueTypeTestClass._publicField),
                    nameof(InternalValueTypeTestClass._publicField2)
                },
                immediateTypeInternal.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalValueTypeTestClass.PublicPropertyGetSet),
                    nameof(InternalValueTypeTestClass.PublicPropertyGet),
                    nameof(InternalValueTypeTestClass.PublicPropertyGetPrivateSet),
                    nameof(InternalValueTypeTestClass.PublicPropertySet)
                },
                immediateTypeInternal.Properties.Select(property => property.Name));
        }

        [Test]
        public void ImmediateTypeReferenceType()
        {
            // Public class
            var immediateTypePublic = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            Assert.AreEqual(typeof(PublicReferenceTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicReferenceTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicReferenceTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicReferenceTypeTestClass._publicField),
                    nameof(PublicReferenceTypeTestClass._publicField2)
                },
                immediateTypePublic.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet),
                    nameof(PublicReferenceTypeTestClass.PublicPropertyGet),
                    nameof(PublicReferenceTypeTestClass.PublicPropertyGetPrivateSet),
                    nameof(PublicReferenceTypeTestClass.PublicPropertySet)
                },
                immediateTypePublic.Properties.Select(property => property.Name));

            // Internal class
            var immediateTypeInternal = new ImmediateType(typeof(InternalReferenceTypeTestClass));
            Assert.AreEqual(typeof(InternalReferenceTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalReferenceTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalReferenceTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalReferenceTypeTestClass._publicField),
                    nameof(InternalReferenceTypeTestClass._publicField2)
                },
                immediateTypeInternal.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalReferenceTypeTestClass.PublicPropertyGetSet),
                    nameof(InternalReferenceTypeTestClass.PublicPropertyGet),
                    nameof(InternalReferenceTypeTestClass.PublicPropertyGetPrivateSet),
                    nameof(InternalReferenceTypeTestClass.PublicPropertySet)
                },
                immediateTypeInternal.Properties.Select(property => property.Name));
        }

        [Test]
        public void ImmediateTypeNestedType()
        {
            // Public class
            var nestedImmediateTypePublic = new ImmediateType(typeof(PublicTestClass.PublicNestedClass));
            Assert.AreEqual(typeof(PublicTestClass.PublicNestedClass), nestedImmediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicTestClass.PublicNestedClass), nestedImmediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicTestClass)}+{nameof(PublicTestClass.PublicNestedClass)}",
                nestedImmediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<string>(),
                nestedImmediateTypePublic.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicTestClass.PublicNestedClass.NestedTestValue)
                },
                nestedImmediateTypePublic.Properties.Select(property => property.Name));

            // Internal class
            var nestedImmediateTypeInternal = new ImmediateType(typeof(PublicTestClass.InternalNestedClass));
            Assert.AreEqual(typeof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Type);
            Assert.AreEqual(nameof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicTestClass)}+{nameof(PublicTestClass.InternalNestedClass)}",
                nestedImmediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<string>(),
                nestedImmediateTypeInternal.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicTestClass.InternalNestedClass.NestedTestValue)
                },
                nestedImmediateTypeInternal.Properties.Select(property => property.Name));

            // Protected class
            var nestedImmediateTypeProtected = new ImmediateType(typeof(ProtectedNestedClass));
            Assert.AreEqual(typeof(ProtectedNestedClass), nestedImmediateTypeProtected.Type);
            Assert.AreEqual(nameof(ProtectedNestedClass), nestedImmediateTypeProtected.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(ProtectedNestedClass)}",
                nestedImmediateTypeProtected.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<string>(),
                nestedImmediateTypeProtected.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(ProtectedNestedClass.NestedTestValue)
                },
                nestedImmediateTypeProtected.Properties.Select(property => property.Name));

            // Private class
            var nestedImmediateTypePrivate = new ImmediateType(typeof(PrivateNestedClass));
            Assert.AreEqual(typeof(PrivateNestedClass), nestedImmediateTypePrivate.Type);
            Assert.AreEqual(nameof(PrivateNestedClass), nestedImmediateTypePrivate.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(PrivateNestedClass)}",
                nestedImmediateTypePrivate.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<string>(),
                nestedImmediateTypePrivate.Fields.Select(field => field.Name));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PrivateNestedClass.NestedTestValue)
                },
                nestedImmediateTypePrivate.Properties.Select(property => property.Name));
        }

        [Test]
        public void ImmediateFieldEquality()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(immediateType1, immediateType2);
            Assert.IsTrue(immediateType1.Equals((object)immediateType2));
            Assert.IsFalse(immediateType1.Equals(null));

            var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1, immediateType3);
            Assert.IsFalse(immediateType1.Equals((object)immediateType3));
        }

        [Test]
        public void ImmediateFieldHashCode()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).GetHashCode(), immediateType1.GetHashCode());

            var immediateType2 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.GetHashCode(), immediateType2.GetHashCode());
        }

        [Test]
        public void ImmediateFieldToString()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).ToString(), immediateType1.ToString());

            var immediateType2 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.ToString(), immediateType2.ToString());
        }
    }
}