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

The Field or Property cache implementations consider as a cache the fact of having the right `PropertyInfo` ready to use with GetValue/SetValue.

## Results

### Get a field value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Field | 2.132 ns | 0.0500 ns | 0.1475 ns | 1.00 | 0.10 |
| GetFieldInfo_Field | 54.608 ns | 1.7228 ns | 5.0527 ns | 25.74 | 3.02 |
| GetFieldInfoCache_Field | 39.251 ns | 1.5068 ns | 4.4429 ns | 18.50 | 2.48 |
| GetFastMember_Field | 19.781 ns | 0.6826 ns | 2.0127 ns | 9.32 | 1.16 |
| **GetImmediateField_Field** | **5.592 ns** | **0.1391 ns** | **0.3386 ns** | **2.64** | **0.25** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Field | 1.662 ns | 0.0332 ns | 0.0925 ns | 1.00 | 0.08 |
| GetFieldInfo_Field | 110.691 ns | 3.9028 ns | 11.5075 ns | 66.79 | 7.82 |
| GetFieldInfoCache_Field | 59.468 ns | 2.0769 ns | 6.1239 ns | 35.88 | 4.17 |
| GetFastMember_Field | 26.620 ns | 0.9029 ns | 2.5170 ns | 16.06 | 1.75 |
| **GetImmediateField_Field** | **4.405 ns** | **0.1057 ns** | **0.0989 ns** | **2.66** | **0.16** |

### Set a field value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Field | 1.671 ns | 0.0147 ns | 0.0145 ns | 1.00 | 0.01 |
| SetFieldInfo_Field | 45.179 ns | 0.4532 ns | 0.4017 ns | 27.03 | 0.32 |
| SetFieldInfoCache_Field | 35.066 ns | 0.6594 ns | 0.6168 ns | 20.98 | 0.40 |
| SetFastMember_Field | 17.670 ns | 0.3511 ns | 0.3903 ns | 10.57 | 0.24 |
| **SetImmediateField_Field** | **4.521 ns** | **0.0995 ns** | **0.1106 ns** | **2.71** | **0.07** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Field | 1.411 ns | 0.0204 ns | 0.0191 ns | 1.00 | 0.02 |
| SetFieldInfo_Field | 108.350 ns | 1.2394 ns | 1.1593 ns | 76.79 | 1.28 |
| SetFieldInfoCache_Field | 75.953 ns | 1.3994 ns | 1.2406 ns | 53.83 | 1.10 |
| SetFastMember_Field | 24.493 ns | 0.4616 ns | 0.4092 ns | 17.36 | 0.36 |
| **SetImmediateField_Field** | **3.953 ns** | **0.0742 ns** | **0.0694 ns** | **2.80** | **0.06** |

---

### Get a property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Property | 1.658 ns | 0.0103 ns | 0.0091 ns | 1.00 | 0.01 |
| GetDelegate_Property | 3.606 ns | 0.0666 ns | 0.0623 ns | 2.18 | 0.04 |
| GetDynamicDelegate_Property | 128.436 ns | 2.5062 ns | 2.4614 ns | 77.47 | 1.50 |
| GetPropertyInfo_Property | 24.779 ns | 0.3351 ns | 0.3134 ns | 14.95 | 0.20 |
| GetPropertyInfoCache_Property | 7.896 ns | 0.1324 ns | 0.1239 ns | 4.76 | 0.08 |
| GetSigilEmit_Property | 3.819 ns | 0.0284 ns | 0.0379 ns | 2.30 | 0.03 |
| GetExpression_Property | 3.092 ns | 0.0325 ns | 0.0304 ns | 1.87 | 0.02 |
| GetFastMember_Property | 13.500 ns | 0.1078 ns | 0.0900 ns | 8.14 | 0.07 |
| **GetImmediateProperty_Property** | **1.941 ns** | **0.0293 ns** | **0.0274 ns** | **1.17** | **0.02** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| GetDirect_Property | 1.404 ns | 0.0089 ns | 0.0069 ns | 1.00 | 0.01 |
| GetDelegate_Property | 3.866 ns | 0.0571 ns | 0.0534 ns | 2.75 | 0.04 |
| GetDynamicDelegate_Property | 609.315 ns | 9.9388 ns | 9.2968 ns | 434.14 | 6.74 |
| GetPropertyInfo_Property | 139.493 ns | 2.2604 ns | 2.1144 ns | 99.39 | 1.53 |
| GetPropertyInfoCache_Property | 84.206 ns | 1.6152 ns | 1.5863 ns | 60.00 | 1.13 |
| GetSigilEmit_Property | 4.350 ns | 0.0636 ns | 0.0595 ns | 3.10 | 0.04 |
| GetExpression_Property | 8.542 ns | 0.1356 ns | 0.1202 ns | 6.09 | 0.09 |
| GetFastMember_Property | 25.788 ns | 0.5080 ns | 0.4989 ns | 18.37 | 0.36 |
| **GetImmediateProperty_Property** | **2.872 ns** | **0.0514 ns** | **0.0429 ns** | **2.05** | **0.03** |

### Set a property value

**.NET 8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Property | 1.664 ns | 0.0181 ns | 0.0160 ns | 1.00 | 0.01 |
| SetDelegate_Property | 3.805 ns | 0.0441 ns | 0.0413 ns | 2.29 | 0.03 |
| SetDynamicDelegate_Property | 131.993 ns | 2.5947 ns | 3.0888 ns | 79.35 | 1.96 |
| SetPropertyInfo_Property | 30.744 ns | 0.2841 ns | 0.5265 ns | 18.48 | 0.36 |
| SetPropertyInfoCache_Property | 13.238 ns | 0.1005 ns | 0.0891 ns | 7.96 | 0.09 |
| SetSigilEmit_Property | 3.811 ns | 0.0542 ns | 0.0481 ns | 2.29 | 0.04 |
| SetExpression_Property | 3.358 ns | 0.0496 ns | 0.0464 ns | 2.02 | 0.03 |
| SetFastMember_Property | 14.350 ns | 0.2972 ns | 0.2634 ns | 8.63 | 0.17 |
| **SetImmediateProperty_Property** | **2.222 ns** | **0.0389 ns** | **0.0558 ns** | **1.34** | **0.04** |

**.NET Framework 4.8**

| Method | Mean | Error | StdDev | Ratio | RatioSD |
|--- |---:|---:|---:|---:|---:|
| SetDirect_Property | 2.840 ns | 0.0506 ns | 0.0473 ns | 1.00 | 0.02 |
| SetDelegate_Property | 4.896 ns | 0.0475 ns | 0.0444 ns | 1.72 | 0.03 |
| SetDynamicDelegate_Property | 580.750 ns | 9.7414 ns | 8.6355 ns | 204.54 | 4.42 |
| SetPropertyInfo_Property | 186.662 ns | 3.5653 ns | 3.3350 ns | 65.74 | 1.56 |
| SetPropertyInfoCache_Property | 140.676 ns | 1.6180 ns | 1.5135 ns | 49.55 | 0.95 |
| SetSigilEmit_Property | 4.871 ns | 0.0566 ns | 0.0529 ns | 1.72 | 0.03 |
| SetExpression_Property | 7.376 ns | 0.1043 ns | 0.0925 ns | 2.60 | 0.05 |
| SetFastMember_Property | 33.201 ns | 0.6840 ns | 1.9069 ns | 11.69 | 0.69 |
| **SetImmediateProperty_Property** | **4.042 ns** | **0.2865 ns** | **0.8447 ns** | **1.42** | **0.30** |

---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.