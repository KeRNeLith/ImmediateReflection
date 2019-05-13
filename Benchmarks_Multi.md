# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

These benchmarks have been done by making multiple get or set on multiple types to avoid caching of the same operation.

## Configuration

`BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.706 (1803/April2018Update/Redstone4)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3394.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3394.0`

## Results

*Get a field value*

|                  Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |-----------:|----------:|----------:|------:|--------:|
|         GetDirect_Field |   624.0 ns | 12.308 ns | 16.847 ns |  1.00 |    0.00 |
|      GetFieldInfo_Field | 1,059.0 ns |  7.878 ns |  6.984 ns |  1.68 |    0.05 |
| GetFieldInfoCache_Field |   864.1 ns | 11.106 ns |  9.845 ns |  1.37 |    0.04 |
|     GetFastMember_Field |   762.1 ns |  6.375 ns |  5.963 ns |  1.21 |    0.03 |
| **GetImmediateField_Field** |   **581.3 ns** |  **2.901 ns** |  **2.265 ns** |  **0.91** |    **0.03** |

*Set a field value*

|                  Method |       Mean |      Error |     StdDev |  Ratio | RatioSD |
|------------------------ |-----------:|-----------:|-----------:|-------:|--------:|
|         SetDirect_Field |   1.466 ns |  0.0251 ns |  0.0209 ns |   1.00 |    0.00 |
|      SetFieldInfo_Field | 541.413 ns | 17.6852 ns | 16.5427 ns | 369.18 |   14.19 |
| SetFieldInfoCache_Field | 354.782 ns |  2.9840 ns |  2.4918 ns | 242.04 |    2.54 |
|     SetFastMember_Field | 176.934 ns |  1.7557 ns |  1.6423 ns | 120.80 |    1.69 |
| **SetImmediateField_Field** |  **20.289 ns** |  **0.2316 ns** |  **0.2166 ns** |  **13.86** |    **0.28** |

---

*Get a property value*

|                        Method |       Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|------:|--------:|
|            GetDirect_Property |   596.2 ns |  2.558 ns |  2.268 ns |  1.00 |    0.00 |
|          GetDelegate_Property |   615.5 ns |  7.549 ns |  7.061 ns |  1.03 |    0.01 |
|   GetDynamicDelegate_Property | 3,193.8 ns | 16.150 ns | 14.316 ns |  5.36 |    0.03 |
|      GetPropertyInfo_Property | 1,379.4 ns |  7.130 ns |  5.954 ns |  2.31 |    0.02 |
| GetPropertyInfoCache_Property | 1,160.7 ns | 19.579 ns | 18.314 ns |  1.95 |    0.03 |
|         GetSigilEmit_Property |   617.2 ns | 11.758 ns | 10.998 ns |  1.03 |    0.02 |
|        GetExpression_Property |   664.6 ns |  8.468 ns |  7.921 ns |  1.12 |    0.01 |
|        GetFastMember_Property |   809.7 ns | 16.100 ns | 17.895 ns |  1.36 |    0.03 |
| **GetImmediateProperty_Property** |   **631.2 ns** |  **5.636 ns** |  **4.996 ns** |  **1.06** |    **0.01** |

*Set a property value*

|                        Method |         Mean |      Error |     StdDev |    Ratio | RatioSD |
|------------------------------ |-------------:|-----------:|-----------:|---------:|--------:|
|            SetDirect_Property |     1.345 ns |  0.0098 ns |  0.0091 ns |     1.00 |    0.00 |
|          SetDelegate_Property |     9.776 ns |  0.1704 ns |  0.1594 ns |     7.27 |    0.10 |
|   SetDynamicDelegate_Property | 2,582.757 ns | 32.6037 ns | 30.4975 ns | 1,920.76 |   27.73 |
|      SetPropertyInfo_Property |   884.921 ns |  5.1935 ns |  4.8580 ns |   658.09 |    4.40 |
| SetPropertyInfoCache_Property |   650.841 ns |  3.9478 ns |  3.4996 ns |   484.19 |    4.39 |
|         SetSigilEmit_Property |     9.352 ns |  0.0462 ns |  0.0409 ns |     6.96 |    0.05 |
|        SetExpression_Property |    35.209 ns |  0.6262 ns |  0.5858 ns |    26.18 |    0.46 |
|        SetFastMember_Property |   193.098 ns |  2.3902 ns |  2.2358 ns |   143.60 |    1.95 |
| **SetImmediateProperty_Property** |    **21.190 ns** |  **0.1476 ns** |  **0.1308 ns** |    **15.76** |    **0.12** |

---

As expected ImmediateReflection library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.