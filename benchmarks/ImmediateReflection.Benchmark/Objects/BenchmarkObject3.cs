namespace ImmediateReflection.Benchmark;

/// <summary>
/// Benchmark object.
/// </summary>
internal sealed class BenchmarkObject3
{
    // ReSharper disable once InconsistentNaming
    public string _benchmarkField = "Benchmark Field string";

    public double BenchmarkProperty { get; set; } = 22.2;
}