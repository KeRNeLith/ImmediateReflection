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

    private readonly MemoryCache<Type, ImmediateTypeCache> _cachedTypes = new();

    [ContractAnnotation("type:null => halt")]
    public ImmediateType GetImmediateType(Type type, BindingFlags flags)
    {
        AssertNotNull(type);

        return _cachedTypes
            .GetOrCreate(type, static t => new ImmediateTypeCache(t))
            .GetOrCreate(flags);
    }

    #endregion

    #region Attributes cache

    private readonly MemoryCache<MemberInfo, AttributesCache> _cachedAttributes = new(new MemberInfoEqualityComparer());

    [ContractAnnotation("member:null => halt")]
    public AttributesCache GetAttributesCache(MemberInfo member)
    {
        AssertNotNull(member);

        return _cachedAttributes.GetOrCreate(member, static m => new AttributesCache(m));
    }

    #endregion

    #region Default constructor cache

    private readonly MemoryCache<Type, ConstructorData<DefaultConstructorDelegate>> _cachedDefaultConstructors = new();

    [ContractAnnotation("type:null => halt")]
    public ConstructorData<DefaultConstructorDelegate> GetDefaultConstructor(Type type)
    {
        AssertNotNull(type);

        return _cachedDefaultConstructors.GetOrCreate(type, static t =>
        {
            DefaultConstructorDelegate ctor = DelegatesFactory.CreateDefaultConstructor(t, out bool hasConstructor);
            return new ConstructorData<DefaultConstructorDelegate>(ctor, hasConstructor);
        });
    }

    #endregion

    #region Copy constructor cache

    private readonly MemoryCache<Type, ConstructorData<CopyConstructorDelegate>> _cachedCopyConstructors = new();

    [ContractAnnotation("type:null => halt")]
    public ConstructorData<CopyConstructorDelegate> GetCopyConstructor(Type type)
    {
        AssertNotNull(type);

        return _cachedCopyConstructors.GetOrCreate(type, static t =>
        {
            CopyConstructorDelegate ctor = DelegatesFactory.CreateCopyConstructor(t, out bool hasConstructor);
            return new ConstructorData<CopyConstructorDelegate>(ctor, hasConstructor);
        });
    }

    #endregion

    #region Field cache

    private readonly MemoryCache<FieldInfo, ImmediateField> _cachedFields = new(new FieldInfoEqualityComparer());

    [ContractAnnotation("field:null => halt")]
    public ImmediateField GetField(FieldInfo field)
    {
        AssertNotNull(field);

        return _cachedFields.GetOrCreate(field, static f => new ImmediateField(f));
    }

    #endregion

    #region Property cache

    private readonly MemoryCache<PropertyInfo, ImmediateProperty> _cachedProperties = new(new PropertyInfoEqualityComparer());

    [ContractAnnotation("property:null => halt")]
    public ImmediateProperty GetProperty(PropertyInfo property)
    {
        AssertNotNull(property);

        return _cachedProperties.GetOrCreate(property, static p => new ImmediateProperty(p));
    }

    #endregion
}