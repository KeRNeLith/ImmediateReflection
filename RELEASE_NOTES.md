# Release notes

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