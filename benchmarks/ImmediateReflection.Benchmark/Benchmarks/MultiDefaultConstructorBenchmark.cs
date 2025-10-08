using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Default constructor benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class MultiDefaultConstructorBenchmark : BenchmarkBase
{
    private static readonly Func<BenchmarkObject> ExpressionConstructor = ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject>();
    private static readonly Func<BenchmarkObject2> ExpressionConstructor2 = ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject2>();
    private static readonly Func<BenchmarkObject3> ExpressionConstructor3 = ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject3>();
    private static readonly Func<BenchmarkObject4> ExpressionConstructor4 = ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject4>();

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Direct_Constructor()
    {
        _ = new BenchmarkObject();
        _ = new BenchmarkObject2();
        _ = new BenchmarkObject3();
        _ = new BenchmarkObject4();
    }

    [Benchmark]
    public void Activator_Constructor()
    {
        _ = (BenchmarkObject)Activator.CreateInstance(BenchmarkObjectType)!;
        _ = (BenchmarkObject2)Activator.CreateInstance(BenchmarkObjectType2)!;
        _ = (BenchmarkObject3)Activator.CreateInstance(BenchmarkObjectType3)!;
        _ = (BenchmarkObject4)Activator.CreateInstance(BenchmarkObjectType4)!;
    }

    [Benchmark]
    public void Expression_Constructor()
    {
        _ = ExpressionConstructor();
        _ = ExpressionConstructor2();
        _ = ExpressionConstructor3();
        _ = ExpressionConstructor4();
    }

    [Benchmark]
    public void FastMember_Constructor()
    {
        _ = (BenchmarkObject)TypeAccessor.CreateNew();
        _ = (BenchmarkObject2)TypeAccessor2.CreateNew();
        _ = (BenchmarkObject3)TypeAccessor3.CreateNew();
        _ = (BenchmarkObject4)TypeAccessor4.CreateNew();
    }

    [Benchmark]
    public void ImmediateType_Constructor()
    {
        _ = (BenchmarkObject)ImmediateType.New();
        _ = (BenchmarkObject2)ImmediateType2.New();
        _ = (BenchmarkObject3)ImmediateType3.New();
        _ = (BenchmarkObject4)ImmediateType4.New();
    }
}