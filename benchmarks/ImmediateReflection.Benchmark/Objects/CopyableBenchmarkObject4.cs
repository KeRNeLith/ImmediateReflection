namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copyable benchmark object.
/// </summary>
internal sealed class CopyableBenchmarkObject4
{
    public CopyableBenchmarkObject4(short value)
    {
        Property = value;
    }

    public CopyableBenchmarkObject4(CopyableBenchmarkObject4 other)
    {
        Property = other.Property;
    }

    public short Property { get; }
}