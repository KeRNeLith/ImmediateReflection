using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Utils
{
    /// <summary>
    /// Helpers to use C# reflection.
    /// </summary>
    internal static class ReflectionHelpers
    {
        /// <summary>
        /// Checks if the given parameter is a parameter using "params".
        /// </summary>
        /// <param name="param">A <see cref="ParameterInfo"/>.</param>
        /// <returns>True if the parameter correspond to a "params" parameter, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("param:null => halt")]
        public static bool IsParams([NotNull] ParameterInfo param)
        {
            return param.IsDefined(typeof(ParamArrayAttribute), false);
        }

        /// <summary>
        /// Checks if the given <paramref name="property"/> is an indexed one.
        /// </summary>
        /// <param name="property">A <see cref="T:System.Reflection.PropertyInfo"/>.</param>
        /// <returns>True if the <paramref name="property"/> is an indexed property, false otherwise.</returns>
        [Pure]
        [ContractAnnotation("property:null => halt")]
        public static bool IsIndexed([NotNull] PropertyInfo property)
        {
            return property.GetIndexParameters().Length != 0;
        }
    }
}