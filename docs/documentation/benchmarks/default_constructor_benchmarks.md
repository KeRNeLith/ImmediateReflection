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
| Direct_Constructor | 2.873 ns | 0.0518 ns | 0.0432 ns | 1.00 | 0.02 |
| Activator_Constructor | 7.451 ns | 0.1070 ns | 0.1001 ns | 2.59 | 0.05 |
| Expression_Constructor | 3.848 ns | 0.0981 ns | 0.1309 ns | 1.34 | 0.05 |
| FastMember_Constructor | 4.210 ns | 0.1084 ns | 0.0906 ns | 1.47 | 0.04 |
| **ImmediateType_Constructor** | **5.297 ns** | **0.1160 ns** | **0.1085 ns** | **1.84** | **0.05** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_Constructor | 3.209 ns | 0.0693 ns | 0.0648 ns | 1.00 | 0.03 |
| Activator_Constructor | 35.288 ns | 0.3935 ns | 0.3680 ns | 11.00 | 0.24 |
| Expression_Constructor | 9.407 ns | 0.1929 ns | 0.1805 ns | 2.93 | 0.08 |
| FastMember_Constructor | 7.377 ns | 0.0996 ns | 0.0883 ns | 2.30 | 0.05 |
| **ImmediateType_Constructor** | **8.094 ns** | **0.1751 ns** | **0.1638 ns** | **2.52** | **0.07** |

### Multiple constructor calls

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_Constructor | 11.74 ns | 0.246 ns | 0.242 ns | 1.00 | 0.03 |
| Activator_Constructor | 27.60 ns | 0.541 ns | 0.532 ns | 2.35 | 0.06 |
| Expression_Constructor | 14.61 ns | 0.314 ns | 0.516 ns | 1.24 | 0.05 |
| FastMember_Constructor | 19.57 ns | 0.409 ns | 0.560 ns | 1.67 | 0.06 |
| **ImmediateType_Constructor** | **23.15 ns** | **0.393 ns** | **0.367 ns** | **1.97** | **0.05** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Direct_Constructor | 10.32 ns | 0.159 ns | 0.149 ns | 1.00 | 0.02 |
| Activator_Constructor | 145.03 ns | 2.570 ns | 2.146 ns | 14.05 | 0.28 |
| Expression_Constructor | 34.41 ns | 0.668 ns | 0.656 ns | 3.33 | 0.08 |
| FastMember_Constructor | 27.25 ns | 0.214 ns | 0.189 ns | 2.64 | 0.04 |
| **ImmediateType_Constructor** | **36.61 ns** | **0.548 ns** | **0.458 ns** | **3.55** | **0.07** |

---

Results demonstrate that **ImmediateReflection** is really faster than using the classic `Activator.CreateInstance()`.
Note that compared to FastMember the slight difference is explained by the fact that **ImmediateReflection** 
tries to keep as much as possible an API like the standard one involving further checks not done in FastMember.
