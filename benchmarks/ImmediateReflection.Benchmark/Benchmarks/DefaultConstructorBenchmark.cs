using System;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Default constructor benchmark class.
    /// </summary>
    public class DefaultConstructorBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly Func<BenchmarkObject> ExpressionConstructor = ExpressionHelpers.CreateDefaultConstructor<BenchmarkObject>();

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public BenchmarkObject Direct_Constructor()
        {
            return new BenchmarkObject();
        }

        [Benchmark]
        public BenchmarkObject Activator_Constructor()
        {
            return (BenchmarkObject)Activator.CreateInstance(BenchmarkObjectType);
        }

        [Benchmark]
        public BenchmarkObject Expression_Constructor()
        {
            return ExpressionConstructor();
        }

        [Benchmark]
        public BenchmarkObject FastMember_Constructor()
        {
            return (BenchmarkObject)TypeAccessor.CreateNew();
        }

        [Benchmark]
        public BenchmarkObject ImmediateType_Constructor()
        {
            return (BenchmarkObject)ImmediateType.New();
        }
    }
}