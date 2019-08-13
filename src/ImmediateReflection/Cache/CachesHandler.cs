#if SUPPORTS_CACHING
using System;
using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Cache storing all ready to use Immediate Reflection data.
    /// </summary>
    /// <remarks>This is a singleton implementation.</remarks>
    internal sealed class CachesHandler
    {
        #region Singleton management

        private CachesHandler()
        {
        }

        /// <summary>
        /// Gets the cache instance.
        /// </summary>
        public static CachesHandler Instance { get; } = InstanceHandler.InternalInstance;

        private static class InstanceHandler
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static InstanceHandler()
            {
            }

            internal static readonly CachesHandler InternalInstance = new CachesHandler();
        }

        #endregion

        #region ImmediateType cache

        private struct TypeCacheKey : IEquatable<TypeCacheKey>
        {
            [NotNull]
            private readonly Type _type;

            private readonly BindingFlags _flags;

            public TypeCacheKey([NotNull] Type type, BindingFlags flags)
            {
                Debug.Assert(type != null);

                _type = type;
                _flags = flags;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is null)
                    return false;
                return obj is TypeCacheKey other && Equals(other);
            }

            /// <inheritdoc />
            public bool Equals(TypeCacheKey other)
            {
                return _type == other._type && _flags == other._flags;
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return (_type.GetHashCode() * 397) ^ (int)_flags;
            }
        }

        [NotNull]
        private volatile MemoryCache<TypeCacheKey, ImmediateType> _cachedTypes = new MemoryCache<TypeCacheKey, ImmediateType>();

        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public ImmediateType GetImmediateType([NotNull] Type type, BindingFlags flags)
        {
            Debug.Assert(type != null);

            return _cachedTypes.GetOrCreate(
                new TypeCacheKey(type, flags),
                () => new ImmediateType(type, flags));
        }

        #endregion

        #region Attributes cache

        [NotNull]
        private volatile MemoryCache<MemberInfo, AttributesCache> _cachedAttributes = new MemoryCache<MemberInfo, AttributesCache>();

        [NotNull]
        [ContractAnnotation("member:null => halt")]
        public AttributesCache GetAttributesCache([NotNull] MemberInfo member)
        {
            Debug.Assert(member != null);

            return _cachedAttributes.GetOrCreate(member, () => new AttributesCache(member));
        }

        #endregion

        #region Default constructor cache

        [NotNull]
        private volatile MemoryCache<Type, DefaultConstructorData> _cachedConstructors =
            new MemoryCache<Type, DefaultConstructorData>();

        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public DefaultConstructorData GetDefaultConstructor([NotNull] Type type)
        {
            Debug.Assert(type != null);

            return _cachedConstructors.GetOrCreate(type, () =>
            {
                DefaultConstructorDelegate ctor = DelegatesFactory.CreateDefaultConstructor(type, out bool hasConstructor);
                return new DefaultConstructorData(ctor, hasConstructor);
            });
        }

        #endregion

        #region Field cache

        [NotNull]
        private volatile MemoryCache<FieldInfo, ImmediateField> _cachedFields = new MemoryCache<FieldInfo, ImmediateField>();

        [NotNull]
        [ContractAnnotation("field:null => halt")]
        public ImmediateField GetField([NotNull] FieldInfo field)
        {
            Debug.Assert(field != null);

            return _cachedFields.GetOrCreate(field, () => new ImmediateField(field));
        }

        [NotNull]
        [ContractAnnotation("field:null => halt;enumType:null => halt")]
        public ImmediateField GetField([NotNull] FieldInfo field, [NotNull] Type enumType)
        {
            Debug.Assert(field != null);
            Debug.Assert(enumType != null);
            Debug.Assert(enumType.IsEnum);

            return _cachedFields.GetOrCreate(field, () => new ImmediateField(field, enumType));
        }

        #endregion

        #region Property cache

        [NotNull]
        private volatile MemoryCache<PropertyInfo, ImmediateProperty> _cachedProperties = new MemoryCache<PropertyInfo, ImmediateProperty>();

        [NotNull]
        [ContractAnnotation("property:null => halt")]
        public ImmediateProperty GetProperty([NotNull] PropertyInfo property)
        {
            Debug.Assert(property != null);

            return _cachedProperties.GetOrCreate(property, () => new ImmediateProperty(property));
        }

        #endregion
    }
}
#endif