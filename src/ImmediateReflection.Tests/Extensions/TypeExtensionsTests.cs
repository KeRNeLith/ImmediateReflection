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
    }
}
#endif