# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

## Configuration

```ini
BenchmarkDotNet=v0.11.5
OS=Windows 10.0.17134.829 (1803/April2018Update/Redstone4)
Processor=Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
```

## Implementation details

These benchmarks have been done by making multiple get of attribute.

The property cache is implemented with an array storing all attributes of a given property.

The Property_ByImmediateReflection_GetAttribute benchmark by getting attributes from a standard `PropertyInfo` with ImmediateReflection helpers.

## Results

|                                      Method |        Mean |      Error |     StdDev | Ratio | RatioSD |
|-------------------------------------------- |------------:|-----------:|-----------:|------:|--------:|
|                       Property_GetAttribute | 3,131.45 ns | 28.8622 ns | 26.9977 ns |  1.00 |    0.00 |
|                  PropertyCache_GetAttribute |    81.15 ns |  1.6264 ns |  1.5214 ns |  0.03 |    0.00 |
|                     FastMember_GetAttribute | 3,123.90 ns | 60.3643 ns | 61.9897 ns |  1.00 |    0.02 |
|              **ImmediateProperty_GetAttribute** |    **48.07 ns** |  **0.2989 ns** |  **0.2649 ns** |  **0.02** |    **0.00** |
| *Property_ByImmediateReflection_GetAttribute* |   *105.46 ns* |  *1.0925 ns* |  *1.0219 ns* |  *0.03* |    *0.00* |

---

Results demonstrate that keeping the use of **ImmediateReflection** as much as possible provide the fastest access to `Type`, `FieldInfo` and `PropertyInfo` attributes.

It also shows that getting attributes from standard code is always possible but has a slightly higher cost.