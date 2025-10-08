using System;
using System.Reflection;
using JetBrains.Annotations;
using static ImmediateReflection.GeneralHelpers;
using static ImmediateReflection.Utils.ReflectionHelpers;

namespace ImmediateReflection;

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

        internal static readonly CachesHandler InternalInstance = new();
    }

    #endregion

    #region ImmediateType cache

    private readonly struct TypeCacheKey : IEquatable<TypeCacheKey>
    {
        private readonly Type _type;
        private readonly BindingFlags _flags;

        public TypeCacheKey(Type type, BindingFlags flags)
        {
            AssertNotNull(type);

            _type = type;
            _flags = flags;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
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

    private volatile MemoryCache<TypeCacheKey, ImmediateType> _cachedTypes = new();

    [ContractAnnotation("type:null => halt")]
    public ImmediateType GetImmediateType(Type type, BindingFlags flags)
    {
        AssertNotNull(type);

        return _cachedTypes.GetOrCreate(
            new TypeCacheKey(type, flags),
            () => new ImmediateType(type, flags));
    }

    #endregion

    #region Attributes cache

    private volatile MemoryCache<MemberInfo, AttributesCache> _cachedAttributes = new(new MemberInfoEqualityComparer());

    [ContractAnnotation("member:null => halt")]
    public AttributesCache GetAttributesCache(MemberInfo member)
    {
        AssertNotNull(member);

        return _cachedAttributes.GetOrCreate(member, () => new AttributesCache(member));
    }

    #endregion

    #region Default constructor cache

    private volatile MemoryCache<Type, ConstructorData<DefaultConstructorDelegate>> _cachedDefaultConstructors = new();

    [ContractAnnotation("type:null => halt")]
    public ConstructorData<DefaultConstructorDelegate> GetDefaultConstructor(Type type)
    {
        AssertNotNull(type);

        return _cachedDefaultConstructors.GetOrCreate(type, () =>
        {
            DefaultConstructorDelegate ctor = DelegatesFactory.CreateDefaultConstructor(type, out bool hasConstructor);
            return new ConstructorData<DefaultConstructorDelegate>(ctor, hasConstructor);
        });
    }

    #endregion

    #region Copy constructor cache

    private volatile MemoryCache<Type, ConstructorData<CopyConstructorDelegate>> _cachedCopyConstructors = new();

    [ContractAnnotation("type:null => halt")]
    public ConstructorData<CopyConstructorDelegate> GetCopyConstructor(Type type)
    {
        AssertNotNull(type);

        return _cachedCopyConstructors.GetOrCreate(type, () =>
        {
            CopyConstructorDelegate ctor = DelegatesFactory.CreateCopyConstructor(type, out bool hasConstructor);
            return new ConstructorData<CopyConstructorDelegate>(ctor, hasConstructor);
        });
    }

    #endregion

    #region Field cache

    private volatile MemoryCache<FieldInfo, ImmediateField> _cachedFields = new();

    [ContractAnnotation("field:null => halt")]
    public ImmediateField GetField(FieldInfo field)
    {
        AssertNotNull(field);

        return _cachedFields.GetOrCreate(field, () => new ImmediateField(field));
    }

    #endregion

    #region Property cache

    private volatile MemoryCache<PropertyInfo, ImmediateProperty> _cachedProperties = new(new PropertyInfoEqualityComparer());

    [ContractAnnotation("property:null => halt")]
    public ImmediateProperty GetProperty(PropertyInfo property)
    {
        AssertNotNull(property);

        return _cachedProperties.GetOrCreate(property, () => new ImmediateProperty(property));
    }

    #endregion
}