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
        public static bool IsParams([NotNull] ParameterInfo param)
        {
            return param.IsDefined(typeof(ParamArrayAttribute), false);
        }
    }
}