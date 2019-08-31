using System;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

// ReSharper disable UnusedVariable

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copy constructor benchmark class.
    /// </summary>
    public class MultiCopyConstructorBenchmark : BenchmarkBase
    {
        [NotNull]
        private static readonly CopyableBenchmarkObject ObjectToCopy = new CopyableBenchmarkObject("Benchmark value");

        [NotNull]
        private static readonly CopyableBenchmarkObject2 ObjectToCopy2 = new CopyableBenchmarkObject2(42);

        [NotNull]
        private static readonly CopyableBenchmarkObject3 ObjectToCopy3 = new CopyableBenchmarkObject3(12.5);

        [NotNull]
        private static readonly CopyableBenchmarkObject4 ObjectToCopy4 = new CopyableBenchmarkObject4(5);

        [NotNull]
        private static readonly Func<CopyableBenchmarkObject, CopyableBenchmarkObject> ExpressionConstructor = 
            ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject>();

        [NotNull]
        private static readonly Func<CopyableBenchmarkObject2, CopyableBenchmarkObject2> ExpressionConstructor2 =
            ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject2>();

        [NotNull]
        private static readonly Func<CopyableBenchmarkObject3, CopyableBenchmarkObject3> ExpressionConstructor3 =
            ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject3>();

        [NotNull]
        private static readonly Func<CopyableBenchmarkObject4, CopyableBenchmarkObject4> ExpressionConstructor4 =
            ExpressionHelpers.CreateCopyConstructor<CopyableBenchmarkObject4>();

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void Direct_CopyConstructor()
        {
            // ReSharper disable ObjectCreationAsStatement
            var obj1 = new CopyableBenchmarkObject(ObjectToCopy);
            var obj2 = new CopyableBenchmarkObject2(ObjectToCopy2);
            var obj3 = new CopyableBenchmarkObject3(ObjectToCopy3);
            var obj4 = new CopyableBenchmarkObject4(ObjectToCopy4);
            // ReSharper restore ObjectCreationAsStatement
        }

        [Benchmark]
        public void Activator_CopyConstructor()
        {
            var obj1 = (CopyableBenchmarkObject)Activator.CreateInstance(CopyableBenchmarkObjectType, ObjectToCopy);
            var obj2 = (CopyableBenchmarkObject2)Activator.CreateInstance(CopyableBenchmarkObjectType2, ObjectToCopy2);
            var obj3 = (CopyableBenchmarkObject3)Activator.CreateInstance(CopyableBenchmarkObjectType3, ObjectToCopy3);
            var obj4 = (CopyableBenchmarkObject4)Activator.CreateInstance(CopyableBenchmarkObjectType4, ObjectToCopy4);
        }

        [Benchmark]
        public void Expression_CopyConstructor()
        {
            CopyableBenchmarkObject obj1 = ExpressionConstructor(ObjectToCopy);
            CopyableBenchmarkObject2 obj2 = ExpressionConstructor2(ObjectToCopy2);
            CopyableBenchmarkObject3 obj3 = ExpressionConstructor3(ObjectToCopy3);
            CopyableBenchmarkObject4 obj4 = ExpressionConstructor4(ObjectToCopy4);
        }

        [Benchmark]
        public void ImmediateType_CopyConstructor()
        {
            var obj1 = (CopyableBenchmarkObject)ImmediateTypeCopyable.Copy(ObjectToCopy);
            var obj2 = (CopyableBenchmarkObject2)ImmediateTypeCopyable2.Copy(ObjectToCopy2);
            var obj3 = (CopyableBenchmarkObject3)ImmediateTypeCopyable3.Copy(ObjectToCopy3);
            var obj4 = (CopyableBenchmarkObject4)ImmediateTypeCopyable4.Copy(ObjectToCopy4);
        }
    }
}