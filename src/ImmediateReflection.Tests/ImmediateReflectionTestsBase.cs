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
        protected static PropertyInfo[] EmptyPropertyInfo = { };

        // Fields //

        [NotNull, ItemNotNull]
        protected static FieldInfo[] EmptyFieldInfo = { };

        #region Types members classifiers

        [Pure]
        [NotNull, ItemNotNull]
        protected static IEnumerable<FieldInfo> IgnoreBackingFields([NotNull, ItemNotNull] IEnumerable<FieldInfo> fields)
        {
            const string backingFieldName = "BackingField";
            return fields.Where(field => !field.Name.Contains(backingFieldName));
        }

        protected struct TypeClassifiedMembers
        {
            public FieldInfo[] PublicInstanceFields { get; set; }
            public FieldInfo[] NonPublicInstanceFields { get; set; }
            public FieldInfo[] StaticFields { get; set; }

            public PropertyInfo[] PublicInstanceProperties { get; set; }
            public PropertyInfo[] NonPublicInstanceProperties { get; set; }
            public PropertyInfo[] StaticProperties { get; set; }

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
                        PublicValueTypeStaticPublicFieldFieldsInfo
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
        }

        #endregion

        #region Struct

        // Properties //

        [NotNull]
        protected static PropertyInfo TestStructTestPropertyPropertyInfo =
            typeof(TestStruct).GetProperty(nameof(TestStruct.TestValue)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo TestStructTestFieldFieldInfo =
            typeof(TestStruct).GetField(nameof(TestStruct._testValue)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #region Small objects

        // Properties //

        // Small Object
        [NotNull, ItemNotNull]
        protected static PropertyInfo[] SmallObjectPropertyInfos = typeof(SmallObject).GetProperties();

        [NotNull]
        protected static PropertyInfo SmallObjectTestProperty1PropertyInfo =
            typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty1)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo SmallObjectTestProperty2PropertyInfo =
            typeof(SmallObject).GetProperty(nameof(SmallObject.TestProperty2)) ?? throw new AssertionException("Cannot find property.");


        // Second Small Object
        [NotNull, ItemNotNull]
        protected static PropertyInfo[] SecondSmallObjectPropertyInfos = typeof(SecondSmallObject).GetProperties();


        // Fields //

        // Small Object
        [NotNull, ItemNotNull]
        protected static FieldInfo[] SmallObjectFieldInfos = typeof(SmallObject).GetFields();

        [NotNull]
        protected static FieldInfo SmallObjectTestField1FieldInfo =
            typeof(SmallObject).GetField(nameof(SmallObject._testField1)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo SmallObjectTestField2FieldInfo =
            typeof(SmallObject).GetField(nameof(SmallObject._testField2)) ?? throw new AssertionException("Cannot find field.");


        // Second Small Object
        [NotNull, ItemNotNull]
        protected static FieldInfo[] SecondSmallObjectFieldInfos = typeof(SecondSmallObject).GetFields();

        #endregion

        #region Get/Set objects

        #region Public TestClass

        // PublicValueTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicGetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.InternalPropertyGetSet), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo PublicValueTypePublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypePublicField2FieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypeInternalFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypeProtectedFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypePrivateFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypeStaticPublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");



        // PublicReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo PublicReferenceTypePublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypePublicField2FieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypeInternalFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypeProtectedFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypePrivateFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypeStaticPublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");



        // PublicObjectTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicGetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePublicSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty(nameof(PublicObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypeInternalGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypeProtectedGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicObjectTypePrivateGetSetPropertyPropertyInfo =
            typeof(PublicObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo PublicObjectTypePublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicObjectTypePublicField2FieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicObjectTypeInternalFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicObjectTypeProtectedFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicObjectTypePrivateFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicObjectTypeStaticPublicFieldFieldsInfo =
            typeof(PublicObjectTypeTestClass).GetField(nameof(PublicObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #region Internal TestClass

        // InternalValueTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo InternalValueTypePublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypePublicField2FieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypeInternalFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypeProtectedFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypePrivateFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypeStaticPublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");



        // InternalReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo InternalReferenceTypePublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypePublicField2FieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypeInternalFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypeProtectedFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypePrivateFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypeStaticPublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");



        // InternalObjectTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicVirtualGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicVirtualPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicGetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicPrivateGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyPrivateGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePublicSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypeStaticPublicGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty(nameof(InternalObjectTypeTestClass.PublicStaticPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypeInternalGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("InternalPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypeProtectedGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("ProtectedPropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalObjectTypePrivateGetSetPropertyPropertyInfo =
            typeof(InternalObjectTypeTestClass).GetProperty("PrivatePropertyGetSet", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo InternalObjectTypePublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalObjectTypePublicField2FieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalObjectTypeInternalFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._internalField), BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalObjectTypeProtectedFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField("_protectedField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalObjectTypePrivateFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField("_privateField", BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalObjectTypeStaticPublicFieldFieldsInfo =
            typeof(InternalObjectTypeTestClass).GetField(nameof(InternalObjectTypeTestClass._publicStaticField)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion

        #region Nested class

        // Fields //

        [NotNull]
        protected static FieldInfo PublicNestedPublicFieldFieldInfo =
            typeof(PublicTestClass.PublicNestedClass).GetField(nameof(PublicTestClass.PublicNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalNestedPublicFieldFieldInfo =
            typeof(PublicTestClass.InternalNestedClass).GetField(nameof(PublicTestClass.InternalNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo ProtectedNestedPublicFieldFieldInfo =
            typeof(ProtectedNestedClass).GetField(nameof(ProtectedNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");


        [NotNull, ItemNotNull]
        protected static FieldInfo[] PublicNestedFieldInfos = typeof(PublicTestClass.PublicNestedClass).GetFields();

        // Properties //

        [NotNull]
        protected static PropertyInfo PublicNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.PublicNestedClass).GetProperty(nameof(PublicTestClass.PublicNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.InternalNestedClass).GetProperty(nameof(PublicTestClass.InternalNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo ProtectedNestedPublicGetSetPropertyPropertyInfo =
            typeof(ProtectedNestedClass).GetProperty(nameof(ProtectedNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");


        [NotNull, ItemNotNull]
        protected static PropertyInfo[] PublicNestedPropertyInfos = typeof(PublicTestClass.PublicNestedClass).GetProperties();

        #endregion

        #endregion
    }
}