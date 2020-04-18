using System;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using FastMember;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    public class GetAttributesBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly Type AttributesBenchmarkObjectType = typeof(AttributesBenchmarkObject);

        [NotNull] private static readonly PropertyInfo AttributesBenchmarkProperty =
            AttributesBenchmarkObjectType.GetProperty(nameof(AttributesBenchmarkObject.TestProperty)) 
            ?? throw new InvalidOperationException("Property does not exist.");

        [NotNull, ItemNotNull]
        private static readonly Attribute[] CachedAttributes = AttributesBenchmarkProperty.GetCustomAttributes(false).OfType<Attribute>().ToArray();

        [CanBeNull]
        // ReSharper disable once UnusedMethodReturnValue.Local
        private static TAttribute GetAttributeFromCache<TAttribute>()
            where TAttribute : Attribute
        {
            return (TAttribute)CachedAttributes.FirstOrDefault(attribute => attribute is TAttribute);
        }

        [NotNull]
        private static readonly Member FastMemberProperty = FastMember.TypeAccessor.Create(AttributesBenchmarkObjectType).GetMembers()[0];

        [NotNull]
        private static readonly ImmediateProperty AttributesImmediateProperty = new ImmediateProperty(AttributesBenchmarkProperty);

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void Property_GetAttribute()
        {
            AttributesBenchmarkProperty.GetCustomAttribute<TestClassAttribute>(false);
            AttributesBenchmarkProperty.GetCustomAttribute<ThirdTestClassAttribute>(false);
        }

        [Benchmark]
        public void PropertyCache_GetAttribute()
        {
            GetAttributeFromCache<TestClassAttribute>();
            GetAttributeFromCache<ThirdTestClassAttribute>();
        }

        [Benchmark]
        public void FastMember_GetAttribute()
        {
            FastMemberProperty.GetAttribute(typeof(TestClassAttribute), false);
            FastMemberProperty.GetAttribute(typeof(ThirdTestClassAttribute), false);
        }

        [Benchmark]
        public void ImmediateProperty_GetAttribute()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            AttributesImmediateProperty.GetAttribute<TestClassAttribute>();
            AttributesImmediateProperty.GetAttribute<ThirdTestClassAttribute>();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [Benchmark]
        public void Property_ByImmediateReflection_GetAttribute()
        {
            AttributesBenchmarkProperty.GetImmediateAttribute<TestClassAttribute>();
            AttributesBenchmarkProperty.GetImmediateAttribute<TestClassAttribute>();
        }
    }
}