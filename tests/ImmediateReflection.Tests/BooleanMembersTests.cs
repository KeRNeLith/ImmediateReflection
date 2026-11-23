using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to get/set of boolean members (booleans are boxed with shared instances by the getters).
/// </summary>
[TestFixture]
internal sealed class BooleanMembersTests
{
    private sealed class BooleanContainer
    {
        public bool Field;

        public bool Property { get; set; }
    }

    [TestCase(true)]
    [TestCase(false)]
    public static void BooleanFieldGetValue(bool value)
    {
        var container = new BooleanContainer { Field = value };
        ImmediateField field = TypeAccessor.Get<BooleanContainer>().GetField(nameof(BooleanContainer.Field))!;

        object? gotValue = field.GetValue(container);

        Assert.IsInstanceOf<bool>(gotValue);
        Assert.AreEqual(value, (bool)gotValue!);
    }

    [TestCase(true)]
    [TestCase(false)]
    public static void BooleanPropertyGetValue(bool value)
    {
        var container = new BooleanContainer { Property = value };
        ImmediateProperty property = TypeAccessor.Get<BooleanContainer>().GetProperty(nameof(BooleanContainer.Property))!;

        object? gotValue = property.GetValue(container);

        Assert.IsInstanceOf<bool>(gotValue);
        Assert.AreEqual(value, (bool)gotValue!);
    }

    [Test]
    public static void BooleanGettersReuseBoxedValues()
    {
        ImmediateType type = TypeAccessor.Get<BooleanContainer>();
        ImmediateField field = type.GetField(nameof(BooleanContainer.Field))!;
        ImmediateProperty property = type.GetProperty(nameof(BooleanContainer.Property))!;

        BooleanContainer trueContainer1 = new() { Field = true, Property = true };
        BooleanContainer trueContainer2 = new() { Field = true, Property = true };
        BooleanContainer falseContainer = new() { Field = false, Property = false };

        // The same boxed value is returned whatever the member or the owner
        Assert.AreSame(field.GetValue(trueContainer1), field.GetValue(trueContainer2));
        Assert.AreSame(field.GetValue(trueContainer1), property.GetValue(trueContainer2));
        Assert.AreSame(field.GetValue(falseContainer), property.GetValue(falseContainer));

        Assert.AreNotSame(field.GetValue(trueContainer1), field.GetValue(falseContainer));
    }

    [Test]
    public static void BooleanMembersGetThenSetWithGotValues()
    {
        ImmediateType type = TypeAccessor.Get<BooleanContainer>();
        ImmediateField field = type.GetField(nameof(BooleanContainer.Field))!;
        ImmediateProperty property = type.GetProperty(nameof(BooleanContainer.Property))!;

        BooleanContainer source = new();
        BooleanContainer target = new();

        // Values got from getters (shared boxes) are given back to setters, mixing fields and properties
        foreach (bool value in new[] { true, false, false, true, true, false, true })
        {
            field.SetValue(source, value);
            property.SetValue(source, !value);

            object fieldBox = field.GetValue(source)!;
            object propertyBox = property.GetValue(source)!;

            // Cross set: the field value goes to the property and the property value goes to the field
            property.SetValue(target, fieldBox);
            field.SetValue(target, propertyBox);

            // The setters did not alter the got values
            Assert.AreEqual(value, (bool)fieldBox);
            Assert.AreEqual(!value, (bool)propertyBox);

            // The targets received the right values
            Assert.AreEqual(value, target.Property);
            Assert.AreEqual(!value, target.Field);

            // The sources are unchanged and still give the right values
            Assert.AreEqual(value, (bool)field.GetValue(source)!);
            Assert.AreEqual(!value, (bool)property.GetValue(source)!);
        }
    }
}
