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

## Implementation details

These benchmarks have been done by making multiple get of attribute.

The property cache is implemented with an array storing all attributes of a given property.

The Property_ByImmediateReflection_GetAttribute benchmark by getting attributes from a standard `PropertyInfo` with ImmediateReflection helpers.

## Results

|                                      Method |        Mean |     Error |    StdDev | Ratio |
|-------------------------------------------- |------------:|----------:|----------:|------:|
|                       Property_GetAttribute | 3,015.71 ns | 4.9026 ns | 3.8277 ns |  1.00 |
|                  PropertyCache_GetAttribute |    75.72 ns | 0.2193 ns | 0.2052 ns |  0.03 |
|                     FastMember_GetAttribute | 3,018.84 ns | 7.0476 ns | 5.8850 ns |  1.00 |
|              **ImmediateProperty_GetAttribute** |    **45.16 ns** | **0.0626 ns** | **0.0555 ns** |  **0.01** |
| *Property_ByImmediateReflection_GetAttribute* |   *105.01 ns* | *0.2732 ns* | *0.2555 ns* |  *0.03* |

---

Results demonstrate that keeping the use of **ImmediateReflection** as much as possible provide the fastest access to `Type`, `FieldInfo` and `PropertyInfo` attributes.

It also shows that getting attributes from standard code is always possible but has a slightly higher cost.