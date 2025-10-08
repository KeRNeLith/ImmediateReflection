using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Fasterflect;
using FlashReflection;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Property getter over multiple objects benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class PropertyGetterOverObjectsBenchmark : ObjectsBenchmarkBase
{
    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Reflection_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyReflection(obj);
        }
    }

    [Benchmark]
    public void ReflectionCache_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyReflectionCache(obj);
        }
    }

    [Benchmark]
    public void TypeDescriptor_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyTypeDescriptor(obj);
        }
    }

    [Benchmark]
    public void FastMember_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyFastMember(obj);
        }
    }

    [Benchmark]
    public void FlashReflection_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyFlashReflection(obj);
        }
    }

    [Benchmark]
    public void ImmediateReflection_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyImmediateReflection(obj);
        }
    }

    [Benchmark]
    public void WithFasterflect_PropertyGet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            _ = GetPropertyFasterflect(obj);
        }
    }

    [Benchmark]
    public void Reflection_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyReflection(obj);
        }
    }

    [Benchmark]
    public void ReflectionCache_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyReflectionCache(obj);
        }
    }

    [Benchmark]
    public void TypeDescriptor_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyTypeDescriptor(obj);
        }
    }

    [Benchmark]
    public void FastMember_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyFastMember(obj);
        }
    }

    [Benchmark]
    public void FlashReflection_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyFlashReflection(obj);
        }
    }

    [Benchmark]
    public void ImmediateReflection_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyImmediateReflection(obj);
        }
    }

    [Benchmark]
    public void Fasterflect_PropertyGet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            _ = GetPropertyFasterflect(obj);
        }
    }

    #region Helper methods

    private static object? GetPropertyReflection(object obj)
    {
        PropertyInfo? propertyInfo = obj.GetType().GetProperty(UIntArrayPropertyName);
        if (propertyInfo is null || propertyInfo.PropertyType != typeof(uint[]))
            return null;

        return propertyInfo.GetValue(obj);
    }

    private static object? GetPropertyReflectionCache(object obj)
    {
        if (obj.GetType() != typeof(ObjectsBenchmarkObject1))
            return null;

        return UIntArrayPropertyInfo.GetValue(obj);
    }

    private static object? GetPropertyTypeDescriptor(object obj)
    {
        PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(UIntArrayPropertyName, false);
        return propertyDescriptor?.GetValue(obj);
    }

    private static object? GetPropertyFastMember(object obj)
    {
        var accessor = FastMember.TypeAccessor.Create(obj.GetType());
        bool hasProperty = accessor.GetMembers().Any(m => m.Name == UIntArrayPropertyName);
        if (!hasProperty)
            return null;

        return accessor[obj, UIntArrayPropertyName];
    }

    private static object? GetPropertyFlashReflection(object obj)
    {
        ReflectionType type = ReflectionCache.Instance.GetReflectionType(obj.GetType());
        ReflectionProperty? property = type.Properties[UIntArrayPropertyName];

        return property?.GetValue(obj);
    }

    private static object? GetPropertyImmediateReflection(object obj)
    {
        ImmediateType accessor = ImmediateReflection.TypeAccessor.Get(obj.GetType());
        ImmediateProperty? property = accessor.GetProperty(UIntArrayPropertyName);

        return property?.GetValue(obj);
    }

    private static object? GetPropertyFasterflect(object obj)
    {
        return obj.TryGetPropertyValue(UIntArrayPropertyName);
    }

    #endregion
}