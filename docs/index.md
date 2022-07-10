# ImmediateReflection documentation

## Badges

| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/ImmediateReflection?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/ImmediateReflection) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/ImmediateReflection/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/ImmediateReflection?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=immediate_reflection) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=alert_status)](https://sonarcloud.io/dashboard?id=immediate_reflection) | 
| **Nuget** | [![Nuget status](https://img.shields.io/nuget/v/immediatereflection.svg)](https://www.nuget.org/packages/ImmediateReflection) |
| **License** | [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/KeRNeLith/ImmediateReflection/blob/master/LICENSE) |

## Introduction

This is .NET library that aims to provide a **faster** usage of **C# reflection** features. 
Especially the usage of constructor, members accessors (get/set) and attributes.

It provides these features while trying to keep an API as similar as the standard Reflection API (**Fully documented** and **ReSharper compliant**).

To see how **powerful** the library is you can consult some benchmarks **[there](documentation/benchmarks.md)**.

The library is highly tested to cover as much as possible real cases, because using Reflection is some kind of core code and must be reliable to build on it.

You can find library sources on [GitHub](https://github.com/KeRNeLith/ImmediateReflection).

## Targets

- .NET Standard 2.0+
- .NET Core 2.0+
- .NET Framework 4.0+

Supports Source Link

## Installation

ImmediateReflection is available on [NuGet](https://www.nuget.org/packages/ImmediateReflection)

    PM> Install-Package ImmediateReflection

<img src="images/immediate_reflection_logo.png" width="128" height="128" style="display: block; margin-left: auto; margin-right: auto" />