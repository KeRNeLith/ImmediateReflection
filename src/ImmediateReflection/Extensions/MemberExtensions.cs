#if SUPPORTS_EXTENSIONS
using System;
using System.Reflection;
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberExtensions
    {
        #region Getter

        #region Strongly typed

        [Pure]
        private static bool TryCreateGetterInternal<TOwner, TProperty>([NotNull] this PropertyInfo property, out Func<TOwner, TProperty> getter)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            getter = null;
            MethodInfo getMethod = property.GetGetMethod(true);
            if (getMethod is null)
                return false;

            if (getMethod.IsStatic)
            {
                var staticGetter = (Func<TProperty>)
                    Delegate.CreateDelegate(
                        typeof(Func<TProperty>),
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
                    getter = (Func<TOwner, TProperty>)
                        Delegate.CreateDelegate(
                            typeof(Func<TOwner, TProperty>),
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
        [Pure]
        public static bool TryCreateGetter<TOwner, TProperty>([NotNull] this PropertyInfo property, out Func<TOwner, TProperty> getter)
        {
            getter = null;

            try
            {
                return property.TryCreateGetterInternal(out getter);
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
        /// <returns>The corresponding <see cref="Func{TOwner,TProperty}"/> delegate getter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner and property types.</exception>
        [Pure]
        [NotNull]
        public static Func<TOwner, TProperty> CreateGetter<TOwner, TProperty>([NotNull] this PropertyInfo property)
        {
            if (property.TryCreateGetterInternal(out Func<TOwner, TProperty> getter))
                return getter;
            return owner => default(TProperty);
        }

        #endregion

        #region Partially strongly typed

        [Pure]
        [NotNull]
        private static Func<TOwner, object> GetterHelper<TOwner, TValue>([NotNull] MethodInfo method)
        {
            if (method.IsStatic)
            {
                // Convert the MethodInfo into a strongly typed open delegate
                var staticGetter = (Func<TValue>)
                    Delegate.CreateDelegate(typeof(Func<TValue>), method);

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

            var getter = (Func<TOwner, TValue>)
                Delegate.CreateDelegate(typeof(Func<TOwner, TValue>), method);

            return target => getter(target);
        }

        [Pure]
        [NotNull]
        private static Func<TOwner, object> CreateGetter<TOwner>([NotNull] MethodInfo method)
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
            return (Func<TOwner, object>)delegateConstructor.Invoke(null, new object[] { method });
        }

        [Pure]
        private static bool TryCreateGetterInternal<TOwner>([NotNull] this PropertyInfo property, out Func<TOwner, object> getter)
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
        [Pure]
        public static bool TryCreateGetter<TOwner>([NotNull] this PropertyInfo property, out Func<TOwner, object> getter)
        {
            getter = null;

            try
            {
                return property.TryCreateGetterInternal(out getter);
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
        /// <returns>The corresponding <see cref="Func{TOwner,Object}"/> delegate getter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner type.</exception>
        [Pure]
        [NotNull]
        public static Func<TOwner, object> CreateGetter<TOwner>([NotNull] this PropertyInfo property)
        {
            if (property.TryCreateGetterInternal(out Func<TOwner, object> getter))
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
        private static bool TryCreateSetterInternal<TOwner, TProperty>([NotNull] this PropertyInfo property, out Action<TOwner, TProperty> setter)
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
                var staticSetter = (Action<TProperty>)
                    Delegate.CreateDelegate(
                        typeof(Action<TProperty>),
                        null,
                        setMethod);
                setter = (owner, value) => staticSetter(value);
            }
            else
            {
                setter = (Action<TOwner, TProperty>)
                    Delegate.CreateDelegate(
                        typeof(Action<TOwner, TProperty>),
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
        [Pure]
        public static bool TryCreateSetter<TOwner, TProperty>([NotNull] this PropertyInfo property, out Action<TOwner, TProperty> setter)
            where TOwner : class
        {
            setter = null;

            try
            {
                return property.TryCreateSetterInternal(out setter);
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
        /// <returns>The corresponding <see cref="Action{TOwner,TProperty}"/> delegate setter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner and property types.</exception>
        [Pure]
        [NotNull]
        public static Action<TOwner, TProperty> CreateSetter<TOwner, TProperty>([NotNull] this PropertyInfo property)
            where TOwner : class
        {
            if (property.TryCreateSetterInternal(out Action<TOwner, TProperty> setter))
                return setter;
            return (owner, value) => {};
        }

        #endregion

        #region Partially strongly typed

        [Pure]
        [NotNull]
        private static Action<TOwner, object> SetterHelper<TOwner, TValue>([NotNull] MethodInfo method)
            where TOwner : class
        {
            if (method.IsStatic)
            {
                // Convert the MethodInfo into a strongly typed open delegate
                var staticSetter = (Action<TValue>)
                    Delegate.CreateDelegate(typeof(Action<TValue>), method);

                // Create a more weakly typed delegate which will call the strongly typed one
                return (target, param) => staticSetter((TValue)param);
            }

            var setter = (Action<TOwner, TValue>)
                Delegate.CreateDelegate(typeof(Action<TOwner, TValue>), method);

            return (target, param) => setter(target, (TValue)param);
        }

        [Pure]
        [NotNull]
        private static Action<TOwner, object> CreateSetter<TOwner>([NotNull] MethodInfo method)
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
            return (Action<TOwner, object>)delegateConstructor.Invoke(null, new object[] { method });
        }

        [Pure]
        private static bool TryCreateSetterInternal<TOwner>([NotNull] this PropertyInfo property, out Action<TOwner, object> setter)
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
        [Pure]
        public static bool TryCreateSetter<TOwner>([NotNull] this PropertyInfo property, out Action<TOwner, object> setter)
            where TOwner : class
        {
            setter = null;

            try
            {
                return property.TryCreateSetterInternal(out setter);
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
        /// <returns>The corresponding <see cref="Action{TOwner,Object}"/> delegate setter.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentException">If the given template type does not match owner type.</exception>
        [Pure]
        [NotNull]
        public static Action<TOwner, object> CreateSetter<TOwner>([NotNull] this PropertyInfo property)
            where TOwner : class
        {
            if (property.TryCreateSetterInternal(out Action<TOwner, object> setter))
                return setter;
            return (owner, value) => {};
        }

        #endregion

        #endregion
    }
}
#endif