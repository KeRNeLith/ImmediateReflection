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

|                                      Method |        Mean |      Error |      StdDev | Ratio | RatioSD |
|-------------------------------------------- |------------:|-----------:|------------:|------:|--------:|
|                       Property_GetAttribute | 3,189.22 ns | 78.6381 ns | 102.2517 ns |  1.00 |    0.00 |
|                  PropertyCache_GetAttribute |    76.40 ns |  0.6781 ns |   0.5294 ns |  0.02 |    0.00 |
|                     FastMember_GetAttribute | 3,188.79 ns | 62.4070 ns |  66.7748 ns |  0.99 |    0.03 |
|              **ImmediateProperty_GetAttribute** |    **75.92 ns** |  **0.3393 ns** |   **0.3174 ns** |  **0.02** |    **0.00** |
| *Property_ByImmediateReflection_GetAttribute* |   *111.50 ns* |  *1.2293 ns* |   *1.1498 ns* |  *0.03* |    *0.00* |

---

Results demonstrate that keeping the use of **ImmediateReflection** as much as possible provide the fastest access to `Type`, `FieldInfo` and `PropertyInfo` attributes.

It also shows that getting attributes from standard code is always possible but has a slightly higher cost.