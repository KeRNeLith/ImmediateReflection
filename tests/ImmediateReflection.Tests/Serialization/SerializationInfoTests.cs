#pragma warning disable SYSLIB0050, SYSLIB0051 // Formatter based serialization is what is tested
using System;
using System.Reflection;
using System.Runtime.Serialization;
using NUnit.Framework;
using static ImmediateReflection.Tests.SerializationHelpersTests;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests relative to the (de)serialization from a <see cref="SerializationInfo"/>, without any formatter
/// so it can run on every target (including the ones without a binary formatter).
/// </summary>
[TestFixture]
internal sealed class SerializationInfoTests : ImmediateReflectionTestsBase
{
    #region Test helpers

    private static T Deserialize<T>(SerializationInfo info)
    {
        ConstructorInfo constructor = typeof(T).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [typeof(SerializationInfo), typeof(StreamingContext)],
            null) ?? throw new AssertionException("Cannot find serialization constructor.");

        try
        {
            return (T)constructor.Invoke([info, new StreamingContext()]);
        }
        catch (TargetInvocationException ex)
        {
            // Rethrow the real error
            throw ex.InnerException!;
        }
    }

    private static SerializationInfo GetObjectData(ISerializable serializable)
    {
        SerializationInfo info = CreateInfo(serializable.GetType());
        serializable.GetObjectData(info, new StreamingContext());
        return info;
    }

    #endregion

    [Test]
    public static void ImmediateFieldSerializationInfo()
    {
        ImmediateField field = new(SmallObjectTestField1FieldInfo);
        SerializationInfo info = GetObjectData(field);

        ImmediateField deserialized = Deserialize<ImmediateField>(info);
        Assert.AreEqual(field, deserialized);
    }

    [Test]
    public static void ImmediatePropertySerializationInfo()
    {
        ImmediateProperty property = new(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
        SerializationInfo info = GetObjectData(property);

        ImmediateProperty deserialized = Deserialize<ImmediateProperty>(info);
        Assert.AreEqual(property, deserialized);
    }

    [Test]
    public static void ImmediateFieldsSerializationInfo()
    {
        ImmediateFields fields = new(SmallObjectFieldInfos);
        SerializationInfo info = GetObjectData(fields);

        ImmediateFields deserialized = Deserialize<ImmediateFields>(info);
        Assert.AreEqual(fields, deserialized);

        // Invalid data are refused
        SerializationInfo duplicated = CreateMembersInfo(
            typeof(ImmediateFields),
            "Field",
            SmallObjectTestField1FieldInfo,
            SmallObjectTestField1FieldInfo);
        Assert.Throws<SerializationException>(() => Deserialize<ImmediateFields>(duplicated));
    }

    [Test]
    public static void ImmediatePropertiesSerializationInfo()
    {
        ImmediateProperties properties = new(
        [
            PublicValueTypePublicGetSetPropertyPropertyInfo,
            PublicReferenceTypePublicGetPropertyPropertyInfo
        ]);
        SerializationInfo info = GetObjectData(properties);

        ImmediateProperties deserialized = Deserialize<ImmediateProperties>(info);
        Assert.AreEqual(properties, deserialized);

        // Invalid data are refused
        SerializationInfo duplicated = CreateMembersInfo(
            typeof(ImmediateProperties),
            "Property",
            PublicValueTypePublicGetSetPropertyPropertyInfo,
            PublicValueTypePublicGetSetPropertyPropertyInfo);
        Assert.Throws<SerializationException>(() => Deserialize<ImmediateProperties>(duplicated));
    }

    [Test]
    public static void ImmediateTypeSerializationInfo()
    {
        ImmediateType type = new(typeof(TestObject), BindingFlags.Public | BindingFlags.Instance);
        SerializationInfo info = GetObjectData(type);

        ImmediateType deserialized = Deserialize<ImmediateType>(info);
        Assert.AreEqual(type, deserialized);

        // Invalid data are refused
        SerializationInfo invalidType = CreateInfo(typeof(ImmediateType));
        invalidType.AddValue("Type", null, typeof(Type));
        invalidType.AddValue("Flags", BindingFlags.Public);
        Assert.Throws<SerializationException>(() => Deserialize<ImmediateType>(invalidType));

        SerializationInfo invalidFlags = CreateInfo(typeof(ImmediateType));
        invalidFlags.AddValue("Type", typeof(TestObject));
        invalidFlags.AddValue("Flags", (BindingFlags)(1 << 30));
        Assert.Throws<SerializationException>(() => Deserialize<ImmediateType>(invalidFlags));
    }
}