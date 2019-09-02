#if SUPPORTS_CACHING
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using static ImmediateReflection.Tests.ConstructorTestHelpers;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ObjectExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class ObjectExtensionsTests : ImmediateReflectionTestsBase
    {
        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateObjectHasCopyConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                // Not has a real copy constructor but it's more convenient
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
        }

        [TestCaseSource(nameof(CreateObjectHasCopyConstructorTestCases))]
        public bool HasCopyConstructor([NotNull] object instance)
        {
            return ObjectExtensions.HasCopyConstructor(instance);
        }

        [Test]
        public void HasCopyConstructor_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => ObjectExtensions.HasCopyConstructor<object>(null));
        }

        #region Copy/TryCopy

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateCopyConstructorTestCases))]
        public void Copy([NotNull] Type type, [CanBeNull] object other)
        {
            ConstructorTestHelpers.Copy(
                type,
                other,
                ObjectExtensions.Copy);
        }

        [Test]
        public void Copy_Throws()
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

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateObjectTryCopyConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                // Null instance always return null
                yield return new TestCaseData(typeof(CopyConstructorClass), null, false);
                yield return new TestCaseData(typeof(NoCopyConstructorClass), null, false);
                yield return new TestCaseData(typeof(AbstractCopyConstructor), null, false);

                // Not has a real copy constructor but it's more convenient
                yield return new TestCaseData(typeof(int), 42, false);
                yield return new TestCaseData(typeof(TestStruct), new TestStruct { TestValue = 12 }, false);
                yield return new TestCaseData(typeof(TestEnum), TestEnum.EnumValue2, false);
                yield return new TestCaseData(typeof(string), "string test", false);
                yield return new TestCaseData(typeof(Type), typeof(float), false);

                // Normal copy constructor
                yield return new TestCaseData(typeof(CopyConstructorClass), new CopyConstructorClass(25), false);
                yield return new TestCaseData(typeof(CopyInheritedCopyConstructorClass), new CopyInheritedCopyConstructorClass(66), false);
                yield return new TestCaseData(typeof(MultipleCopyConstructorClass), new MultipleCopyConstructorClass(33), false);
                yield return new TestCaseData(typeof(TemplateCopyConstructor<double>), new TemplateCopyConstructor<double>(7.5), false);

                // No copy constructor
                yield return new TestCaseData(typeof(NoCopyConstructorClass), new NoCopyConstructorClass(), true);
                yield return new TestCaseData(typeof(NoCopyInheritedCopyConstructorClass), new NoCopyInheritedCopyConstructorClass(1), true);
                yield return new TestCaseData(typeof(BaseCopyInheritedCopyConstructorClass), new BaseCopyInheritedCopyConstructorClass(2), true);
                yield return new TestCaseData(typeof(SpecializedCopyConstructorClass), new SpecializedCopyConstructorClass(3), true);
                yield return new TestCaseData(typeof(SpecializedCopyConstructorClass), new InheritedSpecializedCopyConstructorClass(4), true);
                yield return new TestCaseData(typeof(NotAccessibleCopyConstructor), new NotAccessibleCopyConstructor(), true);
                yield return new TestCaseData(typeof(List<int>), new List<int> { 1, 2 }, true);
                yield return new TestCaseData(typeof(Dictionary<int, string>), new Dictionary<int, string> { [1] = "1", [2] = "2" }, true);
                yield return new TestCaseData(typeof(int[]), new[] { 1, 2 }, true);
            }
        }

        [TestCaseSource(nameof(CreateObjectTryCopyConstructorTestCases))]
        public void TryCopy([NotNull] Type type, [CanBeNull] object other, bool expectFail)
        {
            ConstructorTestHelpers.TryCopy(
                type,
                other,
                expectFail,
                (object o, out object instance, out Exception exception) => ObjectExtensions.TryCopy(o, out instance, out exception));
        }

        #endregion
    }
}
#endif