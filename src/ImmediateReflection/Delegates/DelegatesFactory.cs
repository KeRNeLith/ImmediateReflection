using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using static ImmediateReflection.Utils.ReflectionHelpers;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Factory for delegates created dynamically.
    /// </summary>
    internal static class DelegatesFactory
    {
        #region Constructor

        [NotNull]
        private const string RuntimeTypeName = "System.RuntimeType";

        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute, Justification: This type must exists.
        private static readonly Type RuntimeType = Type.GetType(RuntimeTypeName);

        [Pure]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static DefaultConstructorDelegate CreateDefaultConstructor([NotNull] Type type, out bool hasConstructor)
        {
            Debug.Assert(type != null);

            hasConstructor = false;

            if (type == RuntimeType)
                return () => throw new ArgumentException($"Trying to call default constructor on {RuntimeTypeName}.");
            if (type.ContainsGenericParameters)
                return () => throw new ArgumentException($"Class {type.Name} has at least one template parameter not defined.");
            if (type.IsAbstract)
                return () => throw new MissingMethodException($"Abstract class {type.Name} cannot be instantiated.");
            if (type.IsArray)
                // ReSharper disable once PossibleNullReferenceException, Justification: Type is an array so it must have an element type.
                return () => throw new MissingMethodException($"There is no default constructor for array of {type.GetElementType().Name}.");

            DynamicMethod dynamicConstructor = CreateDynamicDefaultConstructor(type.Name);
            dynamicConstructor.InitLocals = true;

            ILGenerator generator = dynamicConstructor.GetILGenerator();

            // Cannot get default constructor for value type, must declare a local
            if (type.IsValueType)
            {
                generator.DeclareLocal(type);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, type);
            }
            // Get the default constructor if available
            else
            {
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor is null)
                {
                    // Last possibility the class has at least one params constructor only
                    if (TryGetParamsConstructorAsDefault(type, out constructor, out Type parameterType, out DefaultConstructorDelegate faultyConstructor))
                    {
                        // Create an empty array to fill the params parameter
                        generator.Emit(OpCodes.Ldc_I4_0);
                        generator.Emit(OpCodes.Newarr, parameterType);
                    }
                    else
                    {
                        return faultyConstructor;
                    }
                }

                generator.Emit(OpCodes.Newobj, constructor);
            }

            MethodReturn(generator);

            hasConstructor = true;
            return (DefaultConstructorDelegate)dynamicConstructor.CreateDelegate(typeof(DefaultConstructorDelegate));
        }

        [Pure]
        [ContractAnnotation("=> true, paramsConstructor:notnull,parameterType:notnull, faultyConstructor:null;" +
                            "=> false, paramsConstructor:null,parameterType:null, faultyConstructor:notnull")]
        private static bool TryGetParamsConstructorAsDefault(
            [NotNull] Type type,
            out ConstructorInfo paramsConstructor,
            out Type parameterType,
            out DefaultConstructorDelegate faultyConstructor)
        {
            Debug.Assert(type != null);

            paramsConstructor = null;
            parameterType = null;
            faultyConstructor = null;

            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length > 0)
            {
                for (int i = 0; i < constructors.Length; ++i)
                {
                    ParameterInfo[] parameters = constructors[i].GetParameters();
                    // Skip constructors with more than one parameter
                    if (parameters.Length > 1)
                        continue;

                    // Params constructor found
                    if (IsParams(parameters[0]))
                    {
                        if (paramsConstructor is null)
                        {
                            paramsConstructor = constructors[i];
                            parameterType = parameters[0].ParameterType.GetElementType();
                            // Continue the search to detect ambiguity with another constructor
                        }
                        else
                        {
                            // At least 2 constructors available => ambiguity
                            faultyConstructor = () => throw new AmbiguousMatchException("There is at least 2 constructors accepting \"params\" parameter only.");
                            return false;
                        }
                    }
                }
            }

            if (paramsConstructor is null)
            {
                faultyConstructor = () => throw new MissingMethodException($"Class {type.Name} does not contain any default constructor.");
                return false;
            }

            return true;
        }

        [NotNull]
        private static readonly ConstructorInfo ArgumentExceptionCtor =
            typeof(ArgumentException).GetConstructor(new[] { typeof(string), typeof(string) })
            ?? throw new InvalidOperationException($"{nameof(ArgumentException)} must have a (String, String) constructor.");

        [NotNull]
        private static readonly MethodInfo GetTypeMethod =
            typeof(object).GetMethod(nameof(GetType))
            ?? throw new InvalidOperationException($"{nameof(GetType)} not found.");

        [NotNull]
        private static readonly MethodInfo TypeEqualsMethod =
            typeof(Type).GetMethod(
                "op_Equality",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new[] { typeof(Type), typeof(Type) },
                null) ?? throw new InvalidOperationException("Cannot find == operator method on Type.");

        [NotNull]
        private static readonly MethodInfo GetTypeFromHandleMethod =
            typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))
            ?? throw new InvalidOperationException($"{nameof(Type.GetTypeFromHandle)} not found.");

        [Pure]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static CopyConstructorDelegate CreateCopyConstructor([NotNull] Type type, out bool hasConstructor)
        {
            Debug.Assert(type != null);

            hasConstructor = false;

            if (type == RuntimeType)
                return other => throw new ArgumentException($"Trying to call copy constructor on {RuntimeTypeName}.");
            if (type.ContainsGenericParameters)
                return other => throw new ArgumentException($"Class {type.Name} has at least one template parameter not defined.");
            if (type.IsArray)
                // ReSharper disable once PossibleNullReferenceException, Justification: Type is an array so it must have an element type.
                return other => throw new MissingMethodException($"There is no copy constructor for array of {type.GetElementType().Name}.");

            // Simply return the value itself for value types
            // String are immutable and does not provide a copy constructor but treat them as if they have one
            // Type are singleton type so the copy is the type itself
            if (type.IsValueType || type == typeof(string) || type == typeof(Type))
            {
                hasConstructor = true;
                return other => other;
            }

            if (type.IsAbstract)
                return other => throw new MissingMethodException($"Abstract class {type.Name} cannot be copied.");

            // Get the copy constructor if available (with exact type matching for the parameter)
            ConstructorInfo constructor = type.GetConstructor(new[]{ type });
            if (constructor is null || constructor.GetParameters()[0].ParameterType != type)
                return other => throw new MissingMethodException($"Class {type.Name} does not contain any copy constructor.");

            DynamicMethod dynamicConstructor = CreateDynamicCopyConstructor(type.Name);
            dynamicConstructor.InitLocals = true;

            ILGenerator generator = dynamicConstructor.GetILGenerator();

            CheckParameterIsOfRightType();

            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Newobj, constructor);

            MethodReturn(generator);

            hasConstructor = true;
            return (CopyConstructorDelegate)dynamicConstructor.CreateDelegate(typeof(CopyConstructorDelegate));

            #region Local function

            void CheckParameterIsOfRightType()
            {
                Label paramIsValid = generator.DefineLabel();

                // other is not null there
                generator.Emit(OpCodes.Ldarg_0);
                CallMethod(generator, GetTypeMethod);
                generator.Emit(OpCodes.Ldtoken, type);
                CallMethod(generator, GetTypeFromHandleMethod);
                CallMethod(generator, TypeEqualsMethod);
                generator.Emit(OpCodes.Brtrue_S, paramIsValid);

                // Throw Argument exception => wrong parameter type
                generator.Emit(OpCodes.Ldstr, "Object to copy has wrong type.");
                generator.Emit(OpCodes.Ldstr, "other");
                generator.Emit(OpCodes.Newobj, ArgumentExceptionCtor);
                generator.Emit(OpCodes.Throw);

                generator.MarkLabel(paramIsValid);
            }

            #endregion
        }

        #endregion

        #region Getter

        [Pure]
        [NotNull]
        [ContractAnnotation("fieldInfo:null => halt")]
        public static GetterDelegate CreateGetter([NotNull] FieldInfo fieldInfo)
        {
            Debug.Assert(fieldInfo != null);

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
        [CanBeNull]
        [ContractAnnotation("propertyInfo:null => halt;getMethod:null => halt")]
        public static GetterDelegate CreateGetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo getMethod)
        {
            Debug.Assert(propertyInfo != null);

            if (!propertyInfo.CanRead)
                return null;

            Debug.Assert(getMethod != null);

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

        #endregion

        #region Setter

        [Pure]
        [NotNull]
        [ContractAnnotation("fieldInfo:null => halt")]
        public static SetterDelegate CreateSetter([NotNull] FieldInfo fieldInfo)
        {
            Debug.Assert(fieldInfo != null);

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

        [Pure]
        [CanBeNull]
        [ContractAnnotation("propertyInfo:null => halt;setMethod:null => halt")]
        public static SetterDelegate CreateSetter([NotNull] PropertyInfo propertyInfo, [NotNull] MethodInfo setMethod)
        {
            Debug.Assert(propertyInfo != null);

            if (!propertyInfo.CanWrite)
                return null;

            Debug.Assert(setMethod != null);

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

        private const string DynamicMethodPrefix = "Immediate";

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
        private static DynamicMethod CreateDynamicMethod([NotNull] string name, [CanBeNull] Type returnType, [CanBeNull] Type[] parameterTypes)
        {
            Debug.Assert(name != null);

            return new DynamicMethod($"{DynamicMethodPrefix}{name}", returnType, parameterTypes, typeof(DelegatesFactory).Module, true);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicProcedure([NotNull] string name, [CanBeNull] Type[] parameterTypes)
        {
            return CreateDynamicMethod(name, typeof(void), parameterTypes);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicDefaultConstructor([NotNull] string name)
        {
            return CreateDynamicMethod($"Constructor_{name}", typeof(object), Type.EmptyTypes);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicCopyConstructor([NotNull] string name)
        {
            return CreateDynamicMethod($"CopyConstructor_{name}", typeof(object), new[] { typeof(object) });
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> of the <paramref name="member"/> owner.
        /// </summary>
        /// <exception cref="InvalidOperationException">If it's impossible to retrieve the owner <see cref="T:System.Type"/>.</exception>
        [Pure]
        [NotNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static Type GetOwnerType([NotNull] MemberInfo member)
        {
            Debug.Assert(member != null);

            return member.DeclaringType
                   ?? member.ReflectedType
                   ?? throw new InvalidOperationException($"Cannot retrieve owner type of member {member.Name}.");
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicGetter([NotNull] string name)
        {
            return CreateDynamicMethod($"Get_{name}", typeof(object), new[] { typeof(object) });
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicGetter([NotNull] MemberInfo member, [NotNull] out Type targetType)
        {
            targetType = GetOwnerType(member);
            return CreateDynamicGetter(member.Name);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("name:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicSetter([NotNull] string name)
        {
            return CreateDynamicProcedure($"Set_{name}", new[] { typeof(object), typeof(object) });
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("member:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static DynamicMethod CreateDynamicSetter([NotNull] MemberInfo member, [NotNull] out Type targetType)
        {
            targetType = GetOwnerType(member);
            return CreateDynamicSetter(member.Name);
        }

        #endregion

        #region ILGenerator Helpers

        [ContractAnnotation("generator:null => halt;field:null => halt")]
        private static void RegisterStaticTargetArgument([NotNull] ILGenerator generator, [NotNull] FieldInfo field)
        {
            // Load static field argument
            generator.Emit(OpCodes.Ldsfld, field);
        }

        [ContractAnnotation("generator:null => halt")]
        private static void NullCheckTarget([NotNull] ILGenerator generator)
        {
            Label notNull = generator.DefineLabel();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldnull);
            generator.Emit(OpCodes.Ceq);
            generator.Emit(OpCodes.Brfalse_S, notNull);
            generator.ThrowException(typeof(TargetException));
            generator.MarkLabel(notNull);
        }

        [ContractAnnotation("generator:null => halt;targetType:null => halt")]
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

        [ContractAnnotation("generator:null => halt;method:null => halt")]
        private static void CallMethod([NotNull] ILGenerator generator, [NotNull] MethodInfo method)
        {
            // Call the method passing the object on the stack (only virtual if needed)
            if (method.IsFinal || !method.IsVirtual)
                generator.Emit(OpCodes.Call, method);
            else
                generator.Emit(OpCodes.Callvirt, method);
        }

        [ContractAnnotation("generator:null => halt;valueType:null => halt")]
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

        [ContractAnnotation("generator:null => halt;valueType:null => halt")]
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

        [ContractAnnotation("generator:null => halt")]
        private static void MethodReturn([NotNull] ILGenerator generator)
        {
            // Return
            generator.Emit(OpCodes.Ret);
        }

        #endregion
    }
}