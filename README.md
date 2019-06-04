| | |
| --- | --- |
| **Build** | [![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/github/KeRNeLith/ImmediateReflection?branch=master&svg=true)](https://ci.appveyor.com/project/KeRNeLith/ImmediateReflection) |
| **Coverage** | <sup>Coveralls</sup> [![Coverage Status](https://coveralls.io/repos/github/KeRNeLith/ImmediateReflection/badge.svg?branch=master)](https://coveralls.io/github/KeRNeLith/ImmediateReflection?branch=master) <sup>SonarQube</sup> [![SonarCloud Coverage](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=coverage)](https://sonarcloud.io/component_measures/metric/coverage/list?id=immediate_reflection) | 
| **Quality** | [![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=immediate_reflection&metric=alert_status)](https://sonarcloud.io/dashboard?id=immediate_reflection) | 
| **Nuget** | [![Nuget downloads](https://img.shields.io/nuget/v/immediatereflection.svg)](https://www.nuget.org/packages/ImmediateReflection) |
| **License** | [![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/KeRNeLith/ImmediateReflection/blob/master/LICENSE) |

# ImmediateReflection

## What is **ImmediateReflection**?

This is .NET library that aims to provide a **faster** usage of **C# reflection** features. 
Especially the usage of constructor, members accessors (get/set) and attributes.

It provides these features while trying to keep an API as similar as the standard Reflection API (**Fully documented** and **ReSharper compliant**).

To see how **powerful** it can be **[here](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks.md)** are some benchmarks of the library.

Other benchmarks:
- multiple get/set of multiple types to avoid eventual processor caching issues, see [there](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks_Multi.md).
- constructor calls [there](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks_Constructor.md).
- multiple get of attribute [there](https://github.com/KeRNeLith/ImmediateReflection/blob/master/Benchmarks_Attributes.md).

The library is highly tested to cover as much as possible real cases, because using Reflection is some kind of core code and must be reliable to build on it.

## Getting started

See the library [documentation](https://kernelith.github.io/ImmediateReflection/).

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

### Instantiate a type

The `ImmediateType` allows to instantiate types via their default constructor if available. This feature is faster than making a traditional call to `Activator.CreateInstance(Type)`.

Here is a quick example:

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

// Create a new instance of MySuperType
object newInstance = type.New();

// You can also use the version that not throws in case of failure
bool succeed = type.TryNew(out object instance, out Exception _);
```

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

// For all members
IEnumerable<ImmediateMember> members = type.Members;
// or
IEnumerable<ImmediateMember> members = type.GetMembers();

// For a member
ImmediateMember member = type.GetMember("MemberName");
// or
ImmediateMember member = type["MemberName"];
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

### Getting attributes

Both `ImmediateType`, `ImmediateField` and `ImmediateProperty` inherit from `ImmediateMember` which provide an API to check/get attributes that are applied respectively to a type, field or a property.

All methods are accessible in their templated and not templated versions.
Following some examples of accessible methods:

```csharp
ImmediateType type = ...;
bool hasAttribute = type.IsDefined<MyAttribute>();

ImmediateField field = ...;
MyAttribute attribute = field.GetAttribute<MyAttribute>(inherit: true);

ImmediateProperty property = ...;
IEnumerable<Attribute> attributes = property.GetAttributes(typeof(MyAttribute));

IEnumerable<Attribute> attributes = type.GetAllAttributes(inherit: true);
```

It is also possible to directly retrieve attributes of a given `MemberInfo` from the built in cache if target is higher than .NET Framework 4.0.

```csharp
PropertyInfo property = ...;
bool hasAttribute = property.IsDefinedImmediateAttribute<MyAttribute>();

FieldInfo field = ...;
MyAttribute attribute = field.GetImmediateAttribute<MyAttribute>();
```

### Object wrapper

By using `ImmediateType` API you can manipulate get/set on object via "open" methods, meaning you can specify the instance on which applying the method.

ImmediateReflection also provides an `ObjectWrapper` that does the same job as `ImmediateType` but on a "closed" way. It means that get/set will be applied only on the wrapped instance.

Following a quick example:

```csharp
MyClass myObject = new MyClass();

ObjectWrapper wrapper = new ObjectWrapper(myObject);

// Properties/Fields
ImmediateField field = wrapper.GetField("_myField");
ImmediateProperty property = wrapper.GetProperty("MyProperty");

// Get
object propertyValue = wrapper.GetPropertyValue("MyProperty");

// Set
wrapper.SetPropertyValue("MyOtherProperty", 42);    // myObject.MyOtherProperty = 42
```

Note that the wrapper gives access to the wrapped object, its `Type`, `ImmediateType` and public members.

### Creating typed delegate (Open delegate)

ImmediateReflection provides an API like standard one for `Type`, `FieldInfo` and `PropertyInfo`, this means get/set for properties use `object` both for target and parameter/return type.

But in some cases you know the type owning a property, or better the type of the property too.

To answer these cases ImmediateReflection provides extensions to `PropertyInfo` that allow you to create strongly typed delegates for an even faster get/set of properties.

See some of the following examples:

```csharp
class MyType
{
    int MyProperty { get; set; }

    string MyStringProperty { get; set; }
}

PropertyInfo myProperty = typeof(MyType).GetProperty(nameof(MyType.MyProperty));
GetterDelegate<MyType, int> getter = myProperty.CreateGetter<MyType, int>();

// Notice that this method can throw if passing invalid types
// There is also a try version
bool succeed = myProperty.TryCreateGetter(out GetterDelegate<MyType, int> getter);

// Then you can use this getter simply like this
MyType myObject = new MyType { MyProperty = 12 };
int value = getter(myObject);  // 12


// Note that the same exists for setter
PropertyInfo myStringProperty = typeof(MyType).GetProperty(nameof(MyType.MyStringProperty));
SetterDelegate<MyType, string> setter = myProperty.CreateSetter<MyType, string>();
// Or
bool succeed = myProperty.TryCreateSetter(out SetterDelegate<MyType, string> setter);

// Then you can use this getter simply like this
MyType myObject = new MyType { MyStringProperty = "Init" };
setter(myObject, "New value");  // Sets myObject.MyStringProperty to "New value"
```

If you only knows the owner type then you can use the alternative version of these delegate helpers that will use object for the property value.

```csharp
PropertyInfo myProperty = typeof(MyType).GetProperty(nameof(MyType.MyProperty));
GetterDelegate<MyType> getter = myProperty.CreateGetter<MyType>();

// Notice that this method can throw if passing invalid types
// There is also a try version
bool succeed = myProperty.TryCreateGetter(out GetterDelegate<MyType> getter);

// Then you can use this getter simply like this
MyType myObject = new MyType { MyProperty = 12 };
object value = getter(myObject);  // 12 wrapped in an object


// Note that the same exists for setter
PropertyInfo myStringProperty = typeof(MyType).GetProperty(nameof(MyType.MyStringProperty));
SetterDelegate<MyType> setter = myProperty.CreateSetter<MyType>();
// Or
bool succeed = myProperty.TryCreateSetter(out SetterDelegate<MyType> setter);

// Then you can use this getter simply like this
MyType myObject = new MyType { MyStringProperty = "Init" };
setter(myObject, "New value");  // Sets myObject.MyStringProperty to "New value"
```

You can then stores these delegate to boost your reflection get/set over properties.

### Extensions

The library also provides some extensions for standard types to easily get Immediate Reflection types.

Like those:

```csharp
Type myType = ...;

ImmediateType myImmediateType = myType.GetImmediateType();
```

---

## Targets

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