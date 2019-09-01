#if SUPPORTS_CACHING
using System;
using System.Reflection;
#if SUPPORTS_AGGRESSIVE_INLINING
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;

namespace ImmediateReflection
{
    /// <summary>
    /// Extensions to work with <see cref="Type"/>.
    /// </summary>
    [PublicAPI]
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if this <paramref name="type"/> has a default constructor.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if the <paramref name="type"/> has a default constructor, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt")]
        public static bool HasDefaultConstructor(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type)
#else
            [NotNull] Type type)
#endif
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetDefaultConstructor(type).HasConstructor;
        }

        /// <summary>
        /// Creates an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException"><see cref="Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="AmbiguousMatchException"><see cref="Type"/> has several constructors defining "params" parameter only.</exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static object New(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type)
#else
            [NotNull] Type type)
#endif
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetDefaultConstructor(type).Constructor();
        }

        /// <summary>
        /// Tries to create an instance of this <paramref name="type"/> with that type's default constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public static bool TryNew(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
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
        /// <remarks>It finally uses <see cref="Activator.CreateInstance(Type,object[])"/>.</remarks>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException"><see cref="Type"/> a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true).</exception>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="args"/> is null.</exception>
        /// <exception cref="MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
        /// <exception cref="MissingMethodException">No matching public constructor was found.</exception>
        /// <exception cref="NotSupportedException">
        /// If the <see cref="Type"/> cannot be a TypeBuilder.
        /// -or- Creation of <see cref="TypedReference"/>, ArgIterator, <see cref="Void"/>, and <see cref="RuntimeArgumentHandle"/> types, or arrays of those types, is not supported.
        /// -or- The assembly that contains type is a dynamic assembly that was created with Save.
        /// -or- The constructor that best matches args has varargs arguments.
        /// </exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception.</exception>
        /// <exception cref="TypeLoadException">If the <see cref="Type"/> is not a valid type.</exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt;args:null => halt")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static object New(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
            [NotNull, ItemCanBeNull] params object[] args)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            return TypeAccessor.Get(type).New(args);
        }

        /// <summary>
        /// Tries to create an instance of this <see cref="Type"/> with the best matching constructor.
        /// </summary>
        /// <remarks>
        /// This method will not throw if instantiation failed.
        /// It finally uses <see cref="Activator.CreateInstance(Type,object[])"/>.
        /// </remarks>
        /// <param name="type"><see cref="Type"/> to instantiate.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <param name="args">
        /// An array of arguments that match in number, order, and type the parameters of the constructor to invoke.
        /// If <paramref name="args"/> is an empty array or null, the constructor that takes no parameters (the default constructor) is invoked.
        /// </param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> or <paramref name="args"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt;args:null => halt;=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
#if SUPPORTS_AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool TryNew(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
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
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if the <paramref name="type"/> has a copy constructor, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("type:null => halt")]
        public static bool HasCopyConstructor(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type)
#else
            [NotNull] Type type)
#endif
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetCopyConstructor(type).HasConstructor;
        }

        /// <summary>
        /// Creates a copy instance of <paramref name="other"/> with this <paramref name="type"/>'s copy constructor.
        /// </summary>
        /// <param name="type"><see cref="Type"/> of the object to copy.</param>
        /// <param name="other">Object to copy.</param>
        /// <returns>A reference to the newly created object.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> is a RuntimeType or is an open generic type (that is, the ContainsGenericParameters property returns true),
        /// or if the <paramref name="other"/> instance is not exactly an instance of <paramref name="type"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        /// <exception cref="MissingMethodException">
        /// No matching public copy constructor was found,
        /// or constructor exists but was not considered as copy constructor.
        /// </exception>
        [PublicAPI]
        [NotNull]
        [ContractAnnotation("type:null => halt")]
        public static object Copy(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
            [CanBeNull] object other)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            return CachesHandler.Instance.GetCopyConstructor(type).Constructor(other);
        }

        /// <summary>
        /// Tries to create a copy instance of <paramref name="other"/> with this <paramref name="type"/>'s copy constructor.
        /// </summary>
        /// <remarks>This method will not throw if instantiation failed.</remarks>
        /// <param name="type"><see cref="Type"/> of the object to copy.</param>
        /// <param name="other">Object to copy.</param>
        /// <param name="newInstance">A reference to the newly created object, otherwise null.</param>
        /// <param name="exception">Caught exception if the instantiation failed, otherwise null.</param>
        /// <returns>True if the new instance was successfully created, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="type"/> is null.</exception>
        [PublicAPI]
        [ContractAnnotation("=> true, newInstance:notnull, exception:null;=> false, newInstance:null, exception:notnull")]
        public static bool TryCopy(
#if SUPPORTS_EXTENSIONS
            [NotNull] this Type type,
#else
            [NotNull] Type type,
#endif
            [CanBeNull] object other,
            out object newInstance,
            out Exception exception)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                exception = null;
                newInstance = CachesHandler.Instance.GetCopyConstructor(type).Constructor(other);
                return true;
            }
            catch (Exception ex)
            {
                newInstance = null;
                exception = ex;
                return false;
            }
        }
    }
}
#endif