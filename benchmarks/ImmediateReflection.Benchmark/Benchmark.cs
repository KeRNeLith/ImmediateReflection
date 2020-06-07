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
            IConfig config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#else
            IConfig config = DefaultConfig.Instance;
#endif

            // Constructor
            BenchmarkRunner.Run<DefaultConstructorBenchmark>(config);
            BenchmarkRunner.Run<MultiDefaultConstructorBenchmark>(config);
            BenchmarkRunner.Run<CopyConstructorBenchmark>(config);
            BenchmarkRunner.Run<MultiCopyConstructorBenchmark>(config);

            // Single get/set
            BenchmarkRunner.Run<FieldGetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyGetterBenchmark>(config);
            BenchmarkRunner.Run<FieldSetterBenchmark>(config);
            BenchmarkRunner.Run<PropertySetterBenchmark>(config);

            // Multi get/set
            BenchmarkRunner.Run<FieldMultiGetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyMultiGetterBenchmark>(config);
            BenchmarkRunner.Run<FieldMultiSetterBenchmark>(config);
            BenchmarkRunner.Run<PropertyMultiSetterBenchmark>(config);

            BenchmarkRunner.Run<PropertyGetterOverObjectsBenchmark>(config);
            BenchmarkRunner.Run<PropertySetterOverObjectsBenchmark>(config);

            // Attributes
            BenchmarkRunner.Run<GetAttributesBenchmark>(config);
        }
    }
}