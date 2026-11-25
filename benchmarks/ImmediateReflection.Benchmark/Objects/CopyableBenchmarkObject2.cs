namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copyable benchmark object.
/// </summary>
internal sealed class CopyableBenchmarkObject2
{
    public CopyableBenchmarkObject2(int value)
    {
        Property = value;
    }

    public CopyableBenchmarkObject2(CopyableBenchmarkObject2 other)
    {
        Property = other.Property;
    }

    public int Property { get; }
}