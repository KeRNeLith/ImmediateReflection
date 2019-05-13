# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

## Configuration

`BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17134.706 (1803/April2018Update/Redstone4)
Intel Core i7-4720HQ CPU 2.60GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
Frequency=2533210 Hz, Resolution=394.7561 ns, Timer=TSC
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3394.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3394.0`

## Results

*Get a field value*

|                  Method |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------------:|------:|--------:|
|         GetDirect_Field |   0.0147 ns | 0.0254 ns | 0.0282 ns |   0.0000 ns |     ? |       ? |
|      GetFieldInfo_Field | 155.9170 ns | 3.1413 ns | 2.9384 ns | 156.1697 ns |     ? |       ? |
| GetFieldInfoCache_Field |  85.8316 ns | 0.8684 ns | 0.8123 ns |  85.9824 ns |     ? |       ? |
|     GetFastMember_Field |  42.8260 ns | 0.4328 ns | 0.4049 ns |  43.0462 ns |     ? |       ? |
| **GetImmediateField_Field** |   **7.2577 ns** | **0.0810 ns** | **0.0757 ns** |   **7.2758 ns** |     ? |       ? |

*Set a field value*

|                  Method |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------------:|------:|--------:|
|         SetDirect_Field |   0.0103 ns | 0.0166 ns | 0.0155 ns |   0.0013 ns |     ? |       ? |
|      SetFieldInfo_Field | 189.4856 ns | 5.7389 ns | 5.6364 ns | 187.7984 ns |     ? |       ? |
| SetFieldInfoCache_Field | 124.1563 ns | 2.4189 ns | 2.4840 ns | 123.7972 ns |     ? |       ? |
|     SetFastMember_Field |  43.6264 ns | 0.7829 ns | 0.6941 ns |  43.6652 ns |     ? |       ? |
| **SetImmediateField_Field** |   **7.5640 ns** | **0.0830 ns** | **0.0776 ns** |   **7.5905 ns** |     ? |       ? |

---

*Get a property value*

|                        Method |        Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------------ |------------:|-----------:|-----------:|------:|--------:|
|            GetDirect_Property |   0.0123 ns |  0.0154 ns |  0.0134 ns |     ? |       ? |
|          GetDelegate_Property |   6.6114 ns |  0.0519 ns |  0.0460 ns |     ? |       ? |
|   GetDynamicDelegate_Property | 739.9169 ns | 15.5476 ns | 14.5432 ns |     ? |       ? |
|      GetPropertyInfo_Property | 214.3223 ns |  2.6594 ns |  2.4876 ns |     ? |       ? |
| GetPropertyInfoCache_Property | 130.9820 ns |  1.4167 ns |  1.3252 ns |     ? |       ? |
|         GetSigilEmit_Property |   7.6240 ns |  0.0939 ns |  0.0878 ns |     ? |       ? |
|        GetExpression_Property |  15.4046 ns |  0.3038 ns |  0.2983 ns |     ? |       ? |
|        GetFastMember_Property |  44.1442 ns |  0.9606 ns |  1.7074 ns |     ? |       ? |
| **GetImmediateProperty_Property** |   **5.2596 ns** |  **0.0826 ns** |  **0.0772 ns** |     ? |       ? |

*Set a property value*

|                        Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|-------:|--------:|
|            SetDirect_Property |   2.106 ns | 0.0225 ns | 0.0211 ns |   1.00 |    0.00 |
|          SetDelegate_Property |   6.200 ns | 0.0502 ns | 0.0469 ns |   2.94 |    0.04 |
|   SetDynamicDelegate_Property | 794.723 ns | 4.4044 ns | 4.1199 ns | 377.41 |    4.49 |
|      SetPropertyInfo_Property | 289.381 ns | 3.4420 ns | 3.2197 ns | 137.42 |    2.06 |
| SetPropertyInfoCache_Property | 213.145 ns | 1.5090 ns | 1.3377 ns | 101.32 |    1.19 |
|         SetSigilEmit_Property |   6.242 ns | 0.0198 ns | 0.0175 ns |   2.97 |    0.03 |
|        SetExpression_Property |  11.445 ns | 0.1126 ns | 0.1053 ns |   5.43 |    0.05 |
|        SetFastMember_Property |  45.380 ns | 0.4711 ns | 0.4407 ns |  21.55 |    0.34 |
| **SetImmediateProperty_Property** |   **5.854 ns** | **0.0171 ns** | **0.0142 ns** |   2.79 |    0.03 |

---

As expected ImmediateReflection library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.