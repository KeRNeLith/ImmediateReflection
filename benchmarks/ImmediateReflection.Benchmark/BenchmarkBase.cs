using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Benchmark base class.
    /// </summary>
    public class BenchmarkBase
    {
        #region Types

        [NotNull]
        protected static readonly Type BenchmarkObjectType = typeof(BenchmarkObject);

        [NotNull]
        protected static readonly Type BenchmarkObjectType2 = typeof(BenchmarkObject2);

        [NotNull]
        protected static readonly Type BenchmarkObjectType3 = typeof(BenchmarkObject3);

        [NotNull]
        protected static readonly Type BenchmarkObjectType4 = typeof(BenchmarkObject4);

        [NotNull]
        protected static readonly Type CopyableBenchmarkObjectType = typeof(CopyableBenchmarkObject);

        [NotNull]
        protected static readonly Type CopyableBenchmarkObjectType2 = typeof(CopyableBenchmarkObject2);

        [NotNull]
        protected static readonly Type CopyableBenchmarkObjectType3 = typeof(CopyableBenchmarkObject3);

        [NotNull]
        protected static readonly Type CopyableBenchmarkObjectType4 = typeof(CopyableBenchmarkObject4);

        #endregion

        #region Fields

        [NotNull]
        protected const string BenchmarkObjectFieldName = nameof(BenchmarkObject._benchmarkField);

        [NotNull]
        protected const string BenchmarkObjectFieldName2 = nameof(BenchmarkObject2._benchmarkField);

        [NotNull]
        protected const string BenchmarkObjectFieldName3 = nameof(BenchmarkObject3._benchmarkField);

        [NotNull]
        protected const string BenchmarkObjectFieldName4 = nameof(BenchmarkObject4._benchmarkField);

        [NotNull]
        protected static readonly FieldInfo FieldInfo = BenchmarkObjectType.GetField(BenchmarkObjectFieldName) ?? throw new InvalidOperationException("Field does not exist.");

        [NotNull]
        protected static readonly FieldInfo FieldInfo2 = BenchmarkObjectType2.GetField(BenchmarkObjectFieldName2) ?? throw new InvalidOperationException("Field does not exist.");

        [NotNull]
        protected static readonly FieldInfo FieldInfo3 = BenchmarkObjectType3.GetField(BenchmarkObjectFieldName3) ?? throw new InvalidOperationException("Field does not exist.");

        [NotNull]
        protected static readonly FieldInfo FieldInfo4 = BenchmarkObjectType4.GetField(BenchmarkObjectFieldName4) ?? throw new InvalidOperationException("Field does not exist.");

        #endregion

        #region Properties

        [NotNull]
        protected const string BenchmarkObjectPropertyName = nameof(BenchmarkObject.BenchmarkProperty);

        [NotNull]
        protected const string BenchmarkObjectPropertyName2 = nameof(BenchmarkObject2.BenchmarkProperty);

        [NotNull]
        protected const string BenchmarkObjectPropertyName3 = nameof(BenchmarkObject3.BenchmarkProperty);

        [NotNull]
        protected const string BenchmarkObjectPropertyName4 = nameof(BenchmarkObject4.BenchmarkProperty);

        [NotNull]
        protected static readonly PropertyInfo PropertyInfo = BenchmarkObjectType.GetProperty(BenchmarkObjectPropertyName) ?? throw new InvalidOperationException("Property does not exist.");

        [NotNull]
        protected static readonly PropertyInfo PropertyInfo2 = BenchmarkObjectType2.GetProperty(BenchmarkObjectPropertyName2) ?? throw new InvalidOperationException("Property does not exist.");

        [NotNull]
        protected static readonly PropertyInfo PropertyInfo3 = BenchmarkObjectType3.GetProperty(BenchmarkObjectPropertyName3) ?? throw new InvalidOperationException("Property does not exist.");

        [NotNull]
        protected static readonly PropertyInfo PropertyInfo4 = BenchmarkObjectType4.GetProperty(BenchmarkObjectPropertyName4) ?? throw new InvalidOperationException("Property does not exist.");

        #endregion

        #region Objects instances

        [NotNull]
        internal static readonly BenchmarkObject BenchmarkObject = new BenchmarkObject();

        [NotNull]
        internal static readonly BenchmarkObject2 BenchmarkObject2 = new BenchmarkObject2();

        [NotNull]
        internal static readonly BenchmarkObject3 BenchmarkObject3 = new BenchmarkObject3();

        [NotNull]
        internal static readonly BenchmarkObject4 BenchmarkObject4 = new BenchmarkObject4();

        #endregion

        #region FastMember

        [NotNull]
        protected static readonly FastMember.TypeAccessor TypeAccessor = FastMember.TypeAccessor.Create(BenchmarkObjectType);

        [NotNull]
        protected static readonly FastMember.TypeAccessor TypeAccessor2 = FastMember.TypeAccessor.Create(BenchmarkObjectType2);

        [NotNull]
        protected static readonly FastMember.TypeAccessor TypeAccessor3 = FastMember.TypeAccessor.Create(BenchmarkObjectType3);

        [NotNull]
        protected static readonly FastMember.TypeAccessor TypeAccessor4 = FastMember.TypeAccessor.Create(BenchmarkObjectType4);

        #endregion

        #region Immediate Reflection

        [NotNull]
        protected static readonly ImmediateType ImmediateType = new ImmediateType(BenchmarkObjectType);

        [NotNull]
        protected static readonly ImmediateType ImmediateType2 = new ImmediateType(BenchmarkObjectType2);

        [NotNull]
        protected static readonly ImmediateType ImmediateType3 = new ImmediateType(BenchmarkObjectType3);

        [NotNull]
        protected static readonly ImmediateType ImmediateType4 = new ImmediateType(BenchmarkObjectType4);

        [NotNull]
        protected static readonly ImmediateType ImmediateTypeCopyable = new ImmediateType(CopyableBenchmarkObjectType);

        [NotNull]
        protected static readonly ImmediateType ImmediateTypeCopyable2 = new ImmediateType(CopyableBenchmarkObjectType2);

        [NotNull]
        protected static readonly ImmediateType ImmediateTypeCopyable3 = new ImmediateType(CopyableBenchmarkObjectType3);

        [NotNull]
        protected static readonly ImmediateType ImmediateTypeCopyable4 = new ImmediateType(CopyableBenchmarkObjectType4);


        [NotNull]
        protected static readonly ImmediateField ImmediateField = new ImmediateField(FieldInfo);

        [NotNull]
        protected static readonly ImmediateField ImmediateField2 = new ImmediateField(FieldInfo2);

        [NotNull]
        protected static readonly ImmediateField ImmediateField3 = new ImmediateField(FieldInfo3);

        [NotNull]
        protected static readonly ImmediateField ImmediateField4 = new ImmediateField(FieldInfo4);



        [NotNull]
        protected static readonly ImmediateProperty ImmediateProperty = new ImmediateProperty(PropertyInfo);

        [NotNull]
        protected static readonly ImmediateProperty ImmediateProperty2 = new ImmediateProperty(PropertyInfo2);

        [NotNull]
        protected static readonly ImmediateProperty ImmediateProperty3 = new ImmediateProperty(PropertyInfo3);

        [NotNull]
        protected static readonly ImmediateProperty ImmediateProperty4 = new ImmediateProperty(PropertyInfo4);

        #endregion
    }
}