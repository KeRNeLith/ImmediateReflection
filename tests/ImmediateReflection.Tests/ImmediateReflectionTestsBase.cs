using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Base class for unit tests.
/// </summary>
internal abstract class ImmediateReflectionTestsBase
{
    protected sealed class ProtectedNestedClass
    {
        // ReSharper disable once InconsistentNaming
        public int _nestedTestValue;

        public int NestedTestValue { get; set; } = 25;
    }

    #region Test Helpers

    // Properties //
    protected static readonly PropertyInfo[] EmptyPropertyInfo = [];

    // Fields //
    protected static readonly FieldInfo[] EmptyFieldInfo = [];

    [Pure]
    protected static bool IsAnonymousType(Type type)
    {
        bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length != 0;
        bool nameContainsAnonymousType = type.FullName is not null && type.FullName.Contains("AnonymousType");
        bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

        return isAnonymousType;
    }

    #region Types members classifiers

    protected struct TypeClassifiedMembers
    {
        public FieldInfo[] PublicInstanceFields { get; set; }
        public FieldInfo[] NonPublicInstanceFields { get; set; }
        public FieldInfo[] StaticFields { get; set; }
        public FieldInfo[] ConstFields { get; set; }

        public IEnumerable<FieldInfo> AllPublicFields => PublicInstanceFields
            .Concat(StaticFields)
            .Concat(ConstFields);

        public IEnumerable<FieldInfo> AllFields => AllPublicFields
            .Concat(NonPublicInstanceFields);

        public PropertyInfo[] PublicInstanceProperties { get; set; }
        public PropertyInfo[] NonPublicInstanceProperties { get; set; }
        public PropertyInfo[] StaticProperties { get; set; }

        public IEnumerable<PropertyInfo> AllPublicProperties => PublicInstanceProperties
            .Concat(StaticProperties);

        public IEnumerable<PropertyInfo> AllProperties => AllPublicProperties
            .Concat(NonPublicInstanceProperties);

        public IEnumerable<MemberInfo> AllPublicMembers
        {
            get
            {
                foreach (FieldInfo field in AllPublicFields)
                    yield return field;
                foreach (PropertyInfo property in AllPublicProperties)
                    yield return property;
            }
        }

        public IEnumerable<MemberInfo> AllMembers
        {
            get
            {
                foreach (FieldInfo field in AllFields)
                    yield return field;
                foreach (PropertyInfo property in AllProperties)
                    yield return property;
            }
        }

        #region Predefined classifiers

