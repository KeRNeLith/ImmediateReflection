# Object wrapper

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