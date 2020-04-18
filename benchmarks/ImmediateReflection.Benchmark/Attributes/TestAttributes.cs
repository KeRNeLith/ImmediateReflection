using System;

namespace ImmediateReflection.Benchmark
{
    [AttributeUsage(AttributeTargets.All)]
    public class TestClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SecondTestClassAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ThirdTestClassAttribute : Attribute
    {
    }
}