using System;
using System.Reflection;
using System.Reflection.Emit;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Factory for delegates created dynamically.
    /// </summary>
    internal static class DelegatesFactory
    {
        [CanBeNull]
        public static GetterDelegate CreateGetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo getMethod)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (getMethod is null)
                throw new ArgumentNullException(nameof(getMethod));

            if (!propertyInfo.CanRead)
                return null;

            Type targetType = propertyInfo.DeclaringType
                              ?? propertyInfo.ReflectedType
                              ?? throw new InvalidOperationException($"Cannot retrieve owner type of property {propertyInfo.Name}.");

            DynamicMethod dynamicGetter = CreateDynamicGetter(propertyInfo.Name, targetType);

            ILGenerator generator = dynamicGetter.GetILGenerator();

            if (!getMethod.IsStatic)
                RegisterTargetArgument(generator, targetType);

            CallMethod(generator, getMethod);

            // Box the result if needed
            BoxIfNeeded(generator, propertyInfo.PropertyType);

            MethodReturn(generator);

            return (GetterDelegate)dynamicGetter.CreateDelegate(typeof(GetterDelegate));
        }

        [CanBeNull]
        public static SetterDelegate CreateSetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo setMethod)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (setMethod is null)
                throw new ArgumentNullException(nameof(setMethod));

            if (!propertyInfo.CanWrite)
                return null;

            Type targetType = propertyInfo.DeclaringType
                              ?? propertyInfo.ReflectedType
                              ?? throw new InvalidOperationException($"Cannot retrieve owner type of property {propertyInfo.Name}.");

            DynamicMethod dynamicSetter = CreateDynamicSetter(propertyInfo.Name, targetType);

            ILGenerator generator = dynamicSetter.GetILGenerator();

            if (!setMethod.IsStatic)
                RegisterTargetArgument(generator, targetType);

            // Load second argument to the stack
            generator.Emit(OpCodes.Ldarg_1);

            // Unbox the set value if needed
            UnboxIfNeeded(generator, propertyInfo.PropertyType);

            CallMethod(generator, setMethod);

            MethodReturn(generator);

            return (SetterDelegate)dynamicSetter.CreateDelegate(typeof(SetterDelegate));
        }

        #region Dynamic method helpers

        [NotNull]
        private static DynamicMethod CreateDynamicMethod([NotNull] string name, [CanBeNull] Type returnType, [CanBeNull] Type[] parameterTypes, [NotNull] Type owner)
        {
            return owner.IsInterface
                ? new DynamicMethod(name, returnType, parameterTypes, owner.Assembly.ManifestModule, true)
                : new DynamicMethod(name, returnType, parameterTypes, owner, true);
        }

        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicProcedure([NotNull] string name, [CanBeNull] Type[] parameterTypes, [NotNull] Type owner)
        {
            return CreateDynamicMethod(name, typeof(void), parameterTypes, owner);
        }

        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicGetter([NotNull] string name, [NotNull] Type owner)
        {
            return CreateDynamicMethod($"Get{name}", typeof(object), new[] { typeof(object) }, owner);
        }

        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicSetter([NotNull] string name, [NotNull] Type owner)
        {
            return CreateDynamicProcedure($"Set{name}", new[] { typeof(object), typeof(object) }, owner);
        }

        #endregion

        #region ILGenerator Helpers

        private static void RegisterTargetArgument([NotNull] ILGenerator generator, [NotNull] Type targetType)
        {
            // Load first argument to the stack
            generator.Emit(OpCodes.Ldarg_0);

            // Cast the object on the stack to the appropriate type
            generator.Emit(
                targetType.IsValueType
                    ? OpCodes.Unbox
                    : OpCodes.Castclass,
                targetType);
        }

        private static void CallMethod([NotNull] ILGenerator generator, [NotNull] MethodInfo method)
        {
            // Call the method passing the object on the stack (only virtual if needed)
            if (method.IsFinal || !method.IsVirtual)
                generator.Emit(OpCodes.Call, method);
            else
                generator.Emit(OpCodes.Callvirt, method);
        }

        private static void BoxIfNeeded([NotNull] ILGenerator generator, [NotNull] Type valueType)
        {
            // If the type is a value type (int/DateTime/..) box it, otherwise cast it
            generator.Emit(
                valueType.IsValueType 
                    ? OpCodes.Box 
                    : OpCodes.Castclass, 
                valueType);
        }

        private static void UnboxIfNeeded([NotNull] ILGenerator generator, [NotNull] Type valueType)
        {
            // If the type is a value type (int/DateTime/..) unbox it, otherwise cast it
            generator.Emit(
                valueType.IsValueType 
                    ? OpCodes.Unbox_Any 
                    : OpCodes.Castclass, 
                valueType);
        }

        private static void MethodReturn([NotNull] ILGenerator generator)
        {
            // Return
            generator.Emit(OpCodes.Ret);
        }

        #endregion
    }
}