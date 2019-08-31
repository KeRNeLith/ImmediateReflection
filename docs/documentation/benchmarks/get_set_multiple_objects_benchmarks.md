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

Each benchmark consists in iterating over a set of objects (or mixed objects) and getting a property accessor to get or set the target property of the treated object.

In the case of the set of objects, it's an array of objects containing the same object type.

In the case of the set of mixed objects, it's an array of objects containing 2 types of objects with alternation between those 2 types.

## Results

### Get property value

|                                                Method |         Mean |       Error |      StdDev | Ratio | RatioSD |
|------------------------------------------------------ |-------------:|------------:|------------:|------:|--------:|
|                Reflection_PropertyGet_BenchmarkObject |    161.82 us |   2.8856 us |   2.6992 us |  1.00 |    0.00 |
|           ReflectionCache_PropertyGet_BenchmarkObject |     87.94 us |   0.9080 us |   0.8494 us |  0.54 |    0.01 |
|           HyperDescriptor_PropertyGet_BenchmarkObject |    827.82 us |   2.9013 us |   2.7138 us |  5.12 |    0.09 |
|                FastMember_PropertyGet_BenchmarkObject |     87.99 us |   1.2164 us |   1.0783 us |  0.54 |    0.01 |
|           FlashReflection_PropertyGet_BenchmarkObject |    437.09 us |   4.3506 us |   4.0696 us |  2.70 |    0.05 |
|       **ImmediateReflection_PropertyGet_BenchmarkObject** |     **74.13 us** |   **1.1393 us** |   **0.8895 us** |  **0.46** |    **0.01** |
|           WithFasterflect_PropertyGet_BenchmarkObject |     85.21 us |   0.5650 us |   0.5008 us |  0.53 |    0.01 |
|          Reflection_PropertyGet_Mixed_BenchmarkObject |    101.79 us |   1.4084 us |   1.2485 us |  0.63 |    0.01 |
|     ReflectionCache_PropertyGet_Mixed_BenchmarkObject |     44.76 us |   0.1915 us |   0.1698 us |  0.28 |    0.00 |
|     HyperDescriptor_PropertyGet_Mixed_BenchmarkObject |    766.16 us |  10.4081 us |   8.6912 us |  4.73 |    0.09 |
|          FastMember_PropertyGet_Mixed_BenchmarkObject |     76.63 us |   0.3050 us |   0.2704 us |  0.47 |    0.01 |
|     FlashReflection_PropertyGet_Mixed_BenchmarkObject |    463.26 us |   6.3032 us |   5.8961 us |  2.86 |    0.06 |
| **ImmediateReflection_PropertyGet_Mixed_BenchmarkObject** |     **67.88 us** |   **0.6324 us** |   **0.5606 us** |  **0.42** |    **0.01** |
|         Fasterflect_PropertyGet_Mixed_BenchmarkObject | 14,693.05 us | 330.8783 us | 570.7487 us | 90.35 |    4.35 |

### Set property value

|                                                Method |         Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------------------------------------ |-------------:|-----------:|-----------:|------:|--------:|
|                Reflection_PropertySet_BenchmarkObject |    235.83 us |  2.4116 us |  2.0138 us |  1.00 |    0.00 |
|           ReflectionCache_PropertySet_BenchmarkObject |    167.53 us |  1.7663 us |  1.5657 us |  0.71 |    0.01 |
|           HyperDescriptor_PropertySet_BenchmarkObject |  1,054.13 us | 11.7559 us | 10.4213 us |  4.47 |    0.04 |
|                FastMember_PropertySet_BenchmarkObject |     93.97 us |  0.8061 us |  0.6293 us |  0.40 |    0.00 |
|           FlashReflection_PropertySet_BenchmarkObject |    447.69 us |  7.8387 us |  7.3324 us |  1.90 |    0.03 |
|       **ImmediateReflection_PropertySet_BenchmarkObject** |     **78.97 us** |  **0.8561 us** |  **0.7149 us** |  **0.33** |    **0.00** |
|           WithFasterflect_PropertySet_BenchmarkObject |     92.46 us |  1.8772 us |  2.3740 us |  0.40 |    0.01 |
|          Reflection_PropertySet_Mixed_BenchmarkObject |    144.99 us |  2.3385 us |  2.0730 us |  0.61 |    0.01 |
|     ReflectionCache_PropertySet_Mixed_BenchmarkObject |     83.63 us |  0.4110 us |  0.3643 us |  0.35 |    0.00 |
|     HyperDescriptor_PropertySet_Mixed_BenchmarkObject |    953.34 us |  9.3168 us |  8.2591 us |  4.05 |    0.04 |
|          FastMember_PropertySet_Mixed_BenchmarkObject |     79.13 us |  0.7507 us |  0.7022 us |  0.34 |    0.00 |
|     FlashReflection_PropertySet_Mixed_BenchmarkObject |    464.35 us |  9.2689 us | 10.3024 us |  1.98 |    0.06 |
| **ImmediateReflection_PropertySet_Mixed_BenchmarkObject** |     **71.93 us** |  **0.8027 us** |  **0.7115 us** |  **0.30** |    **0.00** |
|         Fasterflect_PropertySet_Mixed_BenchmarkObject | 14,004.96 us | 69.2554 us | 57.8314 us | 59.39 |    0.66 |

---