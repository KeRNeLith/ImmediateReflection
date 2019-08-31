using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copyable benchmark object.
    /// </summary>
    public class CopyableBenchmarkObject4
    {
        public CopyableBenchmarkObject4(short value)
        {
            Property = value;
        }

        public CopyableBenchmarkObject4([NotNull] CopyableBenchmarkObject4 other)
        {
            Property = other.Property;
        }

        public short Property { get; }
    }
}