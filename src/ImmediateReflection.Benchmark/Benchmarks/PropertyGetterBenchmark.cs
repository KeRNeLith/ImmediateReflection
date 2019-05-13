using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using Sigil;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Property getter benchmark class.
    /// </summary>
    public class PropertyGetterBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly Func<BenchmarkObject, string> GetterDelegate = (Func<BenchmarkObject, string>)
            Delegate.CreateDelegate(typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod());

        [NotNull]
        private static readonly Delegate DynamicGetterDelegate = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod());

        [NotNull]
        private static readonly Func<BenchmarkObject, string> SigilEmitGetter = Emit<Func<BenchmarkObject, string>>
            .NewDynamicMethod("GetProperty")
            .LoadArgument(0)
            .Call(PropertyInfo.GetGetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Func<BenchmarkObject, object> ExpressionGetter = ExpressionHelpers.CreateGetter<BenchmarkObject>(PropertyInfo);

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public string GetDirect_Property()
        {
            return BenchmarkObject.BenchmarkProperty;
        }

        [Benchmark]
        public string GetDelegate_Property()
        {
            return GetterDelegate(BenchmarkObject);
        }

        [Benchmark]
        public string GetDynamicDelegate_Property()
        {
            return (string)DynamicGetterDelegate.DynamicInvoke(BenchmarkObject);
        }

        [Benchmark]
        public string GetPropertyInfo_Property()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            PropertyInfo benchmarkProperty = benchmarkType.GetProperty(BenchmarkObjectPropertyName);
            // ReSharper disable once PossibleNullReferenceException
            return (string)benchmarkProperty.GetValue(BenchmarkObject);
        }

        [Benchmark]
        public string GetPropertyInfoCache_Property()
        {
            return (string)PropertyInfo.GetValue(BenchmarkObject);
        }

        [Benchmark]
        public string GetSigilEmit_Property()
        {
            return SigilEmitGetter(BenchmarkObject);
        }

        [Benchmark]
        public string GetExpression_Property()
        {
            return (string)ExpressionGetter(BenchmarkObject);
        }

        [Benchmark]
        public string GetFastMember_Property()
        {
            return (string)TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName];
        }

        [Benchmark]
        public string GetImmediateProperty_Property()
        {
            return (string)ImmediateProperty.GetValue(BenchmarkObject);
        }
    }
}