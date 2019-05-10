using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Field setter benchmark class.
    /// </summary>
    public class FieldSetterBenchmark : BenchmarkBase
    {
        private const int ValueToSet = 12;

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void SetDirect_Field()
        {
            BenchmarkObject._benchmarkField = ValueToSet;
        }

        [Benchmark]
        public void SetFieldInfo_Field()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName);
            // ReSharper disable once PossibleNullReferenceException
            benchmarkField.SetValue(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetFieldInfoCache_Field()
        {
            FieldInfo.SetValue(BenchmarkObject, ValueToSet);
        }

        [Benchmark]
        public void SetFastMember_Field()
        {
            TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName] = ValueToSet;
        }

        [Benchmark]
        public void SetImmediateField_Field()
        {
            ImmediateField.SetValue(BenchmarkObject, ValueToSet);
        }
    }
}