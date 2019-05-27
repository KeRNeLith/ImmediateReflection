# Benchmarks

Benchmarks have been implemented with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

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

*Get a field value*

|                  Method |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------------:|------:|--------:|
|         GetDirect_Field |   0.0262 ns | 0.0246 ns | 0.0230 ns |   0.0189 ns |     ? |       ? |
|      GetFieldInfo_Field | 110.6631 ns | 1.4372 ns | 1.3443 ns | 110.7806 ns |     ? |       ? |
| GetFieldInfoCache_Field |  65.2877 ns | 1.3246 ns | 1.2390 ns |  65.4026 ns |     ? |       ? |
|     GetFastMember_Field |  32.1864 ns | 0.5634 ns | 0.5270 ns |  32.0441 ns |     ? |       ? |
| **GetImmediateField_Field** |   **5.2315 ns** | **0.0953 ns** | **0.0845 ns** |   **5.2173 ns** |     **?** |       **?** |

**GetDirect_Field** is a too quick action to be benchmark, considering it as immediate!

*Set a field value*

|                  Method |        Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------:|--------:|
|         SetDirect_Field |   0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |
|      SetFieldInfo_Field | 129.1976 ns | 0.2704 ns | 0.2111 ns |     ? |       ? |
| SetFieldInfoCache_Field |  84.7197 ns | 0.8674 ns | 0.8114 ns |     ? |       ? |
|     SetFastMember_Field |  30.1511 ns | 0.6079 ns | 0.6243 ns |     ? |       ? |
| **SetImmediateField_Field** |   **6.2244 ns** | **0.1199 ns** | **0.1121 ns** |     **?** |       **?** |

---

*Get a property value*

|                        Method |        Mean |      Error |     StdDev | Ratio | RatioSD |
|------------------------------ |------------:|-----------:|-----------:|------:|--------:|
|            GetDirect_Property |   0.0000 ns |  0.0000 ns |  0.0000 ns |     ? |       ? |
|          GetDelegate_Property |   4.8418 ns |  0.1164 ns |  0.1554 ns |     ? |       ? |
|   GetDynamicDelegate_Property | 605.9580 ns | 11.7338 ns | 16.8282 ns |     ? |       ? |
|      GetPropertyInfo_Property | 154.3991 ns |  4.1930 ns |  4.1181 ns |     ? |       ? |
| GetPropertyInfoCache_Property |  93.5667 ns |  0.9849 ns |  0.8731 ns |     ? |       ? |
|         GetSigilEmit_Property |   4.9171 ns |  0.0611 ns |  0.0571 ns |     ? |       ? |
|        GetExpression_Property |  10.3674 ns |  0.1333 ns |  0.1247 ns |     ? |       ? |
|        GetFastMember_Property |  30.3588 ns |  0.4167 ns |  0.3694 ns |     ? |       ? |
| **GetImmediateProperty_Property** |   **3.8736 ns** |  **0.1001 ns** |  **0.0936 ns** |     **?** |       **?** |

**GetDirect_Property** is a too quick action to be benchmark, considering it as immediate!

*Set a property value*

|                        Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|-------:|--------:|
|            SetDirect_Property |   1.327 ns | 0.0052 ns | 0.0046 ns |   1.00 |    0.00 |
|          SetDelegate_Property |   4.647 ns | 0.0420 ns | 0.0372 ns |   3.50 |    0.02 |
|   SetDynamicDelegate_Property | 584.270 ns | 4.6850 ns | 3.9122 ns | 440.25 |    3.24 |
|      SetPropertyInfo_Property | 216.180 ns | 1.5930 ns | 1.4121 ns | 162.94 |    1.17 |
| SetPropertyInfoCache_Property | 146.659 ns | 1.4143 ns | 1.1042 ns | 110.44 |    0.79 |
|         SetSigilEmit_Property |   4.338 ns | 0.0184 ns | 0.0172 ns |   3.27 |    0.02 |
|        SetExpression_Property |   7.539 ns | 0.0171 ns | 0.0151 ns |   5.68 |    0.03 |
|        SetFastMember_Property |  31.166 ns | 0.4283 ns | 0.4006 ns |  23.47 |    0.34 |
| **SetImmediateProperty_Property** |   **3.653 ns** | **0.0269 ns** | **0.0238 ns** |   **2.75** |    **0.02** |

---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.