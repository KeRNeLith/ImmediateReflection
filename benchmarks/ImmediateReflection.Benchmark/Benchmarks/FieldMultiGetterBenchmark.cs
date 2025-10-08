using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Field multi getter benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class FieldMultiGetterBenchmark : BenchmarkBase
{
    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void GetDirect_Field()
    {
        _ = BenchmarkObject._benchmarkField;
        _ = BenchmarkObject2._benchmarkField;
        _ = BenchmarkObject3._benchmarkField;
        _ = BenchmarkObject4._benchmarkField;
    }

    [Benchmark]
    public void GetFieldInfo_Field()
    {
        Type benchmarkType = BenchmarkObject.GetType();
        FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName)!;

        Type benchmarkType2 = BenchmarkObject2.GetType();
        FieldInfo benchmarkField2 = benchmarkType2.GetField(BenchmarkObjectFieldName2)!;

        Type benchmarkType3 = BenchmarkObject3.GetType();
        FieldInfo benchmarkField3 = benchmarkType3.GetField(BenchmarkObjectFieldName3)!;

        Type benchmarkType4 = BenchmarkObject4.GetType();
        FieldInfo benchmarkField4 = benchmarkType4.GetField(BenchmarkObjectFieldName4)!;

        _ = (int)benchmarkField.GetValue(BenchmarkObject)!;
        _ = (float)benchmarkField2.GetValue(BenchmarkObject2)!;
        _ = (string)benchmarkField3.GetValue(BenchmarkObject3)!;
        _ = (uint)benchmarkField4.GetValue(BenchmarkObject4)!;
    }

    [Benchmark]
    public void GetFieldInfoCache_Field()
    {
        _ = (int)FieldInfo.GetValue(BenchmarkObject)!;
        _ = (float)FieldInfo2.GetValue(BenchmarkObject2)!;
        _ = (string)FieldInfo3.GetValue(BenchmarkObject3)!;
        _ = (uint)FieldInfo4.GetValue(BenchmarkObject4)!;
    }

    [Benchmark]
    public void GetFastMember_Field()
    {
        _ = (int)TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName];
        _ = (float)TypeAccessor2[BenchmarkObject2, BenchmarkObjectFieldName2];
        _ = (string)TypeAccessor3[BenchmarkObject3, BenchmarkObjectFieldName3];
        _ = (uint)TypeAccessor4[BenchmarkObject4, BenchmarkObjectFieldName4];
    }

    [Benchmark]
    public void GetImmediateField_Field()
    {
        _ = (int)ImmediateField.GetValue(BenchmarkObject)!;
        _ = (float)ImmediateField2.GetValue(BenchmarkObject2)!;
        _ = (string)ImmediateField3.GetValue(BenchmarkObject3)!;
        _ = (uint)ImmediateField4.GetValue(BenchmarkObject4)!;
    }
}