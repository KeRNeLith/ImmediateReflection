using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using Sigil;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Property multi setter benchmark class.
    /// </summary>
    public class PropertyMultiSetterBenchmark : BenchmarkBase
    {
        #region Setter Delegates

        [NotNull]
        private static readonly Action<BenchmarkObject, string> SetterDelegate = (Action<BenchmarkObject, string>)
            Delegate.CreateDelegate(typeof(Action<BenchmarkObject, string>), null, PropertyInfo.GetSetMethod());

        [NotNull]
        private static readonly Action<BenchmarkObject2, int> SetterDelegate2 = (Action<BenchmarkObject2, int>)
            Delegate.CreateDelegate(typeof(Action<BenchmarkObject2, int>), null, PropertyInfo2.GetSetMethod());

        [NotNull]
        private static readonly Action<BenchmarkObject3, double> SetterDelegate3 = (Action<BenchmarkObject3, double>)
            Delegate.CreateDelegate(typeof(Action<BenchmarkObject3, double>), null, PropertyInfo3.GetSetMethod());

        [NotNull]
        private static readonly Action<BenchmarkObject4, short> SetterDelegate4 = (Action<BenchmarkObject4, short>)
            Delegate.CreateDelegate(typeof(Action<BenchmarkObject4, short>), null, PropertyInfo4.GetSetMethod());

        #endregion

        #region Dynamic Delegates

        [NotNull]
        private static readonly Delegate DynamicSetterDelegate = Delegate.CreateDelegate(
            typeof(Action<BenchmarkObject, string>), null, PropertyInfo.GetSetMethod());

        [NotNull]
        private static readonly Delegate DynamicSetterDelegate2 = Delegate.CreateDelegate(
            typeof(Action<BenchmarkObject2, int>), null, PropertyInfo2.GetSetMethod());

        [NotNull]
        private static readonly Delegate DynamicSetterDelegate3 = Delegate.CreateDelegate(
            typeof(Action<BenchmarkObject3, double>), null, PropertyInfo3.GetSetMethod());

        [NotNull]
        private static readonly Delegate DynamicSetterDelegate4 = Delegate.CreateDelegate(
            typeof(Action<BenchmarkObject4, short>), null, PropertyInfo4.GetSetMethod());

        #endregion

        #region Sigil Delegates

        [NotNull]
        private static readonly Action<BenchmarkObject, string> SigilEmitSetter = Emit<Action<BenchmarkObject, string>>
            .NewDynamicMethod("SetProperty")
            .LoadArgument(0)
            .LoadArgument(1)
            .Call(PropertyInfo.GetSetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Action<BenchmarkObject2, int> SigilEmitSetter2 = Emit<Action<BenchmarkObject2, int>>
            .NewDynamicMethod("SetProperty2")
            .LoadArgument(0)
            .LoadArgument(1)
            .Call(PropertyInfo2.GetSetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Action<BenchmarkObject3, double> SigilEmitSetter3 = Emit<Action<BenchmarkObject3, double>>
            .NewDynamicMethod("SetProperty3")
            .LoadArgument(0)
            .LoadArgument(1)
            .Call(PropertyInfo3.GetSetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Action<BenchmarkObject4, short> SigilEmitSetter4 = Emit<Action<BenchmarkObject4, short>>
            .NewDynamicMethod("SetProperty4")
            .LoadArgument(0)
            .LoadArgument(1)
            .Call(PropertyInfo4.GetSetMethod())
            .Return()
            .CreateDelegate();

        #endregion

        #region Expression Delegates

        [NotNull]
        private static readonly Action<BenchmarkObject, object> ExpressionSetter = ExpressionHelpers.CreateSetter<BenchmarkObject>(PropertyInfo);

        [NotNull]
        private static readonly Action<BenchmarkObject2, object> ExpressionSetter2 = ExpressionHelpers.CreateSetter<BenchmarkObject2>(PropertyInfo2);

        [NotNull]
        private static readonly Action<BenchmarkObject3, object> ExpressionSetter3 = ExpressionHelpers.CreateSetter<BenchmarkObject3>(PropertyInfo3);

        [NotNull]
        private static readonly Action<BenchmarkObject4, object> ExpressionSetter4 = ExpressionHelpers.CreateSetter<BenchmarkObject4>(PropertyInfo4);

        #endregion

        #region Values to set

        [NotNull]
        private const string ValueToSet = "Updated benchmark string";

        private const int ValueToSet2 = 51;

        private const double ValueToSet3 = 72.5;

        private const short ValueToSet4 = 1;

        #endregion

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public void SetDirect_Property()
        {
            BenchmarkObject.BenchmarkProperty = ValueToSet;
            BenchmarkObject2.BenchmarkProperty = ValueToSet2;
            BenchmarkObject3.BenchmarkProperty = ValueToSet3;
            BenchmarkObject4.BenchmarkProperty = ValueToSet4;
        }

        [Benchmark]
        public void SetDelegate_Property()
        {
            SetterDelegate(BenchmarkObject, ValueToSet);
            SetterDelegate2(BenchmarkObject2, ValueToSet2);
            SetterDelegate3(BenchmarkObject3, ValueToSet3);
            SetterDelegate4(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetDynamicDelegate_Property()
        {
            DynamicSetterDelegate.DynamicInvoke(BenchmarkObject, ValueToSet);
            DynamicSetterDelegate2.DynamicInvoke(BenchmarkObject2, ValueToSet2);
            DynamicSetterDelegate3.DynamicInvoke(BenchmarkObject3, ValueToSet3);
            DynamicSetterDelegate4.DynamicInvoke(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetPropertyInfo_Property()
        {
            Type benchmarkType = BenchmarkObject.GetType();
            PropertyInfo benchmarkProperty = benchmarkType.GetProperty(BenchmarkObjectPropertyName);

            Type benchmarkType2 = BenchmarkObject2.GetType();
            PropertyInfo benchmarkProperty2 = benchmarkType2.GetProperty(BenchmarkObjectPropertyName2);

            Type benchmarkType3 = BenchmarkObject3.GetType();
            PropertyInfo benchmarkProperty3 = benchmarkType3.GetProperty(BenchmarkObjectPropertyName3);

            Type benchmarkType4 = BenchmarkObject4.GetType();
            PropertyInfo benchmarkProperty4 = benchmarkType4.GetProperty(BenchmarkObjectPropertyName4);

            // ReSharper disable PossibleNullReferenceException
            benchmarkProperty.SetValue(BenchmarkObject, ValueToSet);
            benchmarkProperty2.SetValue(BenchmarkObject2, ValueToSet2);
            benchmarkProperty3.SetValue(BenchmarkObject3, ValueToSet3);
            benchmarkProperty4.SetValue(BenchmarkObject4, ValueToSet4);
            // ReSharper restore PossibleNullReferenceException
        }

        [Benchmark]
        public void SetPropertyInfoCache_Property()
        {
            PropertyInfo.SetValue(BenchmarkObject, ValueToSet);
            PropertyInfo2.SetValue(BenchmarkObject2, ValueToSet2);
            PropertyInfo3.SetValue(BenchmarkObject3, ValueToSet3);
            PropertyInfo4.SetValue(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetSigilEmit_Property()
        {
            SigilEmitSetter(BenchmarkObject, ValueToSet);
            SigilEmitSetter2(BenchmarkObject2, ValueToSet2);
            SigilEmitSetter3(BenchmarkObject3, ValueToSet3);
            SigilEmitSetter4(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetExpression_Property()
        {
            ExpressionSetter(BenchmarkObject, ValueToSet);
            ExpressionSetter2(BenchmarkObject2, ValueToSet2);
            ExpressionSetter3(BenchmarkObject3, ValueToSet3);
            ExpressionSetter4(BenchmarkObject4, ValueToSet4);
        }

        [Benchmark]
        public void SetFastMember_Property()
        {
            TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName] = ValueToSet;
            TypeAccessor2[BenchmarkObject2, BenchmarkObjectPropertyName2] = ValueToSet2;
            TypeAccessor3[BenchmarkObject3, BenchmarkObjectPropertyName3] = ValueToSet3;
            TypeAccessor4[BenchmarkObject4, BenchmarkObjectPropertyName4] = ValueToSet4;
        }

        [Benchmark]
        public void SetImmediateProperty_Property()
        {
            ImmediateProperty.SetValue(BenchmarkObject, ValueToSet);
            ImmediateProperty2.SetValue(BenchmarkObject2, ValueToSet2);
            ImmediateProperty3.SetValue(BenchmarkObject3, ValueToSet3);
            ImmediateProperty4.SetValue(BenchmarkObject4, ValueToSet4);
        }
    }
}