using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using Sigil;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Property setter benchmark class.
    /// </summary>
    public class PropertySetterBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly Action<BenchmarkObject, string> SetterDelegate = (Action<BenchmarkObject, string>)
            Delegate.CreateDelegate(typeof(Action<BenchmarkObject, string>), null, PropertyInfo.GetSetMethod());

        [NotNull]
        private static readonly Delegate DynamicSetterDelegate = Delegate.CreateDelegate(
            typeof(Action<BenchmarkObject, string>), null, PropertyInfo.GetSetMethod());

        [NotNull]
        private static readonly Action<BenchmarkObject, string> SigilEmitSetter = Emit<Action<BenchmarkObject, string>>
            .NewDynamicMethod("SetProperty")
            .LoadArgument(0)
            .LoadArgument(1)
            .Call(PropertyInfo.GetSetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Action<BenchmarkObject, object> ExpressionSetter = ExpressionHelpers.CreateSetter<BenchmarkObject>(PropertyInfo);

        [NotNull]
        private const string ValueToSet = "Updated benchmark string";

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void SetDirect_Property()
        {
            BenchmarkObject.BenchmarkProperty = ValueToSet;
        }

        [Benchmark]
        public void SetDelegate_Property()
        {
            SetterDelegate(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetDynamicDelegate_Property()
        {
            DynamicSetterDelegate.DynamicInvoke(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetPropertyInfo_Property()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            PropertyInfo benchmarkProperty = benchmarkType.GetProperty(BenchmarkObjectPropertyName);
            // ReSharper disable once PossibleNullReferenceException
            benchmarkProperty.SetValue(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetPropertyInfoCache_Property()
        {
            PropertyInfo.SetValue(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetSigilEmit_Property()
        {
            SigilEmitSetter(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetExpression_Property()
        {
            ExpressionSetter(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetFastMember_Property()
        {
            TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName] = ValueToSet;
        }

        [Benchmark]
        public void SetImmediateProperty_Property()
        {
            ImmediateProperty.SetValue(BenchmarkObject, ValueToSet);
        }
    }
}