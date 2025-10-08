using System;
using System.Reflection;

namespace ImmediateReflection.Benchmark;

/// <summary>
/// Benchmark base class.
/// </summary>
public class BenchmarkBase
{
    #region Types

    protected static readonly Type BenchmarkObjectType = typeof(BenchmarkObject);
    protected static readonly Type BenchmarkObjectType2 = typeof(BenchmarkObject2);
    protected static readonly Type BenchmarkObjectType3 = typeof(BenchmarkObject3);
    protected static readonly Type BenchmarkObjectType4 = typeof(BenchmarkObject4);
    protected static readonly Type CopyableBenchmarkObjectType = typeof(CopyableBenchmarkObject);
    protected static readonly Type CopyableBenchmarkObjectType2 = typeof(CopyableBenchmarkObject2);
    protected static readonly Type CopyableBenchmarkObjectType3 = typeof(CopyableBenchmarkObject3);
    protected static readonly Type CopyableBenchmarkObjectType4 = typeof(CopyableBenchmarkObject4);

    #endregion

    #region Fields

    protected const string BenchmarkObjectFieldName = nameof(BenchmarkObject._benchmarkField);
    protected const string BenchmarkObjectFieldName2 = nameof(BenchmarkObject2._benchmarkField);
    protected const string BenchmarkObjectFieldName3 = nameof(BenchmarkObject3._benchmarkField);
    protected const string BenchmarkObjectFieldName4 = nameof(BenchmarkObject4._benchmarkField);

    protected static readonly FieldInfo FieldInfo = BenchmarkObjectType.GetField(BenchmarkObjectFieldName) ?? throw new InvalidOperationException("Field does not exist.");
    protected static readonly FieldInfo FieldInfo2 = BenchmarkObjectType2.GetField(BenchmarkObjectFieldName2) ?? throw new InvalidOperationException("Field does not exist.");
    protected static readonly FieldInfo FieldInfo3 = BenchmarkObjectType3.GetField(BenchmarkObjectFieldName3) ?? throw new InvalidOperationException("Field does not exist.");
    protected static readonly FieldInfo FieldInfo4 = BenchmarkObjectType4.GetField(BenchmarkObjectFieldName4) ?? throw new InvalidOperationException("Field does not exist.");

    #endregion

    #region Properties

    protected const string BenchmarkObjectPropertyName = nameof(BenchmarkObject.BenchmarkProperty);
    protected const string BenchmarkObjectPropertyName2 = nameof(BenchmarkObject2.BenchmarkProperty);
    protected const string BenchmarkObjectPropertyName3 = nameof(BenchmarkObject3.BenchmarkProperty);
    protected const string BenchmarkObjectPropertyName4 = nameof(BenchmarkObject4.BenchmarkProperty);

    protected static readonly PropertyInfo PropertyInfo = BenchmarkObjectType.GetProperty(BenchmarkObjectPropertyName) ?? throw new InvalidOperationException("Property does not exist.");
    protected static readonly PropertyInfo PropertyInfo2 = BenchmarkObjectType2.GetProperty(BenchmarkObjectPropertyName2) ?? throw new InvalidOperationException("Property does not exist.");
    protected static readonly PropertyInfo PropertyInfo3 = BenchmarkObjectType3.GetProperty(BenchmarkObjectPropertyName3) ?? throw new InvalidOperationException("Property does not exist.");
    protected static readonly PropertyInfo PropertyInfo4 = BenchmarkObjectType4.GetProperty(BenchmarkObjectPropertyName4) ?? throw new InvalidOperationException("Property does not exist.");

    #endregion

    #region Objects instances

    internal static readonly BenchmarkObject BenchmarkObject = new();
    internal static readonly BenchmarkObject2 BenchmarkObject2 = new();
    internal static readonly BenchmarkObject3 BenchmarkObject3 = new();
    internal static readonly BenchmarkObject4 BenchmarkObject4 = new();

    #endregion

    #region FastMember

    protected static readonly FastMember.TypeAccessor TypeAccessor = FastMember.TypeAccessor.Create(BenchmarkObjectType);
    protected static readonly FastMember.TypeAccessor TypeAccessor2 = FastMember.TypeAccessor.Create(BenchmarkObjectType2);
    protected static readonly FastMember.TypeAccessor TypeAccessor3 = FastMember.TypeAccessor.Create(BenchmarkObjectType3);
    protected static readonly FastMember.TypeAccessor TypeAccessor4 = FastMember.TypeAccessor.Create(BenchmarkObjectType4);

    #endregion

    #region Immediate Reflection

    protected static readonly ImmediateType ImmediateType = new(BenchmarkObjectType);
    protected static readonly ImmediateType ImmediateType2 = new(BenchmarkObjectType2);
    protected static readonly ImmediateType ImmediateType3 = new(BenchmarkObjectType3);
    protected static readonly ImmediateType ImmediateType4 = new(BenchmarkObjectType4);
    protected static readonly ImmediateType ImmediateTypeCopyable = new(CopyableBenchmarkObjectType);
    protected static readonly ImmediateType ImmediateTypeCopyable2 = new(CopyableBenchmarkObjectType2);
    protected static readonly ImmediateType ImmediateTypeCopyable3 = new(CopyableBenchmarkObjectType3);
    protected static readonly ImmediateType ImmediateTypeCopyable4 = new(CopyableBenchmarkObjectType4);

    protected static readonly ImmediateField ImmediateField = new(FieldInfo);
    protected static readonly ImmediateField ImmediateField2 = new(FieldInfo2);
    protected static readonly ImmediateField ImmediateField3 = new(FieldInfo3);
    protected static readonly ImmediateField ImmediateField4 = new(FieldInfo4);

    protected static readonly ImmediateProperty ImmediateProperty = new(PropertyInfo);
    protected static readonly ImmediateProperty ImmediateProperty2 = new(PropertyInfo2);
    protected static readonly ImmediateProperty ImmediateProperty3 = new(PropertyInfo3);
    protected static readonly ImmediateProperty ImmediateProperty4 = new(PropertyInfo4);

    #endregion
}