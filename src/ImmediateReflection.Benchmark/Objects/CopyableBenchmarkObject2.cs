using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copyable benchmark object.
    /// </summary>
    public class CopyableBenchmarkObject2
    {
        public CopyableBenchmarkObject2(int value)
        {
            Property = value;
        }

        public CopyableBenchmarkObject2([NotNull] CopyableBenchmarkObject2 other)
        {
            Property = other.Property;
        }

        public int Property { get; }
    }
}