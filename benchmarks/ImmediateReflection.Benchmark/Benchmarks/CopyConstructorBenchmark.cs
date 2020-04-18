using System;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copy constructor benchmark class.
    /// </summary>
    public class CopyConstructorBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly CopyableBenchmarkObject ObjectToCopy = new CopyableBenchmarkObject("Benchmark value");

        [NotNull]
        private static readonly Func<CopyableBenchmarkObject, CopyableBenchmarkObject> ExpressionConstructor = 
            ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject>();

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public CopyableBenchmarkObject Direct_CopyConstructor()
        {
            return new CopyableBenchmarkObject(ObjectToCopy);
        }

        [Benchmark]
        public CopyableBenchmarkObject Activator_CopyConstructor()
        {
            return (CopyableBenchmarkObject)Activator.CreateInstance(CopyableBenchmarkObjectType, ObjectToCopy);
        }

        [Benchmark]
        public CopyableBenchmarkObject Expression_CopyConstructor()
        {
            return ExpressionConstructor(ObjectToCopy);
        }

        [Benchmark]
        public CopyableBenchmarkObject ImmediateType_CopyConstructor()
        {
            return (CopyableBenchmarkObject)ImmediateTypeCopyable.Copy(ObjectToCopy);
        }
    }
}