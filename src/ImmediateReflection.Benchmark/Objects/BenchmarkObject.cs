namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Benchmark object.
    /// </summary>
    public class BenchmarkObject
    {
        // ReSharper disable once InconsistentNaming
        public int _benchmarkField = 42;

        public string BenchmarkProperty { get; set; } = "Benchmark Property string";
    }
}