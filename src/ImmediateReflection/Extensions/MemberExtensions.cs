using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="MemberInfo"/>.
    /// </summary>
    [PublicAPI]
    public static class MemberExtensions
    {
        #region Getter

        #region Strongly typed

        [Pure]
        [ContractAnnotation("property:null => halt;=> true, getter:notnull;=> false, getter:null")]
        private static bool TryCreateGetterInternal<TOwner, TProperty>([NotNull] PropertyInfo property, out GetterDelegate<TOwner, TProperty> getter)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            getter = null;
            MethodInfo getMethod = property.GetGetMethod(true);
            if (getMethod is null)
                return false;

            if (getMethod.IsStatic)
            {
                var staticGetter = (StaticGetterDelegate<TProperty>)
                    Delegate.CreateDelegate(
                        typeof(StaticGetterDelegate<TProperty>),
                        null,
                        getMethod);
                getter = owner => staticGetter();
            }
            else
            {
                // For value type use a ref delegate
                if (typeof(TOwner).IsValueType)
                {
                    var refGetter = (RefGetterDelegate<TOwner, TProperty>)
                        Delegate.CreateDelegate(
                            typeof(RefGetterDelegate<TOwner, TProperty>),
                            null,
                            getMethod);
                    getter = owner => refGetter(ref owner);
                }
                else
                {
                    getter = (GetterDelegate<TOwner, TProperty>)
                        Delegate.CreateDelegate(
                            typeof(GetterDelegate<TOwner, TProperty>),
                            null,
                            getMethod);
                }
            }

            return true;
        }

        /// <summary>
        /// Tries to create a delegate getter for this <see cref="PropertyInfo"/>, if the property has no get method
        /// available then it returns false and a null <paramref name="getter"/>.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <typeparam name="TProperty">Property type.</typeparam>
        /// <param name="property">Property for which creating a getter.</param>
        /// <param name="getter">Created getter delegate.</param>
        /// <returns>True if the getter was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("property:null => halt;=> true, getter:notnull;=> false, getter:null")]
        public static bool TryCreateGetter<TOwner, TProperty>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property,
#else
            [NotNull] PropertyInfo property,
#endif
            out GetterDelegate<TOwner, TProperty> getter)
        {
            getter = null;

            try
            {
                return TryCreateGetterInternal(property, out getter);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException)
                    throw;

                return false;
            }
        }

        /// <summary>
        /// Creates a delegate getter for this <see cref="PropertyInfo"/>, if the property has no get method
        /// available then it creates a default delegate that returns the default value of <typeparamref name="TProperty"/>.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <typeparam name="TProperty">Property type.</typeparam>
        /// <param name="property">Property for which creating a getter.</param>
        /// <returns>The corresponding <see cref="GetterDelegate{TOwner,TProperty}"/> delegate getter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner and property types.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("property:null => halt")]
        public static GetterDelegate<TOwner, TProperty> CreateGetter<TOwner, TProperty>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property
#else
            [NotNull] PropertyInfo property
#endif
            )
        {
            if (TryCreateGetterInternal(property, out GetterDelegate<TOwner, TProperty> getter))
                return getter;
            return owner => default(TProperty);
        }

        #endregion

        #region Partially strongly typed

        [Pure]
        [NotNull]
        [ContractAnnotation("method:null => halt")]
        private static GetterDelegate<TOwner> GetterHelper<TOwner, TValue>([NotNull] MethodInfo method)
        {
            if (method.IsStatic)
            {
                // Convert the MethodInfo into a strongly typed open delegate
                var staticGetter = (StaticGetterDelegate<TValue>)
                    Delegate.CreateDelegate(typeof(StaticGetterDelegate<TValue>), method);

                // Create a more weakly typed delegate which will call the strongly typed one
                return target => staticGetter();
            }

            // For value type use a ref delegate
            if (typeof(TOwner).IsValueType)
            {
                var refGetter = (RefGetterDelegate<TOwner, TValue>)
                    Delegate.CreateDelegate(typeof(RefGetterDelegate<TOwner, TValue>), method);

                return target => refGetter(ref target);
            }

            var getter = (GetterDelegate<TOwner, TValue>)
                Delegate.CreateDelegate(typeof(GetterDelegate<TOwner, TValue>), method);

            return target => getter(target);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("method:null => halt")]
        private static GetterDelegate<TOwner> CreateGetter<TOwner>([NotNull] MethodInfo method)
        {
            // Fetch the generic helper
            MethodInfo genericHelper = typeof(MemberExtensions)
                .GetMethod(
                    nameof(GetterHelper),
                    BindingFlags.Static | BindingFlags.NonPublic);

            if (genericHelper is null)
                throw new InvalidOperationException("Cannot find the generic setter helper.");

            // Supply type arguments
            MethodInfo delegateConstructor = genericHelper.MakeGenericMethod(
                typeof(TOwner),
                method.ReturnType);

            // Call the helper to generate the delegate
            return (GetterDelegate<TOwner>)delegateConstructor.Invoke(null, new object[] { method });
        }

        [Pure]
        [ContractAnnotation("property:null => halt;=> true, getter:notnull;=> false, getter:null")]
        private static bool TryCreateGetterInternal<TOwner>([NotNull] PropertyInfo property, out GetterDelegate<TOwner> getter)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            // ReSharper disable once PossibleNullReferenceException, Justification: PropertyInfo always have a declaring type.
            if (!property.DeclaringType.IsAssignableFrom(typeof(TOwner)))
                throw new ArgumentException("Template is not the owner type of this property.");

            getter = null;
            MethodInfo getMethod = property.GetGetMethod(true);
            if (getMethod is null)
                return false;

            getter = CreateGetter<TOwner>(getMethod);
            return true;
        }

        /// <summary>
        /// Tries to create a delegate getter for this <see cref="PropertyInfo"/>, if the property has no get method
        /// available then it returns false and a null <paramref name="getter"/>.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <param name="property">Property for which creating a setter.</param>
        /// <param name="getter">Created getter delegate.</param>
        /// <returns>True if the getter was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("property:null => halt;=> true, getter:notnull;=> false, getter:null")]
        public static bool TryCreateGetter<TOwner>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property,
