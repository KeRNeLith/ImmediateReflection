using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;

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
        protected static FieldInfo PrivateNestedPublicFieldFieldInfo =
            typeof(PrivateNestedClass).GetField(nameof(PrivateNestedClass._nestedTestValue)) ?? throw new AssertionException("Cannot find field.");

        // Properties //

        [NotNull]
        protected static PropertyInfo PrivateNestedPublicGetSetPropertyPropertyInfo =
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

        #region Test classes

        private class DefaultConstructor
        {
            public override bool Equals(object obj)
            {
                return Equals(obj as DefaultConstructor);
            }

            private bool Equals(DefaultConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private abstract class AbstractDefaultConstructor
        {
        }

        private class NoDefaultConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public NoDefaultConstructor(int value)
            {
            }
        }

        private class NotAccessibleDefaultConstructor
        {
            private NotAccessibleDefaultConstructor()
            {
            }
        }

        // ReSharper disable once UnusedTypeParameter
        private struct TemplateStruct<TTemplate>
        {
        }

        // ReSharper disable once UnusedTypeParameter
        private class TemplateDefaultConstructor<TTemplate>
        {
            public override bool Equals(object obj)
            {
                return Equals(obj as TemplateDefaultConstructor<TTemplate>);
            }

            private bool Equals(TemplateDefaultConstructor<TTemplate> other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private class DefaultConstructorThrows
        {
            public DefaultConstructorThrows()
            {
                throw new InvalidOperationException("Constructor throws.");
            }
        }

        private class ParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public ParamsOnlyConstructor(params object[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsOnlyConstructor);
            }

            private bool Equals(ParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private class ParamsConstructor
        {
            // ReSharper disable UnusedParameter.Local
            public ParamsConstructor(int value, params object[] args)
                // ReSharper restore UnusedParameter.Local
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsConstructor);
            }

            private bool Equals(ParamsConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private class AmbiguousParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            public AmbiguousParamsOnlyConstructor(params object[] args)
            {
            }

            // ReSharper disable once UnusedParameter.Local
            // ReSharper disable once UnusedMember.Local
            public AmbiguousParamsOnlyConstructor(params int[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ParamsOnlyConstructor);
            }

            private bool Equals(ParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private class IntParamsOnlyConstructor
        {
            // ReSharper disable once UnusedParameter.Local
            public IntParamsOnlyConstructor(params int[] args)
            {
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as IntParamsOnlyConstructor);
            }

            private bool Equals(IntParamsOnlyConstructor other)
            {
                if (other is null)
                    return false;
                return true;
            }

            public override int GetHashCode()
            {
                return 1;
            }
        }

        private static class StaticClass
        {
        }

        #endregion

        private static IEnumerable<TestCaseData> CreateDefaultConstructorTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int));
                yield return new TestCaseData(typeof(TestStruct));
                yield return new TestCaseData(typeof(DefaultConstructor));
                yield return new TestCaseData(typeof(TemplateStruct<double>));
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>));
            }
        }

        [TestCaseSource(nameof(CreateDefaultConstructorTestCases))]
        public void NewParameterLess([NotNull] Type type)
        {
            var immediateType = new ImmediateType(type);

            object instance = immediateType.New();
            Assert.IsNotNull(instance);
            Assert.AreEqual(Activator.CreateInstance(type), instance);
        }

        [Test]
        public void NewParamsOnly()
        {
            var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
            object instance = immediateType.New();
            Assert.IsNotNull(instance);
            Assert.AreEqual(new ParamsOnlyConstructor(), instance);

            immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
            instance = immediateType.New();
            Assert.IsNotNull(instance);
            Assert.AreEqual(new IntParamsOnlyConstructor(), instance);
        }

        [Test]
        public void NewParameterLess_Throws()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            var immediateType = new ImmediateType(typeof(NoDefaultConstructor));
            Assert.Throws<MissingMethodException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(NotAccessibleDefaultConstructor));
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

            immediateType = new ImmediateType(typeof(AmbiguousParamsOnlyConstructor));
            Assert.Throws<AmbiguousMatchException>(() => immediateType.New());

            // ReSharper disable once PossibleMistakenCallToGetType.2
            immediateType = new ImmediateType(typeof(DefaultConstructor).GetType());
            Assert.Throws<ArgumentException>(() => immediateType.New());

            immediateType = new ImmediateType(typeof(DefaultConstructorThrows));
            Assert.Throws(Is.InstanceOf<Exception>(), () => immediateType.New());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }

        private static IEnumerable<TestCaseData> CreateDefaultConstructorNoThrowTestCases
        {
            [UsedImplicitly]
            get
            {
                yield return new TestCaseData(typeof(int), false);
                yield return new TestCaseData(typeof(TestStruct), false);
                yield return new TestCaseData(typeof(DefaultConstructor), false);
                yield return new TestCaseData(typeof(TemplateStruct<double>), false);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<int>), false);

                yield return new TestCaseData(typeof(NoDefaultConstructor), true);
                yield return new TestCaseData(typeof(NotAccessibleDefaultConstructor), true);
                yield return new TestCaseData(typeof(AbstractDefaultConstructor), true);
                yield return new TestCaseData(typeof(StaticClass), true);
                yield return new TestCaseData(typeof(TemplateStruct<>), true);
                yield return new TestCaseData(typeof(TemplateDefaultConstructor<>), true);
                // ReSharper disable once PossibleMistakenCallToGetType.2
                yield return new TestCaseData(typeof(DefaultConstructor).GetType(), true);
                yield return new TestCaseData(typeof(DefaultConstructorThrows), true);
            }
        }

        [TestCaseSource(nameof(CreateDefaultConstructorNoThrowTestCases))]
        public void NewParameterLess_NoThrow([NotNull] Type type, bool expectFail)
        {
            var immediateType = new ImmediateType(type);

            Assert.AreEqual(!expectFail, immediateType.TryNew(out object instance, out Exception ex));
            if (expectFail)
            {
                Assert.IsNull(instance);
                Assert.IsNotNull(ex);
            }
            else
            {
                Assert.IsNotNull(instance);
                Assert.AreEqual(Activator.CreateInstance(type), instance);
            }
        }

        [Test]
        public void NewParameterLess_NoThrow()
        {
            var immediateType = new ImmediateType(typeof(ParamsOnlyConstructor));
            Assert.IsTrue(immediateType.TryNew(out object instance, out Exception _));
            Assert.IsNotNull(instance);
            Assert.AreEqual(new ParamsOnlyConstructor(), instance);

            immediateType = new ImmediateType(typeof(IntParamsOnlyConstructor));
            Assert.IsTrue(immediateType.TryNew(out instance, out Exception _));
            Assert.IsNotNull(instance);
            Assert.AreEqual(new IntParamsOnlyConstructor(), instance);
        }

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