using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    [TestFixture]
    internal class ImmediateTypeTests
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
            var immediateTypePublic = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicValueTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicValueTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicValueTypeTestClass._publicField)
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


            var immediateTypeInternal = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreEqual(typeof(InternalValueTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalValueTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalValueTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalValueTypeTestClass._publicField)
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
            var immediateTypePublic = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            Assert.AreEqual(typeof(PublicReferenceTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicReferenceTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicReferenceTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(PublicReferenceTypeTestClass._publicField)
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


            var immediateTypeInternal = new ImmediateType(typeof(InternalReferenceTypeTestClass));
            Assert.AreEqual(typeof(InternalReferenceTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalReferenceTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalReferenceTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    nameof(InternalReferenceTypeTestClass._publicField)
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
    }
}