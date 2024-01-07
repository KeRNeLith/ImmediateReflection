using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Helpers to make tests related to constructors.
    /// </summary>
    internal static class ConstructorTestHelpers
    {
        #region Test classes

        public struct ParameterConstructorStruct
        {
            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            public ParameterConstructorStruct(int value)
            {
            }
        }

        public class DefaultConstructor
        {
            private readonly bool _baseValue;   // This serve as a check that this default constructor
                                                // has been called when instantiating a sub class.

            public DefaultConstructor()
            {
                _baseValue = true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as DefaultConstructor);
            }

            private bool Equals(DefaultConstructor other)
            {
                if (other is null)
                    return false;
                return _baseValue == other._baseValue;
            }

            public override int GetHashCode()
            {
                return _baseValue.GetHashCode();
            }
        }

        public abstract class AbstractDefaultConstructor
        {
            private readonly bool _baseValue;   // This serve as a check that this default constructor
                                                // has been called when instantiating a sub class.

            public AbstractDefaultConstructor()
            {
                _baseValue = true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as AbstractDefaultConstructor);
            }

            private bool Equals(AbstractDefaultConstructor other)
            {
                if (other is null)
                    return false;
                return _baseValue == other._baseValue;
            }

            public override int GetHashCode()
            {
                return _baseValue.GetHashCode();
            }
        }

        public abstract class AbstractNoConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public AbstractNoConstructor(int value)
            {
            }
        }

        public class NoDefaultConstructor
        {
            private readonly bool _baseValue;   // This serve as a check that this default constructor
                                                // has been called when instantiating a sub class.

            // ReSharper disable once UnusedParameter.Local
            public NoDefaultConstructor(int value)
            {
                _baseValue = true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as NoDefaultConstructor);
            }

            private bool Equals(NoDefaultConstructor other)
            {
                if (other is null)
                    return false;
                return _baseValue == other._baseValue;
            }

            public override int GetHashCode()
            {
                return _baseValue.GetHashCode();
            }
        }

        public class MultiParametersConstructor
        {
            // ReSharper disable UnusedParameter.Local
            public MultiParametersConstructor(int value, float value2)
            // ReSharper restore UnusedParameter.Local
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as MultiParametersConstructor);
            }

            private static bool Equals(MultiParametersConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class NotAccessibleDefaultConstructor
        {
            private NotAccessibleDefaultConstructor()
            {
            }
        }

        public class NotAccessibleConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            private NotAccessibleConstructor(int value)
            {
            }
        }

        // ReSharper disable once UnusedTypeParameter
        public struct TemplateStruct<TTemplate>
        {
        }

        // ReSharper disable once UnusedTypeParameter
        public class TemplateDefaultConstructor<TTemplate>
        {
            public override bool Equals(object obj)
            {
                return Equals(obj as TemplateDefaultConstructor<TTemplate>);
            }

            private static bool Equals(TemplateDefaultConstructor<TTemplate> other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        // ReSharper disable once UnusedTypeParameter
        public class TemplateNoDefaultConstructor<TTemplate>
        {
            // ReSharper disable once UnusedParameter.Local
            public TemplateNoDefaultConstructor(int value)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TemplateNoDefaultConstructor<TTemplate>);
            }

            private static bool Equals(TemplateNoDefaultConstructor<TTemplate> other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class DefaultConstructorThrows
        {
            // ReSharper disable once UnusedMember.Local
            public DefaultConstructorThrows()
            {
                throw new InvalidOperationException("Constructor throws.");
            }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedParameter.Local
            public DefaultConstructorThrows(int value)
            {
                throw new InvalidOperationException("Constructor throws (int).");
            }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable UnusedParameter.Local
            public DefaultConstructorThrows(int value, float value2)
            // ReSharper restore UnusedParameter.Local
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as DefaultConstructorThrows);
            }

            private static bool Equals(DefaultConstructorThrows other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class NotDefaultConstructorThrows
        {
            // ReSharper disable once UnusedParameter.Local
            public NotDefaultConstructorThrows(int value)
            {
                throw new InvalidOperationException("Constructor throws.");
            }
        }

        public class MultipleConstructors
        {
            // ReSharper disable UnusedMember.Local
            public MultipleConstructors()
            {
            }

            // ReSharper disable UnusedParameter.Local
            public MultipleConstructors(int value)
            {
            }

            public MultipleConstructors(int value, float value2)
            {
            }

            // ReSharper restore UnusedParameter.Local
            // ReSharper restore UnusedMember.Local
            public override bool Equals(object obj)
            {
                return Equals(obj as MultipleConstructors);
            }

            private static bool Equals(MultipleConstructors other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class ParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public ParamsOnlyConstructor(params object[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsOnlyConstructor);
            }

            private static bool Equals(ParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class ParamsConstructor
        {
            // ReSharper disable UnusedParameter.Local
            public ParamsConstructor(int value, params object[] args)
            // ReSharper restore UnusedParameter.Local
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsConstructor);
            }

            private static bool Equals(ParamsConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class AmbiguousParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            public AmbiguousParamsOnlyConstructor(params object[] args)
            {
            }

            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            public AmbiguousParamsOnlyConstructor(params int[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsOnlyConstructor);
            }

            private static bool Equals(ParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class IntParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public IntParamsOnlyConstructor(params int[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as IntParamsOnlyConstructor);
            }

            private static bool Equals(IntParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class NullableIntParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public NullableIntParamsOnlyConstructor(params int?[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as NullableIntParamsOnlyConstructor);
            }

            private static bool Equals(NullableIntParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class DefaultInheritedDefaultConstructor : DefaultConstructor
        {
        }

        public class DefaultInheritedNoDefaultConstructor : NoDefaultConstructor
        {
            public DefaultInheritedNoDefaultConstructor()
                : base(777)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as DefaultInheritedNoDefaultConstructor);
            }

            private static bool Equals(DefaultInheritedNoDefaultConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public class DefaultInheritedFromAbstractClass : AbstractDefaultConstructor
        {
        }

        public class NoDefaultInheritedDefaultConstructor : DefaultConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public NoDefaultInheritedDefaultConstructor(int value)
            {
            }
        }

        public class NoDefaultInheritedNoDefaultConstructor : NoDefaultConstructor
        {
            public NoDefaultInheritedNoDefaultConstructor(int value)
                : base(value)
            {
            }
        }

        public class NoDefaultInheritedFromAbstractClass : AbstractDefaultConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public NoDefaultInheritedFromAbstractClass(int value)
            {
            }
        }

        public class NoCopyConstructorClass
        {
        }

        public class CopyConstructorClass
        {
            private readonly int _value;

            public CopyConstructorClass(int value)
            {
                _value = value;
            }

            public CopyConstructorClass(CopyConstructorClass other)
            {
                _value = other._value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as CopyConstructorClass);
            }

            private bool Equals(CopyConstructorClass other)
            {
                if (other is null)
                    return false;
                return _value == other._value;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        public class NoCopyInheritedCopyConstructorClass : CopyConstructorClass
        {
            public NoCopyInheritedCopyConstructorClass(int value)
                : base(value)
            {
            }
        }

        public class CopyInheritedCopyConstructorClass : CopyConstructorClass
        {
            public CopyInheritedCopyConstructorClass(int value)
                : base(value)
            {
            }

            public CopyInheritedCopyConstructorClass(CopyInheritedCopyConstructorClass other)
                : base(other)
            {
            }
        }

        public class BaseCopyInheritedCopyConstructorClass : CopyConstructorClass
        {
            public BaseCopyInheritedCopyConstructorClass(int value)
                : base(value)
            {
            }

            public BaseCopyInheritedCopyConstructorClass(CopyConstructorClass other)
                : base(other)
            {
            }
        }

        public class SpecializedCopyConstructorClass
        {
            private readonly double _value;

            public SpecializedCopyConstructorClass(double value)
            {
                _value = value;
            }

            public SpecializedCopyConstructorClass(InheritedSpecializedCopyConstructorClass other)
            {
                _value = other._value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as SpecializedCopyConstructorClass);
            }

            private bool Equals(SpecializedCopyConstructorClass other)
            {
                if (other is null)
                    return false;
                return Math.Abs(_value - other._value) < double.Epsilon;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        public class InheritedSpecializedCopyConstructorClass : SpecializedCopyConstructorClass
        {
            public InheritedSpecializedCopyConstructorClass(double value)
                : base(value)
            {
            }
        }

        public class MultipleCopyConstructorClass
        {
            private readonly double _value;

            public MultipleCopyConstructorClass(double value)
            {
                _value = value;
            }

            public MultipleCopyConstructorClass(MultipleCopyConstructorClass other)
            {
                _value = other._value;
            }

            // ReSharper disable once UnusedParameter.Local
            public MultipleCopyConstructorClass(InheritedMultipleCopyConstructorClass other)
            {
                _value = -1;    // Used during tests to assert that this constructor
                                // is not called when copying a MultipleCopyConstructorClass
                                // with other instance being an InheritedMultipleCopyConstructorClass
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as MultipleCopyConstructorClass);
            }

            private bool Equals(MultipleCopyConstructorClass other)
            {
                if (other is null)
                    return false;
                return Math.Abs(_value - other._value) < double.Epsilon;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }
        }

        public class InheritedMultipleCopyConstructorClass : MultipleCopyConstructorClass
        {
            public InheritedMultipleCopyConstructorClass(double value)
                : base(value)
            {
            }
        }

        public class TemplateCopyConstructor<TTemplate>
        {
            private readonly TTemplate _value;

            public TemplateCopyConstructor(TTemplate value)
            {
                _value = value;
            }

            public TemplateCopyConstructor(TemplateCopyConstructor<TTemplate> other)
            {
                _value = other._value;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TemplateCopyConstructor<TTemplate>);
            }

            private bool Equals(TemplateCopyConstructor<TTemplate> other)
            {
                if (other is null)
                    return false;
                return Equals(_value, other._value);
            }

            public override int GetHashCode()
            {
                return _value?.GetHashCode() ?? 0;
            }
        }

        public abstract class AbstractCopyConstructor
        {
            public AbstractCopyConstructor()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            public AbstractCopyConstructor(AbstractCopyConstructor other)
            {
            }
        }

        public class NotAccessibleCopyConstructor
        {
            public NotAccessibleCopyConstructor()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            private NotAccessibleCopyConstructor(NotAccessibleCopyConstructor other)
            {
            }
        }

        public class CopyConstructorThrows
        {
            public CopyConstructorThrows()
            {
            }

            // ReSharper disable once UnusedParameter.Local
            public CopyConstructorThrows(CopyConstructorThrows other)
            {
                throw new InvalidOperationException("Copy Constructor throws.");
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as CopyConstructorThrows);
            }

            private static bool Equals(CopyConstructorThrows other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        public static class StaticClass
        {
        }

        #endregion

        #region New/TryNew

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateHasDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TestStruct)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TestEnum)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(MultipleConstructors)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TemplateStruct<double>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor)) { ExpectedResult = true };  // Considered as default
                yield return new TestCaseData(typeof(DefaultConstructorThrows)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(List<int>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(Dictionary<int, string>)) { ExpectedResult = true };

                yield return new TestCaseData(typeof(string)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(Type)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NoDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(AbstractDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(StaticClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateStruct<>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(ParamsConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(AmbiguousParamsOnlyConstructor)) { ExpectedResult = false };
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType()) { ExpectedResult = false };
                yield return new TestCaseData(typeof(IList<int>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(IDictionary<int, string>)) { ExpectedResult = false };
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int));
                yield return new TestCaseData(typeof(TestStruct));
                yield return new TestCaseData(typeof(TestEnum));
                yield return new TestCaseData(typeof(DefaultConstructor));
                yield return new TestCaseData(typeof(MultipleConstructors));
                yield return new TestCaseData(typeof(TemplateStruct<double>));
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>));
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor));
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor));
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass));
                yield return new TestCaseData(typeof(List<int>));
                yield return new TestCaseData(typeof(Dictionary<int, string>));
            }
        }

        public static void NewParameterLess([NotNull] Type type, [NotNull, InstantHandle] Func<object> ctor)
        {
            object instance = ctor();
            Assert.IsNotNull(instance);
            Assert.AreEqual(Activator.CreateInstance(type), instance);
        }

        public static void NewParamsOnly([NotNull, InstantHandle] Func<object> ctor, [NotNull, InstantHandle] Func<object> expectedObject)
        {
            object instance = ctor();
            Assert.IsNotNull(instance);
            Assert.AreEqual(expectedObject(), instance);
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateDefaultConstructorNoThrowTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), false);
                yield return new TestCaseData(typeof(TestStruct), false);
                yield return new TestCaseData(typeof(TestEnum), false);
                yield return new TestCaseData(typeof(DefaultConstructor), false);
                yield return new TestCaseData(typeof(MultipleConstructors), false);
                yield return new TestCaseData(typeof(TemplateStruct<double>), false);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), false);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), false);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), false);
                yield return new TestCaseData(typeof(List<int>), false);
                yield return new TestCaseData(typeof(Dictionary<int, string>), false);

                yield return new TestCaseData(typeof(string), true);
                yield return new TestCaseData(typeof(Type), true);
                yield return new TestCaseData(typeof(NoDefaultConstructor), true);
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true);
                yield return new TestCaseData(typeof(IList<int>), true);
                yield return new TestCaseData(typeof(IDictionary<int, string>), true);
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true);
                yield return new TestCaseData(typeof(StaticClass), true);
                yield return new TestCaseData(typeof(TemplateStruct<>), true);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true);
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), true);
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), true);
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), true);
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true);
            }
        }

        public delegate bool TryCtor(out object instance, out Exception exception);

        public static void TryNewParameterLess([NotNull] Type type, bool expectFail, [NotNull, InstantHandle] TryCtor tryCtor)
        {
            Assert.AreEqual(!expectFail, tryCtor(out object instance, out Exception ex));
            if (expectFail)
            {
                Assert.IsNull(instance);
                Assert.IsNotNull(ex);
            }
            else
            {
                Assert.IsNotNull(instance);
                Assert.AreEqual(Activator.CreateInstance(type), instance);
            }
        }

        public static void TryNewParameterLess([NotNull, InstantHandle] TryCtor tryCtor, [NotNull, InstantHandle] Func<object> expectedObject)
        {
            Assert.IsTrue(tryCtor(out object instance, out Exception _));
            Assert.IsNotNull(instance);
            Assert.AreEqual(expectedObject(), instance);
        }

        #endregion

        #region New(params)/TryNew(params)

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorNullParamsTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), null);
                yield return new TestCaseData(typeof(TestStruct), null);
                yield return new TestCaseData(typeof(TestEnum), null);
                yield return new TestCaseData(typeof(DefaultConstructor), null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), null);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), null);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), null);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), null);
                yield return new TestCaseData(typeof(List<int>), null);
                yield return new TestCaseData(typeof(Dictionary<int, string>), null);
            }
        }

        [NotNull, ItemNotNull]
        private static readonly object[] NoArgs = { };

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorNotNullParamsTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), NoArgs);
                yield return new TestCaseData(typeof(TestStruct), NoArgs);
                yield return new TestCaseData(typeof(TestEnum), NoArgs);
                yield return new TestCaseData(typeof(DefaultConstructor), NoArgs);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), NoArgs);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), NoArgs);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), NoArgs);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), NoArgs);
                yield return new TestCaseData(typeof(List<int>), NoArgs);
                yield return new TestCaseData(typeof(Dictionary<int, string>), NoArgs);

                yield return new TestCaseData(typeof(ParameterConstructorStruct), new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(MultiParametersConstructor), new object[] { 12, 42.5f });
                yield return new TestCaseData(typeof(MultipleConstructors), new object[] { 12 });
                yield return new TestCaseData(typeof(MultipleConstructors), new object[] { 12, 42.5f });
                yield return new TestCaseData(typeof(TemplateNoDefaultConstructor<int>), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), new object[] { 12, 15.4f });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), new object[] { 12, 15 });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), new object[] { 12, null, 25 });
                yield return new TestCaseData(typeof(ParamsConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsConstructor), new object[] { 12, 15.4f });
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), new object[] { 42 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), new object[] { 25 });
                yield return new TestCaseData(typeof(int[]), new object[] { 5 });
                yield return new TestCaseData(typeof(List<int>), new object[] { 2 });
                yield return new TestCaseData(typeof(List<int>), new object[] { Enumerable.Range(0, 5) });
                yield return new TestCaseData(typeof(Dictionary<int, string>), new object[] { 3 });
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                foreach (TestCaseData testCase in CreateNotDefaultConstructorNullParamsTestCases)
                    yield return testCase;
                foreach (TestCaseData testCase in CreateNotDefaultConstructorNotNullParamsTestCases)
                    yield return testCase;
            }
        }

        public delegate object ArgsCtor(params object[] args);

        public static void NewWithParameters([NotNull] Type type, [NotNull, InstantHandle] ArgsCtor ctor, [CanBeNull, ItemCanBeNull] params object[] args)
        {
            object instance = ctor(args);
            Assert.IsNotNull(instance);
            Assert.AreEqual(Activator.CreateInstance(type, args), instance);
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorNoThrowNullParamsTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), false, null);
                yield return new TestCaseData(typeof(TestStruct), false, null);
                yield return new TestCaseData(typeof(TestEnum), false, null);
                yield return new TestCaseData(typeof(DefaultConstructor), false, null);
                yield return new TestCaseData(typeof(MultipleConstructors), false, null);
                yield return new TestCaseData(typeof(TemplateStruct<double>), false, null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false, null);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), false, null);
                yield return new TestCaseData(typeof(List<int>), false, null);
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, null);

                yield return new TestCaseData(typeof(string), true, null);
                yield return new TestCaseData(typeof(Type), true, null);
                yield return new TestCaseData(typeof(NoDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(StaticClass), true, null);
                yield return new TestCaseData(typeof(TemplateStruct<>), true, null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true, null);
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true, null);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, null);
                yield return new TestCaseData(typeof(int[]), true, null);
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorNoThrowNotNullParamsTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), false, NoArgs);
                yield return new TestCaseData(typeof(TestStruct), false, NoArgs);
                yield return new TestCaseData(typeof(TestEnum), false, NoArgs);
                yield return new TestCaseData(typeof(DefaultConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(MultipleConstructors), false, NoArgs);
                yield return new TestCaseData(typeof(MultipleConstructors), false, new object[] { 12, 12.5f });
                yield return new TestCaseData(typeof(TemplateStruct<double>), false, NoArgs);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false, NoArgs);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), false, new object[] { 45, 51.0f });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, new object[] { 12, 45.5f });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, new object[] { 45, 54 });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, new object[] { 12, null, 25 });
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), false, NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), false, NoArgs);
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), false, new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), false, new object[] { 42 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), false, new object[] { 25 });
                yield return new TestCaseData(typeof(int[]), false, new object[] { 5 });
                yield return new TestCaseData(typeof(List<int>), false, NoArgs);
                yield return new TestCaseData(typeof(List<int>), false, new object[] { 2 });
                yield return new TestCaseData(typeof(List<int>), false, new object[] { Enumerable.Range(0, 5) });
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, NoArgs);
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, new object[] { 3 });

                yield return new TestCaseData(typeof(int), true, new object[] { 12 });
                yield return new TestCaseData(typeof(TestStruct), true, new object[] { 12 });
                yield return new TestCaseData(typeof(TestEnum), true, new object[] { 1 });
                yield return new TestCaseData(typeof(string), true, NoArgs);
                yield return new TestCaseData(typeof(string), true, new object[] { "str" });
                yield return new TestCaseData(typeof(Type), true, NoArgs);
                yield return new TestCaseData(typeof(Type), true, new object[] { typeof(int) });
                yield return new TestCaseData(typeof(DefaultConstructor), true, new object[] { 12 });
                yield return new TestCaseData(typeof(MultipleConstructors), true, new object[] { 12.5f, 12 });
                yield return new TestCaseData(typeof(TemplateStruct<double>), true, new object[] { 25 });
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), true, new object[] { 25 });
                yield return new TestCaseData(typeof(NoDefaultConstructor), true, NoArgs);
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true, NoArgs);
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true, NoArgs);
                yield return new TestCaseData(typeof(StaticClass), true, NoArgs);
                yield return new TestCaseData(typeof(TemplateStruct<>), true, NoArgs);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true, NoArgs);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), true, new object[] { 45 });
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), true, new object[] { 51 });
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), true, new object[] { 72 });
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), true, new object[] { 45, 35 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), true, new object[] { 51, 25 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), true, new object[] { 72, 15 });
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true, NoArgs);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, NoArgs);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, new object[] { 12 });
                yield return new TestCaseData(typeof(int[]), true, NoArgs);
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateNotDefaultConstructorNoThrowTestCases
        {
            [UsedImplicitly]
            get
            {
                foreach (TestCaseData testCase in CreateNotDefaultConstructorNoThrowNullParamsTestCases)
                    yield return testCase;
                foreach (TestCaseData testCase in CreateNotDefaultConstructorNoThrowNotNullParamsTestCases)
                    yield return testCase;
            }
        }

        public delegate bool TryArgsCtor(out object instance, out Exception exception, params object[] args);

        public static void TryNewWithParameters([NotNull] Type type, bool expectFail, [NotNull, InstantHandle] TryArgsCtor tryCtor, [CanBeNull, ItemCanBeNull] params object[] args)
        {
            Assert.AreEqual(!expectFail, tryCtor(out object instance, out Exception ex, args));
            if (expectFail)
            {
                Assert.IsNull(instance);
                Assert.IsNotNull(ex);
            }
            else
            {
                Assert.IsNotNull(instance);
                Assert.AreEqual(Activator.CreateInstance(type, args), instance);
            }
        }

        #endregion

        #region Copy/TryCopy

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateHasCopyConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                // Not has a real copy constructor but it's more convenient
                yield return new TestCaseData(typeof(int)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TestStruct)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TestEnum)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(string)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(Type)) { ExpectedResult = true };

                // Normal copy constructor
                yield return new TestCaseData(typeof(CopyConstructorClass)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(CopyInheritedCopyConstructorClass)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(MultipleCopyConstructorClass)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TemplateCopyConstructor<double>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(CopyConstructorThrows)) { ExpectedResult = true };

                // No copy constructor
                yield return new TestCaseData(typeof(NoCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NoCopyInheritedCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(BaseCopyInheritedCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(SpecializedCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(InheritedSpecializedCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(InheritedMultipleCopyConstructorClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NotAccessibleCopyConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(AbstractCopyConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(StaticClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateStruct<>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateCopyConstructor<>)) { ExpectedResult = false };
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(CopyConstructorClass).GetType()) { ExpectedResult = false };
                yield return new TestCaseData(typeof(IList<int>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(IDictionary<int, string>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(int[])) { ExpectedResult = false };
            }
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateCopyConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(CopyConstructorClass), null);
                yield return new TestCaseData(typeof(NoCopyConstructorClass), null);
                yield return new TestCaseData(typeof(AbstractCopyConstructor), null);

                yield return new TestCaseData(typeof(int), 25);
                yield return new TestCaseData(typeof(TestStruct), new TestStruct { TestValue = 12 });
                yield return new TestCaseData(typeof(TestEnum), TestEnum.EnumValue2);
                yield return new TestCaseData(typeof(string), "string test value");
                yield return new TestCaseData(typeof(Type), typeof(int));
                yield return new TestCaseData(typeof(CopyConstructorClass), new CopyConstructorClass(42));
                yield return new TestCaseData(typeof(CopyInheritedCopyConstructorClass), new CopyInheritedCopyConstructorClass(28));
                yield return new TestCaseData(typeof(MultipleCopyConstructorClass), new MultipleCopyConstructorClass(56));
                yield return new TestCaseData(typeof(TemplateCopyConstructor<char>), new TemplateCopyConstructor<char>('A'));
            }
        }

        private static void AssertEqualInstances([NotNull] Type type, [CanBeNull] object other, [CanBeNull] object instance)
        {
            if (other is null)
            {
                Assert.IsNull(instance);
            }
            else if (type.IsValueType || type == typeof(string) || type == typeof(Type))
            {
                Assert.AreEqual(other, instance);
            }
            else
            {
                Assert.AreEqual(Activator.CreateInstance(type, other), instance);
                Assert.AreNotSame(other, instance);
            }
        }

        public static void Copy([NotNull] Type type, [CanBeNull] object other, [NotNull, InstantHandle] Func<object, object> ctor)
        {
            object instance = ctor(other);
            AssertEqualInstances(type, other, instance);
        }

        [NotNull, ItemNotNull]
        public static IEnumerable<TestCaseData> CreateCopyConstructorNoThrowTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(CopyConstructorClass), null, false);
                yield return new TestCaseData(typeof(NoCopyConstructorClass), null, false);
                yield return new TestCaseData(typeof(AbstractCopyConstructor), null, false);

                yield return new TestCaseData(typeof(int), 1, false);
                yield return new TestCaseData(typeof(TestStruct), new TestStruct { TestValue = 2 }, false);
                yield return new TestCaseData(typeof(TestEnum), TestEnum.EnumValue2, false);
                yield return new TestCaseData(typeof(string), "string test value", false);
                yield return new TestCaseData(typeof(Type), typeof(int), false);
                yield return new TestCaseData(typeof(CopyConstructorClass), new CopyConstructorClass(3), false);
                yield return new TestCaseData(typeof(CopyInheritedCopyConstructorClass), new CopyInheritedCopyConstructorClass(4), false);
                yield return new TestCaseData(typeof(MultipleCopyConstructorClass), new MultipleCopyConstructorClass(5), false);
                yield return new TestCaseData(typeof(TemplateCopyConstructor<char>), new TemplateCopyConstructor<char>('B'), false);

                yield return new TestCaseData(typeof(NoCopyConstructorClass), new NoCopyConstructorClass(), true);
                yield return new TestCaseData(typeof(NoCopyInheritedCopyConstructorClass), new NoCopyInheritedCopyConstructorClass(1), true);
                yield return new TestCaseData(typeof(NoCopyInheritedCopyConstructorClass), new CopyConstructorClass(2), true);
                yield return new TestCaseData(typeof(BaseCopyInheritedCopyConstructorClass), new BaseCopyInheritedCopyConstructorClass(3), true);
                yield return new TestCaseData(typeof(BaseCopyInheritedCopyConstructorClass), new CopyConstructorClass(4), true);   // Constructor exists but is not considered as copy constructor
                yield return new TestCaseData(typeof(SpecializedCopyConstructorClass), new SpecializedCopyConstructorClass(5), true);
                yield return new TestCaseData(typeof(SpecializedCopyConstructorClass), new InheritedSpecializedCopyConstructorClass(6), true); // Constructor exists but is not considered as copy constructor
                yield return new TestCaseData(typeof(MultipleCopyConstructorClass), new InheritedMultipleCopyConstructorClass(7), true);       // Constructor exists but is not considered as copy constructor
                yield return new TestCaseData(typeof(NotAccessibleCopyConstructor), new NotAccessibleCopyConstructor(), true);
                yield return new TestCaseData(typeof(IList<int>), new List<int>(), true);
                yield return new TestCaseData(typeof(IDictionary<int, string>), new Dictionary<int, string>(), true);
                yield return new TestCaseData(typeof(int[]), new int[0], true);
                yield return new TestCaseData(typeof(AbstractCopyConstructor), new CopyConstructorClass(12), true);
                yield return new TestCaseData(typeof(StaticClass), new CopyConstructorClass(12), true);
                yield return new TestCaseData(typeof(TemplateStruct<>), new CopyConstructorClass(12), true);
                yield return new TestCaseData(typeof(TemplateCopyConstructor<>), new CopyConstructorClass(12), true);
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(CopyConstructorClass).GetType(), new CopyConstructorClass(12), true);
                yield return new TestCaseData(typeof(CopyConstructorThrows), new CopyConstructorThrows(), true);
            }
        }

        public delegate bool TryCopyCtor([CanBeNull] object other, [CanBeNull] out object instance, [CanBeNull] out Exception exception);

        public static void TryCopy([NotNull] Type type, [CanBeNull] object other, bool expectFail, [NotNull, InstantHandle] TryCopyCtor tryCtor)
        {
            Assert.AreEqual(!expectFail, tryCtor(other, out object instance, out Exception ex));
            if (expectFail)
            {
                Assert.IsNull(instance);
                Assert.IsNotNull(ex);
            }
            else
            {
                AssertEqualInstances(type, other, instance);
            }
        }

        #endregion
    }
}