# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

These benchmarks have been done by making multiple get or set on multiple types to avoid caching of the same operation.

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

|                  Method |       Mean |    Error |    StdDev | Ratio | RatioSD |
|------------------------ |-----------:|---------:|----------:|------:|--------:|
|         GetDirect_Field |   558.4 ns | 11.13 ns | 21.718 ns |  1.00 |    0.00 |
|      GetFieldInfo_Field | 1,003.5 ns | 20.03 ns | 31.765 ns |  1.81 |    0.08 |
| GetFieldInfoCache_Field |   803.4 ns | 19.99 ns | 25.283 ns |  1.46 |    0.09 |
|     GetFastMember_Field |   771.2 ns | 15.03 ns | 23.834 ns |  1.39 |    0.07 |
| **GetImmediateField_Field** |   **559.7 ns** | **10.49 ns** |  **8.764 ns** |  **1.02** |    **0.04** |

### Set a field value

|                  Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------ |-----------:|----------:|----------:|-------:|--------:|
|         SetDirect_Field |   1.326 ns | 0.0057 ns | 0.0053 ns |   1.00 |    0.00 |
|      SetFieldInfo_Field | 489.305 ns | 2.9321 ns | 2.7427 ns | 369.13 |    2.34 |
| SetFieldInfoCache_Field | 322.758 ns | 0.9229 ns | 0.8633 ns | 243.49 |    1.18 |
|     SetFastMember_Field | 154.080 ns | 0.4601 ns | 0.3842 ns | 116.32 |    0.41 |
| **SetImmediateField_Field** |  **20.319 ns** | **0.1331 ns** | **0.1180 ns** |  **15.33** |    **0.10** |

---

### Get a property value

|                        Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|------:|--------:|
|            GetDirect_Property |   749.4 ns | 15.550 ns | 13.785 ns |  1.00 |    0.00 |
|          GetDelegate_Property |   752.2 ns |  7.655 ns |  6.786 ns |  1.00 |    0.02 |
|   GetDynamicDelegate_Property | 3,356.4 ns | 11.508 ns | 10.201 ns |  4.48 |    0.08 |
|      GetPropertyInfo_Property | 1,471.6 ns | 13.947 ns | 13.046 ns |  1.96 |    0.04 |
| GetPropertyInfoCache_Property | 1,238.6 ns |  2.543 ns |  2.123 ns |  1.65 |    0.03 |
|         GetSigilEmit_Property |   757.3 ns |  6.912 ns |  6.466 ns |  1.01 |    0.02 |
|        GetExpression_Property |   786.1 ns |  5.315 ns |  4.972 ns |  1.05 |    0.02 |
|        GetFastMember_Property |   959.8 ns |  9.776 ns |  8.667 ns |  1.28 |    0.03 |
| **GetImmediateProperty_Property** |   **764.3 ns** |  **1.906 ns** |  **1.690 ns** |  **1.02** |    **0.02** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.GetValue` as reference).

### Set a property value

|                        Method |         Mean |      Error |     StdDev |    Ratio | RatioSD |
|------------------------------ |-------------:|-----------:|-----------:|---------:|--------:|
|            SetDirect_Property |     1.403 ns |  0.0147 ns |  0.0138 ns |     1.00 |    0.00 |
|          SetDelegate_Property |    10.580 ns |  0.2104 ns |  0.2808 ns |     7.56 |    0.23 |
|   SetDynamicDelegate_Property | 2,512.460 ns | 21.4628 ns | 19.0262 ns | 1,789.19 |   17.07 |
|      SetPropertyInfo_Property |   844.363 ns |  2.8357 ns |  2.5138 ns |   601.30 |    5.41 |
| SetPropertyInfoCache_Property |   631.593 ns |  6.9640 ns |  6.5141 ns |   450.08 |    5.53 |
|         SetSigilEmit_Property |    10.470 ns |  0.2040 ns |  0.2505 ns |     7.46 |    0.17 |
|        SetExpression_Property |    32.912 ns |  0.1921 ns |  0.1604 ns |    23.45 |    0.22 |
|        SetFastMember_Property |   162.898 ns |  0.6902 ns |  0.6119 ns |   116.01 |    1.26 |
| **SetImmediateProperty_Property** |    **22.358 ns** |  **0.4367 ns** |  **0.3871 ns** |    **15.92** |    **0.31** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.SetValue` as reference).


---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.