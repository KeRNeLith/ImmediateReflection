using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ImmediateReflection.Benchmark
{
    internal class Benchmark
    {
        /// <summary>
        /// Benchmark Main.
        /// </summary>
        public static void Main(string[] args)
        {
#if DEBUG
            // Allow to run even if in Debug target (But Benchmark must be run in RELEASE in the end
            IConfig config = DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator);
#else
            IConfig config = DefaultConfig.Instance;
#endif

            BenchmarkRunner.Run<FieldGetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyGetterBenchmark>(config);
            BenchmarkRunner.Run<FieldSetterBenchmark>(config);
            BenchmarkRunner.Run<PropertySetterBenchmark>(config);

            if (args.Length <= 0 || !args[0].Contains("FullBenchmark"))
                return;

            BenchmarkRunner.Run<FieldMultiGetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyMultiGetterBenchmark>(config);
            BenchmarkRunner.Run<FieldMultiSetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyMultiSetterBenchmark>(config);
        }
    }
}