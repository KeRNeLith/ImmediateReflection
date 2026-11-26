using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="ObjectWrapper"/>.
/// </summary>
[TestFixture]
[NonParallelizable] // Modifies static members of shared test types
internal sealed class ObjectWrapperTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void ObjectWrapperNullObject()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new ObjectWrapper(null!));
    }

    #region ObjectWrapper infos

    [Test]
    public static void ObjectWrapperInfo()
    {
        PublicValueTypeTestClass testObject = new();

        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

        ObjectWrapper objectWrapper = new(testObject);
        Assert.AreSame(testObject, objectWrapper.Object);
        Assert.AreEqual(typeof(PublicValueTypeTestClass), objectWrapper.Type);
        Assert.AreEqual(
            new ImmediateType(typeof(PublicValueTypeTestClass)),
            objectWrapper.ImmediateType);
        CollectionAssert.AreEquivalent(
            classifiedMembers.AllPublicMembers.Select(member => member.Name),
            objectWrapper.Members.Select(field => field.Name));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
            objectWrapper.Fields.Select(field => field.FieldInfo));
        CollectionAssert.AreEquivalent(
            classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
            objectWrapper.Properties.Select(property => property.PropertyInfo));
    }

    #endregion

    #region Members

    [Test]
    public static void ObjectWrapperGetMembers()
    {
        PublicValueTypeTestClass testObject = new();
        TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

        ObjectWrapper objectWrapper = new(testObject);

        CollectionAssert.AreEquivalent(
            classifiedMembers.AllPublicMembers,
            SelectAllMemberInfos(objectWrapper.Members));
        CollectionAssert.AreEquivalent(
            classifiedMembers.AllPublicMembers,
            SelectAllMemberInfos(objectWrapper.GetMembers()));

        #region Local function

        static IEnumerable<MemberInfo> SelectAllMemberInfos(IEnumerable<ImmediateMember> members)
        {
            return members.Select<ImmediateMember, MemberInfo>(member =>
            {
                if (member is ImmediateField field)
                    return field.FieldInfo;
                if (member is ImmediateProperty property)
                    return property.PropertyInfo;

                throw new InvalidOperationException("Members contain an unexpected value");
            });
        }

        #endregion
    }

    [Test]
    public static void ObjectWrapperGetMember()
    {
        PublicValueTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);
        string memberName = nameof(PublicValueTypeTestClass._publicField);
        Assert.AreEqual(objectWrapper.Fields[memberName], objectWrapper.GetMember(memberName));
        Assert.AreEqual(objectWrapper.Fields[memberName], objectWrapper[memberName]);

        memberName = nameof(PublicValueTypeTestClass.PublicPropertyGet);
        Assert.AreEqual(objectWrapper.Properties[memberName], objectWrapper.GetMember(memberName));
        Assert.AreEqual(objectWrapper.Properties[memberName], objectWrapper[memberName]);

        memberName = "NotExists";
        Assert.IsNull(objectWrapper.GetMember(memberName));
        Assert.IsNull(objectWrapper[memberName]);

        Assert.Throws<ArgumentNullException>(() => _ = objectWrapper.GetMember(null!));
        // ReSharper disable once UnusedVariable
        Assert.Throws<ArgumentNullException>(() => _ = objectWrapper[null!]);
    }

    #endregion

    #region Fields

    [Test]
    public static void ObjectWrapperGetFields()
    {
        PublicValueTypeTestClass testObject1 = new();
        PublicReferenceTypeTestClass testObject2 = new();

        ObjectWrapper objectWrapper1 = new(testObject1);
        CollectionAssert.AreEquivalent(objectWrapper1.Fields, objectWrapper1.GetFields());

        ObjectWrapper objectWrapper2 = new(testObject2);
        CollectionAssert.AreNotEquivalent(objectWrapper1.GetFields(), objectWrapper2.GetFields());
    }

    [Test]
    public static void ObjectWrapperGetField()
    {
        PublicValueTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);
        string fieldName = nameof(PublicValueTypeTestClass._publicField);
        Assert.AreEqual(objectWrapper.Fields[fieldName], objectWrapper.GetField(fieldName));

        fieldName = "NotExists";
        Assert.IsNull(objectWrapper.GetField(fieldName));

        Assert.Throws<ArgumentNullException>(() => _ = objectWrapper.GetField(null!));
    }

    #region GetValue

    private static IEnumerable<TestCaseData> CreateObjectWrapperGetFieldValueTestCases()
    {
        #region Struct

        TestStruct testStruct = new()        {
            _testValue = 12
        };

        yield return new TestCaseData(testStruct, TestStructTestFieldFieldInfo, 12);

        #endregion

        #region Value type

        PublicValueTypeTestClass publicValueTypeTestObject = new()        {
            _publicField = 1
        };

        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicFieldFieldsInfo, 1);

        InternalValueTypeTestClass internalValueTypeTestObject = new()        {
            _publicField = 2
        };

        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicFieldFieldsInfo, 2);

        #endregion
    }

    [TestCaseSource(nameof(CreateObjectWrapperGetFieldValueTestCases))]
    public static void ObjectWrapperGetFieldValue(object target, FieldInfo field, object? expectedValue)
    {
        ObjectWrapper objectWrapper = new(target);

        object? gotValue = objectWrapper.GetFieldValue(field.Name);
        if (expectedValue is null)
        {
            Assert.IsNull(gotValue);
        }
        else if (expectedValue.GetType().IsValueType)
        {
            Assert.AreEqual(expectedValue, gotValue);
        }
        else
        {
            Assert.AreSame(expectedValue, gotValue);
        }
    }

    [Test]
    public static void ObjectWrapperGetFieldValue_NonPublic()
    {
        PublicValueTypeTestClass publicTestObject = new();

        ObjectWrapper objectWrapper = new(publicTestObject);
        Assert.IsNull(objectWrapper.GetFieldValue(nameof(PublicValueTypeTestClass._internalField)));
        Assert.IsNull(objectWrapper.GetFieldValue("_protectedField"));
        Assert.IsNull(objectWrapper.GetFieldValue("_privateField"));

        InternalValueTypeTestClass internalTestObject = new();

        objectWrapper = new ObjectWrapper(internalTestObject);
        Assert.IsNull(objectWrapper.GetFieldValue(nameof(InternalValueTypeTestClass._internalField)));
        Assert.IsNull(objectWrapper.GetFieldValue("_protectedField"));
        Assert.IsNull(objectWrapper.GetFieldValue("_privateField"));
    }

    [Test]
    public static void ObjectWrapperGetFieldValue_Static()
    {
        PublicValueTypeTestClass testObject = new();

        PublicValueTypeTestClass._publicStaticField = 12;
        ObjectWrapper objectWrapper = new(testObject);
        Assert.AreEqual(12, objectWrapper.GetFieldValue(nameof(PublicValueTypeTestClass._publicStaticField)));
    }

    [Test]
    public static void ObjectWrapperGetFieldValue_NotExists()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        Assert.IsNull(objectWrapper.GetFieldValue("NotExists"));
    }

    #endregion

    #region SetValue

    [Test]
    public static void ObjectWrapperSetFieldValue_Struct()
    {
        TestStruct testStruct = new();

        ObjectWrapper objectWrapper = new(testStruct);
        objectWrapper.SetFieldValue(nameof(TestStruct._testValue), 45);
        Assert.AreEqual(0, testStruct._testValue);  // Not updated there (but on the shadow copy yes) since struct are immutable
                                                    // Limitation is the same with classic FieldInfo
    }

    [Test]
    public static void ObjectWrapperSetFieldValue_Class()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        objectWrapper.SetFieldValue(nameof(PublicValueTypeTestClass._publicField), 12);
        Assert.AreEqual(12, testObject._publicField);

        // Object wrapper does not provide access to not public members
        objectWrapper.SetFieldValue(nameof(PublicValueTypeTestClass._internalField), 24);
        Assert.AreEqual(0, testObject._internalField);

        objectWrapper.SetFieldValue("_protectedField", 48);
        Assert.AreEqual(0, testObject.GetProtectedFieldValue());

        objectWrapper.SetFieldValue("_privateField", 962);
        Assert.AreEqual(0, testObject.GetPrivateFieldValue());
    }

    [Test]
    public static void ObjectWrapperSetFieldValue_NotExists()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        // Nothing set but not throw
        Assert.DoesNotThrow(() => objectWrapper.SetFieldValue("NotExists", 12));
    }

    [Test]
    public static void ObjectWrapperSetFieldValue_Static()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        objectWrapper.SetFieldValue(nameof(PublicValueTypeTestClass._publicStaticField), 36);
        Assert.AreEqual(36, PublicValueTypeTestClass._publicStaticField);
    }

    [Test]
    public static void ObjectWrapperSetFieldValue_WrongValue()
    {
        PublicReferenceTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        Assert.Throws<InvalidCastException>(() => objectWrapper.SetFieldValue(nameof(PublicReferenceTypeTestClass._publicField), 12));
        Assert.Throws<InvalidCastException>(() => objectWrapper.SetFieldValue(nameof(PublicReferenceTypeTestClass._publicField), new SmallObject()));
    }

    #endregion

    #endregion

    #region Properties

    [Test]
    public static void ObjectWrapperGetProperties()
    {
        PublicValueTypeTestClass testObject1 = new();
        PublicReferenceTypeTestClass testObject2 = new();

        ObjectWrapper objectWrapper1 = new(testObject1);
        CollectionAssert.AreEquivalent(objectWrapper1.Properties, objectWrapper1.GetProperties());

        ObjectWrapper objectWrapper2 = new(testObject2);
        CollectionAssert.AreNotEquivalent(objectWrapper1.GetProperties(), objectWrapper2.GetProperties());
    }

    [Test]
    public static void ObjectWrapperGetProperty()
    {
        PublicValueTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);
        string propertyName = nameof(PublicValueTypeTestClass.PublicPropertyGetSet);
        Assert.AreEqual(objectWrapper.Properties[propertyName], objectWrapper.GetProperty(propertyName));

        propertyName = "NotExists";
        Assert.IsNull(objectWrapper.GetProperty(propertyName));

        Assert.Throws<ArgumentNullException>(() => _ = objectWrapper.GetProperty(null!));
    }

    #region GetValue

    private static IEnumerable<TestCaseData> CreateObjectWrapperGetPropertyValueTestCases()
    {
        #region Struct

        TestStruct testStruct = new()        {
            TestValue = 42
        };

        yield return new TestCaseData(testStruct, TestStructTestPropertyPropertyInfo, 42);

        #endregion

        #region Value type

        PublicValueTypeTestClass publicValueTypeTestObject = new(3, 4)
        {
            PublicPropertyGetSet = 1,
            PublicVirtualPropertyGetSet = 2,
            PublicPropertyPrivateGetSet = 5
        };

        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetSetPropertyPropertyInfo, 1);
        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicVirtualGetSetPropertyPropertyInfo, 2);
        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPropertyPropertyInfo, 3);
        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicGetPrivateSetPropertyPropertyInfo, 4);
        yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicPrivateGetSetPropertyPropertyInfo, 5);    // Private Get but gettable via Reflection

        InternalValueTypeTestClass internalValueTypeTestObject = new(8, 9)
        {
            PublicPropertyGetSet = 6,
            PublicVirtualPropertyGetSet = 7,
            PublicPropertyPrivateGetSet = 10
        };

        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetSetPropertyPropertyInfo, 6);
        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicVirtualGetSetPropertyPropertyInfo, 7);
        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPropertyPropertyInfo, 8);
        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicGetPrivateSetPropertyPropertyInfo, 9);
        yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicPrivateGetSetPropertyPropertyInfo, 10);    // Private Get but gettable via Reflection

        #endregion
    }

    [TestCaseSource(nameof(CreateObjectWrapperGetPropertyValueTestCases))]
    public static void ObjectWrapperGetPropertyValue(object target, PropertyInfo property, object? expectedValue)
    {
        ObjectWrapper objectWrapper = new(target);

        object? gotValue = objectWrapper.GetPropertyValue(property.Name);
        if (expectedValue is null)
        {
            Assert.IsNull(gotValue);
        }
        else if (expectedValue.GetType().IsValueType)
        {
            Assert.AreEqual(expectedValue, gotValue);
        }
        else
        {
            Assert.AreSame(expectedValue, gotValue);
        }
    }

    [Test]
    public static void ObjectWrapperGetPropertyValue_Static()
    {
        PublicValueTypeTestClass testObject = new();

        PublicValueTypeTestClass.PublicStaticPropertyGetSet = 66;
        ObjectWrapper objectWrapper = new(testObject);
        Assert.AreEqual(66, objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet)));
    }

    [Test]
    public static void ObjectWrapperGetPropertyValue_NonPublic()
    {
        PublicValueTypeTestClass publicTestObject = new();

        ObjectWrapper objectWrapper = new(publicTestObject);
        Assert.IsNull(objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.InternalPropertyGetSet)));
        Assert.IsNull(objectWrapper.GetPropertyValue("ProtectedPropertyGetSet"));
        Assert.IsNull(objectWrapper.GetPropertyValue("PrivatePropertyGetSet"));


        InternalValueTypeTestClass internalTestObject = new();

        objectWrapper = new ObjectWrapper(internalTestObject);
        Assert.IsNull(objectWrapper.GetPropertyValue(nameof(InternalValueTypeTestClass.InternalPropertyGetSet)));
        Assert.IsNull(objectWrapper.GetPropertyValue("ProtectedPropertyGetSet"));
        Assert.IsNull(objectWrapper.GetPropertyValue("PrivatePropertyGetSet"));
    }

    [Test]
    public static void ObjectWrapperGetPropertyValue_NotExists()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        Assert.IsNull(objectWrapper.GetPropertyValue("NotExists"));
    }

    [Test]
    public static void ObjectWrapperGetPropertyValue_NoGetter()
    {
        PublicValueTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);

        Assert.Throws<ArgumentException>(() => _ = objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertySet)));
    }

    #endregion

    #region SetValue

    [Test]
    public static void ObjectWrapperSetPropertyValue_Struct()
    {
        TestStruct testStruct = new();

        ObjectWrapper objectWrapper = new(testStruct);
        objectWrapper.SetPropertyValue(nameof(TestStruct.TestValue), 51);
        Assert.AreEqual(0, testStruct.TestValue);   // Not updated there (but on the shadow copy yes) since struct are immutable
        // Limitation is the same with classic PropertyInfo
    }

    [Test]
    public static void ObjectWrapperSetPropertyValue_Class()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertyGetSet), 12);
        Assert.AreEqual(12, testObject.PublicPropertyGetSet);

        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicVirtualPropertyGetSet), 24);
        Assert.AreEqual(24, testObject.PublicVirtualPropertyGetSet);

        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertyGetPrivateSet), 48);
        Assert.AreEqual(48, testObject.PublicPropertyGetPrivateSet);

        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertyPrivateGetSet), 96);
        Assert.AreEqual(96, testObject._publicField);   // => Setter set the public field value (for check)

        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertySet), 192);
        Assert.AreEqual(192, testObject._publicField);   // => Setter set the public field value (for check)

        // Object wrapper does not provide access to not public members
        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.InternalPropertyGetSet), 384);
        Assert.AreEqual(0, testObject._internalField);

        objectWrapper.SetPropertyValue("ProtectedPropertyGetSet", 728);
        Assert.AreEqual(0, testObject.GetProtectedFieldValue());

        objectWrapper.SetPropertyValue("PrivatePropertyGetSet", 1536);
        Assert.AreEqual(0, testObject.GetPrivateFieldValue());
    }

    [Test]
    public static void ObjectWrapperSetPropertyValue_NotExists()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        // Nothing set but not throw
        Assert.DoesNotThrow(() => objectWrapper.SetPropertyValue("NotExists", 12));
    }

    [Test]
    public static void ObjectWrapperSetPropertyValue_Static()
    {
        PublicValueTypeTestClass testObject = new();

        ObjectWrapper objectWrapper = new(testObject);
        objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet), 77);
        Assert.AreEqual(77, PublicValueTypeTestClass.PublicStaticPropertyGetSet);
    }

    [Test]
    public static void ObjectWrapperSetPropertyValue_WrongValue()
    {
        PublicReferenceTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);

        Assert.Throws<InvalidCastException>(() => objectWrapper.SetPropertyValue(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet), 12));
        Assert.Throws<InvalidCastException>(() => objectWrapper.SetPropertyValue(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet), new SmallObject()));
    }

    [Test]
    public static void ObjectWrapperSetPropertyValue_NoSetter()
    {
        PublicValueTypeTestClass testObject = new();
        ObjectWrapper objectWrapper = new(testObject);
        Assert.Throws<ArgumentException>(() => objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertyGet), 51));
    }

    #endregion

    #endregion

    #region Equals/HashCode/ToString

    [Test]
    public static void ObjectWrapperEquality()
    {
        PublicValueTypeTestClass testObject = new();
        PublicValueTypeTestClass testObject2 = new();

        ObjectWrapper objectWrapper1 = new(testObject);
        ObjectWrapper objectWrapper2 = new(testObject);
        Assert.IsTrue(objectWrapper1.Equals(objectWrapper1));
        Assert.IsTrue(objectWrapper1.Equals(objectWrapper2));
        Assert.IsTrue(objectWrapper1.Equals((object)objectWrapper2));

        ObjectWrapper objectWrapper3 = new(testObject2);
        Assert.IsFalse(objectWrapper1.Equals(objectWrapper3));
        Assert.IsFalse(objectWrapper1.Equals((object)objectWrapper3));

        Assert.IsFalse(objectWrapper1.Equals(null));
    }

    [Test]
    public static void ObjectWrapperHashCode()
    {
        PublicValueTypeTestClass testObject = new();
        PublicValueTypeTestClass testObject2 = new();
        PublicValueTypeTestClass testObject3 = new() { PublicPropertyGetSet = 25 };

        ObjectWrapper objectWrapper1 = new(testObject);
        ObjectWrapper objectWrapper2 = new(testObject);
        Assert.AreEqual(testObject.GetHashCode(), objectWrapper1.GetHashCode());
        Assert.AreEqual(objectWrapper1.GetHashCode(), objectWrapper2.GetHashCode());

        ObjectWrapper objectWrapper3 = new(testObject2);
        Assert.AreEqual(testObject2.GetHashCode(), objectWrapper3.GetHashCode());
        Assert.AreNotEqual(objectWrapper1.GetHashCode(), objectWrapper3.GetHashCode());

        ObjectWrapper objectWrapper4 = new(testObject3);
        Assert.AreNotEqual(objectWrapper1.GetHashCode(), objectWrapper4.GetHashCode());
    }

    [Test]
    public static void ObjectWrapperToString()
    {
        PublicValueTypeTestClass testObject = new();
        InternalValueTypeTestClass testObject2 = new();

        ObjectWrapper objectWrapper1 = new(testObject);
        Assert.AreEqual(testObject.ToString(), objectWrapper1.ToString());

        ObjectWrapper objectWrapper2 = new(testObject2);
        Assert.AreNotEqual(objectWrapper1.ToString(), objectWrapper2.ToString());
    }

    #endregion
}