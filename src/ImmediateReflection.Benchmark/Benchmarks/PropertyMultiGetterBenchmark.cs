using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JetBrains.Annotations;
using Sigil;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Property multi getter benchmark class.
    /// </summary>
    public class PropertyMultiGetterBenchmark : BenchmarkBase
    {
        #region Getter Delegates

        [NotNull]
        private static readonly Func<BenchmarkObject, string> GetterDelegate = (Func<BenchmarkObject, string>)
            Delegate.CreateDelegate(typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod());

        [NotNull]
        private static readonly Func<BenchmarkObject2, int> GetterDelegate2 = (Func<BenchmarkObject2, int>)
            Delegate.CreateDelegate(typeof(Func<BenchmarkObject2, int>), null, PropertyInfo2.GetGetMethod());

        [NotNull]
        private static readonly Func<BenchmarkObject3, double> GetterDelegate3 = (Func<BenchmarkObject3, double>)
            Delegate.CreateDelegate(typeof(Func<BenchmarkObject3, double>), null, PropertyInfo3.GetGetMethod());

        [NotNull]
        private static readonly Func<BenchmarkObject4, short> GetterDelegate4 = (Func<BenchmarkObject4, short>)
            Delegate.CreateDelegate(typeof(Func<BenchmarkObject4, short>), null, PropertyInfo4.GetGetMethod());

        #endregion

        #region Dynamic Delegates

        [NotNull]
        private static readonly Delegate DynamicGetterDelegate = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject, string>), null, PropertyInfo.GetGetMethod());

        [NotNull]
        private static readonly Delegate DynamicGetterDelegate2 = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject2, int>), null, PropertyInfo2.GetGetMethod());

        [NotNull]
        private static readonly Delegate DynamicGetterDelegate3 = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject3, double>), null, PropertyInfo3.GetGetMethod());

        [NotNull]
        private static readonly Delegate DynamicGetterDelegate4 = Delegate.CreateDelegate(
            typeof(Func<BenchmarkObject4, short>), null, PropertyInfo4.GetGetMethod());

        #endregion

        #region Sigil Delegates

        [NotNull]
        private static readonly Func<BenchmarkObject, string> SigilEmitGetter = Emit<Func<BenchmarkObject, string>>
            .NewDynamicMethod("GetProperty")
            .LoadArgument(0)
            .Call(PropertyInfo.GetGetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Func<BenchmarkObject2, int> SigilEmitGetter2 = Emit<Func<BenchmarkObject2, int>>
            .NewDynamicMethod("GetProperty2")
            .LoadArgument(0)
            .Call(PropertyInfo2.GetGetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Func<BenchmarkObject3, double> SigilEmitGetter3 = Emit<Func<BenchmarkObject3, double>>
            .NewDynamicMethod("GetProperty3")
            .LoadArgument(0)
            .Call(PropertyInfo3.GetGetMethod())
            .Return()
            .CreateDelegate();

        [NotNull]
        private static readonly Func<BenchmarkObject4, short> SigilEmitGetter4 = Emit<Func<BenchmarkObject4, short>>
            .NewDynamicMethod("GetProperty4")
            .LoadArgument(0)
            .Call(PropertyInfo4.GetGetMethod())
            .Return()
            .CreateDelegate();

        #endregion

        #region Expression Delegates

        [NotNull]
        private static readonly Func<BenchmarkObject, object> ExpressionGetter = ExpressionHelpers.CreateGetter<BenchmarkObject>(PropertyInfo);

        [NotNull]
        private static readonly Func<BenchmarkObject2, object> ExpressionGetter2 = ExpressionHelpers.CreateGetter<BenchmarkObject2>(PropertyInfo2);

        [NotNull]
        private static readonly Func<BenchmarkObject3, object> ExpressionGetter3 = ExpressionHelpers.CreateGetter<BenchmarkObject3>(PropertyInfo3);

        [NotNull]
        private static readonly Func<BenchmarkObject4, object> ExpressionGetter4 = ExpressionHelpers.CreateGetter<BenchmarkObject4>(PropertyInfo4);

        #endregion

        // Benchmark methods
        [Benchmark(Baseline = true)]
        public string GetDirect_Property()
        {
            return $"{BenchmarkObject.BenchmarkProperty}{BenchmarkObject2.BenchmarkProperty}{BenchmarkObject3.BenchmarkProperty}{BenchmarkObject4.BenchmarkProperty}";
        }

        [Benchmark]
        public string GetDelegate_Property()
        {
            return $"{GetterDelegate(BenchmarkObject)}{GetterDelegate2(BenchmarkObject2)}{GetterDelegate3(BenchmarkObject3)}{GetterDelegate4(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetDynamicDelegate_Property()
        {
            return $"{(string)DynamicGetterDelegate.DynamicInvoke(BenchmarkObject)}{(int)DynamicGetterDelegate2.DynamicInvoke(BenchmarkObject2)}{(double)DynamicGetterDelegate3.DynamicInvoke(BenchmarkObject3)}{(short)DynamicGetterDelegate4.DynamicInvoke(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetPropertyInfo_Property()
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
            return $"{(string)benchmarkProperty.GetValue(BenchmarkObject)}{(int)benchmarkProperty2.GetValue(BenchmarkObject2)}{(double)benchmarkProperty3.GetValue(BenchmarkObject3)}{(short)benchmarkProperty4.GetValue(BenchmarkObject4)}";
            // ReSharper restore PossibleNullReferenceException
        }

        [Benchmark]
        public string GetPropertyInfoCache_Property()
        {
            return $"{(string)PropertyInfo.GetValue(BenchmarkObject)}{(int)PropertyInfo2.GetValue(BenchmarkObject2)}{(double)PropertyInfo3.GetValue(BenchmarkObject3)}{(short)PropertyInfo4.GetValue(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetSigilEmit_Property()
        {
            return $"{SigilEmitGetter(BenchmarkObject)}{SigilEmitGetter2(BenchmarkObject2)}{SigilEmitGetter3(BenchmarkObject3)}{SigilEmitGetter4(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetExpression_Property()
        {
            return $"{(string)ExpressionGetter(BenchmarkObject)}{(int)ExpressionGetter2(BenchmarkObject2)}{(double)ExpressionGetter3(BenchmarkObject3)}{(short)ExpressionGetter4(BenchmarkObject4)}";
        }

        [Benchmark]
        public string GetFastMember_Property()
        {
            return $"{(string)TypeAccessor[BenchmarkObject, BenchmarkObjectPropertyName]}{(int)TypeAccessor2[BenchmarkObject2, BenchmarkObjectPropertyName2]}{(double)TypeAccessor3[BenchmarkObject3, BenchmarkObjectPropertyName3]}{(short)TypeAccessor4[BenchmarkObject4, BenchmarkObjectPropertyName4]}";
        }

        [Benchmark]
        public string GetImmediateProperty_Property()
        {
            return $"{(string)ImmediateProperty.GetValue(BenchmarkObject)}{(int)ImmediateProperty2.GetValue(BenchmarkObject2)}{(double)ImmediateProperty3.GetValue(BenchmarkObject3)}{(short)ImmediateProperty4.GetValue(BenchmarkObject4)}";
        }
    }
}