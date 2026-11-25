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

Each benchmark consists in iterating over a set of objects (or mixed objects) and getting a property accessor to get or set the target property of the treated object.

In the case of the set of objects, it's an array of objects containing the same object type.

In the case of the set of mixed objects, it's an array of objects containing 2 types of objects with alternation between those 2 types.

## Results

### Get property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Reflection_PropertyGet_BenchmarkObject | 26.257 μs | 0.3960 μs | 0.6725 μs | 1.00 | 0.04 |
| ReflectionCache_PropertyGet_BenchmarkObject | 7.909 μs | 0.0913 μs | 0.0854 μs | 0.30 | 0.01 |
| TypeDescriptor_PropertyGet_BenchmarkObject | 313.648 μs | 5.9876 μs | 6.1489 μs | 11.95 | 0.37 |
| FastMember_PropertyGet_BenchmarkObject | 42.690 μs | 0.5062 μs | 0.4227 μs | 1.63 | 0.04 |
| FlashReflection_PropertyGet_BenchmarkObject | 231.105 μs | 3.0185 μs | 2.8235 μs | 8.81 | 0.24 |
| **ImmediateReflection_PropertyGet_BenchmarkObject** | **23.398 μs** | **0.4597 μs** | **0.4919 μs** | **0.89** | **0.03** |
| WithFasterflect_PropertyGet_BenchmarkObject | 44.740 μs | 0.7099 μs | 0.6640 μs | 1.70 | 0.05 |
| Reflection_PropertyGet_Mixed_BenchmarkObject | 20.041 μs | 0.3548 μs | 0.3319 μs | 0.76 | 0.02 |
| ReflectionCache_PropertyGet_Mixed_BenchmarkObject | 4.036 μs | 0.0280 μs | 0.0234 μs | 0.15 | 0.00 |
| TypeDescriptor_PropertyGet_Mixed_BenchmarkObject | 310.880 μs | 6.1243 μs | 9.5348 μs | 11.85 | 0.46 |
| FastMember_PropertyGet_Mixed_BenchmarkObject | 39.011 μs | 0.7716 μs | 0.7924 μs | 1.49 | 0.05 |
| FlashReflection_PropertyGet_Mixed_BenchmarkObject | 240.995 μs | 2.9788 μs | 2.7864 μs | 9.18 | 0.25 |
| **ImmediateReflection_PropertyGet_Mixed_BenchmarkObject** | **23.142 μs** | **0.2718 μs** | **0.2409 μs** | **0.88** | **0.02** |
| Fasterflect_PropertyGet_Mixed_BenchmarkObject | 4,109.957 μs | 51.3667 μs | 48.0484 μs | 156.62 | 4.27 |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Reflection_PropertyGet_BenchmarkObject | 174.484 μs | 1.9912 μs | 1.6628 μs | 1.00 | 0.01 |
| ReflectionCache_PropertyGet_BenchmarkObject | 98.385 μs | 1.1965 μs | 1.1192 μs | 0.56 | 0.01 |
| TypeDescriptor_PropertyGet_BenchmarkObject | 681.711 μs | 13.1285 μs | 18.8285 μs | 3.91 | 0.11 |
| FastMember_PropertyGet_BenchmarkObject | 68.222 μs | 1.0622 μs | 0.8870 μs | 0.39 | 0.01 |
| FlashReflection_PropertyGet_BenchmarkObject | 380.610 μs | 5.8341 μs | 5.4573 μs | 2.18 | 0.04 |
| **ImmediateReflection_PropertyGet_BenchmarkObject** | **42.911 μs** | **0.8426 μs** | **1.6031 μs** | **0.25** | **0.01** |
| WithFasterflect_PropertyGet_BenchmarkObject | 70.534 μs | 0.6255 μs | 1.2051 μs | 0.40 | 0.01 |
| Reflection_PropertyGet_Mixed_BenchmarkObject | 98.427 μs | 1.2670 μs | 1.1851 μs | 0.56 | 0.01 |
| ReflectionCache_PropertyGet_Mixed_BenchmarkObject | 49.708 μs | 0.4329 μs | 0.3615 μs | 0.28 | 0.00 |
| TypeDescriptor_PropertyGet_Mixed_BenchmarkObject | 584.832 μs | 7.5651 μs | 7.0764 μs | 3.35 | 0.05 |
| FastMember_PropertyGet_Mixed_BenchmarkObject | 60.535 μs | 0.5792 μs | 0.5418 μs | 0.35 | 0.00 |
| FlashReflection_PropertyGet_Mixed_BenchmarkObject | 401.419 μs | 7.7399 μs | 8.2816 μs | 2.30 | 0.05 |
| **ImmediateReflection_PropertyGet_Mixed_BenchmarkObject** | **37.690 μs** | **0.5735 μs** | **0.5365 μs** | **0.22** | **0.00** |
| Fasterflect_PropertyGet_Mixed_BenchmarkObject | 12,174.064 μs | 240.8539 μs | 295.7902 μs | 69.78 | 1.78 |

