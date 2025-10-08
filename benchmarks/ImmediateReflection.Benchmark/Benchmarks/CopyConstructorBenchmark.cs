using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copy constructor benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class CopyConstructorBenchmark : BenchmarkBase
{
    private static readonly CopyableBenchmarkObject ObjectToCopy = new("Benchmark value");

    private static readonly Func<CopyableBenchmarkObject, CopyableBenchmarkObject> ExpressionConstructor =
        ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject>();

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Direct_CopyConstructor()
    {
        _ = new CopyableBenchmarkObject(ObjectToCopy);
    }

    [Benchmark]
    public void Activator_CopyConstructor()
    {
        _ = (CopyableBenchmarkObject)Activator.CreateInstance(CopyableBenchmarkObjectType, ObjectToCopy)!;
    }

    [Benchmark]
    public void Expression_CopyConstructor()
    {
        _ = ExpressionConstructor(ObjectToCopy);
    }

    [Benchmark]
    public void ImmediateType_CopyConstructor()
    {
        _ = (CopyableBenchmarkObject)ImmediateTypeCopyable.Copy(ObjectToCopy);
    }
}