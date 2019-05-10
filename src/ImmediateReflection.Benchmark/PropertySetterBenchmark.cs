using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

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