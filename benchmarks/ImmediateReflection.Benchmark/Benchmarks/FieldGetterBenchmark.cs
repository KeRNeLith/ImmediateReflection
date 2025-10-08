using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Field getter benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class FieldGetterBenchmark : BenchmarkBase
{
    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void GetDirect_Field()
    {
        _ = BenchmarkObject._benchmarkField;
    }

    [Benchmark]
    public void GetFieldInfo_Field()
    {
        Type benchmarkType = BenchmarkObject.GetType();
        FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName)!;
        _ = (int)benchmarkField.GetValue(BenchmarkObject)!;
    }

    [Benchmark]
    public void GetFieldInfoCache_Field()
    {
        _ = (int)FieldInfo.GetValue(BenchmarkObject)!;
    }

    [Benchmark]
    public void GetFastMember_Field()
    {
        _ = (int)TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName];
    }

    [Benchmark]
    public void GetImmediateField_Field()
    {
        _ = (int)ImmediateField.GetValue(BenchmarkObject)!;
    }
}