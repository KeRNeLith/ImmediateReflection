using System;
using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ImmediateProperties"/>.
/// </summary>
[TestFixture]
[Parallelizable]
internal sealed class ImmediatePropertiesTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void ImmediatePropertiesInfo()
    {
        ImmediateProperties immediateProperties1 = new(SmallObjectPropertyInfos);
        CollectionAssert.AreEquivalent(
            new[]
            {
                new ImmediateProperty(SmallObjectTestProperty1PropertyInfo),
                new ImmediateProperty(SmallObjectTestProperty2PropertyInfo)
            },
            immediateProperties1);

        ImmediateProperties immediateProperties2 = new(SecondSmallObjectPropertyInfos);
        CollectionAssert.AreNotEquivalent(immediateProperties1, immediateProperties2);

        ImmediateProperties immediateProperties3 = new(EmptyPropertyInfo);
        CollectionAssert.IsEmpty(immediateProperties3);
    }

    [Test]
    public static void ImmediatePropertiesInfoWithNew()
    {
        ImmediateProperties immediateProperties = new([ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo, BaseClassPublicGetPropertyPropertyInfo]);
        CollectionAssert.AreEquivalent(
            new[] { new ImmediateProperty(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo) },
            immediateProperties);

        immediateProperties = new ImmediateProperties([BaseClassPublicGetPropertyPropertyInfo, ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo]);
        CollectionAssert.AreEquivalent(
            new[] { new ImmediateProperty(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo) },
            immediateProperties);
    }

    [Test]
    public static void GetProperty()
    {
        ImmediateProperties immediateProperties = new(SmallObjectPropertyInfos);
        ImmediateProperty expectedProperty = new(SmallObjectTestProperty1PropertyInfo);
        Assert.AreEqual(expectedProperty, immediateProperties[nameof(SmallObject.TestProperty1)]);
        Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(SmallObject.TestProperty1)));

        Assert.IsNull(immediateProperties["NotExists"]);
        Assert.IsNull(immediateProperties.GetProperty("NotExists"));

        Assert.Throws<ArgumentNullException>(() => _ = immediateProperties[null!]);
        Assert.Throws<ArgumentNullException>(() => _ = immediateProperties.GetProperty(null!));
    }

    [Test]
    public static void GetPropertyWithNew()
    {
        ImmediateProperties immediateProperties = new([ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo, BaseClassPublicGetPropertyPropertyInfo]);
        ImmediateProperty expectedProperty = new(ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo);
        Assert.AreEqual(expectedProperty, immediateProperties[nameof(ChildTypeRedefinitionTestClass.Property)]);
        Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(ChildTypeRedefinitionTestClass.Property)));

        immediateProperties = new ImmediateProperties([BaseClassPublicGetPropertyPropertyInfo, ChildTypeRedefinitionClassPublicGetPropertyPropertyInfo]);
        Assert.AreEqual(expectedProperty, immediateProperties[nameof(ChildTypeRedefinitionTestClass.Property)]);
        Assert.AreEqual(expectedProperty, immediateProperties.GetProperty(nameof(ChildTypeRedefinitionTestClass.Property)));
    }

    #region Equals/HashCode/ToString

    [Test]
    public static void ImmediatePropertiesEquality()
    {
        ImmediateProperties immediateProperties1 = new(SmallObjectPropertyInfos);
        ImmediateProperties immediateProperties2 = new(SmallObjectPropertyInfos);
        Assert.IsTrue(immediateProperties1.Equals(immediateProperties1));
        Assert.IsTrue(immediateProperties1.Equals(immediateProperties2));
        Assert.IsTrue(immediateProperties1.Equals((object)immediateProperties2));

        ImmediateProperties immediateProperties3 = new(SecondSmallObjectPropertyInfos);
        Assert.IsFalse(immediateProperties1.Equals(immediateProperties3));
        Assert.IsFalse(immediateProperties1.Equals((object)immediateProperties3));

        ImmediateProperties immediateProperties4 = new(PublicNestedPropertyInfos);
        Assert.IsFalse(immediateProperties4.Equals(immediateProperties1));
        Assert.IsFalse(immediateProperties4.Equals((object)immediateProperties1));

        // Same names but not the same properties
        ImmediateProperties immediateProperties5 = new([SameNamesTestClassAPropPropertyInfo]);
        ImmediateProperties immediateProperties6 = new([SameNamesTestClassBPropPropertyInfo]);
        Assert.IsFalse(immediateProperties5.Equals(immediateProperties6));

        // Same count but not the same names
        ImmediateProperties immediateProperties7 = new([OtherNamesTestClassOtherPropPropertyInfo]);
        Assert.IsFalse(immediateProperties5.Equals(immediateProperties7));

        Assert.IsFalse(immediateProperties1.Equals(null));
    }

    [Test]
    public static void ImmediatePropertiesHashCode()
    {
        ImmediateProperties immediateProperties1 = new(SmallObjectPropertyInfos);
        ImmediateProperties immediateProperties2 = new(SmallObjectPropertyInfos);
        Assert.AreEqual(immediateProperties1.GetHashCode(), immediateProperties2.GetHashCode());

        ImmediateProperties immediateProperties3 = new(SecondSmallObjectPropertyInfos);
        Assert.AreNotEqual(immediateProperties1.GetHashCode(), immediateProperties3.GetHashCode());
    }

    [Test]
    public static void ImmediatePropertiesToString()
    {
        ImmediateProperties immediateProperties1 = new(SmallObjectPropertyInfos);
        string expectedToString = $"[{string.Join(", ", SmallObjectPropertyInfos.Select(p => p.ToString()).ToArray())}]";
        Assert.AreEqual(expectedToString, immediateProperties1.ToString());

        ImmediateProperties immediateProperties2 = new(EmptyPropertyInfo);
        Assert.AreEqual("[]", immediateProperties2.ToString());
    }

    #endregion
}