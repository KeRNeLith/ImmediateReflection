#pragma warning disable SYSLIB0050 // Formatter based serialization is what is tested
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using ImmediateReflection.Utils;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="SerializationHelpers"/>.
/// </summary>
[TestFixture]
internal sealed class SerializationHelpersTests : ImmediateReflectionTestsBase
{
    #region Test helpers

    internal static SerializationInfo CreateInfo(Type type)
    {
        return new SerializationInfo(type, new FormatterConverter());
    }

    internal static SerializationInfo CreateMembersInfo(Type type, string prefix, params MemberInfo?[] members)
    {
        SerializationInfo info = CreateInfo(type);
        info.AddValue("Count", members.Length);
        for (int i = 0; i < members.Length; ++i)
        {
            info.AddValue($"{prefix}{i}", members[i], members[i]?.GetType() ?? typeof(MemberInfo));
        }

        return info;
    }

    #endregion

    [Test]
    public static void ReadType()
    {
        SerializationInfo info = CreateInfo(typeof(ImmediateType));
        info.AddValue("Type", typeof(TestObject));
        Assert.AreEqual(typeof(TestObject), SerializationHelpers.ReadType(info, "Type"));

        // Null type
        info.AddValue("Null", null, typeof(Type));
        Assert.Throws<SerializationException>(() => SerializationHelpers.ReadType(info, "Null"));

        Assert.Throws<ArgumentNullException>(() => SerializationHelpers.ReadType(null, "Type"));
    }

    [Test]
    public static void ReadFlags()
    {
        SerializationInfo info = CreateInfo(typeof(ImmediateType));
        info.AddValue("Flags", TypeAccessor.DefaultFlags);
        Assert.AreEqual(TypeAccessor.DefaultFlags, SerializationHelpers.ReadFlags(info, "Flags"));

        // Null flags
        info.AddValue("Null", null, typeof(BindingFlags));
        Assert.Throws<SerializationException>(() => SerializationHelpers.ReadFlags(info, "Null"));

        // Unknown flags
        info.AddValue("Invalid", (BindingFlags)(1 << 30));
        Assert.Throws<SerializationException>(() => SerializationHelpers.ReadFlags(info, "Invalid"));

        Assert.Throws<ArgumentNullException>(() => SerializationHelpers.ReadFlags(null, "Flags"));
    }

    [Test]
    public static void ReadMembers()
    {
        // Valid
        SerializationInfo info = CreateMembersInfo(
            typeof(ImmediateFields),
            "Field",
            SmallObjectTestField1FieldInfo,
            SmallObjectTestField2FieldInfo);
        List<FieldInfo> fields = SerializationHelpers.ReadMembers<FieldInfo>(info, "Field");
        CollectionAssert.AreEqual(new[] { SmallObjectTestField1FieldInfo, SmallObjectTestField2FieldInfo }, fields);

        // Empty
        Assert.IsEmpty(SerializationHelpers.ReadMembers<FieldInfo>(CreateMembersInfo(typeof(ImmediateFields), "Field"), "Field"));

        // Invalid counts
        foreach (int count in new[] { -1, 65536 })
        {
            SerializationInfo invalidCount = CreateInfo(typeof(ImmediateFields));
            invalidCount.AddValue("Count", count);
            Assert.Throws<SerializationException>(() => SerializationHelpers.ReadMembers<FieldInfo>(invalidCount, "Field"));
        }

        // Null member
        SerializationInfo nullMember = CreateMembersInfo(typeof(ImmediateFields), "Field", SmallObjectTestField1FieldInfo, null);
        Assert.Throws<SerializationException>(() => SerializationHelpers.ReadMembers<FieldInfo>(nullMember, "Field"));

        // Duplicated member
        SerializationInfo duplicated = CreateMembersInfo(
            typeof(ImmediateFields),
            "Field",
            SmallObjectTestField1FieldInfo,
            SmallObjectTestField1FieldInfo);
        Assert.Throws<SerializationException>(() => SerializationHelpers.ReadMembers<FieldInfo>(duplicated, "Field"));

        Assert.Throws<ArgumentNullException>(() => SerializationHelpers.ReadMembers<FieldInfo>(null, "Field"));
    }
}