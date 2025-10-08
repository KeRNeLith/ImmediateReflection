using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Sigil;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Property multi getter benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class PropertyMultiGetterBenchmark : BenchmarkBase
{
    #region Getter Delegates

    private static readonly Func<BenchmarkObject, string> GetterDelegate = (Func<BenchmarkObject, string>)
        Delegate.CreateDelegate(typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod()!);

    private static readonly Func<BenchmarkObject2, int> GetterDelegate2 = (Func<BenchmarkObject2, int>)
        Delegate.CreateDelegate(typeof(Func<BenchmarkObject2, int>), null, PropertyInfo2.GetGetMethod()!);

    private static readonly Func<BenchmarkObject3, double> GetterDelegate3 = (Func<BenchmarkObject3, double>)
        Delegate.CreateDelegate(typeof(Func<BenchmarkObject3, double>), null, PropertyInfo3.GetGetMethod()!);

    private static readonly Func<BenchmarkObject4, short> GetterDelegate4 = (Func<BenchmarkObject4, short>)
        Delegate.CreateDelegate(typeof(Func<BenchmarkObject4, short>), null, PropertyInfo4.GetGetMethod()!);

    #endregion

    #region Dynamic Delegates

    private static readonly Delegate DynamicGetterDelegate = Delegate.CreateDelegate(
        typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod()!);

    private static readonly Delegate DynamicGetterDelegate2 = Delegate.CreateDelegate(
        typeof(Func<BenchmarkObject2, int>), null, PropertyInfo2.GetGetMethod()!);

    private static readonly Delegate DynamicGetterDelegate3 = Delegate.CreateDelegate(
        typeof(Func<BenchmarkObject3, double>), null, PropertyInfo3.GetGetMethod()!);

    private static readonly Delegate DynamicGetterDelegate4 = Delegate.CreateDelegate(
        typeof(Func<BenchmarkObject4, short>), null, PropertyInfo4.GetGetMethod()!);

    #endregion

    #region Sigil Delegates

    private static readonly Func<BenchmarkObject, string> SigilEmitGetter = Emit<Func<BenchmarkObject, string>>
        .NewDynamicMethod("GetProperty")
        .LoadArgument(0)
        .Call(PropertyInfo.GetGetMethod())
        .Return()
        .CreateDelegate();

    private static readonly Func<BenchmarkObject2, int> SigilEmitGetter2 = Emit<Func<BenchmarkObject2, int>>
        .NewDynamicMethod("GetProperty2")
        .LoadArgument(0)
        .Call(PropertyInfo2.GetGetMethod())
        .Return()
        .CreateDelegate();

    private static readonly Func<BenchmarkObject3, double> SigilEmitGetter3 = Emit<Func<BenchmarkObject3, double>>
        .NewDynamicMethod("GetProperty3")
        .LoadArgument(0)
        .Call(PropertyInfo3.GetGetMethod())
        .Return()
        .CreateDelegate();

    private static readonly Func<BenchmarkObject4, short> SigilEmitGetter4 = Emit<Func<BenchmarkObject4, short>>
        .NewDynamicMethod("GetProperty4")
        .LoadArgument(0)
        .Call(PropertyInfo4.GetGetMethod())
        .Return()
        .CreateDelegate();

    #endregion

    #region Expression Delegates

    private static readonly Func<BenchmarkObject, object> ExpressionGetter = ExpressionHelpers.CreateGetter<BenchmarkObject>(PropertyInfo);
    private static readonly Func<BenchmarkObject2, object> ExpressionGetter2 = ExpressionHelpers.CreateGetter<BenchmarkObject2>(PropertyInfo2);
    private static readonly Func<BenchmarkObject3, object> ExpressionGetter3 = ExpressionHelpers.CreateGetter<BenchmarkObject3>(PropertyInfo3);
    private static readonly Func<BenchmarkObject4, object> ExpressionGetter4 = ExpressionHelpers.CreateGetter<BenchmarkObject4>(PropertyInfo4);

    #endregion

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void GetDirect_Property()
    {
        _ = BenchmarkObject.BenchmarkProperty;
        _ = BenchmarkObject2.BenchmarkProperty;
        _ = BenchmarkObject3.BenchmarkProperty;
        _ = BenchmarkObject4.BenchmarkProperty;
    }

    [Benchmark]
    public void GetDelegate_Property()
    {
        _ = GetterDelegate(BenchmarkObject);
        _ = GetterDelegate2(BenchmarkObject2);
        _ = GetterDelegate3(BenchmarkObject3);
        _ = GetterDelegate4(BenchmarkObject4);
    }

    [Benchmark]
    public void GetDynamicDelegate_Property()
    {
        _ = (string)DynamicGetterDelegate.DynamicInvoke(BenchmarkObject)!;
        _ = (int)DynamicGetterDelegate2.DynamicInvoke(BenchmarkObject2)!;
        _ = (double)DynamicGetterDelegate3.DynamicInvoke(BenchmarkObject3)!;
        _ = (short)DynamicGetterDelegate4.DynamicInvoke(BenchmarkObject4)!;
    }

    [Benchmark]
    public void GetPropertyInfo_Property()
    {
        Type benchmarkType = BenchmarkObject.GetType();
        PropertyInfo benchmarkProperty = benchmarkType.GetProperty(BenchmarkObjectPropertyName)!;

        Type benchmarkType2 = BenchmarkObject2.GetType();
        PropertyInfo benchmarkProperty2 = benchmarkType2.GetProperty(BenchmarkObjectPropertyName2)!;

        Type benchmarkType3 = BenchmarkObject3.GetType();
        PropertyInfo benchmarkProperty3 = benchmarkType3.GetProperty(BenchmarkObjectPropertyName3)!;

        Type benchmarkType4 = BenchmarkObject4.GetType();
        PropertyInfo benchmarkProperty4 = benchmarkType4.GetProperty(BenchmarkObjectPropertyName4)!;

        _ = (string)benchmarkProperty.GetValue(BenchmarkObject)!;
        _ = (int)benchmarkProperty2.GetValue(BenchmarkObject2)!;
        _ = (double)benchmarkProperty3.GetValue(BenchmarkObject3)!;
        _ = (short)benchmarkProperty4.GetValue(BenchmarkObject4)!;
    }

    [Benchmark]
    public void GetPropertyInfoCache_Property()
    {
        _ = (string)PropertyInfo.GetValue(BenchmarkObject)!;
        _ = (int)PropertyInfo2.GetValue(BenchmarkObject2)!;
        _ = (double)PropertyInfo3.GetValue(BenchmarkObject3)!;
        _ = (short)PropertyInfo4.GetValue(BenchmarkObject4)!;
    }

    [Benchmark]
    public void GetSigilEmit_Property()
    {
        _ = SigilEmitGetter(BenchmarkObject);
        _ = SigilEmitGetter2(BenchmarkObject2);
        _ = SigilEmitGetter3(BenchmarkObject3);
        _ = SigilEmitGetter4(BenchmarkObject4);
    }

    [Benchmark]
    public void GetExpression_Property()
    {
        _ = (string)ExpressionGetter(BenchmarkObject);
        _ = (int)ExpressionGetter2(BenchmarkObject2);
        _ = (double)ExpressionGetter3(BenchmarkObject3);
        _ = (short)ExpressionGetter4(BenchmarkObject4);
    }

    [Benchmark]
    public void GetFastMember_Property()
    {
        _ = (string)TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName];
        _ = (int)TypeAccessor2[BenchmarkObject2, BenchmarkObjectPropertyName2];
        _ = (double)TypeAccessor3[BenchmarkObject3, BenchmarkObjectPropertyName3];
        _ = (short)TypeAccessor4[BenchmarkObject4, BenchmarkObjectPropertyName4];
    }

    [Benchmark]
    public void GetImmediateProperty_Property()
    {
        _ = (string)ImmediateProperty.GetValue(BenchmarkObject)!;
        _ = (int)ImmediateProperty2.GetValue(BenchmarkObject2)!;
        _ = (double)ImmediateProperty3.GetValue(BenchmarkObject3)!;
        _ = (short)ImmediateProperty4.GetValue(BenchmarkObject4)!;
    }
}