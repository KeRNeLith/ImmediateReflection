using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ObjectWrapper"/>.
    /// </summary>
    [TestFixture]
    internal class ObjectWrapperTests : ImmediateReflectionTestsBase
    {
        #region ObjectWrapper infos

        [Test]
        public void ObjectWrapperInfo()
        {
            var testObject = new PublicValueTypeTestClass();

            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var objectWrapper = new ObjectWrapper(testObject);
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
        public void ObjectWrapperGetMembers()
        {
            var testObject = new PublicValueTypeTestClass();
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var objectWrapper = new ObjectWrapper(testObject);

            CollectionAssert.AreEquivalent(
                classifiedMembers.AllPublicMembers,
                SelectAllMemberInfos(objectWrapper.Members));
            CollectionAssert.AreEquivalent(
                classifiedMembers.AllPublicMembers,
                SelectAllMemberInfos(objectWrapper.GetMembers()));

            #region Local function

            IEnumerable<MemberInfo> SelectAllMemberInfos(IEnumerable<ImmediateMember> members)
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
        public void ObjectWrapperGetMember()
        {
            var testObject = new PublicValueTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);
            string memberName = nameof(PublicValueTypeTestClass._publicField);
            Assert.AreEqual(objectWrapper.Fields[memberName], objectWrapper.GetMember(memberName));
            Assert.AreEqual(objectWrapper.Fields[memberName], objectWrapper[memberName]);

            memberName = nameof(PublicValueTypeTestClass.PublicPropertyGet);
            Assert.AreEqual(objectWrapper.Properties[memberName], objectWrapper.GetMember(memberName));
            Assert.AreEqual(objectWrapper.Properties[memberName], objectWrapper[memberName]);

            memberName = "NotExists";
            Assert.IsNull(objectWrapper.GetMember(memberName));
            Assert.IsNull(objectWrapper[memberName]);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => objectWrapper.GetMember(null));
            // ReSharper disable once UnusedVariable
            Assert.Throws<ArgumentNullException>(() => { ImmediateMember member = objectWrapper[null]; });
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Fields

        [Test]
        public void ObjectWrapperGetFields()
        {
            var testObject1 = new PublicValueTypeTestClass();
            var testObject2 = new PublicReferenceTypeTestClass();

            var objectWrapper1 = new ObjectWrapper(testObject1);
            CollectionAssert.AreEquivalent(objectWrapper1.Fields, objectWrapper1.GetFields());

            var objectWrapper2 = new ObjectWrapper(testObject2);
            CollectionAssert.AreNotEquivalent(objectWrapper1.GetFields(), objectWrapper2.GetFields());
        }

        [Test]
        public void ObjectWrapperGetField()
        {
            var testObject = new PublicValueTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);
            string fieldName = nameof(PublicValueTypeTestClass._publicField);
            Assert.AreEqual(objectWrapper.Fields[fieldName], objectWrapper.GetField(fieldName));

            fieldName = "NotExists";
            Assert.IsNull(objectWrapper.GetField(fieldName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => objectWrapper.GetField(null));
        }

        #region GetValue

        private static IEnumerable<TestCaseData> CreateObjectWrapperGetFieldValueTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Struct

                var testStruct = new TestStruct
                {
                    _testValue = 12
                };

                yield return new TestCaseData(testStruct, TestStructTestFieldFieldInfo, 12);

                #endregion

                #region Value type

                var publicValueTypeTestObject = new PublicValueTypeTestClass
                {
                    _publicField = 1
                };

                yield return new TestCaseData(publicValueTypeTestObject, PublicValueTypePublicFieldFieldsInfo, 1);

                var internalValueTypeTestObject = new InternalValueTypeTestClass
                {
                    _publicField = 2
                };

                yield return new TestCaseData(internalValueTypeTestObject, InternalValueTypePublicFieldFieldsInfo, 2);

                #endregion
            }
        }

        [TestCaseSource(nameof(CreateObjectWrapperGetFieldValueTestCases))]
        public void ObjectWrapperGetFieldValue([NotNull] object target, [NotNull] FieldInfo field, [CanBeNull] object expectedValue)
        {
            var objectWrapper = new ObjectWrapper(target);

            object gotValue = objectWrapper.GetFieldValue(field.Name);
            if (expectedValue is null)
                Assert.IsNull(gotValue);
            else if (expectedValue.GetType().IsValueType)
                Assert.AreEqual(expectedValue, gotValue);
            else
                Assert.AreSame(expectedValue, gotValue);
        }

        [Test]
        public void ObjectWrapperGetFieldValue_NonPublic()
        {
            var publicTestObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(publicTestObject);
            Assert.IsNull(objectWrapper.GetFieldValue(nameof(PublicValueTypeTestClass._internalField)));
            Assert.IsNull(objectWrapper.GetFieldValue("_protectedField"));
            Assert.IsNull(objectWrapper.GetFieldValue("_privateField"));

            var internalTestObject = new InternalValueTypeTestClass();

            objectWrapper = new ObjectWrapper(internalTestObject);
            Assert.IsNull(objectWrapper.GetFieldValue(nameof(InternalValueTypeTestClass._internalField)));
            Assert.IsNull(objectWrapper.GetFieldValue("_protectedField"));
            Assert.IsNull(objectWrapper.GetFieldValue("_privateField"));
        }

        [Test]
        public void ObjectWrapperGetFieldValue_Static()
        {
            var testObject = new PublicValueTypeTestClass();

            PublicValueTypeTestClass._publicStaticField = 12;
            var objectWrapper = new ObjectWrapper(testObject);
            Assert.AreEqual(12, objectWrapper.GetFieldValue(nameof(PublicValueTypeTestClass._publicStaticField)));
        }

        [Test]
        public void ObjectWrapperGetFieldValue_NotExists()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            Assert.IsNull(objectWrapper.GetFieldValue("NotExists"));
        }

        #endregion

        #region SetValue

        [Test]
        public void ObjectWrapperSetFieldValue_Struct()
        {
            var testStruct = new TestStruct();

            var objectWrapper = new ObjectWrapper(testStruct);
            objectWrapper.SetFieldValue(nameof(TestStruct._testValue), 45);
            Assert.AreEqual(0, testStruct._testValue);  // Not updated there (but on the shadow copy yes) since struct are immutable
                                                                      // Limitation is the same with classic FieldInfo
        }

        [Test]
        public void ObjectWrapperSetFieldValue_Class()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
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
        public void ObjectWrapperSetFieldValue_NotExists()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            // Nothing set but not throw
            Assert.DoesNotThrow(() => objectWrapper.SetFieldValue("NotExists", 12));
        }

        [Test]
        public void ObjectWrapperSetFieldValue_Static()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            objectWrapper.SetFieldValue(nameof(PublicValueTypeTestClass._publicStaticField), 36);
            Assert.AreEqual(36, PublicValueTypeTestClass._publicStaticField);
        }

        [Test]
        public void ObjectWrapperSetFieldValue_WrongValue()
        {
            var testObject = new PublicReferenceTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            Assert.Throws<InvalidCastException>(() => objectWrapper.SetFieldValue(nameof(PublicReferenceTypeTestClass._publicField), 12));
            Assert.Throws<InvalidCastException>(() => objectWrapper.SetFieldValue(nameof(PublicReferenceTypeTestClass._publicField), new SmallObject()));
        }

        #endregion

        #endregion

        #region Properties

        [Test]
        public void ObjectWrapperGetProperties()
        {
            var testObject1 = new PublicValueTypeTestClass();
            var testObject2 = new PublicReferenceTypeTestClass();

            var objectWrapper1 = new ObjectWrapper(testObject1);
            CollectionAssert.AreEquivalent(objectWrapper1.Properties, objectWrapper1.GetProperties());

            var objectWrapper2 = new ObjectWrapper(testObject2);
            CollectionAssert.AreNotEquivalent(objectWrapper1.GetProperties(), objectWrapper2.GetProperties());
        }

        [Test]
        public void ObjectWrapperGetProperty()
        {
            var testObject = new PublicValueTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);
            string propertyName = nameof(PublicValueTypeTestClass.PublicPropertyGetSet);
            Assert.AreEqual(objectWrapper.Properties[propertyName], objectWrapper.GetProperty(propertyName));

            propertyName = "NotExists";
            Assert.IsNull(objectWrapper.GetProperty(propertyName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => objectWrapper.GetProperty(null));
        }

        #region GetValue

        private static IEnumerable<TestCaseData> CreateObjectWrapperGetPropertyValueTestCases
        {
            [UsedImplicitly]
            get
            {
                #region Struct

                var testStruct = new TestStruct
                {
                    TestValue = 42
                };

                yield return new TestCaseData(testStruct, TestStructTestPropertyPropertyInfo, 42);

                #endregion

                #region Value type

                var publicValueTypeTestObject = new PublicValueTypeTestClass(3, 4)
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

                var internalValueTypeTestObject = new InternalValueTypeTestClass(8, 9)
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
        }

        [TestCaseSource(nameof(CreateObjectWrapperGetPropertyValueTestCases))]
        public void ObjectWrapperGetPropertyValue([NotNull] object target, [NotNull] PropertyInfo property, [CanBeNull] object expectedValue)
        {
            var objectWrapper = new ObjectWrapper(target);

            object gotValue = objectWrapper.GetPropertyValue(property.Name);
            if (expectedValue is null)
                Assert.IsNull(gotValue);
            else if (expectedValue.GetType().IsValueType)
                Assert.AreEqual(expectedValue, gotValue);
            else
                Assert.AreSame(expectedValue, gotValue);
        }

        [Test]
        public void ObjectWrapperGetPropertyValue_Static()
        {
            var testObject = new PublicValueTypeTestClass();

            PublicValueTypeTestClass.PublicStaticPropertyGetSet = 66;
            var objectWrapper = new ObjectWrapper(testObject);
            Assert.AreEqual(66, objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet)));
        }

        [Test]
        public void ObjectWrapperGetPropertyValue_NonPublic()
        {
            var publicTestObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(publicTestObject);
            Assert.IsNull(objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.InternalPropertyGetSet)));
            Assert.IsNull(objectWrapper.GetPropertyValue("ProtectedPropertyGetSet"));
            Assert.IsNull(objectWrapper.GetPropertyValue("PrivatePropertyGetSet"));


            var internalTestObject = new InternalValueTypeTestClass();

            objectWrapper = new ObjectWrapper(internalTestObject);
            Assert.IsNull(objectWrapper.GetPropertyValue(nameof(InternalValueTypeTestClass.InternalPropertyGetSet)));
            Assert.IsNull(objectWrapper.GetPropertyValue("ProtectedPropertyGetSet"));
            Assert.IsNull(objectWrapper.GetPropertyValue("PrivatePropertyGetSet"));
        }

        [Test]
        public void ObjectWrapperGetPropertyValue_NotExists()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            Assert.IsNull(objectWrapper.GetPropertyValue("NotExists"));
        }

        [Test]
        public void ObjectWrapperGetPropertyValue_NoGetter()
        {
            var testObject = new PublicValueTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentException>(() => objectWrapper.GetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertySet)));
        }

        #endregion

        #region SetValue

        [Test]
        public void ObjectWrapperSetPropertyValue_Struct()
        {
            var testStruct = new TestStruct();

            var objectWrapper = new ObjectWrapper(testStruct);
            objectWrapper.SetPropertyValue(nameof(TestStruct.TestValue), 51);
            Assert.AreEqual(0, testStruct.TestValue);   // Not updated there (but on the shadow copy yes) since struct are immutable
                                                                      // Limitation is the same with classic PropertyInfo
        }

        [Test]
        public void ObjectWrapperSetPropertyValue_Class()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
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
        public void ObjectWrapperSetPropertyValue_NotExists()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            // Nothing set but not throw
            Assert.DoesNotThrow(() => objectWrapper.SetPropertyValue("NotExists", 12));
        }

        [Test]
        public void ObjectWrapperSetPropertyValue_Static()
        {
            var testObject = new PublicValueTypeTestClass();

            var objectWrapper = new ObjectWrapper(testObject);
            objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicStaticPropertyGetSet), 77);
            Assert.AreEqual(77, PublicValueTypeTestClass.PublicStaticPropertyGetSet);
        }

        [Test]
        public void ObjectWrapperSetPropertyValue_WrongValue()
        {
            var testObject = new PublicReferenceTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);

            Assert.Throws<InvalidCastException>(() => objectWrapper.SetPropertyValue(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet), 12));
            Assert.Throws<InvalidCastException>(() => objectWrapper.SetPropertyValue(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet), new SmallObject()));
        }

        [Test]
        public void ObjectWrapperSetPropertyValue_NoSetter()
        {
            var testObject = new PublicValueTypeTestClass();
            var objectWrapper = new ObjectWrapper(testObject);
            Assert.Throws<ArgumentException>(() => objectWrapper.SetPropertyValue(nameof(PublicValueTypeTestClass.PublicPropertyGet), 51));
        }

        #endregion

        #endregion

        #region Equals/HashCode/ToString

        [Test]
        public void ObjectWrapperEquality()
        {
            var testObject = new PublicValueTypeTestClass();
            var testObject2 = new PublicValueTypeTestClass();

            var objectWrapper1 = new ObjectWrapper(testObject);
            var objectWrapper2 = new ObjectWrapper(testObject);
            Assert.IsTrue(objectWrapper1.Equals(objectWrapper1));
            Assert.IsTrue(objectWrapper1.Equals(objectWrapper2));
            Assert.IsTrue(objectWrapper1.Equals((object)objectWrapper2));
            Assert.IsFalse(objectWrapper1.Equals(null));

            var objectWrapper3 = new ObjectWrapper(testObject2);
            Assert.IsFalse(objectWrapper1.Equals(objectWrapper3));
            Assert.IsFalse(objectWrapper1.Equals((object)objectWrapper3));
        }

        [Test]
        public void ObjectWrapperHashCode()
        {
            var testObject = new PublicValueTypeTestClass();
            var testObject2 = new PublicValueTypeTestClass();
            var testObject3 = new PublicValueTypeTestClass { PublicPropertyGetSet = 25 };

            var objectWrapper1 = new ObjectWrapper(testObject);
            var objectWrapper2 = new ObjectWrapper(testObject);
            Assert.AreEqual(testObject.GetHashCode(), objectWrapper1.GetHashCode());
            Assert.AreEqual(objectWrapper1.GetHashCode(), objectWrapper2.GetHashCode());

            var objectWrapper3 = new ObjectWrapper(testObject2);
            Assert.AreEqual(testObject2.GetHashCode(), objectWrapper3.GetHashCode());
            Assert.AreNotEqual(objectWrapper1.GetHashCode(), objectWrapper3.GetHashCode());

            var objectWrapper4 = new ObjectWrapper(testObject3);
            Assert.AreNotEqual(objectWrapper1.GetHashCode(), objectWrapper4.GetHashCode());
        }

        [Test]
        public void ObjectWrapperToString()
        {
            var testObject = new PublicValueTypeTestClass();
            var testObject2 = new InternalValueTypeTestClass();

            var objectWrapper1 = new ObjectWrapper(testObject);
            Assert.AreEqual(testObject.ToString(), objectWrapper1.ToString());

            var objectWrapper2 = new ObjectWrapper(testObject2);
            Assert.AreNotEqual(objectWrapper1.ToString(), objectWrapper2.ToString());
        }

        #endregion
    }
}