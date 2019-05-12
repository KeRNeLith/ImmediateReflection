using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Benchmark base class.
    /// </summary>
    public class BenchmarkBase
    {
        [NotNull]
        protected static readonly Type BenchmarkObjectType = typeof(BenchmarkObject);

        [NotNull]
        protected const string BenchmarkObjectFieldName = nameof(BenchmarkObject._benchmarkField);

        [NotNull]
        protected const string BenchmarkObjectPropertyName = nameof(BenchmarkObject.BenchmarkProperty);

        [NotNull]
        protected static readonly FieldInfo FieldInfo = BenchmarkObjectType.GetField(BenchmarkObjectFieldName) ?? throw new InvalidOperationException("Field does not exist.");

        [NotNull]
        protected static readonly PropertyInfo PropertyInfo = BenchmarkObjectType.GetProperty(BenchmarkObjectPropertyName) ?? throw new InvalidOperationException("Property does not exist.");

        [NotNull]
        internal static readonly BenchmarkObject BenchmarkObject = new BenchmarkObject();

        [NotNull]
        protected static readonly FastMember.TypeAccessor TypeAccessor = FastMember.TypeAccessor.Create(BenchmarkObjectType);

        [NotNull]
        protected static readonly ImmediateField ImmediateField = new ImmediateField(FieldInfo);

        [NotNull]
        protected static readonly ImmediateProperty ImmediateProperty = new ImmediateProperty(PropertyInfo);
    }
}