#else
            [NotNull] PropertyInfo property,
#endif
            out GetterDelegate<TOwner> getter)
        {
            getter = null;

            try
            {
                return TryCreateGetterInternal(property, out getter);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException)
                    throw;

                return false;
            }
        }

        /// <summary>
        /// Creates a delegate setter for this <see cref="PropertyInfo"/>, if the property has no get method
        /// available then it creates a default delegate that returns the default property type value.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <param name="property">Property for which creating a getter.</param>
        /// <returns>The corresponding <see cref="GetterDelegate{TOwner}"/> delegate getter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner type.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("property:null => halt")]
        public static GetterDelegate<TOwner> CreateGetter<TOwner>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property
#else
            [NotNull] PropertyInfo property
#endif
            )
        {
            if (TryCreateGetterInternal(property, out GetterDelegate<TOwner> getter))
                return getter;

            if (property.PropertyType.IsValueType)
                return owner => Activator.CreateInstance(property.PropertyType);
            return owner => null;
        }

        #endregion

        #endregion

        #region Setter

        #region Strongly typed

        [Pure]
        [ContractAnnotation("property:null => halt;=> true, setter:notnull;=> false, setter:null")]
        private static bool TryCreateSetterInternal<TOwner, TProperty>([NotNull] PropertyInfo property, out SetterDelegate<TOwner, TProperty> setter)
            where TOwner : class
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            setter = null;
            MethodInfo setMethod = property.GetSetMethod(true);
            if (setMethod is null)
                return false;

            if (setMethod.IsStatic)
            {
                var staticSetter = (StaticSetterDelegate<TProperty>)
                    Delegate.CreateDelegate(
                        typeof(StaticSetterDelegate<TProperty>),
                        null,
                        setMethod);
                setter = (owner, value) => staticSetter(value);
            }
            else
            {
                setter = (SetterDelegate<TOwner, TProperty>)
                    Delegate.CreateDelegate(
                        typeof(SetterDelegate<TOwner, TProperty>),
                        null,
                        setMethod);
            }

            return true;
        }

        /// <summary>
        /// Tries to create a delegate setter for this <see cref="PropertyInfo"/>, if the property has no set method
        /// available then it returns false and a null <paramref name="setter"/>.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <typeparam name="TProperty">Property type.</typeparam>
        /// <param name="property">Property for which creating a setter.</param>
        /// <param name="setter">Created setter delegate.</param>
        /// <returns>True if the setter was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("property:null => halt;=> true, setter:notnull;=> false, setter:null")]
        public static bool TryCreateSetter<TOwner, TProperty>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property,
#else
            [NotNull] PropertyInfo property,
#endif
            out SetterDelegate<TOwner, TProperty> setter)
            where TOwner : class
        {
            setter = null;

            try
            {
                return TryCreateSetterInternal(property, out setter);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException)
                    throw;

                return false;
            }
        }

        /// <summary>
        /// Creates a delegate setter for this <see cref="PropertyInfo"/>, if the property has no set method
        /// available then it creates a default delegate that does nothing.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <typeparam name="TProperty">Property type.</typeparam>
        /// <param name="property">Property for which creating a setter.</param>
        /// <returns>The corresponding <see cref="SetterDelegate{TOwner,TProperty}"/> delegate setter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner and property types.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("property:null => halt")]
        public static SetterDelegate<TOwner, TProperty> CreateSetter<TOwner, TProperty>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property
