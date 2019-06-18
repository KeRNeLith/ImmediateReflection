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

Each benchmark consists in iterating over a set of objects (or mixed objects) and getting a property accessor to get or set the target property of the treated object.

In the case of the set of objects, it's an array of objects containing the same object type.

In the case of the set of mixed objects, it's an array of objects containing 2 types of objects with alternation between those 2 types.

## Results

### Get property value

|                                                Method |         Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------------------------------------ |-------------:|-----------:|-----------:|------:|--------:|
|                Reflection_PropertyGet_BenchmarkObject |    167.99 us |   1.913 us |   1.598 us |  1.00 |    0.00 |
|           ReflectionCache_PropertyGet_BenchmarkObject |     95.85 us |   1.678 us |   1.401 us |  0.57 |    0.01 |
|           HyperDescriptor_PropertyGet_BenchmarkObject |    973.49 us |  18.626 us |  23.555 us |  5.85 |    0.14 |
|                FastMember_PropertyGet_BenchmarkObject |     97.44 us |   1.939 us |   2.233 us |  0.58 |    0.01 |
|           FlashReflection_PropertyGet_BenchmarkObject |    407.91 us |   8.036 us |  13.646 us |  2.48 |    0.09 |
|       **ImmediateReflection_PropertyGet_BenchmarkObject** |     **80.17 us** |   **1.088 us** |   **1.018 us** |  **0.48** |    **0.01** |
|           WithFasterflect_PropertyGet_BenchmarkObject |     96.89 us |   1.798 us |   1.682 us |  0.58 |    0.01 |
|          Reflection_PropertyGet_Mixed_BenchmarkObject |    113.72 us |   2.191 us |   2.250 us |  0.67 |    0.02 |
|     ReflectionCache_PropertyGet_Mixed_BenchmarkObject |     50.63 us |   1.001 us |   1.498 us |  0.30 |    0.01 |
|     HyperDescriptor_PropertyGet_Mixed_BenchmarkObject |    906.34 us |  15.062 us |  14.089 us |  5.41 |    0.11 |
|          FastMember_PropertyGet_Mixed_BenchmarkObject |     84.61 us |   1.654 us |   2.092 us |  0.50 |    0.01 |
|     FlashReflection_PropertyGet_Mixed_BenchmarkObject |    443.51 us |   8.717 us |  10.705 us |  2.63 |    0.06 |
| **ImmediateReflection_PropertyGet_Mixed_BenchmarkObject** |     **78.81 us** |   **1.575 us** |   **2.920 us** |  **0.48** |    **0.01** |
|         Fasterflect_PropertyGet_Mixed_BenchmarkObject | 16,747.95 us | 324.643 us | 838.006 us | 97.43 |    4.71 |

### Set property value

|                                                Method |         Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------------------------------------ |-------------:|-----------:|-----------:|------:|--------:|
|                Reflection_PropertySet_BenchmarkObject |    263.10 us |   4.041 us |   3.583 us |  1.00 |    0.00 |
|           ReflectionCache_PropertySet_BenchmarkObject |    184.27 us |   2.934 us |   2.601 us |  0.70 |    0.01 |
|           HyperDescriptor_PropertySet_BenchmarkObject |  1,219.58 us |  24.302 us |  27.986 us |  4.63 |    0.15 |
|                FastMember_PropertySet_BenchmarkObject |     99.87 us |   1.834 us |   1.531 us |  0.38 |    0.01 |
|           FlashReflection_PropertySet_BenchmarkObject |    401.15 us |   6.793 us |   6.354 us |  1.53 |    0.03 |
|       **ImmediateReflection_PropertySet_BenchmarkObject** |     **91.76 us** |   **1.830 us** |   **3.057 us** |  **0.34** |    **0.02** |
|           WithFasterflect_PropertySet_BenchmarkObject |    105.34 us |   2.060 us |   3.145 us |  0.40 |    0.01 |
|          Reflection_PropertySet_Mixed_BenchmarkObject |    166.06 us |   3.044 us |   2.847 us |  0.63 |    0.01 |
|     ReflectionCache_PropertySet_Mixed_BenchmarkObject |     97.62 us |   1.862 us |   1.912 us |  0.37 |    0.01 |
|     HyperDescriptor_PropertySet_Mixed_BenchmarkObject |  1,083.02 us |  21.551 us |  41.522 us |  4.19 |    0.14 |
|          FastMember_PropertySet_Mixed_BenchmarkObject |     83.45 us |   1.499 us |   1.329 us |  0.32 |    0.01 |
|     FlashReflection_PropertySet_Mixed_BenchmarkObject |    423.68 us |   8.080 us |  10.506 us |  1.62 |    0.05 |
| **ImmediateReflection_PropertySet_Mixed_BenchmarkObject** |     **78.97 us** |   **1.494 us** |   **1.397 us** |  **0.30** |    **0.01** |
|         Fasterflect_PropertySet_Mixed_BenchmarkObject | 16,319.73 us | 320.488 us | 342.918 us | 61.95 |    1.81 |

---