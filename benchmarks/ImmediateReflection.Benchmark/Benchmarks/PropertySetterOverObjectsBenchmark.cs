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
    /// Property setter over multiple objects benchmark class.
    /// </summary>
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
        public void HyperDescriptor_PropertySet_BenchmarkObject()
        {
            foreach (object obj in BenchmarkObjects)
            {
                SetPropertyHyperDescriptor(obj);
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
        public void HyperDescriptor_PropertySet_Mixed_BenchmarkObject()
        {
            foreach (object obj in BenchmarkMixedObjects)
            {
                SetPropertyHyperDescriptor(obj);
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

        private static readonly uint[] ValueToSet = { 2u, 3u };

        public void SetPropertyReflection([NotNull] object obj)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(UIntArrayPropertyName);
            if (propertyInfo is null || propertyInfo.PropertyType != typeof(uint[]))
                return;

            propertyInfo.SetValue(obj, ValueToSet);
        }

        public void SetPropertyReflectionCache([NotNull] object obj)
        {
            if (obj.GetType() != typeof(ObjectsBenchmarkObject1))
                return;

            UIntArrayPropertyInfo.SetValue(obj, ValueToSet);
        }

        public void SetPropertyHyperDescriptor([NotNull] object obj)
        {
            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(obj).Find(UIntArrayPropertyName, false);
            propertyDescriptor?.SetValue(obj, ValueToSet);
        }

        public void SetPropertyFastMember([NotNull] object obj)
        {
            FastMember.TypeAccessor accessor = FastMember.TypeAccessor.Create(obj.GetType());
            bool hasProperty = accessor.GetMembers().Any(m => m.Name == UIntArrayPropertyName);
            if (!hasProperty)
                return;

            accessor[obj, UIntArrayPropertyName] = ValueToSet;
        }

        public void SetPropertyFlashReflection([NotNull] object obj)
        {
            ReflectionType type = ReflectionCache.Instance.GetReflectionType(obj.GetType());
            ReflectionProperty property = type.Properties[UIntArrayPropertyName];

            property?.SetValue(obj, ValueToSet);
        }

        public void SetPropertyImmediateReflection([NotNull] object obj)
        {
            ImmediateType accessor = ImmediateReflection.TypeAccessor.Get(obj.GetType());
            ImmediateProperty property = accessor.GetProperty(UIntArrayPropertyName);

            property?.SetValue(obj, ValueToSet);
        }

        public void SetPropertyFasterflect([NotNull] object obj)
        {
            obj.TrySetPropertyValue(UIntArrayPropertyName, ValueToSet);
        }

        #endregion
    }
}