        public static TypeClassifiedMembers GetForPublicValueTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    PublicValueTypePublicFieldFieldsInfo,
                    PublicValueTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    PublicValueTypeInternalFieldFieldsInfo,
                    PublicValueTypeProtectedFieldFieldsInfo,
                    PublicValueTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    PublicValueTypeStaticPublicFieldFieldsInfo,
                    PublicValueTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                    PublicValueTypeConstPublicFieldFieldsInfo
                ],
                PublicInstanceProperties =
                [
                    PublicValueTypePublicGetSetPropertyPropertyInfo,
                    PublicValueTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicValueTypePublicGetPropertyPropertyInfo,
                    PublicValueTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicValueTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicValueTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    PublicValueTypeInternalGetSetPropertyPropertyInfo,
                    PublicValueTypeProtectedGetSetPropertyPropertyInfo,
                    PublicValueTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    PublicValueTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        public static TypeClassifiedMembers GetForInternalValueTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    InternalValueTypePublicFieldFieldsInfo,
                    InternalValueTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    InternalValueTypeInternalFieldFieldsInfo,
                    InternalValueTypeProtectedFieldFieldsInfo,
                    InternalValueTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    InternalValueTypeStaticPublicFieldFieldsInfo,
                    InternalValueTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                    InternalValueTypeConstPublicFieldFieldsInfo
                ],
                PublicInstanceProperties =
                [
                    InternalValueTypePublicGetSetPropertyPropertyInfo,
                    InternalValueTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalValueTypePublicGetPropertyPropertyInfo,
                    InternalValueTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalValueTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalValueTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    InternalValueTypeInternalGetSetPropertyPropertyInfo,
                    InternalValueTypeProtectedGetSetPropertyPropertyInfo,
                    InternalValueTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    InternalValueTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        public static TypeClassifiedMembers GetForPublicReferenceTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    PublicReferenceTypePublicFieldFieldsInfo,
                    PublicReferenceTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    PublicReferenceTypeInternalFieldFieldsInfo,
                    PublicReferenceTypeProtectedFieldFieldsInfo,
                    PublicReferenceTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    PublicReferenceTypeStaticPublicFieldFieldsInfo,
                    PublicReferenceTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                ],
                PublicInstanceProperties =
                [
                    PublicReferenceTypePublicGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicGetPropertyPropertyInfo,
                    PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicReferenceTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    PublicReferenceTypeInternalGetSetPropertyPropertyInfo,
                    PublicReferenceTypeProtectedGetSetPropertyPropertyInfo,
                    PublicReferenceTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        public static TypeClassifiedMembers GetForInternalReferenceTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    InternalReferenceTypePublicFieldFieldsInfo,
                    InternalReferenceTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    InternalReferenceTypeInternalFieldFieldsInfo,
                    InternalReferenceTypeProtectedFieldFieldsInfo,
                    InternalReferenceTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    InternalReferenceTypeStaticPublicFieldFieldsInfo,
                    InternalReferenceTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                ],
                PublicInstanceProperties =
                [
                    InternalReferenceTypePublicGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicGetPropertyPropertyInfo,
                    InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalReferenceTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    InternalReferenceTypeInternalGetSetPropertyPropertyInfo,
                    InternalReferenceTypeProtectedGetSetPropertyPropertyInfo,
                    InternalReferenceTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        public static TypeClassifiedMembers GetForPublicObjectTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    PublicObjectTypePublicFieldFieldsInfo,
                    PublicObjectTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    PublicObjectTypeInternalFieldFieldsInfo,
                    PublicObjectTypeProtectedFieldFieldsInfo,
                    PublicObjectTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    PublicObjectTypeStaticPublicFieldFieldsInfo,
                    PublicObjectTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                    PublicObjectTypeConstPublicFieldFieldsInfo
                ],
                PublicInstanceProperties =
                [
                    PublicObjectTypePublicGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicGetPropertyPropertyInfo,
                    PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                    PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                    PublicObjectTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    PublicObjectTypeInternalGetSetPropertyPropertyInfo,
                    PublicObjectTypeProtectedGetSetPropertyPropertyInfo,
                    PublicObjectTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        public static TypeClassifiedMembers GetForInternalObjectTypeTestObject()
        {
            return new TypeClassifiedMembers
            {
                PublicInstanceFields =
                [
                    InternalObjectTypePublicFieldFieldsInfo,
                    InternalObjectTypePublicField2FieldsInfo
                ],
                NonPublicInstanceFields =
                [
                    InternalObjectTypeInternalFieldFieldsInfo,
                    InternalObjectTypeProtectedFieldFieldsInfo,
                    InternalObjectTypePrivateFieldFieldsInfo
                ],
                StaticFields =
                [
                    InternalObjectTypeStaticPublicFieldFieldsInfo,
                    InternalObjectTypeStaticReadonlyPublicFieldFieldsInfo
                ],
                ConstFields =
                [
                    InternalObjectTypeConstPublicFieldFieldsInfo
                ],
                PublicInstanceProperties =
                [
                    InternalObjectTypePublicGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicGetPropertyPropertyInfo,
                    InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                    InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                    InternalObjectTypePublicSetPropertyPropertyInfo
                ],
                NonPublicInstanceProperties =
                [
                    InternalObjectTypeInternalGetSetPropertyPropertyInfo,
                    InternalObjectTypeProtectedGetSetPropertyPropertyInfo,
                    InternalObjectTypePrivateGetSetPropertyPropertyInfo
                ],
                StaticProperties =
                [
                    InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo
                ]
            };
        }

        #endregion
    }

