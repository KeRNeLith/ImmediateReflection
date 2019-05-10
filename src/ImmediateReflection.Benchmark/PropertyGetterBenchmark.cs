using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

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
        private static readonly Delegate DynamicSetterDelegate = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod());

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
            return (string)DynamicSetterDelegate.DynamicInvoke(BenchmarkObject);
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