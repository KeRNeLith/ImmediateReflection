namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Benchmark object for attributes.
    /// </summary>
    public class AttributesBenchmarkObject
    {
        [TestClass]
        [SecondTestClass]
        [ThirdTestClass]
        public int TestProperty { get; set; }
    }
}