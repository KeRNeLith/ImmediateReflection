namespace ImmediateReflection.Benchmark;

/// <summary>
/// Copyable benchmark object.
/// </summary>
internal sealed class CopyableBenchmarkObject
{
    public CopyableBenchmarkObject(string str)
    {
        Property = str;
    }

    public CopyableBenchmarkObject(CopyableBenchmarkObject other)
    {
        Property = other.Property;
    }

    public string Property { get; }
}