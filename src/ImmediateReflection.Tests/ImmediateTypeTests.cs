using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;
using static ImmediateReflection.Tests.ConstructorTestHelpers;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="ImmediateType"/>.
    /// </summary>
    [TestFixture]
    internal class ImmediateTypeTests : ImmediateReflectionTestsBase
    {
        #region ImmediateType infos

        #region Test classes

        private class EmptyType
        {
        }

        private class PrivateNestedClass
        {
#pragma warning disable 649
            // ReSharper disable once InconsistentNaming
            public int _nestedTestValue;
#pragma warning restore 649

            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int NestedTestValue { get; set; } = 25;
        }

        #region Test helpers

        // Fields //

        [NotNull]
        private static readonly FieldInfo PrivateNestedPublicFieldFieldInfo =
            typeof(PrivateNestedClass).GetField(nameof(PrivateNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        // Properties //

        [NotNull]
        private static readonly PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
            typeof(PrivateNestedClass).GetProperty(nameof(PrivateNestedClass.NestedTestValue)) ?? throw new AssertionException("Cannot find property.");

        #endregion

        #endregion

        [Test]
        public void ImmediateTypeEmptyType()
        {
            var emptyType = new ImmediateType(typeof(EmptyType));
            Assert.AreEqual(typeof(EmptyType), emptyType.Type);
            Assert.AreEqual(nameof(EmptyType), emptyType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(EmptyType)}",
                emptyType.FullName);
            CollectionAssert.AreEqual(
                Enumerable.Empty<FieldInfo>(),
                emptyType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEqual(
                Enumerable.Empty<PropertyInfo>(),
                emptyType.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeValueType()
        {
            // Public class
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var immediateTypePublic = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicValueTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicValueTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            classifiedMembers = TypeClassifiedMembers.GetForInternalValueTypeTestObject();

            var immediateTypeInternal = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreEqual(typeof(InternalValueTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalValueTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalValueTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeReferenceType()
        {
            // Public class
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicReferenceTypeTestObject();

            var immediateTypePublic = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            Assert.AreEqual(typeof(PublicReferenceTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicReferenceTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicReferenceTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            classifiedMembers = TypeClassifiedMembers.GetForInternalReferenceTypeTestObject();

            var immediateTypeInternal = new ImmediateType(typeof(InternalReferenceTypeTestClass));
            Assert.AreEqual(typeof(InternalReferenceTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalReferenceTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalReferenceTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeObjectReferenceType()
        {
            // Public class
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicObjectTypeTestObject();

            var immediateTypePublic = new ImmediateType(typeof(PublicObjectTypeTestClass));
            Assert.AreEqual(typeof(PublicObjectTypeTestClass), immediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicObjectTypeTestClass), immediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicObjectTypeTestClass)}",
                immediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            classifiedMembers = TypeClassifiedMembers.GetForInternalObjectTypeTestObject();

            var immediateTypeInternal = new ImmediateType(typeof(InternalObjectTypeTestClass));
            Assert.AreEqual(typeof(InternalObjectTypeTestClass), immediateTypeInternal.Type);
            Assert.AreEqual(nameof(InternalObjectTypeTestClass), immediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(InternalObjectTypeTestClass)}",
                immediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                immediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                immediateTypeInternal.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeNestedType()
        {
            // Public class
            var nestedImmediateTypePublic = new ImmediateType(typeof(PublicTestClass.PublicNestedClass));
            Assert.AreEqual(typeof(PublicTestClass.PublicNestedClass), nestedImmediateTypePublic.Type);
            Assert.AreEqual(nameof(PublicTestClass.PublicNestedClass), nestedImmediateTypePublic.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicTestClass)}+{nameof(PublicTestClass.PublicNestedClass)}",
                nestedImmediateTypePublic.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicNestedPublicFieldFieldInfo
                },
                nestedImmediateTypePublic.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PublicNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypePublic.Properties.Select(property => property.PropertyInfo));

            // Internal class
            var nestedImmediateTypeInternal = new ImmediateType(typeof(PublicTestClass.InternalNestedClass));
            Assert.AreEqual(typeof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Type);
            Assert.AreEqual(nameof(PublicTestClass.InternalNestedClass), nestedImmediateTypeInternal.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicTestClass)}+{nameof(PublicTestClass.InternalNestedClass)}",
                nestedImmediateTypeInternal.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalNestedPublicFieldFieldInfo
                },
                nestedImmediateTypeInternal.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    InternalNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypeInternal.Properties.Select(property => property.PropertyInfo));

            // Protected class
            var nestedImmediateTypeProtected = new ImmediateType(typeof(ProtectedNestedClass));
            Assert.AreEqual(typeof(ProtectedNestedClass), nestedImmediateTypeProtected.Type);
            Assert.AreEqual(nameof(ProtectedNestedClass), nestedImmediateTypeProtected.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateReflectionTestsBase)}+{nameof(ProtectedNestedClass)}",
                nestedImmediateTypeProtected.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ProtectedNestedPublicFieldFieldInfo
                },
                nestedImmediateTypeProtected.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ProtectedNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypeProtected.Properties.Select(property => property.PropertyInfo));

            // Private class
            var nestedImmediateTypePrivate = new ImmediateType(typeof(PrivateNestedClass));
            Assert.AreEqual(typeof(PrivateNestedClass), nestedImmediateTypePrivate.Type);
            Assert.AreEqual(nameof(PrivateNestedClass), nestedImmediateTypePrivate.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ImmediateTypeTests)}+{nameof(PrivateNestedClass)}",
                nestedImmediateTypePrivate.FullName);
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PrivateNestedPublicFieldFieldInfo
                },
                nestedImmediateTypePrivate.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    PrivateNestedPublicGetSetPropertyPropertyInfo
                },
                nestedImmediateTypePrivate.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeEnumType()
        {
            // Simple test enum
            CheckEnumType(
                typeof(TestEnum),
                new[]
                {
                    TestEnumFieldValueFieldInfo,
                    TestEnumField1FieldInfo,
                    TestEnumField2FieldInfo
                });

            // Test enum (inherit ulong)
            CheckEnumType(
                typeof(TestEnumULong),
                new[]
                {
                    TestEnumULongFieldValueFieldInfo,
                    TestEnumULongField1FieldInfo,
                    TestEnumULongField2FieldInfo
                });

            // Flags test enum
            CheckEnumType(
                typeof(TestEnumFlags),
                new[]
                {
                    TestEnumFlagsFieldValueFieldInfo,
                    TestEnumFlagsField1FieldInfo,
                    TestEnumFlagsField2FieldInfo,
                    TestEnumFlagsField3FieldInfo
                });

            #region Local functions

            void CheckEnumType(Type enumType, IEnumerable<FieldInfo> enumFields)
            {
                var immediateType = new ImmediateType(enumType);
                Assert.AreEqual(enumType, immediateType.Type);
                Assert.AreEqual(enumType.Name, immediateType.Name);
                Assert.AreEqual(
                    $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{enumType.Name}",
                    immediateType.FullName);

                CollectionAssert.AreEquivalent(
                    enumFields,
                    immediateType.Fields.Select(field => field.FieldInfo));
                CollectionAssert.AreEquivalent(
                    Enumerable.Empty<PropertyInfo>(),
                    immediateType.Properties.Select(property => property.PropertyInfo));
            }

            #endregion
        }

        [Test]
        public void ImmediateTypeWithFlags()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var testType = new ImmediateType(typeof(PublicValueTypeTestClass)); // BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
            CollectionAssert.AreEqual(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.NonPublic | BindingFlags.Instance);
            CollectionAssert.AreEqual(
                classifiedMembers.NonPublicInstanceFields,
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.NonPublicInstanceProperties,
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            CollectionAssert.AreEqual(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.NonPublicInstanceFields),
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.NonPublicInstanceProperties),
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            CollectionAssert.AreEqual(
                classifiedMembers.StaticFields.Concat(classifiedMembers.ConstFields),
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.StaticProperties,
                testType.Properties.Select(property => property.PropertyInfo));

            testType = new ImmediateType(typeof(PublicValueTypeTestClass), BindingFlags.IgnoreCase);
            CollectionAssert.AreEqual(
                Enumerable.Empty<FieldInfo>(),
                testType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEqual(
                Enumerable.Empty<PropertyInfo>(),
                testType.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeNewKeyword()
        {
            var immediateType = new ImmediateType(typeof(BaseTestClass));
            Assert.AreEqual(typeof(BaseTestClass), immediateType.Type);
            Assert.AreEqual(nameof(BaseTestClass), immediateType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(BaseTestClass)}",
                immediateType.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<FieldInfo>(),
                immediateType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    BaseClassPublicGetPropertyPropertyInfo
                },
                immediateType.Properties.Select(property => property.PropertyInfo));

            immediateType = new ImmediateType(typeof(ChildTestClass));
            Assert.AreEqual(typeof(ChildTestClass), immediateType.Type);
            Assert.AreEqual(nameof(ChildTestClass), immediateType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildTestClass)}",
                immediateType.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<FieldInfo>(),
                immediateType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ChildClassPublicGetPropertyPropertyInfo
                },
                immediateType.Properties.Select(property => property.PropertyInfo));
        }

        [Test]
        public void ImmediateTypeIndexedProperties()
        {
            var immediateType = new ImmediateType(typeof(ChildItemTestClass));
            Assert.AreEqual(typeof(ChildItemTestClass), immediateType.Type);
            Assert.AreEqual(nameof(ChildItemTestClass), immediateType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(ChildItemTestClass)}",
                immediateType.FullName);
            CollectionAssert.AreEquivalent(
                Enumerable.Empty<FieldInfo>(),
                immediateType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                new[]
                {
                    ChildItemClassPublicGetPropertyPropertyInfo
                    // Indexed properties ignored
                },
                immediateType.Properties.Select(property => property.PropertyInfo));
        }

        #endregion

        #region Members

        [Test]
        public void ImmediateTypeGetMembers()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass), TypeAccessor.DefaultFlags | BindingFlags.NonPublic);

            CollectionAssert.AreEquivalent(
                classifiedMembers.AllMembers,
                SelectAllMemberInfos(immediateType.Members));
            CollectionAssert.AreEquivalent(
                classifiedMembers.AllMembers,
                SelectAllMemberInfos(immediateType.GetMembers()));

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
        public void ImmediateTypeGetMember()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            string memberName = nameof(PublicValueTypeTestClass._publicField);
            Assert.AreEqual(immediateType.Fields[memberName], immediateType.GetMember(memberName));
            Assert.AreEqual(immediateType.Fields[memberName], immediateType[memberName]);

            memberName = nameof(PublicValueTypeTestClass.PublicPropertyGet);
            Assert.AreEqual(immediateType.Properties[memberName], immediateType.GetMember(memberName));
            Assert.AreEqual(immediateType.Properties[memberName], immediateType[memberName]);

            memberName = "NotExists";
            Assert.IsNull(immediateType.GetMember(memberName));
            Assert.IsNull(immediateType[memberName]);

            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetMember(null));
            // ReSharper disable once UnusedVariable
            Assert.Throws<ArgumentNullException>(() => { ImmediateMember member = immediateType[null]; });
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Fields

        [Test]
        public void ImmediateTypeGetFields()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            CollectionAssert.AreEquivalent(immediateType1.Fields, immediateType1.GetFields());

            var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            CollectionAssert.AreNotEquivalent(immediateType1.GetFields(), immediateType2.GetFields());
        }

        [Test]
        public void ImmediateTypeGetField()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            string fieldName = nameof(PublicValueTypeTestClass._publicField);
            Assert.AreEqual(immediateType.Fields[fieldName], immediateType.GetField(fieldName));

            fieldName = "NotExists";
            Assert.IsNull(immediateType.GetField(fieldName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetField(null));
        }

        #endregion

        #region Properties

        [Test]
        public void ImmediateTypeGetProperties()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            CollectionAssert.AreEquivalent(immediateType1.Properties, immediateType1.GetProperties());

            var immediateType2 = new ImmediateType(typeof(PublicReferenceTypeTestClass));
            CollectionAssert.AreNotEquivalent(immediateType1.GetProperties(), immediateType2.GetProperties());
        }

        [Test]
        public void ImmediateTypeGetProperty()
        {
            var immediateType = new ImmediateType(typeof(PublicValueTypeTestClass));
            string propertyName = nameof(PublicValueTypeTestClass.PublicPropertyGetSet);
            Assert.AreEqual(immediateType.Properties[propertyName], immediateType.GetProperty(propertyName));

            propertyName = "NotExists";
            Assert.IsNull(immediateType.GetProperty(propertyName));

            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => immediateType.GetProperty(null));
        }

        #endregion

        #region New/Constructor

        #region Has Default Constructor

        private static IEnumerable<TestCaseData> CreateHasDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TestStruct)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(MultipleConstructors)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TemplateStruct<double>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass)) { ExpectedResult = true };
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor)) { ExpectedResult = true };  // Considered as default
                yield return new TestCaseData(typeof(DefaultConstructorThrows)) { ExpectedResult = true };

                yield return new TestCaseData(typeof(NoDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(AbstractDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(StaticClass)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateStruct<>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(ParamsConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor)) { ExpectedResult = false };
                yield return new TestCaseData(typeof(AmbiguousParamsOnlyConstructor)) { ExpectedResult = false };
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType()) { ExpectedResult = false };
            }
        }

        [TestCaseSource(nameof(CreateHasDefaultConstructorTestCases))]
        public bool HasDefaultConstructor([NotNull] Type type)
        {
            var immediateType = new ImmediateType(type);
            return immediateType.HasDefaultConstructor;
        }

        #endregion

        #region New/TryNew

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorTestCases))]
        public void NewParameterLess([NotNull] Type type)
        {
            ConstructorTestHelpers.NewParameterLess(
                type,
                () =>
                {
                    var immediateType = new ImmediateType(type);
                    return immediateType.New();
                });
        }

        [Test]
        public void NewParamsOnly()
        {
            ConstructorTestHelpers.NewParamsOnly(
                () =>
                {
                    var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
                    return immediateType.New();
                },
                () => new ParamsOnlyConstructor());

            ConstructorTestHelpers.NewParamsOnly(
                () =>
                {
                    var immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
                    return immediateType.New();
                },
                () => new IntParamsOnlyConstructor());

            ConstructorTestHelpers.NewParamsOnly(
                () =>
                {
                    var immediateType = new ImmediateType(typeof(NullableIntParamsOnlyConstructor));
                    return immediateType.New();
                },
                () => new NullableIntParamsOnlyConstructor());
        }

        [Test]
        public void NewParameterLess_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var immediateType = new ImmediateType(typeof(NoDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(NotAccessibleDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(IList<int>));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(IDictionary<int, string>));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(AbstractDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(StaticClass));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(TemplateStruct<>));
            Assert.Throws<ArgumentException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(TemplateDefaultConstructor<>));
            Assert.Throws<ArgumentException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(ParamsConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(NoDefaultInheritedDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(AmbiguousParamsOnlyConstructor));
            Assert.Throws<AmbiguousMatchException>(() => immediateType.New());

            // ReSharper disable once PossibleMistakenCallToGetType.2
            immediateType = new ImmediateType(typeof(DefaultConstructor).GetType());
            Assert.Throws<ArgumentException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(DefaultConstructorThrows));
            Assert.Throws(Is.InstanceOf<Exception>(), () => immediateType.New());

            immediateType = new ImmediateType(typeof(int[]));
            Assert.Throws<MissingMethodException>(() => immediateType.New());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        [TestCaseSource(typeof(ConstructorTestHelpers), nameof(CreateDefaultConstructorNoThrowTestCases))]
        public void TryNewParameterLess([NotNull] Type type, bool expectFail)
        {
            ConstructorTestHelpers.TryNewParameterLess(
                type,
                expectFail,
                (out object instance, out Exception exception) =>
                {
                    var immediateType = new ImmediateType(type);
                    return immediateType.TryNew(out instance, out exception);
                });
        }

        [Test]
        public void TryNewParameterLess()
        {
            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) =>
                {
                    var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
                    return immediateType.TryNew(out instance, out exception);
                },
                () => new ParamsOnlyConstructor());

            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) =>
                {
                    var immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
                    return immediateType.TryNew(out instance, out exception);
                },
                () => new IntParamsOnlyConstructor());

            ConstructorTestHelpers.TryNewParameterLess(
                (out object instance, out Exception exception) =>
                {
                    var immediateType = new ImmediateType(typeof(NullableIntParamsOnlyConstructor));
                    return immediateType.TryNew(out instance, out exception);
                },
                () => new NullableIntParamsOnlyConstructor());
        }

        #endregion

        #region New(params)/TryNew(params)

        private static IEnumerable<TestCaseData> CreateNotDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), null);
                yield return new TestCaseData(typeof(int), new object[] { });
                yield return new TestCaseData(typeof(TestStruct), null);
                yield return new TestCaseData(typeof(TestStruct), new object[] { });
                yield return new TestCaseData(typeof(DefaultConstructor), null);
                yield return new TestCaseData(typeof(DefaultConstructor), new object[] { });
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), new object[] { });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), new object[] { });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), new object[] { });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), null);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), null);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), null);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), new object[] { });
                yield return new TestCaseData(typeof(List<int>), null);
                yield return new TestCaseData(typeof(List<int>), new object[] { });
                yield return new TestCaseData(typeof(Dictionary<int, string>), null);
                yield return new TestCaseData(typeof(Dictionary<int, string>), new object[] { });

                yield return new TestCaseData(typeof(ParameterConstructorStruct), new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(MultiParametersConstructor), new object[] { 12, 42.5f });
                yield return new TestCaseData(typeof(MultipleConstructors), new object[] { 12 });
                yield return new TestCaseData(typeof(MultipleConstructors), new object[] { 12, 42.5f });
                yield return new TestCaseData(typeof(TemplateNoDefaultConstructor<int>), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), new object[] { 12, 15.4f });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), new object[] { 12, 15 });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), null);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), new object[] { 12, null, 25 });
                yield return new TestCaseData(typeof(ParamsConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(ParamsConstructor), new object[] { 12, 15.4f });
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), new object[] { 42 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), new object[] { 25 });
                yield return new TestCaseData(typeof(int[]), new object[] { 5 });
                yield return new TestCaseData(typeof(List<int>), new object[] { 2 });
                yield return new TestCaseData(typeof(List<int>), new object[] { Enumerable.Range(0, 5) });
                yield return new TestCaseData(typeof(Dictionary<int, string>), new object[] { 3 });
            }
        }

        [TestCaseSource(nameof(CreateNotDefaultConstructorTestCases))]
        public void NewWithParameters([NotNull] Type type, [CanBeNull, ItemCanBeNull] params object[] args)
        {
            var immediateType = new ImmediateType(type);

            object instance = immediateType.New(args);
            Assert.IsNotNull(instance);
            Assert.AreEqual(Activator.CreateInstance(type, args), instance);
        }

        [Test]
        public void NewWithParameters_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var immediateType = new ImmediateType(typeof(NoDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New(12, 42));

            immediateType = new ImmediateType(typeof(NotAccessibleConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New(12));

            immediateType = new ImmediateType(typeof(MultiParametersConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New(12f, 12));

            immediateType = new ImmediateType(typeof(ParamsConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New(12f, 12));

            immediateType = new ImmediateType(typeof(NoDefaultInheritedDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New(12f));

            immediateType = new ImmediateType(typeof(AbstractNoConstructor));
            Assert.Throws<MemberAccessException>(() => immediateType.New(12));

            immediateType = new ImmediateType(typeof(TemplateNoDefaultConstructor<>));
            Assert.Throws<ArgumentException>(() => immediateType.New(12));

            immediateType = new ImmediateType(typeof(NotDefaultConstructorThrows));
            Assert.Throws<TargetInvocationException>(() => immediateType.New(12));
        }

        private static IEnumerable<TestCaseData> CreateNotDefaultConstructorNoThrowTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), false, null);
                yield return new TestCaseData(typeof(int), false, new object[] { });
                yield return new TestCaseData(typeof(TestStruct), false, null);
                yield return new TestCaseData(typeof(TestStruct), false, new object[] { });
                yield return new TestCaseData(typeof(DefaultConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(MultipleConstructors), false, null);
                yield return new TestCaseData(typeof(MultipleConstructors), false, new object[] { });
                yield return new TestCaseData(typeof(MultipleConstructors), false, new object[] { 12, 12.5f });
                yield return new TestCaseData(typeof(TemplateStruct<double>), false, null);
                yield return new TestCaseData(typeof(TemplateStruct<double>), false, new object[] { });
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false, null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false, new object[] { });
                yield return new TestCaseData(typeof(DefaultConstructorThrows), false, new object[] { 45, 51.0f });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(ParamsOnlyConstructor), false, new object[] { 12, 45.5f });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(IntParamsOnlyConstructor), false, new object[] { 45, 54 });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, null);
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(NullableIntParamsOnlyConstructor), false, new object[] { 12, null, 25 });
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), false, new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), false, null);
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), false, new object[] { });
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), false, new object[] { 12 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), false, new object[] { 42 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), false, new object[] { 25 });
                yield return new TestCaseData(typeof(int[]), false, new object[] { 5 });
                yield return new TestCaseData(typeof(List<int>), false, null);
                yield return new TestCaseData(typeof(List<int>), false, new object[] { });
                yield return new TestCaseData(typeof(List<int>), false, new object[] { 2 });
                yield return new TestCaseData(typeof(List<int>), false, new object[] { Enumerable.Range(0, 5) });
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, null);
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, new object[] { });
                yield return new TestCaseData(typeof(Dictionary<int, string>), false, new object[] { 3 });

                yield return new TestCaseData(typeof(int), true, new object[] { 12 });
                yield return new TestCaseData(typeof(TestStruct), true, new object[] { 12 });
                yield return new TestCaseData(typeof(DefaultConstructor), true, new object[] { 12 });
                yield return new TestCaseData(typeof(MultipleConstructors), true, new object[] { 12.5f, 12 });
                yield return new TestCaseData(typeof(TemplateStruct<double>), true, new object[] { 25 });
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), true, new object[] { 25 });
                yield return new TestCaseData(typeof(NoDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(NoDefaultConstructor), true, new object[] { });
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true, new object[] { });
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true, null);
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true, new object[] { });
                yield return new TestCaseData(typeof(StaticClass), true, null);
                yield return new TestCaseData(typeof(StaticClass), true, new object[] { });
                yield return new TestCaseData(typeof(TemplateStruct<>), true, null);
                yield return new TestCaseData(typeof(TemplateStruct<>), true, new object[] { });
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true, null);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true, new object[] { });
                yield return new TestCaseData(typeof(DefaultInheritedDefaultConstructor), true, new object[] { 45 });
                yield return new TestCaseData(typeof(DefaultInheritedNoDefaultConstructor), true, new object[] { 51 });
                yield return new TestCaseData(typeof(DefaultInheritedFromAbstractClass), true, new object[] { 72 });
                yield return new TestCaseData(typeof(NoDefaultInheritedDefaultConstructor), true, new object[] { 45, 35 });
                yield return new TestCaseData(typeof(NoDefaultInheritedNoDefaultConstructor), true, new object[] { 51, 25 });
                yield return new TestCaseData(typeof(NoDefaultInheritedFromAbstractClass), true, new object[] { 72, 15 });
                // ReSharper disable PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true, null);
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true, new object[] { });
                // ReSharper restore PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, null);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, new object[] { });
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true, new object[] { 12 });
                yield return new TestCaseData(typeof(int[]), true, null);
                yield return new TestCaseData(typeof(int[]), true, new object[] { });
            }
        }

        [TestCaseSource(nameof(CreateNotDefaultConstructorNoThrowTestCases))]
        public void TryNewWithParameters([NotNull] Type type, bool expectFail, [CanBeNull, ItemCanBeNull] params object[] args)
        {
            var immediateType = new ImmediateType(type);

            Assert.AreEqual(!expectFail, immediateType.TryNew(out object instance, out Exception ex, args));
            if (expectFail)
            {
                Assert.IsNull(instance);
                Assert.IsNotNull(ex);
            }
            else
            {
                Assert.IsNotNull(instance);
                Assert.AreEqual(Activator.CreateInstance(type, args), instance);
            }
        }

        #endregion

        #endregion

        #region Equals/HashCode/ToString

        [Test]
        public void ImmediateTypeEquality()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.IsTrue(immediateType1.Equals(immediateType1));
            Assert.IsTrue(immediateType1.Equals(immediateType2));
            Assert.IsTrue(immediateType1.Equals((object)immediateType2));
            Assert.IsFalse(immediateType1.Equals(null));

            var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.IsFalse(immediateType1.Equals(immediateType3));
            Assert.IsFalse(immediateType1.Equals((object)immediateType3));
        }

        [Test]
        public void ImmediateTypeHashCode()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            var immediateType2 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).GetHashCode(), immediateType1.GetHashCode());
            Assert.AreEqual(immediateType1.GetHashCode(), immediateType2.GetHashCode());

            var immediateType3 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.GetHashCode(), immediateType3.GetHashCode());
        }

        [Test]
        public void ImmediateTypeToString()
        {
            var immediateType1 = new ImmediateType(typeof(PublicValueTypeTestClass));
            Assert.AreEqual(typeof(PublicValueTypeTestClass).ToString(), immediateType1.ToString());

            var immediateType2 = new ImmediateType(typeof(InternalValueTypeTestClass));
            Assert.AreNotEqual(immediateType1.ToString(), immediateType2.ToString());
        }

        #endregion
    }
}