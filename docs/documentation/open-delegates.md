# Creating typed delegate (Open delegate)

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