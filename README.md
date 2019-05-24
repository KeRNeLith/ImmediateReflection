| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/ImmediateReflection?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/ImmediateReflection) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/ImmediateReflection/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/ImmediateReflection?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=immediate_reflection) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=alert_status)](https://sonarcloud.io/dashboard?id=immediate_reflection) | 
| **Nuget** | [![Nuget downloads](https://img.shields.io/nuget/v/immediatereflection.svg)](https://www.nuget.org/packages/ImmediateReflection) |
| **License** | [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/KeRNeLith/ImmediateReflection/blob/master/LICENSE) |

# ImmediateReflection

## What is **ImmediateReflection**?

This is .NET library that aims to provide a faster usage of C# reflection features. 
Especially the usage of constructor and members accessors (get/set).

It provides these features while trying to keep an API as similar as the standard Reflection API (Fully documented and ReSharper compliant).

To see how powerful it can be [here](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks.md) are some benchmarks of the library.

Other benchmarks made on multiple get/set of multiple types to avoid eventual processor caching issues, see [there](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks_Multi.md).

## Getting started

### Getting a type

The library is pretty simple to use, it has wrappers of standard `Type`, `FieldInfo` and `PropertyInfo` that are respectively called `ImmediateType`, `ImmediateField` and `ImmediateProperty`.

The get access to fields and properties it is like the standard way, you get access to a `Type` and then request its fields and properties.
The entry point of the library is the `TypeAccessor`.

See following examples:

```csharp
ImmediateType type = TypeAccessor.Get(typeof(MySuperType));

// or

ImmediateType type = TypeAccessor.Get<MySuperType>();
```

Note that there are other access methods that allow to get an `ImmediateType` with non public member or by specifying `BindingFlags`.

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>(includeNonPublicMembers: true);

// or

// Flags allow to get a type with member that fulfill requested flags
ImmediateType type = TypeAccessor.Get<MySuperType>(BindingFlags.Public | BindingFlags.Static);
```

IMPORTANT: In versions targeting .NET Framework 4.0 or higher and .NET Standard 2.0, there is a built-in cache behind the `TypeAccessor`.

### Getting a field or a property

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

// For fields
ImmediateField field = type.GetField("FieldName");
// or
ImmediateField field = type.Fields["FieldName"];
// There is also type.GetFields()

// For properties
ImmediateProperty property = type.GetProperty("PropertyName");
// or
ImmediateProperty property = type.Properties["PropertyName"];
// There is also type.GetProperties()
```

When you have type wrapping a field or a property you are able to get or set it like in a standard way.

```csharp
object instance = new MySuperType();

ImmediateProperty property = type.GetProperty("PropertyName");

// Get
object propertyValue = property.GetValue(instance);

// Set
property.SetValue(instance, "New Value");
```

To let the user of the library access eventual missing functionalities, each wrapping type from ImmediateReflection gives an access to the equivalent standard structure.

```csharp
ImmediateProperty property = type.GetProperty("PropertyName");

PropertyInfo propertyInfo = property.PropertyInfo;
```

---

## Target

- .NET Standard 2.0+
- .NET Core 2.0+
- .NET Framework 2.0+

Supports Source Link

---

## Dependencies

For targets higher than .NET Standard 2.0:
- System.Reflection.Emit.LightWeight

### Notes

- It uses NUnit3 for unit testing (not published).

- The library code is published annotated with JetBrains annotations that are embedded in the library. But they will **not conflict** with any of your referenced packages or project defined attributes as they are **internal** to ImmediateReflection.

---

## Installation

ImmediateReflection is available on [NuGet](https://www.nuget.org/packages/ImmediateReflection)

	PM> Install-Package ImmediateReflection

---