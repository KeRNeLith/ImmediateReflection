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

## Implementation details

These benchmarks have been done by making multiple get of attribute.

The property cache is implemented with an array storing all attributes of a given property.

The Property_ByImmediateReflection_GetAttribute benchmark by getting attributes from a standard `PropertyInfo` with ImmediateReflection helpers.

## Results

|                                      Method |        Mean |     Error |     StdDev |      Median | Ratio | RatioSD |
|-------------------------------------------- |------------:|----------:|-----------:|------------:|------:|--------:|
|                       Property_GetAttribute | 3,247.60 ns | 71.388 ns |  73.310 ns | 3,232.89 ns |  1.00 |    0.00 |
|                  PropertyCache_GetAttribute |    83.00 ns |  1.643 ns |   2.017 ns |    82.96 ns |  0.03 |    0.00 |
|                     FastMember_GetAttribute | 3,398.33 ns | 67.154 ns | 140.175 ns | 3,396.13 ns |  1.07 |    0.05 |
|              **ImmediateProperty_GetAttribute** |    **51.51 ns** |  **1.037 ns** |   **1.732 ns** |    **51.30 ns** |  **0.02** |    **0.00** |
| *Property_ByImmediateReflection_GetAttribute* |   *163.84 ns* |  *3.268 ns* |   *8.435 ns* |   *160.93 ns* |  *0.05* |    *0.00* |

---

Results demonstrate that keeping the use of **ImmediateReflection** as much as possible provide the fastest access to `Type`, `FieldInfo` and `PropertyInfo` attributes.

It also shows that getting attributes from standard code is always possible but has a slightly higher cost.