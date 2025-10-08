using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Default constructor benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class DefaultConstructorBenchmark : BenchmarkBase
{
    private static readonly Func<BenchmarkObject> ExpressionConstructor =
        ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject>();

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Direct_Constructor()
    {
        _ = new BenchmarkObject();
    }

    [Benchmark]
    public void Activator_Constructor()
    {
        _ = (BenchmarkObject)Activator.CreateInstance(BenchmarkObjectType)!;
    }

    [Benchmark]
    public void Expression_Constructor()
    {
        _ = ExpressionConstructor();
    }

    [Benchmark]
    public void FastMember_Constructor()
    {
        _ = (BenchmarkObject)TypeAccessor.CreateNew();
    }

    [Benchmark]
    public void ImmediateType_Constructor()
    {
        _ = (BenchmarkObject)ImmediateType.New();
    }
}