using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Field getter benchmark class.
    /// </summary>
    public class FieldGetterBenchmark : BenchmarkBase
    {
        // Benchmark methods
        [Benchmark(Baseline = true)]
        public int GetDirect_Field()
        {
            return BenchmarkObject._benchmarkField;
        }

        [Benchmark]
        public int GetFieldInfo_Field()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName);
            // ReSharper disable once PossibleNullReferenceException
            return (int)benchmarkField.GetValue(BenchmarkObject);
        }

        [Benchmark]
        public int GetFieldInfoCache_Field()
        {
            return (int)FieldInfo.GetValue(BenchmarkObject);
        }

        [Benchmark]
        public int GetFastMember_Field()
        {
            return (int)TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName];
        }

        [Benchmark]
        public int GetImmediateField_Field()
        {
            return (int)ImmediateField.GetValue(BenchmarkObject);
        }
    }
}