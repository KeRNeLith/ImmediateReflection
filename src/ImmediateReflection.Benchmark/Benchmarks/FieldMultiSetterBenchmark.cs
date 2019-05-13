using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Field multi setter benchmark class.
    /// </summary>
    public class FieldMultiSetterBenchmark : BenchmarkBase
    {
        private const int ValueToSet = 12;

        private const float ValueToSet2 = 14.1f;

        [NotNull]
        private const string ValueToSet3 = "Updated Benchmark Field";

        private const uint ValueToSet4 = 6;

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void SetDirect_Field()
        {
            BenchmarkObject._benchmarkField = ValueToSet;
            BenchmarkObject2._benchmarkField = ValueToSet2;
            BenchmarkObject3._benchmarkField = ValueToSet3;
            BenchmarkObject4._benchmarkField = ValueToSet4;
        }

        [Benchmark]
        public void SetFieldInfo_Field()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            FieldInfo benchmarkField = benchmarkType.GetField(BenchmarkObjectFieldName);

            Type benchmarkType2 = BenchmarkObject2.GetType();
            FieldInfo benchmarkField2 = benchmarkType2.GetField(BenchmarkObjectFieldName2);

            Type benchmarkType3 = BenchmarkObject3.GetType();
            FieldInfo benchmarkField3 = benchmarkType3.GetField(BenchmarkObjectFieldName3);

            Type benchmarkType4 = BenchmarkObject4.GetType();
            FieldInfo benchmarkField4 = benchmarkType4.GetField(BenchmarkObjectFieldName4);

            // ReSharper disable PossibleNullReferenceException
            benchmarkField.SetValue(BenchmarkObject, ValueToSet);
            benchmarkField2.SetValue(BenchmarkObject2, ValueToSet2);
            benchmarkField3.SetValue(BenchmarkObject3, ValueToSet3);
            benchmarkField4.SetValue(BenchmarkObject4, ValueToSet4);
            // ReSharper restore PossibleNullReferenceException
        }

        [Benchmark]
        public void SetFieldInfoCache_Field()
        {
            FieldInfo.SetValue(BenchmarkObject, ValueToSet);
            FieldInfo2.SetValue(BenchmarkObject2, ValueToSet2);
            FieldInfo3.SetValue(BenchmarkObject3, ValueToSet3);
            FieldInfo4.SetValue(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetFastMember_Field()
        {
            TypeAccessor[BenchmarkObject, BenchmarkObjectFieldName] = ValueToSet;
            TypeAccessor2[BenchmarkObject2, BenchmarkObjectFieldName2] = ValueToSet2;
            TypeAccessor3[BenchmarkObject3, BenchmarkObjectFieldName3] = ValueToSet3;
            TypeAccessor4[BenchmarkObject4, BenchmarkObjectFieldName4] = ValueToSet4;
        }

        [Benchmark]
        public void SetImmediateField_Field()
        {
            ImmediateField.SetValue(BenchmarkObject, ValueToSet);
            ImmediateField2.SetValue(BenchmarkObject2, ValueToSet2);
            ImmediateField3.SetValue(BenchmarkObject3, ValueToSet3);
            ImmediateField4.SetValue(BenchmarkObject4, ValueToSet4);
        }
    }
}