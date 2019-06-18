# ImmediateType and TypeAccessor

## Getting a type

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

Note: there is a built-in cache behind the `TypeAccessor`.

## Instantiate a type

The `ImmediateType` allows to instantiate types via their default constructor if available. This feature is faster than making a traditional call to `Activator.CreateInstance(Type)`.

Here is a quick example:

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

// Create a new instance of MySuperType
object newInstance = type.New();

// You can also use the version that not throws in case of failure
bool succeed = type.TryNew(out object instance, out Exception _);