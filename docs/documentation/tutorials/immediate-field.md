# Getting a field

```csharp
ImmediateType type = TypeAccessor.Get<MySuperType>();

ImmediateField field = type.GetField("_fieldName");
// or
ImmediateField field = type.Fields["_fieldName"];
// There is also type.GetFields()
```

When you have type wrapping a field you are able to get or set it like in a standard way.

```csharp
object instance = new MySuperType();

ImmediateField field = type.GetField("_fieldName");

// Get
object fieldValue = field.GetValue(instance);

// Set
field.SetValue(instance, "New Value");
```

To let the user of the library access eventual missing functionalities, each wrapping type from ImmediateReflection gives an access to the equivalent standard structure.

```csharp
ImmediateField field = type.GetField("_fieldName");

FieldInfo fieldInfo = field.FieldInfo;
```