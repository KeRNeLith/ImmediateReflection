## Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

*Get a field value*

|                  Method |          Mean |     Error |    StdDev |    Ratio | RatioSD |
|------------------------ |--------------:|----------:|----------:|---------:|--------:|
|         GetDirect_Field |     0.9082 ns | 0.0113 ns | 0.0101 ns |     1.00 |    0.00 |
|      GetFieldInfo_Field | 1,068.2783 ns | 8.6150 ns | 7.6370 ns | 1,176.37 |   15.69 |
| GetFieldInfoCache_Field |   989.5415 ns | 4.6663 ns | 3.6431 ns | 1,088.26 |   10.65 |
|     GetFastMember_Field |    28.5185 ns | 0.3009 ns | 0.2513 ns |    31.41 |    0.50 |
| **GetImmediateField_Field** |     **6.3111 ns** | **0.2211 ns** | **0.2715 ns** |     **7.05** |    **0.31** |

*Set a field value*

|                  Method |          Mean |     Error |    StdDev |    Ratio | RatioSD |
|------------------------ |--------------:|----------:|----------:|---------:|--------:|
|         SetDirect_Field |     0.8962 ns | 0.0101 ns | 0.0084 ns |     1.00 |    0.00 |
|      SetFieldInfo_Field | 1,104.1402 ns | 9.9162 ns | 8.7905 ns | 1,233.05 |   13.84 |
| SetFieldInfoCache_Field |   990.2330 ns | 2.4879 ns | 2.0775 ns | 1,105.00 |   11.56 |
|     SetFastMember_Field |    27.8781 ns | 0.0877 ns | 0.0778 ns |    31.12 |    0.30 |
| **SetImmediateField_Field** |     **5.7449 ns** | **0.1408 ns** | **0.2502 ns** |     **6.20** |    **0.16** |

---

*Get a property value*

|                        Method |        Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |------------:|----------:|----------:|-------:|--------:|
|            GetDirect_Property |   0.9181 ns | 0.0757 ns | 0.0671 ns |   1.00 |    0.00 |
|          GetDelegate_Property |   2.7163 ns | 0.0630 ns | 0.0558 ns |   2.97 |    0.20 |
|   GetDynamicDelegate_Property | 524.9968 ns | 3.3247 ns | 2.9473 ns | 574.53 |   39.39 |
|      GetPropertyInfo_Property | 124.2704 ns | 0.3083 ns | 0.2884 ns | 135.99 |    9.51 |
| GetPropertyInfoCache_Property |  63.1564 ns | 0.1596 ns | 0.1415 ns |  69.12 |    4.87 |
|        GetFastMember_Property |  27.3103 ns | 0.1265 ns | 0.1183 ns |  29.90 |    2.07 |
| **GetImmediateProperty_Property** |   **3.8488 ns** | **0.0334 ns** | **0.0279 ns** |   **4.23** |    **0.30** |

*Get a property value*

|                        Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|-------:|--------:|
|            SetDirect_Property |   2.036 ns | 0.0151 ns | 0.0134 ns |   1.00 |    0.00 |
|          SetDelegate_Property |   4.038 ns | 0.0368 ns | 0.0345 ns |   1.98 |    0.02 |
|   SetDynamicDelegate_Property | 633.429 ns | 6.2525 ns | 5.8486 ns | 311.21 |    4.28 |
|      SetPropertyInfo_Property | 190.114 ns | 2.8883 ns | 2.7017 ns |  93.32 |    1.71 |
| SetPropertyInfoCache_Property | 121.591 ns | 0.7281 ns | 0.6080 ns |  59.76 |    0.48 |
|        SetFastMember_Property |  28.747 ns | 0.0292 ns | 0.0273 ns |  14.12 |    0.09 |
| **SetImmediateProperty_Property** |   **4.738 ns** | **0.0131 ns** | **0.0122 ns** |   **2.33** |    **0.02** |  

---

As expected ImmediateReflection library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.