| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/ImmediateReflection?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/ImmediateReflection) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/ImmediateReflection/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/ImmediateReflection?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=immediate_reflection) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=alert_status)](https://sonarcloud.io/dashboard?id=immediate_reflection) | 
| **Nuget** | [![Nuget downloads](https://img.shields.io/nuget/v/immediatereflection.svg)](https://www.nuget.org/packages/ImmediateReflection) |
| **License** | [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/KeRNeLith/ImmediateReflection/blob/master/LICENSE) |

# ImmediateReflection

## What is **ImmediateReflection**?

This is .NET library that aims to provider faster usage of C# reflection features. 
Especially the usage of constructor and members accessors (get/set).

---

## Target

- .NET Standard 2.0+
- .NET Core 2.0+
- .NET Framework 2.0+

Supports Source Link

---

## Dependencies

**No package dependencies.**

### Notes

- It uses NUnit3 for unit testing (not published).

- The library code is published annotated with JetBrains annotations that are embedded in the library. But they will **not conflict** with any of your referenced packages or project defined attributes as they are **internal** to ImmediateReflection.

---

## Installation

ImmediateReflection is available on [NuGet](https://www.nuget.org/packages/ImmediateReflection)

	PM> Install-Package ImmediateReflection

---