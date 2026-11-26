using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using static ImmediateReflection.Utils.ReflectionHelpers;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ImmediateReflection.Utils.ReflectionHelpers"/> member hashcodes and comparers.
/// </summary>
/// <remarks>
/// Under NET5.0+ inherited members have a distinct <see cref="MemberInfo"/> instance per reflected type but share
/// the same default hashcode (https://github.com/dotnet/runtime/issues/114280).
/// </remarks>
[TestFixture]
internal sealed class ReflectionHelpersTests : ImmediateReflectionTestsBase
{
    #region Test classes

    // ReSharper disable UnusedMember.Global

    public class HashBase
    {
        public int Field = 1;
        public static int StaticField = 1;
        public int Plain { get; set; }
        public int Property { get; set; }
        public static int StaticProperty { get; set; }
        protected int ProtectedProperty { get; set; }
        public virtual int VirtualProperty { get; set; }

        public int this[int index]
        {
            get => index;
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }
    }

    public class HashDerived<T> : HashBase
    {
        public override int VirtualProperty { get; set; }
        public new int Property { get; set; }
        public T? Own { get; set; }
    }

    // ReSharper disable once UnusedTypeParameter
    public class HashWrapper<T>;

    // ReSharper restore UnusedMember.Global

    #endregion

    #region Test helpers

    private const BindingFlags AllMembers =
        BindingFlags.Public
        | BindingFlags.NonPublic
        | BindingFlags.Instance
        | BindingFlags.Static
        | BindingFlags.FlattenHierarchy;

    private const int TypesCount = 150;

    // Builds distinct closed generic types deriving from HashBase
    private static Type[] CreateDerivedTypes()
    {
        var arguments = new List<Type> { typeof(int) };
        while (arguments.Count < TypesCount)
        {
            arguments.Add(typeof(HashWrapper<>).MakeGenericType(arguments[arguments.Count - 1]));
        }
        return arguments.Select(argument => typeof(HashDerived<>).MakeGenericType(argument)).ToArray();
    }

    // Gets the member declared in HashBase as seen from each given type
    private static PropertyInfo[] GetInheritedProperties(Type[] types, string name)
    {
        return types
            .SelectMany(type => type.GetProperties(AllMembers).Where(property => name.Equals(property.Name, StringComparison.Ordinal)))
            .Where(property => property.DeclaringType == typeof(HashBase))
            .ToArray();
    }

    private static FieldInfo[] GetInheritedFields(Type[] types, string name)
    {
        return types
            .SelectMany(type => type.GetFields(AllMembers).Where(field => name.Equals(field.Name, StringComparison.Ordinal)))
            .Where(field => field.DeclaringType == typeof(HashBase))
            .ToArray();
    }

    #endregion

    #region PropertyInfo

    private static IEnumerable<TestCaseData> CreateInheritedPropertyCases()
    {
        yield return new TestCaseData(nameof(HashBase.Plain));
        yield return new TestCaseData(nameof(HashBase.StaticProperty));
        yield return new TestCaseData("ProtectedProperty");
        yield return new TestCaseData("Item");
    }

    [TestCaseSource(nameof(CreateInheritedPropertyCases))]
    public static void PropertyInfoHashCodeInherited(string name)
    {
        Type[] types = CreateDerivedTypes();
        PropertyInfo[] properties = GetInheritedProperties(types, name);
        Assert.AreEqual(types.Length, properties.Length);

        // Hashcodes must not collapse to a handful of values (one per reflected type is expected)
        int distinctHashes = properties.Select(GetPropertyInfoHashCode).Distinct().Count();
        Assert.GreaterOrEqual(distinctHashes, properties.Length * 0.95, $"{name}: {distinctHashes} hashes for {properties.Length} properties.");
    }

    [Test]
    public static void PropertyInfoHashCodeStability()
    {
        PropertyInfo property1 = typeof(HashDerived<int>).GetProperty(nameof(HashBase.Plain))!;
        PropertyInfo property2 = typeof(HashDerived<int>).GetProperty(nameof(HashBase.Plain), BindingFlags.Public | BindingFlags.Instance)!;
        Assert.AreEqual(property1, property2);
        Assert.AreEqual(GetPropertyInfoHashCode(property1), GetPropertyInfoHashCode(property2));
    }

