# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

These benchmarks have been done by making multiple get or set on multiple types to avoid caching of the same operation.

## Configuration

```ini
BenchmarkDotNet=v0.11.5
OS=Windows 10.0.17134.765 (1803/April2018Update/Redstone4)
Processor=Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
```

## Implementation details

The Field or Property cache implementations consider as a cache the fact of having the right `PropertyInfo` ready to use with GetValue/SetValue.

## Results

### Get a field value

|                  Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |-----------:|----------:|----------:|------:|--------:|
|         GetDirect_Field |   570.0 ns | 10.163 ns |  9.506 ns |  1.00 |    0.00 |
|      GetFieldInfo_Field | 1,010.4 ns | 30.228 ns | 32.344 ns |  1.78 |    0.07 |
| GetFieldInfoCache_Field |   836.6 ns | 10.933 ns | 10.227 ns |  1.47 |    0.03 |
|     GetFastMember_Field |   757.3 ns | 14.700 ns | 21.082 ns |  1.34 |    0.05 |
| **GetImmediateField_Field** |   **585.8 ns** |  **4.471 ns** |  **3.734 ns** |  **1.03** |    **0.02** |

### Set a field value

|                  Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------ |-----------:|----------:|----------:|-------:|--------:|
|         SetDirect_Field |   1.228 ns | 0.0028 ns | 0.0027 ns |   1.00 |    0.00 |
|      SetFieldInfo_Field | 527.551 ns | 0.9556 ns | 0.8471 ns | 429.50 |    1.26 |
| SetFieldInfoCache_Field | 349.815 ns | 0.6424 ns | 0.5695 ns | 284.80 |    0.85 |
|     SetFastMember_Field | 156.822 ns | 1.6745 ns | 1.5663 ns | 127.69 |    1.36 |
| **SetImmediateField_Field** |  **19.925 ns** | **0.3469 ns** | **0.3075 ns** |  **16.22** |    **0.25** |

---

### Get a property value

|                        Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|------:|--------:|
|            GetDirect_Property |   597.3 ns | 16.650 ns | 16.352 ns |  1.00 |    0.00 |
|          GetDelegate_Property |   592.1 ns |  2.049 ns |  1.600 ns |  0.99 |    0.03 |
|   GetDynamicDelegate_Property | 3,221.4 ns | 29.153 ns | 25.843 ns |  5.38 |    0.15 |
|      GetPropertyInfo_Property | 1,313.1 ns |  2.735 ns |  2.424 ns |  2.19 |    0.06 |
| GetPropertyInfoCache_Property | 1,087.3 ns | 19.480 ns | 17.269 ns |  1.82 |    0.05 |
|         GetSigilEmit_Property |   598.7 ns |  3.547 ns |  2.962 ns |  1.00 |    0.03 |
|        GetExpression_Property |   638.6 ns |  3.694 ns |  3.456 ns |  1.07 |    0.03 |
|        GetFastMember_Property |   771.0 ns |  2.561 ns |  2.395 ns |  1.29 |    0.03 |
| **GetImmediateProperty_Property** |   **620.8 ns** |  **4.857 ns** |  **4.056 ns** |  **1.04** |    **0.03** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.GetValue` as reference).

### Set a property value

|                        Method |         Mean |     Error |    StdDev |    Ratio | RatioSD |
|------------------------------ |-------------:|----------:|----------:|---------:|--------:|
|            SetDirect_Property |     1.238 ns | 0.0032 ns | 0.0029 ns |     1.00 |    0.00 |
|          SetDelegate_Property |     9.552 ns | 0.0900 ns | 0.0841 ns |     7.71 |    0.08 |
|   SetDynamicDelegate_Property | 2,565.402 ns | 9.1200 ns | 8.0847 ns | 2,071.69 |    9.37 |
|      SetPropertyInfo_Property |   889.661 ns | 7.2569 ns | 6.4331 ns |   718.44 |    4.80 |
| SetPropertyInfoCache_Property |   644.607 ns | 2.8729 ns | 2.5467 ns |   520.55 |    2.79 |
|         SetSigilEmit_Property |     9.148 ns | 0.0526 ns | 0.0492 ns |     7.38 |    0.05 |
|        SetExpression_Property |    33.259 ns | 0.2909 ns | 0.2578 ns |    26.86 |    0.24 |
|        SetFastMember_Property |   173.049 ns | 1.0150 ns | 0.8998 ns |   139.75 |    0.91 |
| **SetImmediateProperty_Property** |    **20.275 ns** | **0.1285 ns** | **0.1202 ns** |    **16.38** |    **0.08** |

Note that **ImmediateReflection** performs really well if we take into account that the only better benchmark concern implementation using strong types considered as known which is in fact not the case in the mindset of **ImmediateReflection**.
Indeed **ImmediateReflection** must work with `object` in a first approach and not the real property type (see `PropertyInfo.SetValue` as reference).


---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.