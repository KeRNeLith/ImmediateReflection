# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

## Configuration

```ini
BenchmarkDotNet=v0.11.5
OS=Windows 10.0.18362
Processor=Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.3815.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.3815.0
```

## Results

### Single constructor call

|                    Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------------- |----------:|----------:|----------:|------:|--------:|
|        Direct_Constructor |  3.775 ns | 0.1453 ns | 0.1784 ns |  1.00 |    0.00 |
|     Activator_Constructor | 40.120 ns | 0.9452 ns | 1.9308 ns | 10.83 |    0.73 |
|    Expression_Constructor |  9.413 ns | 0.1010 ns | 0.0843 ns |  2.50 |    0.12 |
|    FastMember_Constructor |  5.523 ns | 0.1383 ns | 0.1294 ns |  1.47 |    0.07 |
| **ImmediateType_Constructor** |  **6.081 ns** | **0.1902 ns** |  **5.938 ns** |  **1.62** |    **0.12** |

### Multiple constructor calls

|                    Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------------- |----------:|----------:|----------:|------:|--------:|
|        Direct_Constructor |  10.06 ns | 0.0308 ns | 0.0273 ns |  1.00 |    0.00 |
|     Activator_Constructor | 188.84 ns | 1.7316 ns | 1.5350 ns | 18.77 |    0.15 |
|    Expression_Constructor |  35.45 ns | 0.4622 ns | 0.4098 ns |  3.52 |    0.04 |
|    FastMember_Constructor |  23.54 ns | 0.4225 ns | 0.3746 ns |  2.34 |    0.04 |
| ImmediateType_Constructor |  **23.35 ns** | **0.3559 ns** | **0.2778 ns** |  2.32** |    **0.03** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()`.
Note that compared to FastMember the slight difference is explained by the fact that **ImmediateReflection** 
tries to keep as much as possible an API like the standard one involving further checks not done in FastMember.
