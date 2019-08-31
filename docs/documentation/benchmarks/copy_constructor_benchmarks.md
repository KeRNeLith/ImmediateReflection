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

|                        Method |       Mean |      Error |     StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|-----------:|-----------:|-------:|--------:|
|        Direct_CopyConstructor |   2.953 ns |  0.1308 ns |  0.2488 ns |   1.00 |    0.00 |
|     Activator_CopyConstructor | 566.443 ns | 11.2951 ns | 13.0075 ns | 185.85 |   19.21 |
|    Expression_CopyConstructor |   7.727 ns |  0.2140 ns |  0.2548 ns |   2.54 |    0.21 |
| **ImmediateType_CopyConstructor** |   **6.313 ns** |  **0.1977 ns** |  **0.4774 ns** |   **2.18** |    **0.27** |

### Multiple constructor calls

|                        Method |         Mean |      Error |     StdDev |  Ratio | RatioSD |
|------------------------------ |-------------:|-----------:|-----------:|-------:|--------:|
|        Direct_CopyConstructor |     8.716 ns |  0.1260 ns |  0.1178 ns |   1.00 |    0.00 |
|     Activator_CopyConstructor | 2,255.524 ns | 18.1062 ns | 16.9366 ns | 258.83 |    4.22 |
|    Expression_CopyConstructor |    30.300 ns |  0.1712 ns |  0.1602 ns |   3.48 |    0.05 |
| **ImmediateType_CopyConstructor** |    **23.860 ns** |  **0.4966 ns** |  **0.4147 ns** |   **2.74** |    **0.05** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()` to do copy constructions.