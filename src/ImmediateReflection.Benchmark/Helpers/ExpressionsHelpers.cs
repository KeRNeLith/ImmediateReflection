using System;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection.Benchmark
{
    /// <summary>
    /// Helpers to create get/set with Expression framework.
    /// </summary>
    internal static class ExpressionHelpers
    {
        [Pure]
        [NotNull]
        public static Func<T> CreateConstructor<T>()
        {
            Expression body = Expression.New(typeof(T));
            return (Func<T>)Expression.Lambda(body).Compile();
        }

        [Pure]
        [NotNull]
        public static Func<T, object> CreateGetter<T>([NotNull] PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (typeof(T) != property.DeclaringType)
                throw new ArgumentException("Trying to create a delegate for the wrong type.");

            ParameterExpression target = Expression.Parameter(property.DeclaringType, "target");
            MemberExpression propertyInfo = Expression.Property(target, property);
            UnaryExpression convert = Expression.TypeAs(propertyInfo, typeof(object));

            return (Func<T, object>)Expression.Lambda(convert, target).Compile();
        }

        [Pure]
        [NotNull]
        public static Action<T, object> CreateSetter<T>([NotNull] PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (typeof(T) != property.DeclaringType)
                throw new ArgumentException("Trying to create a delegate for the wrong type.");

            ParameterExpression target = Expression.Parameter(property.DeclaringType, "target");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            MethodCallExpression setterCall = Expression.Call(target, property.GetSetMethod(), Expression.Convert(value, property.PropertyType));

            return (Action<T, object>)Expression.Lambda(setterCall, target, value).Compile();
        }
    }
}