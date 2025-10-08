using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Fasterflect;
using FlashReflection;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Property setter over multiple objects benchmark class.
/// </summary>
[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
public class PropertySetterOverObjectsBenchmark : ObjectsBenchmarkBase
{
    // Benchmark methods
    [Benchmark(Baseline = true)]
    public void Reflection_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyReflection(obj);
        }
    }

    [Benchmark]
    public void ReflectionCache_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyReflectionCache(obj);
        }
    }

    [Benchmark]
    public void TypeDescriptor_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyTypeDescriptor(obj);
        }
    }

    [Benchmark]
    public void FastMember_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyFastMember(obj);
        }
    }

    [Benchmark]
    public void FlashReflection_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyFlashReflection(obj);
        }
    }

    [Benchmark]
    public void ImmediateReflection_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyImmediateReflection(obj);
        }
    }

    [Benchmark]
    public void WithFasterflect_PropertySet_BenchmarkObject()
    {
        foreach (object obj in BenchmarkObjects)
        {
            SetPropertyFasterflect(obj);
        }
    }

    [Benchmark]
    public void Reflection_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyReflection(obj);
        }
    }

    [Benchmark]
    public void ReflectionCache_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyReflectionCache(obj);
        }
    }

    [Benchmark]
    public void TypeDescriptor_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyTypeDescriptor(obj);
        }
    }

    [Benchmark]
    public void FastMember_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyFastMember(obj);
        }
    }

    [Benchmark]
    public void FlashReflection_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyFlashReflection(obj);
        }
    }

    [Benchmark]
    public void ImmediateReflection_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyImmediateReflection(obj);
        }
    }

    [Benchmark]
    public void Fasterflect_PropertySet_Mixed_BenchmarkObject()
    {
        foreach (object obj in BenchmarkMixedObjects)
        {
            SetPropertyFasterflect(obj);
        }
    }

    #region Helper methods

    private static readonly uint[] ValueToSet = [2u, 3u];

    private static void SetPropertyReflection(object obj)
    {
        PropertyInfo? propertyInfo = obj.GetType().GetProperty(UIntArrayPropertyName);
        if (propertyInfo is null || propertyInfo.PropertyType != typeof(uint[]))
            return;

        propertyInfo.SetValue(obj, ValueToSet);
    }

    private static void SetPropertyReflectionCache(object obj)
    {
        if (obj.GetType() != typeof(ObjectsBenchmarkObject1))
            return;

        UIntArrayPropertyInfo.SetValue(obj, ValueToSet);
    }

    private static void SetPropertyTypeDescriptor(object obj)
    {
        PropertyDescriptor? propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(UIntArrayPropertyName, false);
        propertyDescriptor?.SetValue(obj, ValueToSet);
    }

    private static void SetPropertyFastMember(object obj)
    {
        var accessor = FastMember.TypeAccessor.Create(obj.GetType());
        bool hasProperty = accessor.GetMembers().Any(m => m.Name == UIntArrayPropertyName);
        if (!hasProperty)
            return;

        accessor[obj, UIntArrayPropertyName] = ValueToSet;
    }

    private static void SetPropertyFlashReflection(object obj)
    {
        ReflectionType type = ReflectionCache.Instance.GetReflectionType(obj.GetType());
        ReflectionProperty? property = type.Properties[UIntArrayPropertyName];

        property?.SetValue(obj, ValueToSet);
    }

    private static void SetPropertyImmediateReflection(object obj)
    {
        ImmediateType accessor = ImmediateReflection.TypeAccessor.Get(obj.GetType());
        ImmediateProperty? property = accessor.GetProperty(UIntArrayPropertyName);

        property?.SetValue(obj, ValueToSet);
    }

    private static void SetPropertyFasterflect(object obj)
    {
        obj.TrySetPropertyValue(UIntArrayPropertyName, ValueToSet);
    }

    #endregion
}