### Set property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Reflection_PropertySet_BenchmarkObject | 30.907 μs | 0.2662 μs | 0.2490 μs | 1.00 | 0.01 |
| ReflectionCache_PropertySet_BenchmarkObject | 13.771 μs | 0.2291 μs | 0.2031 μs | 0.45 | 0.01 |
| TypeDescriptor_PropertySet_BenchmarkObject | 376.450 μs | 5.8464 μs | 5.4687 μs | 12.18 | 0.20 |
| FastMember_PropertySet_BenchmarkObject | 43.868 μs | 0.6959 μs | 0.6169 μs | 1.42 | 0.02 |
| FlashReflection_PropertySet_BenchmarkObject | 233.639 μs | 3.9423 μs | 3.4948 μs | 7.56 | 0.12 |
| **ImmediateReflection_PropertySet_BenchmarkObject** | **25.924 μs** | **0.2789 μs** | **0.2472 μs** | **0.84** | **0.01** |
| WithFasterflect_PropertySet_BenchmarkObject | 47.313 μs | 0.9138 μs | 0.8975 μs | 1.53 | 0.03 |
| Reflection_PropertySet_Mixed_BenchmarkObject | 22.836 μs | 0.3295 μs | 0.2921 μs | 0.74 | 0.01 |
| ReflectionCache_PropertySet_Mixed_BenchmarkObject | 7.057 μs | 0.0879 μs | 0.0734 μs | 0.23 | 0.00 |
| TypeDescriptor_PropertySet_Mixed_BenchmarkObject | 342.120 μs | 5.6928 μs | 5.0465 μs | 11.07 | 0.18 |
| FastMember_PropertySet_Mixed_BenchmarkObject | 39.098 μs | 0.7307 μs | 0.7504 μs | 1.27 | 0.03 |
| FlashReflection_PropertySet_Mixed_BenchmarkObject | 236.979 μs | 3.1933 μs | 2.9870 μs | 7.67 | 0.11 |
| **ImmediateReflection_PropertySet_Mixed_BenchmarkObject** | **23.920 μs** | **0.3938 μs** | **0.3683 μs** | **0.77** | **0.01** |
| Fasterflect_PropertySet_Mixed_BenchmarkObject | 4,054.556 μs | 66.9264 μs | 59.3285 μs | 131.20 | 2.12 |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| Reflection_PropertySet_BenchmarkObject | 218.942 μs | 4.2176 μs | 3.9452 μs | 1.00 | 0.02 |
| ReflectionCache_PropertySet_BenchmarkObject | 159.224 μs | 2.1351 μs | 1.7829 μs | 0.73 | 0.01 |
| TypeDescriptor_PropertySet_BenchmarkObject | 947.609 μs | 51.9340 μs | 153.1287 μs | 4.33 | 0.70 |
| FastMember_PropertySet_BenchmarkObject | 70.689 μs | 1.4064 μs | 1.0980 μs | 0.32 | 0.01 |
| FlashReflection_PropertySet_BenchmarkObject | 393.641 μs | 7.6811 μs | 8.5376 μs | 1.80 | 0.05 |
| **ImmediateReflection_PropertySet_BenchmarkObject** | **42.224 μs** | **0.5932 μs** | **0.5549 μs** | **0.19** | **0.00** |
| WithFasterflect_PropertySet_BenchmarkObject | 72.734 μs | 1.3370 μs | 1.2507 μs | 0.33 | 0.01 |
| Reflection_PropertySet_Mixed_BenchmarkObject | 133.134 μs | 2.5690 μs | 3.2489 μs | 0.61 | 0.02 |
| ReflectionCache_PropertySet_Mixed_BenchmarkObject | 79.148 μs | 1.2089 μs | 1.0095 μs | 0.36 | 0.01 |
| TypeDescriptor_PropertySet_Mixed_BenchmarkObject | 694.103 μs | 13.5938 μs | 25.1970 μs | 3.17 | 0.13 |
| FastMember_PropertySet_Mixed_BenchmarkObject | 59.076 μs | 0.6394 μs | 0.5981 μs | 0.27 | 0.01 |
| FlashReflection_PropertySet_Mixed_BenchmarkObject | 421.187 μs | 8.2889 μs | 7.3479 μs | 1.92 | 0.05 |
| **ImmediateReflection_PropertySet_Mixed_BenchmarkObject** | **38.337 μs** | **0.5772 μs** | **0.5399 μs** | **0.18** | **0.00** |
| Fasterflect_PropertySet_Mixed_BenchmarkObject | 12,201.365 μs | 174.8862 μs | 163.5886 μs | 55.75 | 1.21 |

---