using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Copyable benchmark object.
    /// </summary>
    public class CopyableBenchmarkObject
    {
        public CopyableBenchmarkObject([NotNull] string str)
        {
            Property = str;
        }

        public CopyableBenchmarkObject([NotNull] CopyableBenchmarkObject other)
        {
            Property = other.Property;
        }

        public string Property { get; }
    }
}