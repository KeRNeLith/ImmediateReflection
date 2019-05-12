using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateType"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateTypeTests : ImmediateReflectionTestsBase
    {
        #region Test classes

        private class EmptyType
        {
        }

        private class PrivateNestedClass
        {
#pragma warning disable 649
            // ReSharper disable once InconsistentNaming
            public int _nestedTestValue;
#pragma warning restore 649

            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int NestedTestValue { get; set; } = 25;
        }

        #region Test helpers

        // Fields //

        [NotNull]
        protected static FieldInfo PrivateNestedPublicFieldFieldInfo =
            typeof(PrivateNestedClass).GetField(nameof(PrivateNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        // Properties //

        [NotNull]
        protected static PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
            typeof(PrivateNestedClass).GetProperty(nameof(PrivateNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        #endregion

        #endregion

        [Test]
        public void ImmediateTypeEmptyType()
        {
            var emptyType = new ImmediateType(typeof(EmptyType));
            Assert.AreEqual(typeof(EmptyType), emptyType.Type);
            Assert.AreEqual(nameof(EmptyType), emptyType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(EmptyType)}",
                emptyType.FullName);
            CollectionAssert.AreEqual(
                Enumerable.Empty<FieldInfo>(),
                emptyType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEqual(
                Enumerable.Empty<PropertyInfo>(),
                emptyType.Properties.Select(property => property.PropertyInfo));
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
                    PublicValueTypePublicFieldFieldsInfo,
                    PublicValueTypePublicField2FieldsInfo
                },
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicValueTypePublicGetSetPropertyPropertyInfo,
                    PublicValueTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicValueTypePublicGetPropertyPropertyInfo,
                    PublicValueTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicValueTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicValueTypePublicSetPropertyPropertyInfo
                },
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

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
                    InternalValueTypePublicFieldFieldsInfo,
                    InternalValueTypePublicField2FieldsInfo
                },
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalValueTypePublicGetSetPropertyPropertyInfo,
                    InternalValueTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalValueTypePublicGetPropertyPropertyInfo,
                    InternalValueTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalValueTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalValueTypePublicSetPropertyPropertyInfo
                },
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
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
                    PublicReferenceTypePublicFieldFieldsInfo,
                    PublicReferenceTypePublicField2FieldsInfo
                },
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicReferenceTypePublicGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicGetPropertyPropertyInfo,
                    PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicReferenceTypePublicSetPropertyPropertyInfo
                },
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

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
                    InternalReferenceTypePublicFieldFieldsInfo,
                    InternalReferenceTypePublicField2FieldsInfo
                },
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalReferenceTypePublicGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicGetPropertyPropertyInfo,
                    InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalReferenceTypePublicSetPropertyPropertyInfo
                },
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeObjectReferenceType()
        {
            // Public class
            var immediateTypePublic = new ImmediateType(typeof(PublicObjectTypeTestClass));
            Assert.AreEqual(typeof(PublicObjectTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicObjectTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicObjectTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicObjectTypePublicFieldFieldsInfo,
                    PublicObjectTypePublicField2FieldsInfo
                },
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicObjectTypePublicGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicGetPropertyPropertyInfo,
                    PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicObjectTypePublicSetPropertyPropertyInfo
                },
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            var immediateTypeInternal = new ImmediateType(typeof(InternalObjectTypeTestClass));
            Assert.AreEqual(typeof(InternalObjectTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalObjectTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalObjectTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalObjectTypePublicFieldFieldsInfo,
                    InternalObjectTypePublicField2FieldsInfo
                },
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalObjectTypePublicGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicGetPropertyPropertyInfo,
                    InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalObjectTypePublicSetPropertyPropertyInfo
                },
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
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
                new[]
                {
                    PublicNestedPublicFieldFieldInfo
                },
                nestedImmediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            var nestedImmediateTypeInternal = new ImmediateType(typeof(PublicTestClass.InternalNestedClass));
            Assert.AreEqual(typeof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Type);
            Assert.AreEqual(nameof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicTestClass)}+{nameof(PublicTestClass.InternalNestedClass)}",
                nestedImmediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalNestedPublicFieldFieldInfo
                },
                nestedImmediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypeInternal.Properties.Select(property => property.PropertyInfo));

            // Protected class
            var nestedImmediateTypeProtected = new ImmediateType(typeof(ProtectedNestedClass));
            Assert.AreEqual(typeof(ProtectedNestedClass), nestedImmediateTypeProtected.Type);
            Assert.AreEqual(nameof(ProtectedNestedClass), nestedImmediateTypeProtected.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateReflectionTestsBase)}+{nameof(ProtectedNestedClass)}",
                nestedImmediateTypeProtected.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ProtectedNestedPublicFieldFieldInfo
                },
                nestedImmediateTypeProtected.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ProtectedNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypeProtected.Properties.Select(property => property.PropertyInfo));

            // Private class
            var nestedImmediateTypePrivate = new ImmediateType(typeof(PrivateNestedClass));
            Assert.AreEqual(typeof(PrivateNestedClass), nestedImmediateTypePrivate.Type);
            Assert.AreEqual(nameof(PrivateNestedClass), nestedImmediateTypePrivate.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(PrivateNestedClass)}",
                nestedImmediateTypePrivate.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PrivateNestedPublicFieldFieldInfo
                },
                nestedImmediateTypePrivate.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PrivateNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypePrivate.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeWithFlags()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var testType = new ImmediateType(typeof(PublicValueTypeTestClass)); // BindingFlags.Public | BindingFlags.Instance
            CollectionAssert.AreEqual(
                classifiedMembers.PublicInstanceFields,
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties,
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.NonPublic | BindingFlags.Instance);
            CollectionAssert.AreEqual(
                classifiedMembers.NonPublicInstanceFields,
                IgnoreBackingFields(testType.Fields.Select(field => field.FieldInfo)));
            CollectionAssert.AreEquivalent(
                classifiedMembers.NonPublicInstanceProperties,
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            CollectionAssert.AreEqual(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.NonPublicInstanceFields),
                IgnoreBackingFields(testType.Fields.Select(field => field.FieldInfo)));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.NonPublicInstanceProperties),
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            CollectionAssert.AreEqual(
                classifiedMembers.StaticFields,
                IgnoreBackingFields(testType.Fields.Select(field => field.FieldInfo)));
            CollectionAssert.AreEquivalent(
                classifiedMembers.StaticProperties,
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.IgnoreCase);
            CollectionAssert.AreEqual(
                Enumerable.Empty<FieldInfo>(),
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEqual(
                Enumerable.Empty<PropertyInfo>(),
                testType.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeGetFields()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            CollectionAssert.AreEquivalent(immediateType1.Fields, immediateType1.GetFields());

            var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            CollectionAssert.AreNotEquivalent(immediateType1.GetFields(), immediateType2.GetFields());
        }

        [Test]
        public void ImmediateTypeGetField()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            string fieldName = nameof(PublicValueTypeTestClass._publicField);
            Assert.AreEqual(immediateType.Fields[fieldName], immediateType.GetField(fieldName));

            fieldName = "NotExists";
            Assert.IsNull(immediateType.GetField(fieldName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetField(null));
        }

        [Test]
        public void ImmediateTypeGetProperties()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            CollectionAssert.AreEquivalent(immediateType1.Properties, immediateType1.GetProperties());

            var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            CollectionAssert.AreNotEquivalent(immediateType1.GetProperties(), immediateType2.GetProperties());
        }

        [Test]
        public void ImmediateTypeGetProperty()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            string propertyName = nameof(PublicValueTypeTestClass.PublicPropertyGetSet);
            Assert.AreEqual(immediateType.Properties[propertyName], immediateType.GetProperty(propertyName));

            propertyName = "NotExists";
            Assert.IsNull(immediateType.GetProperty(propertyName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetProperty(null));
        }

        [Test]
        public void ImmediateTypeEquality()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.IsTrue(immediateType1.Equals(immediateType1));
            Assert.IsTrue(immediateType1.Equals(immediateType2));
            Assert.IsTrue(immediateType1.Equals((object)immediateType2));
            Assert.IsFalse(immediateType1.Equals(null));

            var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.IsFalse(immediateType1.Equals(immediateType3));
            Assert.IsFalse(immediateType1.Equals((object)immediateType3));
        }

        [Test]
        public void ImmediateTypeHashCode()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).GetHashCode(), immediateType1.GetHashCode());
            Assert.AreEqual(immediateType1.GetHashCode(), immediateType2.GetHashCode());

            var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.GetHashCode(), immediateType3.GetHashCode());
        }

        [Test]
        public void ImmediateTypeToString()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).ToString(), immediateType1.ToString());

            var immediateType2 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.ToString(), immediateType2.ToString());
        }
    }
}