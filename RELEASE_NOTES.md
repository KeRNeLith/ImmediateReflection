# Release notes

## What's new in 1.6.0 August 31 2020
### Fixes:
* ImmediateType properly handle type having redefined properties with a type different from initial type.

### New:
* Use signing key to strong name library assemby.

### Misc:
* JetBrains.Annotations are embedded in the assembly (internal).

## What's new in 1.5.0 January 23 2020
### Fixes:
* IsDefined/GetAttribute(s) properly handle get of attribute when dealing inheriting attributes.

### New:
* All ImmediateReflection types are serializable via C# standard serialization.

### Misc:
* JetBrains.Annotations are no more embedded in the assembly, replaced by a private reference to official NuGet.
* Minor optimization.

## What's new in 1.4.1 September 2 2019
### Fixes:
* Properly handle null parameter for Copy and TryCopy (return null).

### Changes:
* Copy and TryCopy consider string and Type as copyable types and return themselves if asked.

## What's new in 1.4.0 September 1 2019
### New:
* Add an access to the Declaring Type directly through ImmediateType, ImmediateProperty and ImmediateField.
* Add an access to the Base Type directly through ImmediateType.
* Add CopyConstructorDelegate delegate.
* Add the possibility to call Copy constructor in a faster way than Activator from ImmediateType.
* Add a type extension to check if a type has a default constructor.
* Add type extensions to check if a type has a copy constructor and to directly call it.
* Add object extensions to check if an instance can be copied by a copy constructor and to directly call it.

### Changes:
* Globally optimize the library by reducing the number of redundant null checks.
* Slightly optimize the branching in generated code.

---

## What's new in 1.3.0 July 23 2019
### New:
* Add type extensions to directly call a default constructor delegate from a Type (without ImmediateType).
* Add type extensions to directly call a constructor delegate from a Type (without ImmediateType).

### Fixes:
* ImmediateType properly handle arrays which were crashing before.

### Changes:
* Default constructor delegates available via ImmediateType are now cached and shared across several instance of ImmediateType.
* ImmediateProperty are now cached and shared across several instance of ImmediateType.
* ImmediateField are now cached and shared across several instance of ImmediateType.

---

## What's new in 1.2.0 July 18 2019
### New:
* Add ConstructorDelegate delegate.

### Fixes:
* Classes with indexed properties does not crash anymore.

### Changes:
* Lazily initialize fields property of ImmediateType.

---

## What's new in 1.1.0 June 24 2019
### Changes:
* Improve performances of memory caching within the library.
* Extend support of built-in cache to every target.
* Make some methods only available as extensions accessible as normal methods on targets not supporting extensions.
* IL generated methods are now prefixed to help identify them.

### Misc:
* API Reference and documentation generated based on sources.

---

## What's new in 1.0.0 May 31 2019
### Fixes:
* Properly supports static readonly and constant fields.
* Properly handle reflection on enumeration types.

### Changes:
* Default flags taken into account when getting an ImmediateType are Public | Instance | Static
* Get rid of cache system references replaced by a simpler internal caching system.
* Extend caching support to target .NET Framework 4.0.

### New:
* Add the possibility to call the default constructor of type in a faster way (with or without throw).
* ImmediateType provides access to every members via Members/GetMembers()/indexed member APIs.
* ImmediateType, ImmediateField and ImmediateProperty provide a faster access to attributes.
* Extensions to retrieve Immediate Reflection types from standard types.
* Extensions to retrieve attributes from standard MemberInfo.
* Provide helpers to easily create strongly typed delegate to get/set properties.
* Add an object wrapper that allows to get/set value on a specific object instance.

### Misc:
* Improve library documentation.
* Library is fully ReSharper annotated.

---

## What's new in 0.1.0 May 14 2019
* First implementation of a fast ("immediate") access and usage of C# Reflection features.
* Supports Type fields and properties getter/setter.