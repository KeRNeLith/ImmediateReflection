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

|                  Method |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |---------:|----------:|----------:|------:|--------:|
|         GetDirect_Field | 530.0 ns | 11.161 ns | 24.499 ns |  1.00 |    0.00 |
|      GetFieldInfo_Field | 966.9 ns | 11.199 ns | 10.475 ns |  1.74 |    0.08 |
| GetFieldInfoCache_Field | 788.5 ns |  6.924 ns |  5.782 ns |  1.40 |    0.05 |
|     GetFastMember_Field | 691.2 ns |  6.194 ns |  5.794 ns |  1.24 |    0.05 |
| **GetImmediateField_Field** | **538.6 ns** | **14.479 ns** | **12.835 ns** |  **0.96** |    **0.05** |

### Set a field value

|                  Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------ |-----------:|----------:|----------:|-------:|--------:|
|         SetDirect_Field |   1.343 ns | 0.0212 ns | 0.0188 ns |   1.00 |    0.00 |
|      SetFieldInfo_Field | 508.308 ns | 4.1885 ns | 3.9179 ns | 378.54 |    7.06 |
| SetFieldInfoCache_Field | 334.152 ns | 3.7199 ns | 3.4796 ns | 248.86 |    4.38 |
|     SetFastMember_Field | 153.589 ns | 1.2526 ns | 1.0459 ns | 114.37 |    1.59 |
| **SetImmediateField_Field** |  **23.006 ns** | **1.0165 ns** | **1.2101 ns** |  **17.31** |    **1.12** |

---

### Get a property value

|                        Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|------:|--------:|
|            GetDirect_Property |   700.8 ns |  2.185 ns |  1.824 ns |  1.00 |    0.00 |
|          GetDelegate_Property |   723.6 ns |  3.828 ns |  3.394 ns |  1.03 |    0.01 |
|   GetDynamicDelegate_Property | 3,323.6 ns | 17.185 ns | 15.234 ns |  4.74 |    0.03 |
|      GetPropertyInfo_Property | 1,482.8 ns | 32.301 ns | 34.561 ns |  2.12 |    0.05 |
| GetPropertyInfoCache_Property | 1,210.4 ns |  9.714 ns |  9.087 ns |  1.73 |    0.01 |
|         GetSigilEmit_Property |   728.7 ns |  7.606 ns |  6.742 ns |  1.04 |    0.01 |
|        GetExpression_Property |   748.6 ns |  2.830 ns |  2.509 ns |  1.07 |    0.01 |
|        GetFastMember_Property |   936.6 ns | 18.140 ns | 20.163 ns |  1.34 |    0.03 |
| **GetImmediateProperty_Property** |   **727.7 ns** |  **9.243 ns** |  **7.719 ns** |  **1.04** |    **0.01** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.GetValue` as reference).

### Set a property value

|                        Method |         Mean |      Error |     StdDev |    Ratio | RatioSD |
|------------------------------ |-------------:|-----------:|-----------:|---------:|--------:|
|            SetDirect_Property |     1.402 ns |  0.0287 ns |  0.0254 ns |     1.00 |    0.00 |
|          SetDelegate_Property |    10.876 ns |  0.2157 ns |  0.4051 ns |     7.73 |    0.36 |
|   SetDynamicDelegate_Property | 2,536.329 ns | 37.2463 ns | 34.8403 ns | 1,809.40 |   45.17 |
|      SetPropertyInfo_Property |   858.440 ns | 13.0924 ns | 12.2467 ns |   613.07 |   13.73 |
| SetPropertyInfoCache_Property |   638.475 ns |  7.7110 ns |  6.8356 ns |   455.62 |    8.86 |
|         SetSigilEmit_Property |    10.987 ns |  0.2192 ns |  0.5926 ns |     7.65 |    0.31 |
|        SetExpression_Property |    32.915 ns |  0.1765 ns |  0.1564 ns |    23.49 |    0.45 |
|        SetFastMember_Property |   164.237 ns |  1.0621 ns |  0.9416 ns |   117.20 |    1.72 |
| **SetImmediateProperty_Property** |    **22.787 ns** |  **0.5222 ns** |  **0.8282 ns** |    **16.70** |    **0.70** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.SetValue` as reference).


---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.