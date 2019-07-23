#if SUPPORTS_CACHING
using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;
using static ImmediateReflection.Tests.ConstructorTestHelpers;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="TypeExtensions"/>.
    /// </summary>
    [TestFixture]
    internal class TypeExtensionsTests : ImmediateReflectionTestsBase
    {
        #region New/TryNew

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorTestCases))]
        public void NewParameterLess([NotNull] Type type)
        {
            ConstructorTestHelpers.NewParameterLess(
                type,
                () => TypeExtensions.New(type));
        }

        [Test]
        public void NewParamsOnly()
        {
            ConstructorTestHelpers.NewParamsOnly(
                () => TypeExtensions.New(typeof(ParamsOnlyConstructor)),
                () => new ParamsOnlyConstructor());

            ConstructorTestHelpers.NewParamsOnly(
                () => TypeExtensions.New(typeof(IntParamsOnlyConstructor)),
                () => new IntParamsOnlyConstructor());

            ConstructorTestHelpers.NewParamsOnly(
                () => TypeExtensions.New(typeof(NullableIntParamsOnlyConstructor)),
                () => new NullableIntParamsOnlyConstructor());
        }

        [Test]
        public void NewParameterLess_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.New(null));

            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NoDefaultConstructor)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NotAccessibleDefaultConstructor)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(IList<int>)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(IDictionary<int, string>)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(AbstractDefaultConstructor)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(StaticClass)));
            Assert.Throws<ArgumentException>(() => TypeExtensions.New(typeof(TemplateStruct<>)));
            Assert.Throws<ArgumentException>(() => TypeExtensions.New(typeof(TemplateDefaultConstructor<>)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(ParamsConstructor)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NoDefaultInheritedDefaultConstructor)));
            Assert.Throws<AmbiguousMatchException>(() => TypeExtensions.New(typeof(AmbiguousParamsOnlyConstructor)));
            // ReSharper disable once PossibleMistakenCallToGetType.2
            Assert.Throws<ArgumentException>(() => TypeExtensions.New(typeof(DefaultConstructor).GetType()));
            Assert.Throws(Is.InstanceOf<Exception>(), () => TypeExtensions.New(typeof(DefaultConstructorThrows)));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(int[])));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorNoThrowTestCases))]
        public void TryNewParameterLess([NotNull] Type type, bool expectFail)
        {
            ConstructorTestHelpers.TryNewParameterLess(
                type,
                expectFail,
                (out object instance, out Exception exception) => TypeExtensions.TryNew(type, out instance, out exception));
        }

        [Test]
        public void TryNewParameterLess()
        {
            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) => TypeExtensions.TryNew(typeof(ParamsOnlyConstructor), out instance, out exception),
                () => new ParamsOnlyConstructor());

            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) => TypeExtensions.TryNew(typeof(IntParamsOnlyConstructor), out instance, out exception),
                () => new IntParamsOnlyConstructor());

            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) => TypeExtensions.TryNew(typeof(NullableIntParamsOnlyConstructor), out instance, out exception),
                () => new NullableIntParamsOnlyConstructor());
        }

        [Test]
        public void TryNewParameterLess_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.TryNew(null, out _, out _));
        }

        #endregion

        #region New(params)/TryNew(params)

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateNotDefaultConstructorNotNullParamsTestCases))]
        public void NewWithParameters([NotNull] Type type, [CanBeNull, ItemCanBeNull] params object[] arguments)
        {
            ConstructorTestHelpers.NewWithParameters(
                type,
                args => TypeExtensions.New(type, args),
                arguments);
        }

        [Test]
        public void NewWithParameters_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.New(null, 12));
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.New(typeof(ParamsConstructor), null));
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.New(null, null));
            // ReSharper restore AssignNullToNotNullAttribute

            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NoDefaultConstructor), 12, 42));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NotAccessibleConstructor), 12));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(MultiParametersConstructor), 12f, 12));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(ParamsConstructor), 12f, 12));
            Assert.Throws<MissingMethodException>(() => TypeExtensions.New(typeof(NoDefaultInheritedDefaultConstructor), 12f));
            Assert.Throws<MemberAccessException>(() => TypeExtensions.New(typeof(AbstractNoConstructor), 12));
            Assert.Throws<ArgumentException>(() => TypeExtensions.New(typeof(TemplateNoDefaultConstructor<>), 12));
            Assert.Throws<TargetInvocationException>(() => TypeExtensions.New(typeof(NotDefaultConstructorThrows), 12));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateNotDefaultConstructorNoThrowNotNullParamsTestCases))]
        public void TryNewWithParameters([NotNull] Type type, bool expectFail, [CanBeNull, ItemCanBeNull] params object[] arguments)
        {
            ConstructorTestHelpers.TryNewWithParameters(
                type,
                expectFail,
                (out object instance, out Exception exception, object[] args) => TypeExtensions.TryNew(type, out instance, out exception, args),
                arguments);
        }

        [Test]
        public void TryNewWithParameters_Throws()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.TryNew(null, out _, out _, 12));
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.TryNew(typeof(ParamsConstructor), out _, out _, null));
            Assert.Throws<ArgumentNullException>(() => TypeExtensions.TryNew(null, out _, out _, null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion
    }
}
#endif