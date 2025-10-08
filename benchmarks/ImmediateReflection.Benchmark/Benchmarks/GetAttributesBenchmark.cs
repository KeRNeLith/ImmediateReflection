using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using FastMember;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Get attributes benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class GetAttributesBenchmark : BenchmarkBase
{
    private static readonly Type AttributesBenchmarkObjectType = typeof(AttributesBenchmarkObject);

    private static readonly PropertyInfo AttributesBenchmarkProperty =
        AttributesBenchmarkObjectType.GetProperty(nameof(AttributesBenchmarkObject.TestProperty))
        ?? throw new InvalidOperationException("Property does not exist.");

    private static readonly Attribute[] CachedAttributes = AttributesBenchmarkProperty
        .GetCustomAttributes(false)
        .OfType<Attribute>()
        .ToArray();

    private static TAttribute? GetAttributeFromCache<TAttribute>()
        where TAttribute : Attribute
    {
        return (TAttribute?)CachedAttributes.FirstOrDefault(attribute => attribute is TAttribute);
    }

    private static readonly Member FastMemberProperty = FastMember.TypeAccessor.Create(AttributesBenchmarkObjectType).GetMembers()[0];

    private static readonly ImmediateProperty AttributesImmediateProperty = new(AttributesBenchmarkProperty);

    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Property_GetAttribute()
    {
        _ = AttributesBenchmarkProperty.GetCustomAttribute<TestClassAttribute>(false);
        _ = AttributesBenchmarkProperty.GetCustomAttribute<ThirdTestClassAttribute>(false);
    }

    [Benchmark]
    public void PropertyCache_GetAttribute()
    {
        _ = GetAttributeFromCache<TestClassAttribute>();
        _ = GetAttributeFromCache<ThirdTestClassAttribute>();
    }

    [Benchmark]
    public void FastMember_GetAttribute()
    {
        _ = FastMemberProperty.GetAttribute(typeof(TestClassAttribute), false);
        _ = FastMemberProperty.GetAttribute(typeof(ThirdTestClassAttribute), false);
    }

    [Benchmark]
    public void ImmediateProperty_GetAttribute()
    {
        _ = AttributesImmediateProperty.GetAttribute<TestClassAttribute>();
        _ = AttributesImmediateProperty.GetAttribute<ThirdTestClassAttribute>();
    }

    [Benchmark]
    public void Property_ByImmediateReflection_GetAttribute()
    {
        _ = AttributesBenchmarkProperty.GetImmediateAttribute<TestClassAttribute>();
        _ = AttributesBenchmarkProperty.GetImmediateAttribute<ThirdTestClassAttribute>();
    }
}