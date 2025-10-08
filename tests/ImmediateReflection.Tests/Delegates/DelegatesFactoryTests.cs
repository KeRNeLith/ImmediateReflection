using NUnit.Framework;
using static ImmediateReflection.DelegatesFactory;

namespace ImmediateReflection.Tests;

/// <summary>
/// Tests related to <see cref="DelegatesFactory"/>.
/// </summary>
[TestFixture]
internal sealed class DelegatesFactoryTests : ImmediateReflectionTestsBase
{
    [Test]
    public static void CreatePropertyGetter_CanRead()
    {
        Assert.IsNull(CreateGetter(PublicValueTypePublicSetPropertyPropertyInfo, PublicValueTypePublicSetPropertyPropertyInfo.GetGetMethod()!));
        Assert.IsNotNull(CreateGetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetGetMethod()!));
    }

    [Test]
    public static void CreatePropertySetter_CanWrite()
    {
        Assert.IsNull(CreateSetter(PublicValueTypePublicGetPropertyPropertyInfo, PublicValueTypePublicGetPropertyPropertyInfo.GetSetMethod()!));
        Assert.IsNotNull(CreateSetter(PublicValueTypePublicGetSetPropertyPropertyInfo, PublicValueTypePublicGetSetPropertyPropertyInfo.GetSetMethod()!));
    }
}