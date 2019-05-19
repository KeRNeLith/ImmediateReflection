using System;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

// ReSharper disable UnusedVariable

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Constructor benchmark class.
    /// </summary>
    public class MultiConstructorBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly Func<BenchmarkObject> ExpressionConstructor = ExpressionHelpers.CreateConstructor<BenchmarkObject>();

        [NotNull]
        private static readonly Func<BenchmarkObject2> ExpressionConstructor2 = ExpressionHelpers.CreateConstructor<BenchmarkObject2>();

        [NotNull]
        private static readonly Func<BenchmarkObject3> ExpressionConstructor3 = ExpressionHelpers.CreateConstructor<BenchmarkObject3>();

        [NotNull]
        private static readonly Func<BenchmarkObject4> ExpressionConstructor4 = ExpressionHelpers.CreateConstructor<BenchmarkObject4>();

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void Direct_Constructor()
        {
            // ReSharper disable ObjectCreationAsStatement
            var obj1 = new BenchmarkObject();
            var obj2 = new BenchmarkObject2();
            var obj3 = new BenchmarkObject3();
            var obj4 = new BenchmarkObject4();
            // ReSharper restore ObjectCreationAsStatement
        }

        [Benchmark]
        public void Activator_Constructor()
        {
            var obj1 = (BenchmarkObject)Activator.CreateInstance(BenchmarkObjectType);
            var obj2 = (BenchmarkObject2)Activator.CreateInstance(BenchmarkObjectType2);
            var obj3 = (BenchmarkObject3)Activator.CreateInstance(BenchmarkObjectType3);
            var obj4 = (BenchmarkObject4)Activator.CreateInstance(BenchmarkObjectType4);
        }

        [Benchmark]
        public void Expression_Constructor()
        {
            BenchmarkObject obj1 = ExpressionConstructor();
            BenchmarkObject2 obj2 = ExpressionConstructor2();
            BenchmarkObject3 obj3 = ExpressionConstructor3();
            BenchmarkObject4 obj4 = ExpressionConstructor4();
        }

        [Benchmark]
        public void FastMember_Constructor()
        {
            var obj1 = (BenchmarkObject)TypeAccessor.CreateNew();
            var obj2 = (BenchmarkObject2)TypeAccessor2.CreateNew();
            var obj3 = (BenchmarkObject3)TypeAccessor3.CreateNew();
            var obj4 = (BenchmarkObject4)TypeAccessor4.CreateNew();
        }

        [Benchmark]
        public void ImmediateType_Constructor()
        {
            var obj1 = (BenchmarkObject)ImmediateType.New();
            var obj2 = (BenchmarkObject2)ImmediateType2.New();
            var obj3 = (BenchmarkObject3)ImmediateType3.New();
            var obj4 = (BenchmarkObject4)ImmediateType4.New();
        }
    }
}