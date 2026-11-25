# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

These benchmarks have been done by making multiple get or set on multiple types to avoid caching of the same operation.

## Configuration

```ini
BenchmarkDotNet v0.15.4, Windows 11 (10.0.26200.9711)
AMD Ryzen Threadripper PRO 5945WX 12-Cores 4.10GHz, 1 CPU, 24 logical and 12 physical cores
  [Host]             : .NET Framework 4.8.1 (4.8.9363.0), X64 RyuJIT VectorSize=256
  .NET 8.0           : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v3
  .NET Framework 4.8 : .NET Framework 4.8.1 (4.8.9363.0), X64 RyuJIT VectorSize=256
```

## Implementation details

The Field or Property cache implementations consider as a cache the fact of having the right `PropertyInfo` ready to use with GetValue/SetValue.

## Results

### Get a field value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Field | 1.680 ns | 0.0322 ns | 0.0330 ns | 1.00 | 0.03 |
| GetFieldInfo_Field | 164.911 ns | 2.4729 ns | 2.1922 ns | 98.17 | 2.25 |
| GetFieldInfoCache_Field | 119.919 ns | 1.1855 ns | 1.1089 ns | 71.39 | 1.50 |
| GetFastMember_Field | 61.077 ns | 0.8500 ns | 0.7950 ns | 36.36 | 0.83 |
| **GetImmediateField_Field** | **15.476 ns** | **0.3241 ns** | **0.4098 ns** | **9.21** | **0.30** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Field | 1.675 ns | 0.0259 ns | 0.0230 ns | 1.00 | 0.02 |
| GetFieldInfo_Field | 328.207 ns | 5.4586 ns | 4.2617 ns | 196.00 | 3.56 |
| GetFieldInfoCache_Field | 193.891 ns | 1.0852 ns | 0.9620 ns | 115.79 | 1.63 |
| GetFastMember_Field | 111.975 ns | 2.2296 ns | 3.5364 ns | 66.87 | 2.26 |
| **GetImmediateField_Field** | **19.532 ns** | **0.4105 ns** | **0.5041 ns** | **11.66** | **0.33** |

### Set a field value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Field | 1.909 ns | 0.0115 ns | 0.0107 ns | 1.00 | 0.01 |
| SetFieldInfo_Field | 177.080 ns | 2.7251 ns | 2.4157 ns | 92.77 | 1.32 |
| SetFieldInfoCache_Field | 135.020 ns | 2.7322 ns | 3.1464 ns | 70.73 | 1.65 |
| SetFastMember_Field | 62.582 ns | 1.2387 ns | 1.3768 ns | 32.79 | 0.73 |
| **SetImmediateField_Field** | **16.933 ns** | **0.3615 ns** | **0.4018 ns** | **8.87** | **0.21** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Field | 2.812 ns | 0.0291 ns | 0.0227 ns | 1.00 | 0.01 |
| SetFieldInfo_Field | 425.192 ns | 5.8437 ns | 5.4662 ns | 151.20 | 2.22 |
| SetFieldInfoCache_Field | 294.530 ns | 3.7197 ns | 3.4794 ns | 104.74 | 1.45 |
| SetFastMember_Field | 107.138 ns | 2.0060 ns | 3.1231 ns | 38.10 | 1.13 |
| **SetImmediateField_Field** | **21.123 ns** | **0.2910 ns** | **0.2272 ns** | **7.51** | **0.10** |

---

