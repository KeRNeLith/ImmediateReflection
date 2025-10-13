using System;

namespace ImmediateReflection.Benchmark;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class TestClassAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class SecondTestClassAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ThirdTestClassAttribute : Attribute
{
}