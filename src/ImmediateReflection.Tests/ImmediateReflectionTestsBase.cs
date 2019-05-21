using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Base class for unit tests.
    /// </summary>
    [TestFixture]
    internal class ImmediateReflectionTestsBase
    {
        protected class ProtectedNestedClass
        {
            // ReSharper disable once InconsistentNaming
            public int _nestedTestValue;

            public int NestedTestValue { get; set; } = 25;
        }

        #region Test Helpers

        // Properties //
        [NotNull, ItemNotNull]
        protected static readonly PropertyInfo[] EmptyPropertyInfo = { };

        // Fields //

        [NotNull, ItemNotNull]
        protected static readonly FieldInfo[] EmptyFieldInfo = { };

        #region Types members classifiers

        protected struct TypeClassifiedMembers
        {
            [NotNull, ItemNotNull]
            public FieldInfo[] PublicInstanceFields { get; set; }

            [NotNull, ItemNotNull]
            public FieldInfo[] NonPublicInstanceFields { get; set; }

            [NotNull, ItemNotNull]
            public FieldInfo[] StaticFields { get; set; }

            [NotNull, ItemNotNull]
            public FieldInfo[] ConstFields { get; set; }

            [NotNull, ItemNotNull]
            public IEnumerable<FieldInfo> AllFields => PublicInstanceFields
                .Concat(NonPublicInstanceFields)
                .Concat(StaticFields)
                .Concat(ConstFields);

            [NotNull, ItemNotNull]
            public PropertyInfo[] PublicInstanceProperties { get; set; }

            [NotNull, ItemNotNull]
            public PropertyInfo[] NonPublicInstanceProperties { get; set; }

            [NotNull, ItemNotNull]
            public PropertyInfo[] StaticProperties { get; set; }

            [NotNull, ItemNotNull]
            public IEnumerable<PropertyInfo> AllProperties => PublicInstanceProperties
                .Concat(NonPublicInstanceProperties)
                .Concat(StaticProperties);

            [NotNull, ItemNotNull]
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
                    PublicInstanceFields = new[]
                    {
                        PublicValueTypePublicFieldFieldsInfo,
                        PublicValueTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        PublicValueTypeInternalFieldFieldsInfo,
                        PublicValueTypeProtectedFieldFieldsInfo,
                        PublicValueTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        PublicValueTypeStaticPublicFieldFieldsInfo,
                        PublicValueTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new[]
                    {
                        PublicValueTypeConstPublicFieldFieldsInfo
                    },
                    PublicInstanceProperties = new[]
                    {
                        PublicValueTypePublicGetSetPropertyPropertyInfo,
                        PublicValueTypePublicVirtualGetSetPropertyPropertyInfo,
                        PublicValueTypePublicGetPropertyPropertyInfo,
                        PublicValueTypePublicPrivateGetSetPropertyPropertyInfo,
                        PublicValueTypePublicGetPrivateSetPropertyPropertyInfo,
                        PublicValueTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        PublicValueTypeInternalGetSetPropertyPropertyInfo,
                        PublicValueTypeProtectedGetSetPropertyPropertyInfo,
                        PublicValueTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        PublicValueTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            public static TypeClassifiedMembers GetForInternalValueTypeTestObject()
            {
                return new TypeClassifiedMembers
                {
                    PublicInstanceFields = new[]
                    {
                        InternalValueTypePublicFieldFieldsInfo,
                        InternalValueTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        InternalValueTypeInternalFieldFieldsInfo,
                        InternalValueTypeProtectedFieldFieldsInfo,
                        InternalValueTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        InternalValueTypeStaticPublicFieldFieldsInfo,
                        InternalValueTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new[]
                    {
                        InternalValueTypeConstPublicFieldFieldsInfo
                    },
                    PublicInstanceProperties = new[]
                    {
                        InternalValueTypePublicGetSetPropertyPropertyInfo,
                        InternalValueTypePublicVirtualGetSetPropertyPropertyInfo,
                        InternalValueTypePublicGetPropertyPropertyInfo,
                        InternalValueTypePublicPrivateGetSetPropertyPropertyInfo,
                        InternalValueTypePublicGetPrivateSetPropertyPropertyInfo,
                        InternalValueTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        InternalValueTypeInternalGetSetPropertyPropertyInfo,
                        InternalValueTypeProtectedGetSetPropertyPropertyInfo,
                        InternalValueTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        InternalValueTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            public static TypeClassifiedMembers GetForPublicReferenceTypeTestObject()
            {
                return new TypeClassifiedMembers
                {
                    PublicInstanceFields = new[]
                    {
                        PublicReferenceTypePublicFieldFieldsInfo,
                        PublicReferenceTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        PublicReferenceTypeInternalFieldFieldsInfo,
                        PublicReferenceTypeProtectedFieldFieldsInfo,
                        PublicReferenceTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        PublicReferenceTypeStaticPublicFieldFieldsInfo,
                        PublicReferenceTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new FieldInfo[]
                    {
                    },
                    PublicInstanceProperties = new[]
                    {
                        PublicReferenceTypePublicGetSetPropertyPropertyInfo,
                        PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                        PublicReferenceTypePublicGetPropertyPropertyInfo,
                        PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                        PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                        PublicReferenceTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        PublicReferenceTypeInternalGetSetPropertyPropertyInfo,
                        PublicReferenceTypeProtectedGetSetPropertyPropertyInfo,
                        PublicReferenceTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            public static TypeClassifiedMembers GetForInternalReferenceTypeTestObject()
            {
                return new TypeClassifiedMembers
                {
                    PublicInstanceFields = new[]
                    {
                        InternalReferenceTypePublicFieldFieldsInfo,
                        InternalReferenceTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        InternalReferenceTypeInternalFieldFieldsInfo,
                        InternalReferenceTypeProtectedFieldFieldsInfo,
                        InternalReferenceTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        InternalReferenceTypeStaticPublicFieldFieldsInfo,
                        InternalReferenceTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new FieldInfo[]
                    {
                    },
                    PublicInstanceProperties = new[]
                    {
                        InternalReferenceTypePublicGetSetPropertyPropertyInfo,
                        InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo,
                        InternalReferenceTypePublicGetPropertyPropertyInfo,
                        InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo,
                        InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo,
                        InternalReferenceTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        InternalReferenceTypeInternalGetSetPropertyPropertyInfo,
                        InternalReferenceTypeProtectedGetSetPropertyPropertyInfo,
                        InternalReferenceTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            public static TypeClassifiedMembers GetForPublicObjectTypeTestObject()
            {
                return new TypeClassifiedMembers
                {
                    PublicInstanceFields = new[]
                    {
                        PublicObjectTypePublicFieldFieldsInfo,
                        PublicObjectTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        PublicObjectTypeInternalFieldFieldsInfo,
                        PublicObjectTypeProtectedFieldFieldsInfo,
                        PublicObjectTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        PublicObjectTypeStaticPublicFieldFieldsInfo,
                        PublicObjectTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new[]
                    {
                        PublicObjectTypeConstPublicFieldFieldsInfo
                    },
                    PublicInstanceProperties = new[]
                    {
                        PublicObjectTypePublicGetSetPropertyPropertyInfo,
                        PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                        PublicObjectTypePublicGetPropertyPropertyInfo,
                        PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                        PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                        PublicObjectTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        PublicObjectTypeInternalGetSetPropertyPropertyInfo,
                        PublicObjectTypeProtectedGetSetPropertyPropertyInfo,
                        PublicObjectTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            public static TypeClassifiedMembers GetForInternalObjectTypeTestObject()
            {
                return new TypeClassifiedMembers
                {
                    PublicInstanceFields = new[]
                    {
                        InternalObjectTypePublicFieldFieldsInfo,
                        InternalObjectTypePublicField2FieldsInfo
                    },
                    NonPublicInstanceFields = new[]
                    {
                        InternalObjectTypeInternalFieldFieldsInfo,
                        InternalObjectTypeProtectedFieldFieldsInfo,
                        InternalObjectTypePrivateFieldFieldsInfo
                    },
                    StaticFields = new[]
                    {
                        InternalObjectTypeStaticPublicFieldFieldsInfo,
                        InternalObjectTypeStaticReadonlyPublicFieldFieldsInfo
                    },
                    ConstFields = new[]
                    {
                        InternalObjectTypeConstPublicFieldFieldsInfo
                    },
                    PublicInstanceProperties = new[]
                    {
                        InternalObjectTypePublicGetSetPropertyPropertyInfo,
                        InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo,
                        InternalObjectTypePublicGetPropertyPropertyInfo,
                        InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo,
                        InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo,
                        InternalObjectTypePublicSetPropertyPropertyInfo
                    },
                    NonPublicInstanceProperties = new[]
                    {
                        InternalObjectTypeInternalGetSetPropertyPropertyInfo,
                        InternalObjectTypeProtectedGetSetPropertyPropertyInfo,
                        InternalObjectTypePrivateGetSetPropertyPropertyInfo
                    },
                    StaticProperties = new[]
                    {
                        InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo
                    }
                };
            }

            #endregion
        }

        #endregion

        #region Enums

        private const string EnumValueFieldName = "value__";

        // TestEnum

        [NotNull]
        protected static readonly FieldInfo TestEnumFieldValueFieldInfo =
            typeof(TestEnum).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumField1FieldInfo =
            typeof(TestEnum).GetField(nameof(TestEnum.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumField2FieldInfo =
            typeof(TestEnum).GetField(nameof(TestEnum.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

        // TestEnumULong

        [NotNull]
        protected static readonly FieldInfo TestEnumULongFieldValueFieldInfo =
            typeof(TestEnumULong).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumULongField1FieldInfo =
            typeof(TestEnumULong).GetField(nameof(TestEnumULong.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumULongField2FieldInfo =
            typeof(TestEnumULong).GetField(nameof(TestEnumULong.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

        // TestEnumFlags

        [NotNull]
        protected static readonly FieldInfo TestEnumFlagsFieldValueFieldInfo =
            typeof(TestEnumFlags).GetField(EnumValueFieldName) ?? throw new AssertionException("Cannot find enum value field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumFlagsField1FieldInfo =
            typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue1)) ?? throw new AssertionException("Cannot find enum field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumFlagsField2FieldInfo =
            typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue2)) ?? throw new AssertionException("Cannot find enum field.");

        [NotNull]
        protected static readonly FieldInfo TestEnumFlagsField3FieldInfo =
            typeof(TestEnumFlags).GetField(nameof(TestEnumFlags.EnumValue3)) ?? throw new AssertionException("Cannot find enum field.");

        #endregion

        #region Struct

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo TestStructTestPropertyPropertyInfo =
            typeof(TestStruct).GetProperty(nameof(TestStruct.TestValue)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo TestStructTestFieldFieldInfo =
            typeof(TestStruct).GetField(nameof(TestStruct._testValue)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #region Small objects

        // Properties //

        // Small Object
        [NotNull, ItemNotNull]
        protected static readonly PropertyInfo[] SmallObjectPropertyInfos = typeof(SmallObject).GetProperties();

        [NotNull]
        protected static readonly PropertyInfo SmallObjectTestProperty1PropertyInfo =
            typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo SmallObjectTestProperty2PropertyInfo =
            typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty2)) ?? throw new AssertionException("Cannot find property.");


        // Second Small Object
        [NotNull, ItemNotNull]
        protected static readonly PropertyInfo[] SecondSmallObjectPropertyInfos = typeof(SecondSmallObject).GetProperties();


        // Fields //

        // Small Object
        [NotNull, ItemNotNull]
        protected static readonly FieldInfo[] SmallObjectFieldInfos = typeof(SmallObject).GetFields();

        [NotNull]
        protected static readonly FieldInfo SmallObjectTestField1FieldInfo =
            typeof(SmallObject).GetField(nameof(SmallObject._testField1)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo SmallObjectTestField2FieldInfo =
            typeof(SmallObject).GetField(nameof(SmallObject._testField2)) ?? throw new AssertionException("Cannot find field.");


        // Second Small Object
        [NotNull, ItemNotNull]
        protected static readonly FieldInfo[] SecondSmallObjectFieldInfos = typeof(SecondSmallObject).GetFields();

        #endregion

        #region Get/Set objects

        #region Public TestClass

        // PublicValueTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicGetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePublicSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.InternalPropertyGetSet), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicValueTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo PublicValueTypePublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypePublicField2FieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypeInternalFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypeProtectedFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypePrivateFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypeStaticPublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicValueTypeConstPublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");


        // PublicReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicGetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePublicSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicReferenceTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypePublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypePublicField2FieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypeInternalFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypeProtectedFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypePrivateFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypeStaticPublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicReferenceTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");



        // PublicObjectTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicGetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePublicSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo PublicObjectTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypePublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypePublicField2FieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypeInternalFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypeProtectedFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypePrivateFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypeStaticPublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo PublicObjectTypeConstPublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #region Internal TestClass

        // InternalValueTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicGetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePublicSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalValueTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo InternalValueTypePublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypePublicField2FieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypeInternalFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypeProtectedFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypePrivateFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypeStaticPublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalValueTypeConstPublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");



        // InternalReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicGetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePublicSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalReferenceTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypePublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypePublicField2FieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypeInternalFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypeProtectedFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypePrivateFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypeStaticPublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalReferenceTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");


        // InternalObjectTypeTestClass

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicGetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePublicSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalObjectTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypePublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypePublicField2FieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypeInternalFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypeProtectedFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypePrivateFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypeStaticPublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypeStaticReadonlyPublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicStaticReadonlyField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalObjectTypeConstPublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicConstField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion

        #region Nested class

        // Fields //

        [NotNull]
        protected static readonly FieldInfo PublicNestedPublicFieldFieldInfo =
            typeof(PublicTestClass.PublicNestedClass).GetField(nameof(PublicTestClass.PublicNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo InternalNestedPublicFieldFieldInfo =
            typeof(PublicTestClass.InternalNestedClass).GetField(nameof(PublicTestClass.InternalNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static readonly FieldInfo ProtectedNestedPublicFieldFieldInfo =
            typeof(ProtectedNestedClass).GetField(nameof(ProtectedNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");


        [NotNull, ItemNotNull]
        protected static readonly FieldInfo[] PublicNestedFieldInfos = typeof(PublicTestClass.PublicNestedClass).GetFields();

        // Properties //

        [NotNull]
        protected static readonly PropertyInfo PublicNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.PublicNestedClass).GetProperty(nameof(PublicTestClass.PublicNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo InternalNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.InternalNestedClass).GetProperty(nameof(PublicTestClass.InternalNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static readonly PropertyInfo ProtectedNestedPublicGetSetPropertyPropertyInfo =
            typeof(ProtectedNestedClass).GetProperty(nameof(ProtectedNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");


        [NotNull, ItemNotNull]
        protected static readonly PropertyInfo[] PublicNestedPropertyInfos = typeof(PublicTestClass.PublicNestedClass).GetProperties();

        #endregion

        #endregion
    }
}