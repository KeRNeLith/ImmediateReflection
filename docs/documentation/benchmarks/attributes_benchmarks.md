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

## Implementation details

These benchmarks have been done by making multiple get of attribute.

The property cache is implemented with an array storing all attributes of a given property.

The Property_ByImmediateReflection_GetAttribute benchmark by getting attributes from a standard `PropertyInfo` with ImmediateReflection helpers.

## Results

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Property_GetAttribute | 1,011.10 ns | 11.210 ns | 9.361 ns | 1.00 | 0.01 |
| PropertyCache_GetAttribute | 44.65 ns | 0.829 ns | 0.776 ns | 0.04 | 0.00 |
| FastMember_GetAttribute | 1,016.26 ns | 19.724 ns | 21.923 ns | 1.01 | 0.02 |
| **ImmediateProperty_GetAttribute** | **12.50 ns** | **0.193 ns** | **0.181 ns** | **0.01** | **0.00** |
| *Property_ByImmediateReflection_GetAttribute* | *43.95 ns* | *0.696 ns* | *0.651 ns* | *0.04* | *0.00* |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Property_GetAttribute | 3,095.36 ns | 55.037 ns | 51.482 ns | 1.000 | 0.02 |
| PropertyCache_GetAttribute | 68.98 ns | 1.402 ns | 1.773 ns | 0.022 | 0.00 |
| FastMember_GetAttribute | 3,082.28 ns | 44.513 ns | 41.638 ns | 0.996 | 0.02 |
| **ImmediateProperty_GetAttribute** | **28.79 ns** | **0.298 ns** | **0.264 ns** | **0.009** | **0.00** |
| *Property_ByImmediateReflection_GetAttribute* | *67.13 ns* | *1.358 ns* | *1.334 ns* | *0.022* | *0.00* |

---

Results demonstrate that keeping the use of **ImmediateReflection** as much as possible provide the fastest access to `Type`, `FieldInfo` and `PropertyInfo` attributes.

It also shows that getting attributes from standard code is always possible but has a slightly higher cost.