using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ImmediateReflection.Tests
{
    /// <summary>
    /// Tests related to <see cref="TypeAccessor"/>.
    /// </summary>
    [TestFixture]
    internal class TypeAccessorTests : ImmediateReflectionTestsBase
    {
        [Test]
        public void Get()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            ImmediateType immediateType = TypeAccessor.Get<PublicValueTypeTestClass>();
            CheckPublicInstanceMembers(immediateType);

            immediateType = TypeAccessor.Get(typeof(PublicValueTypeTestClass));
            CheckPublicInstanceMembers(immediateType);

            immediateType = TypeAccessor.Get<PublicValueTypeTestClass>(false);
            CheckPublicInstanceMembers(immediateType);

            immediateType = TypeAccessor.Get(typeof(PublicValueTypeTestClass), false);
            CheckPublicInstanceMembers(immediateType);

            #region Local function

            void CheckPublicInstanceMembers(ImmediateType type)
            {
                Assert.IsNotNull(type);
                Assert.AreEqual(typeof(PublicValueTypeTestClass), type.Type);
                // Public instance members
                CollectionAssert.AreEqual(
                    classifiedMembers.PublicInstanceFields.Concat(classifiedMembers.StaticFields).Concat(classifiedMembers.ConstFields),
                    type.Fields.Select(field => field.FieldInfo));
                CollectionAssert.AreEquivalent(
                    classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.StaticProperties),
                    type.Properties.Select(property => property.PropertyInfo));
            }

            #endregion
        }

        [Test]
        public void GetWithNonPublic()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            ImmediateType immediateType = TypeAccessor.Get<PublicValueTypeTestClass>(true);
            CheckPublicAndNonPublicInstanceMembers(immediateType);

            immediateType = TypeAccessor.Get(typeof(PublicValueTypeTestClass), true);
            CheckPublicAndNonPublicInstanceMembers(immediateType);

            #region Local functions

            void CheckPublicAndNonPublicInstanceMembers(ImmediateType type)
            {
                Assert.IsNotNull(type);
                Assert.AreEqual(typeof(PublicValueTypeTestClass), type.Type);
                // Public & Non Public instance members
                CollectionAssert.AreEqual(
                    classifiedMembers.AllFields,
                    immediateType.Fields.Select(field => field.FieldInfo));
                CollectionAssert.AreEquivalent(
                    classifiedMembers.PublicInstanceProperties.Concat(classifiedMembers.NonPublicInstanceProperties).Concat(classifiedMembers.StaticProperties),
                    immediateType.Properties.Select(property => property.PropertyInfo));
            }

            #endregion
        }

        [Test]
        public void GetWithFlags()
        {
            TypeClassifiedMembers classifiedMembers = TypeClassifiedMembers.GetForPublicValueTypeTestObject();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            ImmediateType immediateType = TypeAccessor.Get<PublicValueTypeTestClass>(flags);
            CheckStaticInstanceMembers(immediateType);

            immediateType = TypeAccessor.Get(typeof(PublicValueTypeTestClass), flags);
            CheckStaticInstanceMembers(immediateType);

            #region Local functions

            void CheckStaticInstanceMembers(ImmediateType type)
            {
                Assert.IsNotNull(type);
                Assert.AreEqual(typeof(PublicValueTypeTestClass), type.Type);
                // Static members
                CollectionAssert.AreEqual(
                    classifiedMembers.StaticFields.Concat(classifiedMembers.ConstFields),
                    immediateType.Fields.Select(field => field.FieldInfo));
                CollectionAssert.AreEquivalent(
                    classifiedMembers.StaticProperties,
                    immediateType.Properties.Select(property => property.PropertyInfo));
            }

            #endregion
        }

        [Test]
        public void Get_NullType()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => TypeAccessor.Get(null));
            Assert.Throws<ArgumentNullException>(() => TypeAccessor.Get(null, false));
            Assert.Throws<ArgumentNullException>(() => TypeAccessor.Get(null, true));
            Assert.Throws<ArgumentNullException>(() => TypeAccessor.Get(null, BindingFlags.Public | BindingFlags.Instance));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

#if SUPPORTS_CACHING
        [Test]
        public void GetCached()
        {
            ImmediateType immediateType1 = TypeAccessor.Get(typeof(PublicValueTypeTestClass));
            Assert.IsNotNull(immediateType1);

            ImmediateType immediateType2 = TypeAccessor.Get<PublicValueTypeTestClass>();
            Assert.IsNotNull(immediateType2);

            Assert.AreSame(immediateType1, immediateType2);
        }
#endif
    }
}