using System;
using System.Linq;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ImmediateFields"/>.
/// </summary>
[TestFixture]
[Parallelizable]
internal sealed class ImmediateFieldsTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void ImmediateFieldsInfo()
    {
        ImmediateFields immediateFields1 = new(SmallObjectFieldInfos);
        CollectionAssert.AreEquivalent(
            new[]
            {
                new ImmediateField(SmallObjectTestField1FieldInfo),
                new ImmediateField(SmallObjectTestField2FieldInfo)
            },
            immediateFields1);

        ImmediateFields immediateFields2 = new(SecondSmallObjectFieldInfos);
        CollectionAssert.AreNotEquivalent(immediateFields1, immediateFields2);

        ImmediateFields immediateFields3 = new(EmptyFieldInfo);
        CollectionAssert.IsEmpty(immediateFields3);
    }

    [Test]
    public static void GetField()
    {
        ImmediateFields immediateFields = new(SmallObjectFieldInfos);
        ImmediateField expectedField = new(SmallObjectTestField1FieldInfo);
        Assert.AreEqual(expectedField, immediateFields[nameof(SmallObject._testField1)]);
        Assert.AreEqual(expectedField, immediateFields.GetField(nameof(SmallObject._testField1)));

        Assert.IsNull(immediateFields["NotExists"]);
        Assert.IsNull(immediateFields.GetField("NotExists"));

        Assert.Throws<ArgumentNullException>(() => _ = immediateFields[null!]);
        Assert.Throws<ArgumentNullException>(() => _ = immediateFields.GetField(null!));
    }

    #region Equals/HashCode/ToString

    [Test]
    public static void ImmediateFieldsEquality()
    {
        ImmediateFields immediateFields1 = new(SmallObjectFieldInfos);
        ImmediateFields immediateFields2 = new(SmallObjectFieldInfos);
        Assert.IsTrue(immediateFields1.Equals(immediateFields1));
        Assert.IsTrue(immediateFields1.Equals(immediateFields2));
        Assert.IsTrue(immediateFields1.Equals((object)immediateFields2));

        ImmediateFields immediateFields3 = new(SecondSmallObjectFieldInfos);
        Assert.IsFalse(immediateFields1.Equals(immediateFields3));
        Assert.IsFalse(immediateFields1.Equals((object)immediateFields3));

        ImmediateFields immediateFields4 = new(PublicNestedFieldInfos);
        Assert.IsFalse(immediateFields4.Equals(immediateFields1));
        Assert.IsFalse(immediateFields4.Equals((object)immediateFields1));

        // Same names but not the same fields
        ImmediateFields immediateFields5 = new([SameNamesTestClassAValueFieldInfo]);
        ImmediateFields immediateFields6 = new([SameNamesTestClassBValueFieldInfo]);
        Assert.IsFalse(immediateFields5.Equals(immediateFields6));

        // Same count but not the same names
        ImmediateFields immediateFields7 = new([OtherNamesTestClassOtherValueFieldInfo]);
        Assert.IsFalse(immediateFields5.Equals(immediateFields7));

        Assert.IsFalse(immediateFields1.Equals(null));
    }

    [Test]
    public static void ImmediateFieldsHashCode()
    {
        ImmediateFields immediateFields1 = new(SmallObjectFieldInfos);
        ImmediateFields immediateFields2 = new(SmallObjectFieldInfos);
        Assert.AreEqual(immediateFields1.GetHashCode(), immediateFields2.GetHashCode());

        ImmediateFields immediateFields3 = new(SecondSmallObjectFieldInfos);
        Assert.AreNotEqual(immediateFields1.GetHashCode(), immediateFields3.GetHashCode());
    }

    [Test]
    public static void ImmediateFieldsToString()
    {
        ImmediateFields immediateFields1 = new(SmallObjectFieldInfos);
        string expectedToString = $"[{string.Join(", ", SmallObjectFieldInfos.Select(p => p.ToString()).ToArray())}]";
        Assert.AreEqual(expectedToString, immediateFields1.ToString());

        ImmediateFields immediateFields2 = new(EmptyFieldInfo);
        Assert.AreEqual("[]", immediateFields2.ToString());
    }

    #endregion
}