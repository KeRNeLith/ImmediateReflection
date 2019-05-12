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
        #region Field Get/Set

        [Pure]
        [NotNull]
        public static GetterDelegate CreateGetter([NotNull] FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            DynamicMethod dynamicGetter = CreateDynamicGetter(fieldInfo, out Type targetType);

            ILGenerator generator = dynamicGetter.GetILGenerator();

            if (fieldInfo.IsStatic)
                RegisterStaticTargetArgument(generator, fieldInfo);
            else
                RegisterTargetArgument(generator, targetType);

            // Load field value to the stack
            generator.Emit(OpCodes.Ldfld, fieldInfo);

            // Box the result if needed
            BoxIfNeeded(generator, fieldInfo.FieldType);

            MethodReturn(generator);

            return (GetterDelegate)dynamicGetter.CreateDelegate(typeof(GetterDelegate));
        }

        [Pure]
        [NotNull]
        public static SetterDelegate CreateSetter([NotNull] FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));

            DynamicMethod dynamicSetter = CreateDynamicSetter(fieldInfo, out Type targetType);

            ILGenerator generator = dynamicSetter.GetILGenerator();

            if (fieldInfo.IsStatic)
                RegisterStaticTargetArgument(generator, fieldInfo);
            else
                RegisterTargetArgument(generator, targetType);

            // Load second argument to the stack
            generator.Emit(OpCodes.Ldarg_1);

            // Unbox the set value if needed
            UnboxIfNeeded(generator, fieldInfo.FieldType);

            // Set field
            generator.Emit(OpCodes.Stfld, fieldInfo);

            MethodReturn(generator);

            return (SetterDelegate)dynamicSetter.CreateDelegate(typeof(SetterDelegate));
        }

        #endregion

        #region Property Get/Set

        [Pure]
        [CanBeNull]
        public static GetterDelegate CreateGetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo getMethod)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (!propertyInfo.CanRead)
                return null;

            if (getMethod is null)
                throw new ArgumentNullException(nameof(getMethod));

            DynamicMethod dynamicGetter = CreateDynamicGetter(propertyInfo, out Type targetType);

            ILGenerator generator = dynamicGetter.GetILGenerator();

            if (!getMethod.IsStatic)
                RegisterTargetArgument(generator, targetType);

            CallMethod(generator, getMethod);

            // Box the result if needed
            BoxIfNeeded(generator, propertyInfo.PropertyType);

            MethodReturn(generator);

            return (GetterDelegate)dynamicGetter.CreateDelegate(typeof(GetterDelegate));
        }

        [Pure]
        [CanBeNull]
        public static SetterDelegate CreateSetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo setMethod)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (!propertyInfo.CanWrite)
                return null;

            if (setMethod is null)
                throw new ArgumentNullException(nameof(setMethod));

            DynamicMethod dynamicSetter = CreateDynamicSetter(propertyInfo, out Type targetType);

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

        #endregion

        #region Dynamic method helpers

        [Pure]
        [NotNull]
        private static DynamicMethod CreateDynamicMethod([NotNull] string name, [CanBeNull] Type returnType, [CanBeNull] Type[] parameterTypes, [NotNull] Type owner)
        {
            return owner.IsInterface
                ? new DynamicMethod(name, returnType, parameterTypes, owner.Assembly.ManifestModule, true)
                : new DynamicMethod(name, returnType, parameterTypes, owner, true);
        }

        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicProcedure([NotNull] string name, [CanBeNull] Type[] parameterTypes, [NotNull] Type owner)
        {
            return CreateDynamicMethod(name, typeof(void), parameterTypes, owner);
        }

        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicGetter([NotNull] string name, [NotNull] Type owner)
        {
            return CreateDynamicMethod($"Get{name}", typeof(object), new[] { typeof(object) }, owner);
        }

        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicSetter([NotNull] string name, [NotNull] Type owner)
        {
            return CreateDynamicProcedure($"Set{name}", new[] { typeof(object), typeof(object) }, owner);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <paramref name="member"/> owner.
        /// </summary>
        /// <exception cref="InvalidOperationException">If it's impossible to retrieve the owner <see cref="Type"/>.</exception>
        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static Type GetOwnerType([NotNull] MemberInfo member)
        {
            return member.DeclaringType
                   ?? member.ReflectedType
                   ?? throw new InvalidOperationException($"Cannot retrieve owner type of member {member.Name}.");
        }

        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicGetter([NotNull] MemberInfo member, [NotNull] out Type targetType)
        {
            targetType = GetOwnerType(member);
            return CreateDynamicGetter(member.Name, targetType);
        }

        [Pure]
        [NotNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicSetter([NotNull] MemberInfo member, [NotNull] out Type targetType)
        {
            targetType = GetOwnerType(member);
            return CreateDynamicSetter(member.Name, targetType);
        }

        #endregion

        #region ILGenerator Helpers

        private static void RegisterStaticTargetArgument([NotNull] ILGenerator generator, [NotNull] FieldInfo field)
        {
            // Load static field argument
            generator.Emit(OpCodes.Ldsfld, field);
        }

        private static void NullCheckTarget([NotNull] ILGenerator generator)
        {
            Label notNull = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldnull);
            generator.Emit(OpCodes.Ceq);
            generator.Emit(OpCodes.Brfalse, notNull);
            generator.ThrowException(typeof(TargetException));
            generator.MarkLabel(notNull);
        }

        private static void RegisterTargetArgument([NotNull] ILGenerator generator, [NotNull] Type targetType)
        {
            // If the target object is null throw TargetException
            NullCheckTarget(generator);

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
            // Already the right type
            if (valueType == typeof(object))
                return;

            // If the type is a value type (int/DateTime/..) box it, otherwise cast it
            generator.Emit(
                valueType.IsValueType
                    ? OpCodes.Box
                    : OpCodes.Castclass,
                valueType);
        }

        private static void UnboxIfNeeded([NotNull] ILGenerator generator, [NotNull] Type valueType)
        {
            // Already the right type
            if (valueType == typeof(object))
                return;

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