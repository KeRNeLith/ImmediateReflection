using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using static ImmediateReflection.Tests.ConstructorTestHelpers;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ImmediateType"/>.
/// </summary>
[TestFixture]
internal sealed class ImmediateTypeTests : ImmediateReflectionTestsBase
{
    #region ImmediateType infos

    #region Test classes

    private sealed class EmptyType
    {
    }

    private sealed class PrivateNestedClass
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
    private static readonly FieldInfo PrivateNestedPublicFieldFieldInfo =
        typeof(PrivateNestedClass).GetField(nameof(PrivateNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

    // Properties //
    private static readonly PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
        typeof(PrivateNestedClass).GetProperty(nameof(PrivateNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

    #endregion

    #endregion

    [Test]
    public static void ImmediateTypeEmptyType()
    {
        var emptyType = new ImmediateType(typeof(EmptyType));
        Assert.AreEqual(typeof(EmptyType), emptyType.Type);
        Assert.AreEqual(typeof(object), emptyType.BaseType);
        Assert.AreEqual(typeof(ImmediateTypeTests), emptyType.DeclaringType);
        Assert.AreEqual(nameof(EmptyType), emptyType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(EmptyType)}",
            emptyType.FullName);
        CollectionAssert.IsEmpty(emptyType.Fields);
        CollectionAssert.IsEmpty(emptyType.Properties);
    }

    [Test]
    public static void ImmediateTypeValueType()
    {
        // Public class
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

        var immediateTypePublic = new ImmediateType(typeof(PublicValueTypeTestClass));
        Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateTypePublic.Type);
        Assert.AreEqual(typeof(object), immediateTypePublic.BaseType);
        Assert.IsNull(immediateTypePublic.DeclaringType);
        Assert.AreEqual(nameof(PublicValueTypeTestClass), immediateTypePublic.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicValueTypeTestClass)}",
            immediateTypePublic.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypePublic.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypePublic.Properties.Select(property => property.PropertyInfo));

        // Internal class
        classifiedMembers = TypeClassifiedMembers.GetForInternalValueTypeTestObject();

        var immediateTypeInternal = new ImmediateType(typeof(InternalValueTypeTestClass));
        Assert.AreEqual(typeof(InternalValueTypeTestClass), immediateTypeInternal.Type);
        Assert.AreEqual(typeof(object), immediateTypeInternal.BaseType);
        Assert.IsNull(immediateTypeInternal.DeclaringType);
        Assert.AreEqual(nameof(InternalValueTypeTestClass), immediateTypeInternal.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalValueTypeTestClass)}",
            immediateTypeInternal.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypeInternal.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeReferenceType()
    {
        // Public class
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicReferenceTypeTestObject();

        var immediateTypePublic = new ImmediateType(typeof(PublicReferenceTypeTestClass));
        Assert.AreEqual(typeof(PublicReferenceTypeTestClass), immediateTypePublic.Type);
        Assert.AreEqual(typeof(object), immediateTypePublic.BaseType);
        Assert.IsNull(immediateTypePublic.DeclaringType);
        Assert.AreEqual(nameof(PublicReferenceTypeTestClass), immediateTypePublic.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicReferenceTypeTestClass)}",
            immediateTypePublic.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypePublic.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypePublic.Properties.Select(property => property.PropertyInfo));

        // Internal class
        classifiedMembers = TypeClassifiedMembers.GetForInternalReferenceTypeTestObject();

        var immediateTypeInternal = new ImmediateType(typeof(InternalReferenceTypeTestClass));
        Assert.AreEqual(typeof(InternalReferenceTypeTestClass), immediateTypeInternal.Type);
        Assert.AreEqual(typeof(object), immediateTypeInternal.BaseType);
        Assert.IsNull(immediateTypeInternal.DeclaringType);
        Assert.AreEqual(nameof(InternalReferenceTypeTestClass), immediateTypeInternal.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalReferenceTypeTestClass)}",
            immediateTypeInternal.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypeInternal.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeObjectReferenceType()
    {
        // Public class
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicObjectTypeTestObject();

        var immediateTypePublic = new ImmediateType(typeof(PublicObjectTypeTestClass));
        Assert.AreEqual(typeof(PublicObjectTypeTestClass), immediateTypePublic.Type);
        Assert.AreEqual(typeof(object), immediateTypePublic.BaseType);
        Assert.IsNull(immediateTypePublic.DeclaringType);
        Assert.AreEqual(nameof(PublicObjectTypeTestClass), immediateTypePublic.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicObjectTypeTestClass)}",
            immediateTypePublic.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypePublic.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypePublic.Properties.Select(property => property.PropertyInfo));

        // Internal class
        classifiedMembers = TypeClassifiedMembers.GetForInternalObjectTypeTestObject();

        var immediateTypeInternal = new ImmediateType(typeof(InternalObjectTypeTestClass));
        Assert.AreEqual(typeof(InternalObjectTypeTestClass), immediateTypeInternal.Type);
        Assert.AreEqual(typeof(object), immediateTypeInternal.BaseType);
        Assert.IsNull(immediateTypeInternal.DeclaringType);
        Assert.AreEqual(nameof(InternalObjectTypeTestClass), immediateTypeInternal.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalObjectTypeTestClass)}",
            immediateTypeInternal.FullName);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            immediateTypeInternal.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeNestedType()
    {
        // Public class
        var nestedImmediateTypePublic = new ImmediateType(typeof(PublicTestClass.PublicNestedClass));
        Assert.AreEqual(typeof(PublicTestClass.PublicNestedClass), nestedImmediateTypePublic.Type);
        Assert.AreEqual(typeof(object), nestedImmediateTypePublic.BaseType);
        Assert.AreEqual(typeof(PublicTestClass), nestedImmediateTypePublic.DeclaringType);
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
        Assert.AreEqual(typeof(object), nestedImmediateTypeInternal.BaseType);
        Assert.AreEqual(typeof(PublicTestClass), nestedImmediateTypeInternal.DeclaringType);
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
        Assert.AreEqual(typeof(object), nestedImmediateTypeProtected.BaseType);
        Assert.AreEqual(typeof(ImmediateReflectionTestsBase), nestedImmediateTypeProtected.DeclaringType);
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
        Assert.AreEqual(typeof(object), nestedImmediateTypePrivate.BaseType);
        Assert.AreEqual(typeof(ImmediateTypeTests), nestedImmediateTypePrivate.DeclaringType);
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
    public static void ImmediateTypeInheritedType()
    {
        var immediateType = new ImmediateType(typeof(object));
        Assert.AreEqual(typeof(object), immediateType.Type);
        Assert.IsNull(immediateType.BaseType);
        Assert.IsNull(immediateType.DeclaringType);
        Assert.AreEqual("Object", immediateType.Name);
        Assert.AreEqual(
            $"{nameof(System)}.Object",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.IsEmpty(immediateType.Properties);

        immediateType = new ImmediateType(typeof(ChildTestClass));
        Assert.AreEqual(typeof(ChildTestClass), immediateType.Type);
        Assert.AreEqual(typeof(BaseTestClass), immediateType.BaseType);
        Assert.IsNull(immediateType.DeclaringType);
        Assert.AreEqual(nameof(ChildTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ChildClassPublicGetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeEnumType()
    {
        // Simple test enum
        CheckEnumType(
            typeof(TestEnum),
            [
                TestEnumFieldValueFieldInfo,
                TestEnumField1FieldInfo,
                TestEnumField2FieldInfo
            ]);

        // Test enum (inherit ulong)
        CheckEnumType(
            typeof(TestEnumULong),
            [
                TestEnumULongFieldValueFieldInfo,
                TestEnumULongField1FieldInfo,
                TestEnumULongField2FieldInfo
            ]);

        // Flags test enum
        CheckEnumType(
            typeof(TestEnumFlags),
            [
                TestEnumFlagsFieldValueFieldInfo,
                TestEnumFlagsField1FieldInfo,
                TestEnumFlagsField2FieldInfo,
                TestEnumFlagsField3FieldInfo
            ]);

        #region Local function

        static void CheckEnumType(Type enumType, IEnumerable<FieldInfo> enumFields)
        {
            var immediateType = new ImmediateType(enumType);
            Assert.AreEqual(enumType, immediateType.Type);
            Assert.AreEqual(typeof(Enum), immediateType.BaseType);
            Assert.IsNull(immediateType.DeclaringType);
            Assert.AreEqual(enumType.Name, immediateType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{enumType.Name}",
                immediateType.FullName);

            CollectionAssert.AreEquivalent(
                enumFields,
                immediateType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.IsEmpty(immediateType.Properties);
        }

        #endregion
    }

    [Test]
    public static void ImmediateTypeInterface()
    {
        // Base interface
        var immediateType = new ImmediateType(typeof(IBaseTestInterface));
        Assert.AreEqual(typeof(IBaseTestInterface), immediateType.Type);
        Assert.IsNull(immediateType.BaseType);
        Assert.IsNull(immediateType.DeclaringType);
        Assert.AreEqual(nameof(IBaseTestInterface), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(IBaseTestInterface)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                BaseInterfaceGetPropertyPropertyInfo,
                BaseInterfaceSetPropertyPropertyInfo,
                BaseInterfaceGetSetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));

        // Child interface
        immediateType = new ImmediateType(typeof(IChildTestInterface));
        Assert.AreEqual(typeof(IChildTestInterface), immediateType.Type);
        Assert.IsNull(immediateType.BaseType);
        Assert.IsNull(immediateType.DeclaringType);
        Assert.AreEqual(nameof(IChildTestInterface), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(IChildTestInterface)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ChildInterfaceGetSetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));

        // Implementation
        immediateType = new ImmediateType(typeof(ImplementationInterfacesTestClass));
        Assert.AreEqual(typeof(ImplementationInterfacesTestClass), immediateType.Type);
        Assert.AreEqual(typeof(object), immediateType.BaseType);
        Assert.IsNull(immediateType.DeclaringType);
        Assert.AreEqual(nameof(ImplementationInterfacesTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImplementationInterfacesTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ImplementationBaseInterfaceFromGetPropertyPropertyInfo,
                ImplementationBaseInterfaceFromSetPropertyPropertyInfo,
                ImplementationBaseInterfaceFromGetSetPropertyPropertyInfo,
                ImplementationChildInterfaceFromGetSetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeAnonymousType()
    {
        var testObject = new
        {
            TestIntProperty = 1,
            TestReferenceProperty = new TestObject(),
            TestObjectProperty = new object()
        };
        Type anonymousType = testObject.GetType();

        var immediateAnonymousType = new ImmediateType(anonymousType);
        Assert.IsTrue(IsAnonymousType(immediateAnonymousType.Type));
        Assert.AreEqual(typeof(object), immediateAnonymousType.BaseType);
        Assert.IsNull(immediateAnonymousType.DeclaringType);
        // Name & FullName are not relevant checks
        CollectionAssert.IsEmpty(immediateAnonymousType.Fields);
        CollectionAssert.AreEquivalent(
            anonymousType.GetProperties(),
            immediateAnonymousType.Properties.Select(property => property.PropertyInfo));
    }

    [Test]
    public static void ImmediateTypeWithFlags()
    {
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

        var testType = new ImmediateType(typeof(PublicValueTypeTestClass)); // BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            testType.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            testType.Properties.Select(property => property.PropertyInfo));

        testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.NonPublic | BindingFlags.Instance);
        CollectionAssert.AreEquivalent(
            classifiedMembers.NonPublicInstanceFields,
            testType.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.NonPublicInstanceProperties,
            testType.Properties.Select(property => property.PropertyInfo));

        testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.NonPublicInstanceFields),
            testType.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.NonPublicInstanceProperties),
            testType.Properties.Select(property => property.PropertyInfo));

        testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        CollectionAssert.AreEquivalent(
            classifiedMembers.StaticFields.Concat(classifiedMembers.ConstFields),
            testType.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.StaticProperties,
            testType.Properties.Select(property => property.PropertyInfo));

        testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.IgnoreCase);
        CollectionAssert.IsEmpty(testType.Fields);
        CollectionAssert.IsEmpty(testType.Properties);
    }

    [Test]
    public static void ImmediateTypeNewKeyword()
    {
        var immediateType = new ImmediateType(typeof(BaseTestClass));
        Assert.AreEqual(typeof(BaseTestClass), immediateType.Type);
        Assert.AreEqual(nameof(BaseTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(BaseTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                BaseClassPublicGetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));

        // Case same type redefinition
        immediateType = new ImmediateType(typeof(ChildTestClass));
        Assert.AreEqual(typeof(ChildTestClass), immediateType.Type);
        Assert.AreEqual(nameof(ChildTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ChildClassPublicGetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));

        // Case different type definition
        immediateType = new ImmediateType(typeof(ChildTypeRedefinitionTestClass));
        Assert.AreEqual(typeof(ChildTypeRedefinitionTestClass), immediateType.Type);
        Assert.AreEqual(nameof(ChildTypeRedefinitionTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildTypeRedefinitionTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo
            },
            immediateType.Properties.Select(property => property.PropertyInfo));

    }

    [Test]
    public static void ImmediateTypeIndexedProperties()
    {
        var immediateType = new ImmediateType(typeof(ChildItemTestClass));
        Assert.AreEqual(typeof(ChildItemTestClass), immediateType.Type);
        Assert.AreEqual(nameof(ChildItemTestClass), immediateType.Name);
        Assert.AreEqual(
            $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildItemTestClass)}",
            immediateType.FullName);
        CollectionAssert.IsEmpty(immediateType.Fields);
        CollectionAssert.AreEquivalent(
            new[]
            {
                ChildItemClassPublicGetPropertyPropertyInfo
                // Indexed properties ignored
            },
            immediateType.Properties.Select(property => property.PropertyInfo));
    }

    #endregion

    #region Members

    [Test]
    public static void ImmediateTypeGetMembers()
    {
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

        var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass), TypeAccessor.DefaultFlags | BindingFlags.NonPublic);

        CollectionAssert.AreEquivalent(
            classifiedMembers.AllMembers,
            SelectAllMemberInfos(immediateType.Members));
        CollectionAssert.AreEquivalent(
            classifiedMembers.AllMembers,
            SelectAllMemberInfos(immediateType.GetMembers()));

        #region Local function

        static IEnumerable<MemberInfo> SelectAllMemberInfos(IEnumerable<ImmediateMember> members)
        {
            return members.Select<ImmediateMember, MemberInfo>(member =>
            {
                if (member is ImmediateField field)
                    return field.FieldInfo;
                if (member is ImmediateProperty property)
                    return property.PropertyInfo;

                throw new InvalidOperationException("Members contain an unexpected value");
            });
        }

        #endregion
    }

    [Test]
    public static void ImmediateTypeGetMember()
    {
        var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
        string memberName = nameof(PublicValueTypeTestClass._publicField);
        Assert.AreEqual(immediateType.Fields[memberName], immediateType.GetMember(memberName));
        Assert.AreEqual(immediateType.Fields[memberName], immediateType[memberName]);

        memberName = nameof(PublicValueTypeTestClass.PublicPropertyGet);
        Assert.AreEqual(immediateType.Properties[memberName], immediateType.GetMember(memberName));
        Assert.AreEqual(immediateType.Properties[memberName], immediateType[memberName]);

        memberName = "NotExists";
        Assert.IsNull(immediateType.GetMember(memberName));
        Assert.IsNull(immediateType[memberName]);

        Assert.Throws<ArgumentNullException>(() => _ = immediateType.GetMember(null!));
        Assert.Throws<ArgumentNullException>(() => _ = immediateType[null!]);
    }

    #endregion

    #region Fields

    [Test]
    public static void ImmediateTypeGetFields()
    {
        var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
        CollectionAssert.AreEquivalent(immediateType1.Fields, immediateType1.GetFields());

        var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
        CollectionAssert.AreNotEquivalent(immediateType1.GetFields(), immediateType2.GetFields());
    }

    [Test]
    public static void ImmediateTypeGetField()
    {
        var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
        string fieldName = nameof(PublicValueTypeTestClass._publicField);
        Assert.AreEqual(immediateType.Fields[fieldName], immediateType.GetField(fieldName));

        fieldName = "NotExists";
        Assert.IsNull(immediateType.GetField(fieldName));

        Assert.Throws<ArgumentNullException>(() => _ = immediateType.GetField(null!));
    }

    #endregion

    #region Properties

    [Test]
    public static void ImmediateTypeGetProperties()
    {
        var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
        CollectionAssert.AreEquivalent(immediateType1.Properties, immediateType1.GetProperties());

        var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
        CollectionAssert.AreNotEquivalent(immediateType1.GetProperties(), immediateType2.GetProperties());
    }

    [Test]
    public static void ImmediateTypeGetProperty()
    {
        var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
        string propertyName = nameof(PublicValueTypeTestClass.PublicPropertyGetSet);
        Assert.AreEqual(immediateType.Properties[propertyName], immediateType.GetProperty(propertyName));

        propertyName = "NotExists";
        Assert.IsNull(immediateType.GetProperty(propertyName));

        Assert.Throws<ArgumentNullException>(() => _ = immediateType.GetProperty(null!));
    }

    #endregion

    #region New/Constructor

    #region Has Default Constructor

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateHasDefaultConstructorTestCases))]
    public static bool HasDefaultConstructor(Type type)
    {
        var immediateType = new ImmediateType(type);
        return immediateType.HasDefaultConstructor;
    }

    #endregion

    #region New/TryNew

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorTestCases))]
    public static void NewParameterLess(Type type)
    {
        ConstructorTestHelpers.NewParameterLess(
            type,
            () =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.New();
            });
    }

    [Test]
    public static void NewParamsOnly()
    {
        ConstructorTestHelpers.NewParamsOnly(
            () =>
            {
                var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
                return immediateType.New();
            },
            () => new ParamsOnlyConstructor());

        ConstructorTestHelpers.NewParamsOnly(
            () =>
            {
                var immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
                return immediateType.New();
            },
            () => new IntParamsOnlyConstructor());

        ConstructorTestHelpers.NewParamsOnly(
            () =>
            {
                var immediateType = new ImmediateType(typeof(NullableIntParamsOnlyConstructor));
                return immediateType.New();
            },
            () => new NullableIntParamsOnlyConstructor());
    }

    [Test]
    public static void NewParameterLess_Throws()
    {
        var immediateType = new ImmediateType(typeof(NoDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(NotAccessibleDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(IList<int>));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(IDictionary<int, string>));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(AbstractDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(StaticClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(TemplateStruct<>));
        Assert.Throws<ArgumentException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(TemplateDefaultConstructor<>));
        Assert.Throws<ArgumentException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(ParamsConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(NoDefaultInheritedDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(AmbiguousParamsOnlyConstructor));
        Assert.Throws<AmbiguousMatchException>(() => _ = immediateType.New());

        // ReSharper disable once PossibleMistakenCallToGetType.2
        immediateType = new ImmediateType(typeof(DefaultConstructor).GetType());
        Assert.Throws<ArgumentException>(() => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(DefaultConstructorThrows));
        Assert.Throws(Is.InstanceOf<Exception>(), () => _ = immediateType.New());

        immediateType = new ImmediateType(typeof(int[]));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New());
    }

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorNoThrowTestCases))]
    public static void TryNewParameterLess(Type type, bool expectFail)
    {
        ConstructorTestHelpers.TryNewParameterLess(
            type,
            expectFail,
            (out object? instance, out Exception? exception) =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.TryNew(out instance, out exception);
            });
    }

    [Test]
    public static void TryNewParameterLess()
    {
        ConstructorTestHelpers.TryNewParameterLess(
            (out object? instance, out Exception? exception) =>
            {
                var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
                return immediateType.TryNew(out instance, out exception);
            },
            () => new ParamsOnlyConstructor());

        ConstructorTestHelpers.TryNewParameterLess(
            (out object? instance, out Exception? exception) =>
            {
                var immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
                return immediateType.TryNew(out instance, out exception);
            },
            () => new IntParamsOnlyConstructor());

        ConstructorTestHelpers.TryNewParameterLess(
            (out object? instance, out Exception? exception) =>
            {
                var immediateType = new ImmediateType(typeof(NullableIntParamsOnlyConstructor));
                return immediateType.TryNew(out instance, out exception);
            },
            () => new NullableIntParamsOnlyConstructor());
    }

    #endregion

    #region New(params)/TryNew(params)

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateNotDefaultConstructorTestCases))]
    public static void NewWithParameters(Type type, params object?[] arguments)
    {
        ConstructorTestHelpers.NewWithParameters(
            type,
            args =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.New(args);
            },
            arguments);
    }

    [Test]
    public static void NewWithParameters_Throws()
    {
        var immediateType = new ImmediateType(typeof(NoDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New(12, 42));

        immediateType = new ImmediateType(typeof(NotAccessibleConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New(12));

        immediateType = new ImmediateType(typeof(MultiParametersConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New(12f, 12));

        immediateType = new ImmediateType(typeof(ParamsConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New(12f, 12));

        immediateType = new ImmediateType(typeof(NoDefaultInheritedDefaultConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.New(12f));

        immediateType = new ImmediateType(typeof(AbstractNoConstructor));
        Assert.Throws<MemberAccessException>(() => _ = immediateType.New(12));

        immediateType = new ImmediateType(typeof(TemplateNoDefaultConstructor<>));
        Assert.Throws<ArgumentException>(() => _ = immediateType.New(12));

        immediateType = new ImmediateType(typeof(NotDefaultConstructorThrows));
        Assert.Throws<TargetInvocationException>(() => _ = immediateType.New(12));
    }

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateNotDefaultConstructorNoThrowTestCases))]
    public static void TryNewWithParameters(Type type, bool expectFail, params object?[] arguments)
    {
        ConstructorTestHelpers.TryNewWithParameters(
            type,
            expectFail,
            (out object? instance, out Exception? exception, object?[] args) =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.TryNew(out instance, out exception, args);
            },
            arguments);
    }

    #endregion

    #endregion

    #region Copy Constructor

    #region Has Copy Constructor

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateHasCopyConstructorTestCases))]
    public static bool HasCopyConstructor(Type type)
    {
        var immediateType = new ImmediateType(type);
        return immediateType.HasCopyConstructor;
    }

    #endregion

    #region Copy/TryCopy

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateCopyConstructorTestCases))]
    public static void Copy(Type type, object? other)
    {
        ConstructorTestHelpers.Copy(
            type,
            other,
            o =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.Copy(o);
            });
    }

    [Test]
    public static void Copy_Throws()
    {
        var immediateType = new ImmediateType(typeof(NoCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new NoCopyConstructorClass()));

        immediateType = new ImmediateType(typeof(NotAccessibleCopyConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new NotAccessibleCopyConstructor()));

        immediateType = new ImmediateType(typeof(IList<int>));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new List<int>()));

        immediateType = new ImmediateType(typeof(IDictionary<int, string>));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new Dictionary<int, string>()));

        immediateType = new ImmediateType(typeof(AbstractCopyConstructor));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new CopyConstructorClass(12)));

        immediateType = new ImmediateType(typeof(StaticClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new CopyConstructorClass(12)));

        immediateType = new ImmediateType(typeof(TemplateStruct<>));
        Assert.Throws<ArgumentException>(() => _ = immediateType.Copy(new CopyConstructorClass(12)));

        immediateType = new ImmediateType(typeof(TemplateCopyConstructor<>));
        Assert.Throws<ArgumentException>(() => _ = immediateType.Copy(new CopyConstructorClass(12)));

        immediateType = new ImmediateType(typeof(NoCopyInheritedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new NoCopyInheritedCopyConstructorClass(1)));

        immediateType = new ImmediateType(typeof(NoCopyInheritedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new CopyConstructorClass(2)));

        immediateType = new ImmediateType(typeof(BaseCopyInheritedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new BaseCopyInheritedCopyConstructorClass(3)));

        immediateType = new ImmediateType(typeof(BaseCopyInheritedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new CopyConstructorClass(4))); // Constructor exists but is not considered as copy constructor

        immediateType = new ImmediateType(typeof(SpecializedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new SpecializedCopyConstructorClass(5)));

        immediateType = new ImmediateType(typeof(SpecializedCopyConstructorClass));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new InheritedSpecializedCopyConstructorClass(6))); // Constructor exists but is not considered as copy constructor

        immediateType = new ImmediateType(typeof(MultipleCopyConstructorClass));
        Assert.Throws<ArgumentException>(() => _ = immediateType.Copy(new InheritedMultipleCopyConstructorClass(12))); // Constructor exists but is not considered as copy constructor

        // ReSharper disable once PossibleMistakenCallToGetType.2
        immediateType = new ImmediateType(typeof(CopyConstructorClass).GetType());
        Assert.Throws<ArgumentException>(() => _ = immediateType.Copy(new CopyConstructorClass(12)));

        immediateType = new ImmediateType(typeof(CopyConstructorThrows));
        Assert.Throws(Is.InstanceOf<Exception>(), () => _ = immediateType.Copy(new CopyConstructorThrows()));

        immediateType = new ImmediateType(typeof(int[]));
        Assert.Throws<MissingMethodException>(() => _ = immediateType.Copy(new int[0]));

        // Wrong argument
        immediateType = new ImmediateType(typeof(CopyConstructorClass));
        Assert.Throws<ArgumentException>(() => _ = immediateType.Copy(new NoCopyConstructorClass()));
    }

    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateCopyConstructorNoThrowTestCases))]
    public static void TryCopy(Type type, object? other, bool expectFail)
    {
        ConstructorTestHelpers.TryCopy(
            type,
            other,
            expectFail,
            (object? o, out object? instance, out Exception? exception) =>
            {
                var immediateType = new ImmediateType(type);
                return immediateType.TryCopy(o, out instance, out exception);
            });
    }

    #endregion

    #endregion

    #region Equals/HashCode/ToString

    [Test]
    public static void ImmediateTypeEquality()
    {
        var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
        var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
        Assert.IsTrue(immediateType1.Equals(immediateType1));
        Assert.IsTrue(immediateType1.Equals(immediateType2));
        Assert.IsTrue(immediateType1.Equals((object)immediateType2));

        var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
        Assert.IsFalse(immediateType1.Equals(immediateType3));
        Assert.IsFalse(immediateType1.Equals((object)immediateType3));

        Assert.IsFalse(immediateType1.Equals(null));
    }

    [Test]
    public static void ImmediateTypeHashCode()
    {
        var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
        var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
        Assert.AreEqual(typeof(PublicValueTypeTestClass).GetHashCode(), immediateType1.GetHashCode());
        Assert.AreEqual(immediateType1.GetHashCode(), immediateType2.GetHashCode());

        var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
        Assert.AreNotEqual(immediateType1.GetHashCode(), immediateType3.GetHashCode());
    }

    [Test]
    public static void ImmediateTypeToString()
    {
        var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
        Assert.AreEqual(typeof(PublicValueTypeTestClass).ToString(), immediateType1.ToString());

        var immediateType2 = new ImmediateType(typeof(InternalValueTypeTestClass));
        Assert.AreNotEqual(immediateType1.ToString(), immediateType2.ToString());
    }

    #endregion
}