    #endregion

    #region Enums

    protected const string EnumValueFieldName = "value__";

    // TestEnum

    protected static readonly FieldInfo TestEnumFieldValueFieldInfo =
        typeof(TestEnum).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

    protected static readonly FieldInfo TestEnumField1FieldInfo =
        typeof(TestEnum).GetField(nameof(TestEnum.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

    protected static readonly FieldInfo TestEnumField2FieldInfo =
        typeof(TestEnum).GetField(nameof(TestEnum.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

    // TestEnumULong

    protected static readonly FieldInfo TestEnumULongFieldValueFieldInfo =
        typeof(TestEnumULong).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

    protected static readonly FieldInfo TestEnumULongField1FieldInfo =
        typeof(TestEnumULong).GetField(nameof(TestEnumULong.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

    protected static readonly FieldInfo TestEnumULongField2FieldInfo =
        typeof(TestEnumULong).GetField(nameof(TestEnumULong.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

    // TestEnumFlags

    protected static readonly FieldInfo TestEnumFlagsFieldValueFieldInfo =
        typeof(TestEnumFlags).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

    protected static readonly FieldInfo TestEnumFlagsField1FieldInfo =
        typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

    protected static readonly FieldInfo TestEnumFlagsField2FieldInfo =
        typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

    protected static readonly FieldInfo TestEnumFlagsField3FieldInfo =
        typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue3)) ?? throw new AssertionException("Cannot find enum field.");

    #endregion

    #region Struct

    // Properties //

    protected static readonly PropertyInfo TestStructTestPropertyPropertyInfo =
        typeof(TestStruct).GetProperty(nameof(TestStruct.TestValue)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo TestStructStaticTestPropertyPropertyInfo =
        typeof(TestStruct).GetProperty(nameof(TestStruct.TestStaticValue)) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo TestStructTestFieldFieldInfo =
        typeof(TestStruct).GetField(nameof(TestStruct._testValue)) ?? throw new AssertionException("Cannot find field.");

    #endregion

    #region Small objects

    // Properties //

    // Small Object
    protected static readonly PropertyInfo[] SmallObjectPropertyInfos = typeof(SmallObject).GetProperties();

    protected static readonly PropertyInfo SmallObjectTestProperty1PropertyInfo =
        typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo SmallObjectTestProperty2PropertyInfo =
        typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty2)) ?? throw new AssertionException("Cannot find property.");


    // Second Small Object
    protected static readonly PropertyInfo[] SecondSmallObjectPropertyInfos = typeof(SecondSmallObject).GetProperties();


    // Fields //

    // Small Object
    protected static readonly FieldInfo[] SmallObjectFieldInfos = typeof(SmallObject).GetFields();

    protected static readonly FieldInfo SmallObjectTestField1FieldInfo =
        typeof(SmallObject).GetField(nameof(SmallObject._testField1)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo SmallObjectTestField2FieldInfo =
        typeof(SmallObject).GetField(nameof(SmallObject._testField2)) ?? throw new AssertionException("Cannot find field.");


    // Second Small Object
    protected static readonly FieldInfo[] SecondSmallObjectFieldInfos = typeof(SecondSmallObject).GetFields();

    #endregion

    #region Get/Set objects

    #region Public TestClass

    // PublicValueTypeTestClass

    // Properties //

    protected static readonly PropertyInfo PublicValueTypePublicGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicGetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypeInternalGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.InternalPropertyGetSet), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypeProtectedGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePrivateGetSetPropertyPropertyInfo =
        typeof(PublicValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicAbstractGetSetPropertyPropertyInfo =
        typeof(AbstractPublicValueTypeTestClass).GetProperty(nameof(AbstractPublicValueTypeTestClass.PublicAbstractGetSetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicValueTypePublicConcreteGetSetPropertyPropertyInfo =
        typeof(ConcretePublicValueTypeTestClass).GetProperty(nameof(ConcretePublicValueTypeTestClass.PublicAbstractGetSetProperty)) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo PublicValueTypePublicFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypePublicField2FieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypeInternalFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypeProtectedFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypePrivateFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypeStaticPublicFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicValueTypeConstPublicFieldFieldsInfo =
        typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");


    // PublicReferenceTypeTestClass

    // Properties //

    protected static readonly PropertyInfo PublicReferenceTypePublicGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePublicGetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePublicSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypeInternalGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypeProtectedGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicReferenceTypePrivateGetSetPropertyPropertyInfo =
        typeof(PublicReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo PublicReferenceTypePublicFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypePublicField2FieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypeInternalFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypeProtectedFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypePrivateFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypeStaticPublicFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicReferenceTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");



    // PublicObjectTypeTestClass

    // Properties //

    protected static readonly PropertyInfo PublicObjectTypePublicGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePublicGetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePublicSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypeInternalGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypeProtectedGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo PublicObjectTypePrivateGetSetPropertyPropertyInfo =
        typeof(PublicObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo PublicObjectTypePublicFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypePublicField2FieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypeInternalFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypeProtectedFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypePrivateFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypeStaticPublicFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo PublicObjectTypeConstPublicFieldFieldsInfo =
        typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");

    #endregion

    #region Internal TestClass

    // InternalValueTypeTestClass

    // Properties //

    protected static readonly PropertyInfo InternalValueTypePublicGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePublicGetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePublicSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypeInternalGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypeProtectedGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalValueTypePrivateGetSetPropertyPropertyInfo =
        typeof(InternalValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo InternalValueTypePublicFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypePublicField2FieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypeInternalFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypeProtectedFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypePrivateFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypeStaticPublicFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalValueTypeConstPublicFieldFieldsInfo =
        typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");



    // InternalReferenceTypeTestClass

    // Properties //

    protected static readonly PropertyInfo InternalReferenceTypePublicGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePublicGetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePublicSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypeInternalGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypeProtectedGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalReferenceTypePrivateGetSetPropertyPropertyInfo =
        typeof(InternalReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo InternalReferenceTypePublicFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypePublicField2FieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypeInternalFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypeProtectedFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypePrivateFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypeStaticPublicFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalReferenceTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");


    // InternalObjectTypeTestClass

    // Properties //

    protected static readonly PropertyInfo InternalObjectTypePublicGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePublicGetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePublicSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypeInternalGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypeProtectedGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalObjectTypePrivateGetSetPropertyPropertyInfo =
        typeof(InternalObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

    // Fields //

    protected static readonly FieldInfo InternalObjectTypePublicFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypePublicField2FieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypeInternalFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypeProtectedFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypePrivateFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypeStaticPublicFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypeStaticReadonlyPublicFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalObjectTypeConstPublicFieldFieldsInfo =
        typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");

    #endregion

    #endregion

    #region Nested class

    // Fields //

    protected static readonly FieldInfo PublicNestedPublicFieldFieldInfo =
        typeof(PublicTestClass.PublicNestedClass).GetField(nameof(PublicTestClass.PublicNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo InternalNestedPublicFieldFieldInfo =
        typeof(PublicTestClass.InternalNestedClass).GetField(nameof(PublicTestClass.InternalNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

    protected static readonly FieldInfo ProtectedNestedPublicFieldFieldInfo =
        typeof(ProtectedNestedClass).GetField(nameof(ProtectedNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");


    protected static readonly FieldInfo[] PublicNestedFieldInfos = typeof(PublicTestClass.PublicNestedClass).GetFields();

    // Properties //

    protected static readonly PropertyInfo PublicNestedPublicGetSetPropertyPropertyInfo =
        typeof(PublicTestClass.PublicNestedClass).GetProperty(nameof(PublicTestClass.PublicNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo InternalNestedPublicGetSetPropertyPropertyInfo =
        typeof(PublicTestClass.InternalNestedClass).GetProperty(nameof(PublicTestClass.InternalNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ProtectedNestedPublicGetSetPropertyPropertyInfo =
        typeof(ProtectedNestedClass).GetProperty(nameof(ProtectedNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");


    protected static readonly PropertyInfo[] PublicNestedPropertyInfos = typeof(PublicTestClass.PublicNestedClass).GetProperties();

    #endregion

    #region New keyword

    protected static readonly PropertyInfo BaseClassPublicGetPropertyPropertyInfo =
        typeof(BaseTestClass).GetProperty(nameof(BaseTestClass.Property)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ChildClassPublicGetPropertyPropertyInfo =
        typeof(ChildTestClass).GetProperty(nameof(ChildTestClass.Property)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo =
        typeof(ChildTypeRedefinitionTestClass).GetProperty(
            nameof(ChildTypeRedefinitionTestClass.Property),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
        ?? throw new AssertionException("Cannot find property.");

    #endregion

    #region Item & indexed property

    protected static readonly PropertyInfo ChildItemClassPublicGetPropertyPropertyInfo =
        typeof(ChildItemTestClass).GetProperties().FirstOrDefault(
            p => p.Name == nameof(ChildItemTestClass.Item) && p.GetIndexParameters().Length == 0)
        ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ChildIndexedItemClassPublicGetPropertyPropertyInfo =
        typeof(ChildItemTestClass).GetProperties().FirstOrDefault(
            p => p.Name == "Item" && p.GetIndexParameters().Length > 0)
        ?? throw new AssertionException("Cannot find property.");

    #endregion

    #region Interfaces & implementations

    // Base interface
    protected static readonly PropertyInfo BaseInterfaceGetPropertyPropertyInfo =
        typeof(IBaseTestInterface).GetProperty(nameof(IBaseTestInterface.TestGetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo BaseInterfaceSetPropertyPropertyInfo =
        typeof(IBaseTestInterface).GetProperty(nameof(IBaseTestInterface.TestSetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo BaseInterfaceGetSetPropertyPropertyInfo =
        typeof(IBaseTestInterface).GetProperty(nameof(IBaseTestInterface.TestGetSetProperty)) ?? throw new AssertionException("Cannot find property.");


    // Child interface
    protected static readonly PropertyInfo ChildInterfaceGetSetPropertyPropertyInfo =
        typeof(IChildTestInterface).GetProperty(nameof(IChildTestInterface.TestChildProperty)) ?? throw new AssertionException("Cannot find property.");


    // Interface implementation
    protected static readonly PropertyInfo ImplementationBaseInterfaceFromGetPropertyPropertyInfo =
        typeof(ImplementationInterfacesTestClass).GetProperty(nameof(ImplementationInterfacesTestClass.TestGetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ImplementationBaseInterfaceFromSetPropertyPropertyInfo =
        typeof(ImplementationInterfacesTestClass).GetProperty(nameof(ImplementationInterfacesTestClass.TestSetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ImplementationBaseInterfaceFromGetSetPropertyPropertyInfo =
        typeof(ImplementationInterfacesTestClass).GetProperty(nameof(ImplementationInterfacesTestClass.TestGetSetProperty)) ?? throw new AssertionException("Cannot find property.");

    protected static readonly PropertyInfo ImplementationChildInterfaceFromGetSetPropertyPropertyInfo =
        typeof(ImplementationInterfacesTestClass).GetProperty(nameof(ImplementationInterfacesTestClass.TestChildProperty)) ?? throw new AssertionException("Cannot find property.");

    #endregion

    #endregion
}