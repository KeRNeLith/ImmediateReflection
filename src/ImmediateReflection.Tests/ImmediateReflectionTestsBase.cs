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
        #region Test Helpers

        #region Small objects

        // Properties //

        [NotNull, ItemNotNull]
        protected static PropertyInfo[] EmptyPropertyInfo = { };

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
        [NotNull, ItemNotNull]
        protected static FieldInfo[] EmptyFieldInfo = { };

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
        protected static PropertyInfo PublicValueTypePublicGetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicValueTypePublicSetPropertyPropertyInfo =
            typeof(PublicValueTypeTestClass).GetProperty(nameof(PublicValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo PublicValueTypePublicFieldFieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicValueTypePublicField2FieldsInfo =
            typeof(PublicValueTypeTestClass).GetField(nameof(PublicValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");



        // PublicReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo PublicReferenceTypePublicSetPropertyPropertyInfo =
            typeof(PublicReferenceTypeTestClass).GetProperty(nameof(PublicReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo PublicReferenceTypePublicFieldFieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo PublicReferenceTypePublicField2FieldsInfo =
            typeof(PublicReferenceTypeTestClass).GetField(nameof(PublicReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #region Internal TestClass

        // InternalValueTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalValueTypePublicSetPropertyPropertyInfo =
            typeof(InternalValueTypeTestClass).GetProperty(nameof(InternalValueTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo InternalValueTypePublicFieldFieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalValueTypePublicField2FieldsInfo =
            typeof(InternalValueTypeTestClass).GetField(nameof(InternalValueTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");



        // InternalReferenceTypeTestClass

        // Properties //

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicGetPrivateSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertyGetPrivateSet)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalReferenceTypePublicSetPropertyPropertyInfo =
            typeof(InternalReferenceTypeTestClass).GetProperty(nameof(InternalReferenceTypeTestClass.PublicPropertySet)) ?? throw new AssertionException("Cannot find property.");

        // Fields //

        [NotNull]
        protected static FieldInfo InternalReferenceTypePublicFieldFieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField)) ?? throw new AssertionException("Cannot find field.");

        [NotNull]
        protected static FieldInfo InternalReferenceTypePublicField2FieldsInfo =
            typeof(InternalReferenceTypeTestClass).GetField(nameof(InternalReferenceTypeTestClass._publicField2)) ?? throw new AssertionException("Cannot find field.");

        #endregion

        #endregion

        #region Nested class

        [NotNull]
        protected static PropertyInfo PublicNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.PublicNestedClass).GetProperty(nameof(PublicTestClass.PublicNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        [NotNull]
        protected static PropertyInfo InternalNestedPublicGetSetPropertyPropertyInfo =
            typeof(PublicTestClass.InternalNestedClass).GetProperty(nameof(PublicTestClass.InternalNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        #endregion

        #endregion
    }
}