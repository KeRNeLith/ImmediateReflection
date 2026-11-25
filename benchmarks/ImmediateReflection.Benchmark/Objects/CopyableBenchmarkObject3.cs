namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copyable benchmark object.
/// </summary>
internal sealed class CopyableBenchmarkObject3
{
    public CopyableBenchmarkObject3(double value)
    {
        Property = value;
    }

    public CopyableBenchmarkObject3(CopyableBenchmarkObject3 other)
    {
        Property = other.Property;
    }

    public double Property { get; }
}