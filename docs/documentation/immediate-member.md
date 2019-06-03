# ImmediateMember

Both `ImmediateType`, `ImmediateField` and `ImmediateProperty` inherit from `ImmediateMember` which provide an API to check/get attributes that are applied respectively to a type, field or a property.

## Get a member

An `ImmediateType` gives access to its members meaning fields and properties.

Here are some examples:

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

IEnumerable<ImmediateMember> members = type.Members;
// or
IEnumerable<ImmediateMember> members = type.GetMembers();

// For a member
ImmediateMember member = type.GetMember("MemberName");
// or
ImmediateMember member = type["MemberName"];
```

## Get attributes

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