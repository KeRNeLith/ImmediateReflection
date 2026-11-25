using static ImmediateReflection.GeneralHelpers;

namespace ImmediateReflection;

/// <summary>
/// Data stored in a constructor cache.
/// </summary>
/// <typeparam name="TConstructorDelegate">Constructor delegate type.</typeparam>
internal sealed class ConstructorData<TConstructorDelegate>
{
    /// <summary>
    /// Indicates if there is a constructor.
    /// </summary>
    public bool HasConstructor { get; }

    /// <summary>
    /// Constructor delegate.
    /// </summary>
    public TConstructorDelegate Constructor { get; }

    public ConstructorData(TConstructorDelegate constructor, bool hasConstructor)
    {
        AssertNotNull(constructor);

        HasConstructor = hasConstructor;
        Constructor = constructor;
    }
}