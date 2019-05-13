using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Field multi getter benchmark class.
    /// </summary>
    public class FieldMultiGetterBenchmark : BenchmarkBase
    {
        // Benchmark methods
        [Benchmark(Baseline = true)]
        public string GetDirect_Field()
        {
            return $"{BenchmarkObject._benchmarkField}{BenchmarkObject2._benchmarkField}{BenchmarkObject3._benchmarkField}{BenchmarkObject4._benchmarkField}";
        }

        [Benchmark]
        public string GetFieldInfo_Field()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName);

            Type benchmarkType2 = BenchmarkObject2.GetType();
            FieldInfo benchmarkField2 = benchmarkType2.GetField(BenchmarkObjectFieldName2);

            Type benchmarkType3 = BenchmarkObject3.GetType();
            FieldInfo benchmarkField3 = benchmarkType3.GetField(BenchmarkObjectFieldName3);

            Type benchmarkType4 = BenchmarkObject4.GetType();
            FieldInfo benchmarkField4 = benchmarkType4.GetField(BenchmarkObjectFieldName4);

            // ReSharper disable PossibleNullReferenceException
            return $"{(int)benchmarkField.GetValue(BenchmarkObject)}{(float)benchmarkField2.GetValue(BenchmarkObject2)}{(string)benchmarkField3.GetValue(BenchmarkObject3)}{(uint)benchmarkField4.GetValue(BenchmarkObject4)}";
            // ReSharper restore PossibleNullReferenceException
        }

        [Benchmark]
        public string GetFieldInfoCache_Field()
        {
            return $"{(int)FieldInfo.GetValue(BenchmarkObject)}{(float)FieldInfo2.GetValue(BenchmarkObject2)}{(string)FieldInfo3.GetValue(BenchmarkObject3)}{(uint)FieldInfo4.GetValue(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetFastMember_Field()
        {
            return $"{(int)TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName]}{(float)TypeAccessor2[BenchmarkObject2, BenchmarkObjectFieldName2]}{(string)TypeAccessor3[BenchmarkObject3, BenchmarkObjectFieldName3]}{(uint)TypeAccessor4[BenchmarkObject4, BenchmarkObjectFieldName4]}";
        }

        [Benchmark]
        public string GetImmediateField_Field()
        {
            return $"{(int)ImmediateField.GetValue(BenchmarkObject)}{(float)ImmediateField2.GetValue(BenchmarkObject2)}{(string)ImmediateField3.GetValue(BenchmarkObject3)}{(uint)ImmediateField4.GetValue(BenchmarkObject4)}";
        }
    }
}