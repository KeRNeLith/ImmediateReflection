using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Sigil;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Property getter benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class PropertyGetterBenchmark : BenchmarkBase
{
    private static readonly Func<BenchmarkObject, string> GetterDelegate = (Func<BenchmarkObject, string>)
        Delegate.CreateDelegate(typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod()!);

    private static readonly Delegate DynamicGetterDelegate = Delegate.CreateDelegate(
        typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod()!);

    private static readonly Func<BenchmarkObject, string> SigilEmitGetter = Emit<Func<BenchmarkObject, string>>
        .NewDynamicMethod("GetProperty")
        .LoadArgument(0)
        .Call(PropertyInfo.GetGetMethod())
        .Return()
        .CreateDelegate();

    private static readonly Func<BenchmarkObject, object> ExpressionGetter = ExpressionHelpers.CreateGetter<BenchmarkObject>(PropertyInfo);

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void GetDirect_Property()
    {
        _ = BenchmarkObject.BenchmarkProperty;
    }

    [Benchmark]
    public void GetDelegate_Property()
    {
        _ = GetterDelegate(BenchmarkObject);
    }

    [Benchmark]
    public void GetDynamicDelegate_Property()
    {
        _ = (string)DynamicGetterDelegate.DynamicInvoke(BenchmarkObject)!;
    }

    [Benchmark]
    public void GetPropertyInfo_Property()
    {
        Type benchmarkType = BenchmarkObject.GetType();
        PropertyInfo benchmarkProperty = benchmarkType.GetProperty(BenchmarkObjectPropertyName)!;
        _ = (string)benchmarkProperty.GetValue(BenchmarkObject)!;
    }

    [Benchmark]
    public void GetPropertyInfoCache_Property()
    {
        _ = (string)PropertyInfo.GetValue(BenchmarkObject)!;
    }

    [Benchmark]
    public void GetSigilEmit_Property()
    {
        _ = SigilEmitGetter(BenchmarkObject);
    }

    [Benchmark]
    public void GetExpression_Property()
    {
        _ = (string)ExpressionGetter(BenchmarkObject);
    }

    [Benchmark]
    public void GetFastMember_Property()
    {
        _ = (string)TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName];
    }

    [Benchmark]
    public void GetImmediateProperty_Property()
    {
        _ = (string)ImmediateProperty.GetValue(BenchmarkObject)!;
    }
}