#else
            [NotNull] PropertyInfo property
#endif
            )
            where TOwner : class
        {
            if (TryCreateSetterInternal(property, out SetterDelegate<TOwner, TProperty> setter))
                return setter;
            return (owner, value) => { };
        }

        #endregion

        #region Partially strongly typed

        [Pure]
        [NotNull]
        [ContractAnnotation("method:null => halt")]
        private static SetterDelegate<TOwner> SetterHelper<TOwner, TValue>([NotNull] MethodInfo method)
            where TOwner : class
        {
            if (method.IsStatic)
            {
                // Convert the MethodInfo into a strongly typed open delegate
                var staticSetter = (StaticSetterDelegate<TValue>)
                    Delegate.CreateDelegate(typeof(StaticSetterDelegate<TValue>), method);

                // Create a more weakly typed delegate which will call the strongly typed one
                return (target, param) => staticSetter((TValue)param);
            }

            var setter = (SetterDelegate<TOwner, TValue>)
                Delegate.CreateDelegate(typeof(SetterDelegate<TOwner, TValue>), method);

            return (target, param) => setter(target, (TValue)param);
        }

        [Pure]
        [NotNull]
        [ContractAnnotation("method:null => halt")]
        private static SetterDelegate<TOwner> CreateSetter<TOwner>([NotNull] MethodInfo method)
            where TOwner : class
        {
            // Fetch the generic helper
            MethodInfo genericHelper = typeof(MemberExtensions)
                .GetMethod(
                    nameof(SetterHelper),
                    BindingFlags.Static | BindingFlags.NonPublic);

            if (genericHelper is null)
                throw new InvalidOperationException("Cannot find the generic setter helper.");

            // Supply type arguments
            MethodInfo delegateConstructor = genericHelper.MakeGenericMethod(
                typeof(TOwner),
                method.GetParameters()[0].ParameterType);

            // Call the helper to generate the delegate
            return (SetterDelegate<TOwner>)delegateConstructor.Invoke(null, new object[] { method });
        }

        [Pure]
        [ContractAnnotation("property:null => halt;=> true, setter:notnull;=> false, setter:null")]
        private static bool TryCreateSetterInternal<TOwner>([NotNull] PropertyInfo property, out SetterDelegate<TOwner> setter)
            where TOwner : class
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            // ReSharper disable once PossibleNullReferenceException, Justification: PropertyInfo always have a declaring type.
            if (!property.DeclaringType.IsAssignableFrom(typeof(TOwner)))
                throw new ArgumentException("Template is not the owner type of this property.");

            setter = null;
            MethodInfo setMethod = property.GetSetMethod(true);
            if (setMethod is null)
                return false;

            setter = CreateSetter<TOwner>(setMethod);
            return true;
        }

        /// <summary>
        /// Tries to create a delegate setter for this <see cref="PropertyInfo"/>, if the property has no set method
        /// available then it returns false and a null <paramref name="setter"/>.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <param name="property">Property for which creating a setter.</param>
        /// <param name="setter">Created setter delegate.</param>
        /// <returns>True if the setter was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        [PublicAPI]
        [Pure]
        [ContractAnnotation("property:null => halt;=> true, setter:notnull;=> false, setter:null")]
        public static bool TryCreateSetter<TOwner>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property,
#else
            [NotNull] PropertyInfo property,
#endif
            out SetterDelegate<TOwner> setter)
            where TOwner : class
        {
            setter = null;

            try
            {
                return TryCreateSetterInternal(property, out setter);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException)
                    throw;

                return false;
            }
        }

        /// <summary>
        /// Creates a delegate setter for this <see cref="PropertyInfo"/>, if the property has no set method
        /// available then it creates a default delegate that does nothing.
        /// </summary>
        /// <typeparam name="TOwner">Property owner type.</typeparam>
        /// <param name="property">Property for which creating a setter.</param>
        /// <returns>The corresponding <see cref="SetterDelegate{TOwner}"/> delegate setter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner type.</exception>
        [PublicAPI]
        [Pure]
        [NotNull]
        [ContractAnnotation("property:null => halt")]
        public static SetterDelegate<TOwner> CreateSetter<TOwner>(
#if SUPPORTS_EXTENSIONS
            [NotNull] this PropertyInfo property
#else
            [NotNull] PropertyInfo property
#endif
            )
            where TOwner : class
        {
            if (TryCreateSetterInternal(property, out SetterDelegate<TOwner> setter))
                return setter;
            return (owner, value) => { };
        }

        #endregion

        #endregion
    }
}