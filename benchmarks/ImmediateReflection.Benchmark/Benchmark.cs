using System;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ImmediateReflection.Benchmark;

internal static class Benchmark
{
    private const string FastModeArgument = "--fast";

    /// <summary>
    /// Benchmark Main.
    /// </summary>
    /// <remarks>
    /// Usage (from the benchmark project folder, always in Release):
    ///   dotnet run -c Release -f net8.0                                 Full run (accurate, used to update the documentation).
    ///   dotnet run -c Release -f net8.0 -- --fast                       Fast run (short jobs, less accurate, just to check a trend).
    ///   dotnet run -c Release -f net8.0 -- --filter "*FieldGetter*"     Only run matching benchmarks (can be combined with --fast).
    ///
    /// Each benchmark is run on .NET Framework 4.8 and .NET 8, the host framework (-f) does not matter.
    /// Any other argument is forwarded to BenchmarkDotNet. Do not use its --job argument, it adds a job on top of the ones above.
    /// </remarks>
    public static void Main(string[] args)
    {
        bool isFast = args.Contains(FastModeArgument);
        args = args.Where(arg => arg != FastModeArgument).ToArray();

        // Without arguments run every benchmark of the assembly
        if (args.Length == 0)
        {
            args = ["--filter", "*"];
        }

        Console.WriteLine(isFast ? "Fast mode (short jobs)." : "Full mode (default jobs).");

#if DEBUG
        // Allow to run even if in Debug target (But Benchmark must be run in RELEASE in the end
        IConfig config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);
#else
        IConfig config = DefaultConfig.Instance;
#endif

        Job baseJob = isFast ? Job.ShortRun : Job.Default;
        config = config
            .AddJob(baseJob.WithRuntime(ClrRuntime.Net48).WithId(".NET Framework 4.8"))
            .AddJob(baseJob.WithRuntime(CoreRuntime.Core80).WithId(".NET 8.0"));

        BenchmarkSwitcher.FromAssembly(typeof(Benchmark).Assembly).Run(args, config);
    }
}
