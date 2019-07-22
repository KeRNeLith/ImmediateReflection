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

The Field or Property cache implementations consider as a cache the fact of having the right `PropertyInfo` ready to use with GetValue/SetValue.

## Results

### Get a field value

|                  Method |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------------:|------:|--------:|
|         GetDirect_Field |   0.0073 ns | 0.0091 ns | 0.0081 ns |   0.0052 ns |     ? |       ? |
|      GetFieldInfo_Field | 103.2361 ns | 0.8377 ns | 0.7426 ns | 103.0017 ns |     ? |       ? |
| GetFieldInfoCache_Field |  62.3059 ns | 0.6476 ns | 0.6058 ns |  62.2037 ns |     ? |       ? |
|     GetFastMember_Field |  29.9017 ns | 0.1978 ns | 0.1544 ns |  29.9552 ns |     ? |       ? |
| **GetImmediateField_Field** |   **4.8914 ns** | **0.0653 ns** | **0.0579 ns** |   **4.8962 ns** |     **?** |       **?** |

**GetDirect_Field** is a too quick action to be benchmark, considering it as immediate!

### Set a field value

|                  Method |        Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------:|--------:|
|         SetDirect_Field |   0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |
|      SetFieldInfo_Field | 127.6475 ns | 0.9885 ns | 0.9246 ns |     ? |       ? |
| SetFieldInfoCache_Field |  77.3834 ns | 0.6383 ns | 0.5971 ns |     ? |       ? |
|     SetFastMember_Field |  29.3380 ns | 0.3022 ns | 0.2827 ns |     ? |       ? |
| **SetImmediateField_Field** |   **5.1049 ns** | **0.0754 ns** | **0.0705 ns** |     **?** |       **?** |

---

### Get a property value

|                        Method |        Mean |     Error |    StdDev |    Ratio | RatioSD |
|------------------------------ |------------:|----------:|----------:|---------:|--------:|
|            GetDirect_Property |   0.2308 ns | 0.0263 ns | 0.0233 ns |     1.00 |    0.00 |
|          GetDelegate_Property |   5.1924 ns | 0.1013 ns | 0.1748 ns |    22.74 |    2.24 |
|   GetDynamicDelegate_Property | 544.1249 ns | 5.3205 ns | 4.9768 ns | 2,378.40 |  234.63 |
|      GetPropertyInfo_Property | 143.0557 ns | 1.2193 ns | 1.1405 ns |   624.91 |   62.66 |
| GetPropertyInfoCache_Property |  88.0197 ns | 1.0542 ns | 0.9345 ns |   384.82 |   37.64 |
|         GetSigilEmit_Property |   6.3718 ns | 0.1253 ns | 0.2162 ns |    27.98 |    2.87 |
|        GetExpression_Property |   9.7486 ns | 0.0679 ns | 0.0602 ns |    42.62 |    4.21 |
|        GetFastMember_Property |  28.8541 ns | 0.1960 ns | 0.1834 ns |   126.08 |   12.11 |
| **GetImmediateProperty_Property** |   **5.1149 ns** | **0.1653 ns** | **0.2573 ns** |    **22.51** |    **3.27** |

**GetDirect_Property** is a too quick action to be benchmark, considering it as immediate!

### Set a property value

|                        Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|-------:|--------:|
|            SetDirect_Property |   1.388 ns | 0.0173 ns | 0.0162 ns |   1.00 |    0.00 |
|          SetDelegate_Property |   4.889 ns | 0.0952 ns | 0.1454 ns |   3.51 |    0.12 |
|   SetDynamicDelegate_Property | 589.718 ns | 5.4911 ns | 5.1364 ns | 424.98 |    7.58 |
|      SetPropertyInfo_Property | 200.290 ns | 0.7330 ns | 0.6121 ns | 144.04 |    1.76 |
| SetPropertyInfoCache_Property | 142.318 ns | 1.1069 ns | 1.0354 ns | 102.56 |    1.67 |
|         SetSigilEmit_Property |   4.862 ns | 0.0949 ns | 0.1734 ns |   3.48 |    0.11 |
|        SetExpression_Property |   8.060 ns | 0.0414 ns | 0.0388 ns |   5.81 |    0.08 |
|        SetFastMember_Property |  30.527 ns | 0.3143 ns | 0.2940 ns |  22.00 |    0.33 |
| **SetImmediateProperty_Property** |   **5.007 ns** | **0.1219 ns** | **0.1197 ns** |   **3.60** |    **0.12** |

---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.