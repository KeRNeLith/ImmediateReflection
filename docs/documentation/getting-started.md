# Getting started

## Reflection

The library is pretty simple to use, it has wrappers of standard `Type`, `FieldInfo` and `PropertyInfo` that are respectively called `ImmediateType`, `ImmediateField` and `ImmediateProperty`.

The get access to fields and properties it is like the standard way, you get access to a `Type` and then request its fields and properties.
The entry point of the library is the `TypeAccessor` which allows to get an `ImmediateType`.

See the [ImmediateType](immediate-type.md) section for more details.

## Delegates

The library also provides the possibility to create strongly typed get/set "open" delegates.

See the [Open delegates](open-delegates.md) section for more details.