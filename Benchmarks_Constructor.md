# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

## Configuration

```ini
BenchmarkDotNet=v0.11.5
OS=Windows 10.0.17134.765 (1803/April2018Update/Redstone4)
Processor=Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
```

## Results

* Single constructor call *

|                    Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------------- |----------:|----------:|----------:|------:|--------:|
|        Direct_Constructor |  3.828 ns | 0.1630 ns | 0.2538 ns |  1.00 |    0.00 |
|     Activator_Constructor | 41.936 ns | 0.8829 ns | 0.9447 ns | 10.84 |    0.73 |
|    Expression_Constructor | 11.170 ns | 0.2850 ns | 0.4352 ns |  2.93 |    0.22 |
|    FastMember_Constructor |  8.535 ns | 0.1685 ns | 0.1655 ns |  2.20 |    0.15 |
| **ImmediateType_Constructor** |  **6.911 ns** | **0.1938 ns** | **0.1903 ns** |  **1.78** |    **0.13** |

* Multiple constructor calls*

|                    Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------------- |----------:|----------:|----------:|------:|--------:|
|        Direct_Constructor |  14.32 ns | 0.3072 ns | 0.4205 ns |  1.00 |    0.00 |
|     Activator_Constructor | 210.04 ns | 2.8246 ns | 2.6422 ns | 14.55 |    0.48 |
|    Expression_Constructor |  44.76 ns | 0.7928 ns | 0.7028 ns |  3.11 |    0.11 |
|    FastMember_Constructor |  24.61 ns | 0.5150 ns | 0.6697 ns |  1.72 |    0.07 |
| **ImmediateType_Constructor** |  **26.29** ns | **0.5082 ns** | **0.4754 ns** |  **1.82** |    **0.05** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()`.