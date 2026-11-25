namespace ImmediateReflection.Benchmark;

/// <summary>
/// Benchmark object for attributes.
/// </summary>
internal sealed class AttributesBenchmarkObject
{
    [TestClass]
    [SecondTestClass]
    [ThirdTestClass]
    public int TestProperty { get; set; }
}