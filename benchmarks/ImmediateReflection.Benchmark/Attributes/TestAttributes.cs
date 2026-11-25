using System;

namespace ImmediateReflection.Benchmark;

[AttributeUsage(AttributeTargets.Property)]
public sealed class TestClassAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class SecondTestClassAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class ThirdTestClassAttribute : Attribute
{
}