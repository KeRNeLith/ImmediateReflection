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
|                Reflection_PropertyGet_BenchmarkObject |    153.24 us |   0.6956 us |   0.6167 us |  1.00 |    0.00 |
|           ReflectionCache_PropertyGet_BenchmarkObject |     86.55 us |   0.2897 us |   0.2710 us |  0.57 |    0.00 |
|           HyperDescriptor_PropertyGet_BenchmarkObject |    824.08 us |   4.8925 us |   4.3370 us |  5.38 |    0.04 |
|                FastMember_PropertyGet_BenchmarkObject |     89.39 us |   0.7090 us |   0.6632 us |  0.58 |    0.01 |
|           FlashReflection_PropertyGet_BenchmarkObject |    441.63 us |   2.9188 us |   2.5875 us |  2.88 |    0.02 |
|       **ImmediateReflection_PropertyGet_BenchmarkObject** |     **74.21 us** |   **0.4152 us** |   **0.3681 us** |  **0.48** |    **0.00** |
|           WithFasterflect_PropertyGet_BenchmarkObject |     85.62 us |   0.8516 us |   0.7111 us |  0.56 |    0.00 |
|          Reflection_PropertyGet_Mixed_BenchmarkObject |     99.78 us |   0.2330 us |   0.1945 us |  0.65 |    0.00 |
|     ReflectionCache_PropertyGet_Mixed_BenchmarkObject |     44.08 us |   0.2692 us |   0.2518 us |  0.29 |    0.00 |
|     HyperDescriptor_PropertyGet_Mixed_BenchmarkObject |    760.92 us |  11.8462 us |  10.5013 us |  4.97 |    0.07 |
|          FastMember_PropertyGet_Mixed_BenchmarkObject |     75.34 us |   1.2642 us |   1.1207 us |  0.49 |    0.01 |
|     FlashReflection_PropertyGet_Mixed_BenchmarkObject |    452.75 us |   2.9525 us |   2.6174 us |  2.95 |    0.02 |
| **ImmediateReflection_PropertyGet_Mixed_BenchmarkObject** |     **69.06 us** |   **0.9011 us** |   **0.7988 us** |  **0.45** |    **0.01** |
|         Fasterflect_PropertyGet_Mixed_BenchmarkObject | 14,456.22 us | 218.8338 us | 193.9904 us | 94.34 |    1.42 |

### Set property value

|                                                Method |         Mean |       Error |      StdDev | Ratio | RatioSD |
|------------------------------------------------------ |-------------:|------------:|------------:|------:|--------:|
|                Reflection_PropertySet_BenchmarkObject |    236.29 us |   1.3175 us |   1.1679 us |  1.00 |    0.00 |
|           ReflectionCache_PropertySet_BenchmarkObject |    165.01 us |   0.5586 us |   0.5225 us |  0.70 |    0.00 |
|           HyperDescriptor_PropertySet_BenchmarkObject |  1,060.21 us |  21.1042 us |  18.7083 us |  4.49 |    0.07 |
|                FastMember_PropertySet_BenchmarkObject |     95.04 us |   1.3488 us |   1.1957 us |  0.40 |    0.01 |
|           FlashReflection_PropertySet_BenchmarkObject |    454.48 us |   6.8928 us |   5.7558 us |  1.92 |    0.03 |
|       **ImmediateReflection_PropertySet_BenchmarkObject** |     **76.30 us** |   **0.6909 us** |   **0.6124 us** |  **0.32** |    **0.00** |
|           WithFasterflect_PropertySet_BenchmarkObject |     89.78 us |   0.8892 us |   0.8318 us |  0.38 |    0.00 |
|          Reflection_PropertySet_Mixed_BenchmarkObject |    141.18 us |   0.3571 us |   0.2982 us |  0.60 |    0.00 |
|     ReflectionCache_PropertySet_Mixed_BenchmarkObject |     83.09 us |   0.2719 us |   0.2410 us |  0.35 |    0.00 |
|     HyperDescriptor_PropertySet_Mixed_BenchmarkObject |    945.40 us |  17.6039 us |  15.6054 us |  4.00 |    0.06 |
|          FastMember_PropertySet_Mixed_BenchmarkObject |     78.91 us |   0.5266 us |   0.4668 us |  0.33 |    0.00 |
|     FlashReflection_PropertySet_Mixed_BenchmarkObject |    448.82 us |   1.1675 us |   1.0349 us |  1.90 |    0.01 |
| **ImmediateReflection_PropertySet_Mixed_BenchmarkObject** |     **69.95 us** |   **0.4296 us** |   **0.3809 us** |  **0.30** |    **0.00** |
|         Fasterflect_PropertySet_Mixed_BenchmarkObject | 14,236.33 us | 185.1234 us | 154.5864 us | 60.26 |    0.66 |

---