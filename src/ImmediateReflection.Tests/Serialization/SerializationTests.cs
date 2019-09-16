#if SUPPORTS_SERIALIZATION
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests relative to the serialization of certain types.
    /// </summary>
    [TestFixture]
    internal class SerializationTests : ImmediateReflectionTestsBase
    {
        #region Test helpers

        [Pure]
        [NotNull]
        private static T SerializeAndDeserialize<T>([NotNull] T @object)
        {
            // Round-trip the exception: Serialize and de-serialize with a BinaryFormatter
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                // "Save" object state
                bf.Serialize(ms, @object);

                // Re-use the same stream for de-serialization
                ms.Seek(0, 0);

                // Replace the original exception with de-serialized one
                return (T)bf.Deserialize(ms);
            }
        }

        #endregion

        [Test]
        public void ImmediatePropertySerialization()
        {
            var property = new ImmediateProperty(PublicReferenceTypePublicGetSetPropertyPropertyInfo);
            ImmediateProperty deserializedProperty = SerializeAndDeserialize(property);

            Assert.AreEqual(nameof(PublicReferenceTypeTestClass.PublicPropertyGetSet), deserializedProperty.Name);
            Assert.AreEqual(typeof(PublicReferenceTypeTestClass), deserializedProperty.DeclaringType);
            Assert.AreEqual(typeof(TestObject), deserializedProperty.PropertyType);
            Assert.AreEqual(PublicReferenceTypePublicGetSetPropertyPropertyInfo, deserializedProperty.PropertyInfo);
            Assert.IsTrue(deserializedProperty.CanRead);
            Assert.IsTrue(deserializedProperty.CanWrite);
        }

        [Test]
        public void ImmediatePropertiesSerialization()
        {
            var properties = new ImmediateProperties(new[] 
            {
                PublicValueTypePublicGetSetPropertyPropertyInfo,
                PublicReferenceTypePublicGetPropertyPropertyInfo,
                PublicObjectTypePublicSetPropertyPropertyInfo
            });
            ImmediateProperties deserializedProperties = SerializeAndDeserialize(properties);

            CollectionAssert.AreEquivalent(
                new[]
                {
                    new ImmediateProperty(PublicValueTypePublicGetSetPropertyPropertyInfo),
                    new ImmediateProperty(PublicReferenceTypePublicGetPropertyPropertyInfo),
                    new ImmediateProperty(PublicObjectTypePublicSetPropertyPropertyInfo)
                },
                deserializedProperties);
        }

        [Test]
        public void ImmediateFieldSerialization()
        {
            var field = new ImmediateField(PublicReferenceTypePublicFieldFieldsInfo);
            ImmediateField deserializedField = SerializeAndDeserialize(field);

            Assert.AreEqual(nameof(PublicReferenceTypeTestClass._publicField), deserializedField.Name);
            Assert.AreEqual(typeof(PublicReferenceTypeTestClass), deserializedField.DeclaringType);
            Assert.AreEqual(typeof(TestObject), deserializedField.FieldType);
            Assert.AreEqual(PublicReferenceTypePublicFieldFieldsInfo, deserializedField.FieldInfo);


            field = new ImmediateField(TestEnumField1FieldInfo);
            deserializedField = SerializeAndDeserialize(field);

            Assert.AreEqual(nameof(TestEnum.EnumValue1), deserializedField.Name);
            Assert.AreEqual(typeof(TestEnum), deserializedField.DeclaringType);
            Assert.AreEqual(typeof(TestEnum), deserializedField.FieldType);
            Assert.AreEqual(TestEnumField1FieldInfo, deserializedField.FieldInfo);


            field = new ImmediateField(TestEnumULongFieldValueFieldInfo);
            deserializedField = SerializeAndDeserialize(field);

            Assert.AreEqual(EnumValueFieldName, deserializedField.Name);
            Assert.AreEqual(typeof(TestEnumULong), deserializedField.DeclaringType);
            Assert.AreEqual(typeof(ulong), deserializedField.FieldType);
            Assert.AreEqual(TestEnumULongFieldValueFieldInfo, deserializedField.FieldInfo);
        }

        [Test]
        public void ImmediateFieldsSerialization()
        {
            var fields = new ImmediateFields(new[]
            {
                PublicValueTypePublicFieldFieldsInfo,
                PublicReferenceTypePublicField2FieldsInfo,
                PublicObjectTypeInternalFieldFieldsInfo,
                TestEnumField1FieldInfo,
                TestEnumULongFieldValueFieldInfo
            });
            ImmediateFields deserializedFields = SerializeAndDeserialize(fields);

            CollectionAssert.AreEquivalent(
                new[]
                {
                    new ImmediateField(PublicValueTypePublicFieldFieldsInfo),
                    new ImmediateField(PublicReferenceTypePublicField2FieldsInfo),
                    new ImmediateField(PublicObjectTypeInternalFieldFieldsInfo),
                    new ImmediateField(TestEnumField1FieldInfo),
                    new ImmediateField(TestEnumULongFieldValueFieldInfo)
                },
                deserializedFields);
        }

        [Test]
        public void ImmediateTypeSerialization()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            var type = new ImmediateType(typeof(PublicValueTypeTestClass));
            ImmediateType deserializedType = SerializeAndDeserialize(type);

            Assert.AreEqual(typeof(PublicValueTypeTestClass), deserializedType.Type);
            Assert.AreEqual(typeof(object), deserializedType.BaseType);
            Assert.IsNull(deserializedType.DeclaringType);
            Assert.AreEqual(nameof(PublicValueTypeTestClass), deserializedType.Name);
            Assert.AreEqual(
                $"{nameof(ImmediateReflection)}.{nameof(Tests)}.{nameof(PublicValueTypeTestClass)}",
                deserializedType.FullName);
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                deserializedType.Fields.Select(field => field.FieldInfo));
            CollectionAssert.AreEquivalent(
                classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                deserializedType.Properties.Select(property => property.PropertyInfo));
        }
    }
}
#endif