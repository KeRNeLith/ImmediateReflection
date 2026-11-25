# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

## Configuration

```ini
BenchmarkDotNet v0.15.4, Windows 11 (10.0.26200.9711)
AMD Ryzen Threadripper PRO 5945WX 12-Cores 4.10GHz, 1 CPU, 24 logical and 12 physical cores
  [Host]             : .NET Framework 4.8.1 (4.8.9363.0), X64 RyuJIT VectorSize=256
  .NET 8.0           : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v3
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9363.0), X64 RyuJIT VectorSize=256
```

## Results

### Single constructor call

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_CopyConstructor | 3.311 ns | 0.0879 ns | 0.0976 ns | 1.00 | 0.04 |
| Activator_CopyConstructor | 197.833 ns | 3.0528 ns | 2.8556 ns | 59.79 | 1.90 |
| Expression_CopyConstructor | 4.846 ns | 0.0780 ns | 0.0730 ns | 1.46 | 0.05 |
| **ImmediateType_CopyConstructor** | **6.003 ns** | **0.1134 ns** | **0.1061 ns** | **1.81** | **0.06** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_CopyConstructor | 3.164 ns | 0.0778 ns | 0.0728 ns | 1.00 | 0.03 |
| Activator_CopyConstructor | 645.532 ns | 11.0261 ns | 9.7743 ns | 204.13 | 5.40 |
| Expression_CopyConstructor | 7.853 ns | 0.1311 ns | 0.1095 ns | 2.48 | 0.06 |
| **ImmediateType_CopyConstructor** | **7.067 ns** | **0.0559 ns** | **0.0437 ns** | **2.23** | **0.05** |

### Multiple constructor calls

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_CopyConstructor | 15.738 ns | 0.3105 ns | 0.4453 ns | 1.00 | 0.04 |
| Activator_CopyConstructor | 862.996 ns | 16.5907 ns | 14.7072 ns | 54.88 | 1.74 |
| Expression_CopyConstructor | 21.046 ns | 0.4237 ns | 0.4161 ns | 1.34 | 0.04 |
| **ImmediateType_CopyConstructor** | **21.764 ns** | **0.4524 ns** | **1.0486 ns** | **1.38** | **0.08** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_CopyConstructor | 8.480 ns | 0.1510 ns | 0.1339 ns | 1.00 | 0.02 |
| Activator_CopyConstructor | 2,542.604 ns | 47.9770 ns | 49.2689 ns | 299.89 | 7.26 |
| Expression_CopyConstructor | 29.722 ns | 0.2741 ns | 0.2430 ns | 3.51 | 0.06 |
| **ImmediateType_CopyConstructor** | **29.271 ns** | **0.6044 ns** | **0.5358 ns** | **3.45** | **0.08** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()` to do copy constructions.