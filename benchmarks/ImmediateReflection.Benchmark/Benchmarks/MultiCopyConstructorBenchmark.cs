using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copy constructor benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class MultiCopyConstructorBenchmark : BenchmarkBase
{
    private static readonly CopyableBenchmarkObject ObjectToCopy = new("Benchmark value");
    private static readonly CopyableBenchmarkObject2 ObjectToCopy2 = new(42);
    private static readonly CopyableBenchmarkObject3 ObjectToCopy3 = new(12.5);
    private static readonly CopyableBenchmarkObject4 ObjectToCopy4 = new(5);

    private static readonly Func<CopyableBenchmarkObject, CopyableBenchmarkObject> ExpressionConstructor =
        ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject>();
    private static readonly Func<CopyableBenchmarkObject2, CopyableBenchmarkObject2> ExpressionConstructor2 =
        ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject2>();
    private static readonly Func<CopyableBenchmarkObject3, CopyableBenchmarkObject3> ExpressionConstructor3 =
        ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject3>();
    private static readonly Func<CopyableBenchmarkObject4, CopyableBenchmarkObject4> ExpressionConstructor4 =
        ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject4>();

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Direct_CopyConstructor()
    {
        _ = new CopyableBenchmarkObject(ObjectToCopy);
        _ = new CopyableBenchmarkObject2(ObjectToCopy2);
        _ = new CopyableBenchmarkObject3(ObjectToCopy3);
        _ = new CopyableBenchmarkObject4(ObjectToCopy4);
    }

    [Benchmark]
    public void Activator_CopyConstructor()
    {
        _ = (CopyableBenchmarkObject)Activator.CreateInstance(CopyableBenchmarkObjectType, ObjectToCopy)!;
        _ = (CopyableBenchmarkObject2)Activator.CreateInstance(CopyableBenchmarkObjectType2, ObjectToCopy2)!;
        _ = (CopyableBenchmarkObject3)Activator.CreateInstance(CopyableBenchmarkObjectType3, ObjectToCopy3)!;
        _ = (CopyableBenchmarkObject4)Activator.CreateInstance(CopyableBenchmarkObjectType4, ObjectToCopy4)!;
    }

    [Benchmark]
    public void Expression_CopyConstructor()
    {
        _ = ExpressionConstructor(ObjectToCopy);
        _ = ExpressionConstructor2(ObjectToCopy2);
        _ = ExpressionConstructor3(ObjectToCopy3);
        _ = ExpressionConstructor4(ObjectToCopy4);
    }

    [Benchmark]
    public void ImmediateType_CopyConstructor()
    {
        _ = (CopyableBenchmarkObject)ImmediateTypeCopyable.Copy(ObjectToCopy);
        _ = (CopyableBenchmarkObject2)ImmediateTypeCopyable2.Copy(ObjectToCopy2);
        _ = (CopyableBenchmarkObject3)ImmediateTypeCopyable3.Copy(ObjectToCopy3);
        _ = (CopyableBenchmarkObject4)ImmediateTypeCopyable4.Copy(ObjectToCopy4);
    }
}