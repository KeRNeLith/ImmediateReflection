# Getting a property

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

ImmediateProperty property = type.GetProperty("PropertyName");
// or
ImmediateProperty property = type.Properties["PropertyName"];
// There is also type.GetProperties()
```

When you have type wrapping a property you are able to get or set it like in a standard way.

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