### Get a property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Property | 1.663 ns | 0.0256 ns | 0.0240 ns | 1.00 | 0.02 |
| GetDelegate_Property | 11.701 ns | 0.1432 ns | 0.1270 ns | 7.04 | 0.12 |
| GetDynamicDelegate_Property | 534.540 ns | 10.3967 ns | 9.7251 ns | 321.44 | 7.21 |
| GetPropertyInfo_Property | 111.400 ns | 0.9530 ns | 0.8448 ns | 66.99 | 1.05 |
| GetPropertyInfoCache_Property | 39.983 ns | 0.6476 ns | 0.6058 ns | 24.04 | 0.49 |
| GetSigilEmit_Property | 12.755 ns | 0.0938 ns | 0.1433 ns | 7.67 | 0.14 |
| GetExpression_Property | 13.150 ns | 0.2509 ns | 0.2576 ns | 7.91 | 0.19 |
| GetFastMember_Property | 63.446 ns | 0.7836 ns | 0.6946 ns | 38.15 | 0.67 |
| **GetImmediateProperty_Property** | **14.864 ns** | **0.2779 ns** | **0.2854 ns** | **8.94** | **0.21** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Property | 1.664 ns | 0.0156 ns | 0.0138 ns | 1.00 | 0.01 |
| GetDelegate_Property | 13.927 ns | 0.0955 ns | 0.0798 ns | 8.37 | 0.08 |
| GetDynamicDelegate_Property | 2,375.988 ns | 31.5114 ns | 29.4758 ns | 1,428.25 | 20.60 |
| GetPropertyInfo_Property | 631.828 ns | 8.0480 ns | 7.1343 ns | 379.80 | 5.13 |
| GetPropertyInfoCache_Property | 453.975 ns | 16.6905 ns | 49.2123 ns | 272.89 | 29.53 |
| GetSigilEmit_Property | 16.006 ns | 0.2290 ns | 0.2142 ns | 9.62 | 0.15 |
| GetExpression_Property | 34.938 ns | 0.3228 ns | 0.5983 ns | 21.00 | 0.39 |
| GetFastMember_Property | 130.637 ns | 2.5496 ns | 3.3152 ns | 78.53 | 2.05 |
| **GetImmediateProperty_Property** | **20.677 ns** | **0.2409 ns** | **0.2136 ns** | **12.43** | **0.16** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.GetValue` as reference).

### Set a property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Property | 1.944 ns | 0.0290 ns | 0.0271 ns | 1.00 | 0.02 |
| SetDelegate_Property | 9.655 ns | 0.1268 ns | 0.1186 ns | 4.97 | 0.09 |
| SetDynamicDelegate_Property | 546.484 ns | 8.0423 ns | 6.7157 ns | 281.12 | 5.05 |
| SetPropertyInfo_Property | 134.289 ns | 2.0264 ns | 1.8955 ns | 69.08 | 1.33 |
| SetPropertyInfoCache_Property | 62.872 ns | 0.6895 ns | 0.6113 ns | 32.34 | 0.53 |
| SetSigilEmit_Property | 9.696 ns | 0.1324 ns | 0.1174 ns | 4.99 | 0.09 |
| SetExpression_Property | 13.707 ns | 0.1896 ns | 0.2719 ns | 7.05 | 0.17 |
| SetFastMember_Property | 62.091 ns | 1.0958 ns | 0.9151 ns | 31.94 | 0.63 |
| **SetImmediateProperty_Property** | **14.323 ns** | **0.2940 ns** | **0.3020 ns** | **7.37** | **0.18** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Property | 3.108 ns | 0.0436 ns | 0.0340 ns | 1.00 | 0.01 |
| SetDelegate_Property | 13.982 ns | 0.1728 ns | 0.1616 ns | 4.50 | 0.07 |
| SetDynamicDelegate_Property | 2,717.388 ns | 32.4002 ns | 30.3072 ns | 874.32 | 13.14 |
| SetPropertyInfo_Property | 781.826 ns | 10.7453 ns | 9.5254 ns | 251.55 | 3.96 |
| SetPropertyInfoCache_Property | 582.295 ns | 6.3405 ns | 5.6207 ns | 187.35 | 2.62 |
| SetSigilEmit_Property | 13.892 ns | 0.1584 ns | 0.1482 ns | 4.47 | 0.07 |
| SetExpression_Property | 30.610 ns | 0.4111 ns | 0.3846 ns | 9.85 | 0.16 |
| SetFastMember_Property | 125.995 ns | 2.4529 ns | 3.2746 ns | 40.54 | 1.12 |
| **SetImmediateProperty_Property** | **19.934 ns** | **0.3358 ns** | **0.2977 ns** | **6.41** | **0.11** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.SetValue` as reference).

---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.