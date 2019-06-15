using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Base benchmark class with object arrays.
    /// </summary>
    public abstract class ObjectsBenchmarkBase : BenchmarkBase
    {
        #region Benchmark objects

        protected class ObjectsBenchmarkObject1
        {
            public string StringProperty { get; set; }

            public uint[] UIntArray { get; set; }
        }

        protected class ObjectsBenchmarkObject2
        {
            public int IntProperty { get; set; }

            public long[] LongArray { get; set; }
        }

        [NotNull]
        protected static readonly string UIntArrayPropertyName = nameof(ObjectsBenchmarkObject1.UIntArray);

        [NotNull]
        protected static readonly PropertyInfo UIntArrayPropertyInfo = typeof(ObjectsBenchmarkObject1).GetProperty(UIntArrayPropertyName) 
                                                                       ?? throw new InvalidOperationException("Property does not exist.");

        #endregion

        /// <summary>
        /// Generates <paramref name="nbObjects"/> <see cref="BenchmarkObject"/> instances.
        /// </summary>
        /// <param name="nbObjects">Number of object to generate.</param>
        /// <returns>Generated objects.</returns>
        private static object[] GenerateBenchmarkObjects(int nbObjects)
        {
            return Enumerable
                .Range(1, nbObjects)
                .Select(i => (object)new ObjectsBenchmarkObject1
                {
                    StringProperty = $"string {i}",
                    UIntArray = new []{ 1u, 2u, 3u }
                })
                .ToArray();
        }

        /// <summary>
        /// Generates <paramref name="nbObjects"/> of mixed <see cref="BenchmarkObject"/> and <see cref="BenchmarkObject2"/> instances.
        /// </summary>
        /// <param name="nbObjects">Number of object to generate.</param>
        /// <returns>Generated objects.</returns>
        private static object[] GenerateMixedBenchmarkObjects(int nbObjects)
        {
            return Enumerable
                .Range(1, nbObjects)
                .Select(
                    i => i % 2 == 0
                        ? (object)new ObjectsBenchmarkObject1
                        {
                            StringProperty = $"string {i}",
                            UIntArray = new[] { 1u, 2u, 3u }
                        }
                        : new ObjectsBenchmarkObject2
                        {
                            IntProperty = i,
                            LongArray = new []{ 1L, 2L, 3L }
                        })
                .ToArray();
        }

        private const int NbObjects = 1000;

        /// <summary>
        /// Array of <see cref="BenchmarkObject"/> objects.
        /// </summary>
        [NotNull, ItemNotNull]
        protected static readonly object[] BenchmarkObjects = GenerateBenchmarkObjects(NbObjects);

        /// <summary>
        /// Array of <see cref="BenchmarkObject"/> and <see cref="BenchmarkObject2"/> objects.
        /// </summary>
        [NotNull, ItemNotNull]
        protected static readonly object[] BenchmarkMixedObjects = GenerateMixedBenchmarkObjects(NbObjects);
    }
}