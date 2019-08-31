using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copyable benchmark object.
    /// </summary>
    public class CopyableBenchmarkObject3
    {
        public CopyableBenchmarkObject3(double value)
        {
            Property = value;
        }

        public CopyableBenchmarkObject3([NotNull] CopyableBenchmarkObject3 other)
        {
            Property = other.Property;
        }

        public double Property { get; }
    }
}