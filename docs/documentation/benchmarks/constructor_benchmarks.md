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
|        Direct_Constructor |  3.565 ns | 0.0482 ns | 0.0376 ns |  1.00 |    0.00 |
|     Activator_Constructor | 38.077 ns | 0.4029 ns | 0.3571 ns | 10.69 |    0.14 |
|    Expression_Constructor |  9.140 ns | 0.1282 ns | 0.1199 ns |  2.57 |    0.05 |
|    FastMember_Constructor |  5.499 ns | 0.0723 ns | 0.0676 ns |  1.55 |    0.02 |
| **ImmediateType_Constructor** |  **6.132 ns** | **0.0729 ns** | **0.0647 ns** |  **1.72** |    **0.02** |

### Multiple constructor calls

|                    Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|-------------------------- |----------:|----------:|----------:|------:|--------:|
|        Direct_Constructor |  11.59 ns | 0.1348 ns | 0.1126 ns |  1.00 |    0.00 |
|     Activator_Constructor | 183.74 ns | 1.5670 ns | 1.3085 ns | 15.85 |    0.21 |
|    Expression_Constructor |  37.32 ns | 0.3168 ns | 0.2809 ns |  3.22 |    0.03 |
|    FastMember_Constructor |  22.72 ns | 0.3246 ns | 0.3036 ns |  1.96 |    0.03 |
| **ImmediateType_Constructor** |  **24.63 ns** | **0.4941 ns** | **0.6068 ns** |  **2.11** |    **0.06** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()`.
Note that compared to FastMember the slight difference is explained by the fact that **ImmediateReflection** 
tries to keep as much as possible an API like the standard one involving further checks not done in FastMember.
