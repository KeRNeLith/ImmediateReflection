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

|                  Method |        Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------:|--------:|
|         GetDirect_Field |   0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |
|      GetFieldInfo_Field | 100.6782 ns | 0.1422 ns | 0.1261 ns |     ? |       ? |
| GetFieldInfoCache_Field |  60.4257 ns | 0.7904 ns | 0.7393 ns |     ? |       ? |
|     GetFastMember_Field |  29.9598 ns | 0.4392 ns | 0.3894 ns |     ? |       ? |
| **GetImmediateField_Field** |   **5.2535 ns** | **0.1314 ns** | **0.3018 ns** |     **?** |       **?** |

**GetDirect_Field** is a too quick action to be benchmark, considering it as immediate!

### Set a field value

|                  Method |        Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------ |------------:|----------:|----------:|------:|--------:|
|         SetDirect_Field |   0.0051 ns | 0.0085 ns | 0.0075 ns |     ? |       ? |
|      SetFieldInfo_Field | 129.9163 ns | 0.3037 ns | 0.2371 ns |     ? |       ? |
| SetFieldInfoCache_Field |  81.8266 ns | 0.2578 ns | 0.2153 ns |     ? |       ? |
|     SetFastMember_Field |  28.8047 ns | 0.0672 ns | 0.0524 ns |     ? |       ? |
| **SetImmediateField_Field** |   **5.2266 ns** | **0.1288 ns** | **0.1205 ns** |     **?** |       **?** |

---

### Get a property value

|                        Method |        Mean |     Error |    StdDev |    Ratio | RatioSD |
|------------------------------ |------------:|----------:|----------:|---------:|--------:|
|            GetDirect_Property |   0.3030 ns | 0.0389 ns | 0.0363 ns |     1.00 |    0.00 |
|          GetDelegate_Property |   4.9874 ns | 0.0986 ns | 0.1247 ns |    16.76 |    1.97 |
|   GetDynamicDelegate_Property | 537.3377 ns | 8.8565 ns | 8.2843 ns | 1,796.26 |  207.81 |
|      GetPropertyInfo_Property | 142.9487 ns | 0.7985 ns | 0.7469 ns |   477.96 |   55.61 |
| GetPropertyInfoCache_Property |  87.2921 ns | 0.9230 ns | 0.8182 ns |   288.61 |   32.69 |
|         GetSigilEmit_Property |   5.9783 ns | 0.1159 ns | 0.1190 ns |    19.97 |    2.35 |
|        GetExpression_Property |   9.7760 ns | 0.1546 ns | 0.1446 ns |    32.71 |    4.03 |
|        GetFastMember_Property |  29.1414 ns | 0.3692 ns | 0.3453 ns |    97.45 |   11.57 |
| **GetImmediateProperty_Property** |   **5.1417 ns** | **0.1690 ns** | **0.4271 ns** |    **17.20** |    **2.52** |

**GetDirect_Property** is a too quick action to be benchmark, considering it as immediate!

### Set a property value

|                        Method |       Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------------------ |-----------:|----------:|----------:|-------:|--------:|
|            SetDirect_Property |   1.165 ns | 0.0101 ns | 0.0094 ns |   1.00 |    0.00 |
|          SetDelegate_Property |   4.596 ns | 0.0915 ns | 0.1785 ns |   3.92 |    0.13 |
|   SetDynamicDelegate_Property | 571.378 ns | 6.8747 ns | 6.4306 ns | 490.65 |    6.76 |
|      SetPropertyInfo_Property | 198.768 ns | 1.2488 ns | 1.1681 ns | 170.69 |    2.07 |
| SetPropertyInfoCache_Property | 141.481 ns | 0.8785 ns | 0.7788 ns | 121.51 |    1.46 |
|         SetSigilEmit_Property |   4.644 ns | 0.0911 ns | 0.1152 ns |   4.00 |    0.11 |
|        SetExpression_Property |   7.622 ns | 0.0195 ns | 0.0183 ns |   6.55 |    0.06 |
|        SetFastMember_Property |  31.451 ns | 0.1225 ns | 0.1086 ns |  27.01 |    0.22 |
| **SetImmediateProperty_Property** |   **4.526 ns** | **0.0966 ns** | **0.0857 ns** |   **3.89** |    **0.08** |

---

As expected **ImmediateReflection** library provides get/set access in a very fast way, while also keeping the standard interface of .NET Reflection.