    [Test]
    public static void PropertyInfoHashCodeHiddenAndOverridden()
    {
        Type type = typeof(HashDerived<int>);
        const BindingFlags declaredOnly = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        PropertyInfo hiding = type.GetProperty(nameof(HashBase.Property), declaredOnly)!;
        PropertyInfo hidden = typeof(HashBase).GetProperty(nameof(HashBase.Property))!;
        PropertyInfo overriding = type.GetProperty(nameof(HashBase.VirtualProperty), declaredOnly)!;
        PropertyInfo overridden = typeof(HashBase).GetProperty(nameof(HashBase.VirtualProperty))!;

        Assert.AreNotEqual(GetPropertyInfoHashCode(hiding), GetPropertyInfoHashCode(hidden));
        Assert.AreNotEqual(GetPropertyInfoHashCode(overriding), GetPropertyInfoHashCode(overridden));
    }

    [Test]
    public static void PropertyInfoEqualityComparerAsKeyComparer()
    {
        PropertyInfo[] properties = CreateDerivedTypes()
            .SelectMany(type => type.GetProperties(AllMembers))
            .ToArray();

        // Same kind of storage as the caches
        Hashtable table = new(new PropertyInfoEqualityComparer());
        for (int i = 0; i < properties.Length; ++i)
        {
            table[properties[i]] = i;
        }

        Assert.AreEqual(properties.Distinct().Count(), table.Count);
        for (int i = 0; i < properties.Length; ++i)
        {
            Assert.AreEqual(i, table[properties[i]]);
        }
    }

    [Test]
    public static void PropertyInfoEqualityComparerMembers()
    {
        PropertyInfo property = SameNamesTestClassAPropPropertyInfo;
        PropertyInfo otherProperty = SameNamesTestClassBPropPropertyInfo;

        PropertyInfoEqualityComparer comparer = new();
        Assert.IsTrue(comparer.Equals(property, property));
        Assert.IsFalse(comparer.Equals(property, otherProperty));
        Assert.AreEqual(GetPropertyInfoHashCode(property), comparer.GetHashCode(property));

        // Non generic implementation
        IEqualityComparer nonGeneric = comparer;
        Assert.IsTrue(nonGeneric.Equals(property, property));
        Assert.IsFalse(nonGeneric.Equals(property, otherProperty));
        Assert.AreEqual(GetPropertyInfoHashCode(property), nonGeneric.GetHashCode(property));
    }

    #endregion

    #region FieldInfo

    private static IEnumerable<TestCaseData> CreateInheritedFieldCases()
    {
        yield return new TestCaseData(nameof(HashBase.Field));
        yield return new TestCaseData(nameof(HashBase.StaticField));
    }

    [TestCaseSource(nameof(CreateInheritedFieldCases))]
    public static void FieldInfoHashCodeInherited(string name)
    {
        Type[] types = CreateDerivedTypes();
        FieldInfo[] fields = GetInheritedFields(types, name);
        Assert.AreEqual(types.Length, fields.Length);

        int distinctHashes = fields.Select(GetFieldInfoHashCode).Distinct().Count();
        Assert.GreaterOrEqual(distinctHashes, fields.Length * 0.95, $"{name}: {distinctHashes} hashes for {fields.Length} fields.");
    }

    [Test]
    public static void FieldInfoHashCodeStability()
    {
        FieldInfo field1 = typeof(HashDerived<int>).GetField(nameof(HashBase.Field))!;
        FieldInfo field2 = typeof(HashDerived<int>).GetField(nameof(HashBase.Field), BindingFlags.Public | BindingFlags.Instance)!;
        Assert.AreEqual(field1, field2);
        Assert.AreEqual(GetFieldInfoHashCode(field1), GetFieldInfoHashCode(field2));
    }

