using System;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="T:System.Type"/>.
    /// </summary>
    [PublicAPI]
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if this <paramref name="type"/> has a default constructor.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> to check.</param>
        /// <returns>True if the <paramref name="type"/> has a default constructor, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt")]
        public static bool HasDefaultConstructor([NotNull] this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetDefaultConstructor(type).HasConstructor;
        }

        /// <summary>
        /// Creates an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> to instantiate.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException"><see cref="T:System.Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="T:System.AmbiguousMatchException"><see cref="T:System.Type"/> has several constructors defining "params" parameter only.</exception>
        /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static object New([NotNull] this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetDefaultConstructor(type).Constructor();
        }

        /// <summary>
        /// Tries to create an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="type"><see cref="T:System.Type"/> to instantiate.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public static bool TryNew(
            [NotNull] this Type type,
            out object newInstance,
            out Exception exception)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                exception = null;
                newInstance = CachesHandler.Instance.GetDefaultConstructor(type).Constructor();
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of this <paramref name="type"/> using the constructor that best matches the specified parameters.
        /// </summary>
        /// <remarks>It finally uses <see cref="M:System.Activator.CreateInstance(Type,object[])"/>.</remarks>
        /// <param name="type"><see cref="T:System.Type"/> to instantiate.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException"><see cref="T:System.Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
        /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// If the <see cref="T:System.Type"/> cannot be a TypeBuilder.
        /// -or- Creation of <see cref="T:System.TypedReference"/>, ArgIterator, <see cref="T:System.Void"/>, and <see cref="T:System.RuntimeArgumentHandle"/> types, or arrays of those types, is not supported.
        /// -or- The assembly that contains type is a dynamic assembly that was created with Save.
        /// -or- The constructor that best matches args has varargs arguments.
        /// </exception>
        /// <exception cref="T:System.TypeLoadException">If the <see cref="T:System.Type"/> is not a valid type.</exception>
        /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt;args:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static object New(
            [NotNull] this Type type,
            [NotNull, ItemCanBeNull] params object[] args)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            return TypeAccessor.Get(type).New(args);
        }

        /// <summary>
        /// Tries to create an instance of this <see cref="T:System.Type"/> with the best matching constructor.
        /// </summary>
        /// <remarks>
        /// This method will not throw if instantiation failed.
        /// It finally uses <see cref="M:System.Activator.CreateInstance(Type,object[])"/>.
        /// </remarks>
        /// <param name="type"><see cref="T:System.Type"/> to instantiate.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> or <paramref name="args"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt;args:null => halt;=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool TryNew(
            [NotNull] this Type type,
            out object newInstance,
            out Exception exception,
            [NotNull, ItemCanBeNull] params object[] args)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            return TypeAccessor.Get(type).TryNew(out newInstance, out exception, args);
        }

        /// <summary>
        /// Checks if this <paramref name="type"/> has a copy constructor.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> to check.</param>
        /// <returns>True if the <paramref name="type"/> has a copy constructor, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt")]
        public static bool HasCopyConstructor([NotNull] this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetCopyConstructor(type).HasConstructor;
        }

        [CanBeNull]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static T CopyInternal<T>([NotNull] Type type, [CanBeNull] T instance)
        {
            if (instance == null)
                return default(T);
            return (T)CachesHandler.Instance.GetCopyConstructor(type).Constructor(instance);
        }

        /// <summary>
        /// Creates a copy instance of <paramref name="other"/> with this <paramref name="type"/>'s copy constructor.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="type"><see cref="T:System.Type"/> of the object to copy.</param>
        /// <param name="other">Object to copy.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="type"/> is a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true),
        /// or if the <paramref name="other"/> instance is not exactly an instance of <paramref name="type"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="T:System.MissingMethodException">
        /// No matching public copy constructor was found,
        /// or constructor exists but was not considered as copy constructor.
        /// </exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt; other:null => null; other:notnull => notnull")]
        public static T Copy<T>([NotNull] this Type type, [CanBeNull] T other)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CopyInternal(type, other);
        }

        /// <summary>
        /// Tries to create a copy instance of <paramref name="other"/> with this <paramref name="type"/>'s copy constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="type"><see cref="T:System.Type"/> of the object to copy.</param>
        /// <param name="other">Object to copy.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt;"
                            + "other:null => true, newInstance:null, exception:null;"
                            + "other:notnull => true, newInstance:notnull, exception:null;"
                            + "other:null => false, newInstance:null, exception:notnull;"
                            + "other:notnull => false, newInstance:null, exception:notnull")]
        public static bool TryCopy<T>(
            [NotNull] this Type type,
            [CanBeNull] T other,
            out T newInstance,
            out Exception exception)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                exception = null;
                newInstance = CopyInternal(type, other);
                return true;
            }
            catch (Exception ex)
            {
                newInstance = default(T);
                exception = ex;
                return false;
            }
        }
    }
}