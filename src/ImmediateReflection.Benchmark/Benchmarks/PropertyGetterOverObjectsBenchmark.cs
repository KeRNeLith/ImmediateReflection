using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Fasterflect;
using FlashReflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Property getter over multiple objects benchmark class.
    /// </summary>
    public class PropertyGetterOverObjectsBenchmark : ObjectsBenchmarkBase
    {
        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void Reflection_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyReflection(obj);
            }
        }

        [Benchmark]
        public void ReflectionCache_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyReflectionCache(obj);
            }
        }

        [Benchmark]
        public void HyperDescriptor_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyHyperDescriptor(obj);
            }
        }

        [Benchmark]
        public void FastMember_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyFastMember(obj);
            }
        }

        [Benchmark]
        public void FlashReflection_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyFlashReflection(obj);
            }
        }

        [Benchmark]
        public void ImmediateReflection_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyImmediateReflection(obj);
            }
        }

        [Benchmark]
        public void WithFasterflect_PropertyGet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                GetPropertyFasterflect(obj);
            }
        }

        [Benchmark]
        public void Reflection_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyReflection(obj);
            }
        }

        [Benchmark]
        public void ReflectionCache_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyReflectionCache(obj);
            }
        }

        [Benchmark]
        public void HyperDescriptor_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyHyperDescriptor(obj);
            }
        }

        [Benchmark]
        public void FastMember_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyFastMember(obj);
            }
        }

        [Benchmark]
        public void FlashReflection_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyFlashReflection(obj);
            }
        }

        [Benchmark]
        public void ImmediateReflection_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyImmediateReflection(obj);
            }
        }

        [Benchmark]
        public void Fasterflect_PropertyGet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                GetPropertyFasterflect(obj);
            }
        }

        #region Helper methods

        public object GetPropertyReflection([NotNull] object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(UIntArrayPropertyName);
            if (propertyInfo is null || propertyInfo.PropertyType != typeof(uint[]))
                return null;

            return propertyInfo.GetValue(obj);
        }

        public object GetPropertyReflectionCache([NotNull] object obj)
        {
            if (obj.GetType() != typeof(ObjectsBenchmarkObject1))
                return null;

            return UIntArrayPropertyInfo.GetValue(obj);
        }

        public object GetPropertyHyperDescriptor([NotNull] object obj)
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(UIntArrayPropertyName, false);
            return propertyDescriptor?.GetValue(obj);
        }

        public object GetPropertyFastMember([NotNull] object obj)
        {
            FastMember.TypeAccessor accessor = FastMember.TypeAccessor.Create(obj.GetType());
            bool hasProperty = accessor.GetMembers().Any(m => m.Name == UIntArrayPropertyName);
            if (!hasProperty)
                return null;

            return accessor[obj, UIntArrayPropertyName];
        }

        public object GetPropertyFlashReflection([NotNull] object obj)
        {
            ReflectionType type = ReflectionCache.Instance.GetReflectionType(obj.GetType());
            ReflectionProperty property = type.Properties[UIntArrayPropertyName];

            return property?.GetValue(obj);
        }

        public object GetPropertyImmediateReflection([NotNull] object obj)
        {
            ImmediateType accessor = ImmediateReflection.TypeAccessor.Get(obj.GetType());
            ImmediateProperty property = accessor.GetProperty(UIntArrayPropertyName);

            return property?.GetValue(obj);
        }

        public object GetPropertyFasterflect([NotNull] object obj)
        {
            return obj.TryGetPropertyValue(UIntArrayPropertyName);
        }

        #endregion
    }
}