    [Test]
    public static void FieldInfoEqualityComparerAsKeyComparer()
    {
        FieldInfo[] fields = CreateDerivedTypes()
            .SelectMany(type => type.GetFields(AllMembers))
            .ToArray();

        Hashtable table = new(new FieldInfoEqualityComparer());
        for (int i = 0; i < fields.Length; ++i)
        {
            table[fields[i]] = i;
        }

        Assert.AreEqual(fields.Distinct().Count(), table.Count);
        for (int i = 0; i < fields.Length; ++i)
        {
            Assert.AreEqual(i, table[fields[i]]);
        }
    }

    [Test]
    public static void FieldInfoEqualityComparerMembers()
    {
        FieldInfo field = SameNamesTestClassAValueFieldInfo;
        FieldInfo otherField = SameNamesTestClassBValueFieldInfo;

        FieldInfoEqualityComparer comparer = new();
        Assert.IsTrue(comparer.Equals(field, field));
        Assert.IsFalse(comparer.Equals(field, otherField));
        Assert.AreEqual(GetFieldInfoHashCode(field), comparer.GetHashCode(field));

        // Non generic implementation
        IEqualityComparer nonGeneric = comparer;
        Assert.IsTrue(nonGeneric.Equals(field, field));
        Assert.IsFalse(nonGeneric.Equals(field, otherField));
        Assert.AreEqual(GetFieldInfoHashCode(field), nonGeneric.GetHashCode(field));
    }

    #endregion

    #region MemberInfo

    [Test]
    public static void MemberInfoHashCode()
    {
        PropertyInfo property = typeof(HashDerived<int>).GetProperty(nameof(HashBase.Plain))!;
        FieldInfo field = typeof(HashDerived<int>).GetField(nameof(HashBase.Field))!;

        // Properties and fields use their specialized hashcode, other kinds (like types) the default one
        Assert.AreEqual(GetPropertyInfoHashCode(property), GetMemberInfoHashCode(property));
        Assert.AreEqual(GetFieldInfoHashCode(field), GetMemberInfoHashCode(field));
        Assert.AreEqual(typeof(HashBase).GetHashCode(), GetMemberInfoHashCode(typeof(HashBase)));
    }

    [Test]
    public static void MemberInfoEqualityComparer()
    {
        MemberInfoEqualityComparer comparer = new();
        PropertyInfo property = typeof(HashDerived<int>).GetProperty(nameof(HashBase.Plain))!;
        FieldInfo field = typeof(HashDerived<int>).GetField(nameof(HashBase.Field))!;

        Assert.IsTrue(comparer.Equals(property, typeof(HashDerived<int>).GetProperty(nameof(HashBase.Plain))));
        Assert.IsFalse(comparer.Equals(property, typeof(HashDerived<long>).GetProperty(nameof(HashBase.Plain))));
        Assert.IsFalse(comparer.Equals(property, field));
        Assert.AreEqual(GetMemberInfoHashCode(field), comparer.GetHashCode(field));
    }

    [Test]
    public static void MemberInfoEqualityComparerNonGeneric()
    {
        PropertyInfo property = SameNamesTestClassAPropPropertyInfo;
        PropertyInfo otherProperty = SameNamesTestClassBPropPropertyInfo;

        IEqualityComparer nonGeneric = new MemberInfoEqualityComparer();
        Assert.IsTrue(nonGeneric.Equals(property, property));
        Assert.IsFalse(nonGeneric.Equals(property, otherProperty));
        Assert.AreEqual(GetMemberInfoHashCode(property), nonGeneric.GetHashCode(property));
    }

    #endregion

    #region Immediate members

    [Test]
    public static void ImmediateMembersHashCodeInherited()
    {
        Type[] types = CreateDerivedTypes();

        ImmediateProperty[] properties = types
            .Select(type => TypeAccessor.Get(type).GetProperty(nameof(HashBase.Plain))!)
            .ToArray();
        ImmediateField[] fields = types
            .Select(type => TypeAccessor.Get(type).GetField(nameof(HashBase.Field))!)
            .ToArray();

        Assert.GreaterOrEqual(properties.Select(property => property.GetHashCode()).Distinct().Count(), types.Length * 0.95);
        Assert.GreaterOrEqual(fields.Select(field => field.GetHashCode()).Distinct().Count(), types.Length * 0.95);
    }

    #endregion
}
