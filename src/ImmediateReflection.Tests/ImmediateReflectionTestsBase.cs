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
    }
}