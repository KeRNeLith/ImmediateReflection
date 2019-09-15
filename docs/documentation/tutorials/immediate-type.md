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
```
## Copy an instance

The `ImmediateType` allows to create a copy of a given instance via a copy constructor if available. This feature is faster than making a traditional call to `Activator.CreateInstance(Type, Instance)`.

Here is a quick example:

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

MySuperType instance = new MySuperType
{
    TestProperty = 12
};

// Create a copy instance of MySuperType
object newInstance = type.Copy(instance);

// You can also use the version that not throws in case of failure
bool succeed = type.TryCopy(instance, out object newInstance, out Exception _);
```

Note also that a more easy way of using copy is available as extension directly when manipulating an instance.

```csharp
MySuperType instance = new MySuperType
{
    TestProperty = 12
};

// Create a copy instance of MySuperType
MySuperType newInstance = instance.Copy();
```

Obviously in such situation you would have directly called the copy constructor of `MySuperType`, but we have to keep in mind that it is designed to be use when the instance we manipulate has not been created in such explicit way.
