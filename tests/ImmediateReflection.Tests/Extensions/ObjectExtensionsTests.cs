using System;
using System.Collections.Generic;
using NUnit.Framework;
using static ImmediateReflection.Tests.ConstructorTestHelpers;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ObjectExtensions"/>.
/// </summary>
[TestFixture]
internal sealed class ObjectExtensionsTests : ImmediateReflectionTestsBase
{
    private static IEnumerable<TestCaseData> CreateObjectHasCopyConstructorTestCases()
    {
        // Not has a real copy constructor but it's more convenient
        yield return new TestCaseData((int?)42) { ExpectedResult = true };
        yield return new TestCaseData((TestStruct?)new TestStruct { TestValue = 12 }) { ExpectedResult = true };
        yield return new TestCaseData((TestEnum?)TestEnum.EnumValue2) { ExpectedResult = true };
        yield return new TestCaseData(42) { ExpectedResult = true };
        yield return new TestCaseData(new TestStruct { TestValue = 12 }) { ExpectedResult = true };
        yield return new TestCaseData(TestEnum.EnumValue2) { ExpectedResult = true };
        yield return new TestCaseData("string value") { ExpectedResult = true };
        yield return new TestCaseData(typeof(double)) { ExpectedResult = true };

        // Normal copy constructor
        yield return new TestCaseData(new CopyConstructorClass(25)) { ExpectedResult = true };
        yield return new TestCaseData(new CopyInheritedCopyConstructorClass(66)) { ExpectedResult = true };
        yield return new TestCaseData(new MultipleCopyConstructorClass(33)) { ExpectedResult = true };
        yield return new TestCaseData(new TemplateCopyConstructor<double>(7.5)) { ExpectedResult = true };

        // Has copy constructor but not usable
        yield return new TestCaseData(typeof(AbstractCopyConstructor)) { ExpectedResult = true };

        // No copy constructor
        yield return new TestCaseData(new NoCopyConstructorClass()) { ExpectedResult = false };
        yield return new TestCaseData(new NoCopyInheritedCopyConstructorClass(1)) { ExpectedResult = false };
        yield return new TestCaseData(new BaseCopyInheritedCopyConstructorClass(2)) { ExpectedResult = false };
        yield return new TestCaseData(new SpecializedCopyConstructorClass(3)) { ExpectedResult = false };
        yield return new TestCaseData(new InheritedSpecializedCopyConstructorClass(4)) { ExpectedResult = false };
        yield return new TestCaseData(new NotAccessibleCopyConstructor()) { ExpectedResult = false };
        yield return new TestCaseData(new List<int> { 1, 2 }) { ExpectedResult = false };
        yield return new TestCaseData(new Dictionary<int, string> { [1] = "1", [2] = "2" }) { ExpectedResult = false };
        yield return new TestCaseData(new[] { 1, 2 }) { ExpectedResult = false };
    }

    [TestCaseSource(nameof(CreateObjectHasCopyConstructorTestCases))]
    public static bool HasCopyConstructor(object instance)
    {
        return ObjectExtensions.HasCopyConstructor(instance);
    }

    [Test]
    public static void HasCopyConstructor_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ObjectExtensions.HasCopyConstructor<object>(null!));
    }

    #region Copy/TryCopy

    // T is object (untyped reference)
    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateCopyConstructorTestCases))]
    public static void Copy_AsObject(Type type, object? other, ICopyCase _)
    {
        Copy(
            type,
            other,
            ObjectExtensions.Copy);
    }

    // T is the type of the test case
    [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateCopyConstructorTestCases))]
    public static void Copy_AsCaseType(Type type, object? other, ICopyCase copyCase)
    {
        Copy(
            type,
            other,
            _ => copyCase.CopyFromObjectExtensions());
    }

    [Test]
    public static void Copy_Throws()
    {
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new NoCopyConstructorClass()));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new NotAccessibleCopyConstructor()));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new List<int>()));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new Dictionary<int, string>()));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new NoCopyInheritedCopyConstructorClass(1)));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new BaseCopyInheritedCopyConstructorClass(3)));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new SpecializedCopyConstructorClass(5)));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new InheritedSpecializedCopyConstructorClass(6)));
        Assert.Throws<MissingMethodException>(() => ObjectExtensions.Copy(new int[0]));
    }

    private static IEnumerable<TestCaseData> CreateObjectTryCopyConstructorTestCases()
    {
        // Null instance always return null
        yield return CopyCase<int?>(null, false);
        yield return CopyCase<TestStruct?>(null, false);
        yield return CopyCase<TestEnum?>(null, false);
        yield return CopyCase<string>(null, false);
        yield return CopyCase<CopyConstructorClass>(null, false);
        yield return CopyCase<NoCopyConstructorClass>(null, false);
        yield return CopyCase<AbstractCopyConstructor>(null, false);

        // Not has a real copy constructor but it's more convenient
        yield return CopyCase<int?>(42, false);
        yield return CopyCase<TestStruct?>(new TestStruct { TestValue = 12 }, false);
        yield return CopyCase<TestEnum?>(TestEnum.EnumValue2, false);
        yield return CopyCase(42, false);
        yield return CopyCase(new TestStruct { TestValue = 12 }, false);
        yield return CopyCase(TestEnum.EnumValue2, false);
        yield return CopyCase<string>("string test", false);
        yield return CopyCase(typeof(float), false);

        // Normal copy constructor
        yield return CopyCase(new CopyConstructorClass(25), false);
        yield return CopyCase(new CopyInheritedCopyConstructorClass(66), false);
        yield return CopyCase(new MultipleCopyConstructorClass(33), false);
        yield return CopyCase(new TemplateCopyConstructor<double>(7.5), false);

        // No copy constructor
        yield return CopyCase(new NoCopyConstructorClass(), true);
        yield return CopyCase(new NoCopyInheritedCopyConstructorClass(1), true);
        yield return CopyCase(new BaseCopyInheritedCopyConstructorClass(2), true);
        yield return CopyCase(new SpecializedCopyConstructorClass(3), true);
        yield return CopyCase<SpecializedCopyConstructorClass>(new InheritedSpecializedCopyConstructorClass(4), true);
        yield return CopyCase(new NotAccessibleCopyConstructor(), true);
        yield return CopyCase(new List<int> { 1, 2 }, true);
        yield return CopyCase(new Dictionary<int, string> { [1] = "1", [2] = "2" }, true);
        yield return CopyCase(new[] { 1, 2 }, true);
    }

    // T is object (untyped reference)
    [TestCaseSource(nameof(CreateObjectTryCopyConstructorTestCases))]
    public static void TryCopy_AsObject(Type type, object? other, bool expectFail, ICopyCase _)
    {
        TryCopy(
            type,
            other,
            expectFail,
            (object? obj, out object? instance, out Exception? exception) => ObjectExtensions.TryCopy(obj, out instance, out exception));
    }

    // T is the type of the test case
    [TestCaseSource(nameof(CreateObjectTryCopyConstructorTestCases))]
    public static void TryCopy_AsCaseType(Type type, object? other, bool expectFail, ICopyCase copyCase)
    {
        TryCopy(
            type,
            other,
            expectFail,
            (object? _, out object? instance, out Exception? exception) => copyCase.TryCopyFromObjectExtensions(out instance, out exception));
    }

    #endregion
}
